<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error403.aspx.cs" Inherits="Lexus2._0.Error.Error404" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Error 404 </title>
    <style>
        body { font-family: Arial, sans-serif; text-align: center; padding-top: 50px; background-color: #f4f4f4; color: #333; }
        .container { background: #fff; padding: 40px; border-radius: 8px; display: inline-block; box-shadow: 0px 0px 10px rgba(0,0,0,0.1); max-width: 400px; }
        h1 { color: #f39c12; font-size: 48px; margin-bottom: 10px; }
        p { font-size: 16px; color: #666; line-height: 1.5; }
        .btn { display: inline-block; margin-top: 20px; padding: 10px 20px; background: #3498db; color: #fff; text-decoration: none; border-radius: 5px; }
        .btn:hover { background: #2980b9; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>404</h1>
            <p><strong>Página no encontrada:</strong> No se encuentra.</p>
            <a href="~/Default.aspx" runat="server" class="btn">Volver al Inicio</a>
        </div>
    </form>
 </body>
</html>