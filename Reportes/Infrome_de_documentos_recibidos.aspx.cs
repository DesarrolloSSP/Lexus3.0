using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lexus2._0.Reportes
{
    public partial class Infrome_de_documentos_recibidos : System.Web.UI.Page
    {
        bool valido = true;
        string textoValidacion = "<ul>";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "spinnerver", "spinnerver();", true);

            if (string.IsNullOrEmpty(this.txtFechaini.Text) || string.IsNullOrEmpty(this.txtDateFin.Text))
            {
                textoValidacion += "<li> Es necesario ingresar las fechas para poder generar el reporte. </ li>";
                valido = false;
               

            }

            if (string.IsNullOrEmpty(this.txtDateFin.Text) || string.IsNullOrEmpty(this.txtDateFin.Text))
            {
                textoValidacion += "<li> Es necesario ingresar las fechas para poder generar el reporte. </ li>";
                valido = false;


            }
            if (!valido)
            {
                textoValidacion += "</ul>";
                lblMeNSAJE.Text = textoValidacion;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "AbrirModalValidador", "AbrirModalValidador();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "spinnerocultar", "spinnerocultar();", true);

            }


            else
            {

                this.ReportePdf.Visible = true;
                this.ReportViewer1.ProcessingMode = ProcessingMode.Remote;
                this.ReportViewer1.ServerReport.ReportPath = "/Juridico/Reporte_lexus";
                this.ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://10.8.3.199/reportserver");
                ReportParameter[] parametros = new ReportParameter[2];
                parametros[0] = new ReportParameter("FechaInicial", txtFechaini.Text);
                parametros[1] = new ReportParameter("FechaFinal", txtDateFin.Text);



                ReportViewer1.ServerReport.SetParameters(parametros);
                ReportViewer1.ServerReport.Refresh();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "spinnerocultar", "spinnerocultar();", true);
            }
        }
    }
}