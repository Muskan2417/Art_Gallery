<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="WebApplication7.Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="hero"><h1>My Orders</h1><p>View your purchases and track delivery progress.</p></div>
<asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="grid">
<Columns><asp:BoundField DataField="OrderId" HeaderText="Order ID" /><asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
<asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="&#8377; {0:N2}" /><asp:BoundField DataField="Status" HeaderText="Status" /><asp:BoundField DataField="PaymentStatus" HeaderText="Payment" />
<asp:HyperLinkField DataNavigateUrlFields="OrderId" DataNavigateUrlFormatString="TrackOrder.aspx?id={0}" Text="Track Order" /></Columns></asp:GridView>
</asp:Content>