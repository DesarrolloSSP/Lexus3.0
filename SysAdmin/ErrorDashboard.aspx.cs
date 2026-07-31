using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lexus2._0.SysAdmin
{
    public partial class ErrorDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarErrores();
            }
        }

            /*    private void CargarErrores()
                {
                    string cadena =
                        new EntityConnection(
                            ConfigurationManager
                                .ConnectionStrings["BDJuridicoEntities"]
                                .ConnectionString)
                        .StoreConnection
                        .ConnectionString;

                    using (SqlConnection cn = new SqlConnection(cadena))
                    {
                        string sql = @"
                        SELECT TOP 500
                        ErrorLogId,
                        Fecha,
                        Usuario,
                        ErrorType,
                        Pagina,
                        LEFT(Mensaje,100) as Mensaje
                        FROM ErrorLog
                        ORDER BY Fecha DESC";

                        SqlDataAdapter da =
                            new SqlDataAdapter(sql, cn);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        gvErrores.DataSource = dt;
                        gvErrores.DataBind();
                    }
                }
            */
            /*   protected void btnBuscar_Click(object sender, EventArgs e)
               {
                    CargarErrores();
               }
            */

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DateTime fechaFiltro;
               if (DateTime.TryParseExact(txtFechaB.Text, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaFiltro))
            {
                CargarErrores(fechaFiltro);
            }
            else
            {
                CargarErrores(null);
            }
        }

        private void CargarErrores(DateTime? fechaFiltro = null)
        {
            string cadena =
                new EntityConnection(
                    ConfigurationManager
                        .ConnectionStrings["BDJuridicoEntities"]
                        .ConnectionString)
                .StoreConnection
                .ConnectionString;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string sql = @"
                SELECT TOP 500
                ErrorLogId,
                Fecha,
                Usuario,
                ErrorType,
                Pagina,
                LEFT(Mensaje, 100) as Mensaje
                FROM ErrorLog
                WHERE 1=1 "; 

                if (fechaFiltro.HasValue)
                {
                    sql += " AND CAST(Fecha AS DATE) = @FechaBuscada ";
                }

                sql += " ORDER BY Fecha DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    if (fechaFiltro.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@FechaBuscada", fechaFiltro.Value.Date);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvErrores.DataSource = dt;
                        gvErrores.DataBind();
                    }
                }
            }
        }

        protected void lnkDetalle_Click(object sender, EventArgs e)
        {
            LinkButton btn =
                (LinkButton)sender;

            int id =
                Convert.ToInt32(
                    btn.CommandArgument);

            MostrarDetalle(id);
        }

        private void MostrarDetalle(int errorLogId)
        {
            try
            {
                string cadena =
                    new EntityConnection(
                        ConfigurationManager
                            .ConnectionStrings["BDJuridicoEntities"]
                            .ConnectionString)
                    .StoreConnection
                    .ConnectionString;

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string sql = @"
                    SELECT
                    ErrorLogId,
                    Fecha,
                    Usuario,
                    Pagina,
                    ErrorType,
                    Mensaje,
                    StackTrace,
                    Url,
                    IpAddress,
                    UserAgent
                    FROM ErrorLog
                    WHERE ErrorLogId = @ErrorLogId";
                    SqlCommand cmd =
                        new SqlCommand(sql, cn);

                    cmd.Parameters.AddWithValue(
                        "@ErrorLogId",
                        errorLogId);

                    cn.Open();

                    SqlDataReader dr =
                        cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        //pnlDetalle.Visible = true;

                        lblFecha.Text =

                            Convert.ToDateTime(dr["Fecha"])
                                .ToString("dd/MM/yyyy HH:mm:ss");

                        lblUsuario.Text =

                            Convert.ToString(dr["Usuario"]);

                        lblPagina.Text =

                            Convert.ToString(dr["Pagina"]);

                        lblTipoError.Text =

                            Convert.ToString(dr["ErrorType"]);

                        txtStackTrace.Text =
                            Convert.ToString(dr["StackTrace"]);
                    }

                    ClientScript.RegisterStartupScript(
                    GetType(),
                    "mostrarModal",
                    @"
                    var modal =
                        new bootstrap.Modal(
                            document.getElementById('modalDetalle'));
                    modal.show();",
                    true);
                }
            }
            catch (Exception ex)
            {
                 txtStackTrace.Text =
                 ex.ToString();
                 ClientScript.RegisterStartupScript(
                 GetType(),
                 "mostrarModal",
                 @"
                 var modal =
                 new bootstrap.Modal(
                 document.getElementById('modalDetalle'));
                 modal.show();",
                 true);
            }
        }
    }
}