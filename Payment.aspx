<%@ Page Title="Payment" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="WebApplication7.Payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="hero"><h1>Payment</h1><p>Choose how you want to pay.</p></div>
<div class="admin-box narrow"><p class="price">Amount Payable: &#8377; <asp:Label ID="lblTotal" runat="server"></asp:Label></p>
<div class="field"><label>Payment Method</label><asp:DropDownList ID="ddlPayment" runat="server" CssClass="select"><asp:ListItem Text="UPI (Demo Payment)" Value="UPI"></asp:ListItem><asp:ListItem Text="Card (Demo Payment)" Value="Card"></asp:ListItem><asp:ListItem Text="Cash on Delivery" Value="COD"></asp:ListItem></asp:DropDownList></div><br/>
<p class="muted">Online payment in this academic project is recorded as a demo transaction; no real card/UPI credentials are stored.</p>
<asp:Button ID="btnPay" runat="server" Text="Place Order & Pay" CssClass="btn" OnClick="btnPay_Click" />
<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label></div>
</asp:Content>