using System;
namespace WebApplication7
{
 public partial class Site1 : System.Web.UI.MasterPage
 {
  protected void Page_Load(object sender, EventArgs e) { }
  protected void lnkLogout_Click(object sender, EventArgs e)
  {
   Session.Clear();
   Session.Abandon();
   Response.Redirect("Home.aspx");
  }
 }
}
