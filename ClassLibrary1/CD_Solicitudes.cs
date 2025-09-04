using CapaModelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CapaDatos
{
    public class CD_Solicitudes
    {
        public static CD_Solicitudes _instancia = null;

        private CD_Solicitudes()
        {

        }

        public static CD_Solicitudes Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_Solicitudes();
                }
                return _instancia;
            }
        }

        public List<Solicitud> ObtenerSolicitudes()
        {
            List<Solicitud> rptListaUsuario = new List<Solicitud>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("ListarSolicitudes", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        int aux;
                        aux = Convert.ToInt32(dr["IdSolicitud"]);
                        rptListaUsuario.Add(new Solicitud()
                        {
                            IdSolicitud = aux,
                            AreaExternaNombre = ObtenerNombresAreasExternas(aux),
                            AnalistaNombre = dr["NombreAnalistaLegal"].ToString(),
                            Titulo = dr["TITULO"].ToString(),
                            Asunto = dr["ASUNTO"].ToString(),
                            FechaEventoExterno = Convert.ToDateTime(dr["FECHAVENEXTERNO"].ToString()),
                            NombreEstado = dr["NOMBREESTADO"].ToString(),
                            NombreFase = dr["NOMBREFASE"].ToString(),
                            NumeroDoc = dr["NUMERODOCUMENTO"].ToString(),

                    });
                    }
                    dr.Close();

                    return rptListaUsuario;

                }
                catch (Exception ex)
                {
                    rptListaUsuario = null;
                    return rptListaUsuario;
                }
            }
        }

        public List<Solicitud> ObtenerSolicitudesColaborador(int IdUsuario)
        {
            List<Solicitud> rptListaUsuario = new List<Solicitud>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarSolicitudesColaborador", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Parameters.AddWithValue("ParametroEntrada", IdUsuario);
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        int aux;
                        aux = Convert.ToInt32(dr["IdSolicitud"]);
                        rptListaUsuario.Add(new Solicitud()
                        {
                            IdSolicitud= aux,
                            AreaExternaNombre = ObtenerNombresAreasExternas(aux),
                            AnalistaNombre = dr["NombreAnalistaLegal"].ToString(),
                            Titulo = dr["TITULO"].ToString(),
                            Asunto = dr["ASUNTO"].ToString(),
                            FechaEventoExterno = Convert.ToDateTime(dr["FECHAVENEXTERNO"].ToString()),
                            NombreEstado = dr["NOMBREESTADO"].ToString(),
                            NombreFase = dr["NOMBREFASE"].ToString(),
                            NumeroDoc = dr["NUMERODOCUMENTO"].ToString(),

                        });;
                    }
                    dr.Close();

                    return rptListaUsuario;

                }
                catch (Exception ex)
                {
                    rptListaUsuario = null;
                    return rptListaUsuario;
                }
            }
        }


        public string ObtenerNombresAreasExternas(int idSolicitud)
        {
            int i = 0;
            string rptListaArea="";
            string cadena2;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarAreasExternasPorSolicitud", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Parameters.AddWithValue("ParametroEntrada", idSolicitud);
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        if (i > 0)
                        {
                            rptListaArea += ',';
                        }
                        cadena2 = dr["NombreAreaExterna"].ToString();
                        rptListaArea += cadena2;
                    }
                    dr.Close();

                    return rptListaArea;

                }
                catch (Exception ex)
                {
                    rptListaArea = null;
                    return rptListaArea;
                }
            }
        }

        public bool RegistrarSolicitud(Solicitud oSolicitud,  Dictionary<string, byte[]> archivos = null)
        {
            int respuesta = 0;
            bool respuesta2 = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarExpediente", oConexion);
                    cmd.Parameters.AddWithValue("IDPERSONAREGISTRA", oSolicitud.IdPersonaRegistra);
                    cmd.Parameters.AddWithValue("NUMERODOCUMENTO", oSolicitud.NumeroDoc);
                    cmd.Parameters.AddWithValue("ASUNTO", oSolicitud.Asunto);
                    cmd.Parameters.AddWithValue("REFERENCIA", oSolicitud.Referencia);
                    cmd.Parameters.AddWithValue("NOMBREEMPRESA", oSolicitud.NombreEmpresa);
                    cmd.Parameters.AddWithValue("NOMBREFORMATO", oSolicitud.NombreFormato);
                    cmd.Parameters.AddWithValue("NOMBREPROYECTO", oSolicitud.NombreProyecto);
                    cmd.Parameters.AddWithValue("PENALIDAD", oSolicitud.Penalidad);
                    cmd.Parameters.AddWithValue("NOMBREINSTITUCION", oSolicitud.NombreInstitucion);
                    cmd.Parameters.AddWithValue("NOMBRETIPODOC", oSolicitud.TipoDoc);
                    cmd.Parameters.AddWithValue("PLAZO", oSolicitud.PlazoAtc);
                    cmd.Parameters.AddWithValue("FECHAVENINTERNO", oSolicitud.FechaVencInterno);
                    cmd.Parameters.AddWithValue("FECHAVENEXTERNO", oSolicitud.FechaEventoExterno);
                    cmd.Parameters.AddWithValue("FECHAEMISION", oSolicitud.FechaEmision);
                    cmd.Parameters.AddWithValue("FECHARECEPCION", oSolicitud.FechaRecepcion);
                    cmd.Parameters.AddWithValue("UBICACIONPISO", oSolicitud.Piso);
                    cmd.Parameters.AddWithValue("UBICACIONESTANTE", oSolicitud.Estante);
                    cmd.Parameters.AddWithValue("UBICACIONFILE", oSolicitud.File);
                    cmd.Parameters.AddWithValue("NOMBREANALISTA", string.IsNullOrEmpty(oSolicitud.AnalistaNombre) ? (object)DBNull.Value : oSolicitud.AnalistaNombre);
                    cmd.Parameters.Add("OperacionExitosa", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["OperacionExitosa"].Value);

                    if (respuesta>0 && archivos != null)
                    {
                        foreach (var archivo in archivos)
                        {
                            SqlCommand cmdArchivo = new SqlCommand("sp_InsertarArchivo", oConexion);
                            cmdArchivo.Parameters.AddWithValue("IdSolicitud", respuesta);
                            cmdArchivo.Parameters.AddWithValue("NombreArchivo", archivo.Key);
                            cmdArchivo.Parameters.AddWithValue("Archivo", archivo.Value);
                            cmdArchivo.Parameters.AddWithValue("Contenttype", archivo.Key.Split('.').Last());
                            cmdArchivo.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                            cmdArchivo.CommandType = CommandType.StoredProcedure;
                            cmdArchivo.ExecuteNonQuery();
                            respuesta2 = Convert.ToBoolean(cmdArchivo.Parameters["OperacionExitosa"].Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception message
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta2 = false;
                }
                if (respuesta > 0 && archivos == null)
                {
                    respuesta2 = true;
                }
                return respuesta2;
            }
        }
        public List<Solicitud> ObtenerSolicitudesPorExpDoc(string parametroEntrada)
        {
            List<Solicitud> rptListaUsuario = new List<Solicitud>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_BuscarSoliciutdPorNroExp", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Parameters.AddWithValue("ParametroEntrada", parametroEntrada);
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaUsuario.Add(new Solicitud()
                        {
                            AreaExternaNombre = dr["NombreAreaExterna"].ToString(),
                            AnalistaNombre = dr["NombreAnalistaLegal"].ToString(),
                            Titulo = dr["TITULO"].ToString(),
                            Asunto = dr["ASUNTO"].ToString(),
                            FechaEventoExterno = Convert.ToDateTime(dr["FECHAVENEXTERNO"].ToString()),
                            NombreEstado = dr["NOMBREESTADO"].ToString(),
                            NombreFase = dr["NOMBREFASE"].ToString(),
                            NumeroDoc = dr["NUMERODOCUMENTO"].ToString(),

                        });
                    }
                    dr.Close();

                    return rptListaUsuario;

                }
                catch (Exception ex)
                {
                    rptListaUsuario = null;
                    return rptListaUsuario;
                }
            }
        }
        public bool ModificarSolicitud(string numeroDoc, int parametro, int? id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("ModificarExpediente", oConexion);
                    cmd.Parameters.AddWithValue("NumeroDoc", numeroDoc);
                    cmd.Parameters.AddWithValue("Parametro", parametro);
                    cmd.Parameters.AddWithValue("IDANALISTA", id);
                    cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["OperacionExitosa"].Value);
                }
                catch (Exception ex)
                {
                    // Log the exception message
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta = false;
                }

                return respuesta;
            }
        }
        public Solicitud ObtenerSolicitudPorNumeroDoc(string numeroDoc)
        {
            Solicitud solicitud = null;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_BuscarSoliciutdPorNroExp", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumeroDoc", numeroDoc);

                try
                {
                    oConexion.Open();
                    cmd.CommandTimeout = 10000; // Aumenta el tiempo de espera de la conexión si es necesario
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        solicitud = new Solicitud()
                        {
                            Comentarios = dr["COMENTARIOS"] != DBNull.Value ? dr["COMENTARIOS"].ToString() : null,
                            Titulo = dr["TITULO"].ToString(),
                            NumeroDoc = dr["NUMERODOCUMENTO"].ToString(),
                            Referencia = dr["REFERENCIA"].ToString(),
                            Asunto = dr["ASUNTO"].ToString(),
                            NombreEmpresa = dr["NOMBREEMPRESA"].ToString(),
                            TipoDoc = dr["NOMBRETD"].ToString(),
                            NombreFormato = dr["NombreFormato"].ToString(),
                            NombreInstitucion = dr["NombreInstitucion"].ToString(),
                            NombreProyecto = dr["NombreProyecto"].ToString(),
                            FechaEmision = Convert.ToDateTime(dr["FECHACREACION"]),
                            FechaRecepcion = Convert.ToDateTime(dr["FECHARECEPCION"]),
                            FechaEventoExterno = Convert.ToDateTime(dr["FECHAVENEXT"]),
                            FechaVencInterno = Convert.ToDateTime(dr["FECHAVENINT"]),
                            PlazoAtc = Convert.ToInt32(dr["PLAZO"]),
                            FueRegistradoPor = dr["NombreAnalistaLegal"].ToString(),
                            AnalistaNombre = dr["NombreAnalistaLegal"].ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FECHACREACION"]),
                            NombreFase = dr["NOMBREFACE"].ToString(),
                            NombreEstado = dr["NOMBREESTADO"].ToString(),
                            Piso = dr["UBICACIONPISO"].ToString(),
                            Estante = dr["UBICACIONESTANTE"].ToString(),
                            File = dr["UBICACIONFILE"].ToString(),
                            Penalidad = Convert.ToInt32(dr["PENALIDAD"]),
                            IdSolicitud = Convert.ToInt32(dr["IDSOLICITUD"]),
                        };
                    }
                    dr.Close(); // Asegurarse de cerrar el SqlDataReader

                    if (solicitud != null)
                    {
                        //solicitud.Archivos = new List<HttpPostedFileBase>();
                        solicitud.NombresArchivos = new List<string>();
                        solicitud.ContentTypes = new List<string>();
                        solicitud.TipoArchivo = new List<int>();

                        SqlCommand cmdArchivos = new SqlCommand("sp_ObtenerArchivosPorSolicitud", oConexion);
                        cmdArchivos.CommandType = CommandType.StoredProcedure;
                        cmdArchivos.Parameters.AddWithValue("@IdSolicitud", solicitud.IdSolicitud);
                        cmdArchivos.CommandTimeout = 600; // Aumenta el tiempo de espera de la conexión si es necesario

                        SqlDataReader drArchivos = cmdArchivos.ExecuteReader();
                        while (drArchivos.Read())
                        {
                            int tipoArchivo = drArchivos["TIPOARCHIVO"] != DBNull.Value ? Convert.ToInt32(drArchivos["TIPOARCHIVO"]) : 0;
                            string nombreArchivo = drArchivos["NOMBRE"].ToString();
                            //byte[] archivoBytes = (byte[])drArchivos["ARCHIVO"];
                            string contentType = drArchivos["CONTENTTYPE"].ToString();

                            solicitud.NombresArchivos.Add(nombreArchivo);
                            solicitud.TipoArchivo.Add(tipoArchivo);
                            //solicitud.Archivos.Add(new CustomPostedFile(archivoBytes, nombreArchivo, contentType));
                            solicitud.ContentTypes.Add(contentType);
                        }
                        drArchivos.Close(); // Asegurarse de cerrar el SqlDataReader
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    throw new Exception("Error al obtener la solicitud", ex);
                }
            }
            return solicitud;
        }
        public bool ModificarSolicitudDetalle(string numeroDoc, DateTime? fechaExterna, DateTime? fechaInterna, int? plazo)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("ModificarExpedienteParaDetalle", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Manejar valores nulos
                    cmd.Parameters.AddWithValue("NumeroDoc", numeroDoc);
                    cmd.Parameters.AddWithValue("FECHAVENINTERNO", fechaInterna ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("FECHAVENEXTERNO", fechaExterna ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("PLAZO", plazo ?? (object)DBNull.Value);

                    cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["OperacionExitosa"].Value);
                }
                catch (Exception ex)
                {
                    // Log the exception message
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta = false;
                }
            }
            return respuesta;
        }
		
		public List<Empresa> GetEmpresas()
        {
            var empresas = new List<Empresa>();

            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarEmpresas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empresas.Add(new Empresa
                            {
                                EmpresaId = Convert.ToInt32(reader["IDEMPRESA"]),
                                EmpresaNombre = reader["NOMBRE"].ToString(),
                            });
                        }
                    }
                }
            }
            return empresas;
        }

        public List<TipoDocumento> GetTipoDeDocumentoS()
        {
            var tipoDocumentoS = new List<TipoDocumento>();

            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarTipoDocumento", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipoDocumentoS.Add(new TipoDocumento
                            {
                                TipoDocumentoId = Convert.ToInt32(reader["IDTIPODOCUMENTO"]),
                                TipoDocumentoNombre = reader["NOMBRE"].ToString(),
                            });
                        }
                    }
                }
            }
            return tipoDocumentoS;
        }

        public List<Formato> GetFormatos()
        {
            var formatos = new List<Formato>();

            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarFormato", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            formatos.Add(new Formato
                            {
                                FormatoId = Convert.ToInt32(reader["IDFORMATO"]),
                                FormatoNombre = reader["NOMBRE"].ToString(),
                            });
                        }
                    }
                }
            }
            return formatos;
        }

        public List<Institucion> GetInstituciones()
        {
            var instituciones = new List<Institucion>();

            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarInstitucion", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            instituciones.Add(new Institucion
                            {
                                InstitucionId = Convert.ToInt32(reader["IDINSTITUCION"]),
                                InstitucionNombre = reader["NOMBRE"].ToString(),
                            });
                        }
                    }
                }
            }
            return instituciones;
        }

        public List<Proyecto> GetProyectos()
        {
            var proyectos = new List<Proyecto>();

            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarProyecto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proyectos.Add(new Proyecto
                            {
                                ProyectoId = Convert.ToInt32(reader["IDPROYECTO"]),
                                ProyectoNombre = reader["NOMBRE"].ToString(),
                            });
                        }
                    }
                }
            }
            return proyectos;
        }
        public List<Usuario> GetAnalistas()
        {
            var analistas= new List<Usuario>();
            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarAnalistas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            analistas.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["IDUSUARIO"]),
                                Nombres = reader["NOMBRES"].ToString(),
                            });
                        }
                    }
                }
            }
            return analistas;
        }
        public List<Usuario> GetAprobadores()
        {
            var aprobadores = new List<Usuario>();
            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarAprobadoresJefes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            aprobadores.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["IDUSUARIO"]),
                                Nombres = reader["NOMBRES"].ToString(),
                            });
                        }
                    }
                }
            }
            return aprobadores;
        }

        public List<AreaExterna> GetAreasExternas()
        {
            var areasExternas = new List<AreaExterna>();
            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarAreasExternas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            areasExternas.Add(new AreaExterna
                            {
                                IdAreaExterna = Convert.ToInt32(reader["IDAREAEXTERNA"]),
                                Nombre = reader["NOMBRE"].ToString(),
                            });
                        }
                    }
                }
            }
            return areasExternas;
        }

        public List<Usuario> GetResponsablesPorAreaExterna(int id)
        {
            var responsales = new List<Usuario>();
            using (var connection = new SqlConnection(Conexion.CN))
            {
                using (var command = new SqlCommand("sp_ListarResponsablePorAreaExterna", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdAreaExterna", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            responsales.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["IDUSUARIO"]),
                                Nombres = reader["NOMBRES"].ToString(),
                            });
                        }
                    }
                }
            }
            return responsales;
        }
        public CustomPostedFile ObtenerArchivoPorNombre(int idSolicitud, string nombreArchivo)
        {
            CustomPostedFile archivo = null;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerArchivoPorNombre", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSolicitud", idSolicitud);
                cmd.Parameters.AddWithValue("@NombreArchivo", nombreArchivo);

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        byte[] archivoBytes = (byte[])dr["ARCHIVO"];
                        string fileName = dr["NOMBRE"].ToString();
                        string contentType = dr["CONTENTTYPE"].ToString();
                        archivo = new CustomPostedFile(archivoBytes, fileName, contentType);
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    throw new Exception("Error al obtener el archivo", ex);
                }
            }
            return archivo;
        }

        public bool Verificar(int idSolicitud, string nombreArchivo,int idSubSolicitud)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Verificar", oConexion);
                    cmd.Parameters.AddWithValue("IdSolicitud", idSolicitud);
                    cmd.Parameters.AddWithValue("NombreArchivo", nombreArchivo);
                    cmd.Parameters.AddWithValue("IdSubSolicitud", idSubSolicitud);
                    cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["OperacionExitosa"].Value);
                }
                catch (Exception ex)
                {
                    // Log the exception message
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta = false;
                }

                return respuesta;
            }
        }
        public bool Verificar2(int idSolicitud, string nombreArchivo)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Verificar2", oConexion);
                    cmd.Parameters.AddWithValue("IdSolicitud", idSolicitud);
                    cmd.Parameters.AddWithValue("NombreArchivo", nombreArchivo);
                    cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["OperacionExitosa"].Value);
                }
                catch (Exception ex)
                {
                    // Log the exception message
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta = false;
                }

                return respuesta;
            }
        }
        public bool AgregarArchivosJefes(Solicitud oSolicitud, Dictionary<string, byte[]> archivos = null)
        {
            bool respuesta2 = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    oConexion.Open();
                    foreach (var archivo in archivos)
                    {
                        SqlCommand cmdArchivo = new SqlCommand("sp_InsertarArchivoSolicitudJefe", oConexion);
                        cmdArchivo.Parameters.AddWithValue("IdSolicitud", oSolicitud.IdSolicitud);
                        cmdArchivo.Parameters.AddWithValue("NombreArchivo", archivo.Key);
                        cmdArchivo.Parameters.AddWithValue("Archivo", archivo.Value);
                        cmdArchivo.Parameters.AddWithValue("Contenttype", archivo.Key.Split('.').Last());
                        cmdArchivo.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmdArchivo.CommandType = CommandType.StoredProcedure;
                        cmdArchivo.ExecuteNonQuery();
                        respuesta2 = Convert.ToBoolean(cmdArchivo.Parameters["OperacionExitosa"].Value);
                    }

                }
                catch (Exception ex)
                {
                    // Log the exception message
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta2 = false;
                }
                return respuesta2;
            }
        }
        public List<Solicitud> ObtenerSolicitudesJefes(int IdUsuario)
        {
            List<Solicitud> rptListaUsuario = new List<Solicitud>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarSolicitudesJefes", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ParametroEntrada", IdUsuario);

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        int aux = Convert.ToInt32(dr["IdSolicitud"]);
                        var solicitud = new Solicitud
                        {
                            IdSolicitud = aux,
                            AreaExternaNombre = ObtenerNombresAreasExternas(aux),
                            AnalistaNombre = dr["NombreAnalistaLegal"].ToString(),
                            Titulo = dr["TITULO"].ToString(),
                            Asunto = dr["ASUNTO"].ToString(),
                            FechaEventoExterno = Convert.ToDateTime(dr["FECHAVENEXTERNO"]),
                            NombreEstado = dr["NOMBREESTADO"].ToString(),
                            NombreFase = dr["NOMBREFASE"].ToString(),
                            NumeroDoc = dr["NUMERODOCUMENTO"].ToString()
                        };

                        rptListaUsuario.Add(solicitud);
                    }

                    dr.Close();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            /*
            // Ahora obtenemos los archivos usando una nueva conexión
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                oConexion.Open();

                foreach (var solicitud in rptListaUsuario)
                {
                    solicitud.Archivos = new List<HttpPostedFileBase>();
                    solicitud.NombresArchivos = new List<string>();
                    solicitud.ContentTypes = new List<string>();
                    solicitud.TipoArchivo = new List<int>();

                    SqlCommand cmdArchivos = new SqlCommand("sp_ObtenerArchivosPorSolicitud", oConexion);
                    cmdArchivos.CommandType = CommandType.StoredProcedure;
                    cmdArchivos.Parameters.AddWithValue("@IdSolicitud", solicitud.IdSolicitud);

                    SqlDataReader drArchivos = cmdArchivos.ExecuteReader();

                    while (drArchivos.Read())
                    {
                        int tipoArchivo = drArchivos["TIPOARCHIVO"] != DBNull.Value ? Convert.ToInt32(drArchivos["TIPOARCHIVO"]) : 0;
                        string nombreArchivo = drArchivos["NOMBRE"].ToString();
                        byte[] archivoBytes = (byte[])drArchivos["ARCHIVO"];
                        string contentType = drArchivos["CONTENTTYPE"].ToString();

                        solicitud.NombresArchivos.Add(nombreArchivo);
                        solicitud.TipoArchivo.Add(tipoArchivo);
                        solicitud.Archivos.Add(new CustomPostedFile(archivoBytes, nombreArchivo, contentType));
                        solicitud.ContentTypes.Add(contentType);
                    }

                    drArchivos.Close();
                }
            }
            */
            return rptListaUsuario;
        }
        public bool CambiarEstadoSubSolicitudJefe(int idUsuario, int idSolicitud, string comentarios, int estado)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_CambiarEstadoSubSolicitudJefe", oConexion);
                    cmd.Parameters.AddWithValue("idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("idSolicitud", idSolicitud);
                    cmd.Parameters.AddWithValue("comentarios", comentarios);
                    cmd.Parameters.AddWithValue("estado", estado);
                    cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["OperacionExitosa"].Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta = false;
                }
                return respuesta;
            }
        }
		public bool CambiarEstadoFaseSolicitud(int idSolicitud, int estado, int fase)
		{
			bool respuesta = true;
			using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
			{
				try
				{
					SqlCommand cmd = new SqlCommand("sp_CambiarEstadoYFaseSolicitud", oConexion);
					cmd.Parameters.AddWithValue("idSolicitud", idSolicitud);
					cmd.Parameters.AddWithValue("fase", fase);
					cmd.Parameters.AddWithValue("estado", estado);
					cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;
					oConexion.Open();
					cmd.ExecuteNonQuery();
					respuesta = Convert.ToBoolean(cmd.Parameters["OperacionExitosa"].Value);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
					respuesta = false;
				}
				return respuesta;
			}
		}
		public bool EliminarArchivo(int idSolicitud, string nombreArchivo)
		{
			bool respuesta = true;
			using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
			{
				try
				{
					SqlCommand cmd = new SqlCommand("sp_EliminarArchivo", oConexion);
					cmd.CommandType = CommandType.StoredProcedure;

					// Manejar valores nulos
					cmd.Parameters.AddWithValue("IdSolicitud", idSolicitud);
					cmd.Parameters.AddWithValue("NombreArchivo", nombreArchivo);
					cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
					oConexion.Open();
					cmd.ExecuteNonQuery();
					respuesta = Convert.ToBoolean(cmd.Parameters["OperacionExitosa"].Value);
				}
				catch (Exception ex)
				{
					// Log the exception message
					Console.WriteLine("Error: " + ex.Message);
					respuesta = false;
				}
			}
			return respuesta;
		}
	}
}
