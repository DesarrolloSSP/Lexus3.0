<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorDashboard.aspx.cs" Inherits="Lexus2._0.SysAdmin.ErrorDashboard" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Errores</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css"
      rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js"></script>    
</head>
    <body>
        <form id="formPrincipal" runat="server"> 
          <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <div class="row mb-4"> 
                <div class="col-md-3"> 
                    <h2 class="fw-bold"> Bitácora de errores </h2> 
                    <p class="text-muted"> Consulta y seguimiento de excepciones registradas.  </p>                   
                </div> 
            </div>
            <div class="row mb-4">
                <div class="col-md-4 col-sm-6">         
                    <label for="<%= txtFechaB %>" class="form-label fw-bold">
                        FECHA<span class="text-danger">*</span>
                    </label>        
                    <div class="input-group">
                        <asp:TextBox ID="txtFechaB" runat="server" 
                            CssClass="form-control" 
                            placeholder="dd/MM/yyyy" 
                            autocomplete="off">
                        </asp:TextBox>            
                        <ajaxToolkit:CalendarExtender ID="ceFecha" runat="server" 
                            TargetControlID="txtFechaB" 
                            Format="dd/MM/yyyy" />                
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                            CssClass="btn btn-primary" 
                            OnClick="btnBuscar_Click" />
                    </div>
                </div>
            </div>
            <div class="card-body"> 
                <div class="table-responsive">
                    <asp:GridView ID="gvErrores"
                                  runat="server"
                                  AutoGenerateColumns="False"
                                  DataKeyNames="ErrorLogId" GridLines="None" CssClass="table"> 
                        <Columns>
                            <asp:BoundField
                                DataField="Fecha"
                                HeaderText="Fecha"/>
                            <asp:BoundField
                                DataField="Usuario"
                                HeaderText="Usuario"/>
                            <asp:BoundField
                                DataField="ErrorType"
                                HeaderText="Tipo Error">
                                <ItemStyle Width="180px"/>
                            </asp:BoundField>
                            <asp:BoundField
                                DataField="Pagina"
                                HeaderText="Página"/>
                            <asp:BoundField
                                DataField="Mensaje"
                                HeaderText="Mensaje">
                                <ItemStyle Width="600px"/>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Detalle">
                                <ItemTemplate>
                                    <asp:LinkButton
                                        ID="lnkDetalle"
                                        runat="server"
                                        CommandArgument='<%# Eval("ErrorLogId") %>'
                                        OnClick="lnkDetalle_Click"
                                        Text="🔍 Ver detalle" 
                                        CssClass="btn btn-outline-primary btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                   </asp:GridView>
                </div> 
            </div>
            <div class="modal fade" id="modalDetalle" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-xl modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header"
                             style="background-color:#611232;color:white;">
                            <h5 class="modal-title">
                                Detalle del Error
                            </h5>
                            <button type="button"
                                    class="btn-close btn-close-white"
                                    data-bs-dismiss="modal">
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row mb-3">
                                <div class="col-md-6">
                                    <strong>Fecha:</strong><br />
                                    <asp:Label ID="lblFecha" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <strong>Usuario:</strong><br />
                                    <asp:Label ID="lblUsuario" runat="server" />
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-md-6">
                                    <strong>Página:</strong><br />
                                    <asp:Label ID="lblPagina" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <strong>Tipo Error:</strong><br />
                                    <asp:Label ID="lblTipoError" runat="server" />
                                </div>
                            </div>
                            <hr />
                            <h6>
                                Stack Trace
                            </h6>
                            <asp:TextBox
                                ID="txtStackTrace"
                                runat="server"
                                TextMode="MultiLine"
                                Rows="18"
                                Width="100%"
                                ReadOnly="true"
                                CssClass="form-control font-monospace" />
                        </div>
                        <div class="modal-footer">
                            <button type="button"
                                    class="btn btn-secondary"
                                    data-bs-dismiss="modal">
                                Cerrar
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </body>
</html>

