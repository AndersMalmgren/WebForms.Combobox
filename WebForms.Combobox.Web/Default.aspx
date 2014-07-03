<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForms.Combobox.Web._Default" %>
<%@register tagprefix="web" assembly="WebForms.Combobox.Controls" namespace="WebForms.Combobox.Controls" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <web:Combobox ID="combo" OnItemsRequested="ItemsRequested" runat="server"/>
    <asp:Button OnClick="Postback" Text="Do postback" runat="server"/>
    <asp:Literal ID="text" runat="server"></asp:Literal>
</asp:Content>
