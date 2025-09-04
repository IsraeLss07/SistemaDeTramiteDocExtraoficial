using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
namespace CapaDatos
{
    public class Conexion {

        public static string CN = ConfigurationManager.ConnectionStrings["DBSTD"].ConnectionString;
        /*
        protected DatabaseProviderFactory factory;
        protected Database DEA;
        protected DbCommand DbCommand = null;
        public Conexion(){
            factory = new DatabaseProviderFactory();

            DEA = factory.Create("DBSTD");
        }
        */
        /*
        public int sp_setAccoutPasswordReset(int UserId, string Password) { 
            int i = 0;
            try { 

                DbCommand = DEA.GetStoredProcCommand("dbo.sp_setAccoutPasswordReset");

                DEA.AddInParameter(DbCommand, "@UserId", DbType.Int32, UserId);

                DEA.AddInParameter(DbCommand, "@Password", DbType.String, Password);

                i = DEA.ExecuteNonQuery(DbCommand);
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
            return i;

        }
        public int setUsersAttempts(string email, int Count) {
            int dsResult = 0;

            try {
                DbCommand = DEA.GetStoredProcCommand("dbo.sp_setUsersAttempts");
                DEA.AddInParameter(DbCommand, "@Email", DbType.String, email);
                DEA.AddInParameter(DbCommand, "@Count", DbType.Int32, Count);
                dsResult = DEA.ExecuteNonQuery(DbCommand);
            }

            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
            return dsResult;
            
        }
        */
    }
    class Program
    {
        static void Main()
        {
            // Recuperar la cadena de conexión desde el archivo de configuración
            string connectionString = ConfigurationManager.ConnectionStrings["DBSTD"].ConnectionString;

            // Intentar abrir la conexión
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Abrir la conexión
                    connection.Open();
                    Console.WriteLine("Conexión establecida con éxito.");
                }
                catch (Exception ex)
                {
                    // Si hay un error al intentar conectar, lo capturamos
                    Console.WriteLine("Error de conexión: " + ex.Message);
                }
            }
        }
    }

}
