<%@ Page Title="Bandeja" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consultas.aspx.cs" Inherits="Lexus2._0.Expedientes.Consultas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center p-1 fw-bold mb-2" style="color: #6D132D !important">BÚSQUEDA DE DOCUMENTOS</h2>
    <asp:UpdatePanel runat="server" ID="data">
        <ContentTemplate>
            <section class="shadow p-1 mb-4 bg-body-tertiary rounded">
                <section class="text-center ">
                    <asp:Label ID="Label4" runat="server" Text="Tipo de busqueda:" CssClass="fs-4 pt-2 pb-2 fw-bold"></asp:Label>
                    <div class="row g-4">
                        <div class=" d-flex justify-content-center ">
                            <asp:RadioButtonList ID="rblTipoFecha" runat="server" RepeatDirection="Horizontal" CssClass="col-md-4 ">
                                <asp:ListItem Value="1" Text="DOCUMENTO" />
                                <asp:ListItem Value="2" Text="RECIBIDO EL" />
                                <asp:ListItem Value="3" Text="REGISTRADO EL" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row g-4 pb-4">
                        <div class=" d-flex justify-content-center ">
                            <div class="col-lg-2">
                                <asp:TextBox ID="txtFechaInicial" runat="server" CssClass="form-control" TextMode="date"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 " style="display: flex;">
                                <b>A: </b>
                                <asp:TextBox ID="txtFechaFinal" runat="server" TextMode="date" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-2 pb-5">
                        <div class=" d-flex justify-content-center ">
                            <div class="col-md-8">
                                <div class="form-floating">
                                    <asp:TextBox ID="txtAsuntoDoc" runat="server" Height="7em" CssClass="form-control" AutoPostBack="true" TabIndex="1" TextMode="MultiLine" placeholder="NÚMERO DE DOCUMENTO / TEXTO EN EL ASUNTO:"></asp:TextBox>
                                    <label for="floatingInputGrid">NÚMERO DE DOCUMENTO / TEXTO EN EL ASUNTO:</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row g-2 pb-5">
                        <div class=" d-flex justify-content-center ">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="text-align: center;">
                                <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" Style="font-size: 14px;" OnClick="btnBuscar_Click">
                                    <span class="glyphicon glyphicon-search" aria-hidden="true" style="font-size: 15px;"></span>
                                    &nbsp;BUSCAR
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnLimpiarFiltros" runat="server" CssClass="btn btn-secundario" Style="font-size: 14px;" OnClick="btnLimpiarFiltros_Click">
                                    <span class="glyphicon glyphicon-repeat"aria-hidden="true" style="font-size: 15px;"></span>
                                    &nbsp;LIMPIAR
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </section>
            </section>

            <section class="shadow p-3 mb-4 bg-body-tertiary rounded table-responsive ">
                <div class="d-flex justify-content-end pb-3">
                    <div class="rounded" style="background-color: #cdb22a !important; border-block: #cdb22a !important;">
                        <h3 claa="p-3" style="color: white"><strong class="label label-success">Archivos: <strong runat="server" id="totalregis"></strong></strong></h3>

                    </div>
                </div>
                <asp:GridView runat="server" ID="gvDocumentos" CssClass="table table-bordered bs-table" DataKeyNames="id" AutoGenerateColumns="false"
                    AllowSorting="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvDocumentos_PageIndexChanging"
                    OnRowCommand="gvDocumentos_RowCommand" OnRowDataBound="gvDocumentos_RowDataBound">
                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="header_table">
                            <ItemTemplate>
                                <%# (gvDocumentos.PageSize * gvDocumentos.PageIndex) + (Container.DisplayIndex + 1) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NÚMERO" SortExpression="numeroDocumento" ControlStyle-Font-Size="12px" ItemStyle-CssClass="textoJustificado" HeaderStyle-CssClass="center header_table">
                            <ItemTemplate>
                                <asp:Label ID="lblnumgv" runat="server" name="texto" Text='<%# Eval("numeroDocumento") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="nombreOrgAreaJuridica" HeaderText="ASIGNADO A" SortExpression="nombreOrgAreaJuridica  " HeaderStyle-CssClass="center header_table"></asp:BoundField>
                        <asp:TemplateField HeaderText="FECHAS" HeaderStyle-CssClass="center header_table">
                            <ItemTemplate>
                                <div style="font-size: .8em;"><b><%# Eval("C_regActivo") != null && Boolean.Parse(Eval("C_regActivo").ToString()) != false ? "TÉRMINO: " + Eval("fechaTermino", "{0:g}") :  "FINALIZADO" %> </b></div>
                                <br />
                                <ul style="padding-left: 15px; font-size: .8em;">
                                    <li><b>DOCUMENTO:</b> <%# Eval("fechaDocumento") != null ? Eval("fechaDocumento", "{0:dd/MM/yyyy}") : "" %></li>
                                    <li><b>RECIBIDO EL:</b> <%# Eval("fechaRecepcion", "{0:g}") %></li>
                                    <li><b>REGISTRADO EL:</b> <%# Eval("C_fechaReg", "{0:g}") %></li>
                                </ul>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="nombreOrgOrigen" HeaderText="ORIGEN" SortExpression="nombreOrgOrigen" HeaderStyle-CssClass="center header_table" ItemStyle-CssClass="claseOrigen"></asp:BoundField>
                        <asp:BoundField DataField="nombreOrgDestino" HeaderText="DESTINO" SortExpression="nombreOrgDestino" HeaderStyle-CssClass="center header_table" ItemStyle-CssClass="claseDestino"></asp:BoundField>
                        <asp:TemplateField HeaderText="ASUNTO" SortExpression="asunto" ControlStyle-Font-Size="12px" ItemStyle-CssClass="textoJustificado" HeaderStyle-CssClass="center header_table">
                            <ItemTemplate>
                                <asp:Label ID="lblAsuntogv" runat="server" name="texto" Text='<%# Eval("asunto") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TIPO DE DOCUMENTO" HeaderStyle-CssClass="header_table">
                            <ItemTemplate>
                                <%# Eval("tipo") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="numero" HeaderText="NÚM. EXPEDIENTE" SortExpression="numero" HeaderStyle-CssClass="center header_table"></asp:BoundField>
                        <asp:BoundField DataField="folioExterno" HeaderText="FOLIO" SortExpression="folioExterno" HeaderStyle-CssClass="center header_table" ItemStyle-CssClass="claseOrigen"></asp:BoundField>

                        <asp:TemplateField HeaderText="ACCIONES" HeaderStyle-CssClass="center header_table" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <div class="row">
                                    <div class="d-grid gap-2 d-md-flex justify-content-md-center">
                                        <asp:LinkButton ID="btnVerDocumento" runat="server" CssClass="btn btn-primary" ToolTip="VER DOCUMENTO" AlternateText="VER DOCUMENTO"
                                            CommandArgument='<%# Eval("id") %>' CommandName="VerDetalleDocumento" Style="padding: 3px 9px;">
                                               <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-file-earmark-ruled" viewBox="0 0 16 16">
  <path d="M14 14V4.5L9.5 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2zM9.5 3A1.5 1.5 0 0 0 11 4.5h2V9H3V2a1 1 0 0 1 1-1h5.5v2zM3 12v-2h2v2H3zm0 1h2v2H4a1 1 0 0 1-1-1v-1zm3 2v-2h7v1a1 1 0 0 1-1 1H6zm7-3H6v-2h7v2z"/>
