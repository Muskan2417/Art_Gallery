using System;
using System.Data;
namespace WebApplication7
{
 public partial class TrackOrder:System.Web.UI.Page
 {
  protected void Page_Load(object sender,EventArgs e)
  {
   if(Session["UserId"]==null){Response.Redirect("UserLogin.aspx");return;}
   if(!IsPostBack)LoadOrder();
  }
  private void LoadOrder()
  {
   int id; if(!int.TryParse(Request.QueryString["id"],out id)){pnlOrder.Visible=false;lblMessage.Text="Invalid order.";return;}
   int u=Convert.ToInt32(Session["UserId"]);DataTable dt=Database.GetOrder(u,id);
   if(dt.Rows.Count==0){pnlOrder.Visible=false;lblMessage.Text="Order not found.";return;}
   DataRow r=dt.Rows[0];lblOrderId.Text=r["OrderId"].ToString();lblTotal.Text=Convert.ToDecimal(r["TotalAmount"]).ToString("N2");
   lblPayment.Text = r["PaymentMethod"].ToString() + " - " + r["PaymentStatus"].ToString();
   lblStatus.Text = r["Status"].ToString();
   lblAddress.Text = r["ShippingAddress"].ToString();
   gvTracking.DataSource=Database.GetTracking(u,id);gvTracking.DataBind();gvItems.DataSource=Database.GetOrderItems(u,id);gvItems.DataBind();
  }
 }
}