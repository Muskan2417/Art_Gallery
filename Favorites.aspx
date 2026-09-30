<%@ Page Title="My Favourites" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Favorites.aspx.cs" Inherits="WebApplication7.Favorites" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="hero"><h1>My Favourites</h1><p>Paintings you have saved for later.</p></div>
<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
<div class="cards"><asp:Repeater ID="rptFavorites" runat="server" OnItemCommand="rptFavorites_ItemCommand"><ItemTemplate>
<div class="card"><img src='<%# Server.HtmlEncode(Eval("ImageUrl").ToString()) %>' alt='<%# Server.HtmlEncode(Eval("Title").ToString()) %>' />
<div class="card-body"><h3><%# Server.HtmlEncode(Eval("Title").ToString()) %></h3><p class="muted"><%# Server.HtmlEncode(Eval("Artist").ToString()) %></p><p class="price">&#8377; <%# Eval("Price","{0:N2}") %></p>
<asp:Button ID="btnCart" runat="server" Text="Add to Cart" CssClass="btn" CommandName="Cart" CommandArgument='<%# Eval("ArtId") %>' />
<asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-light" CommandName="Remove" CommandArgument='<%# Eval("ArtId") %>' /></div></div>
</ItemTemplate></asp:Repeater></div>
</asp:Content>