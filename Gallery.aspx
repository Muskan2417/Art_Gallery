<%@ Page Title="Explore Gallery" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Gallery.aspx.cs" Inherits="WebApplication7.Gallery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="hero">
    <h1>Explore Gallery</h1>
    <p>Discover paintings from every collection.</p>
</div>

<div class="filters">
    <asp:TextBox ID="txtSearch" runat="server" CssClass="input" placeholder="Search by painting or artist"></asp:TextBox>
    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select">
        <asp:ListItem Text="All Categories" Value=""></asp:ListItem>
        <asp:ListItem Text="Nature" Value="Nature"></asp:ListItem>
        <asp:ListItem Text="Abstract" Value="Abstract"></asp:ListItem>
        <asp:ListItem Text="Wildlife" Value="Wildlife"></asp:ListItem>
        <asp:ListItem Text="Religious" Value="Religious"></asp:ListItem>
        <asp:ListItem Text="Soulmate" Value="Soulmate"></asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
</div>

<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

<div class="cards">
<asp:Repeater ID="rptArtworks" runat="server" OnItemCommand="rptArtworks_ItemCommand">
<ItemTemplate>
    <div class="card">
        <img src='<%# Server.HtmlEncode(Eval("ImageUrl").ToString()) %>' alt='<%# Server.HtmlEncode(Eval("Title").ToString()) %>' />
        <div class="card-body">
            <span class="tag"><%# Server.HtmlEncode(Eval("Category").ToString()) %></span>
            <h3><%# Server.HtmlEncode(Eval("Title").ToString()) %></h3>
            <p class="muted">by <%# Server.HtmlEncode(Eval("Artist").ToString()) %></p>
            <p class="price">&#8377; <%# Eval("Price", "{0:N2}") %></p>
            <a class="btn btn-light" href='ViewArt.aspx?id=<%# Eval("ArtId") %>'>View Art</a>
            <asp:Button ID="btnFavorite" runat="server" Text="&#10084; Favorite" CssClass="btn btn-light" CommandName="Favorite" CommandArgument='<%# Eval("ArtId") %>' />
            <asp:Button ID="btnCart" runat="server" Text="&#128722; Add to Cart" CssClass="btn" CommandName="Cart" CommandArgument='<%# Eval("ArtId") %>' />
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>
</div>
</asp:Content>
