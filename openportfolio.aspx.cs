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
                        openPortfolio(fileName);
                    }
                }
                else
                {
                    //Response.Redirect(".\\Default.aspx");
                    Response.Write("<script language=javascript>alert('" + common.noPortfolioNameToOpen + "')</script>");
                    Response.Redirect("~/selectportfolio.aspx");
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Response.Redirect("~/Default.aspx");
            }

        }
        protected void GridViewPortfolio_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ScriptName"] = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
        }
        public void openPortfolio(string portfolioFileName)
        {
            bool bIsTestOn = true;
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

                DataTable dt = StockApi.getPortfolioTable(folderPath, portfolioFileName, true, bIsTestOn, apiKey: Session["ApiKey"].ToString());
                GridViewPortfolio.DataSource = dt;
                GridViewPortfolio.DataBind();


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
                Response.Write("<script language=javascript>alert('Exception while opening portfolio: " + ex.Message + "')</script>");
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
                    inTreeNode.ChildNodes.Add(new TreeNode(xScriptNode["name"].Name.ToUpperInvariant() + ": " + xScriptNode["name"].InnerText));

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
                Response.Redirect("~/addnewscript.aspx");
        }
        protected void buttonDeleteSelectedScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridViewPortfolio.SelectedRow != null)
                {
                    string symbol = GridViewPortfolio.SelectedRow.Cells[1].Text.ToString();
                    string date = GridViewPortfolio.SelectedRow.Cells[2].Text.ToString();
                    string price = GridViewPortfolio.SelectedRow.Cells[3].Text.ToString();
                    string qty = GridViewPortfolio.SelectedRow.Cells[4].Text.ToString();
                    string commission = GridViewPortfolio.SelectedRow.Cells[5].Text.ToString();
                    string cost = GridViewPortfolio.SelectedRow.Cells[6].Text.ToString();
                    string filename = Session["PortfolioName"].ToString();
                    StockApi.deleteNode(filename, symbol, price, date, qty, commission, cost);
                    openPortfolio(filename);
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Exception while delering script entry: " + ex.Message + "')</script>");
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
                Response.Redirect("~/getquoteadd.aspx");
        }

        protected void buttonValuation_Click(object sender, EventArgs e)
        {

            //string url = "\\portfoliovaluation.aspx" + "?";
            string url = "~/portfoliovaluation.aspx" + "?";

            if (this.MasterPageFile.Contains("Site.Master"))
            {
                url += "parent=openportfolio.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
            }
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            {
                url += "parent=mopenportfolio.aspx";
                ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
            }

            //ResponseHelper.Redirect(Response, "\\portfolioValuation.aspx", "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
        }
    }
}