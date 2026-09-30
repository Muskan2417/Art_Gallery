<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="WebApplication7.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="hero"><h1>Your Cart</h1><p>Review your paintings before checkout.</p></div>
<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
<asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CssClass="grid" DataKeyNames="CartId" OnRowCommand="gvCart_RowCommand">
<Columns>
<asp:BoundField DataField="Title" HeaderText="Painting" /><asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="&#8377; {0:N2}" />
<asp:TemplateField HeaderText="Quantity"><ItemTemplate><asp:TextBox ID="txtQty" runat="server" Text='<%# Eval("Quantity") %>' Width="50"></asp:TextBox></ItemTemplate></asp:TemplateField>
<asp:BoundField DataField="LineTotal" HeaderText="Total" DataFormatString="&#8377; {0:N2}" />
<asp:ButtonField ButtonType="Button" Text="Update" CommandName="UpdateQty" />
<asp:ButtonField ButtonType="Button" Text="Remove" CommandName="RemoveItem" />
</Columns></asp:GridView>
<div class="admin-box center"><h2>Total: &#8377; <asp:Label ID="lblTotal" runat="server"></asp:Label></h2><a href="Gallery.aspx" class="btn btn-light">Continue Shopping</a>
<asp:Button ID="btnCheckout" runat="server" Text="Proceed to Checkout" CssClass="btn" OnClick="btnCheckout_Click" /></div>
</asp:Content>