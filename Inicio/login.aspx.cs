using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lexus2._0.Inicio
{
    public partial class login : System.Web.UI.Page
    {
        string sUsuarioActual;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login_LoggedIn(object sender, EventArgs e)
        {

        }

        protected void Login_Authenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                MembershipUser usuario = Membership.GetUser(Login.UserName);
                if (Membership.ValidateUser(Login.UserName, Login.Password))
                {
                    if (usuario.IsLockedOut)
                    {
                        lblMsj.Text = "SE HA BLOQUADO EL USUARIO";
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(usuario.UserName, createPersistentCookie: false);
                        Response.Redirect("~/");
                    }
                }
                else
                {
                    if (usuario != null && usuario.IsLockedOut)
                    {
                        lblMsj.Text = "SE HA BLOQUEADO EL USUARIO DEBIDO AL MÁXIMO NÚMERO DE INTENTOS FALLIDOS, FAVOR DE CONTACTAR AL ADMINISTRADOR DEL SISTEMA AL TELÉFONO (228) 1-41-38-00 Ext. 8000";
                    }
                    else
                    {
                        lblMsj.Text = "ERROR DE USUARIO Y/O CONTRASEÑA";
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "error()", true);

            }


        }
    }
}