</svg>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnEditarDocumento" runat="server" CssClass="btn btn-primary" ToolTip="EDITAR DOCUMENTO" AlternateText="EDITAR DOCUMENTO"
                                            CommandArgument='<%# Eval("id") %>' CommandName="EditarDocumento" Style="padding: 3px 9px;">
                                               <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
  <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
  <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z"/>
</svg>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-primary" ToolTip="ELIMINAR DOCUMENTO" AlternateText="ELIMINAR DOCUMENTO" Visible="false"
                                            CommandArgument='<%# Eval("id") %>' CommandName="ELIMINARDocumento" Style="padding: 3px 9px;">
                                              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">
  <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>
</svg>

                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </section>





            <!-- Modal -->
            <div class="modal fade" id="ModalVerDetalleDocumento" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog ">
                    <div class="modal-content">

                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hfIDDoctoVisualizacion" runat="server" />
                                    <div style="width: 100%; text-align: center;">
                                        <asp:Label ID="lblVTitulo" runat="server" Font-Size="Large" Font-Bold="true"></asp:Label>
                                    </div>
                                    <hr style="margin: 5px;" />
                                    <div style="display: flex;">
                                        <div style="min-width: 40%">
                                            <b>DETALLE:</b><br />
                                            <asp:Label ID="lblVDetalle" runat="server"></asp:Label>
                                            <br />
                                            <b>ASUNTO</b><br />
                                            <asp:TextBox ID="txtVAsunto" runat="server" Width="100%" TextMode="MultiLine" Style="min-height: 15em;" Enabled="false" CssClass="form-control"></asp:TextBox>
                                            <br />
                                        </div>
                                        <div style="min-width: 60%; padding: 5px;" id="divVerDocumento" runat="server">
                                            <asp:Literal ID="litPDF" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div style="padding: 10px; width: 100%; display: flex;">
                                        <%--<button type="button" class="btn btn-danger" data-dismiss="modal" style="min-width: 40%;">
                                            <span class="glyphicon glyphicon-remove-circle" aria-hidden="true" style="font-size: 15px;"></span>CERRAR VENTANA
                                        </button>--%>
                                        <asp:LinkButton ID="btnDescargar" runat="server" CssClass="boton boton-primary" ToolTip="DESCARGAR ARCHIVO"
                                            Style="width: 100%;">
                                <span class="glyphicon glyphicon-cloud-download" aria-hidden="true" style="font-size:15px;"></span>&nbsp;DESCARGAR DOCUMENTO
                                        </asp:LinkButton>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnDescargar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Salir</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal" tabindex="-1" id="modalEliminar">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title text-center p-1 fw-bold mb-2" style="color: #6D132D !important">Alerta</h5>
                </div>
                <div class="modal-body">
                    <p>Estas seguro que quires eliminar el documento</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button runat="server" ID="btnAcptEliminar" CssClass="btn btn-primary" Text="Aceptar" OnClick="btnAcptEliminar_Click" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function EliminarModalDocumento() {
            //$('#ModalVerDetalleDocumento').modal('show');
            const myModal = new bootstrap.Modal(document.getElementById('modalEliminar'));
            myModal.show();
        };
        function MostarModalVerDetalleDocumento() {
            //$('#ModalVerDetalleDocumento').modal('show');
            const myModal = new bootstrap.Modal(document.getElementById('ModalVerDetalleDocumento'));
            myModal.show();
        };


        function InicializarRemarcarTexto() {
            //Encontrado y adaptado a partir de http://jsfiddle.net/PELkt/ - Gracias
            $(document).ready(function () {

                search = $("[id*='txtAsuntoDoc']").val();
                if (search) {
                    $('[id*="gvDocumentos"]').each(function () {
                        var regex = new RegExp(search, 'gi');
                        $(this).html($(this).html().replace(regex, "<span style='font-weight: bold;font-size: 1.4em; color:red;'>" + search + "</span>"));
                    });
                }
                var f1 = $("[id*='txtFechaInicial']").val();
                var f2 = $("[id*='txtFechaFinal']").val();
                if (f1 || f2) {
                    let txtF = $("[id*='rblTipoFecha'] input:checked").closest("td").find("label").html();
                    if (txtF)
                        $('[id*="gvDocumentos"]').each(function () {
                            txtF = "<b>" + txtF + ":</b>";
                            var regex = new RegExp(txtF, 'gi');
                            $(this).html($(this).html().replace(regex, "<span style='font-weight: bold;font-size: 1.1em; color:red;'>" + txtF + "</span>"));
                        });
                }

            });
        }


        function ShowProgress() {
            $('#modalcargando').modal('show');
        };

        function HideProgress() {
            $('#modalcargando').modal('hide');
        };


        function guardado() {
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Se elimino el archivo',
                showConfirmButton: false,
                timer: 2500
            })
        }

    </script>
</asp:Content>
