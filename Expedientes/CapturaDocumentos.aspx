<%@ Page Title="Captura documento" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CapturaDocumentos.aspx.cs" Inherits="Lexus2._0.Expedientes.CapturaDocumentos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/jquery-ui-1.8.24.min.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/jquery.ui.1.9.2.tooltip.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/jquery-ui.css") %>" />
    <link href="<%= ResolveUrl("~/Content/jquery.ui.autocomplete.css") %>" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            // Place here the first init of the autocomplete
            IniciarAutocompletes();
            funcionamientoTab();

        });

        function InitializeRequest(sender, args) {

        }

        function EndRequest(sender, args) {
            IniciarAutocompletes();
            funcionamientoTab();

        }

        window.onbeforeunload = function (e) {
            var e = e || window.event;
            if (e) {
                e.returnValue = alert('Estás por cerrar la página, cuidado!');
            }
        }



        function IniciarAutocompletes() {
            var TextViews = [
                "txtOrgProcedencia", "txtDocPersonaRecibio", "txtPersonaRelacionada"
            ];
            var hf = [
                "hfidOrgProced", "hfidPersRecib", "hfIdPersona"
            ];
            for (i = 0; i < TextViews.length; i++) {
                IniciarAutocomplete(TextViews[i], hf[i]);
            }
        }

        function IniciarAutocomplete(idTextView, idhf) {
            $(document).ready(function () {
                $("[id*='" + idTextView + "']").autocomplete({
                    source: function (request, response) {
                        var datos = {};
                        datos.texto = request.term;
                        datos.idHiddenField = idhf;
                        $.ajax({
                            url: "captura_documento.aspx/BuscarTextos",
                            type: "POST",
                            dataType: "json",
                            data: JSON.stringify(datos),
                            dataFilter: function (data) { return data; },
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(
                                    data.d,
                                    function (item) {
                                        return { label: item.nombre, value: item.nombre, id: item.id, presente: item.presente };
                                    }
                                ))
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                //alert("Alerta:" + textStatus + " : " + errorThrown);
                            }
                        });
                    },
                    select: function (e, i) {
                        $("[id*='" + idhf + "']").val(i.item.id);
                    },
                    minLength: 2
                });

            });
        }

        function funcionamientoTab() {
            var tab = document.getElementById('<%= hfIDTab.ClientID%>');
            if (tab != null) {
                var tab = tab.value + "";
                if (tab == "" || tab == "divSeccion_Datos") {
                    tab = "divSeccion_Datos";
                }
                $("[id$='li" + tab + "']").addClass('active');
                $("[id$='" + tab + "']").addClass('tab-pane active');
                document.getElementById('<%=hfIDTab.ClientID %>').value = tab;
            }
        }

        function cambioTab(idTab) {
            if (document.getElementById('<%=hfIDTab.ClientID %>') != null)
                document.getElementById('<%=hfIDTab.ClientID %>').value = idTab;
        }
        function cambioTabBoton(idTab) {
            console.log('EJECUTANDO - cambioTabBoton');

            if (document.getElementById('<%=hfIDTab.ClientID %>') != null) {
                let idPrevio = document.getElementById('<%=hfIDTab.ClientID %>').value;
                $("[id$='li" + idPrevio + "']").removeClass('active');
                $("[id$='" + idPrevio + "']").removeClass('active');
                document.getElementById('<%=hfIDTab.ClientID %>').value = idTab;
                $("[id$='li" + idTab + "']").addClass('active');
                $("[id$='" + idTab + "']").addClass('tab-pane active');
            }

        }

        function myFunction() {
            var x = document.getElementById("myDIV");
            if (x.style.display === "none") {
                x.style.display = "block";
            } else {
                x.style.display = "none";
            }
        }

        function MostarModalConfirmarDeleteInfoTipoAsunto() {
            $('#ModalConfirmarDeleteInfoTipoAsunto').modal('show');
        }
        function OcultarModalConfirmarDeleteInfoTipoAsunto() {
            $('#ModalConfirmarDeleteInfoTipoAsunto').modal('hide');
        }
        function MostarModalDetalleTipoAsunto() {
            $('#ModalDetalleTipoAsunto').modal('show');
        }
        function OcultarModalDetalleTipoAsunto() {
            $('#ModalDetalleTipoAsunto').modal('hide');
        }
        function AccionModal(idModal, accion) {
            $('#' + idModal).modal(accion);
        }
        function ConfirmarEliminacion() {
            return confirm("ESTÁ POR ELIMINAR EL DOCUMENTO DIGITALIZADO QUE FUE CARGADO EN MEMORIA, ¿DESEA CONTINUAR?");
        }
        function MostarModalFinal() {
            $('#modalAceptar').modal('show');
        }
        function CerrarModalFinal() {
            $('#modalAceptar').modal('hide');
        }

        function MostarModalMensajes() {
            $('#ModalMensaje').modal('show');
        }
        // Esto asegura que la pestaña se recupere tras un PostBack parcial (AJAX)
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                funcionamientoTab();
    });

    </script>
    <section>
        <h2 class="text-center p-1 fw-bold mb-2" style="color: #6D132D !important">RECEPCIÓN DE UN NUEVO DOCUMENTO HACIA LA DDJ</h2>
        <h6 class="text-center p-1">FORMULARIO DE REGISTRO DE UN NUEVO DOCUMENTO. ESTE FORMULARIO SE ENCUENTRA SEGMENTADO EN 2 PARTES:  Y DATOS DEL DOCUMENTO.</h6>
        <h6 class="text-center p-1"><strong>PARA UBICARSE EN CADA SECCIÓN, DE CLIC ENCIMA DE CADA BOTÓN DE SECCIÓN</strong></h6>
    </section>
    <ajaxToolkit:ModalPopupExtender ID="mpe_msj" runat="server" PopupControlID="pnl_msj" OkControlID="btn_Ok" TargetControlID="btn_dummy" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnl_msj" runat="server" CssClass="modalPopupGigante" Style="display: none">
        <asp:UpdatePanel ID="upMsj" runat="server">
            <ContentTemplate>
                <br />
                <asp:Label ID="lbl_msj" runat="server"></asp:Label>
                <hr />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="width: 100%; text-align: center;">
            <asp:Button ID="btn_Ok" runat="server" CssClass="btn btn-primary" Text="ACEPTAR" OnClick="btn_Ok_Click" />
        </div>
    </asp:Panel>
    <asp:Button ID="btn_dummy" runat="server" Style="display: none" />
    <asp:Button ID="btnDummy_SelecFechaTermino" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="mpe_SelecFechaTermino" runat="server" TargetControlID="btnDummy_SelecFechaTermino"
        PopupControlID="pnl_SelecFechaTermino" BackgroundCssClass="modalBackground" DropShadow="true"
        OkControlID="btnOkSFT" CancelControlID="btnNoSFT">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnl_SelecFechaTermino" CssClass="modalPopupSeleccionaTermino" Style="display: none">
        <asp:UpdatePanel ID="updPanelTermino" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div style="text-align: center;">
                    OBTENCIÓN DE LA FECHA DE TÉRMINO
                </div>
                <hr />
                <div class="row">
                    <div class="col-md-7">
                        <div class="row">
                            <div class="col-md-4">
                                FECHA RECEPCIÓN:
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblSFTFechaRecep" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                TÉRMINO:
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlSFTTerminos" runat="server" DataSourceID="edsTerninos" DataTextField="texto" DataValueField="id" CssClass="form-select"
                                    AppendDataBoundItems="true" OnSelectedIndexChanged="ddlSFTTerminos_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="-SELECCIONE-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:EntityDataSource runat="server" ID="edsTerninos" DefaultContainerName="BDJuridicoEntities" ConnectionString="name=BDJuridicoEntities"
                                    EnableFlattening="False" EntitySetName="catTerminos" EntityTypeFilter="catTerminos">
                                </asp:EntityDataSource>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                DÍAS:
                            </div>
                            <div class="col-md-8">
                                <asp:RadioButtonList ID="rblSFTTipoDias" runat="server" RepeatDirection="Vertical" OnSelectedIndexChanged="rblSFTTipoDias_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="NATURALES" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="LABORALES" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                FECHA:
                                 <asp:RequiredFieldValidator ID="rfvSFTFechaTerm" runat="server" ControlToValidate="txtSFTFechaTermino" InitialValue="" Text="*" Font-Size="25px"
                                     ForeColor="Red" Font-Bold="true" ValidationGroup="SFT"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtSFTFechaTermino" runat="server" Width="100px"></asp:TextBox>
                                <br />
                                <asp:DropDownList ID="ddlSFTHora" runat="server" Width="50px"></asp:DropDownList>
                                <asp:Label ID="lblSFTSeparador" runat="server" Text=":" Font-Size="X-Large" Font-Bold="true"></asp:Label>
                                <asp:DropDownList ID="ddlSFTMinutos" runat="server" Width="50px"></asp:DropDownList>
                                <asp:Label ID="lblSFTHrs" runat="server" Text="Hrs." Font-Size="X-Large" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5" style="text-align: center;">
                        <asp:Calendar ID="cSFTFechaTermino" runat="server" OnSelectionChanged="cSFTFechaTermino_SelectionChanged" OnDayRender="cSFTFechaTermino_DayRender"></asp:Calendar>
                    </div>
                </div>
                <hr />
                <div class="row">
                    <div class="col-md-6" style="text-align: center;">
                        <asp:LinkButton ID="btn_SeleccionarSFT" runat="server" CssClass="btn btn-success" OnClick="btn_SeleccionarSFT_Click" CausesValidation="true" ValidationGroup="SFT">
                            <span class="glyphicon glyphicon-floppy-disk"aria-hidden="true" style="font-size: 15px;"></span>
                            SELECCIONAR FECHA
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-6" style="text-align: center;">
                        <asp:LinkButton ID="btn_CancelarSFT" runat="server" CssClass="btn btn-danger" OnClick="btn_CancelarSFT_Click">
                            <span class="glyphicon glyphicon-ban-circle"aria-hidden="true" style="font-size: 15px;"></span>
                            CANCELAR
                        </asp:LinkButton>
                    </div>
                </div>
                <asp:Button ID="btnOkSFT" runat="server" Text="Y" Style="display: none" />
                <asp:Button ID="btnNoSFT" runat="server" Text="N" Style="display: none" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlSFTTerminos" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="rblSFTTipoDias" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cSFTFechaTermino" EventName="SelectionChanged" />
                <asp:AsyncPostBackTrigger ControlID="btn_SeleccionarSFT" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>



    <asp:HiddenField ID="hfIDTab" runat="server" />
    <section>
    <div class="d-flex align-items-start">
        
        <!-- Menú de Pestañas Verticales -->
        <div class="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist" aria-orientation="vertical">
            <button class="nav-link active pb-4 mb-5 p-3 shadow-lg rounded" id="v-pills-home-tab" data-bs-toggle="pill" data-bs-target="#v-pills-home" type="button" role="tab" aria-controls="v-pills-home" aria-selected="true" onclick="cambioTab('v-pills-home')">I. DETALLE DE LA PROCEDENCIA</button>
            <button class="nav-link pb-4 mb-5 p-3 shadow-lg rounded" id="v-pills-profile-tab" data-bs-toggle="pill" data-bs-target="#v-pills-profile" type="button" role="tab" aria-controls="v-pills-profile" aria-selected="false" onclick="cambioTab('v-pills-profile')">II. DATOS DEL DOCUMENTO</button>
        </div>

        <!-- Contenedor Principal de las Vistas -->
        <div class="tab-content card shadow-lg p-3 mb-5 bg-body-tertiary rounded w-100" id="v-pills-tabContent" style="border-radius: 10px; border-left: 8px #007bff solid !important; border-right: none; border-top: none; border-bottom: none">
            
            <!-- ================= PESTAÑA 1: DETALLE DE LA PROCEDENCIA ================= -->
            <div class="tab-pane fade show active" id="v-pills-home" role="tabpanel" aria-labelledby="v-pills-home-tab" tabindex="0">
                <div class="container">
                    <div class="row mb-3">
                        <div class="col">
                            <h4 class="text-center pt-3 fw-bold" style="color: #1F8CC2">I. DETALLE DE LA PROCEDENCIA</h4>
                        </div>
                    </div>
                    
                    <div class="row">
                        <asp:Label ID="Label6" runat="server" Font-Size="0.8em" Text="ÁREA ASIGNADA" AssociatedControlID="ddlAreaAsignada"></asp:Label><span class="campoRequerido">*</span><br />
                        <asp:DropDownList ID="ddlAreaAsignada" runat="server" DataTextField="nombre" OnDataBound="ddlAreaAsignada_DataBound" CssClass="form-select"
                            DataValueField="id" DataSourceID="edsOrgAsignada" Width="82%" OnSelectedIndexChanged="ddlAreaAsignada_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:EntityDataSource runat="server" ID="edsOrgAsignada" DefaultContainerName="BDJuridicoEntities" ConnectionString="name=BDJuridicoEntities" EnableFlattening="False"
                            EntitySetName="tbOrganismos" EntityTypeFilter="tbOrganismos" Where="it.id IN {63, 67, 70, 73, 450, 61, 76, 17126}">
                        </asp:EntityDataSource>
                        <br />
                        <div id="divSubarea" runat="server" visible="false">
                            <asp:Label ID="Label5" runat="server" Font-Size="0.8em" Text="Sub-área" AssociatedControlID="ddlSubAre"></asp:Label>
                            <span class="campoRequerido">*</span><br />
                            <asp:DropDownList ID="ddlSubAre" runat="server" CssClass="form-control"
                                Width="82%" AutoPostBack="true" DataSourceID="edsSubAreas" DataTextField="nombre" DataValueField="id">
                            </asp:DropDownList>
                            <asp:EntityDataSource runat="server" ID="edsSubAreas" DefaultContainerName="BDJuridicoEntities"
                                Where="it.id IN {17127, 17128, 17129, 17130, 17131, 18179}"
                                ConnectionString="name=BDJuridicoEntities" EnableFlattening="False" EntitySetName="tbOrganismos">
                            </asp:EntityDataSource>
                        </div>
                        <br />
                        <div id="divProcedenciaTodos" runat="server">
                            <asp:Label ID="Label11" runat="server" Font-Size="0.8em" Text="PROCEDENCIA" AssociatedControlID="txtOrgProcedencia"></asp:Label><span class="campoRequerido">*</span>
                            <asp:TextBox TabIndex="1" ID="txtOrgProcedencia" runat="server" CssClass="txtPlaceHolder form-control" placeholder="COMIENCE A ESCRIBIR LA PROCEDENCIA..." Style="width: 100%;"
                                autocomplete="off" TextMode="MultiLine" Height="8em"></asp:TextBox>
                            <asp:HiddenField ID="hfidOrgProced" runat="server" />
                        </div>

                        <div id="divProcedenciaDDH" runat="server" visible="false">
                            <asp:Label ID="Label3" runat="server" Font-Size="0.8em" Text="PROCEDENCIA" AssociatedControlID="ddlAreasDDH"></asp:Label><span class="campoRequerido">*</span><br />
                            <asp:DropDownList ID="ddlAreasDDH" runat="server" DataSourceID="edsOrgsProcDDH" DataTextField="nombre" DataValueField="id" CssClass="form-select"
                                Width="82%" AutoPostBack="true" OnSelectedIndexChanged="ddlAreasDDH_SelectedIndexChanged" OnDataBound="ddlAreasDDH_DataBound">
                            </asp:DropDownList>
                            <asp:EntityDataSource runat="server" ID="edsOrgsProcDDH" DefaultContainerName="BDJuridicoEntities" ConnectionString="name=BDJuridicoEntities"
                                EnableFlattening="False" EntitySetName="tbOrganismos" EntityTypeFilter="tbOrganismos" Select="it.[id], it.[nombre]"
                                Where="it.id IN {366,367,5553,9014}" OrderBy="it.nombre">
                            </asp:EntityDataSource>
                        </div>

                        <div id="divTipoAsunto" runat="server">
                            <asp:Label ID="Label1" runat="server" Font-Size="0.8em" Text="TIPO DE ASUNTO" AssociatedControlID="ddlTipoAsunto"></asp:Label><br />
                            <asp:DropDownList ID="ddlTipoAsunto" runat="server" DataTextField="tipo" OnDataBound="ddlTipoAsunto_DataBound" Width="82%" CssClass="form-select"
                                OnSelectedIndexChanged="ddlTipoAsunto_SelectedIndexChanged" AutoPostBack="true" DataSourceID="edsTipoAsunto" DataValueField="id">
                            </asp:DropDownList>
                            <asp:EntityDataSource runat="server" ID="edsTipoAsunto" DefaultContainerName="BDJuridicoEntities" ConnectionString="name=BDJuridicoEntities"
                                EnableFlattening="False" EntitySetName="catTipoDeAsunto" EntityTypeFilter="catTipoDeAsunto" Select="it.[id], it.[tipo], it.[idOrgAreaJuridica]"
                                Where="it.idOrgAreaJuridica == @idAreaJuridica" OrderBy="it.tipo">
                                <WhereParameters>
                                    <asp:ControlParameter ControlID="ddlAreaAsignada" Name="idAreaJuridica" Type="Int32" DefaultValue="0" />
                                </WhereParameters>
                            </asp:EntityDataSource>
                            <hr />
                            <div id="divTipoAsuntoOtro" runat="server">
                                <asp:Label ID="Label4" runat="server" Font-Size="0.8em" Text="ESPECIFIQUE EL TIPO DE ASUNTO:" AssociatedControlID="txtTipoAsuntoOtro"></asp:Label><span class="campoRequerido">*</span>
                                <asp:TextBox ID="txtTipoAsuntoOtro" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:HiddenField ID="hfIDTipoAsunto" runat="server" />
                            <asp:Repeater ID="repInformacionTipoAsunto" runat="server" OnItemCommand="repInformacionTipoAsunto_ItemCommand">
                                <HeaderTemplate>
                                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12 textoCentradoLlamativo">ESPECIFICACIÓN</div>
                                    <div class="col-lg-2 col-md-2 col-sm-2 col-xs-3 textoCentradoLlamativo">FECHA</div>
                                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-8 textoCentradoLlamativo">DETALLE</div>
                                    <div class="col-lg-2 col-md-2 col-sm-2 col-xs-12"></div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="row">
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                            <asp:Label ID="lblSubTipo" runat="server" Text='<%# Eval("SubTipo") %>' />
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-2 col-xs-3">
                                            <asp:Label ID="lblFechaSubTipo" runat="server" Text='<%# Eval("FechaSubTipo", "{0:dd/MM/yyyy}") %>' />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-8">
                                            <asp:Label ID="lblInfoSubTipo" runat="server" Text='<%# Eval("InfoSubTipo") %>' />
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                                            <asp:LinkButton ID="btnBorrarDatoST" runat="server" CssClass="boton boton-primary" ToolTip="ELIMINAR DATO" ClientIDMode="AutoID"
                                                CommandArgument='<%# Eval("idSubTipoAsunto") %>' CommandName="EliminarRegistroDato">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3-fill" viewBox="0 0 16 16"><path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5Zm-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5ZM4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06Zm6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528ZM8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5Z"/></svg>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <br />
                                    <div style="text-align: right; padding-top: 20px;">
                                        <asp:LinkButton ID="btnAgregarInfoTipoAsunto" runat="server" CssClass="boton boton-primary" CommandName="NuevoDatoTipoAsunto">
                                            <span class="glyphicon glyphicon-plus" aria-hidden="true" style="font-size: 15px;"></span> AÑADIR DETALLE DE TIPO DE ASUNTO
                                        </asp:LinkButton>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>

                        <fieldset class="scheduler-border">
                            <legend class="scheduler-border fw-bold" style="color: #6D132D !important">PERSONAS</legend>
                            <div style="padding-bottom: 20px;">Llenado opcional. Comience a escribir el nombre y selecciónelo de la lista desplegable. Después, seleccione su relación con el documento (Figura como).</div>
                            <asp:EntityDataSource runat="server" ID="edsFiguras" DefaultContainerName="BDJuridicoEntities" ConnectionString="name=BDJuridicoEntities"
                                EnableFlattening="False" EntitySetName="catFiguras" EntityTypeFilter="catFiguras" Where="it.actuacion==FALSE AND it.id!=1" OrderBy="it.figura">
                            </asp:EntityDataSource>
                            <asp:Repeater ID="repPersonas" runat="server" OnItemDataBound="repPersonas_ItemDataBound" OnItemCommand="repPersonas_ItemCommand">
                                <HeaderTemplate>
                                    <div class="col-lg-6 col-md-6 col-sm-5 col-xs-6 textoCentradoLlamativo">NOMBRE</div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-5 col-xs-6">
                                            <asp:TextBox ID="txtPersonaRelacionada" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                            <asp:HiddenField ID="hfIdPersona" runat="server" />
                                        </div>
                                        <div class="col-lg-5 col-md-5 col-sm-5 col-xs-6">
                                            <asp:DropDownList ID="ddlFiguraComo" runat="server" DataSourceID="edsFiguras" DataTextField="figura" DataValueField="id" AppendDataBoundItems="true" Width="100%" CssClass="form-select">
                                                <asp:ListItem Text="-SELECCIONE-" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-1 col-md-1 col-sm-2 col-xs-11 textoCentrado">
                                            <asp:LinkButton ID="btnBorrar" runat="server" CssClass="btn btn-danger" ToolTip="QUITAR PERSONA" ClientIDMode="AutoID"
                                                CommandArgument='<%# Container.ItemIndex %>' CommandName="EliminarRegistroPersona">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3-fill" viewBox="0 0 16 16"><path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5Zm-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5ZM4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06Zm6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528ZM8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5Z"/></svg>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <br />
                                    <div style="text-align: right;">
                                        <asp:LinkButton ID="btnAgregarPersona" runat="server" CssClass="btn btn-primary" CommandName="NuevaPersona">
                                            AGREGAR NUEVA FILA DE PERSONA
                                        </asp:LinkButton>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </div>
                </div>
            </div>

            <!-- ================= PESTAÑA 2: DATOS DEL DOCUMENTO ================= -->
            <div class="tab-pane fade" id="v-pills-profile" role="tabpanel" aria-labelledby="v-pills-profile-tab" tabindex="0">
                <div class="container">
                    <asp:HiddenField ID="hfPrevioNumDocto" runat="server" />
                    <asp:HiddenField ID="hfPrevioAsunto" runat="server" />
                    
                    <div class="row mb-3">
                        <div class="col">
                            <h4 class="text-center pt-3 fw-bold" style="color: #1F8CC2">II. DATOS DEL DOCUMENTO</h4>
                        </div>
                    </div>

                    <asp:UpdatePanel ID="upSeccion_Actuacion" runat="server">
                        <ContentTemplate>
                            <div class="panel-body">
                                <asp:Label ID="Label12" runat="server" Font-Size="0.8em" Text="PERSONA QUE RECIBIÓ EL DOCUMENTO" Width="100%" AssociatedControlID="txtDocPersonaRecibio"></asp:Label>
                                <asp:TextBox ID="txtDocPersonaRecibio" runat="server" CssClass="txtPlaceHolder form-control" placeholder=" " Style="width: 100%;" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hfidPersRecib" runat="server" />
                                
                                <hr />
                                <div class="row">
                                    <div class="col-lg-5 col-md-4 col-sm-12 col-xs-12">
                                        <label class="placeHolder">
                                            <span class="textoPlaceHolder">NÚMERO DE DOCUMENTO<span class="campoRequerido">*</span></span>
                                            <asp:TextBox ID="txtNumDocto" runat="server" CssClass="txtPlaceHolder form-control" placeholder=" " Style="width: 20em;" autocomplete="off"></asp:TextBox>
                                        </label>
                                        <label class="placeHolder">
                                            <span class="textoPlaceHolder">FECHA DEL DOCUMENTO<span class="campoRequerido">*</span></span>
                                            <asp:TextBox ID="txtFechaRealizacionDocto" runat="server" CssClass="txtPlaceHolder form-control" placeholder=" " Style="width: 20em;" autocomplete="off"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="ceFechaRealizacion" runat="server" TargetControlID="txtFechaRealizacionDocto" Format="dd/MM/yyyy" />
                                        </label>
                                        <label class="placeHolder">
                                            <span class="textoPlaceHolder">FECHA DE RECEPCIÓN<span class="campoRequerido">*</span></span>
                                            <asp:TextBox ID="txtFechaRecepcionDocto" runat="server" CssClass="txtPlaceHolder form-control" placeholder=" " Style="width: 20em;" autocomplete="off"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaRecepcionDocto" Format="dd/MM/yyyy" />
                                        </label>
                                        <asp:Label ID="Label13" runat="server" Font-Size="0.8em" Text="HORA DE RECEPCIÓN" Width="100%" AssociatedControlID="ddlFechaRecepHora"></asp:Label>
                                        <asp:DropDownList ID="ddlFechaRecepHora" runat="server"></asp:DropDownList>
                                        <asp:Label ID="lblSep" runat="server" Text=":" Font-Size="XX-Large" Font-Bold="true"></asp:Label>
                                        <asp:DropDownList ID="ddlFechaRecepMin" runat="server" Width="50px"></asp:DropDownList>
                                        <asp:Label ID="lblhrs" runat="server" Text="Hrs." Font-Size="X-Large" Font-Bold="true"></asp:Label>
                                        <br /><br />
                                        <asp:Label ID="Label8" runat="server" Font-Size="0.8em" Text="TIPO DE DOCUMENTO" AssociatedControlID="ddlTipoDocumento"></asp:Label><span class="campoRequerido">*</span><br />
                                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="edsTipoDocumento" DataTextField="tipo" CssClass="form-select"
                                            DataValueField="id" AppendDataBoundItems="true" Width="80%">
                                            <asp:ListItem Text="-SELECCIONE-" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:EntityDataSource runat="server" ID="edsTipoDocumento" DefaultContainerName="BDJuridicoEntities"
                                            ConnectionString="name=BDJuridicoEntities" EnableFlattening="False" EntitySetName="catTipoDeDocumento"
                                            EntityTypeFilter="catTipoDeDocumento">
                                        </asp:EntityDataSource>
                                        <br />
                                        <asp:Label ID="Label9" runat="server" Font-Size="0.8em" Text="RECIBIDO COMO" AssociatedControlID="ddlTipoRecep"></asp:Label><span class="campoRequerido">*</span><br />
                                        <asp:DropDownList ID="ddlTipoRecep" runat="server" AppendDataBoundItems="true" DataSourceID="edsTipoRecepcionDocumento"
                                            DataTextField="tipo" DataValueField="id" Width="80%" CssClass="form-select">
                                            <asp:ListItem Text="-SELECCIONE-" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:EntityDataSource runat="server" ID="edsTipoRecepcionDocumento" DefaultContainerName="BDJuridicoEntities" ConnectionString="name=BDJuridicoEntities"
                                            EnableFlattening="False" EntitySetName="catTipoRecepcionDocumento" EntityTypeFilter="catTipoRecepcionDocumento">
                                        </asp:EntityDataSource>
                                    </div>
                                    <div class="col-lg-7 col-md-8 col-sm-12 col-xs-12">
                                        <label class="placeHolder">
                                            <span class="textoPlaceHolder">ASUNTO / RESUMEN<span class="campoRequerido">*</span></span>
                                            <asp:TextBox ID="txtAsuntoDoc" runat="server" CssClass="txtPlaceHolder form-control" placeholder=" RESUMEN" Style="width: 35em; height: 26em;" autocomplete="off" TextMode="MultiLine"></asp:TextBox>
                                            <asp:HiddenField ID="hfAsunto" runat="server" />
                                        </label>
                                    </div>
                                </div>

                                <fieldset class="scheduler-border mt-4">
                                    <legend class="scheduler-border">FECHA DE TÉRMINO</legend>
                                    <div class="row">
                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                            <asp:Label ID="lblTieneTermino" runat="server" Text="¿SE REQUIERE ATENCIÓN PRÓXIMA?:"></asp:Label>
                                            <br />
                                            <asp:RadioButtonList ID="rblRequiereTermino" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblRequiereTermino_SelectedIndexChanged">
                                                <asp:ListItem Text="SÍ" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <br />
                                            <asp:Label ID="lblFechaRecepcionNT" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hfHoraFechaRecepcion" runat="server" />
                                        </div>
                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                                            <div id="divSiTermino" runat="server">
                                                <asp:Label ID="lblFechaTerminoText" runat="server" Text="TÉRMINO: *"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvFechaTermino" runat="server" ControlToValidate="txtFechaTermino" InitialValue="" ValidationGroup="RegistrarExpediente"
                                                    ForeColor="Red" Text="*" Font-Bold="true" Font-Size="25px" ErrorMessage="TÉRMINO"></asp:RequiredFieldValidator>
                                                <br />
                                                <asp:TextBox ID="txtFechaTermino" runat="server" Width="120px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="ceFecTerm" runat="server" TargetControlID="txtFechaTermino" Format="dd/MM/yyyy" />
                                                <asp:DropDownList ID="ddlHora" runat="server" Width="50px"></asp:DropDownList>
                                                <asp:Label ID="lblHora" runat="server" Text=":" Font-Size="XX-Large" Font-Bold="true"></asp:Label>
                                                <asp:DropDownList ID="ddlMinutos" runat="server" Width="50px"></asp:DropDownList>
                                                <asp:Label ID="lblMin" runat="server" Text="Hrs." Font-Size="X-Large" Font-Bold="true"></asp:Label>
                                            </div>
                                            <br />
                                            <div id="divNoTermino" runat="server">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblNoTerm" runat="server" Text="FECHA LÍMITE DE RESPUESTA:"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <asp:Label ID="lblFechaLimiteTerm" runat="server"></asp:Label>
                                                        <asp:HiddenField ID="hfFechaLimiteTerm" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>

        </div>
    </div>

    <!-- SECCIÓN DE ACCIONES (REGISTRAR / CANCELAR) -->
    <hr style="margin-top: 5em;" />
    <section>
        <asp:UpdatePanel ID="upFinalizacion" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-6 col-xs-12" style="text-align: center;">
                        <asp:LinkButton ID="btnCrearyContinuar" runat="server" CssClass="btn btn-primary" Style="font-size: 14px;" OnClick="btnCrearyContinuar_Click">
                            <span class="glyphicon glyphicon-floppy-save" aria-hidden="true" style="font-size: 15px;"></span>&nbsp;REGISTRAR DOCUMENTO Y CONTINUAR
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-6 col-xs-12" style="text-align: center;">
                        <asp:LinkButton ID="btnCancelarCreacion" runat="server" Style="font-size: 14px;" CssClass="btn btn-danger" OnClick="btnCancelarCreacion_Click">
                            <span class="glyphicon glyphicon-ban-circle" aria-hidden="true" style="font-size: 15px;"></span>&nbsp;CANCELAR REGISTRO Y SALIR
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-lg-4" style="text-align: center;"></div>
                    <div class="col-lg-4 col-md-6 col-sm-12" style="text-align: center;">
                        <asp:ValidationSummary ID="vsRegistrarExpediente" runat="server" ValidationGroup="RegistrarExpediente" HeaderText="FAVOR DE LLENAR LOS SIGUIENTES CAMPOS"
                            Font-Bold="true" ForeColor="DarkRed" />
                    </div>
                    <div class="col-lg-4" style="text-align: center;"></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </section>
