using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace WebApplication7
{
 public partial class AdminDashboard:System.Web.UI.Page
 {
  protected void Page_Load(object sender,EventArgs e){if(Session["Admin"]==null){Response.Redirect("AdminLogin.aspx");return;}if(!IsPostBack){BindGrid();BindOrders();}}
  private void BindGrid(){gvArtworks.DataSource=Database.GetArtworks();gvArtworks.DataBind();}
  private void BindOrders(){gvOrders.DataSource=Database.GetAllOrders();gvOrders.DataBind();}
  protected void btnSave_Click(object sender,EventArgs e)
  {
   decimal price;if(string.IsNullOrWhiteSpace(txtTitle.Text)||string.IsNullOrWhiteSpace(txtArtist.Text)||string.IsNullOrWhiteSpace(txtCategory.Text)||string.IsNullOrWhiteSpace(txtImageUrl.Text)||!decimal.TryParse(txtPrice.Text,NumberStyles.Any,CultureInfo.InvariantCulture,out price))
   {lblMessage.Text="Please enter title, artist, category, price and image URL correctly.";return;}
   int admin=Database.GetAdminId(Session["Admin"].ToString());
   if(string.IsNullOrEmpty(hfArtId.Value)){Database.AddArtwork(txtTitle.Text.Trim(),txtArtist.Text.Trim(),txtCategory.Text.Trim(),txtMedium.Text.Trim(),txtSize.Text.Trim(),price,txtImageUrl.Text.Trim(),txtDescription.Text.Trim());Database.Log(null,admin,"Add Painting",txtTitle.Text.Trim());lblMessage.Text="Painting added successfully.";}
   else{int id=Convert.ToInt32(hfArtId.Value);Database.UpdateArtwork(id,txtTitle.Text.Trim(),txtArtist.Text.Trim(),txtCategory.Text.Trim(),txtMedium.Text.Trim(),txtSize.Text.Trim(),price,txtImageUrl.Text.Trim(),txtDescription.Text.Trim());Database.Log(null,admin,"Update Painting","ArtId="+id);lblMessage.Text="Painting updated successfully.";}
   ClearForm();BindGrid();
  }
  protected void gvArtworks_RowCommand(object s,GridViewCommandEventArgs e)
  {
   int rowIndex=Convert.ToInt32(e.CommandArgument); int id=Convert.ToInt32(gvArtworks.DataKeys[rowIndex].Value); DataRow r=Database.GetArtwork(id); if(r==null)return; int admin=Database.GetAdminId(Session["Admin"].ToString());
   if(e.CommandName=="DeleteArt"){bool deleted=Database.DeleteArtwork(id);if(deleted){Database.Log(null,admin,"Delete Painting","ArtId="+id);lblMessage.Text="Painting deleted successfully.";}else{lblMessage.Text="This painting cannot be deleted because it is already part of an order. It has been kept for order history.";}BindGrid();}
   else if(e.CommandName=="EditArt"){hfArtId.Value=id.ToString();txtTitle.Text=r["Title"].ToString();txtArtist.Text=r["Artist"].ToString();txtCategory.Text=r["Category"].ToString();txtMedium.Text=r["Medium"].ToString();txtSize.Text=r["Size"].ToString();txtPrice.Text=Convert.ToDecimal(r["Price"]).ToString("0.00");txtImageUrl.Text=r["ImageUrl"].ToString();txtDescription.Text=r["Description"].ToString();lblFormTitle.Text="Update Painting";btnSave.Text="Update Painting";}
  }
  protected void gvOrders_RowCommand(object s,GridViewCommandEventArgs e)
  {
   if(e.CommandName!="Status")return;GridViewRow row=(GridViewRow)((Button)e.CommandSource).NamingContainer;DropDownList ddl=(DropDownList)row.FindControl("ddlStatus");
   int orderId=Convert.ToInt32(e.CommandArgument);Database.UpdateOrderStatus(Database.GetAdminId(Session["Admin"].ToString()),orderId,ddl.SelectedValue,"Order status updated by gallery admin.");lblMessage.Text="Order status updated.";BindOrders();
  }
  protected void btnCancel_Click(object s,EventArgs e){ClearForm();}
  private void ClearForm(){hfArtId.Value="";txtTitle.Text=txtArtist.Text=txtCategory.Text=txtSize.Text=txtPrice.Text=txtImageUrl.Text=txtDescription.Text="";txtMedium.Text="Digital Painting";lblFormTitle.Text="Add New Painting";btnSave.Text="Add Painting";}
  protected void btnLogout_Click(object s,EventArgs e){Session["Admin"]=null;Response.Redirect("AdminLogin.aspx");}
 }
}