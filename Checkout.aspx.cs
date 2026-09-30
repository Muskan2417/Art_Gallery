using System;
namespace WebApplication7
{
 public partial class Checkout:System.Web.UI.Page
 {
  protected void Page_Load(object sender,EventArgs e){if(Session["UserId"]==null){Response.Redirect("UserLogin.aspx");return;}if(!IsPostBack)lblTotal.Text=Database.GetCartTotal(Convert.ToInt32(Session["UserId"])).ToString("N2");}
  protected void btnContinue_Click(object sender,EventArgs e){if(string.IsNullOrWhiteSpace(txtAddress.Text)){lblMessage.Text="Please enter your shipping address.";return;}Session["ShippingAddress"]=txtAddress.Text.Trim();Response.Redirect("Payment.aspx");}
 }
}