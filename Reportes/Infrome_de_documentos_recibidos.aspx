<%@ Page Title="Informe de documentos recibido" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Infrome_de_documentos_recibidos.aspx.cs" Inherits="Lexus2._0.Reportes.Infrome_de_documentos_recibidos" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/jquery-ui-1.8.24.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        function MostarModalMensajes() {
            $('#ModalMensaje').modal('show');
        }

        function spinnerver() {
            let spinner = document.getElementById("spinner");
            spinner.style.visibility = 'visible'; //'hidden'
        }


        function spinnerocultar() {
            let spinner = document.getElementById("spinner");
            spinner.style.visibility = 'hidden'; //'hidden'
        }
    </script>
    <div class="container bg-light">
        <div class="shadow p-3 mb-5 bg-body rounded  ">
            <h2 style="text-align: center; font-size: 2.2EM; color: #851717;">INFORME GENERAL DE RESULTADOS</h2>
            <br />
            <div class="d-grid gap-2 d-md-flex justify-content-md-center" id="spinner" style="visibility: hidden">
                <div class="spinner-border" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            </div>
            <div class="row">
                <div class=" col-md-5">
                    <div class="input-group input-group-sm" id="Fecha1">
                        <asp:TextBox ID="txtFechaini" runat="server" CssClass=" form-control" placeholder="YYYY/MM/DD" autocomplete="off"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaini" Format="yyyy/MM/dd"></ajaxToolkit:CalendarExtender>
                        <span class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </span>
                    </div>
                </div>
                <div class="col-md-5 ">
                    <div class="input-group input-group-sm" id="Fecha2">
                        <asp:TextBox ID="txtDateFin" runat="server" CssClass=" form-control" ReadOnly="false" placeholder="YYYY/MM/DD" autocomplete="off"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFin" Format="yyyy/MM/dd"></ajaxToolkit:CalendarExtender>
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
                <div class=" col-md-2 ">
                    <div class="btn btn-primary">
                        <i class="fas fa-search"></i>
                        <asp:Button runat="server" ID="btnBuscar" CssClass="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </div>
            </div>
            <br />
        </div>
    </div>
    <br />
    <div id="ReportePdf" runat="server" visible="false">
        <div class="container ">
            <div class="card shadow-lg p-3 mb-5 bg-body-tertiary rounded">

            <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Remote" Height="900px" Width="100%">
            </rsweb:ReportViewer>
                </div>
        </div>
    </div>
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
