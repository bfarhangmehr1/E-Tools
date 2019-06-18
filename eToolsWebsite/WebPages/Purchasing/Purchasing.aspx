<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Purchasing.aspx.cs" Inherits="eToolsWebsite.WebPages.Purchasing" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <h1>Purchasing</h1>
</div>
     <asp:Label ID="label1" runat="server" Text=" Employee: ">
    </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:Label ID="EmployeeDisplayName" runat="server" >
    </asp:Label>&nbsp;&nbsp;
    <asp:Label ID="EmployeeName" runat="server" >
    </asp:Label>&nbsp;&nbsp;
    <asp:LinkButton ID="GetUserName" runat="server" OnClick="GetUserName_Click">
         Employee</asp:LinkButton>
        <br /><br /> 
    <div class="row">
       <asp:LinkButton ID="UpdateOrder" runat="server" Font-Size="X-Large" OnClick="UpdateOrder_Click" >Update</asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="PlaceOrder" runat="server" Font-Size="X-Large" OnClick="PlaceOrder_Click">Place</asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="DeleteOrder" runat="server" Font-Size="X-Large" OnClick="DeleteOrder_Click">Delete</asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="Clear" runat="server" Font-Size="X-Large" OnClick="Clear_Click"  >Clear</asp:LinkButton>&nbsp;&nbsp;
    </div>
    <div class="row">
      <div class="col-sm-12">
          <uc1:MessageUserControl runat="server" id="MessageUserControl" />
          </div>
       </div>
    <div class="row" text-align: left;">  
              <div class="col-sm-8">
             <h3 >Current Active Order</h3>
                  <asp:GridView ID="OrderList" runat="server" AutoGenerateColumns="False"
                      BorderStyle="Solid"  style=" margin-left: 14%;"  
                       AllowPaging="true" OnPageIndexChanging="OrderList_PageIndexChanging"
                      CssClass="table tabel-hover tabel stripped" OnSelectedIndexChanged="OrderList_SelectedIndexChanged">
                      <Columns>
                          <asp:TemplateField HeaderText="StockItemID">
                              <ItemTemplate>
                                  <asp:Label runat="server" Text='<%# Eval("StockItemID") %>' ID="StockItemID"></asp:Label>
                                  &nbsp;&nbsp;
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="Description">
                              <ItemTemplate>
                                  <asp:Label runat="server" Text='<%# Eval("Description") %>' ID="Description"></asp:Label>
                                  &nbsp;&nbsp;
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="QOH">
                              <ItemTemplate>
                                  <asp:Label runat="server" Text='<%# Eval("QuantityOnHand") %>' ID="QuantityOnHand"></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="QOO">
                              <ItemTemplate>
                                  <asp:Label runat="server" Text='<%# Eval("QuantityOnOrder") %>' ID="QuantityOnOrder" Width="30"></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="ROL">
                              <ItemTemplate>
                                  <asp:Label runat="server" Text='<%# Eval("ReOrderLevel") %>' ID="ReOrderLevel"></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="QTO">
                              <ItemTemplate>
                                  <asp:TextBox ID="QuantityToOrder" runat="server" Text='<%# Bind("QuantityToOrder") %>' Width="50" Type="number"></asp:TextBox>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="Price">
                              <ItemTemplate>
                                  <asp:TextBox ID="Price" runat="server" Text='<%#   Bind("Price","{0:c2}") %>' Width="70" ></asp:TextBox>
                              </ItemTemplate>
                          </asp:TemplateField>
                    <%--     <asp:TemplateField>                      
                        <ItemTemplate>
                              <asp:Label runat="server" Text='<%# Eval("PurchaseOrderDetailID") %>' ID="PurchaseOrderDetailsID" Visible="false"></asp:Label>                                                      
                        </ItemTemplate>
                    </asp:TemplateField>    --%>          
                          <asp:CommandField SelectText="Remove" ShowSelectButton="True"></asp:CommandField>
                      </Columns>
            </asp:GridView> 
        </div>  
        <div class="col-sm-4">
     <%--  <asp:Label ID="ItemsBy" runat="server"  ></asp:Label>&nbsp;&nbsp;--%>
       <%-- <asp:Label ID="SearchArgID" runat="server" ></asp:Label> &nbsp;   --%>     
         <asp:Label ID="Label10" runat="server" Text="GST" ForeColor="#0033cc"></asp:Label>&nbsp;
            <asp:Label ID="GSTID" runat="server" ></asp:Label>&nbsp;
         <asp:Label ID="Label11" runat="server" Text="SubTotal" ForeColor="#0033cc"></asp:Label>&nbsp;
            <asp:Label ID="SubTotalID" runat="server" ></asp:Label>&nbsp;
         <asp:Label ID="Label13" runat="server" Text="Total" ForeColor="#0033cc"></asp:Label>&nbsp;
            <asp:Label ID="TotalID" runat="server" ></asp:Label>            
        </div> 
        </div>
    <div class="row">       
       <asp:Label ID="Label2" runat="server" Text="Vendors" Font-Bold="true" ForeColor="#3333ff" Font-Size="Large" AssociatedControlID="VendorDLL">
       </asp:Label> &nbsp;&nbsp;  
            <asp:DropDownList ID="VendorDLL" runat="server"
                DataSourceID="VendorDDLODS" 
                DataTextField="VendorName"
                 DataValueField="VendorID"
                 AppendDataBoundItems="true">
                <asp:ListItem Value="0">select...</asp:ListItem>
            </asp:DropDownList>      
          &nbsp;&nbsp;<br /><br />
          <asp:LinkButton ID="VendorFetch" runat="server"  Width="150px" Font-Size="X-Large" OnClick="VendorFetch_Click">Fetch
          </asp:LinkButton><br /><br />
        </div> 

        <div class="row">
             <asp:Label runat="server" ID="Label3" Text="Name:" ForeColor="#0033cc"></asp:Label>
             <asp:Label runat="server" ID="Name"  Width="170px"></asp:Label><br />
        <asp:Label runat="server" ID="Label4"  Text="Phone:" ForeColor="#0033cc"></asp:Label> 
            <asp:Label runat="server" ID="Phone"  Width="90px"></asp:Label><br />           
                <asp:Label runat="server" ID="Label5"  Text="Address:" ForeColor="#0033cc"></asp:Label> 
              <asp:Label runat="server" ID="Address"  Width="200px"></asp:Label> <br />           
              <asp:Label runat="server" ID="Label6"  Text="City:"  ForeColor="#0033cc" ></asp:Label> 
             <asp:Label runat="server"  ID="City"  Width="100px"></asp:Label>  <br />         
             <asp:Label runat="server" ID="Label7"   Text="Province:"  ForeColor="#0033cc"></asp:Label> 
             <asp:Label runat="server"  ID="Province"  Width="40px"></asp:Label> <br />          
             <asp:Label runat="server" ID="Label8"  Text="PostalCode:"  ForeColor="#0033cc"></asp:Label> 
             <asp:Label runat="server" ID="PostalCode"  Width="50px"></asp:Label>  <br />       
        </div>   
   <div class="row">
  <h3>Vendor StockItems</h3>
   <%--<asp:Label runat="server" ID="Vendor"  Width="170px"></asp:Label>--%>
   </div>           
     <div class="row"  style="margin-left: 10%; text-align: left;">        
            <asp:ListView ID="VendorStockItemList" runat="server" 
                 OnItemCommand=" ItemSelectionList_ItemCommand"        
                 >
                <AlternatingItemTemplate>
                    <tr style="background-color: #FFF8DC;">  
                        
                        <td>
                            <asp:Label Text='<%# Eval("StockItemID") %>' runat="server" ID="StockItemIDLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("QuantityOnHand") %>' runat="server" ID="QuantityOnHandLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("QuantityOnOrder") %>' runat="server" ID="QuantityOnOrderLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("ReOrderLevel") %>' runat="server" ID="ReOrderLevelLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Buffer") %>' runat="server" ID="BufferLabel" /></td>
                        <td>
                            <asp:Label Text='<%# string.Format("{0:c2}", (decimal) Eval("PurchasePrice")) %>' runat="server" ID="PurchasePriceLabel" /></td>
                         <td>
                           <asp:LinkButton ID="AddtoCurrentOrder" runat="server" 
                             CssClass="btn" CommandArgument='<%# Eval("StockItemID") %>'>
                            <span aria-hidden="true" >&nbsp; Add</span>
                        </asp:LinkButton></td>
                    </tr>
                </AlternatingItemTemplate>
              
                <EmptyDataTemplate>
                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            
                <ItemTemplate>
                         
                        <td>
                            <asp:Label Text='<%# Eval("StockItemID") %>' runat="server" ID="StockItemIDLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("QuantityOnHand") %>' runat="server" ID="QuantityOnHandLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("QuantityOnOrder") %>' runat="server" ID="QuantityOnOrderLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("ReOrderLevel") %>' runat="server" ID="ReOrderLevelLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Buffer") %>' runat="server" ID="BufferLabel" /></td>
                        <td>
                           <asp:Label Text='<%# string.Format("{0:c2}", (decimal) Eval("PurchasePrice")) %>' runat="server" ID="PurchasePriceLabel" /></td>
                          <td>
                           <asp:LinkButton ID="AddtoCurrentOrder" runat="server" 
                             CssClass="btn" CommandArgument='<%# Eval("StockItemID") %>'>
                            <span aria-hidden="true" >&nbsp; Add</span>
                        </asp:LinkButton></td>
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                        <th runat="server">StockItemID</th>
                                        <th runat="server">Description</th>
                                        <th runat="server">QOH</th>
                                        <th runat="server">QOO</th>
                                        <th runat="server">ROL</th>
                                        <th runat="server">Buffer</th>
                                        <th runat="server">Price</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                <asp:DataPager runat="server" ID="DataPager1" PageSize="20" PagedControlID="VendorStockItemList">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                    <asp:NumericPagerField></asp:NumericPagerField>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                </Fields>
                            </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
               
            </asp:ListView>
        </div>   
  
    <asp:ObjectDataSource ID="VendorDDLODS"
        runat="server"
        OldValuesParameterFormatString="original_{0}"
        SelectMethod="Vendor_List"
        TypeName="eToolsSystem.BLL.VendorComtroller"></asp:ObjectDataSource>
</asp:Content>
