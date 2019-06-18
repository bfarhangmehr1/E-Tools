<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rentals.aspx.cs" Inherits="eToolsWebsite.WebPages.Rentals.Rentals" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <h1>Rentals</h1>

    <asp:Button ID="CreateNewRentalButton" runat="server" Text="New Rental" OnClick="CreateNewRentalButton_Click" CssClass="btn btn-primary" />
    <asp:Button ID="RetturnRentalButton" runat="server" Text="Return Rental" OnClick="RetturnRentalButton_Click" CssClass="btn btn-primary"/>
    
</asp:Content>
