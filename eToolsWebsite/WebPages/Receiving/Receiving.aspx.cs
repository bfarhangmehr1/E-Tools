using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using AppSecurity.Entities;
using eToolsSystem.BLL;
using eTools.Data.Entities;
using eTools.Data.POCOs;
#endregion

namespace eToolsWebsite.WebPages
{
    public partial class Receiving : System.Web.UI.Page
    {
        protected enum InvalidType
        {
            Error,
            Warning
        }

        // Temporary (session level) list of unordered returns
        protected List<int> CartIDs = new List<int>();

        // Custom Colours:
        // Validation
        string hex_error = "#FA6E59"; // Watermelon
        string hex_warning = "#FFB745"; // Orange Juice

        /*
        // Table header
        string hex_tab = "#4F4A45"; // Pewter
        string hex_title = "#CDAB81"; // Parchment

        // Table row
        string hex_row = "#D3D0DA"; // Smoke
        string hex_alt = "#B5B3BE"; // Greystone
        string hex_selected = "#9FB6CD"; // Mist
        string hex_add = "#50C878"; // Emerald

        // Standard text
        string hex_dark = "#323030"; // Cinder
        */

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only users with Receiving Role can use this page
            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    if (!User.IsInRole(SecurityRoles.Receiving))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }

                // On user authentication
                Session["Session_CartList"] = CartIDs;
                ManagePageDisplay(false);
            }
        }

        #region Page Controls
        protected void ManagePageDisplay(bool showAll)
        {
            ReceivingGrid.Visible = showAll;
            ReceivingGrid.Enabled = showAll;

            ReturnList.Visible = showAll;
            ReturnList.Enabled = showAll;

            ActionsPanel.Visible = showAll;
            ActionsPanel.Enabled = showAll;
        }

        protected void ReloadPageContent(int poID = 0)
        {
            // Clear return list
            CancelButton_Click(this, new EventArgs());

            CartIDs.Clear();
            Session["Session_CartList"] = CartIDs;
            ReloadReturnList(new UnorderedPurchaseItemCartController());

            // Clear force close text
            ForceCloseTextBox.Text = "";

            // Order state
            if (poID <= 0) // Closed
            {
                // Clear receiving grid
                ReceivingGrid.DataSource = null;
                ReceivingGrid.DataBind();

                // Refresh order list
                OrderList.SelectedIndex = -1;
                OrderList.DataBind();

                // Clear open order data
                PurchaseOrderID.Text = "";
                Number.Text = "";
                Vendor.Text = "";
                Phone.Text = "";

                // Hide controls
                ManagePageDisplay(false);
            }
            else // Open
            {
                PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                List<OpenOrderDetail> OpenOrderDetails = sysmgr.OpenOrderDetail_List(poID);

                // Refresh receiving grid
                ReceivingGrid.DataSource = OpenOrderDetails;
                ReceivingGrid.DataBind();

                // Show controls
                ManagePageDisplay(true);
            }
        }

        protected void ReloadReturnList(UnorderedPurchaseItemCartController sysmgr)
        {
            // Get list of current UPIC records for this session
            List<UnorderedPurchaseItemCart> UPICs = sysmgr.UPIC_ListByID(CartIDs);

            // Override default ODS
            ReturnList.DataSourceID = null;

            ReturnList.DataSource = UPICs;
            ReturnList.DataBind();

            // Revert text box validity color
            TextBox qtyTextBox = (ReturnList.InsertItem.FindControl("QuantityTextBox") as TextBox);
            SetTextBoxValidity(qtyTextBox, true);
        }

        protected void SetTextBoxValidity(TextBox textBox, bool isValid, InvalidType invalidType = InvalidType.Error)
        {
            if (isValid)
            {
                textBox.BackColor = System.Drawing.Color.White;
            }
            else
            {
                switch (invalidType)
                {
                    case InvalidType.Error:
                        textBox.BackColor = System.Drawing.ColorTranslator.FromHtml(hex_error);
                        break;

                    case InvalidType.Warning:
                        textBox.BackColor = System.Drawing.ColorTranslator.FromHtml(hex_warning);
                        break;
                }
            }
        }
        #endregion

        #region Page Events
        protected void OrderList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                // Note: POID is imbedded on each list row (with the CommandArgument parameter)
                int poID = int.Parse(e.CommandArgument.ToString());

                PurchaseOrderController sysmgr = new PurchaseOrderController();
                OpenOrder openOrder = sysmgr.OpenOrder_FindByID(poID);

                PurchaseOrderID.Text = openOrder.POID.ToString();
                Number.Text = openOrder.PONumber.ToString();
                Vendor.Text = openOrder.VendorName;
                Phone.Text = openOrder.VendorPhone;

                // Need to rebind data source before visibility enabled
                ReturnList.DataBind();

                // Load/Reload page content
                ReloadPageContent(poID);

                // Ensure insert template gets purchase order ID
                HiddenField insertPOID = ReturnList.InsertItem.FindControl("PurchaseOrderIDField") as HiddenField;
                insertPOID.Value = poID.ToString();

                // Highlight selected row
                OrderList.SelectedIndex = e.Item.DisplayIndex;
            }, 
            "Order Found", 
            "Order details successfully retrieved");
        }

        protected void InsertButton_Click(object sender, EventArgs e)
        {
            bool qtyIsValid = true;

            MessageUserControl.TryRun(() =>
            {
                UnorderedPurchaseItemCartController sysmgr = new UnorderedPurchaseItemCartController();
                UnorderedPurchaseItemCart upic = new UnorderedPurchaseItemCart();

                // Load data from insert row into a new UPIC instance
                upic.PurchaseOrderID = int.Parse(PurchaseOrderID.Text);
                upic.Description = (ReturnList.InsertItem.FindControl("DescriptionTextBox") as TextBox).Text;
                upic.VendorStockNumber = (ReturnList.InsertItem.FindControl("VendorStockNumberTextBox") as TextBox).Text;

                TextBox qtyTextBox = (ReturnList.InsertItem.FindControl("QuantityTextBox") as TextBox);
                int qty;

                // Validate user entry
                if (int.TryParse(qtyTextBox.Text, out qty))
                {
                    if (qty > 0)
                    {
                        upic.Quantity = qty;
                    }
                    else // Invalid entry
                    {
                        qtyIsValid = false;
                    }
                }
                else // Invalid entry
                {
                    qtyIsValid = false;
                }

                SetTextBoxValidity(qtyTextBox, qtyIsValid);

                // Only continue with insert if a valid qty is entered
                if (qtyIsValid)
                {
                    // Send UPIC to the BLL for insertion - Receive identity ID for record
                    int cartID = sysmgr.UPIC_Add(upic);

                    // Restore data from previous postbacks
                    CartIDs = Session["Session_CartList"] as List<int>;

                    // Add identity ID to the temporary list
                    CartIDs.Add(cartID);

                    // Save data to session for future postbacks
                    Session["Session_CartList"] = CartIDs;

                    // Refresh list
                    ReloadReturnList(sysmgr);
                }
            }, 
            "Return Added", 
            "Unordered return successfully recorded");

            // Specific validity message
            if (!qtyIsValid)
            {
                MessageUserControl.ShowInfo(
                    "Invalid Quantity", 
                    "Quantity is required to be greater than zero and a valid number");
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            (ReturnList.InsertItem.FindControl("DescriptionTextBox") as TextBox).Text = "";
            (ReturnList.InsertItem.FindControl("VendorStockNumberTextBox") as TextBox).Text = "";

            TextBox qtyTextBox = ReturnList.InsertItem.FindControl("QuantityTextBox") as TextBox;
            qtyTextBox.Text = "";
            SetTextBoxValidity(qtyTextBox, true);
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                UnorderedPurchaseItemCartController sysmgr = new UnorderedPurchaseItemCartController();

                // Retrieve cart ID imbedded on the row user is deleting
                LinkButton deleteButton = (LinkButton)sender;
                int cartID = int.Parse(deleteButton.CommandArgument.ToString());

                // Send cart ID to BLL for deletion
                sysmgr.UPIC_Delete(cartID);

                // Restore data from previous postbacks
                CartIDs = Session["Session_CartList"] as List<int>;

                // Remove cart ID from the temporary list
                CartIDs.Remove(cartID);

                // Save data to session for future postbacks
                Session["Session_CartList"] = CartIDs;

                // Refresh list
                ReloadReturnList(sysmgr);
            }, 
            "Return Removed", 
            "Unordered return permanently deleted");
        }

        protected void ReceiveButton_Click(object sender, EventArgs e)
        {
            bool qtyIsValid = true; // Tracks if any invalid receive or return quantities were entered
            bool reasonEntry = true; // Tracks if a reason is not entered for a returned quantity
            bool userEntry = false; // Tracks if the user has made any entry attempts

            MessageUserControl.TryRun(() =>
            {
                // Data to send to the BLL
                int poID = int.Parse(PurchaseOrderID.Text);
                List<OpenOrderDetail> OODs = new List<OpenOrderDetail>();
                List<int> UPICs = new List<int>();

                // Capture received and returned entries
                foreach (GridViewRow row in ReceivingGrid.Rows)
                {
                    TextBox qtyReceivedCtrl = row.FindControl("QtyReceived") as TextBox;
                    TextBox qtyReturnedCtrl = row.FindControl("QtyReturned") as TextBox;
                    TextBox returnReasonCtrl = row.FindControl("ReturnReason") as TextBox;

                    int qtyReceived;
                    int qtyReturned;
                    int qtyOutstanding = int.Parse((row.FindControl("QtyOutstandingLabel") as Label).Text);

                    // Validate received quantity
                    if (int.TryParse(qtyReceivedCtrl.Text, out qtyReceived))
                    {
                        userEntry = true;

                        if (qtyReceived > 0)
                        {
                            // User can not receive more than the outstanding quantity
                            if (qtyReceived > qtyOutstanding)
                            {
                                qtyIsValid = false;
                                SetTextBoxValidity(qtyReceivedCtrl, false);
                            }
                        }
                        else // Invalid entry
                        {
                            qtyIsValid = false;
                            SetTextBoxValidity(qtyReceivedCtrl, false);
                        }
                    }

                    // Validate returned quantity
                    if (int.TryParse(qtyReturnedCtrl.Text, out qtyReturned))
                    {
                        userEntry = true;

                        if (qtyReturned > 0)
                        {
                            // Return is required to have a reason entered
                            if (string.IsNullOrEmpty(returnReasonCtrl.Text))
                            {
                                reasonEntry = false;
                                SetTextBoxValidity(returnReasonCtrl, false, InvalidType.Warning);
                            }
                        }
                        else // Invalid entry
                        {
                            qtyIsValid = false;
                            SetTextBoxValidity(qtyReturnedCtrl, false);
                        }
                    }

                    // Only capture and hold open order detail if valid quantities are entered (or text fields are empty)
                    if (qtyIsValid && reasonEntry)
                    {
                        OpenOrderDetail openOrderDetail = new OpenOrderDetail();
                        openOrderDetail.POID = poID;
                        openOrderDetail.PODetailID = int.Parse((row.FindControl("PODetailIDLabel") as Label).Text);
                        openOrderDetail.StockItemID = int.Parse((row.FindControl("StockItemIDLabel") as Label).Text);
                        openOrderDetail.Description = (row.FindControl("DescriptionLabel") as Label).Text;
                        openOrderDetail.QtyOrdered = int.Parse((row.FindControl("QtyOrderedLabel") as Label).Text);
                        openOrderDetail.QtyOutstanding = qtyOutstanding;
                        openOrderDetail.QtyReceived = qtyReceived;
                        openOrderDetail.QtyReturned = qtyReturned;
                        openOrderDetail.ReturnReason = returnReasonCtrl.Text;

                        OODs.Add(openOrderDetail);
                    }
                }

                // Only continue with transaction if valid quantities are entered
                if (qtyIsValid && reasonEntry)
                {
                    // Load list with data from previous postbacks
                    UPICs = Session["Session_CartList"] as List<int>;

                    // Ensure the use has made entries (either received items, returns, or unordered items)
                    if (userEntry || UPICs.Count > 0)
                    {
                        userEntry = true;
                        
                        // Call BLL and send updated order details and unordered return cart IDs
                        OpenOrderController sysmgr = new OpenOrderController();
                        bool isClosed = sysmgr.OpenOrder_Receive(poID, OODs, UPICs);

                        // Reload page content
                        if (isClosed)
                        {
                            ReloadPageContent();
                        }
                        else
                        {
                            ReloadPageContent(poID);
                        }
                    }
                }
            }, 
            "Order Received", 
            "Purchase order was successfully processed");

            // External validation messages
            if (qtyIsValid == false)
            {
                MessageUserControl.ShowInfo(
                    "Invalid Quantities", 
                    "Receive and Return fields require a valid number (greater than zero & received qty less than the outstanding qty)");
            }
            else if (reasonEntry == false)
            {
                MessageUserControl.ShowInfo(
                    "Reason Field(s) Void", 
                    "Returned quantities require a reason entry");
            }
            else if (userEntry == false)
            {
                MessageUserControl.ShowInfo(
                    "No Entries", 
                    "No entries were made for receiving or returning");
            }
        }

        protected void ForceCloseButton_Click(object sender, EventArgs e)
        {
            bool reasonExists = true;

            MessageUserControl.TryRun(() =>
            {
                // Data to send to the BLL
                int poID = int.Parse(PurchaseOrderID.Text);
                string forceCloseReason = ForceCloseTextBox.Text;
                List<OpenOrderDetail> OODs = new List<OpenOrderDetail>();

                // Ensure a reason for force close is provided
                if (string.IsNullOrEmpty(forceCloseReason))
                {
                    reasonExists = false;
                }
                else
                {
                    // Pull stock item ID and outstanding qty from each detail row
                    foreach (GridViewRow row in ReceivingGrid.Rows)
                    {
                        OpenOrderDetail openOrderDetail = new OpenOrderDetail();
                        openOrderDetail.StockItemID = int.Parse((row.FindControl("StockItemIDLabel") as Label).Text);
                        openOrderDetail.QtyOutstanding = int.Parse((row.FindControl("QtyOutstandingLabel") as Label).Text);

                        OODs.Add(openOrderDetail);
                    }

                    // Call BLL and send force close information
                    OpenOrderController sysmgr = new OpenOrderController();
                    sysmgr.OpenOrder_ForceClose(poID, forceCloseReason, OODs);

                    // Reload page content
                    ReloadPageContent();
                }
            }, 
            "Order Closed", 
            "Purchase order was successfully closed");

            // External validation message
            if (!reasonExists)
            {
                MessageUserControl.ShowInfo(
                    "Reason Void",
                    "A reason is required for a force close of an order");
            }
        }
        #endregion
    }
}