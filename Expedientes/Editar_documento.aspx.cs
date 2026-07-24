
using Lexus2._0.Datos;
using Lexus2_0.Datos.Auxiliares;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Filters;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Management;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesseract;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace Lexus2._0.Expedientes
{
    public partial class Editar_documento : System.Web.UI.Page
    {
        int idDocto = 0;
        Guid GuididxpedienteEdit;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PreviousPageUrl"] = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "~/Expedientes/Consultas/BandejaDoc.aspx";

                if (Session["idDocumento"] == null || !int.TryParse(Session["idDocumento"].ToString(), out idDocto))
                    Response.Redirect(ViewState["PreviousPageUrl"].ToString());//Response.Redirect("~/Expedientes/Consultas/BandejaDocumentos.aspx");
                if (Session["EnEdicion"] != null && DateTime.Parse(Session["EnEdicion"].ToString()).AddMinutes(1) > DateTime.Now)
                    Response.Redirect(ViewState["PreviousPageUrl"].ToString());//Response.Redirect("~/Expedientes/Consultas/BandejaDocumentos.aspx");

                hfIDDocto.Value = idDocto.ToString();

                Session["idDocumento"] = null;

                if (Session["idExpedEdicion"] != null)
                    hfIdExped.Value = Session["idExpedEdicion"].ToString();
                Session["idExpedEdicion"] = null;
                VisualizarDocumento(idDocto);
            }
        }
        #region SECCIÓN DEFAULT ==================================================
        protected void VisualizarDocumento(int idDocto)
        {
            try
            {

                ddlTipoAsunto.DataBind();
                DefaultFechaHora();
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                var visualizar = ctx.vDetalleDocumentos.Where(a => a.id == idDocto).FirstOrDefault();
                if (visualizar != null)
                {
                    ddlActuacion.DataBind();
                    ddlActuacion.SelectedValue = Convert.ToString(visualizar.idActuacion);
                    divDestino.Visible = visualizar.idActuacion != (int)AdminActuacion.ACTUACIONES.RECEPCION;

                    ddlAreaAsignada.SelectedValue = Convert.ToString(visualizar.idOrgAreaJuridica);
                    if (visualizar.idOrgDestino == 63)
                    {
                        divSubarea.Visible = true;
                        ddlSubAre.SelectedValue = Convert.ToString(visualizar.IdSubAreaJuridica);
                    }
                    else
                    {
                        ddlSubAre.SelectedIndex = -1;
                    }

                    if (visualizar.idOrgOrigen == 67)
                    {
                        divProcedenciaDDH.Visible = true;
                        hfidOrgProced.Value = Convert.ToString(visualizar.idOrgOrigen);
                        ddlAreasDDH.SelectedValue = Convert.ToInt32(visualizar.idOrgOrigen).ToString();
                    }
                    else
                    {
                        divProcedenciaDDH.Visible = false;
                        txtOrgProcedencia.Text = visualizar.nombreOrgOrigen;
                        hfidOrgProced.Value = Convert.ToString(visualizar.idOrgOrigen);
                    }


                    if (visualizar.idTipoAsunto.HasValue)
                    {
                        divTipoAsunto.Visible = true;
                        int x = Convert.ToInt32(visualizar.idTipoAsunto);
                        ddlTipoAsunto.SelectedValue = x.ToString();
                        hfIDTipoAsunto.Value = Convert.ToString(visualizar.idTipoAsunto);
                        Session["idTipo"] = x.ToString();
                        if (visualizar.idExpedAct != null)
                        {
                            //Llenar otro asunto
                            divTipoAsuntoOtro.Visible = true;
                            txtTipoAsuntoOtro.Text = visualizar.extra;
                        }
                        else
                        {
                            divTipoAsuntoOtro.Visible = false;
                        }

                    }
                    else
                    {
                        divTipoAsunto.Visible = true;
                        divTipoAsuntoOtro.Visible = false;
                    }

                    AdminPersonas adminPers = new AdminPersonas();
                    int jerarq = 0;
                    int idAreaPersona = adminPers.idAreaOrganizacional(Roles.GetRolesForUser(User.Identity.Name)[0], out jerarq);
                    bool activo = jerarq == 0 && idAreaPersona != 0;
                    //Si es una cuenta con jerarquía alta y pertenece al área, puede editar
                    // se comento para que dejera a los usuarios editar
                    //ddlAreaAsignada.Enabled = activo;
                    lblMsjCambioArea.Visible = !activo;

                    Session["ExpActuación"] = visualizar.idExpedActuacion;
                    //var ACTUACION = ctx.tbExpedPersonas.Where(p => p.idExpediente == visualizar.idExpediente && p.idFigura != 19).ToList();
                    //hfIdPersona.Value = Convert.ToInt32( ACTUACION.First().idPersona).ToString();

                    var actuaci = (from a in ctx.tbExpedPersonas
                                   join b in ctx.tbPersonas on a.idPersona equals b.id
                                   join c in ctx.catFiguras on a.idFigura equals c.id
                                   where a.idExpediente == visualizar.idExpediente && a.idFigura != 17
                                   select new
                                   {
                                       b.nombreCompleto,
                                       a.idFigura,
                                       a.idPersona,
                                       c.figura,
                                       a.id

                                   }).ToList();
                    if (actuaci.Count != 0)
                    {
                        hfIdPersona.Value = Convert.ToInt32(actuaci.First().idPersona).ToString();
                        gvPersonasRelacionadas.DataSource = actuaci;
                        gvPersonasRelacionadas.DataBind();
                    }
                    else
                    {
                        //repPersonas.DataBind();

                    }





                    var personaRecibio = ctx.tbExpedActPersonas.Where(l => l.idExpedActuacion == visualizar.idExpedActuacion && l.idFigura == 17)
                   .Select(p => new { nombre = p.tbPersonas.nombreCompleto, id = p.idPersona }).FirstOrDefault();
                    if (personaRecibio != null)
                    {
                        txtDocPersonaRecibio.Text = personaRecibio.nombre;
                        hfidPersRecib.Value = personaRecibio.id.ToString();
                        txtDocPersonaRecibio.Enabled = false;
                    }
                    //tbExpedActDocumentos docto = actuacion.tbExpedActDocumentos.FirstOrDefault();
                    var docto = ctx.tbExpedActDocumentos.Where(n => n.idExpedActuaciones == visualizar.idExpedActuacion).FirstOrDefault();
                    if (docto != null)
                    {
                        txtNumDocto.Text = docto.numeroDocumento;
                        txtFechaRealizacionDocto.Text = docto.fechaDocumento.Value.ToShortDateString();
                        txtFechaRecepcionDocto.Text = docto.fechaRecepcion.Value.ToShortDateString();
                        ddlFechaRecepHora.SelectedValue = docto.fechaRecepcion.Value.Hour < 10 ? "0" + docto.fechaRecepcion.Value.Hour : docto.fechaRecepcion.Value.Hour.ToString();
                        ddlFechaRecepMin.SelectedValue = docto.fechaRecepcion.Value.Minute < 10 ? "0" + docto.fechaRecepcion.Value.Minute : docto.fechaRecepcion.Value.Minute.ToString();
                        txtAsuntoDoc.Text = docto.asunto;
                        ddlTipoDocumento.DataBind();
                        ddlTipoDocumento.SelectedValue = docto.idTipoDeDocumento.ToString();
                        ddlTipoRecep.DataBind();
                        ddlTipoRecep.SelectedValue = docto.idTipoRecepcionDocumento.ToString();

                        rblRequiereTermino.SelectedValue = "1";
                        if (visualizar.fechaTermino.HasValue)
                        {
                            txtFechaTermino.Text = visualizar.fechaTermino.Value.ToShortDateString();
                            ddlHora.SelectedValue = visualizar.fechaTermino.Value.Hour < 10 ? "0" + visualizar.fechaTermino.Value.Hour : visualizar.fechaTermino.Value.Hour.ToString();
                            ddlMinutos.SelectedValue = visualizar.fechaTermino.Value.Minute < 10 ? "0" + visualizar.fechaTermino.Value.Minute : visualizar.fechaTermino.Value.Minute.ToString();
                        }
                        //Verificar si el documento tiene su equivalente digital

                    }


                }
            }
            catch (Exception ex)
            {

            }
        }


        private void DefaultFechaHora()
        {
            ddlHora.Items.Clear();
            ddlSFTHora.Items.Clear();
            ddlFechaRecepHora.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    ddlHora.Items.Add(new ListItem("0" + i, i.ToString()));
                    ddlSFTHora.Items.Add(new ListItem("0" + i, i.ToString()));
                    ddlFechaRecepHora.Items.Add(new ListItem("0" + i, i.ToString()));
                }
                else
                {
                    ddlHora.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    ddlSFTHora.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    ddlFechaRecepHora.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            ddlMinutos.Items.Clear();
            ddlSFTMinutos.Items.Clear();
            ddlFechaRecepMin.Items.Clear();
            for (int x = 0; x < 60; x++)
            {
                if (x < 10)
                {
                    ddlMinutos.Items.Add(new ListItem("0" + x, x.ToString()));
                    ddlSFTMinutos.Items.Add(new ListItem("0" + x, x.ToString()));
                    ddlFechaRecepMin.Items.Add(new ListItem("0" + x, x.ToString()));
                }
                else
                {
                    ddlMinutos.Items.Add(new ListItem(x.ToString(), x.ToString()));
                    ddlSFTMinutos.Items.Add(new ListItem(x.ToString(), x.ToString()));
                    ddlFechaRecepMin.Items.Add(new ListItem(x.ToString(), x.ToString()));
                }
            }
        }
        protected void ddlAreasDDH_SelectedIndexChanged(object sender, EventArgs e)
        {
            divProcedenciaDDH.Visible = ddlAreasDDH.SelectedValue != "0";
            divProcedenciaTodos.Visible = ddlAreasDDH.SelectedValue == "0";
        }

        protected void ddlAreasDDH_DataBound(object sender, EventArgs e)
        {
            ddlAreasDDH.Items.Add(new ListItem("-OTRO-", "0"));
        }

        protected void ddlAreaAsignada_DataBound(object sender, EventArgs e)
        {
            ddlAreaAsignada.Items.Add(new ListItem("-SELECCIONE-", "0"));
        }


        protected void ddlAreaAsignada_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idDDH = ((int)AdminOrganismos.AREAS_JURIDICO.DEPTO_DERECHOS_HUMANOS).ToString();
            string idArea = ddlAreaAsignada.SelectedValue;
            divProcedenciaDDH.Visible = idArea == idDDH;
            divProcedenciaTodos.Visible = idArea != idDDH && idArea != "0";

            divTipoAsunto.Visible = idArea != "0";
            ddlAreasDDH.ClearSelection();
            ddlTipoAsunto.DataBind();
            ddlTipoAsunto.SelectedValue = "0";
            Session["DetalleTipoAsunto"] = null;
            repInformacionTipoAsunto.Visible = false;
            //63
            if (ddlAreaAsignada.SelectedValue == "63")
            {
                divSubarea.Visible = true;
            }
            else
            {
                divSubarea.Visible = false;
            }
        }

        #endregion
        #region SECCIÓN HELPERS =========================================================
        static class EntityDataSourceExtensions
        {
            public static TEntity GetItemObject<TEntity>(object dataItem) where TEntity : class
            {
                var entity = dataItem as TEntity;
                if (entity != null)
                {
                    return entity;
                }
                var td = dataItem as ICustomTypeDescriptor;
                if (td != null)
                {
                    return (TEntity)td.GetPropertyOwner(null);
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the MIME type of the file name specified based on the file name's
        /// extension.  If the file's extension is unknown, returns "octet-stream"
        /// generic for streaming file bytes.
        /// </summary>
        /// <param name="sFileName">The name of the file for which the MIME type
        /// refers to.</param>
        public string GetMimeTypeByFileName(string sFileName)
        {
            string sMime = "application/octet-stream";

            string sExtension = System.IO.Path.GetExtension(sFileName);
            if (!string.IsNullOrEmpty(sExtension))
            {
                sExtension = sExtension.Replace(".", "");
                sExtension = sExtension.ToLower();

                if (sExtension == "xls" || sExtension == "xlsx")
                {
                    sMime = "application/ms-excel";
                }
                else if (sExtension == "doc" || sExtension == "docx")
                {
                    sMime = "application/msword";
                }
                else if (sExtension == "ppt" || sExtension == "pptx")
                {
                    sMime = "application/ms-powerpoint";
                }
                else if (sExtension == "rtf")
                {
                    sMime = "application/rtf";
                }
                else if (sExtension == "zip")
                {
                    sMime = "application/zip";
                }
                else if (sExtension == "mp3")
                {
                    sMime = "audio/mpeg";
                }
                else if (sExtension == "bmp")
                {
                    sMime = "image/bmp";
                }
                else if (sExtension == "gif")
                {
                    sMime = "image/gif";
                }
                else if (sExtension == "jpg" || sExtension == "jpeg")
                {
                    sMime = "image/jpeg";
                }
                else if (sExtension == "png")
                {
                    sMime = "image/png";
                }
                else if (sExtension == "tiff" || sExtension == "tif")
                {
                    sMime = "image/tiff";
                }
                else if (sExtension == "txt")
                {
                    sMime = "text/plain";
                }
            }
            return sMime;
        }
        /// <summary>
        /// Streams the bytes specified as a file with the name specified using HTTP to the 
        /// calling browser.
        /// </summary>
        /// <param name="sFileName">The name of the file as it will apear when the user
        /// clicks either open or save as in their browser to accept the file
        /// download.</param>
        /// <param name="fileBytes">The file as a byte array to be streamed.</param>
        public void StreamFileToBrowser(string sFileName, byte[] fileBytes)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = GetMimeTypeByFileName(sFileName);
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + sFileName);
            Response.BinaryWrite(fileBytes);
            Response.Flush();
            Response.End();
        }
        #endregion
        #region SECCIÓN - DETALLE PROCEDENCIA ===========================================
        protected void ddlTipoAsunto_DataBound(object sender, EventArgs e)
        {
            ddlTipoAsunto.Items.Add(new ListItem("-SELECCIONE-", "0"));
        }

        #region  code comentado
        //protected void repPersonas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{




        //    TextBox txtPersonaRelacionada = e.Item.FindControl("txtPersonaRelacionada") as TextBox;
        //    HiddenField hfIdPersona = e.Item.FindControl("hfIdPersona") as HiddenField;
        //    DropDownList ddlFiguraComo = e.Item.FindControl("ddlFiguraComo") as DropDownList;



        //    tbExpedActPersonas reg = EntityDataSourceExtensions.GetItemObject<tbExpedActPersonas>(e.Item.DataItem);
        //    if (ddlFiguraComo != null && reg != null && hfIdPersona != null && txtPersonaRelacionada != null)
        //    {
        //        ddlFiguraComo.SelectedValue = reg.idFigura.ToString();
        //        if (reg.tbPersonas != null)
        //        {
        //            txtPersonaRelacionada.Text = reg.tbPersonas.nombre + " " + reg.tbPersonas.apellidoPaterno + " " + reg.tbPersonas.apellidoMaterno;
        //            hfIdPersona.Value = reg.tbPersonas.id + "";
        //        }
        //    }


        //}
        //protected void repPersonas_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName.Equals("EliminarRegistroPersona"))
        //    {
        //        e.Item.Visible = false;
        //    }
        //    else if (e.CommandName.Equals("NuevaPersona"))
        //    {

        //        List<tbExpedActPersonas> listaPersonasRelacionadas = new List<tbExpedActPersonas>();
        //        foreach (RepeaterItem item in repPersonas.Items)
        //        {
        //            if (item.Visible)
        //            {
        //                TextBox txtPersonaRelacionada = item.FindControl("txtPersonaRelacionada") as TextBox;
        //                HiddenField hfIdPersona = item.FindControl("hfIdPersona") as HiddenField;
        //                DropDownList ddlFiguraComo = item.FindControl("ddlFiguraComo") as DropDownList;
        //                if (txtPersonaRelacionada != null && hfIdPersona != null && ddlFiguraComo != null)
        //                {
        //                    int id = 0;
        //                    listaPersonasRelacionadas.Add(new tbExpedActPersonas()
        //                    {
        //                        tbPersonas = new tbPersonas() { nombre = txtPersonaRelacionada.Text, id = int.TryParse(hfIdPersona.Value, out id) ? id : 0 },
        //                        idFigura = int.TryParse(ddlFiguraComo.SelectedValue, out id) ? id : 0
        //                    });
        //                }
        //            }
        //        }
        //        listaPersonasRelacionadas.Add(new tbExpedActPersonas()
        //        {
        //            idPersona = 0,
        //            idFigura = 0
        //        });
        //        repPersonas.DataSource = listaPersonasRelacionadas;
        //        repPersonas.DataBind();

        //    }
        //}

        //protected List<tbExpedActPersonas> GetListaPersonasRelacionadas()
        //{
        //    List<tbExpedActPersonas> listaPersonasRelacionadas = new List<tbExpedActPersonas>();
        //    foreach (RepeaterItem item in repPersonas.Items)
        //    {
        //        if (item.Visible)
        //        {
        //            TextBox txtPersonaRelacionada = item.FindControl("txtPersonaRelacionada") as TextBox;
        //            HiddenField hfIdPersona = item.FindControl("hfIdPersona") as HiddenField;
        //            DropDownList ddlFiguraComo = item.FindControl("ddlFiguraComo") as DropDownList;
        //            if (txtPersonaRelacionada != null && hfIdPersona != null && ddlFiguraComo != null && !string.IsNullOrEmpty(txtPersonaRelacionada.Text) && !string.IsNullOrWhiteSpace(txtPersonaRelacionada.Text))
        //            {
        //                int id = 0;
        //                listaPersonasRelacionadas.Add(new tbExpedActPersonas()
        //                {
        //                    tbPersonas = new tbPersonas() { nombreCompleto = Regex.Replace(txtPersonaRelacionada.Text.Trim().ToUpper(), @"\s{2,}", @"\s", RegexOptions.IgnoreCase), id = int.TryParse(hfIdPersona.Value, out id) ? id : 0 },
        //                    idFigura = int.TryParse(ddlFiguraComo.SelectedValue, out id) ? id : 0
        //                });
        //            }
        //        }
        //    }
        //    return listaPersonasRelacionadas;
        //}
        #endregion


        #endregion
        #region SECCIÓN - DATOS DEL DOCUMENTO =====================================
        protected void btnCrearyContinuar_Click(object sender, EventArgs e)
        {
            CrearNuevoDocumentoYContinuar();
        }
        protected void CrearNuevoDocumentoYContinuar()
        {
            try
            {
                string textoValidaciones = "";
                bool Puede = true;

                int idOrgProcedencia = 0;
                DateTime fechaTermAct = new DateTime();
                int NUMERO_DOCUMENTO = Convert.ToInt32(Session["ExpActuación"]);
                int idAreaAsignada = 0;


                BDJuridicoEntities ctx = new BDJuridicoEntities();

                if (string.IsNullOrEmpty(ddlAreaAsignada.SelectedValue))
                {
                    textoValidaciones += "<li>SELECCIONE UNA ÁREA ASIGNADA</li>";
                    Puede = false;
                }
                if (DateTime.TryParse(txtFechaTermino.Text, out fechaTermAct))
                {
                    fechaTermAct = new DateTime(fechaTermAct.Year, fechaTermAct.Month, fechaTermAct.Day, int.Parse(ddlHora.SelectedValue), int.Parse(ddlMinutos.SelectedValue), 0);
                }


                int idTipoAsunto = 0;
                if (!int.TryParse(ddlTipoAsunto.SelectedValue, out idTipoAsunto) || idTipoAsunto <= 0)
                {
                    textoValidaciones += "<li>TIPO DE ASUNTO</li>";
                }
                if (!Puede)
                {
                    textoValidaciones += "</ul>";
                    lblTitulo.InnerText = "FAVOR DE LLENAR LO SIGUIENTE";
                    lblMeNSAJE.Text = textoValidaciones;
                    ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalMensajes", "MostarModalMensajes();", true);
                }

                else
                {

                    int idDocto = int.Parse(hfIDDocto.Value);
                    int idExpedActuaciones = 0;

                    #region tbExpedActDocumentos
                    tbExpedActDocumentos ExpedinteActDocuEditada = ctx.tbExpedActDocumentos.Where(d => d.id == idDocto).FirstOrDefault();
                    if (ExpedinteActDocuEditada != null)
                    {
                        ExpedinteActDocuEditada.numeroDocumento = txtNumDocto.Text.Trim();
                        if (txtFechaRealizacionDocto.Text != "")
                            ExpedinteActDocuEditada.fechaDocumento = Convert.ToDateTime(txtFechaRealizacionDocto.Text);
                        else
                            ExpedinteActDocuEditada.fechaDocumento = null;

                        ExpedinteActDocuEditada.asunto = txtAsuntoDoc.Text.ToUpper().Trim();

                        idExpedActuaciones = Convert.ToInt32(ExpedinteActDocuEditada.idExpedActuaciones);
                    }
                    #endregion

                    #region EDITAR tbExpedActuaciones
                    tbExpedActuaciones actuacionEditada = ctx.tbExpedActuaciones.Where(d => d.id == idExpedActuaciones).FirstOrDefault();
                    if (actuacionEditada != null)
                    {
                        GuididxpedienteEdit = (Guid)actuacionEditada.idExpediente;


                        actuacionEditada.idOrganismoDestino = Convert.ToInt32(ddlAreaAsignada.SelectedValue);
                        actuacionEditada.accion = txtAsuntoDoc.Text.ToUpper().Trim();
                        actuacionEditada.fechaTermino = fechaTermAct;
                        if (idOrgProcedencia == 0)
                        {
                            //Buscar y registrar
                            string textoProcedencia = txtOrgProcedencia.Text.Trim();
                            tbOrganismos org = ctx.tbOrganismos.Where(o => o.nombre.Equals(textoProcedencia)).FirstOrDefault();
                            if (org == null)
                            {
                                org = new tbOrganismos();
                                org.nombre = textoProcedencia;
                                org.nivel = 1;
                                org.jerarquia = "";
                                org.C_fechaRegistro = DateTime.Now;
                                ctx.tbOrganismos.Add(org);
                                ctx.SaveChanges();
                            }
                            idOrgProcedencia = org.id;
                        }
                        actuacionEditada.idOrganismoOrigen = idOrgProcedencia;
                        actuacionEditada.idAreaJuridica = Convert.ToInt32(ddlAreaAsignada.SelectedValue);
                        if (ddlAreaAsignada.SelectedValue == "63")
                        {
                            actuacionEditada.IdSubAreaJuridica = Convert.ToInt32(ddlSubAre.SelectedValue);
                        }
                        actuacionEditada.idTipoAsunto = idTipoAsunto;
                        if (divTipoAsuntoOtro.Visible)
                        {

                            var ActuacionesExtraEdit = ctx.tbExpedActExtraAsunto.Where(t => t.idExpedAct == NUMERO_DOCUMENTO).FirstOrDefault();
                            if (ActuacionesExtraEdit != null)
                            {
                                ActuacionesExtraEdit.extra = txtTipoAsuntoOtro.Text;
                            }
                        }
                    }
                    #endregion


                    #region EDITAR tbExpedientes
                    tbExpedientes ExpedinteEditada = ctx.tbExpedientes.Where(d => d.id == GuididxpedienteEdit).FirstOrDefault();
                    if (actuacionEditada != null)
                    {
                        if (txtFechaRecepcionDocto.Text != "")
                        {
                            int idFRHora = int.Parse(ddlFechaRecepHora.SelectedValue);
                            int idFRMin = int.Parse(ddlFechaRecepMin.SelectedValue);
                            DateTime FechaRecep = Convert.ToDateTime(txtFechaRecepcionDocto.Text);
                            ExpedinteEditada.fechaYhoraRecibido = new DateTime(FechaRecep.Year, FechaRecep.Month, FechaRecep.Day, idFRHora, idFRMin, 0);
                        }
                        else
                        {
                            ExpedinteEditada.fechaYhoraRecibido = null;
                        }

                        actuacionEditada.fechaTermino = fechaTermAct;
                        actuacionEditada.idTipoAsunto = Convert.ToInt32(ddlTipoAsunto.SelectedValue);
                        if (divTipoAsuntoOtro.Visible)
                        {
                            var ActuacionesExtraEdit = ctx.tbExpedActExtraAsunto.Where(t => t.idExpedAct == NUMERO_DOCUMENTO).FirstOrDefault();
                            if (ActuacionesExtraEdit != null)
                            {
                                ActuacionesExtraEdit.extra = txtTipoAsuntoOtro.Text;
                            }
                        }
                        ExpedinteEditada.asunto = txtAsuntoDoc.Text.ToUpper().Trim();
                        if (idOrgProcedencia == 0)
                        {
                            //Buscar y registrar
                            string textoProcedencia = txtOrgProcedencia.Text.Trim();
                            tbOrganismos org = ctx.tbOrganismos.Where(o => o.nombre.Equals(textoProcedencia)).FirstOrDefault();
                            if (org == null)
                            {
                                org = new tbOrganismos();
                                org.nombre = textoProcedencia;
                                org.nivel = 1;
                                org.jerarquia = "";
                                org.C_fechaRegistro = DateTime.Now;
                                ctx.tbOrganismos.Add(org);
                                ctx.SaveChanges();
                            }
                            idOrgProcedencia = org.id;
                        }
                        ExpedinteEditada.idOrganismoOrigen = idOrgProcedencia;
                        ExpedinteEditada.idOrganismoDestino = Convert.ToInt32(ddlAreaAsignada.SelectedValue);
                        ExpedinteEditada.idAreaJuridica = Convert.ToInt32(ddlAreaAsignada.SelectedValue);

                    }
                    #endregion



                    ctx.SaveChanges();

                    var mensaje = $"<div style='width:100%; text-align:center; font-weight:bold;'>¡DOCUMENTO ACTUALIZADO CON ÉXITO!</div><hr /> <b>, AHORA PUEDE CONSULTAR EL DOCUMENTO POR SU NÚMERO INGRESADO: <b>{txtNumDocto.Text}";
                    mensajeaceptar.Text = mensaje;
                    ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalFinal", "MostarModalFinal();", true);
                }
            }
            catch (Exception ex)
            {

            }


        }

        protected void btnAgregarPersona_Click(object sender, EventArgs e)
        {
            int NUMERO_exped = Convert.ToInt32(Session["ExpActuación"]);
            BDJuridicoEntities ctx = new BDJuridicoEntities();

            string _Persona = txtPersonaRelacionada.Text.Trim();
            string[] authorsList = _Persona.Split(' ');
            var BuscarPersona = ctx.tbPersonas.Where(t => t.nombreCompleto.Equals(_Persona)).FirstOrDefault();
            if (BuscarPersona == null)
            {
                tbPersonas nuevaperso = new tbPersonas();
                nuevaperso.nombreCompleto = _Persona;
                nuevaperso.nombre = authorsList[0];
                nuevaperso.apellidoPaterno = authorsList[1];
                nuevaperso.apellidoMaterno = authorsList[2];
                nuevaperso.C_fechaReg = DateTime.Now;
                ctx.tbPersonas.Add(nuevaperso);
                ctx.SaveChanges();
            }

            var buscarExpediente = ctx.tbExpedActuaciones.Where(m => m.id == NUMERO_exped).FirstOrDefault();
            if (buscarExpediente != null)
            {
                tbExpedPersonas NuevaPersoExp = new tbExpedPersonas();
                NuevaPersoExp.idExpediente = buscarExpediente.idExpediente;
                NuevaPersoExp.idPersona = BuscarPersona.id;
                NuevaPersoExp.idFigura = Convert.ToInt32(ddlFiguraComo.SelectedValue);
                NuevaPersoExp.C_fechaReg = DateTime.Now;
                ctx.tbExpedPersonas.Add(NuevaPersoExp);
                ctx.SaveChanges();

                var actuaci = (from a in ctx.tbExpedPersonas
                               join b in ctx.tbPersonas on a.idPersona equals b.id
                               join c in ctx.catFiguras on a.idFigura equals c.id
                               where a.idExpediente == buscarExpediente.idExpediente && a.idFigura != 17
                               select new
                               {
                                   b.nombreCompleto,
                                   a.idFigura,
                                   a.idPersona,
                                   c.figura,
                                   a.id

                               }).ToList();

                gvPersonasRelacionadas.DataSource = actuaci;
                gvPersonasRelacionadas.DataBind();


            }
            else
            {
                int idOrgProcedencia = 0;
                DateTime fechaTermAct = new DateTime();
                if (rblRequiereTermino.SelectedValue == "1")
                    if (DateTime.TryParse(txtFechaTermino.Text, out fechaTermAct))
                    {
                        fechaTermAct = new DateTime(fechaTermAct.Year, fechaTermAct.Month, fechaTermAct.Day, int.Parse(ddlHora.SelectedValue), int.Parse(ddlMinutos.SelectedValue), 0);
                    }

                tbExpedActuaciones nuevaActuacion = new tbExpedActuaciones();
                //secrea un documento por que no tiene 
                tbExpedientes nuevoExpe = new tbExpedientes();
                Guid idExpedienteNuevo = Guid.NewGuid();
                nuevoExpe.id = idExpedienteNuevo;
                if (txtFechaRecepcionDocto.Text != "")
                {
                    int idFRHora = int.Parse(ddlFechaRecepHora.SelectedValue);
                    int idFRMin = int.Parse(ddlFechaRecepMin.SelectedValue);
                    DateTime FechaRecep = Convert.ToDateTime(txtFechaRecepcionDocto.Text);
                    nuevoExpe.fechaYhoraRecibido = new DateTime(FechaRecep.Year, FechaRecep.Month, FechaRecep.Day, idFRHora, idFRMin, 0);
                }
                else
                {
                    nuevoExpe.fechaYhoraRecibido = null;
                }
                nuevoExpe.fechaTermino = fechaTermAct;
                nuevoExpe.idTipoAsunto = Convert.ToInt32(ddlTipoAsunto.SelectedValue);
                if (divTipoAsuntoOtro.Visible)
                {
                    nuevaActuacion.tbExpedActExtraAsunto = new tbExpedActExtraAsunto();
                    nuevaActuacion.tbExpedActExtraAsunto.extra = txtTipoAsuntoOtro.Text;
                    nuevaActuacion.tbExpedActExtraAsunto.C_fechaReg = DateTime.Now;
                }
                nuevoExpe.asunto = txtAsuntoDoc.Text.ToUpper().Trim();
                if (idOrgProcedencia == 0)
                {
                    //Buscar y registrar
                    string textoProcedencia = txtOrgProcedencia.Text.Trim();
                    tbOrganismos org = ctx.tbOrganismos.Where(o => o.nombre.Equals(textoProcedencia)).FirstOrDefault();
                    if (org == null)
                    {
                        org = new tbOrganismos();
                        org.nombre = textoProcedencia;
                        org.nivel = 1;
                        org.jerarquia = "";
                        org.C_fechaRegistro = DateTime.Now;
                        ctx.tbOrganismos.Add(org);
                        ctx.SaveChanges();
                    }
                    idOrgProcedencia = org.id;
                }
                nuevoExpe.idOrganismoOrigen = idOrgProcedencia;
                nuevoExpe.idOrganismoDestino = Convert.ToInt32(ddlAreaAsignada.SelectedValue);
                nuevoExpe.idEjercicio = 17;
                nuevoExpe.idAreaJuridica = Convert.ToInt32(ddlAreaAsignada.SelectedValue);
                nuevoExpe.C_fechaReg = DateTime.Now;
                ctx.tbExpedientes.Add(nuevoExpe);

                tbExpedPersonas NuevaPersoExp = new tbExpedPersonas();
                NuevaPersoExp.idExpediente = idExpedienteNuevo;
                NuevaPersoExp.idPersona = BuscarPersona.id;
                NuevaPersoExp.idFigura = Convert.ToInt32(ddlFiguraComo.SelectedValue);
                NuevaPersoExp.C_fechaReg = DateTime.Now;
                ctx.tbExpedPersonas.Add(NuevaPersoExp);
                ctx.SaveChanges();
                gvPersonasRelacionadas.DataBind();

            }



        }


        public class DatosJson
        {
            public int id;                  //idPersona
            public string nombre;           //NombrePersona
            public bool presente; //Si está presente en la BD de Jurídico
            public DatosJson(int id, string nombre)
            {
                this.id = id;
                this.nombre = nombre;
            }
            public DatosJson() { }
        }

        [WebMethod(true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<DatosJson> BuscarTextos(string texto, string idHiddenField)
        {
            if (string.IsNullOrEmpty(texto))
                return null;
            try
            {
                BDJuridicoEntities ctxJur = new BDJuridicoEntities();
                List<DatosJson> consulta = new List<DatosJson>();
                if (idHiddenField.ToLower().Contains("persona"))
                {
                    consulta = (from pers in ctxJur.tbPersonas
                                where (pers.nombreCompleto).Contains(texto)
                                select new DatosJson
                                {
                                    id = pers.id,
                                    nombre = pers.nombreCompleto,
                                    presente = true
                                }).OrderBy(c => c.nombre).ToList();
                }
                else
                {
                    consulta = (from orgs in ctxJur.tbOrganismos
                                where (orgs.nombre).Contains(texto)
                                select new DatosJson
                                {
                                    id = orgs.id,
                                    nombre = orgs.nombre,
                                    presente = true
                                }).OrderBy(c => c.nombre).ToList();
                }
                return consulta;
            }
            catch (Exception e) { return null; }
        }
        #region DOCUMENTO
        protected void btnCancelarCreacion_Click(object sender, EventArgs e)
        {
            Retornar();
        }
        #endregion
        #region FECHA DE TÉRMINO
        protected void rblRequiereTermino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblRequiereTermino.SelectedValue == "1")
            {
                divSiTermino.Visible = true;
                divNoTermino.Visible = false;
            }
            else
            {
                divSiTermino.Visible = false;
                divNoTermino.Visible = true;
                DateTime f2;
                if (!string.IsNullOrEmpty(txtFechaRecepcionDocto.Text) && DateTime.TryParse(txtFechaRecepcionDocto.Text, out f2))
                {
                    DateTime fechaTermAct = new DateTime(f2.Year, f2.Month, f2.Day, int.Parse(ddlFechaRecepHora.SelectedValue), int.Parse(ddlFechaRecepMin.SelectedValue), 0);
                    fechaTermAct = fechaTermAct.AddDays(40);
                    lblFechaLimiteTerm.Text = $"{fechaTermAct}";
                }
                else
                {
                    lblFechaLimiteTerm.Text = "FAVOR DE COLOCAR UNA FECHA DE RECEPCIÓN PARA PODER CALCULAR LA FECHA APROXIMADA DE TÉRMINO";
                }
            }
        }
        protected void btnVisualizarTermino_ServerClick(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(hfHoraFechaRecepcion.Value))
            //{
            //    lbl_msj.Text = "FAVOR DE AGREGAR AL MENOS UN REGISTRO DE DOCUMENTO JUNTO CON SU FECHA DE RECEPCIÓN";
            //    mpe_msj.Show();
            //}
            //else
            //{
            //    lblSFTFechaRecep.Text = hfHoraFechaRecepcion.Value;
            //    mpe_SelecFechaTermino.Show();
            //}
        }
        protected void ddlSFTTerminos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarTerminos();
            mpe_SelecFechaTermino.Show();
        }
        protected void rblSFTTipoDias_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarTerminos();
            mpe_SelecFechaTermino.Show();
        }
        protected void cSFTFechaTermino_SelectionChanged(object sender, EventArgs e)
        {
            txtSFTFechaTermino.Text = cSFTFechaTermino.SelectedDate.ToShortDateString();
            mpe_SelecFechaTermino.Show();
        }
        protected void cSFTFechaTermino_DayRender(object sender, DayRenderEventArgs e)
        {
            BDJuridicoEntities ctx = new BDJuridicoEntities();
            catDiasInhabiles periodoInhabil = ctx.catDiasInhabiles.Where(di => e.Day.Date <= di.fechaFin && e.Day.Date >= di.fechaInicio).FirstOrDefault();
            if (periodoInhabil != null || (e.Day.Date.DayOfWeek == DayOfWeek.Saturday || e.Day.Date.DayOfWeek == DayOfWeek.Sunday))
            {
                e.Day.IsSelectable = false;
                e.Cell.BackColor = System.Drawing.Color.DarkBlue;
                e.Cell.ForeColor = System.Drawing.Color.White;
                if (periodoInhabil != null)
                {
                    e.Cell.ToolTip = periodoInhabil.razon.ToUpper();
                }
            }
            if (e.Day.Date == DateTime.Now.Date)
            {
                e.Cell.BackColor = System.Drawing.Color.DarkGray;
                e.Cell.ForeColor = System.Drawing.Color.White;
                e.Cell.ToolTip = "HOY";
            }
        }
        protected void btn_SeleccionarSFT_Click(object sender, EventArgs e)
        {
            txtFechaTermino.Text = txtSFTFechaTermino.Text;
            ddlHora.SelectedValue = ddlSFTHora.SelectedValue;
            ddlMinutos.SelectedValue = ddlSFTMinutos.SelectedValue;
            mpe_SelecFechaTermino.Hide();
        }
        protected void btn_CancelarSFT_Click(object sender, EventArgs e)
        {
            mpe_SelecFechaTermino.Hide();
        }
        protected void ActualizarTerminos()
        {
            //if (ddlSFTTerminos.SelectedValue == "0")
            //{
            //    txtSFTFechaTermino.Text = "";
            //}
            //else
            //{
            //    DateTime fechaInicial = Convert.ToDateTime(hfHoraFechaRecepcion.Value);
            //    AdminTermino adminTermino = new AdminTermino();
            //    int idTermino = Convert.ToInt32(ddlSFTTerminos.SelectedValue);
            //    int tipoDia = Convert.ToInt32(rblSFTTipoDias.SelectedValue);
            //    DateTime fechaTermino = adminTermino.obtenerFecha(fechaInicial, idTermino, tipoDia);
            //    txtSFTFechaTermino.Text = fechaTermino.ToShortDateString();
            //    string hora = fechaTermino.Hour.ToString();
            //    string minuto = fechaTermino.Minute.ToString();
            //    ddlSFTHora.SelectedValue = hora;
            //    ddlSFTMinutos.SelectedValue = minuto;
            //}
        }

        #endregion

        #endregion
        protected void repInformacionTipoAsunto_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("NuevoDatoTipoAsunto"))
            {
                if (ddlTipoAsunto.SelectedValue != "0")
                    ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalDetalleTipoAsunto", "AccionModal('ModalDetalleTipoAsunto', 'show');", true);
            }
            else if (e.CommandName.Equals("EliminarRegistroDato"))
            {
                int idSubTipoAsunto = 0;
                if (int.TryParse(e.CommandArgument.ToString(), out idSubTipoAsunto))
                {
                    if (Session["DetalleTipoAsunto"] != null)
                    {
                        List<InfoDetalleTipoAsunto> ListaInformacion = Session["DetalleTipoAsunto"] as List<InfoDetalleTipoAsunto>;
                        InfoDetalleTipoAsunto detalle = ListaInformacion.Where(l => l.idSubTipoAsunto == idSubTipoAsunto).FirstOrDefault();
                        if (detalle != null)
                        {
                            ListaInformacion.Remove(detalle);
                        }
                        repInformacionTipoAsunto.DataSource = ListaInformacion;
                        repInformacionTipoAsunto.DataBind();
                        Session["DetalleTipoAsunto"] = ListaInformacion;
                    }
                }
            }
        }

        protected void ddlTipoAsunto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfIDTipoAsunto.Value) && Session["DetalleTipoAsunto"] != null)
            {
                //PEDIR CONFIRMACIÓN
                lblMsjConfirmarCambio.Text = " SE CUENTA CON INFORMACIÓN CARGADA DEL DETALLE DEL TIPO DE ASUNTO. AL CAMBIARLO, SE ELIMINARÁ ESA INFORMACIÓN. ¿DESEA CONTINUAR?";
                ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalConfirmarDeleteInfoTipoAsunto", "MostarModalConfirmarDeleteInfoTipoAsunto();", true);

            }
            else
            {
                //PrepararDetalleTipoAsunto();
            }
        }
        public class InfoDetalleTipoAsunto
        {
            public int idSubTipoAsunto { get; set; }
            public string SubTipo { get; set; }
            public DateTime FechaSubTipo { get; set; }
            public string InfoSubTipo { get; set; }
            public List<tbEASTADOpciones> OpcionesLlenadas { get; set; }
            public string Otro { get; set; }
        }
        protected void PrepararDetalleTipoAsunto()
        {
            if (ddlTipoAsunto.SelectedItem.Text.StartsWith("OTRO"))
            {
                divTipoAsuntoOtro.Visible = true;
                repInformacionTipoAsunto.Visible = false;
                hfIDTipoAsunto.Value = "";
            }
            else
            {
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                int idTipoAsunto = int.Parse(ddlTipoAsunto.SelectedValue);
                if (idTipoAsunto != 0 && (ctx.catSubTipoDeAsunto.Where(s => s.idTipoAsunto == idTipoAsunto).Select(s => s.id).Count() > 0))
                {
                    divTipoAsuntoOtro.Visible = false;
                    repInformacionTipoAsunto.Visible = true;
                    repInformacionTipoAsunto.DataSource = new List<InfoDetalleTipoAsunto>();
                    repInformacionTipoAsunto.DataBind();
                    hfIDTipoAsunto.Value = ddlTipoAsunto.SelectedValue;
                    lblTituloDetalleTipoAsunto.Text = $"INFORMACIÓN ADICIONAL DEL TIPO DE ASUNTO '{ddlTipoAsunto.SelectedItem.Text}'";
                    ddlSubTipoAsunto.DataBind();
                    ddlSubTipoAsunto.SelectedValue = "0";
                    divSubTipoAsuntoOtro.Visible = false;
                    txtFechaSubTipo.Text = "";
                    txtOtroSubTipoAsunto.Text = "";
                    //Label lblTextoAddInfoTipoAsunto = repInformacionTipoAsunto.Controls[repInformacionTipoAsunto.Controls.Count - 1].Controls[0].FindControl("lblTextoAddInfoTipoAsunto") as Label;
                    //if (lblTextoAddInfoTipoAsunto != null)
                    //{
                    //    lblTextoAddInfoTipoAsunto.Text = $"AÑADIR INFORMACIÓN DE {ddlTipoAsunto.SelectedItem.Text.ToUpper()}";
                    //}
                }
                else
                {
                    divTipoAsuntoOtro.Visible = false;
                    repInformacionTipoAsunto.Visible = false;
                    hfIDTipoAsunto.Value = "";
                }
            }
        }

        protected void btnConfirmarDeleteITA_Click(object sender, EventArgs e)
        {
            Session["DetalleTipoAsunto"] = null;
            PrepararDetalleTipoAsunto();
            ScriptManager.RegisterStartupScript(this, GetType(), "OcultarModalConfirmarDeleteInfoTipoAsunto", "OcultarModalConfirmarDeleteInfoTipoAsunto();", true);
        }

        protected void ddlSubTipoAsunto_DataBound(object sender, EventArgs e)
        {
            ddlSubTipoAsunto.Items.Add(new ListItem("-SELECCIONE-", "0"));
        }

        protected void btnGuardarDetalleTA_Click(object sender, EventArgs e)
        {

            DateTime f;
            int idSubTipoAsunto = 0;
            if (!int.TryParse(ddlSubTipoAsunto.SelectedValue, out idSubTipoAsunto) || idSubTipoAsunto <= 0)
            {
                lblDTAMsj.Text = "SELECCIONE UN SUBTIPO DE ASUNTO";
            }
            else if (divTipoAsuntoOtro.Visible && string.IsNullOrEmpty(txtOtroSubTipoAsunto.Text))
                lblDTAMsj.Text = "ESPECIFÍQUE EL SUBTIPO DE ASUNTO";
            else if (!DateTime.TryParse(txtFechaSubTipo.Text, out f))
            {
                lblDTAMsj.Text = "INGRESE UNA FECHA VÁLIDA";
            }
            else
            {
                List<InfoDetalleTipoAsunto> ListaInformacion;
                if (Session["DetalleTipoAsunto"] != null)
                    ListaInformacion = Session["DetalleTipoAsunto"] as List<InfoDetalleTipoAsunto>;
                else
                    ListaInformacion = new List<InfoDetalleTipoAsunto>();
                //HACER VALIDACIÓN
                string textoInfo = "";
                bool error = false;
                InfoDetalleTipoAsunto Informacion = new InfoDetalleTipoAsunto();
                Informacion.idSubTipoAsunto = idSubTipoAsunto;
                Informacion.SubTipo = ddlSubTipoAsunto.SelectedItem.Text;
                Informacion.FechaSubTipo = f;
                Informacion.Otro = divTipoAsuntoOtro.Visible ? txtOtroSubTipoAsunto.Text : "";
                Informacion.OpcionesLlenadas = new List<tbEASTADOpciones>();
                foreach (RepeaterItem item in repOpciones.Items)
                {
                    HiddenField hfIDOpcion = item.FindControl("hfIDOpcion") as HiddenField;
                    TextBox txtSubTipoTexto = item.FindControl("txtSubTipoTexto") as TextBox;
                    TextBox txtSubTipoNumero = item.FindControl("txtSubTipoNumero") as TextBox;
                    Label lblSubTipo = item.FindControl("lblSubTipo") as Label;
                    int idOpcion = 0;
                    if (hfIDOpcion != null && txtSubTipoTexto != null && txtSubTipoNumero != null && int.TryParse(hfIDOpcion.Value, out idOpcion) && idOpcion != 0
                        && lblSubTipo != null)
                    {
                        if (string.IsNullOrEmpty(txtSubTipoTexto.Text) && string.IsNullOrEmpty(txtSubTipoNumero.Text))
                        {
                            lblDTAMsj.Text = $"ES NECESARIO LLENAR TODOS LOS DATOS";
                            error = true;
                            return;
                        }
                        else
                        {
                            //REALIZAR EL "GUARDADO"
                            tbEASTADOpciones opcion = new tbEASTADOpciones();
                            opcion.idSTAOpcion = idOpcion;
                            string info = "";
                            if (txtSubTipoNumero.Visible)
                            {
                                opcion.valorNumero = decimal.Parse(txtSubTipoNumero.Text);
                                info = txtSubTipoNumero.Text;
                            }
                            else
                            {
                                opcion.valorTexto = txtSubTipoTexto.Text;
                                info = txtSubTipoTexto.Text;
                            }
                            textoInfo += $"{lblSubTipo.Text} [{info}]; ";
                            Informacion.OpcionesLlenadas.Add(opcion);
                        }
                    }
                }
                if (!error)
                {
                    Informacion.InfoSubTipo = textoInfo;
                    ListaInformacion.Add(Informacion);
                    Session["DetalleTipoAsunto"] = ListaInformacion;
                    repInformacionTipoAsunto.DataSource = ListaInformacion;
                    repInformacionTipoAsunto.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalDetalleTipoAsunto", "AccionModal('ModalDetalleTipoAsunto', 'hide');", true);
                }
            }
        }

        protected void ddlSubTipoAsunto_SelectedIndexChanged(object sender, EventArgs e)
        {
            divSubTipoAsuntoOtro.Visible = ddlSubTipoAsunto.SelectedItem.Text.StartsWith("OTRO");
        }


        public void ProcesarTextoDocumento(string Texto, out string NumDocto, out DateTime? FechaDocto, out string TextoAsunto, out string TipoDocto)
        {
            //Pre-Procesamiento
            Texto = Regex.Replace(Texto, @"\n\s+\n", "\n\n");
            Texto = Regex.Replace(Texto, @"\n(presente|p\sr\se\ss\se\sn\st\se\s)s?\n", "\n\n\nPRESENTE\n", RegexOptions.IgnoreCase);
            //Número y Tipo de documento
            NumDocto = Regex.Match(Texto, @".{0,30}(Tarjeta|Oficio|Documento).*[0-9]{2,}.*", RegexOptions.IgnoreCase).Value;
            TipoDocto = Regex.Match(NumDocto, @"Tarjeta|Oficio|Documento", RegexOptions.IgnoreCase).Value;
            NumDocto = Regex.Replace(NumDocto, @"^(\n+)?.*(Tarjeta|Oficio|Documento)", "", RegexOptions.IgnoreCase);
            NumDocto = Regex.Replace(NumDocto, @"^\s*:?\s*(N(°|o|um|úm)?(\.|:)?)?\s+", "", RegexOptions.IgnoreCase);
            NumDocto = Regex.Replace(NumDocto, @"(^\n*\s*)|(\s*\n*$)", "", RegexOptions.IgnoreCase);
            //Fecha Documento
            string fecha = Regex.Match(Texto, @"[0-9]{1,2}(\s+de\s+|\s?)(enero|febrero|marzo|abril|mayo|junio|julio|agosto|septiembre|octubre|noviembre|diciembre)(\s+de\s+|\s?)20[0-9]{2,}\n", RegexOptions.IgnoreCase).Value;
            fecha = Regex.Replace(fecha, @"(^\n*\s*)|(\s*\n*$)", "", RegexOptions.IgnoreCase);
            string dia = Regex.Match(fecha, @"^([0-9]|o|°)([0-9]|o|°)", RegexOptions.IgnoreCase).Value;
            dia = Regex.Replace(dia, @"(o|°)", "0", RegexOptions.IgnoreCase);
            string mes = Regex.Match(fecha, @"(enero|febrero|marzo|abril|mayo|junio|julio|agosto|septiembre|octubre|noviembre|diciembre)", RegexOptions.IgnoreCase).Value;
            string anio = Regex.Match(fecha, @"[0-9]([0-9]|o|°)[0-9]([0-9]|o|°)?", RegexOptions.IgnoreCase).Value;
            anio = Regex.Replace(anio, @"(o|°)", "0", RegexOptions.IgnoreCase);
            try
            {
                if (anio.Length == 3)
                    anio = DateTime.Now.Year.ToString();
                List<string> meses = new List<string>() { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" };
                FechaDocto = new DateTime(int.Parse(anio.Trim()), meses.IndexOf(mes.ToLower()) + 1, int.Parse(dia.Trim()));
            }
            catch
            {
                FechaDocto = null;
            }
            //Asunto
            TextoAsunto = Regex.Match(Texto, @"((presente|p\sr\se\ss\se\sn\st\se\s)s?\n?)?(\n(Con\s|De\s|Por\s|El\s|En\s|La\s|Los\s|Un\s))(.+\n){2,}\n(.+\n){1,}", RegexOptions.IgnoreCase).Value;
            TextoAsunto = Regex.Replace(TextoAsunto, @"(^\n*\s*)|(\s*\n*$)", "", RegexOptions.IgnoreCase);
            if (Regex.IsMatch(TextoAsunto, @"[a-z]\s?\n\s?[a-z]", RegexOptions.IgnoreCase))
                TextoAsunto = Regex.Replace(TextoAsunto, @"\s?\n\s?", " ");
            TextoAsunto = Regex.Replace(TextoAsunto, @"(\s{2,}|(presente|p\sr\se\ss\se\sn\st\se\s)s?)", " ", RegexOptions.IgnoreCase).Trim();
            if (string.IsNullOrEmpty(TextoAsunto))
            {
                TextoAsunto = Regex.Match(Texto, @"\nPRESE(.+\n|\n){1,}((sin\sm(a|á)s)|atentamente).+", RegexOptions.IgnoreCase).Value;
                TextoAsunto = Regex.Replace(TextoAsunto, @"(^\n*\s*)|(\s*\n*$)", "", RegexOptions.IgnoreCase);
                if (Regex.IsMatch(TextoAsunto, @"[a-z]\s?\n\s?[a-z]", RegexOptions.IgnoreCase))
                    TextoAsunto = Regex.Replace(TextoAsunto, @"\s?\n\s?", " ");
                TextoAsunto = Regex.Replace(TextoAsunto, @"(\s{2,}|(presente|p\sr\se\ss\se\sn\st\se\s)s?)", " ", RegexOptions.IgnoreCase).Trim();
            }
        }
        public string ObtenerTextoDeArchivo(HttpPostedFile archivo)
        {
            string texto = "";
            try
            {
                string nombre = archivo.FileName;
                string tipo = archivo.ContentType;
                if (tipo.Contains("pdf"))
                {
                    texto = ObtenerTextoDePDF(archivo);
                }
                else if (tipo.Contains("image"))
                {
                    Bitmap bitMap = new Bitmap(archivo.InputStream);
                    texto = ObtenerTextoDeImagen(bitMap);
                }
            }
            catch (Exception ex)
            {
            }
            return texto;
        }

        private string ObtenerTextoDeImagen(Bitmap image)
        {
            string texto = "";
            TesseractEngine engine = new TesseractEngine(Server.MapPath("~/tessdata"), "spa", EngineMode.Default);
            using (var tPage = engine.Process(image))
            {
                texto = tPage.GetText();
            }
            return texto;
        }

        private string ObtenerTextoDePDF(HttpPostedFile archivo)
        {
            string txt = "";
            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(archivo.InputStream)))
            {
                //Leer sólo la primera hoja del documento
                LocationTextExtractionStrategy strategyLocation = new LocationTextExtractionStrategy();
                {
                    var page = pdfDocument.GetPage(1);
                    //Intentar con la estrategia simple
                    txt = PdfTextExtractor.GetTextFromPage(page, strategyLocation);
                    if (string.IsNullOrEmpty(txt.Trim()))
                    {
                        //Intentar con una estrategia por tablas
                        SimpleTextExtractionStrategy strategySimple = new SimpleTextExtractionStrategy();
                        PdfCanvasProcessor parser = new PdfCanvasProcessor(strategySimple);
                        parser.ProcessPageContent(page);
                        txt = strategySimple.GetResultantText();
                    }
                    //Intentar por imagen
                    ImageRenderListener strategyPerImage = new ImageRenderListener();
                    PdfCanvasProcessor parserImg = new PdfCanvasProcessor(strategyPerImage);
                    parserImg.ProcessPageContent(page);
                    string textImage = "";
                    foreach (Bitmap image in strategyPerImage.Images)
                    {
                        textImage = ObtenerTextoDeImagen(image);
                    }
                    if (txt.Length < textImage.Length)
                        txt = textImage;
                }
            }
            return txt.Trim();
        }




        protected void btn_OkFin_Click(object sender, EventArgs e)
        {
            Retornar();
        }
        protected void Retornar()
        {
            Session["EnEdicion"] = null;
            if (!string.IsNullOrEmpty(hfIdExped.Value))
                Session["idExpediente"] = hfIdExped.Value;
            if (ViewState["PreviousPageUrl"] != null && !string.IsNullOrEmpty(ViewState["PreviousPageUrl"].ToString()))
                Response.Redirect(ViewState["PreviousPageUrl"].ToString());
            else
                Response.Redirect("~/Expedientes/Consultas.aspx");
        }
        protected void btnEliminarDoctoMemoria_Click(object sender, EventArgs e)
        {
            Session["ArchivoCargado"] = null;
            Session["NombreArchivoCargado"] = null;
            Session["idArchivoPrevio"] = null;

        }

        protected void gvPersonasRelacionadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarRegistroPersona")
            {
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                int expediente = Convert.ToInt32(e.CommandArgument);
                var borrar_persona = ctx.tbExpedPersonas.Where(a => a.id == expediente).FirstOrDefault();
                if (borrar_persona != null)
                {
                    Guid idexpediente_borrar = (Guid)borrar_persona.idExpediente;

                    ctx.SaveChanges();
                    var mensjae = $"<h3>SE BORRO EL REGISTRO</h3>";
                    lblMeNSAJE.Text = mensjae;
                    ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalMensajes", "MostarModalMensajes();", true);
                    ctx.tbExpedPersonas.Remove(borrar_persona);
                    var actuaci = (from a in ctx.tbExpedPersonas
                                   join b in ctx.tbPersonas on a.idPersona equals b.id
                                   join c in ctx.catFiguras on a.idFigura equals c.id
                                   where a.idExpediente == idexpediente_borrar && a.idFigura != 17
                                   select new
                                   {
                                       b.nombreCompleto,
                                       a.idFigura,
                                       a.idPersona,
                                       c.figura,
                                       a.id

                                   }).ToList();

                    gvPersonasRelacionadas.DataSource = actuaci;
                    gvPersonasRelacionadas.DataBind();


                }

            }
        }
    }
}