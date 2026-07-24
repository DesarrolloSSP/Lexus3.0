<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio_sesion.aspx.cs" Inherits="Lexus2._0.Inicio.Inicio_sesion" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Inicio de sesión</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" />
     <link href="../Content/source/css/GeneralSSP.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center align-items-center min-vh-100">
                <div class="col-12 col-md-6 col-lg-5 card-login">
                    <div class="shadow p-3 mb-5 bg-body-tertiary rounded fadeInDown">
                        <div class="row">

                            <div class="col-md-6 d-flex justify-content-center align-items-center">
                                <img src="../Assents/LEXUS.png" class="img-fluid" style="width: 15dvw !important" />
                            </div>

                            <div class="col-md-6 container text-center">
                                <div class="row">
                                    <h4 class="text-titulo">INICIAR SESIÓN</h4>
                                    <asp:Login ID="LgPrincipal" runat="server" OnLoggedIn="LgPrincipal_LoggedIn" OnAuthenticate="LgPrincipal_Authenticate">
                                        <LayoutTemplate>
                                            <div class="input-group flex-nowrap p-2">
                                                <span class="input-group-text" id="addon-wrapping"><i class="fas fa-user icono-c"></i></span>
                                                <asp:TextBox runat="server" ID="UserName" class="form-control etiquetas" placeholder="Usuario" required autocomplete="off"> </asp:TextBox>
                                            </div>

                                            <div class="input-group flex-nowrap p-2">
                                                <span class="input-group-text" id="addon-wrapping2"><i class="fas fa-lock icono-c"></i></span>
                                                <asp:TextBox runat="server" ID="Password" class="form-control etiquetas" placeholder="Contraseña" TextMode="Password" required autocomplete="off"> </asp:TextBox>
                                            </div>

                                            <asp:Button ID="LoginButton" CommandName="Login" runat="server" class=" btn col-11 mx-auto btn-principal" ValidationGroup="Login1" Text="Ingresar" />
                                            <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="red" Visible="True"></asp:Label>
                                        </LayoutTemplate>
                                    </asp:Login>
                                </div>
                            </div>


                        </div>
                    </div>
                    <div class="text-center fadeInDown mt-3">
                        <img class="avatar" src="~/Content/source/img/SSP_Convivencia.png" runat="server" style="width: 40%" />
                    </div>
                </div>
            </div>
        </div>
        <footer class="footer card pt-2">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-sm-12 col-md-12 footer-texto">
                        <p class="align-self-center text-center">Se prohíbe la reproducción total o parcial contenida en este sistema informático sin el consentimiento expreso y por escrito de la Secretaría de Seguridad Pública del Estado de Veracruz. Esta plataforma digital deberá ser utilizada únicamente por el personal autorizado. El acceso no autorizado a sistemas informáticos es un delito grave. </p>
                        <p class="align-self-center text-center">
                            &copy; <%: DateTime.Now.Year %> - GOBIERNO DEL ESTADO DE VERACRUZ / SECRETARÍA DE SEGURIDAD PÚBLICA/ DERECHOS RESERVADOS.
                        </p>
                        <p class="align-self-center text-center">
                            VERACRUZ.GOB.MX
                        </p>
                    </div>
                </div>
            </div>
        </footer>
    </form>
</body>
</html>
