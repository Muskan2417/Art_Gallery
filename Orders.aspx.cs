using System;
namespace WebApplication7
{
 public partial class Orders:System.Web.UI.Page
 {
  protected void Page_Load(object sender,EventArgs e){if(Session["UserId"]==null){Response.Redirect("UserLogin.aspx");return;}if(!IsPostBack){gvOrders.DataSource=Database.GetOrders(Convert.ToInt32(Session["UserId"]));gvOrders.DataBind();}}
 }
}