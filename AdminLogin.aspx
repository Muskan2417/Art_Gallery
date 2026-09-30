<%@ Page Title="AdminLogin" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="WebApplication7.AdminLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="admin-box" style="max-width:500px;margin:70px auto"><h1>Admin Login</h1><p class="muted">Sign in to manage the gallery collection.</p>
<div class="field"><label>Username</label><asp:TextBox ID="txtUsername" runat="server"></asp:TextBox></div><br/>
<div class="field"><label>Password</label><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></div><br/>
<asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" />
<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label></div>
</asp:Content>