using System;
namespace WebApplication7
{
 public partial class Payment:System.Web.UI.Page
 {
  protected void Page_Load(object sender,EventArgs e)
  {
   if(Session["UserId"]==null){Response.Redirect("UserLogin.aspx");return;}
   if(Session["ShippingAddress"]==null){Response.Redirect("Checkout.aspx");return;}
   if(!IsPostBack)lblTotal.Text=Database.GetCartTotal(Convert.ToInt32(Session["UserId"])).ToString("N2");
  }
  protected void btnPay_Click(object sender,EventArgs e)
  {
   int u=Convert.ToInt32(Session["UserId"]);string method=ddlPayment.SelectedValue;string status=method=="COD"?"Pending":"Paid";
   int orderId=Database.CreateOrder(u,Session["ShippingAddress"].ToString(),method,status);
   if(orderId==0){lblMessage.Text="Your cart is empty.";return;}
   Session.Remove("ShippingAddress");Response.Redirect("TrackOrder.aspx?id="+orderId);
  }
 }
}