using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaTramiteDoc.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult TestConexion()
        {
            string mensaje;

            try
            {
                // Obtener la cadena de conexión desde el archivo web.config
                string connectionString = ConfigurationManager.ConnectionStrings["DBSTD"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Intentar abrir la conexión
                    mensaje = "Conexión exitosa a la base de datos.";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error al conectar a la base de datos: " + ex.Message;
            }

            return Content(mensaje); // Devolver el resultado como texto
        }
    }
}