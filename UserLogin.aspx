<%@ Page Title="User Login" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="WebApplication7.UserLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="admin-box narrow"><h1>Welcome Back</h1><p class="muted">Login to shop, save favourites and track your orders.</p>
<div class="field"><label>Email</label><asp:TextBox ID="txtEmail" runat="server" TextMode="Email"></asp:TextBox></div>
<div class="field"><label>Password</label><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></div>
<br/><asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click"/>
<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
<p>New here? <a href="Signup.aspx">Create an account</a></p></div>
</asp:Content>