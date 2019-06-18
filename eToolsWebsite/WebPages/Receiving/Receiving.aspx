<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Receiving.aspx.cs" Inherits="eToolsWebsite.WebPages.Receiving" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Heading -->
    <h1>Receiving</h1>

    <!-- Controls -->
    <div class="row">
        <div class="col-sm-12">
            <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
        </div>
    </div>
    <hr />

    <!-- Content -->
    <div class="row">
        <!-- Search area -->
        <div class="col-lg-4" 
            style="border-right:1px solid Silver">
            <!-- Heading -->
            <h3 style="text-align:center">
                Outstanding Orders</h3>

            <!-- Styling -->
            <div class="row">
                <div class="col-sm-2"></div>
                <div class="col-sm-8">
                    <hr />
                </div>
                <div class="col-sm-2"></div>
            </div>

            <!-- Content -->
            <asp:ListView
                ID="OrderList" runat="server" 
                DataSourceID="OrderList_ODS"
                OnItemCommand="OrderList_ItemCommand">
                <ItemTemplate>
                    <tr style="background-color: #D3D0DA; color: #323030;">
                        <td style="width:50px ; text-align:end">
                            <asp:Label Text='<%# Eval("PONumber") %>' runat="server" ID="PONumberLabel" /></td>
                        <td style="width:100px">
                            <asp:Label Text='<%# Eval("PODate", "{0:dd/MM/yyyy}") %>' runat="server" ID="PODateLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorName") %>' runat="server" ID="VendorNameLabel" 
                                Font-Size="Small" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorPhone") %>' runat="server" ID="VendorPhoneLabel" /></td>
                        <td>
                            <asp:LinkButton 
                                ID="OrderButton" runat="server"
                                CssClass="btn" 
                                CommandArgument='<%# Eval("POID") %>'>
                                <span aria-hidden="true" class="glyphicon glyphicon-arrow-right"></span>
                            </asp:LinkButton></td>
                    </tr>
                </ItemTemplate>

                <AlternatingItemTemplate>
                    <tr style="background-color: #B5B3BE; color: #323030;">
                        <td style="width:50px ; text-align:end">
                            <asp:Label Text='<%# Eval("PONumber") %>' runat="server" ID="PONumberLabel" /></td>
                        <td style="width:100px">
                            <asp:Label Text='<%# Eval("PODate", "{0:dd/MM/yyyy}") %>' runat="server" ID="PODateLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorName") %>' runat="server" ID="VendorNameLabel" 
                                Font-Size="Small" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorPhone") %>' runat="server" ID="VendorPhoneLabel" /></td>
                        <td>
                            <asp:LinkButton 
                                ID="OrderButton" runat="server"
                                CssClass="btn" 
                                CommandArgument='<%# Eval("POID") %>'>
                                <span aria-hidden="true" class="glyphicon glyphicon-arrow-right"></span>
                            </asp:LinkButton></td>
                    </tr>
                </AlternatingItemTemplate>
          
                <SelectedItemTemplate>
                    <tr style="background-color: #9FB6CD; color: #323030;">
                        <td style="width:50px ; text-align:end">
                            <asp:Label Text='<%# Eval("PONumber") %>' runat="server" ID="PONumberLabel" /></td>
                        <td style="width:100px">
                            <asp:Label Text='<%# Eval("PODate", "{0:dd/MM/yyyy}") %>' runat="server" ID="PODateLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorName") %>' runat="server" ID="VendorNameLabel" 
                                Font-Size="Small" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorPhone") %>' runat="server" ID="VendorPhoneLabel" /></td>
                        <td>
                            <asp:LinkButton 
                                ID="OrderButton" runat="server"
                                CssClass="btn" 
                                CommandArgument='<%# Eval("POID") %>'>
                                <span aria-hidden="true" class="glyphicon glyphicon-arrow-right" style="color: #4F4A45"></span>
                            </asp:LinkButton></td>
                    </tr>
                </SelectedItemTemplate>

                <EmptyDataTemplate>
                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                        <tr>
                            <td>No outstanding orders found on file</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>

                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" 
                                    style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" 
                                        style="background-color: #4F4A45; color: #CDAB81">
                                        <th runat="server" style="text-align:center">PO #</th>
                                        <th runat="server" style="text-align:center">Date</th>
                                        <th runat="server" style="text-align:center">Vendor</th>
                                        <th runat="server" style="text-align:center">Phone #</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;"></td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
        </div>

        <!-- Result area -->
        <div class="col-lg-8"
            style="border-left:1px solid Silver">
            <!-- Heading -->
            <h3 style="padding-left:12%">
                Order Details</h3>

            <!-- Styling -->
            <div class="row">
                <div class="col-sm-1"></div>
                <div class="col-sm-5">
                    <hr />
                </div>
                <div class="col-sm-6"></div>
            </div>

            <style type="text/css">
                .gridview table th {text-align:center; font-family: Verdana, Arial, Helvetica, sans-serif;}
            </style>

            <!-- Content -->
            <asp:Label ID="PurchaseOrderID" runat="server" Text="Label" 
                Visible="False" Enabled="False"></asp:Label>
            
            <div class="row" 
                style="padding-bottom:8px">
                <div class="col-md-1"></div>
                <div class="col-md-2">
                    <asp:Label ID="NumberLabel" runat="server" Text="PO #" 
                        Font-Size="Medium" ForeColor="#323030"></asp:Label>
                </div>
                <div class="col-md-9">
                    <asp:TextBox ID="Number" runat="server" Enabled="False" 
                        Font-Size="Medium" ForeColor="#323030"></asp:TextBox>
                </div>
            </div>

            <div class="row" 
                style="padding-bottom:8px">
                <div class="col-md-1"></div>
                <div class="col-md-2">
                    <asp:Label ID="VendorLabel" runat="server" Text="Vendor" 
                        Font-Size="Medium" ForeColor="#323030"></asp:Label>
                </div>
                <div class="col-md-9">
                    <asp:TextBox ID="Vendor" runat="server" Enabled="False" 
                        Font-Size="Medium" ForeColor="#323030"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="col-md-1"></div>
                <div class="col-md-2">
                    <asp:Label ID="PhoneLabel" runat="server" Text="Phone #" 
                        Font-Size="Medium" ForeColor="#323030"></asp:Label>
                </div>
                <div class="col-md-9">
                    <asp:TextBox ID="Phone" runat="server" Enabled="False" 
                        Font-Size="Medium" ForeColor="#323030"></asp:TextBox>
                </div>
            </div>
            <br />
            <br />

            <div class="gridview">
                <asp:GridView 
                    ID="ReceivingGrid" runat="server" 
                    AutoGenerateColumns="False" 
                    CellPadding="4" CellSpacing="2" 
                    HorizontalAlign="Center">
                    <AlternatingRowStyle BackColor="#B5B3BE" ForeColor="#323030" />            
                    <RowStyle BackColor="#D3D0DA" ForeColor="#323030" />
                    <HeaderStyle Font-Size="Medium" BackColor="#4F4A45" ForeColor="#CDAB81" />
                    <Columns>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("POID") %>' id="POIDLabel"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("PODetailID") %>' id="PODetailIDLabel"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SID" 
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("StockItemID") %>' id="StockItemIDLabel"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Description") %>' id="DescriptionLabel"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="QtyO" 
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("QtyOrdered") %>' id="QtyOrderedLabel"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="QOS" 
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("QtyOutstanding") %>' id="QtyOutstandingLabel"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Receive" 
                            HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="QtyReceived" 
                                    Width="100%" TextMode="Number" Style="text-align:right"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Return" 
                            HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="QtyReturned" 
                                    Width="100%" TextMode="Number" Style="text-align:right"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reason">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ReturnReason" 
                                    MaxLength="50"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EmptyDataTemplate>
                        No order detail records found on file
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <hr />

            <div class="row">
                <!-- Returns -->
                <div class="col-md-9">          
                    <asp:ListView
                        ID="ReturnList" runat="server"
                        DataSourceID="ReturnList_ODS" 
                        InsertItemPosition="LastItem"
                        DataKeyNames="CartID">
                        <ItemTemplate>
                            <tr style="background-color: #D3D0DA; color: #323030;">
                                <asp:HiddenField Value='<%# Eval("PurchaseOrderID") %>' runat="server" ID="PurchaseOrderIDField" />
                                <td style="text-align:center">
                                    <asp:LinkButton
                                        ID="DeleteButton" runat="server"
                                        CssClass="btn" 
                                        CommandArgument='<%# Eval("CartID") %>'
                                        OnClick="DeleteButton_Click" 
                                        ForeColor="#FA6E59">
                                        <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("CartID") %>' runat="server" ID="CartIDLabel" 
                                        Width="50px" Style="text-align:right" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("VendorStockNumber") %>' runat="server" ID="VendorStockNumberLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" 
                                        Width="50px" Style="text-align:right" /></td>
                            </tr>
                        </ItemTemplate>
                        
                        <AlternatingItemTemplate>
                            <tr style="background-color: #B5B3BE; color: #323030;">
                                <asp:HiddenField Value='<%# Eval("PurchaseOrderID") %>' runat="server" ID="PurchaseOrderIDField" />
                                <td style="text-align:center">
                                    <asp:LinkButton
                                        ID="DeleteButton" runat="server"
                                        CssClass="btn" 
                                        CommandArgument='<%# Eval("CartID") %>'
                                        OnClick="DeleteButton_Click" 
                                        ForeColor="#FA6E59">
                                        <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("CartID") %>' runat="server" ID="CartIDLabel" 
                                        Width="50px" Style="text-align:right" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("VendorStockNumber") %>' runat="server" ID="VendorStockNumberLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" 
                                        Width="50px" Style="text-align:right" /></td>
                            </tr>
                        </AlternatingItemTemplate>

                        <InsertItemTemplate>
                            <tr style="background-color:#4F4A45">
                                <asp:HiddenField Value='<%# Bind("PurchaseOrderID") %>' runat="server" ID="PurchaseOrderIDField" />
                                <td>
                                    <asp:LinkButton
                                        ID="InsertButton" runat="server"
                                        CssClass="btn" 
                                        OnClick="InsertButton_Click" 
                                        ForeColor="#50C878">
                                        <span aria-hidden="true" class="glyphicon glyphicon-plus"></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton
                                        ID="CancelButton" runat="server"
                                        CssClass="btn"
                                        OnClick="CancelButton_Click" 
                                        ForeColor="#FFB745">
                                        <span aria-hidden="true" class="glyphicon glyphicon-minus"></span>
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("CartID") %>' runat="server" ID="CartIDTextBox" 
                                        Width="50px" Enabled="False" Style="text-align:right" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" 
                                        MaxLength="200" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("VendorStockNumber") %>' runat="server" ID="VendorStockNumberTextBox" 
                                        MaxLength="50" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" 
                                        Width="50px" TextMode="Number" Style="text-align:right" /></td>
                            </tr>
                        </InsertItemTemplate>

                        <EmptyDataTemplate>
                            <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>

                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" 
                                            style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                            <tr runat="server" style="background-color: #4F4A45; color: #CDAB81;">
                                                <th runat="server" style="text-align:center">Options</th>
                                                <th runat="server" style="text-align:center">CID</th>
                                                <th runat="server" style="text-align:center">Description</th>
                                                <th runat="server" style="text-align:center">VSN</th>
                                                <th runat="server" style="text-align:center">Qty</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;"></td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
                </div>

                <!-- Actions -->
                <div class="col-md-3" 
                    style="text-align:end ; border-left:1px solid #EBEBEB">
                    <asp:Panel ID="ActionsPanel" runat="server">
                        <!-- Receive -->
                        <div class="row" 
                            style="padding-bottom:2px">
                            <div class="col-md-12">
                                <asp:Button ID="ReceiveButton" runat="server" Text="RECEIVE" 
                                    Font-Size="Large" Font-Bold="True" ForeColor="#50C878" 
                                    OnClick="ReceiveButton_Click" />
                            </div>             
                        </div>

                        <!-- Force close -->
                        <div class="row">
                            <div class="col-md-12" 
                                style="padding-top:2px">
                                <asp:Button ID="ForceCloseButton" runat="server" Text="FORCE CLOSE" 
                                    Font-Size="Large" Font-Bold="True" ForeColor="#FA6E59" 
                                    Width="100%" 
                                    OnClick="ForceCloseButton_Click" />
                            </div> 
                            <div class="col-md-12" 
                                style="padding-top:2px">
                                <asp:TextBox ID="ForceCloseTextBox" runat="server" 
                                    TextMode="MultiLine" 
                                    MaxLength="200" 
                                    ToolTip="Enter reason for force close..." 
                                    Width="100%">
                                </asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

            </div>

        </div>

        <!-- Source area (not visible to user) -->
        <asp:ObjectDataSource
            ID="OrderList_ODS" runat="server" 
            OldValuesParameterFormatString="original_{0}" 
            SelectMethod="OpenOrder_List" 
            TypeName="eToolsSystem.BLL.PurchaseOrderController">
        </asp:ObjectDataSource>

        <asp:ObjectDataSource
            ID="ReturnList_ODS" runat="server"
            DataObjectTypeName="eTools.Data.Entities.UnorderedPurchaseItemCart"
            SelectMethod="UPIC_List"
            OldValuesParameterFormatString="original_{0}"
            TypeName="eToolsSystem.BLL.UnorderedPurchaseItemCartController">
        </asp:ObjectDataSource>
    </div>
</asp:Content>
