<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication7.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="hero">
    <img src="https://i.pinimg.com/1200x/63/4a/71/634a7128f68c8f0c6fd33d3a3a2094ab.jpg" alt="Heritage Fame Art Gallery" />
    <h1>Where Art Tells a Story</h1>
    <p>Explore paintings that bring together creativity, culture, emotion and artistic heritage.</p>
    <a class="btn" href="Gallery.aspx">Explore Gallery</a>
</section>

<h2 class="section-title">Categories</h2>
<div class="cards">
    <div class="card"><img src="https://i.pinimg.com/1200x/22/04/e5/2204e586ef5db78623cbfed4ce0225d8.jpg" alt="Nature" /><div class="card-body"><h3>Nature</h3><p class="muted">Peaceful landscapes and natural beauty.</p><a class="btn btn-light" href="Gallery.aspx?category=Nature">View Art</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/8f/33/54/8f3354e21ba4d5d254f6e208845dc4a2.jpg" alt="Abstract" /><div class="card-body"><h3>Abstract</h3><p class="muted">Creative forms, colours and expressive ideas.</p><a class="btn btn-light" href="Gallery.aspx?category=Abstract">View Art</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/97/b0/d5/97b0d5c459590392cc348f7ace5371c3.jpg" alt="Wildlife" /><div class="card-body"><h3>Wildlife</h3><p class="muted">Art inspired by the world of wildlife.</p><a class="btn btn-light" href="Gallery.aspx?category=Wildlife">View Art</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/27/69/b5/2769b5fc6391bd3e3c5e12f3a7b39768.jpg" alt="Religious" /><div class="card-body"><h3>Religious</h3><p class="muted">Meaningful works inspired by faith and culture.</p><a class="btn btn-light" href="Gallery.aspx?category=Religious">View Art</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/72/d4/07/72d4075052905e815be26eeda6ede561.jpg" alt="Soulmate" /><div class="card-body"><h3>Soulmate</h3><p class="muted">Romantic works celebrating love and connection.</p><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Art</a></div></div>
</div>

<h2 class="section-title">Explore Gallery</h2>
<p class="center muted">A special selection of the eight paintings you supplied for the Explore Gallery.</p>
<div class="cards">
    <div class="card"><img src="https://i.pinimg.com/736x/b8/2b/78/b82b7878a869f7db55a3730a1a0638a9.jpg" alt="Two Hearts, One Journey" /><div class="card-body"><h3>Two Hearts, One Journey</h3><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Collection</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/eb/e7/59/ebe759bfa5403bf871c57f8e350f63a8.jpg" alt="Love in Harmony" /><div class="card-body"><h3>Love in Harmony</h3><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Collection</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/f4/b0/a0/f4b0a0ca7ef23f612c29d94baefd79ed.jpg" alt="Eternal Bond" /><div class="card-body"><h3>Eternal Bond</h3><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Collection</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/14/d0/55/14d0551ed54e2235693478ecd6e16e7d.jpg" alt="A Moment for Two" /><div class="card-body"><h3>A Moment for Two</h3><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Collection</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/b9/54/a7/b954a7a95e3946fbd6bac2f41302e9c8.jpg" alt="Heartstrings" /><div class="card-body"><h3>Heartstrings</h3><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Collection</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/736x/7b/b3/9e/7bb39eb0ba1a96a25be3ea83306959d6.jpg" alt="Our Forever" /><div class="card-body"><h3>Our Forever</h3><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Collection</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/1200x/5c/29/d9/5c29d9f91a93f9f0b09fcdfb0cb17dd6.jpg" alt="Together, Always" /><div class="card-body"><h3>Together, Always</h3><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Collection</a></div></div>
    <div class="card"><img src="https://i.pinimg.com/1200x/82/77/36/8277369f1516137870a72cd6810c6f69.jpg" alt="Where Hearts Meet" /><div class="card-body"><h3>Where Hearts Meet</h3><a class="btn btn-light" href="Gallery.aspx?category=Soulmate">View Collection</a></div></div>
</div>
</asp:Content>
