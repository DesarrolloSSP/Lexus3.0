using Lexus2._0.Datos;
using Lexus2_0.Datos.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Lexus2._0.Expedientes
{
    public partial class Consultas : System.Web.UI.Page
    {
        BDJuridicoEntities ctx = new BDJuridicoEntities();
        int tipofecha = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //AdminPersonas adminPers = new AdminPersonas();
                //int nivel = 0;
                //int idArea = adminPers.idAreaOrganizacional(Roles.GetRolesForUser(User.Identity.Name)[0], out nivel);
                //if (idArea == -2)
                //    Response.Redirect("~/Default.aspx");
                //else
                //    hfIdArea.Value = idArea.ToString();
                llenar_grid();


            }
        }

        public void llenar_grid()
        {
            var asutnto = "";
            string fecha1 = "";
            string fecha2 = "";
            if (string.IsNullOrEmpty(txtAsuntoDoc.Text))
            {
                asutnto = "";
            }
            else
            {
                asutnto = txtAsuntoDoc.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "InicializarRemarcarTexto", "InicializarRemarcarTexto();", true);

            }


            if (rblTipoFecha.SelectedValue == "1")
            {
                tipofecha = 1;
            }
            if (rblTipoFecha.SelectedValue == "2")
            {
                tipofecha = 2;
            }
            if (rblTipoFecha.SelectedValue == "3")
            {
                tipofecha = 3;
            }


            if (string.IsNullOrEmpty(txtFechaInicial.Text))
            {
                fecha1 = "";
            }
            else
            {
                fecha1 = txtFechaInicial.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "InicializarRemarcarTexto", "InicializarRemarcarTexto();", true);

            }

            if (string.IsNullOrEmpty(txtFechaFinal.Text))
            {
                fecha2 = "";
            }
            else
            {
                fecha2 = txtFechaFinal.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "InicializarRemarcarTexto", "InicializarRemarcarTexto();", true);

            }

            var busqeda = ctx.sp_DetalleDocumentoExp(asutnto, tipofecha, fecha1, fecha2).ToList();
            gvDocumentos.DataSource = busqeda;
            gvDocumentos.DataBind();
            totalregis.InnerHtml = Convert.ToString(ctx.sp_DetalleDocumentoExp(asutnto, tipofecha, fecha1, fecha2).Count());
        }

        protected void gvDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDocumentos.PageIndex = e.NewPageIndex;
            //LLenarDatosConsejera();
            llenar_grid();
            this.gvDocumentos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            llenar_grid();
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtAsuntoDoc.Text = "";
            txtFechaFinal.Text = "";
            txtFechaInicial.Text = "";
            rblTipoFecha.SelectedIndex = 0;
            CleanControl(this.Controls);
            llenar_grid();
        }
        public void CleanControl(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).ClearSelection();
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())


                    CleanControl(control.Controls);
            }
        }

        protected void gvDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int numFila;
            if (e.CommandName == "EditarDocumento")
            {
                int idExpedActDocumento = Convert.ToInt32(e.CommandArgument.ToString());
                Session["idDocumento"] = idExpedActDocumento;
                Response.Redirect("~/Expedientes/Editar_documento.aspx");
                //Response.Redirect("~/Expedientes/DoctoEdicion.aspx");

            }
            if (e.CommandName == "VerDetalleDocumento")
            {
                int idExpedActDocumento = Convert.ToInt32(e.CommandArgument.ToString());
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                tbExpedActDocumentos documento = ctx.tbExpedActDocumentos.Find(idExpedActDocumento);
                //var exp_actuaciones = documento.idExpedActuaciones;
                //tbExpedActuaciones exp_actu = ctx.tbExpedActuaciones.Find(exp_actuaciones);
                //var idsubarea = exp_actu.IdSubAreaJuridica;
                string CAPTU = "";
                var idex = documento.idExpedActuaciones;
                var personaRecibio = ctx.tbExpedActPersonas.Where(p => p.idFigura == (int)AdminPersonas.FIGURAS_CLAVE.PERSONA_RECIBE && p.idExpedActuacion == idex)
               .Select(p => new { nombre = p.tbPersonas.nombreCompleto, id = p.idPersona }).FirstOrDefault();
                if (personaRecibio != null)
                {
                     CAPTU  = personaRecibio.nombre;
                    
                }


                var documentoExpediente = ctx.vDetalleDocumentos.Where(a => a.id == idExpedActDocumento).FirstOrDefault();
                if (documentoExpediente != null)
                {
                    var subarea = (from a in ctx.tbExpedActuaciones
                                   join b in ctx.tbOrganismos on a.IdSubAreaJuridica equals b.id
                                   where b.id == documentoExpediente.IdSubAreaJuridica
                                   select new { a = b.nombre }).FirstOrDefault();

                    var nombre = "";
                    if (subarea != null)
                    {
                        nombre = subarea.a;
                    }
                    else
                    {
                        nombre = "S/D";
                    }


                    try
                    {
                        lblVTitulo.Text = $"DOCUMENTO [{documentoExpediente.numeroDocumento}]";
                        lblVDetalle.Text = "<ul>" +
                            $"<li><b>FECHA DOCUMENTO:</b> {(documentoExpediente.fechaDocumento.HasValue ? documentoExpediente.fechaDocumento.Value.ToShortDateString() : "S/D")}</li>" +
                            $"<li><b>RECIBIDO EL:</b> {(documentoExpediente.fechaRecepcion.HasValue ? documentoExpediente.fechaRecepcion.Value.ToString() : "S/D")}</li>" +
                             $"<li><b>REGISTRADO EL:</b> {documentoExpediente.C_fechaReg.Value}</li>" +
                            $"<li><b>DE:</b> {(documentoExpediente.nombreOrgOrigen != null ? documentoExpediente.nombreOrgOrigen : "S/D")}</li>" +
                            $"<li><b>PARA:</b> {(documentoExpediente.nombreOrgDestino != null ? documentoExpediente.nombreOrgDestino : "S/D")}</li>" +
                            $"<li><b>Sub área:</b> {(nombre != null ? nombre : "S/D")}</li>" +
                            $"<li><b>CAPTURISTA:</b> {CAPTU}</li>" +
                            "</ul>";
                        txtVAsunto.Text = documentoExpediente.asunto;
                        hfIDDoctoVisualizacion.Value = documentoExpediente.id.ToString();
                        btnDescargar.Visible = documento.tbArchivos != null;
                        divVerDocumento.Style.Add("display", documento.tbArchivos != null ? "block;" : "none");
                        if (documento.tbArchivos != null)
                       {
                            string id = documento.tbArchivos.id.ToString();
                            string embed = "<object data=\"{0}{1}\" type=\"application/pdf\" width=\"800px\" height=\"500px\" style=\"max-width: 100%;\">";
                            embed += "De no ser posible visualizar el archivo, se puede descargar aquí: <a href = \"{0}{1}&download=1\">Aquí</a>";
                            embed += " o descargar <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> para ver el archivo.";
                            embed += "</object>";
                            litPDF.Text = string.Format(embed, ResolveUrl("~/Services/ArchivoBD.ashx?Id="), id);
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalVerDetalleDocumento", "MostarModalVerDetalleDocumento();", true);
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }

            if (e.CommandName == "ELIMINARDocumento")
            {
                int idExpedActDocumento = Convert.ToInt32(e.CommandArgument.ToString());
                Session["idDocumentoEliminar"] = idExpedActDocumento;
                ScriptManager.RegisterStartupScript(this, GetType(), "EliminarModalDocumento", "EliminarModalDocumento();", true);
                //Response.Redirect("~/Expedientes/DoctoEdicion.aspx");

            }
        }

        protected void btnAcptEliminar_Click(object sender, EventArgs e)
        {
            if (Session["idDocumentoEliminar"] != null)
            {

                int idExpedActDocumento = Convert.ToInt32(Session["idDocumentoEliminar"]);

                var buscardoc = ctx.tbExpedActDocumentos.Where(x => x.id == idExpedActDocumento).FirstOrDefault();
                if (buscardoc != null)
                {
                    ctx.tbExpedActDocumentos.Remove(buscardoc);
                    ctx.SaveChanges();
                    ScriptManager.RegisterStartupScript(this, GetType(), "guardado", "guardado();", true);
                    llenar_grid();
                    Console.Write("OK");
                }
            }

        }

        protected void gvDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton editareliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                var sUsuarioActual = System.Web.Security.Membership.GetUser().UserName;
                if (sUsuarioActual == "EGONZALEZC" || sUsuarioActual == "BELISARIO")
                {
                    editareliminar.Visible = true;
                }
                else
                {
                    editareliminar.Visible = false;
                }

            }
        }
    }
}