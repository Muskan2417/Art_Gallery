using System;
namespace WebApplication7
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtName.Text)||string.IsNullOrWhiteSpace(txtEmail.Text)||string.IsNullOrWhiteSpace(txtPassword.Text))
            { lblMessage.Text="Please fill all required fields."; return; }
            if(txtPassword.Text!=txtConfirm.Text){ lblMessage.Text="Passwords do not match."; return; }
            int id=Database.CreateUser(txtName.Text.Trim(),txtEmail.Text.Trim(),txtPassword.Text,txtPhone.Text.Trim());
            if(id==-1){ lblMessage.Text="An account with this email already exists."; return; }
            Session["UserId"]=id;
            Response.Redirect("Gallery.aspx");
        }
    }
}