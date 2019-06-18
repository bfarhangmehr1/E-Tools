<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sales.aspx.cs" Inherits="eToolsWebsite.WebPages.Sales" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- TODO: Remove before flight BEGIN -->
    <asp:Button runat="server" ID="TestLogin" Text="Log in as mPants user (has sales role)" OnClick="TestLogin_Click" Visible='<%# !IsSalesUser %>' OnLoad="DataBindOnLoad"/>
    <!-- TODO: Remove before flight END -->

    <!-- BEGIN Sales Messages -->
    <div class="row sales-messages">
        <div class="col-sm-12">
            <uc1:MessageUserControl runat="server" id="MessageUserControl" />
        </div>
    </div>
    <!-- END Sales Messages -->

    <!-- BEGIN Sales Navigation -->
    <div class="row sales-navigation">
        <asp:Button ID="btnTabShopping" runat="server" Text = "Shopping" CssClass='<%# (SalesTabView.ActiveViewIndex == 0) ? "btn btn-primary col-sm-6" : "btn col-sm-6"  %>' OnLoad="DataBindOnLoad" OnClick="btnTabShopping_Click"/>
        <asp:Button ID="btnTabCheckout" runat="server" Text = "Checkout" CssClass='<%# (SalesTabView.ActiveViewIndex == 1) ? "btn btn-primary col-sm-6" : "btn col-sm-6"  %>' OnLoad="DataBindOnLoad" OnClick="btnTabCheckout_Click"/>
    </div>
    <!-- END Sales Navigation -->

    <!-- BEGIN Sales Main Content -->
    <div class="row sales-main-content">

        <asp:MultiView id="SalesTabView" runat="server" ActiveViewIndex="0">
            <asp:View ID="TabShopping" runat="server">
                <!-- BEGIN Sales Categories -->
                <div class="col-sm-2 sales-categories" style="padding-left: 0; padding-right:0">
                    <h4>Categories</h4>
                    <asp:ListView runat="server" ID="CategoryListControl" DataSourceID="CategoryListODS" DataKeyNames="CategoryID, CategoryName">
                        <LayoutTemplate>
                            <div runat="server" class="list-group" id="itemPlaceholderContainer">
                                <div runat="server" id="itemPlaceholder"></div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="list-group-item">
                                <asp:Label Text='<%# Eval("CategoryID") %>' runat="server" ID="CategoryIDLabel" visible="false"/>
                                <asp:LinkButton CommandName="Select" CommandArgument="" Text='<%# Eval("CategoryName") %>' runat="server" ID="SelectCategoryButton" />
                                <asp:Label Text='<%# Eval("ProductCount") %>' runat="server" ID="CategoryProductCountLabel" CssClass="badge badge-secondary"/>
                            </div>
                        </ItemTemplate>
                        <SelectedItemTemplate>
                            <div class="list-group-item active">
                                <asp:Label Text='<%# Eval("CategoryID") %>' runat="server" ID="CategoryIDLabel" visible="false"/>
                                <asp:LinkButton CommandName="Select" CommandArgument="" Text='<%# Eval("CategoryName") %>' runat="server" ID="SelectCategoryButton" style="color: white"/>
                                <asp:Label Text='<%# Eval("ProductCount") %>' runat="server" ID="CategoryProductCountLabel" CssClass="badge badge-secondary"/>
                            </div>
                        </SelectedItemTemplate>
                        <EmptyDataTemplate>
                            <div class="list-group-item">
                                No data found.
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
                <!-- END Sales Categories -->

                <!-- BEGIN Sales Products -->
                <div class="col-sm-8 sales-products">
                    <h4 runat="server"><%= (CategoryListControl.SelectedDataKey == null) ? "All" : CategoryListControl.SelectedDataKey.Values["CategoryName"] %> Products</h4>
                    <asp:ListView runat="server" ID="ProductListControl" DataSourceID="ProductListODS">
                        <LayoutTemplate>
                            <table class="table" id="itemPlaceholderContainer">
                                <thead>
                                    <tr>
                                        <th scope="col" style="width: 60%">Description</th>
                                        <th scope="col" >Unit Price</th>
                                        <th scope="col" style="text-align: right">Stock</th>
                                        <th scope="col" >Qty</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                    <tr><td colspan="5"></td></tr>
                                </tbody>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label Text='<%# Eval("ProductID") %>' runat="server" ID="ProductIDLabel" visible="false" />
                                    <asp:Label Text='<%# Eval("ProductDescription") %>' runat="server" ID="ProductDescriptionLabel" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Label Text='<%# string.Format("{0:C2}",(decimal)Eval("SellingPrice")) %>' runat="server" ID="SellingPriceLabel" />
                                </td>
                                <%-- Display qty on hand + qty input box + add to cart button if product is on stock --%>
                                <td runat="server" style="text-align: right" Visible='<%# (int)Eval("QtyOnHand") > 0 %>'>
                                    <asp:Label runat="server" Text='<%# Eval("QtyOnHand") %>' ID="QtyOnHandLabel" />
                                </td>
                                <td runat="server" Visible='<%# (int)Eval("QtyOnHand") > 0 %>'>
                                    <asp:TextBox runat="server" ID="AddToCartQty" Text="1" TextMode="Number" Width="70px"/>
                                    &nbsp;
                                    <%-- If user is not logged in or has no Sales role, then display an inactive button with a tooltip --%>
                                    <asp:LinkButton runat="server" ID="AddToCartButtonDisabled" 
                                        Visible='<%# !IsSalesUser%>'
                                        CssClass="btn btn-sm btn-info" 
                                        Text="<i class='fa fa-cart-plus'></i> Add" 
                                        ToolTip="Log in to add products to cart" 
                                        OnClick="AddToCartButton_Click"
                                        />
                                    <%-- If user is not logged in and has Sales role, display an active button --%>
                                    <asp:LinkButton runat="server" ID="AddToCartButton" 
                                        Visible='<%# IsSalesUser%>'
                                        CssClass="btn btn-sm btn-primary" 
                                        Text="<i class='fa fa-cart-plus'></i> Add" 
                                        OnClick="AddToCartButton_Click"
                                        />
                                </td>
                                <%-- Display out of stock message + qty on order if product is out of stock --%>
                                <td runat="server" colspan="2" Visible='<%# (int)Eval("QtyOnHand") <= 0 %>' style="color: red">
                                    <div>
                                        <span>Out of stock</span>
                                    </div>
                                    <div>
                                        <asp:Label runat="server" 
                                            Text='<%# string.Format("{0} on order since {1:yyyy-MMM-dd}", Eval("QtyOnOrder"), Eval("PendingOrderDate")) %>' 
                                            Visible='<%# (int)Eval("QtyOnOrder") > 0 && Eval("PendingOrderDate") != null %>' />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <tr>
                                <td colspan="5">
                                    No data found.
                                </td>
                            </tr>
                        </EmptyDataTemplate>
                    </asp:ListView>

                </div>
                <!-- END Sales Products -->
            </asp:View>

            <asp:View ID="TabCheckout" runat="server">
                    <!-- BEGIN Checkout Products -->
                    <div class="col-sm-10 sales-products">
                        <h4>Products in Cart</h4>

                        <asp:ListView runat="server" ID="CartItemsView" DataSourceID="CartItemsODS">
                            <LayoutTemplate>
                                <table class="table" id="itemPlaceholderContainer">
                                    <thead>
                                        <tr>
                                            <th scope="col" style="width: 50%">Description</th>
                                            <th scope="col" style="text-align: right">Unit Price</th>
                                            <th scope="col" style="text-align: center">Qty</th>
                                            <th scope="col" style="text-align: right">Subtotal</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                        <tr><td colspan="4"></td></tr>
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td runat="server" >
                                        <asp:Label Text='<%# Eval("ProductID") %>' runat="server" ID="ProductIDLabel" visible="false" />
                                        <asp:Label Text='<%# Eval("QtyOnHand") %>' runat="server" ID="QtyOnHandLabel" visible="false" />
                                        <asp:Label Text='<%# Eval("ProductDescription") %>' runat="server" ID="ProductDescriptionLabel" />
                                    </td>
                                    <td runat="server" style="text-align: right">
                                        <asp:Label Text='<%# string.Format("{0:C2}",(decimal)Eval("SellingPrice")) %>' runat="server" ID="SellingPriceLabel" />
                                    </td>
                                    <%-- If product is in stock, display qty controls and subtotal --%>
                                    <td runat="server" style="text-align: left" visible='<%# (int)Eval("QtyOnHand") > 0 %>'>
                                        &nbsp;
                                        <asp:LinkButton runat="server" ID="btnDeleteCartItem" OnClick="btnDeleteCartItem_Click"
                                            CssClass="btn btn-sm btn-danger" Text='<i class="glyphicon glyphicon-trash"></i>'></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton runat="server" ID="btnDecCartItem" OnClick="btnDecCartItem_Click"
                                            CssClass='<%# ((int)Eval("QtyInCart") > 1) ? "btn btn-sm btn-primary" : "btn btn-sm btn-info" %>'  
                                            ToolTip='<%# ((int)Eval("QtyInCart") > 1) ? "" : "Not allowed, use delete instead" %>'  
                                            Text='<i class="glyphicon glyphicon-minus"></i>'></asp:LinkButton>
                                        <asp:TextBox runat="server" ID="QtyInCart" Text='<%# Eval("QtyInCart") %>' TextMode="Number" Width="70px" 
                                            AutoPostBack="true" OnTextChanged="btnUpdateCartItem_Click" />
                                        <asp:LinkButton runat="server" ID="btnIncCartItem" OnClick="btnIncCartItem_Click"
                                            CssClass='<%# ((int)Eval("QtyInCart") < (int)Eval("QtyOnHand")) ? "btn btn-sm btn-primary" : "btn btn-sm btn-info" %>'  
                                            ToolTip='<%# ((int)Eval("QtyInCart") < (int)Eval("QtyOnHand")) ? "" : "No more in stock" %>'  
                                            Text='<i class="glyphicon glyphicon-plus"></i>'></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton runat="server" ID="btnUpdateCartItem" OnClick="btnUpdateCartItem_Click"
                                            CssClass="btn btn-sm btn-primary" Text='<i class="glyphicon glyphicon-refresh"></i>'></asp:LinkButton>
                                    </td>
                                    <td runat="server" style="text-align: right" visible='<%# (int)Eval("QtyOnHand") > 0 %>'>
                                        <asp:Label Text='<%# string.Format("{0:C2}",(decimal)Eval("TotalPrice")) %>' runat="server" ID="Label1" />
                                    </td>
                                    <%-- If product is out of stock, display a message and only allow delete --%>
                                    <td runat="server" style="text-align: left; color: red" colspan="2" visible='<%# (int)Eval("QtyOnHand") <= 0 %>'>
                                        &nbsp;
                                        <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnDeleteCartItem_Click"
                                            CssClass="btn btn-sm btn-danger" Text='<i class="glyphicon glyphicon-trash"></i>'></asp:LinkButton>
                                        &nbsp;
                                        <i class="glyphicon glyphicon-alert"></i> Out of stock.
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <tr>
                                    <td colspan="4">
                                        No data found.
                                    </td>
                                </tr>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </div>
                    <!-- END Checkout Products -->
            </asp:View>

            <asp:View ID="TabReceipt" runat="server">
                <!-- BEGIN Sales Receipt -->
                <div class="col-sm-10 sales-receipt">
                    <h4>Sales Receipt</h4>
                    <div class="row form-group">
                        <div class="col-sm-4" style="text-align: left">
                            Receipt #<asp:Label runat="server" ID="Receipt_ID"></asp:Label>
                            - <asp:Label runat="server" ID="Receipt_PaymentType"></asp:Label>
                        </div>
                        <div class="col-sm-4" style="text-align: center">
                            Salesperson: <asp:Label runat="server" ID="Receipt_EmployeeName"></asp:Label>
                        </div>
                        <div class="col-sm-4" style="text-align: right">
                            <asp:Label runat="server" ID="Receipt_Date"></asp:Label>
                        </div>
                    </div>
                        
                    <table class="table" id="itemPlaceholderContainer">
                        <thead>
                            <tr>
                                <th scope="col" style="width: 60%">Item</th>
                                <th scope="col" style="text-align: right">Unit Price</th>
                                <th scope="col" style="text-align: right">Qty</th>
                                <th scope="col" style="text-align: right">Subtotal</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="ReceiptItemList">
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:Label Text='<%# Eval("ProductName") %>' runat="server" ID="ReceiptItem_ProductName" /></td>
                                        <td style="text-align: right"><asp:Label Text='<%# string.Format("{0:C2}",(decimal)Eval("SellingPrice")) %>' runat="server" ID="ReceiptItem_SellingPrice" /></td>
                                        <td style="text-align: right"><asp:Label Text='<%# Eval("Qty") %>' runat="server" ID="ReceiptItem_Qty" /></td>
                                        <td style="text-align: right"><asp:Label Text='<%# string.Format("{0:C2}",(decimal)Eval("SubTotal")) %>' runat="server" ID="ReceiptItem_SubTotal" /></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                        <tfoot style="font-weight: bold">
                            <tr>
                                <td>SUBTOTAL</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td style="text-align: right"><asp:Label runat="server" ID="Receipt_Subtotal"></asp:Label></td>
                            </tr>
                            <tr runat="server" id="Receipt_DiscountLine">
                                <td><asp:Label runat="server" ID="Receipt_DiscountCoupon"></asp:Label></td>
                                <td>&nbsp;</td>
                                <td style="text-align: right"><asp:Label runat="server" ID="Receipt_DiscountPercent"></asp:Label></td>
                                <td style="text-align: right"><asp:Label runat="server" ID="Receipt_DiscountAmount"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>GST</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td style="text-align: right"><asp:Label runat="server" ID="Receipt_TaxAmount"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>TOTAL</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td style="text-align: right"><asp:Label runat="server" ID="Receipt_TotalAmount"></asp:Label></td>
                            </tr>
                            <tr><td colspan="4"></td></tr>
                        </tfoot>
                    </table>
                </div>
                <!-- END Sales Receipt -->
            </asp:View>

        </asp:MultiView>

        <!-- BEGIN Sales Cart Summary -->
        <div runat="server" ID="CartSummary" class="col-sm-2 sales-cart-summary" style="padding-right:0; padding-left:0">
            <h4>Cart Summary</h4>
            <asp:Panel runat="server" ID="CartSummaryPanelNotLoggedIn" CssClass="alert alert-info" Visible="true">
                Log in with Sales role to see shopping cart.
            </asp:Panel>
            <asp:Panel runat="server" ID="CartSummaryPanel" CssClass="alert alert-info" Visible="true">
                <div class="form-group row">
                    <label class="col-sm-12 col-form-label">
                        <span>Salesperson:</span><br/>
                        <asp:Label runat="server" ID="CartSummary_EmployeeID" Visible="false"/>
                        <asp:Label runat="server" ID="CartSummary_EmployeeName" />
                    </label>
                </div>
                <asp:Panel runat="server" ID="CartSummaryPanelEmpty" Visible="true">
                    Cart is empty. Add products to the cart to see the summary.
                </asp:Panel>
                <asp:Panel runat="server" ID="CartSummaryPanelNotEmpty" Visible="true">
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:LinkButton runat="server" ID="CancelButton" 
                                Text="<i class='glyphicon glyphicon-remove'></i> Cancel" 
                                CssClass="btn btn-danger pull-right"
                                OnClick="CancelButton_Click"/> 
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-6 col-form-label">Products</label>
                        <label class="col-sm-6 col-form-label text-right">
                            <asp:Label runat="server" ID="CartSummary_ProductCount" />
                        </label>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-6 col-form-label">Items</label>
                        <label class="col-sm-6 col-form-label text-right">
                            <asp:Label runat="server" ID="CartSummary_ItemCount" />
                        </label>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-6 col-form-label">Subtotal</label>
                        <label class="col-sm-6 col-form-label text-right">
                            <asp:Label runat="server" ID="CartSummary_SubtotalAmount" />
                        </label>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="CartSummary_Coupon" CssClass="form-control" placeholder="Coupon" OnTextChanged="CartSummary_Coupon_TextChanged" AutoPostBack="true"/>
                        </div>
                        <label for="coupon-code" class="col-sm-5 col-form-label">
                            <asp:Label runat="server" ID="CartSummary_DicountIcon_OK"><span class="fa fa-check"></span></asp:Label>
                            <asp:Label runat="server" ID="CartSummary_DiscontIcon_Error" style="color: red"><span class="fa fa-exclamation-triangle"></span></asp:Label>
                            <asp:Label runat="server" ID="CartSummary_DiscountPercent" />
                            <asp:Label runat="server" ID="CartSummary_DiscountError" style="color: red" Text="Expired" />
                        </label>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-6 col-form-label">Discount</label>
                        <label class="col-sm-6 col-form-label text-right">
                            <asp:Label runat="server" ID="CartSummary_DiscountAmount" />
                        </label>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-6 col-form-label">Tax</label>
                        <label class="col-sm-6 col-form-label text-right">
                            <asp:Label runat="server" ID="CartSummary_TaxAmount" />
                        </label>
                    </div>
                    <hr />
                    <div class="form-group row">
                        <label class="col-sm-6 col-form-label">Total</label>
                        <label class="col-sm-6 col-form-label text-right">
                            <asp:Label runat="server" ID="CartSummary_TotalAmount" />
                        </label>
                    </div>
                    <% if (SalesTabView.ActiveViewIndex == 0) { %>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton runat="server" ID="checkoutButton" 
                                    CssClass="btn btn-primary pull-right"
                                    OnClick="btnTabCheckout_Click"
                                    Text="<span class='fa fa-shopping-cart'></span> Checkout">
                                </asp:LinkButton>
                            </div>
                        </div>
                    <% } else { %>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:DropDownList runat="server" ID="PaymentTypeDDL" AutoPostBack="true" CssClass="form-control">
                                    <asp:ListItem Text="Select payment..." Value="0" Selected="true"/>
                                    <asp:ListItem Text="Money" Value="M" />
                                    <asp:ListItem Text="Credit" Value="C" />
                                    <asp:ListItem Text="Debit" Value="D" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton runat="server" ID="btnContinueShopping" 
                                    CssClass="btn btn-default pull-left"
                                    OnClick="btnTabShopping_Click"
                                    Width="48%"
                                    Text="Continue<br />Shopping">
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnPlaceOrder" 
                                    Visible='<%# PaymentTypeDDL.SelectedValue != "0" %>'
                                    CssClass="btn btn-primary pull-right"
                                    OnClick="btnPlaceOrder_Click"
                                    Width="48%"
                                    OnLoad="DataBindOnLoad"
                                    Text="Place<br />Order">
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="AddToCartButtonDisabled" 
                                    Visible='<%# PaymentTypeDDL.SelectedValue == "0" %>'
                                    CssClass="btn btn-info pull-right" 
                                    Width="48%"
                                    OnLoad="DataBindOnLoad"
                                    Text="Place<br />Order" 
                                    ToolTip="Select payment method to place order">
                                </asp:LinkButton>
                            </div>
                        </div>
                    <% } %>
                </asp:Panel>
            </asp:Panel>
        </div>
        <!-- END Sales Cart Summary -->

    </div>
    <!-- BEGIN Sales Main Content -->
    
    <asp:ObjectDataSource ID="CategoryListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="ListAllCategories" 
        TypeName="eToolsSystem.BLL.SalesCategoryController">
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ProductListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="ProductListByCategory" 
        TypeName="eToolsSystem.BLL.SalesProductController">
        <SelectParameters>
            <asp:ControlParameter 
                ControlID="CategoryListControl" 
                PropertyName="SelectedValue" 
                DefaultValue="0" 
                Name="categoryID" 
                Type="Int32">
            </asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="CartItemsODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="ShoppingCart_GetItems" 
        TypeName="eToolsSystem.BLL.SalesCartController">
        <SelectParameters>
            <asp:ControlParameter 
                ControlID="CartSummary_EmployeeID" 
                PropertyName="Text" 
                DefaultValue="0" 
                Name="employeeID" 
                Type="Int32">
            </asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

</asp:Content>
