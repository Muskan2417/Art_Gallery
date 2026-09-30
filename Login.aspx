<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication7.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="admin-box narrow center"><h1>Login</h1><p class="muted">Choose how you would like to continue.</p>
<div class="login-choice"><div><h2>&#128100; User</h2><p>Sign in to shop, save favourites and track orders.</p><a class="btn" href="UserLogin.aspx">User Login</a><a class="btn btn-light" href="Signup.aspx">New User / Sign Up</a></div>
<div><h2>&#128104; Admin</h2><p>Manage paintings and customer orders.</p><a class="btn" href="AdminLogin.aspx">Admin Login</a></div></div></div>
</asp:Content>