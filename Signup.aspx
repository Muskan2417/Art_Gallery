<%@ Page Title="Create Account" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="WebApplication7.Signup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="admin-box narrow"><h1>Create Account</h1><p class="muted">Create your gallery account to purchase and save paintings.</p>
<div class="field"><label>Full Name</label><asp:TextBox ID="txtName" runat="server"></asp:TextBox></div>
<div class="field"><label>Email</label><asp:TextBox ID="txtEmail" runat="server" TextMode="Email"></asp:TextBox></div>
<div class="field"><label>Phone</label><asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></div>
<div class="field"><label>Password</label><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></div>
<div class="field"><label>Confirm Password</label><asp:TextBox ID="txtConfirm" runat="server" TextMode="Password"></asp:TextBox></div>
<br/><asp:Button ID="btnSignup" runat="server" Text="Create Account" CssClass="btn" OnClick="btnSignup_Click"/>
<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
<p>Already registered? <a href="UserLogin.aspx">Login</a></p></div>
</asp:Content>