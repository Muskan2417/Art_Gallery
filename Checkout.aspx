<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="WebApplication7.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="hero"><h1>Checkout</h1><p>Enter the delivery address for your order.</p></div>
<div class="admin-box narrow"><div class="field"><label>Shipping Address</label><asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"></asp:TextBox></div>
<p class="price">Order Total: &#8377; <asp:Label ID="lblTotal" runat="server"></asp:Label></p>
<asp:Button ID="btnContinue" runat="server" Text="Continue to Payment" CssClass="btn" OnClick="btnContinue_Click" />
<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label></div>
</asp:Content>