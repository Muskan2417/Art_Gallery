using System;
using System.Web.UI.WebControls;
namespace WebApplication7
{
 public partial class Cart:System.Web.UI.Page
 {
  protected void Page_Load(object sender,EventArgs e){if(Session["UserId"]==null){Response.Redirect("UserLogin.aspx");return;}if(!IsPostBack)Bind();}
  private void Bind(){gvCart.DataSource=Database.GetCart(Convert.ToInt32(Session["UserId"]));gvCart.DataBind();lblTotal.Text=Database.GetCartTotal(Convert.ToInt32(Session["UserId"])).ToString("N2");}
  protected void gvCart_RowCommand(object s,GridViewCommandEventArgs e)
  {
   if(e.CommandName!="UpdateQty"&&e.CommandName!="RemoveItem")return;
   int row=Convert.ToInt32(e.CommandArgument); int u=Convert.ToInt32(Session["UserId"]); int cartId=Convert.ToInt32(gvCart.DataKeys[row].Value);
   if(e.CommandName=="RemoveItem") Database.RemoveCart(u,cartId);
   else {TextBox t=(TextBox)gvCart.Rows[row].FindControl("txtQty");int q; if(!int.TryParse(t.Text,out q))q=1;Database.UpdateCartQuantity(u,cartId,q);}
   Bind();
  }
  protected void btnCheckout_Click(object sender,EventArgs e){if(Database.GetCartTotal(Convert.ToInt32(Session["UserId"]))<=0){lblMessage.Text="Your cart is empty.";return;}Response.Redirect("Checkout.aspx");}
 }
}