using System;
using System.Web.UI.WebControls;

namespace WebApplication7
{
    public partial class Gallery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string category = Request.QueryString["category"];
                if (!string.IsNullOrWhiteSpace(category) && ddlCategory.Items.FindByValue(category) != null)
                    ddlCategory.SelectedValue = category;
                BindArtworks();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindArtworks();
        }

        private void BindArtworks()
        {
            rptArtworks.DataSource = Database.GetArtworks(txtSearch.Text.Trim(), ddlCategory.SelectedValue);
            rptArtworks.DataBind();
        }

        protected void rptArtworks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("UserLogin.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            int artId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Cart")
            {
                Database.AddToCart(userId, artId);
                lblMessage.Text = "Painting added to your cart.";
            }
            else if (e.CommandName == "Favorite")
            {
                Database.ToggleFavorite(userId, artId);
                lblMessage.Text = Database.IsFavorite(userId, artId) ? "Added to favourites." : "Removed from favourites.";
            }
        }
    }
}
