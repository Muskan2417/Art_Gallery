using System;
namespace WebApplication7
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Database.ValidateAdmin(txtUsername.Text.Trim(), txtPassword.Text))
            {
                Session["Admin"] = txtUsername.Text.Trim();
                Database.Log(null, Database.GetAdminId(txtUsername.Text.Trim()), "Admin Login", "Admin login successful");
                Response.Redirect("AdminDashboard.aspx");
            }
            else lblMessage.Text = "Invalid username or password.";
        }
    }
}