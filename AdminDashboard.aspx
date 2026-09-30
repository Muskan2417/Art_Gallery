<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="WebApplication7.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="hero"><h1>Admin Dashboard</h1><p>Manage paintings and customer orders.</p></div>
<div class="admin-box"><h2><asp:Label ID="lblFormTitle" runat="server">Add New Painting</asp:Label></h2>
<div class="form-grid">
<div class="field"><label>Title</label><asp:TextBox ID="txtTitle" runat="server"></asp:TextBox></div><div class="field"><label>Artist</label><asp:TextBox ID="txtArtist" runat="server"></asp:TextBox></div>
<div class="field"><label>Category</label><asp:TextBox ID="txtCategory" runat="server"></asp:TextBox></div><div class="field"><label>Medium</label><asp:TextBox ID="txtMedium" runat="server" Text="Digital Painting"></asp:TextBox></div>
<div class="field"><label>Size</label><asp:TextBox ID="txtSize" runat="server"></asp:TextBox></div><div class="field"><label>Price</label><asp:TextBox ID="txtPrice" runat="server"></asp:TextBox></div>
<div class="field full"><label>Image URL</label><asp:TextBox ID="txtImageUrl" runat="server"></asp:TextBox></div>
<div class="field full"><label>Description</label><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox></div>
</div><br/><asp:Button ID="btnSave" runat="server" Text="Add Painting" CssClass="btn" OnClick="btnSave_Click" /><asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="btn btn-light" OnClick="btnCancel_Click" />
<asp:HiddenField ID="hfArtId" runat="server" /><asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label></div>

<div class="admin-box"><h2>Paintings</h2><asp:GridView ID="gvArtworks" runat="server" AutoGenerateColumns="False" CssClass="grid" DataKeyNames="ArtId" OnRowCommand="gvArtworks_RowCommand">
<Columns><asp:BoundField DataField="ArtId" HeaderText="ID" /><asp:ImageField DataImageUrlField="ImageUrl" HeaderText="Image" ControlStyle-Width="65" ControlStyle-Height="75" />
<asp:BoundField DataField="Title" HeaderText="Title" /><asp:BoundField DataField="Category" HeaderText="Category" /><asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="&#8377; {0:N2}" />
<asp:ButtonField ButtonType="Button" Text="Edit" CommandName="EditArt" /><asp:ButtonField ButtonType="Button" Text="Delete" CommandName="DeleteArt" /></Columns></asp:GridView></div>

<div class="admin-box"><h2>Customer Orders</h2><asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="grid" DataKeyNames="OrderId" OnRowCommand="gvOrders_RowCommand">
<Columns><asp:BoundField DataField="OrderId" HeaderText="Order" /><asp:BoundField DataField="FullName" HeaderText="Customer" /><asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="&#8377; {0:N2}" />
<asp:BoundField DataField="Status" HeaderText="Status" /><asp:BoundField DataField="PaymentStatus" HeaderText="Payment" />
<asp:TemplateField HeaderText="Update"><ItemTemplate><asp:DropDownList ID="ddlStatus" runat="server" CssClass="select">
<asp:ListItem>Order Placed</asp:ListItem><asp:ListItem>Confirmed</asp:ListItem><asp:ListItem>Preparing</asp:ListItem><asp:ListItem>Shipped</asp:ListItem><asp:ListItem>Out for Delivery</asp:ListItem><asp:ListItem>Delivered</asp:ListItem><asp:ListItem>Cancelled</asp:ListItem>
</asp:DropDownList><asp:Button ID="btnStatus" runat="server" Text="Save" CssClass="btn" CommandName="Status" CommandArgument='<%# Eval("OrderId") %>' /></ItemTemplate></asp:TemplateField>
</Columns></asp:GridView></div>
<asp:Button ID="btnLogout" runat="server" Text="Admin Logout" CssClass="btn" OnClick="btnLogout_Click" />
</asp:Content>