using Lexus2._0.Datos;
using Lexus2_0.Datos.Auxiliares;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;


namespace Lexus2._0.Expedientes
{
    public partial class CapturaDocumentos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PrepararNuevoDocumento();
                divSubarea.Visible = false;
            }
          //  throw new HttpException(403, "Prueba de acceso denegado forzada");
        }

        #region SECCIÓN DEFAULT ==================================================
        protected void PrepararNuevoDocumento()
        {
            DefaultSeccionDetalleProcedencia();
            DefaultSeccionDocumentoInicial();
        }
        protected void DefaultSeccionDetalleProcedencia()
        {
            txtOrgProcedencia.Text = "";
            hfidOrgProced.Value = "";

            divTipoAsuntoOtro.Visible = false;
            AdminPersonas adminPers = new AdminPersonas();
            int jerarq = 0;
            int idAreaPersona = adminPers.idAreaOrganizacional(Roles.GetRolesForUser(User.Identity.Name)[0], out jerarq);
            bool activo = idAreaPersona == 0;
            ddlAreaAsignada.DataBind();
            ddlAreaAsignada.ClearSelection();
            ddlAreaAsignada.SelectedValue = idAreaPersona.ToString();

            divTipoAsunto.Visible = !activo;
            ddlTipoAsunto.DataBind();
            ddlTipoAsunto.SelectedValue = "0";
            repInformacionTipoAsunto.Visible = false;
            hfIDTipoAsunto.Value = "";
            /**/
            int idDDH = (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_DERECHOS_HUMANOS;
            divProcedenciaDDH.Visible = idAreaPersona == idDDH;
            divProcedenciaTodos.Visible = idAreaPersona != idDDH && idAreaPersona != 0;
            ddlAreasDDH.ClearSelection();
            DefaultPersonas();
            DefaultFechaHora();
        }
        protected void DefaultPersonas()
        {
            List<tbExpedActPersonas> listaPersonasRelacionadas = new List<tbExpedActPersonas>();
            listaPersonasRelacionadas.Add(new tbExpedActPersonas()
            {
                idPersona = 0,
                idFigura = (int)AdminPersonas.FIGURAS_CLAVE.REMITENTE
            });
            listaPersonasRelacionadas.Add(new tbExpedActPersonas()
            {
                idPersona = 0,
                idFigura = (int)AdminPersonas.FIGURAS_CLAVE.DESTINATARIO
            });
            repPersonas.DataSource = listaPersonasRelacionadas;
            repPersonas.DataBind();
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
        protected void DefaultSeccionDocumentoInicial()
        {
            int idPersona = -1;
            string persona = AdminPersonas.NombrePersonaUsuario(User.Identity.Name, out idPersona);
            if (!string.IsNullOrEmpty(persona))
            {
                txtDocPersonaRecibio.Text = persona;
                hfidPersRecib.Value = idPersona.ToString();
                txtDocPersonaRecibio.Enabled = false;
            }
            else
            {
                txtDocPersonaRecibio.Text = "";
                hfidPersRecib.Value = "";
            }
            txtNumDocto.Text = "";
            txtFechaRealizacionDocto.Text = "";
            txtFechaRecepcionDocto.Text = "";
            txtAsuntoDoc.Text = "";
            ddlTipoDocumento.ClearSelection();
            ddlTipoRecep.ClearSelection();
            // divArchivoSubido.Visible = false;
            //Término
            rblRequiereTermino.ClearSelection();
            divSiTermino.Visible = false;
            divNoTermino.Visible = false;
            lblFechaRecepcionNT.Text = "";
            txtFechaTermino.Text = "";
            hfFechaLimiteTerm.Value = "";
            hfHoraFechaRecepcion.Value = "";
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

        #endregion
        #region SECCIÓN - DETALLE PROCEDENCIA ===========================================
        protected void ddlTipoAsunto_DataBound(object sender, EventArgs e)
        {
            ddlTipoAsunto.Items.Add(new ListItem("-SELECCIONE-", "0"));
        }
        protected void repPersonas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            TextBox txtPersonaRelacionada = e.Item.FindControl("txtPersonaRelacionada") as TextBox;
            HiddenField hfIdPersona = e.Item.FindControl("hfIdPersona") as HiddenField;
            DropDownList ddlFiguraComo = e.Item.FindControl("ddlFiguraComo") as DropDownList;
            tbExpedActPersonas reg = EntityDataSourceExtensions.GetItemObject<tbExpedActPersonas>(e.Item.DataItem);
            if (ddlFiguraComo != null && reg != null && hfIdPersona != null && txtPersonaRelacionada != null)
            {
                ddlFiguraComo.SelectedValue = reg.idFigura.ToString();
                if (reg.tbPersonas != null)
                {
                    txtPersonaRelacionada.Text = reg.tbPersonas.nombre;
                    hfIdPersona.Value = reg.tbPersonas.id + "";
                }
            }
        }

        protected void repPersonas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("EliminarRegistroPersona"))
            {
                e.Item.Visible = false;
            }
            else if (e.CommandName.Equals("NuevaPersona"))
            {
                List<tbExpedActPersonas> listaPersonasRelacionadas = new List<tbExpedActPersonas>();
                foreach (RepeaterItem item in repPersonas.Items)
                {
                    if (item.Visible)
                    {
                        TextBox txtPersonaRelacionada = item.FindControl("txtPersonaRelacionada") as TextBox;
                        HiddenField hfIdPersona = item.FindControl("hfIdPersona") as HiddenField;
                        DropDownList ddlFiguraComo = item.FindControl("ddlFiguraComo") as DropDownList;
                        if (txtPersonaRelacionada != null && hfIdPersona != null && ddlFiguraComo != null)
                        {
                            int id = 0;
                            listaPersonasRelacionadas.Add(new tbExpedActPersonas()
                            {
                                tbPersonas = new tbPersonas() { nombre = txtPersonaRelacionada.Text, id = int.TryParse(hfIdPersona.Value, out id) ? id : 0 },
                                idFigura = int.TryParse(ddlFiguraComo.SelectedValue, out id) ? id : 0
                            });
                        }
                    }
                }
                listaPersonasRelacionadas.Add(new tbExpedActPersonas()
                {
                    idPersona = 0,
                    idFigura = 0
                });
                repPersonas.DataSource = listaPersonasRelacionadas;
                repPersonas.DataBind();
            }
        }
        protected List<tbExpedActPersonas> GetListaPersonasRelacionadas()
        {
            List<tbExpedActPersonas> listaPersonasRelacionadas = new List<tbExpedActPersonas>();
            foreach (RepeaterItem item in repPersonas.Items)
            {
                if (item.Visible)
                {
                    TextBox txtPersonaRelacionada = item.FindControl("txtPersonaRelacionada") as TextBox;
                    HiddenField hfIdPersona = item.FindControl("hfIdPersona") as HiddenField;
                    DropDownList ddlFiguraComo = item.FindControl("ddlFiguraComo") as DropDownList;
                    if (txtPersonaRelacionada != null && hfIdPersona != null && ddlFiguraComo != null && !string.IsNullOrEmpty(txtPersonaRelacionada.Text) && !string.IsNullOrWhiteSpace(txtPersonaRelacionada.Text))
                    {
                        int id = 0;
                        listaPersonasRelacionadas.Add(new tbExpedActPersonas()
                        {
                            tbPersonas = new tbPersonas() { nombreCompleto = Regex.Replace(txtPersonaRelacionada.Text.Trim().ToUpper(), @"\s{2,}", @"\s", RegexOptions.IgnoreCase), id = int.TryParse(hfIdPersona.Value, out id) ? id : 0 },
                            idFigura = int.TryParse(ddlFiguraComo.SelectedValue, out id) ? id : 0
                        });
                    }
                }
            }
            return listaPersonasRelacionadas;
        }

        protected List<tbExpedPersonas> GuardarPersonasRelacionadas()
        {
            List<tbExpedPersonas> listaPersonasRelacionadas = new List<tbExpedPersonas>();
            foreach (RepeaterItem item in repPersonas.Items)
            {
                if (item.Visible)
                {
                    TextBox txtPersonaRelacionada = item.FindControl("txtPersonaRelacionada") as TextBox;
                    HiddenField hfIdPersona = item.FindControl("hfIdPersona") as HiddenField;
                    DropDownList ddlFiguraComo = item.FindControl("ddlFiguraComo") as DropDownList;
                    if (txtPersonaRelacionada != null && hfIdPersona != null && ddlFiguraComo != null && !string.IsNullOrEmpty(txtPersonaRelacionada.Text) && !string.IsNullOrWhiteSpace(txtPersonaRelacionada.Text))
                    {
                        int id = 0;
                        listaPersonasRelacionadas.Add(new tbExpedPersonas()
                        {
                            tbPersonas = new tbPersonas() { nombreCompleto = txtPersonaRelacionada.Text.Trim().ToUpper(), id = int.TryParse(hfIdPersona.Value, out id) ? id : 0 },
                            idFigura = int.TryParse(ddlFiguraComo.SelectedValue, out id) ? id : 0
                        });
                    }
                }
            }
            return listaPersonasRelacionadas;
        }
        #endregion
        #region SECCIÓN - DATOS DEL DOCUMENTO =====================================
        protected void btnCrearyContinuar_Click(object sender, EventArgs e)
        {
            BDJuridicoEntities ctx = new BDJuridicoEntities();
            string textoValidaciones = "";
            bool Puede = true;
            int idAreaAsignada = 0;
            int idOrgProcedencia = 0;
            int.TryParse(hfidOrgProced.Value, out idOrgProcedencia);
            DateTime fechaTermAct = new DateTime();

            if (!int.TryParse(ddlAreaAsignada.SelectedValue, out idAreaAsignada) || idAreaAsignada <= 0)
                textoValidaciones += "<li>ÁREA ASIGNADA</li>";

            if (divProcedenciaTodos.Visible && idOrgProcedencia <= 0 && (string.IsNullOrEmpty(txtOrgProcedencia.Text) || string.IsNullOrEmpty(txtOrgProcedencia.Text.Trim())))
                textoValidaciones += "<li>PROCEDENCIA DEL DOCUMENTO</li>";
            else
            {
                int.TryParse(ddlAreasDDH.SelectedValue, out idOrgProcedencia);
                if (divProcedenciaDDH.Visible && idOrgProcedencia <= 0)
                {
                    textoValidaciones += "<li>PROCEDENCIA DEL DOCUMENTO</li>";
                }
            }
            int idTipoAsunto = 0;
            if (!int.TryParse(ddlTipoAsunto.SelectedValue, out idTipoAsunto) || idTipoAsunto <= 0)
            {
                textoValidaciones += "<li>TIPO DE ASUNTO</li>";
            }
            if (divTipoAsuntoOtro.Visible && string.IsNullOrEmpty(txtTipoAsuntoOtro.Text))
                textoValidaciones += "<li>ESPECIFÍQUE EL TIPO DE ASUNTO (OTRO)</li>";
            if (string.IsNullOrEmpty(txtDocPersonaRecibio.Text))// || string.IsNullOrEmpty(hfidPersRecib.Value))
                textoValidaciones += "<li>PERSONA QUE RECIBIÓ</li>";
            if (string.IsNullOrEmpty(txtNumDocto.Text))
                textoValidaciones += "<li>NÚMERO DE DOCUMENTO</li>";
            DateTime f1, f2;
            if (!string.IsNullOrEmpty(txtFechaRealizacionDocto.Text) && !DateTime.TryParse(txtFechaRealizacionDocto.Text, out f1))
                textoValidaciones += "<li>FECHA VÁLIDA DEL DOCUMENTO</li>";
            if (string.IsNullOrEmpty(txtFechaRecepcionDocto.Text) || !DateTime.TryParse(txtFechaRecepcionDocto.Text, out f2))
                textoValidaciones += "<li>FECHA VÁLIDA DE RECEPCIÓN DEL DOCUMENTO</li>";
            if (string.IsNullOrEmpty(txtAsuntoDoc.Text))
                textoValidaciones += "<li>EL ASUNTO DEL DOCUMENTO</li>";
            if (ddlTipoDocumento.SelectedValue == "0")
                textoValidaciones += "<li>TIPO DE DOCUMENTO</li>";
            if (ddlTipoRecep.SelectedValue == "0")
                textoValidaciones += "<li>RECIBIDO COMO</li>";
            if (rblRequiereTermino.SelectedValue == "1")
                if (DateTime.TryParse(txtFechaTermino.Text, out fechaTermAct))
                    fechaTermAct = new DateTime(fechaTermAct.Year, fechaTermAct.Month, fechaTermAct.Day, int.Parse(ddlHora.SelectedValue), int.Parse(ddlMinutos.SelectedValue), 0);
                else
                    textoValidaciones += "<li>FAVOR DE INGRESAR UNA FECHA DE TÉRMINO VÁLIDA. EN CASO DE NO REQUERIRLA, SELECCIONE LA OPCIÓN <b>'NO'</b> EN ¿SE REQUIERE ATENCIÓN PRÓXIMA?</li>";
            if (!string.IsNullOrEmpty(txtFechaRecepcionDocto.Text) && DateTime.TryParse(txtFechaRecepcionDocto.Text, out f2))
            {
                fechaTermAct = new DateTime(f2.Year, f2.Month, f2.Day, int.Parse(ddlFechaRecepHora.SelectedValue), int.Parse(ddlFechaRecepMin.SelectedValue), 0);
                fechaTermAct = fechaTermAct.AddDays(40);
            }
            else
            {
                textoValidaciones += "<li>FAVOR DE INGRESAR LA FECHA Y HORA DE RECEPCIÓN DEL DOCUMENTO PARA PODER CONSIDERAR LA FECHA DE TÉRMINO</li>";
            }
            if (!string.IsNullOrEmpty(hfHoraFechaRecepcion.Value) && fechaTermAct < Convert.ToDateTime(hfHoraFechaRecepcion.Value))
                textoValidaciones += "<li>ATENCIÓN: LA FECHA DE TÉRMINO DEBE SER MAYOR O IGUAL A LA FECHA DE RECEPCIÓN</li>";
            List<tbExpedActPersonas> listaPersonas = GetListaPersonasRelacionadas();
            int totalPersonasAnomalas = listaPersonas.Where(p => (!p.idFigura.HasValue || p.idFigura <= 0) && p.tbPersonas != null).Count();
            if (listaPersonas.Count > 0 && totalPersonasAnomalas > 0)
            {
                textoValidaciones += $"<li>SE ENCONTRARON {totalPersonasAnomalas} REGISTROS DE PERSONAS SIN DECLARACIÓN DE 'FIGURA COMO'</li>";
            }

            List<tbExpedPersonas> listaPersonasExp = GuardarPersonasRelacionadas();
            int totalPersonasAnomala = listaPersonasExp.Where(p => (!p.idFigura.HasValue || p.idFigura <= 0) && p.tbPersonas != null).Count();
            if (listaPersonas.Count > 0 && totalPersonasAnomala > 0)
            {
                textoValidaciones += $"<li>SE ENCONTRARON {totalPersonasAnomala} REGISTROS DE PERSONAS SIN DECLARACIÓN DE 'FIGURA COMO'</li>";
            }
            /* REGISTRO para el guardado*/
            if (string.IsNullOrEmpty(textoValidaciones))
            {
                tbExpedientes nuevoExpe = new tbExpedientes();
                tbExpedActuaciones nuevaActuacion = new tbExpedActuaciones(); //YA EDIT
                tbExpedPersonas nuevapersona = new tbExpedPersonas();
                tbExpedActPersonas nuevaPersonaAct = new tbExpedActPersonas();
                tbExpedActDocumentos nuevoDocumento = new tbExpedActDocumentos();
                try
                {
                   

                    // Generamos el expediente
                    #region crear expediente
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

                    #endregion

                    //generar NUEVA ACTUACIÓN
                    #region crear expediente actuacion
                    nuevaActuacion.idExpediente = idExpedienteNuevo;
                    nuevaActuacion.idActuacion = Convert.ToInt32(AdminActuacion.ACTUACIONES.RECEPCION);
                    nuevaActuacion.idOrganismoDestino = Convert.ToInt32(ddlAreaAsignada.SelectedValue);
                    nuevaActuacion.accion = txtAsuntoDoc.Text.ToUpper().Trim();
                    nuevaActuacion.fechaTermino = fechaTermAct;
                    nuevoExpe.C_fechaReg = DateTime.Now;
                    nuevaActuacion.C_regActivo = true;
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
                    nuevaActuacion.idOrganismoOrigen = idOrgProcedencia;
                    nuevaActuacion.idAreaJuridica = Convert.ToInt32(ddlAreaAsignada.SelectedValue);
                    if (ddlAreaAsignada.SelectedValue == "63")
                    {
                        nuevaActuacion.IdSubAreaJuridica = Convert.ToInt32(ddlSubAre.SelectedValue);
                    }
                    nuevaActuacion.idTipoAsunto = idTipoAsunto;
                    if (divTipoAsuntoOtro.Visible)
                    {
                        nuevaActuacion.tbExpedActExtraAsunto = new tbExpedActExtraAsunto();
                        nuevaActuacion.tbExpedActExtraAsunto.extra = txtTipoAsuntoOtro.Text;
                        nuevaActuacion.tbExpedActExtraAsunto.C_fechaReg = DateTime.Now;
                    }
                    if (Session["DetalleTipoAsunto"] != null)
                    {
                        List<InfoDetalleTipoAsunto> ListaInformacion = Session["DetalleTipoAsunto"] as List<InfoDetalleTipoAsunto>;
                        foreach (InfoDetalleTipoAsunto detalle in ListaInformacion)
                        {
                            tbExpedActSTAsuntoDetalle AD = new tbExpedActSTAsuntoDetalle();
                            AD.idSubTipo = detalle.idSubTipoAsunto;
                            AD.fecha = detalle.FechaSubTipo;
                            if (!string.IsNullOrEmpty(detalle.Otro))
                                AD.opcionOtro = detalle.Otro;
                            AD.C_fechaReg = DateTime.Now;
                            foreach (tbEASTADOpciones opcion in detalle.OpcionesLlenadas)
                            {
                                opcion.C_fechaReg = DateTime.Now;
                                AD.tbEASTADOpciones.Add(opcion);
                            }
                            nuevaActuacion.tbExpedActSTAsuntoDetalle.Add(AD);
                        }
                    }
                    ctx.tbExpedActuaciones.Add(nuevaActuacion);
                    ctx.SaveChanges();
                    #endregion

                    string textoNombrePersonaRecibe = txtDocPersonaRecibio.Text;
                    if (!string.IsNullOrEmpty(hfidPersRecib.Value) && !string.IsNullOrEmpty(textoNombrePersonaRecibe))
                    {
                        //El nombre de la persona fue seleccionado de la lista desplegable
                        //tbExpedActPersonas nuevaRelEAP = new tbExpedActPersonas();
                        nuevaPersonaAct.idExpedActuacion = nuevaActuacion.id;
                        nuevaPersonaAct.idFigura = (int)AdminPersonas.FIGURAS_CLAVE.PERSONA_RECIBE;
                        nuevaPersonaAct.idPersona = Convert.ToInt32(hfidPersRecib.Value);
                        nuevaPersonaAct.C_fechaReg = DateTime.Now;
                        ctx.tbExpedActPersonas.Add(nuevaPersonaAct);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        //El nombre de la persona no fue seleccionado, consultar si existe algún nombre semejante
                        if (!string.IsNullOrEmpty(textoNombrePersonaRecibe) && !string.IsNullOrEmpty(textoNombrePersonaRecibe = textoNombrePersonaRecibe.Trim()))
                        {
                            tbPersonas persona = ctx.tbPersonas.Where(p => p.nombreCompleto.Equals(textoNombrePersonaRecibe)).FirstOrDefault();
                            if (persona == null)
                            {
                                persona = new tbPersonas();
                                persona.nombreCompleto = textoNombrePersonaRecibe;
                                persona.C_fechaReg = DateTime.Now;
                                ctx.tbPersonas.Add(persona);
                                ctx.SaveChanges();
                            }
                            tbExpedActPersonas nuevaRelEAP = new tbExpedActPersonas();
                            nuevaRelEAP.idExpedActuacion = nuevaActuacion.id;
                            nuevaRelEAP.idFigura = (int)AdminPersonas.FIGURAS_CLAVE.PERSONA_RECIBE;
                            nuevaRelEAP.idPersona = persona.id;
                            nuevaRelEAP.C_fechaReg = DateTime.Now;
                            ctx.tbExpedActPersonas.Add(nuevaRelEAP);
                            ctx.SaveChanges();
                        }
                    }

                    //guardar personas expedientes
                    foreach (tbExpedPersonas relPer in listaPersonasExp)
                    {
                        int idPersona = relPer.tbPersonas.id;
                        if (idPersona == 0)
                        {
                            //Buscar la persona por los nombres
                            tbPersonas personaBD = ctx.tbPersonas.Where(p => p.nombreCompleto.Equals(relPer.tbPersonas.nombreCompleto)).FirstOrDefault();
                            if (personaBD == null)
                            {
                                //Registrar nueva persona
                                personaBD = new tbPersonas();
                                personaBD.nombreCompleto = relPer.tbPersonas.nombreCompleto;
                                personaBD.C_fechaReg = DateTime.Now;
                                BDJuridicoEntities ctxPer = new BDJuridicoEntities();
                                ctxPer.tbPersonas.Add(personaBD);
                                ctxPer.SaveChanges();
                            }
                            idPersona = personaBD.id;
                        }

                        nuevapersona.idPersona = idPersona;
                        nuevapersona.idExpediente = idExpedienteNuevo;
                        nuevapersona.idFigura = relPer.idFigura;
                        nuevapersona.C_fechaReg= DateTime.Now;
                        ctx.tbExpedPersonas.Add(nuevapersona);
                        ctx.SaveChanges();
                       
                    }


                    #region crear documento

                    nuevoDocumento.numeroDocumento = txtNumDocto.Text.Trim();
                    string datoPrevioND = nuevoDocumento.numeroDocumento;
                    nuevoDocumento.idExpedActuaciones = nuevaActuacion.id;
                    if (txtFechaRealizacionDocto.Text != "")
                        nuevoDocumento.fechaDocumento = Convert.ToDateTime(txtFechaRealizacionDocto.Text);
                    else
                        nuevoDocumento.fechaDocumento = null;
                    if (txtFechaRecepcionDocto.Text != "")
                    {
                        int idFRHora = int.Parse(ddlFechaRecepHora.SelectedValue);
                        int idFRMin = int.Parse(ddlFechaRecepMin.SelectedValue);
                        DateTime FechaRecep = Convert.ToDateTime(txtFechaRecepcionDocto.Text);
                        nuevoDocumento.fechaRecepcion = new DateTime(FechaRecep.Year, FechaRecep.Month, FechaRecep.Day, idFRHora, idFRMin, 0);
                    }

                    else
                    nuevoDocumento.fechaRecepcion = null;
                    nuevoDocumento.asunto = txtAsuntoDoc.Text.ToUpper().Trim();
                    string datoPrevioAsunto = nuevoDocumento.asunto;
                    hfAsunto.Value = txtAsuntoDoc.Text.ToUpper().Trim();
                    int idTipoDocumento = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
                    if (idTipoDocumento != 0)
                        nuevoDocumento.idTipoDeDocumento = idTipoDocumento;
                    int idTipoRecepDoc = Convert.ToInt32(ddlTipoRecep.SelectedValue);
                    if (idTipoRecepDoc != 0)
                        nuevoDocumento.idTipoRecepcionDocumento = idTipoRecepDoc;

                    nuevoDocumento.C_fechaReg = DateTime.Now;
                    ctx.tbExpedActDocumentos.Add(nuevoDocumento);
                  
                    ctx.SaveChanges();

                    string Siglas = "";
                    if (ddlAreaAsignada.SelectedValue == "61")
                    {
                        Siglas = "DGJ";
                    }
                    if (ddlAreaAsignada.SelectedValue == "63")
                    {
                        Siglas = "CDP";
                    }
                    if (ddlAreaAsignada.SelectedValue == "67")
                    {
                        Siglas = "DH";
                    }
                    if (ddlAreaAsignada.SelectedValue == "70")
                    {
                        Siglas = "CN";
                    }
                    if (ddlAreaAsignada.SelectedValue == "73")
                    {
                        Siglas = "AFP";
                    }
                    if (ddlAreaAsignada.SelectedValue == "76")
                    {
                        Siglas = "DJ";
                    }
                    if (ddlAreaAsignada.SelectedValue == "450")
                    {
                        Siglas = "EA";
                    }
                    if (ddlAreaAsignada.SelectedValue == "17126")
                    {
                        Siglas = "EPJ";
                    }

                    #endregion
                    var folio = ctx.SP_FOLIO_DOCUMENTO(nuevoDocumento.id, Siglas).First();

                    AdminSeguimiento adminSeg = new AdminSeguimiento();
                    adminSeg.guardarSegimientoCreacion(AdminSeguimiento.AccionRealizada.CREACION_DOCUMENTO, "tbExpedActuaciones", nuevaActuacion.id.ToString(), "", DateTime.Now, User.Identity.Name);

                    string txt = "<br /><br /><span style='font-weight:bold;'>SE HA CREADO EL DOCUMENTO. PARA FACILITAR LA SIGUIENTE CAPTURA, <b style='color:red;'> SE HAN CONSERVADO LOS DATOS DEL DOCUMENTO PREVIO.</b> SI REQUIERE UN FORMULARIO NUEVO, DE CLIC EN LA OPCIÓN DEL MENÚ 'NUEVO DOCUMENTO' EN LA PARTE SUPERIOR</span>";

                    var mensaje = $"<div style='width:100%; text-align:center; font-weight:bold;'>¡DOCUMENTO REGISTRADO CON ÉXITO!</div><hr />FOLIO INTERNOS: {folio} <b>, AHORA PUEDE CONSULTAR EL DOCUMENTO POR SU NÚMERO INGRESADO: <b>{txtNumDocto.Text}</b> EN EL BUSCADOR DE DOCUMENTOS. {txt}";
                    mensajeaceptar.Text = mensaje;
                    ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalFinal", "MostarModalFinal();", true);

                    ctx.SaveChanges();
                }


                catch (Exception ex)
                {
                    ctx.tbExpedientes.Remove(nuevoExpe);
                    ctx.tbExpedActuaciones.Remove(nuevaActuacion);
                    ctx.tbExpedActDocumentos.Remove(nuevoDocumento);

                    foreach (tbExpedActPersonas per in nuevaActuacion.tbExpedActPersonas.ToList())
                    {
                        ctx.tbExpedActPersonas.Remove(per);
                    }
                   
                    foreach (tbExpedActDocumentos doc in nuevaActuacion.tbExpedActDocumentos.ToList())
                    {
                        if (doc.tbArchivos != null)
                        {
                            ctx.tbArchivos.Remove(doc.tbArchivos);
                        }
                        //ctx.tbExpedActDocumentos.Remove(doc);
                    }



                    var _persona = ctx.tbExpedPersonas.Where(t => t.idExpediente == nuevapersona.idExpediente).ToList();
                    if(_persona != null)
                    {
                        foreach (tbExpedPersonas persona in _persona)
                        {
                            ctx.tbExpedPersonas.Remove(persona);
                        }
                    }
                    ctx.SaveChanges();
                    //OCURRIÓ UN PROBLEMA AL REGISTRAR LA ACTUACIÓN DEL DOCUMENTO, POR LO QUE NO SE HA GUARDADO NADA
                    var mensjae = $"<h3>PROBLEMA AL COMENZAR EL REGISTRO</h3><p>OCURRIÓ EL SIGUIENTE PROBLEMA [X001]: {ex.Message} | {(ex.InnerException != null ? ex.InnerException.Message : "")} | {(ex.InnerException != null && ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "")}</p>";
                    //mpe_msj.Show();
                    ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalMensajes", "MostarModalMensajes();", true);
                    lblTitulo.InnerText = "OCURRIÓ UN PROBLEMA";
                    lblMeNSAJE.Text = mensjae;
                }
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalMensajes", "MostarModalMensajes();", true);
                lblTitulo.InnerText = "FAVOR DE LLENAR LO SIGUIENTE";
                lblMeNSAJE.Text = textoValidaciones;

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
            Response.Redirect("~/Default.aspx");
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
            if (string.IsNullOrEmpty(hfHoraFechaRecepcion.Value))
            {
                lbl_msj.Text = "FAVOR DE AGREGAR AL MENOS UN REGISTRO DE DOCUMENTO JUNTO CON SU FECHA DE RECEPCIÓN";
                mpe_msj.Show();
            }
            else
            {
                lblSFTFechaRecep.Text = hfHoraFechaRecepcion.Value;
                mpe_SelecFechaTermino.Show();
            }
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
            if (ddlSFTTerminos.SelectedValue == "0")
            {
                txtSFTFechaTermino.Text = "";
            }
            else
            {
                DateTime fechaInicial = Convert.ToDateTime(hfHoraFechaRecepcion.Value);
                AdminTermino adminTermino = new AdminTermino();
                int idTermino = Convert.ToInt32(ddlSFTTerminos.SelectedValue);
                int tipoDia = Convert.ToInt32(rblSFTTipoDias.SelectedValue);
                DateTime fechaTermino = adminTermino.obtenerFecha(fechaInicial, idTermino, tipoDia);
                txtSFTFechaTermino.Text = fechaTermino.ToShortDateString();
                string hora = fechaTermino.Hour.ToString();
                string minuto = fechaTermino.Minute.ToString();
                ddlSFTHora.SelectedValue = hora;
                ddlSFTMinutos.SelectedValue = minuto;
            }
        }

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
                lblMsjConfirmarCambio.Text = "SE CUENTA CON INFORMACIÓN CARGADA DEL DETALLE DEL TIPO DE ASUNTO. AL CAMBIARLO, SE ELIMINARÁ ESA INFORMACIÓN. ¿DESEA CONTINUAR?";
                ScriptManager.RegisterStartupScript(this, GetType(), "MostarModalConfirmarDeleteInfoTipoAsunto", "MostarModalConfirmarDeleteInfoTipoAsunto();", true);

            }
            else
            {
                PrepararDetalleTipoAsunto();
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


        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/Expedientes/Consultas.aspx");
            txtNumDocto.Text = "";
            ScriptManager.RegisterStartupScript(this, GetType(), "CerrarModalFinal", "CerrarModalFinal();", true);
        }
        

        #endregion


    }
}