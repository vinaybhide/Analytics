using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Analytics
{
    public partial class openportfolio : System.Web.UI.Page
    {
        string strPreviousRowID = string.Empty;
        // To keep track the Index of Group Total    
        int intSubTotalIndex = 1;
        // To temporarily store Sub Total    
        double dblSubTotalQuantity = 0;
        double dblSubTotalCost = 0;
        double dblSubTotalValue = 0;
        // To temporarily store Grand Total    
        double dblGrandTotalQuantity = 0;
        double dblGrandTotalCost = 0;
        double dblGrandTotalValue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["EmailId"] != null)
            //{
            //    Master.UserID = Session["emailid"].ToString();
            //}

            string fileName = "";
            if (Session["EmailId"] != null)
            {
                if (Session["PortfolioName"] != null)
                {
                    //Master.Portfolio = Session["PortfolioName"].ToString();
                    fileName = Session["PortfolioName"].ToString();
                    if (!IsPostBack)
                    {
                        ViewState["FetchedData"] = null;
                        ViewState["SelectedIndex"] = null;
                    }
                    openPortfolio(fileName);
                }
                else
                {
                    //Response.Redirect(".\\Default.aspx");
                    //Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioNameToOpen + "');", true);
                    Response.Redirect("~/mselectportfolio.aspx");
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }

        }

        protected void grdViewOrders_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd = false;
            bool IsGrandTotalRowNeedtoAdd = false;
            GridView gridViewPortfolio = (GridView)sender;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ScriptID") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "ScriptID").ToString())
                    IsSubTotalRowNeedToAdd = true;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ScriptID") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }
            #region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "ScriptID") != null))
            {
                //GridView gridViewPortfolio= (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "Script : " + DataBinder.Eval(e.Row.DataItem, "ScriptID").ToString();
                cell.ColumnSpan = 9;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
            }
            #endregion
            if (IsSubTotalRowNeedToAdd)
            {
                #region Adding Sub Total Row
                //GridView GridViewPortfolio = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                cell.Text = "Sub Total";
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 4;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Quantity Column            
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalQuantity);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding empty commisionpaid col
                cell = new TableCell();
                cell.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Cost col
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalCost);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding empty price col
                cell = new TableCell();
                cell.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Value Column         
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblSubTotalValue);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle"; row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                #endregion
                #region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "ScriptID") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "Script : " + DataBinder.Eval(e.Row.DataItem, "ScriptID").ToString();
                    cell.ColumnSpan = 9;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                }
                #endregion
                #region Reseting the Sub Total Variables
                dblSubTotalQuantity = 0;
                dblSubTotalCost = 0;
                dblSubTotalValue = 0;
                #endregion
            }
            if (IsGrandTotalRowNeedtoAdd)
            {
                #region Grand Total Row
                //GridView gridViewPortfolio = (GridView)sender;
                // Creating a Row      
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cell = new TableCell();
                cell.Text = "Grand Total";
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 6;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Unit Price Column          
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalCost);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding empty price col
                cell = new TableCell();
                cell.Text = "";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Quantity Column           
                cell = new TableCell();
                cell.Text = string.Format("{0:0.00}", dblGrandTotalValue);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid     
                gridViewPortfolio.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
                #endregion
            }
        }

        /// <summary>    
        /// Event fires when data binds to each row   
        /// Used for calculating Group Total     
        /// </summary>   
        /// /// <param name="sender"></param>    
        /// <param name="e"></param>    
        protected void grdViewOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "ScriptID").ToString();
                double dblQuantity = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "PurchaseQty").ToString());
                double dblCost = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "CostofInvestment").ToString());
                double dblValue = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "CurrentValue").ToString());
                // Cumulating Sub Total            
                dblSubTotalQuantity += dblQuantity;
                dblSubTotalCost += dblCost;
                dblSubTotalValue += dblValue;
                // Cumulating Grand Total           
                dblGrandTotalQuantity += dblQuantity;
                dblGrandTotalCost += dblCost;
                dblGrandTotalValue += dblValue;

                // This is for cumulating the values  
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView gridViewPortfolio = (GridView)sender; ;

                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ddd'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewPortfolio, "Select$" + e.Row.RowIndex);
                }
            }
        }

        protected void grdViewOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string scriptName = GridViewPortfolio.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                Session["ScriptName"] = GridViewPortfolio.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                Session["CompanyName"] = GridViewPortfolio.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[0].Text;
                string purchaseDate = GridViewPortfolio.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[2].Text;
                lblScript.Text = scriptName;
                lblDate.Text = purchaseDate;
                //ViewState["SelectedIndex"] = e.CommandArgument.ToString();
                //GridViewPortfolio.SelectedIndex = System.Convert.ToInt32(e.CommandArgument.ToString());
            }
        }

        protected void GridViewPortfolio_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ScriptName"] = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
            Session["CompanyName"] = GridViewPortfolio.SelectedRow.Cells[0].Text.ToString();
            ViewState["SelectedIndex"] = GridViewPortfolio.SelectedIndex;
        }

        public void openPortfolio(string portfolioFileName)
        {
            bool bIsTestOn = true;
            DataTable dt;
            int selectedrow = -1;

            string folderPath = Server.MapPath("~/scriptdata/");
            try
            {
                if (Session["IsTestOn"] != null)
                {
                    bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                }

                if (Session["TestDataFolder"] != null)
                {
                    folderPath = Session["TestDataFolder"].ToString();
                }

                if ((ViewState["FetchedData"] == null) || (((DataTable)ViewState["FetchedData"]).Rows.Count == 0))
                {
                    dt = StockApi.getPortfolioTable(folderPath, portfolioFileName, true, bIsTestOn, apiKey: Session["ApiKey"].ToString());
                }
                else
                {
                    dt = (DataTable)ViewState["FetchedData"];
                    if (ViewState["SelectedIndex"] != null)
                    {
                        selectedrow = System.Convert.ToInt32(ViewState["SelectedIndex"].ToString());
                    }
                }
                GridViewPortfolio.DataSource = dt;
                GridViewPortfolio.DataBind();

                //if(selectedrow >= 0)
                //{
                //    GridViewPortfolio.SelectedIndex = selectedrow;
                //}

                XmlDocument xmldoc = new XmlDocument();
                //FileStream fs = new FileStream(Server.MapPath(".\\data\\demo_portfolio.xml"), FileMode.Open, FileAccess.Read);

                XmlNode xmlnode;

                //xmldoc.Load(Server.MapPath(".\\data\\demo_portfolio.xml"));
                xmldoc.Load(portfolioFileName);
                xmlnode = xmldoc.ChildNodes[0];
                TreeViewPortfolio.Nodes.Clear();
                TreeViewPortfolio.Nodes.Add(new TreeNode(xmldoc.DocumentElement.Name));
                TreeNode tNode;
                tNode = TreeViewPortfolio.Nodes[0];
                AddNode(xmlnode, tNode);
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while opening portfolio: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while opening portfolio:" + ex.Message + "');", true);
            }
        }
        public void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xScriptNode, xRowNode;
            TreeNode tNode;
            XmlNodeList nodeList, rowList, fieldList;
            int i, j, k;
            string rowValue;
            if (inXmlNode.HasChildNodes)
            {
                //this will give all script nodes
                nodeList = inXmlNode.ChildNodes;
                //Get the list of <script>
                for (i = 0; i < nodeList.Count; i++)
                {
                    //get the current <script> node
                    xScriptNode = inXmlNode.ChildNodes[i];
                    //add the current script to tree

                    if (xScriptNode["name"].HasAttribute("companyname"))
                    {
                        inTreeNode.ChildNodes.Add(new TreeNode((xScriptNode["name"]).Attributes["companyname"].Value + ": " + xScriptNode["name"].InnerText));
                    }
                    else
                    {
                        inTreeNode.ChildNodes.Add(new TreeNode(xScriptNode["name"].Name.ToUpperInvariant() + ": " + xScriptNode["name"].InnerText));
                    }
                    //this will give handle of the current <script> in the tree
                    tNode = inTreeNode.ChildNodes[i];

                    //now get the <row> under this <script>
                    if (xScriptNode.HasChildNodes)
                    {
                        //get the <row> list under this <script>
                        rowList = xScriptNode.ChildNodes;

                        for (j = 0; j < rowList.Count; j++)
                        {
                            xRowNode = rowList[j];

                            if ((xRowNode.Name.ToUpper().Equals("NAME") == false) && (xRowNode.HasChildNodes))
                            {
                                fieldList = xRowNode.ChildNodes;
                                rowValue = "";
                                for (k = 0; k < fieldList.Count; k++)
                                {
                                    rowValue += (fieldList[k].Name.ToUpperInvariant() + ": " + fieldList[k].InnerText + "\t");
                                }
                                rowValue.Trim();
                                tNode.ChildNodes.Add(new TreeNode(rowValue));
                                //tNode = inTreeNode.ChildNodes[i];
                            }
                        }
                    }
                }
            }
        }
        protected void ButtonAddNew_Click(object sender, EventArgs e)
        {
            //ResponseHelper.Redirect(Response, "\\addnewscript.aspx", "_self", "menubar=0,scrollbars=1,width=780,height=900,top=10");
            //ResponseHelper.Redirect(Response, ".\\addnewscript.aspx", "", "");
            if (this.MasterPageFile.Contains("Site.Master"))
                //Response.Redirect(".\\addnewscript.aspx");
                Response.Redirect("~/addnewscript.aspx");
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                Response.Redirect("~/maddnewscript.aspx");
            else
                Response.Redirect("~/maddnewscript.aspx");
        }
        protected void buttonDeleteSelectedScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridViewPortfolio.SelectedRow != null)
                {
                    string companyname = GridViewPortfolio.SelectedRow.Cells[0].Text.ToString();
                    string symbol = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
                    string date = GridViewPortfolio.SelectedRow.Cells[2].Text.ToString();
                    string price = GridViewPortfolio.SelectedRow.Cells[3].Text.ToString();
                    string qty = GridViewPortfolio.SelectedRow.Cells[4].Text.ToString();
                    string commission = GridViewPortfolio.SelectedRow.Cells[5].Text.ToString();
                    string cost = GridViewPortfolio.SelectedRow.Cells[6].Text.ToString();
                    string filename = Session["PortfolioName"].ToString();
                    StockApi.deleteNode(filename, symbol, price, date, qty, commission, cost);
                    //openPortfolio(filename);
                    if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/openportfolio.aspx");
                    else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        Response.Redirect("~/mopenportfolio.aspx");
                    else
                        Response.Redirect("~/mopenportfolio.aspx");

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTxnSelected + "');", true);
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while delering script entry: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while deleting script:" + ex.Message + "');", true);
            }

        }

        protected void buttonGetQuote_Click(object sender, EventArgs e)
        {
            if(this.MasterPageFile.Contains("Site.Master"))
                //Response.Redirect(".\\getquoteadd.aspx");
                Response.Redirect("~/getquoteadd.aspx");
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                Response.Redirect("~/mgetquoteadd.aspx");
            else
                Response.Redirect("~/mgetquoteadd.aspx");
        }

        protected void buttonValuation_Click(object sender, EventArgs e)
        {

            //string url = "\\portfoliovaluation.aspx" + "?";
            string url = "~/portfoliovaluation.aspx" + "?";

            if (this.MasterPageFile.Contains("Site.Master"))
            {
                url += "parent=openportfolio.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
            }
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            {
                url += "parent=mopenportfolio.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0, left=0");
            }

            //ResponseHelper.Redirect(Response, "\\portfolioValuation.aspx", "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridViewPortfolio.SelectedRow != null)
                {
                    string companyname = GridViewPortfolio.SelectedRow.Cells[0].Text.ToString();
                    string symbol = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
                    string date = GridViewPortfolio.SelectedRow.Cells[2].Text.ToString();
                    string price = GridViewPortfolio.SelectedRow.Cells[3].Text.ToString();
                    string qty = GridViewPortfolio.SelectedRow.Cells[4].Text.ToString();
                    string commission = GridViewPortfolio.SelectedRow.Cells[5].Text.ToString();
                    string cost = GridViewPortfolio.SelectedRow.Cells[6].Text.ToString();
                    string filename = Session["PortfolioName"].ToString();

                    if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/editscript.aspx?symbol=" + symbol + "&companyname=" + Server.UrlEncode(companyname) + "&price=" + price + "&date=" + date 
                            + "&qty=" + qty + "&comission=" + commission + "&cost=" + cost);
                    else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        Response.Redirect("~/meditscript.aspx?symbol=" + symbol + "&companyname=" + Server.UrlEncode(companyname) + "&price=" + price + "&date=" + date
                            + "&qty=" + qty + "&comission=" + commission + "&cost=" + cost);
                    else
                        Response.Redirect("~/meditscript.aspx?symbol=" + symbol + "&companyname=" + Server.UrlEncode(companyname) + "&price=" + price + "&date=" + date
                            + "&qty=" + qty + "&comission=" + commission + "&cost=" + cost);


                    //StockApi.deleteNode(filename, symbol, price, date, qty, commission, cost);
                    ////openPortfolio(filename);
                    //if (this.MasterPageFile.Contains("Site.Master"))
                    //    Response.Redirect("~/openportfolio.aspx");
                    //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    //    Response.Redirect("~/mopenportfolio.aspx");
                    //else
                    //    Response.Redirect("~/mopenportfolio.aspx");

                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language=javascript>alert('Exception while delering script entry: " + ex.Message + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception while updating the script:" + ex.Message + "');", true);
            }
        }
    }
}