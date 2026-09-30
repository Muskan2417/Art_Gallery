using System;
namespace WebApplication7
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            int id=Database.ValidateUser(txtEmail.Text.Trim(),txtPassword.Text);
            if(id>0){ Session["UserId"]=id; Response.Redirect("Gallery.aspx"); }
            else lblMessage.Text="Invalid email or password.";
        }
    }
}