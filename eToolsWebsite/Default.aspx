<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="eToolsWebsite._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="text-align:center">
        <h1>Home</h1>
        <h3>of the</h3>
        <asp:Image ID="LogoImage" runat="server" 
                ImageUrl="~/Images/MMFA_Logo.png" 
                Width="50%"/>
    </div>

    <div class="row">
        <div class="col-md-8">
            <h2>Members</h2>
            <table class="teammembers">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Subsystem</th>
                        <th>Shared Components</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Behnam Farhangmehr</td>
                        <td>Purchasing</td>
                        <td>Entities (reverse engineering)</td>
                    </tr>
                    <tr>
                        <td>Linus Stern</td>
                        <td>Receiving</td>
                        <td>Security<br />About page</td>
                    </tr>
                    <tr>
                        <td>Sean Laing</td>
                        <td>Rentals</td>
                        <td>Readme<br />Artwork</td>
                    </tr>
                    <tr>
                        <td>Zsolt Endre</td>
                        <td>Sales</td>
                        <td>Security setup<br />Home page</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="col-md-4">
            <h2>Bugs / Issues</h2>
            <p>
                We keep track of our bugs and issues on GitHub.com
            </p>
            <p>
                <a class="btn btn-default" href="https://github.com/DMIT-2018/eTools_2018_E01_Team_E/issues/">Issue List &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
