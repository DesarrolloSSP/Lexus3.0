using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.EntityClient;

namespace Lexus2._0.Clases
{
    public class ErrorL
    {
        public static void Registrar(Exception ex)
        {
            if (ex == null) return;
            try
            {
                string usuario = "Anónimo";
                string url = "";
                string pagina = "";
                string ip = "";
                string userAgent = "";
                string errorType = ex.GetType().Name;
                if (HttpContext.Current != null)
                {
                    var request = HttpContext.Current.Request;
                    usuario = HttpContext.Current.User?.Identity?.Name ?? "Anónimo";
                    url = request.Url?.ToString() ?? "";
                    pagina = request.Url?.AbsolutePath ?? "";
                    ip = request.UserHostAddress ?? "";
                    userAgent = request.UserAgent ?? "";
                }
                var entityConnection = new EntityConnection(
                    ConfigurationManager
                        .ConnectionStrings["BDJuridicoEntities"]   /// conexion a la bd 
                        .ConnectionString);

                string sqlConnectionString =
                    entityConnection.StoreConnection.ConnectionString;

                using (SqlConnection cn = new SqlConnection(sqlConnectionString))
                {
                    cn.Open();
                    string sql = @"
                    INSERT INTO ErrorLog
                    (
                        Fecha,
                        Usuario,
                        Url,
                        Pagina,
                        Mensaje,
                        StackTrace,
                        ErrorType,
                        IpAddress,
                        UserAgent
                    )
                    VALUES
                    (
                        @Fecha,
                        @Usuario,
                        @Url,
                        @Pagina,
                        @Mensaje,
                        @StackTrace,
                        @ErrorType,
                        @IpAddress,
                        @UserAgent
                    )";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Usuario", usuario);
                        cmd.Parameters.AddWithValue("@Url", url);
                        cmd.Parameters.AddWithValue("@Pagina", pagina);
                        cmd.Parameters.AddWithValue("@Mensaje", ex.Message);
                        cmd.Parameters.AddWithValue("@StackTrace", ex.ToString());
                        cmd.Parameters.AddWithValue("@ErrorType", errorType);
                        cmd.Parameters.AddWithValue("@IpAddress", ip);
                        cmd.Parameters.AddWithValue("@UserAgent", userAgent);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exLogger)
            {

               try
                  {
                      if (HttpContext.Current != null)
                      {
                          string rutaFichero = HttpContext.Current.Server.MapPath("~/Fallo_Logger.txt");
                          System.IO.File.WriteAllText(rutaFichero, "Motivo del fallo: " + exLogger.ToString());
                      }
                  }
                  catch { }
            }
        }
    }
}