</section>




    <!-- Modal -->
    <div class="modal fade" id="ModalConfirmarDeleteInfoTipoAsunto" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modalPantallaMediana">
            <div class="modal-content modalContenido">
                <asp:UpdatePanel ID="upConfirmarITA" runat="server">
                    <ContentTemplate>
                        <div style="width: 100%; text-align: center;">
                            <asp:Label ID="lblVTitulo" runat="server" Font-Size="Large" Font-Bold="true" Text="CONFIRMAR CAMBIO"></asp:Label>
                        </div>
                        <hr />
                        <div>
                            <asp:Label ID="lblMsjConfirmarCambio" runat="server"></asp:Label>
                        </div>
                        <div style="padding: 10px; width: 100%; display: flex;">
                            <asp:LinkButton ID="btnConfirmarDeleteITA" runat="server" CssClass="boton boton-primary" ToolTip="CAMBIAR TIPO DE ASUNTO" ClientIDMode="AutoID"
                                Style="width: 100%;" OnClick="btnConfirmarDeleteITA_Click">
                                <span class="glyphicon glyphicon-transfer" aria-hidden="true" style="font-size:15px;"></span>&nbsp;CAMBIAR TIPO DE ASUNTO
                            </asp:LinkButton>
                            <button type="button" class="boton boton-gray" data-dismiss="modal" style="width: 100%;">
                                <span class="glyphicon glyphicon-remove-circle" aria-hidden="true" style="font-size: 15px;"></span>CONSERVAR INFORMACIÓN
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->







    <!-- Modal -->
    <div class="modal fade" id="ModalDetalleTipoAsunto" tabindex="-1" role="dialog"
        aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modalPantallaMediana">
            <div class="modal-content modalContenido">
                <asp:UpdatePanel ID="upDTA" runat="server">
                    <ContentTemplate>
                        <div style="width: 100%; text-align: center;">
                            <asp:Label ID="lblTituloDetalleTipoAsunto" runat="server" Font-Size="Large" Font-Bold="true"></asp:Label>
                        </div>
                        <hr />
                        <div>
                            <asp:Label ID="Label2" runat="server" Font-Size="0.8em" Text="SUBTIPO DE ASUNTO" AssociatedControlID="ddlSubTipoAsunto"></asp:Label><span class="campoRequerido">*</span><br />
                            <asp:DropDownList ID="ddlSubTipoAsunto" runat="server" DataSourceID="edsSubTipoAsunto" DataTextField="subTipo" DataValueField="id" Width="100%"
                                AutoPostBack="true" OnDataBound="ddlSubTipoAsunto_DataBound" OnSelectedIndexChanged="ddlSubTipoAsunto_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:EntityDataSource runat="server" ID="edsSubTipoAsunto" DefaultContainerName="BDJuridicoEntities" ConnectionString="name=BDJuridicoEntities" EnableFlattening="False"
                                EntitySetName="catSubTipoDeAsunto" EntityTypeFilter="catSubTipoDeAsunto" Select="it.[id], it.[idTipoAsunto], it.[subTipo]"
                                Where="it.idTipoAsunto == @idTipoAsunto">
                                <WhereParameters>
                                    <asp:ControlParameter ControlID="hfIDTipoAsunto" Name="idTipoAsunto" Type="Int32" DefaultValue="0" />
                                </WhereParameters>
                            </asp:EntityDataSource>
                            <br />
                            <br />
                            <div id="divSubTipoAsuntoOtro" runat="server">
                                <label class="placeHolder">
                                    <asp:TextBox ID="txtOtroSubTipoAsunto" runat="server" CssClass="txtPlaceHolder" placeholder=" " Style="width: 100%" autocomplete="off"></asp:TextBox>
                                    <span class="textoPlaceHolder">ESPECIFIQUE EL SUBTIPO DE ASUNTO<span class="campoRequerido">*</span></span>
                                </label>
                            </div>
                            <br />
                            <label class="placeHolder">
                                <asp:TextBox ID="txtFechaSubTipo" runat="server" CssClass="txtPlaceHolder" placeholder=" " Style="width: 20em;" autocomplete="off"></asp:TextBox>
                                <span class="textoPlaceHolder">FECHA DEL EVENTO<span class="campoRequerido">*</span></span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaSubTipo" Format="dd/MM/yyyy" />
                            </label>
                            <br />
                            <br />
                            <asp:Repeater ID="repOpciones" runat="server" DataSourceID="edsOpcionesSubTipo">
                                <ItemTemplate>
                                    <div class="row">
                                        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-122">
                                            <asp:Label ID="lblSubTipo" runat="server" Text='<%# Eval("texto") %>' />
                                            <asp:HiddenField ID="hfIDOpcion" runat="server" Value='<%# Eval("id") %>' />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                            <asp:TextBox ID="txtSubTipoTexto" runat="server" Visible='<%# !Boolean.Parse(Eval("esNumero").ToString()) %>' />
                                            <asp:TextBox ID="txtSubTipoNumero" runat="server" Visible='<%# Boolean.Parse(Eval("esNumero").ToString()) %>' TextMode="Number" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:EntityDataSource runat="server" ID="edsOpcionesSubTipo" DefaultContainerName="BDJuridicoEntities" ConnectionString="name=BDJuridicoEntities"
                                EnableFlattening="False" EntitySetName="catSTAOpciones" EntityTypeFilter="catSTAOpciones"
                                Where="it.idSubTipo == @idSubTipo">
                                <WhereParameters>
                                    <asp:ControlParameter ControlID="ddlSubTipoAsunto" Name="idSubTipo" DefaultValue="0" Type="Int32" />
                                </WhereParameters>
                            </asp:EntityDataSource>
                        </div>
                        <div style="padding: 10px; width: 100%; display: flex;">
                            <asp:LinkButton ID="btnGuardarDetalleTA" runat="server" CssClass="boton boton-primary" ToolTip="CAMBIAR TIPO DE ASUNTO" ClientIDMode="AutoID"
                                Style="width: 100%;" OnClick="btnGuardarDetalleTA_Click">
                                <span class="glyphicon glyphicon-save" aria-hidden="true" style="font-size:15px;"></span>&nbsp;AÑADIR INFORMACIÓN
                            </asp:LinkButton>
                            <button type="button" class="boton boton-gray" data-dismiss="modal" style="width: 100%;">
                                <span class="glyphicon glyphicon-remove-circle" aria-hidden="true" style="font-size: 15px;"></span>CANCELAR Y CERRAR
                            </button>
                        </div>
                        <div style="padding-top: 5px;">
                            <asp:Label ID="lblDTAMsj" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->



    <div class="modal" id="modalAceptar" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-content">

                        <div class="modal-body">
                            <br />
                            <asp:Label ID="mensajeaceptar" runat="server"></asp:Label>
                            <hr />
                        </div>

                        <div class="modal-footer">
                            <asp:LinkButton runat="server" ID="btnlnkaceptar" OnClick="btn_Ok_Click" CssClass="btn btn-success">Aceptar</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->

    <div class="modal" tabindex="-1" id="ModalMensaje">
        <div class="modal-dialog">
            <asp:UpdatePanel runat="server" ID="b">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server" id="lblTitulo" style="text-align: center !important">Modal title</h5>
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="lblMeNSAJE" runat="server"></asp:Label>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Aceptar</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
</asp:Content>