<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Lexus2._0.Inicio.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link runat="server" href="~/Content/login.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" />
    <title>Inicio de sesión</title>
    <%--<script src="<%= ResolveUrl("~/Scripts/sweetalert2.all.js") %>" type="text/javascript"></script>--%>

    <script type="text/javascript">
        function error() {
            swal({
                title: "Error!",
                text: "El usuario y la contraseña no son correctos",
                icon: "error",
                button: "Aceptar",

            });
            return false
        }
    </script>
    <style>
        #logo {
            margin-left: 10px;
        }

        #logo2 {
            margin-left: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager runat="server" ID="v"></asp:ScriptManager>

        <div class="container">
            <div class="row">
                <div class="wrapper fadeInDown">

                    <asp:Login ID="Login" runat="server" OnLoggedIn="Login_LoggedIn" OnAuthenticate="Login_Authenticate">
                        <LayoutTemplate>
                            <div id="formContent">
                                <br />
                                <!-- Tabs Titles -->
                                <!-- Icon -->
                                <div class="fadeIn first">
                                    <img src="~/Assents/LEXUS.png" style="display: block; width: 50%; margin: 10px auto;" runat="server"/>

                                </div>
                                <br />


                                <!-- Login Form -->
                                <asp:TextBox runat="server" ID="UserName" class="fadeIn second" placeholder=" Usuario" required autocomplete="off"> </asp:TextBox>
                                <br />
                                <asp:TextBox runat="server" ID="Password" CssClass="fadeIn second" placeholder=" Contraseña" TextMode="Password" required autocomplete="off"></asp:TextBox>
                                <br />
                                <asp:Button ID="LoginButton" CommandName="Login" runat="server" class="login100-form-btn btn-success pt-4" ValidationGroup="Login1" Text="Inicio de sesi&#243;n" />

                                <!-- Remind Passowrd -->

                                <div id="formFooter">
                                    <img class="avatar" src="~/Assents/Identidad_2018.png" runat="server" style="width: 100%" />
                                </div>
                            </div>
                        </LayoutTemplate>
                    </asp:Login>
                    <div style="width: 100%; padding-top: 60px; text-align: center;">
                        <asp:Label ID="lblMsj" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <footer class="footer p-2  card" style="background-color: #AA983F;">
            <div class="container-fluid float-md-start">
                <div class="row">
                    <%--                    <div class=" col-sm-12 col-md-1">
                        <img src="../Images/veracruz_orgullo.png" id="escudo" width="100%" />
                    </div>--%>
                    <div class="col-sm-12 col-md-12" style="color: white">
                        <p class="align-self-center text-center">Se prohíbe la reproducción total o parcial contenida en este sistema informático sin el consentimiento expreso y por escrito de la Secretaría de Seguridad Pública de Estado de Veracruz. Esta plataforma digital deberá ser utilizada únicamente por el personal autorizado y debidamente certificado de esta Secretaría. Cualquier violación de la integridad del sistema será castigado severamente. </p>
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
