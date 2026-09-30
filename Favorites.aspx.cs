using System;
using System.Web.UI.WebControls;
namespace WebApplication7
{
 public partial class Favorites:System.Web.UI.Page
 {
  protected void Page_Load(object sender,EventArgs e){if(Session["UserId"]==null){Response.Redirect("UserLogin.aspx");return;}if(!IsPostBack)Bind();}
  private void Bind(){rptFavorites.DataSource=Database.GetFavorites(Convert.ToInt32(Session["UserId"]));rptFavorites.DataBind();}
  protected void rptFavorites_ItemCommand(object s,RepeaterCommandEventArgs e)
  {int u=Convert.ToInt32(Session["UserId"]);int a=Convert.ToInt32(e.CommandArgument);if(e.CommandName=="Cart"){Database.AddToCart(u,a);lblMessage.Text="Added to cart.";}else{Database.ToggleFavorite(u,a);lblMessage.Text="Removed from favourites.";Bind();}}
 }
}