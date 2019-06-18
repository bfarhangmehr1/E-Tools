<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateNewRental.aspx.cs" Inherits="eToolsWebsite.WebPages.Rentals.CreateNewRental" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Register Src="~/UserControls/MessageUserControl.ascx" 
    TagPrefix="uc1" TagName="MessageUserControl" %>
    
    
    <h1>New Rental</h1>
    <!--Customer feilds-->
    <div>
        <div class="col-sm-12">
           <uc1:MessageUserControl runat="server" id="MessageUserControl" />
       </div>

        <div>
            <h2>Customer Lookup</h2>
            <asp:Label ID="CustomerPhoneLookupLable" runat="server" Text="Customer Phone Lookup"></asp:Label>
            <asp:TextBox ID="CustomerPhoneLookupTextbox" runat="server"></asp:TextBox> &nbsp;

            <asp:Label ID="CustomerEmailLookupLable" runat="server" Text="Customer Email Lookup"></asp:Label>
            <asp:TextBox ID="CustomerEmailLookupTextbox" runat="server"></asp:TextBox>&nbsp;

            <asp:Button ID="LookupButton" runat="server" Text="Search" OnClick="SearchForCustomers" CssClass="btn btn-primary"/>
            <div class=".col-sm-3">
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
            </div>



        </div>

        <div>
            <h2>Customer Information</h2>

            <asp:GridView ID="CustomerInfoGridview" runat="server"></asp:GridView>


            <asp:Label ID="CreditCardLable" runat="server" Text="Credit Card"></asp:Label>
            <asp:TextBox ID="CreditCardTextbox" runat="server"></asp:TextBox>

        </div>

        <div>
            <h2>Equipment Rental</h2> &nbsp; &nbsp;
            <asp:Label ID="EquipmentTypeLabel" runat="server" Text="Equipment Description/Model Number"></asp:Label>
            <asp:TextBox ID="EquipmentDescriptionSearch" runat="server"></asp:TextBox>
            
            <asp:Button ID="EquipmentSearchButton" runat="server" Text="Search" OnClick="EquipmentSearchButton_Click" CssClass="btn btn-primary"/>&nbsp;

            <asp:Label ID="AvalibleEquipmentListLable" runat="server" Text="Avalible Equipment"></asp:Label>
            <asp:ListView ID="AvalibleEquipmentListview" runat="server" 
                OnItemCommand="EquipmentSelected_Click">
                <AlternatingItemTemplate>
                    <tr style="background-color: #FFFFFF; color: #284775;">
                        
                         <td>
                        <asp:LinkButton ID="AddtoRental" runat="server"
                                CssClass="btn btn-success" CommandArgument='<%# Eval("RentalEquipmentID") %>'>
                            <span aria-hidden="true" class="glyphicon glyphicon-plus"> Add&nbsp;</span>
                        </asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("ModelNumber") %>' runat="server" ID="ModelNumberLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("SerialNumber") %>' runat="server" ID="SerialNumberLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("DailyRate") %>' runat="server" ID="DailyRateLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Condition") %>' runat="server" ID="ConditionLabel" /></td>                        
                       
                    </tr>
                </AlternatingItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                        <tr>
                            <td>No equipment was found.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr style="background-color: #E0FFFF; color: #333333;">
                         <td>
                        <asp:LinkButton ID="LinkButton1" runat="server"
                                CssClass="btn btn-success" CommandArgument='<%# Eval("RentalEquipmentID") %>'>
                            <span aria-hidden="true" class="glyphicon glyphicon-plus"> Add&nbsp;</span>
                        </asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("ModelNumber") %>' runat="server" ID="ModelNumberLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("SerialNumber") %>' runat="server" ID="SerialNumberLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("DailyRate") %>' runat="server" ID="DailyRateLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Condition") %>' runat="server" ID="ConditionLabel" /></td>
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                        <th runat="server">Add to Rental</th>
                                        <th runat="server">Description</th>
                                        <th runat="server">ModelNumber</th>
                                        <th runat="server">SerialNumber</th>
                                        <th runat="server">DailyRate</th>
                                        <th runat="server">Condition</th>
                                        
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                                <asp:DataPager runat="server" ID="DataPager2">
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


            <!--Second list view to show what is in the rental currently-->

            <asp:ListView ID="EquipmentBeingRentedListview" runat="server"
                OnItemCommand="EquipmentRemoved_Click">
                <AlternatingItemTemplate>
                    <tr style="background-color: #FFFFFF; color: #284775;">

                        <td>
                            <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("ModelNumber") %>' runat="server" ID="ModelNumberLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("SerialNumber") %>' runat="server" ID="SerialNumberLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("DailyRate") %>' runat="server" ID="DailyRateLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Condition") %>' runat="server" ID="ConditionLabel" /></td>
                        <td>
                            <asp:LinkButton ID="RemoveFromRental" runat="server"
                                CssClass="btn btn-danger" CommandArgument='<%# Eval("RentalEquipmentID") %>'>
                            <span aria-hidden="true" class="glyphicon glyphicon-minus"> Remove&nbsp;</span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                        <tr>
                            <td>No equipment was found.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr style="background-color: #E0FFFF; color: #333333;">
                        <td>
                            <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("ModelNumber") %>' runat="server" ID="ModelNumberLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("SerialNumber") %>' runat="server" ID="SerialNumberLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("DailyRate") %>' runat="server" ID="DailyRateLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Condition") %>' runat="server" ID="ConditionLabel" /></td>
                        <td>
                            <asp:LinkButton ID="RemoveFromRental" runat="server"
                                CssClass="btn btn-danger" CommandArgument='<%# Eval("RentalEquipmentID") %>'>
                            <span aria-hidden="true" class="glyphicon glyphicon-minus"> Remove&nbsp;</span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                        <th runat="server">Description</th>
                                        <th runat="server">ModelNumber</th>
                                        <th runat="server">SerialNumber</th>
                                        <th runat="server">DailyRate</th>
                                        <th runat="server">Condition</th>
                                        <th runat="server">Remove From Rental</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                                <asp:DataPager runat="server" ID="DataPager2">
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

        <div>
            <h3>Finish</h3>

            <asp:Label ID="CouponLable" runat="server" Text="Coupon ID"></asp:Label>
            <asp:TextBox ID="CouponTextbox" runat="server"></asp:TextBox>
            <asp:Button ID="CouponCheck" runat="server" Text="Check Coupon" OnClick="CouponCheck_Click" CssClass="btn btn-primary"/>
            &nbsp;
            &nbsp;
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_Click" CssClass="btn btn-danger"/>
            <asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click" CssClass="btn btn-primary"/>
        </div>
    </div>

    <asp:Label ID="RentalID" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="CustomerID" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="EmployeeID" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="CouponID" runat="server" Text="0" Visible="false"></asp:Label>

</asp:Content>
