using CapaDatos;
using CapaModelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class CD_SubSolcitudJefe
    {
        public static CD_SubSolcitudJefe _instancia = null;

        private CD_SubSolcitudJefe()
        {

        }

        public static CD_SubSolcitudJefe Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CD_SubSolcitudJefe();
                }
                return _instancia;
            }
        }

        public bool RegistrarSubSolicitudJefe(SubSolicitudJefe OSubSolicitudJefe)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarSubSolicitudJefe", oConexion);
                    cmd.Parameters.AddWithValue("NombreAprobador", OSubSolicitudJefe.NombreAprobador);
                    cmd.Parameters.AddWithValue("IdSolicitud", OSubSolicitudJefe.IdSolicitud);
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
        public List<SubSolicitudJefe> ObtenerSubSolicitudesJefes(int IdSolicitud)
        {
            List<SubSolicitudJefe> rptListaUsuario = new List<SubSolicitudJefe>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarSubSolicitudJefe", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("IdSolicitud", IdSolicitud);
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaUsuario.Add(new SubSolicitudJefe()
                        {
                            NombreAprobador = dr["Aprobador"].ToString(),
                            NombreEstado = dr["Estado"].ToString(),
                            ComentarioSubSolicitudJefe = dr["Comentario"].ToString(),
                            IdSubSolicitudJefe= Convert.ToInt32(dr["IdSubSolicitudJefe"]),
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
        public bool EliminarSubSolicitudJefe(int idSubSolicitudJefe)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarSubSolicitudJefe", oConexion);
                    cmd.Parameters.AddWithValue("IdSubSolicitudJefe", idSubSolicitudJefe);
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
        public bool EnviarCorreoSolicitudAprobacion(string destinatario, int? idSolicitud)
        {
            bool respuesta = true;
            Solicitud detalles = ObtenerDetallesSolicitud(idSolicitud);

            string asunto = $"[External] STD – Soliciutd de Aprobacion: {{titulo}}";
            /*string plantillaHtml = File.ReadAllText("C:/Users/mlimo/Desktop/SistemaTramiteDoc (1)/SistemaTramiteDoc (2)/SistemaTramiteDoc/ClassLibrary1/Archivos");*/

            string plantillaHtml = @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Solicitud de Aprobacion</title>
            </head>
            <body style='font-family: Arial, sans-serif; margin: 0; padding: 40px 0; background-color: #f4f4f7; '>
                <table width='600' align='center' cellpadding='0' cellspacing='0' style='background - color: #ffffff; border: 1px solid #e1e1e1; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.08); font-size: 14px; color: #333;'>
                    <tr>
                        <td style='padding: 24px 24px 16px; font - size: 18px; font - weight: bold; border - bottom: 1px solid #f0f0f0;'>
                            STD – Solicitud de Aprobación: EXP - GTH - 00005 - 2025
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 16px 24px; line-height: 1.6;'>
                            < p style = 'margin: 0 0 8px;'>
                                <strong > Solicitado por:</ strong > {{solicitante}} - Gilat Peru<br>
                                <a href= 'mailto:MLimo@gilatla.com' style = 'color: #0066cc; text-decoration: none;' > {{correo}} </a>
                            </p>
                            < p style='margin: 0 0 8px;'>
                                < strong > Fecha de creación:</ strong > {{fechaCreacion}}
                            </ p >
                            <p style = 'margin: 0 0 16px;'>
                                < strong > Documento:</strong>
                                < a href = '#' style = 'color: #0066cc; text-decoration: underline;'> Ver documento </a>
                            </p>
                            < p style ='margin: 0 0 16px;'>
                                {{solicitante}} - Gilat Peru solicita la aprobación del documento correspondiente al expediente<strong>{{titulo}}</ strong >.
                            </p> 
                            <p style = 'margin: 0;'>
                                < strong > Enlace a documentos:</ strong >< br>
                                < a href = 'https://gilatsatnet.sharepoint.com/sites/TramiteDocumentarioLegal/DocumentosTramite/EXP-GTH-00005-2025/Respuesta'
                                    style = 'color: #0066cc; text-decoration: underline; word-break: break-word;' >
                                    https://gilatsatnet.sharepoint.com/sites/TramiteDocumentarioLegal/DocumentosTramite/EXP-GTH-00005-2025/Respuesta
                                </a>
                            </p>
                        </ td >
                </tr>
                <tr>
                    <td style='padding: 16px 24px; background - color: #f9f9f9; font-size: 12px; color: #666; border-top: 1px solid #f0f0f0;'>
                        Sistema de Trámite Documentario<br>
                        Gerencia Legal
                    </td>
                </tr>
                </table>
            </body>
            </html>";

            string cuerpo = plantillaHtml
            .Replace("{{solicitante}}", detalles.AnalistaNombre)
            .Replace("{{Titulo}}", detalles.Titulo)
            .Replace("{{correo}}", detalles.AnalistaCorreo)
            .Replace("{{fechaCreacion}}", detalles.FechaRegistro.ToString("dddd, MMMM d, yyyy h:mm tt"));

            string paraemtro2 = detalles.AnalistaNombre + " - Gilat Peru";
            var fromAddress = new MailAddress(detalles.AnalistaCorreo, paraemtro2);
            var toAddress = new MailAddress(destinatario);
            const string fromPassword = "L@picero12345"; // Utiliza una contraseña de aplicación si tienes habilitada la autenticación de dos factores

            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = asunto,
                Body = cuerpo,
                IsBodyHtml = true // Importante para que el correo se envíe como texto plano
            })
            {
                smtp.Send(message);
            }
            respuesta = GuardarCorreo(destinatario, destinatario, asunto, cuerpo);
            return respuesta;
        }
        private Solicitud ObtenerDetallesSolicitud(int? idSolicitud)
        {
            Solicitud detalles = null;

            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerDetallesSolicitud", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSolicitud", idSolicitud);

                oConexion.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        detalles = new Solicitud
                        {
                            Titulo = reader["Titulo"].ToString(),
                            AnalistaNombre = reader["ANALISTALEGAL"].ToString(),
                            AnalistaCorreo = reader["CORREO"].ToString(),
                            FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"].ToString())
                        };
                    }
                }
            }
            return detalles;
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
        public bool CambiarEstadoSubSolicitudJefe(int idSolicitud,int estado)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_CambiarEstadoSubSolicitudJefe2", oConexion);
                    cmd.Parameters.AddWithValue("idSolicitud", idSolicitud);
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
    }
}
