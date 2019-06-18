<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="eToolsWebsite.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <!-- Heading -->
    <h2>About The Mighty Morphin' Flower Arrangers</h2>
    <hr />
    
    <!-- Members -->
    <h3>Members</h3>
    <div class ="row">
        <!-- Behnam -->
        <div class="col-md-6" 
            style="text-align:center ; border-right:1px solid #EBEBEB">
            <asp:Label ID="BehnamLabel" runat="server" Text="Behnam Farhangmehr"></asp:Label>
            <br />
            <asp:Image ID="BehnamImage" runat="server" 
                ImageUrl="~/Images/PhotoPlaceholder.png" 
                Width="100%" />
        </div>

        <!-- Linus -->
        <div class="col-md-6" 
            style="text-align:center">
            <asp:Label ID="LinusLabel" runat="server" Text="Linus Stern"></asp:Label>
            <br />
            <asp:Image ID="LinusImage" runat="server" 
                ImageUrl="~/Images/MMFA_LinusHeadshot.png" 
                Width="100%" />
        </div>
    </div>
    <hr />

    <div class ="row">
        <!-- Sean -->
        <div class="col-md-6" 
            style="text-align:center ; border-right:1px solid #EBEBEB">
            <asp:Label ID="SeanLabel" runat="server" Text="Sean Laing"></asp:Label>
            <br />
            <asp:Image ID="SeanImage" runat="server" 
                ImageUrl="~/Images/PhotoPlaceholder.png" 
                Width="100%" />
        </div>

        <!-- Zsolt -->
        <div class="col-md-6" 
            style="text-align:center">
            <asp:Label ID="ZsoltLabel" runat="server" Text="Zsolt Endre"></asp:Label>
            <br />
            <asp:Image ID="ZsoltImage" runat="server" 
                ImageUrl="~/Images/PhotoPlaceholder.png" 
                Width="100%" />
        </div>
    </div>
    <hr />

    <!-- Security -->
    <h3>Security</h3>
    <div class="row">
        <div class="col-md-12">
            <!-- Roles -->
            <p style="font-weight:bold">Role Info</p>
            <asp:ListBox ID="RoleList" runat="server">
                <asp:ListItem>WebsiteAdmins</asp:ListItem>
                <asp:ListItem>RegisteredUsers</asp:ListItem>
                <asp:ListItem>Staff</asp:ListItem>
                <asp:ListItem>Purchasing</asp:ListItem>
                <asp:ListItem>Receiving</asp:ListItem>
                <asp:ListItem>Sales</asp:ListItem>
                <asp:ListItem>Rentals</asp:ListItem>
            </asp:ListBox>
            <br />
            <br />

            <!-- Users -->
            <p style="font-weight:bold">User Info</p>
            <asp:ListView
                ID="UserList" runat="server" 
                DataSourceID="UserList_ODS"
                ItemType="AppSecurity.POCOs.UserProfile"
                DataKeyNames="UserId">
                <EmptyDataTemplate>
                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                        <tr>
                            <td>No registered users found</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>

                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                        <th runat="server">User</th>
                                        <th runat="server">Password</th>
                                        <th runat="server">Roles</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>

                <ItemTemplate>
                    <tr style="background-color: #DCDCDC; color: #000000;">
                        <td>
                            <asp:Label Text='<%# Eval("UserName") %>' runat="server" ID="UserNameLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("RequestedPassord") %>' runat="server" ID="RequestedPassordLabel" /></td>
                        <td>
                            <div class="col-sm-3">
                                <asp:Repeater ID="RoleUserReapter" runat="server"
                                    DataSource="<%# Item.RoleMemberships %>"
                                    ItemType="System.String">
                                    <ItemTemplate>
                                            <%# Item %>
                                    </ItemTemplate>
                                    <SeparatorTemplate>, </SeparatorTemplate>
                                </asp:Repeater>
                            </div></td>
                    </tr>
                </ItemTemplate>
                
                <AlternatingItemTemplate>
                    <tr style="background-color: #FFF8DC;">
                        <td>
                            <asp:Label Text='<%# Eval("UserName") %>' runat="server" ID="UserNameLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("RequestedPassord") %>' runat="server" ID="RequestedPassordLabel" /></td>
                        <td>
                            <div class="col-sm-3">
                                <asp:Repeater ID="RoleUserReapter" runat="server"
                                    DataSource="<%# Item.RoleMemberships %>"
                                    ItemType="System.String">
                                    <ItemTemplate>
                                            <%# Item %>
                                    </ItemTemplate>
                                    <SeparatorTemplate>, </SeparatorTemplate>
                                </asp:Repeater>
                            </div></td>
                    </tr>
                </AlternatingItemTemplate> 
            </asp:ListView>

            <asp:ObjectDataSource
                ID="UserList_ODS" runat="server" 
                OldValuesParameterFormatString="original_{0}" 
                SelectMethod="ListAllUserInfo" 
                TypeName="AppSecurity.BLL.ApplicationUserManager"
                OnObjectCreating="UserListViewODS_ObjectCreating">
            </asp:ObjectDataSource>
        </div>
    </div>
    <hr />

    <!-- Connections -->
    <h3>Connection Strings</h3>
    <pre>
        &lt;connectionStrings&gt;
          &lt;add 
            name="DefaultConnection" 
            connectionString="Data Source=.; Initial Catalog=eTools; Integrated Security=True" 
            providerName="System.Data.SqlClient" /&gt;
          &lt;add
            name="eToolsDB"
            connectionString="Data Source=.; Initial Catalog=eTools; Integrated Security=true"
            providerName="System.Data.SqlClient"/&gt;
          &lt;!--In the event SQL server configuration did not use the default "." as the name, replace in above connectionString--&gt;
        &lt;/connectionStrings&gt;
    </pre>
</asp:Content>
