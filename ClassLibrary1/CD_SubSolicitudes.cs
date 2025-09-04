using CapaDatos;
using CapaModelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class CD_SubSolicitudes
    {
        public static CD_SubSolicitudes _instancia = null;

        private CD_SubSolicitudes()
        {

        }

        public static CD_SubSolicitudes Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_SubSolicitudes();
                }
                return _instancia;
            }
        }

        public bool RegistrarSubSolicitudes(List<SubSolicitud> oSubSolicitudes)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    oConexion.Open();

                    foreach (var oSubSolicitud in oSubSolicitudes)
                    {
                        SqlCommand cmd = new SqlCommand("sp_InsertarSubSolicitud", oConexion);
                        cmd.Parameters.AddWithValue("NombreAreaExterna", oSubSolicitud.NombreAreaExterna);
                        cmd.Parameters.AddWithValue("NombreResponsable", oSubSolicitud.NombreResponsable);
                        cmd.Parameters.AddWithValue("IdSolicitud", oSubSolicitud.IdSolicitud);
                        cmd.Parameters.AddWithValue("Comentario", oSubSolicitud.Comentario);
                        cmd.Parameters.AddWithValue("TituloSolicitud", oSubSolicitud.TituloSolicitud);
                        cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();

                        respuesta = Convert.ToBoolean(cmd.Parameters["OperacionExitosa"].Value);
                    }
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

        public List<SubSolicitud> ObtenerSubSolicitudes( int IdSolicitud)
        {
            List<SubSolicitud> rptListaUsuario = new List<SubSolicitud>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarSubSolicitud", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("IdSolicitud", IdSolicitud);
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaUsuario.Add(new SubSolicitud()
                        {
                            IdSubSolicitud= Convert.ToInt32(dr["IdSubSolicitud"]),
                            NombreAreaExterna = dr["AExterna"].ToString(),
                            Comentario = dr["Comentarios"].ToString(),
                            Observacion = dr["Observacion"].ToString(),
                            ComentarioAreaExterna = dr["ComentarioAExterna"].ToString(),
                            NombreResponsable = dr["Responsable"].ToString(),
                            FechaSolicitud = Convert.ToDateTime(dr["FechaRegistro"].ToString()),
                            FechaRespuesta = string.IsNullOrEmpty(dr["FechaRespuesta"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dr["FechaRespuesta"].ToString()),
                            NombreEstado = dr["Estado"].ToString(),
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
        
        public bool CambiarEstadoSubSolicitudValidad(int? IdSubSolicitud)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_CambiarEstadoSubSolValidado", oConexion);
                    cmd.Parameters.AddWithValue("IdSubSolicitud", IdSubSolicitud);
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
        public bool ModificarEstadoSubSolicitudObservado(int? IdSubSolicitud,string Observacion,int? IdSolicitud)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_CambiarEstadoSubSolObservado", oConexion);
                    cmd.Parameters.AddWithValue("IdSubSolicitud", IdSubSolicitud);
                    cmd.Parameters.AddWithValue("Observacion", Observacion);
                    cmd.Parameters.AddWithValue("IdSolicitud", IdSolicitud);
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


        public bool EliminarSubSolicitud(int IdSubSolicitud)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarSubSolicitud", oConexion);
                    cmd.Parameters.AddWithValue("IdSubSolicitud", IdSubSolicitud);
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
        public void EnviarCorreo(string destinatario,int? idSubSolicitud)
        {
            bool respuesta=true;
            DetallesSubSolicitud detalles = ObtenerDetallesSubSolicitud(idSubSolicitud);

            string asunto = $"STD – Atención de Requerimiento: {detalles.Titulo} / Área: {detalles.NombreAreaExterna} / N° Doc.: {detalles.NumeroDoc}";
            /*string plantillaHtml = File.ReadAllText("C:/Users/mlimo/Desktop/SistemaTramiteDoc (1)/SistemaTramiteDoc (2)/SistemaTramiteDoc/ClassLibrary1/Archivos");*/
            
            string plantillaHtml = @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Notificación de Expediente</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>El requerimiento para el expediente <strong>{{expediente_codigo}}</strong> enviado por <strong>{{remitente}}</strong> - <strong>Gilat Peru</strong> para el área de <strong>{{area_destino}}</strong> ha sido atendido.</p>
            
                <h3>Datos del Expediente</h3>
                <ul>
                    <li><strong>Institución emisora:</strong> {{institucion_emisora}}</li>
                    <li><strong>Empresa destino:</strong> {{empresa_destino}}</li>
                    <li><strong>Nro Documento:</strong> {{numero_documento}}</li>
                    <li><strong>Asunto:</strong> {{asunto}}</li>
                    <li><strong>Proyecto:</strong> {{proyecto}}</li>
                </ul>
            
                <h3>Atención del Requerimiento</h3>
                <ul>
                    <li><strong>Responsable:</strong> {{responsable}}</li>
                    <li><strong>Área:</strong> {{area_responsable}}</li>
                    <li><strong>Comentarios:</strong> {{comentarios}}</li>
                </ul>
            
                <h3>Documentos adjuntos:</h3>
            
                <p><a href='https://apps.powerapps.com/play/e/default-7300b1a3-573a-4010-92a6-1c65cd85e927/a/17824d6b-ab81-4df9-9a86-89e19b1aec0c?tenantId=7300b1a3-573a-4010-92a6-1c65cd85e927' target='_blank'>Ir al sistema</a></p>
            
                <p>
                    <strong>Sistema de Trámite Documentario</strong><br>
                    Gerencia Legal
                </p>
            </body>
            </html>";

            string cuerpo = plantillaHtml
            .Replace("{{expediente_codigo}}", detalles.Titulo)
            .Replace("{{remitente}}", detalles.FueRegistradoPor)
            .Replace("{{area_destino}}", detalles.NombreAreaExterna)
            .Replace("{{institucion_emisora}}", detalles.NombreInstitucion)
            .Replace("{{empresa_destino}}", detalles.NombreEmpresa)
            .Replace("{{numero_documento}}", detalles.NumeroDoc)
            .Replace("{{asunto}}", detalles.Titulo)
            .Replace("{{proyecto}}", detalles.NombreProyecto)
            .Replace("{{responsable}}", detalles.NombreResponsable)
            .Replace("{{area_responsable}}", detalles.NombreAreaExterna)
            .Replace("{{comentarios}}", detalles.Comentario);

            respuesta = GuardarCorreo(destinatario, destinatario, asunto, cuerpo);
        }

        public bool GuardarCorreo(string reciptients, string copy_reciptients, string asunto, string cuerpo)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    oConexion.Open();
                  
                    SqlCommand cmd = new SqlCommand("sp_GuardarCorreo", oConexion);
                    cmd.Parameters.AddWithValue("recipients", reciptients);
                    cmd.Parameters.AddWithValue("copy_recipients", copy_reciptients);
                    cmd.Parameters.AddWithValue("subject", asunto);
                    cmd.Parameters.AddWithValue("body", cuerpo);
                    cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
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

        public void AgregarComentario(int? idSubSolicitud,string comments)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    oConexion.Open();

                    SqlCommand cmd = new SqlCommand("sp_AgregarComentarioSubSolicitud", oConexion);
                    cmd.Parameters.AddWithValue("ComentarioAreaExt", comments);
                    cmd.Parameters.AddWithValue("IdSubSolicitud", idSubSolicitud);
                    cmd.Parameters.Add("OperacionExitosa", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
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
            //return respuesta;
        }

        public List<SubSolicitud> ObtenerSubSolicitudesColaboradores(int IdSolicitud,int? id)
        {
            List<SubSolicitud> rptListaUsuario = new List<SubSolicitud>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarSubSolicitudesColaboradores", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("IdSolicitud", IdSolicitud);
                cmd.Parameters.AddWithValue("IdResponsable", id);
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaUsuario.Add(new SubSolicitud()
                        {
                            NombreAreaExterna = dr["NOMBRE"].ToString(),
                            FechaSolicitud = Convert.ToDateTime(dr["FECHA"].ToString()),
                            Comentario = dr["COMENTARIO"].ToString(),
                            NombreEstado = dr["NOMBREESTADO"].ToString(),
                            IdSubSolicitud= Convert.ToInt32(dr["IdSubSolicitud"]),
                            NombresArchivos = ObtenerNombresArchivos(Convert.ToInt32(dr["IdSubSolicitud"]))
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
        public List<string> ObtenerNombresArchivos(int idSubSolicitud)
        {
            List<string> nombresArchivos = new List<string>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerNombresArchivos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("IdSubSolicitud", idSubSolicitud);
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        nombresArchivos.Add(dr["NombreArchivo"].ToString());
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    nombresArchivos = null;
                }
            }
            return nombresArchivos;
        }
        public bool CambiarEstadoSubSolicitud(int? idSubSolicitud)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_CambiarEstadoSubSolicitud", oConexion);
                    cmd.Parameters.AddWithValue("IdSubSolicitud", idSubSolicitud == null ? (object)DBNull.Value : idSubSolicitud);
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

        private DetallesSubSolicitud ObtenerDetallesSubSolicitud(int? idSubSolicitud)
        {
            DetallesSubSolicitud detalles = null;

            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerDetallesSubSolicitud", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSubSolicitud", idSubSolicitud);

                oConexion.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        detalles = new DetallesSubSolicitud
                        {
                            Titulo = reader["Titulo"].ToString(),
                            NombreAreaExterna = reader["NombreAreaExterna"].ToString(),
                            NumeroDoc = reader["NumeroDoc"].ToString(),
                            NombreInstitucion = reader["NombreInstitucion"].ToString(),
                            NombreEmpresa = reader["NombreEmpresa"].ToString(),
                            NombreProyecto = reader["NombreProyecto"].ToString(),
                            NombreResponsable = reader["NombreResponsable"].ToString(),
                            FueRegistradoPor = reader["NombrePersonaRegistra"].ToString(),
                            Comentario = reader["Comentario"].ToString()
                        };
                    }
                }
            }

            return detalles;
        }
        public bool EnviarAAreasSubSolicitudes(int? idSolicitud)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_CambiarEstadoSubSolicitudPendiente", oConexion);
                    cmd.Parameters.AddWithValue("IdSolicitud", idSolicitud == null ? (object)DBNull.Value : idSolicitud);
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
        public bool AgregarArchivos(SubSolicitud oSolicitud, Dictionary<string, byte[]> archivos = null)
        {
            bool respuesta2 = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    oConexion.Open();
                    foreach (var archivo in archivos)
                    {
                        SqlCommand cmdArchivo = new SqlCommand("sp_InsertarArchivoSubSolicitud", oConexion);
                        cmdArchivo.Parameters.AddWithValue("IdSolicitud", oSolicitud.IdSolicitud);
                        cmdArchivo.Parameters.AddWithValue("IdSubSolicitud", oSolicitud.IdSubSolicitud);
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
    }
}
