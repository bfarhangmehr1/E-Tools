<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReturnRental.aspx.cs" Inherits="eToolsWebsite.WebPages.Rentals.ReturnRental" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Register Src="~/UserControls/MessageUserControl.ascx" 
    TagPrefix="uc1" TagName="MessageUserControl" %>
    <div>
    <h1>Rental Return</h1>
    <div>
        div class="col-sm-12">
           <uc1:MessageUserControl runat="server" id="MessageUserControl" />
       </div>

        <div>
            <h2>Rental Lookup</h2>

            <asp:Label ID="CustomerEmailLookupLable" runat="server" Text="Customer Email"></asp:Label>
            <asp:TextBox ID="CustomerEmailLookupTextbox" runat="server"></asp:TextBox> &nbsp;
            
            <asp:Label ID="CustomerPhoneLookupLable" runat="server" Text="Customer Phone"></asp:Label>
            <asp:TextBox ID="CustomerPhoneLookupTextbox" runat="server"></asp:TextBox> &nbsp;

            <asp:Button ID="CustomerRentalsLookupButton" runat="server" Text="Search" OnClick="CustomerRentalsLookupButton_Click" />



            <asp:ListView ID="CustomerList" runat="server" 
                    OnItemCommand="CustomerSelected_Click">
                    <AlternatingItemTemplate>
                        <tr style="background-color: #FFFFFF; color: #284775;">
                            <td>
                                <asp:LinkButton ID="SelectCustomer" runat="server"
                                    CssClass="btn btn-primary" CommandArgument='<%# Eval("CustomerID") %>'>
                                    <span aria-hidden="true" class="glyphicon glyphicon-ok"> Select&nbsp;</span>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label Text='<%# Eval("LastName") %>' runat="server" ID="LastNameLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("FirstName") %>' runat="server" ID="FirstNameLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("Address") %>' runat="server" ID="AddressLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("City") %>' runat="server" ID="CityLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("Province") %>' runat="server" ID="ProvinceLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("PostalCode") %>' runat="server" ID="PostalCodeLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("ContactPhone") %>' runat="server" ID="ContactPhoneLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("EmailAddress") %>' runat="server" ID="EmailAddressLabel" /></td>
                            <td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr style="background-color: #E0FFFF; color: #333333;">
                            <td>
                                <asp:LinkButton ID="SelectCustomer" runat="server"
                                    CssClass="btn btn-primary" CommandArgument='<%# Eval("CustomerID") %>'>
                                    <span aria-hidden="true" class="glyphicon glyphicon-ok"> Select&nbsp;</span>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label Text='<%# Eval("LastName") %>' runat="server" ID="LastNameLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("FirstName") %>' runat="server" ID="FirstNameLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("Address") %>' runat="server" ID="AddressLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("City") %>' runat="server" ID="CityLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("Province") %>' runat="server" ID="ProvinceLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("PostalCode") %>' runat="server" ID="PostalCodeLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("ContactPhone") %>' runat="server" ID="ContactPhoneLabel" /></td>
                            <td>
                                <asp:Label Text='<%# Eval("EmailAddress") %>' runat="server" ID="EmailAddressLabel" /></td>
                            <td>
                        </tr>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <td runat="server">
                                    <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                        <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                            <th runat="server">Select Customer</th>
                                            <th runat="server">LastName</th>
                                            <th runat="server">FirstName</th>
                                            <th runat="server">Address</th>
                                            <th runat="server">City</th>
                                            <th runat="server">Province</th>
                                            <th runat="server">PostalCode</th>
                                            <th runat="server">ContactPhone</th>
                                            <th runat="server">EmailAddress</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server">
                                <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                                    <asp:DataPager runat="server" ID="DataPager1">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True"></asp:NextPreviousPagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>

            <h2>Customer Information</h2>

            <asp:GridView ID="CustomerInfoGridview" runat="server"></asp:GridView>




            
            <asp:ListView ID="RentalListListview" runat="server"></asp:ListView>




        </div>


        <div>
            <h2>Equipment Returns</h2>

            <asp:Label ID="RentedEquipmentLable" runat="server" Text="Rented Equipment"></asp:Label>
            <asp:ListView ID="RentedEquipmentTextbox" runat="server"></asp:ListView> <!--List all equipment that is being returned-->
        </div>


        <div>
            <h2>Checkout</h2>

            <!--Subtotal Total GST coupon/discount-->
            <asp:Label ID="CheckoutSubtotalLable" runat="server" Text="Subtotal"></asp:Label>
            <asp:TextBox ID="TextBCheckoutSubtoalTextboxox2" runat="server"></asp:TextBox>&nbsp;

            <asp:Label ID="CheckoutGSTLable" runat="server" Text="GST"></asp:Label>
            <asp:TextBox ID="CheckoutGSTTextbox" runat="server"></asp:TextBox>&nbsp;

            <asp:Label ID="CheckoutDiscountAmountLable" runat="server" Text="DsicountAmount"></asp:Label>
            <asp:TextBox ID="CheckoutDiscountAmountTextbox" runat="server"></asp:TextBox>&nbsp;

            <asp:Label ID="CheckoutTotalLable" runat="server" Text="Total"></asp:Label>
            <asp:TextBox ID="CheckoutTotalTextbox" runat="server"></asp:TextBox>&nbsp;
            
            

            <asp:Label ID="paymentTypelable" runat="server" Text="Total"></asp:Label>
            <asp:TextBox ID="PaymentType" runat="server"></asp:TextBox>&nbsp;

            <asp:Button ID="PayNowButton" runat="server" Text="Pay Now" OnClick="PayNowButton_Click" />
        </div>
    

    </div>


    

    <asp:Label ID="RentalID" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="CustomerID" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="EmployeeID" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="CouponID" runat="server" Text="0" Visible="false"></asp:Label>


</asp:Content>
