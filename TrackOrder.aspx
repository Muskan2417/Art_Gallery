<%@ Page Title="Track Order" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="TrackOrder.aspx.cs" Inherits="WebApplication7.TrackOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="hero"><h1>Track Your Order</h1><p>Follow the progress of your purchase.</p></div>
<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
<asp:Panel ID="pnlOrder" runat="server">
<div class="admin-box"><h2>Order #<asp:Label ID="lblOrderId" runat="server"></asp:Label></h2>
<p>Total: &#8377; <asp:Label ID="lblTotal" runat="server"></asp:Label> · Payment: <asp:Label ID="lblPayment" runat="server"></asp:Label></p>
<p>Current Status: <strong><asp:Label ID="lblStatus" runat="server"></asp:Label></strong></p><p>Delivery Address: <asp:Label ID="lblAddress" runat="server"></asp:Label></p>
<h3>Tracking History</h3><asp:GridView ID="gvTracking" runat="server" AutoGenerateColumns="False" CssClass="grid"><Columns>
<asp:BoundField DataField="Status" HeaderText="Status" /><asp:BoundField DataField="Note" HeaderText="Update" /><asp:BoundField DataField="UpdatedAt" HeaderText="Time" DataFormatString="{0:dd MMM yyyy hh:mm tt}" />
</Columns></asp:GridView></div>
<div class="admin-box"><h3>Items</h3><asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False" CssClass="grid"><Columns>
<asp:BoundField DataField="Title" HeaderText="Painting" /><asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="&#8377; {0:N2}" /><asp:BoundField DataField="Quantity" HeaderText="Qty" /><asp:BoundField DataField="LineTotal" HeaderText="Total" DataFormatString="&#8377; {0:N2}" />
</Columns></asp:GridView></div>
</asp:Panel>
</asp:Content>