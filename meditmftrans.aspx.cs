using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics
{
    public partial class meditmftrans : System.Web.UI.Page
    {
        public string FundHouse
        {
            get
            {
                return textboxFundHouse.Text.Trim();
            }
        }

        public string FundNameSelected
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return ddlFundName.Items[ddlFundName.SelectedIndex].Text;
            }
        }
        public string FundNameSelectedValue
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return ddlFundName.SelectedValue;
            }
        }

        public string FundHouseSelectedValue
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return ddlFundHouse.SelectedValue;
            }
        }

        public string FundHouseSelected
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return ddlFundHouse.Items[ddlFundHouse.SelectedIndex].Text;
            }
        }

        public string FundName
        {
            get
            {
                return textboxFundName.Text.Trim();
            }
        }

        public string SchemeCode
        {
            get
            {
                return textboxSchemeCode.Text.Trim();
            }
        }

        public string PurchaseDate
        {
            get
            {
                return System.Convert.ToDateTime(textboxPurchaseDate.Text.Trim()).ToShortDateString();
            }
        }
        public string PurchaseUnits
        {
            get
            {
                return textboxUnits.Text.Trim();
            }
        }
        public string PurchaseNAV
        {
            get
            {
                return textboxPurchaseNAV.Text.Trim();
            }
        }
        public string ValueAtCost
        {
            get
            {
                return textboxValueAtCost.Text.Trim();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PortfolioRowId"] != null)
            {
                //Master.Portfolio = Session["PortfolioName"].ToString();
                if (!IsPostBack)
                {
                    //ViewState["MFHistoryTable"] = null;
                    if (Request.QueryString.Count > 0)
                    {
                        string folderPath = Server.MapPath("~/mfdata/");
                        if (Session["TestDataFolderMF"] != null)
                        {
                            folderPath = Session["TestDataFolderMF"].ToString();
                        }

                        textboxFundHouse.Text = System.Web.HttpUtility.HtmlDecode(Request.QueryString["fundhouse"].ToString());
                        textboxFundName.Text = System.Web.HttpUtility.HtmlDecode(Request.QueryString["fundname"].ToString());
                        textboxPurchaseDate.Text = System.Convert.ToDateTime(Request.QueryString["purchasedate"].ToString()).ToString("yyyy-MM-dd");
                        textboxSchemeCode.Text = Request.QueryString["schemecode"].ToString();
                        textboxPurchaseNAV.Text = string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["purchasenav"].ToString()));
                        textboxUnits.Text = string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["purchaseunits"].ToString()));
                        textboxValueAtCost.Text = string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["valueatcost"].ToString()));

                        ddlFundHouse.Items.Clear();
                        //ddlFundHouse.Items.AddRange(MFAPI.listFundHouseMaster);
                        LoadFundHouseList();

                        //string mfCode = MFAPI.getMFCodefromFundHouseMaster(textboxFundHouse.Text);

                        //ddlFundHouse.SelectedValue = mfCode;
                        
                        ddlFundHouse.SelectedValue = DataManager.getFundHouseCode(textboxFundHouse.Text).ToString();

                        LoadFundList();

                        ddlFundName.SelectedValue = textboxSchemeCode.Text;
                        //DataTable mfHistoryTable = MFAPI.getHistoryNAV(folderPath, mfCode, textboxPurchaseDate.Text);

                        //if ((mfHistoryTable != null) && (mfHistoryTable.Rows.Count > 0))
                        //{

                        //    mfHistoryTable = mfHistoryTable.DefaultView.ToTable(true, new string[] { "SCHEME_NAME", "SCHEME_CODE", "NET_ASSET_VALUE" });
                        //    ViewState["MFHistoryTable"] = mfHistoryTable;

                        //    ddlFundName.DataTextField = "SCHEME_NAME";
                        //    ddlFundName.DataValueField = "SCHEME_NAME";
                        //    ddlFundName.DataSource = mfHistoryTable;
                        //    ddlFundName.DataBind();

                        //    ListItem li = new ListItem("-- Select New Fund --", "-1");
                        //    ddlFundName.Items.Insert(0, li);
                        //}
                    }
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noPortfolioName + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioName + "');", true);
                //Response.Redirect(".\\Default.aspx");
                Response.Redirect("~/Default.aspx");
            }

        }

        public void LoadFundHouseList()
        {
            DataTable fundHouseTable = DataManager.getFundHouseTable();
            if ((fundHouseTable != null) && (fundHouseTable.Rows.Count > 0))
            {
                // Columns - FUNDHOUSECODE, NAME
                ddlFundHouse.DataTextField = "NAME";
                ddlFundHouse.DataValueField = "FUNDHOUSECODE";
                ddlFundHouse.DataSource = fundHouseTable;
                ddlFundHouse.DataBind();
            }
        }

        public bool LoadFundList()
        {
            bool breturn = false;

            DataTable mfSchemeTable = DataManager.getSchemesTable(fundhousecode: System.Convert.ToInt32(FundHouseSelectedValue));
            if ((mfSchemeTable != null) && (mfSchemeTable.Rows.Count > 0))
            {
                //columns... SCHEME_TYPE.ID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME
                ddlFundName.DataTextField = "SCHEMENAME";
                ddlFundName.DataValueField = "SCHEMECODE";
                ddlFundName.DataSource = mfSchemeTable;
                ddlFundName.DataBind();
                ListItem li = new ListItem("-- Select Fund Name --", "-1");
                ddlFundName.Items.Insert(0, li);
                breturn = true;
            }
            return breturn;
        }
        protected void textboxUnits_TextChanged(object sender, EventArgs e)
        {
            if (textboxPurchaseNAV.Text.Length > 0 && textboxUnits.Text.Length > 0)
            {
                double purchaseNAV = System.Convert.ToDouble(textboxPurchaseNAV.Text);
                double purchaseUnits = System.Convert.ToDouble(textboxUnits.Text);

                double valueatcost = (purchaseNAV * purchaseUnits);

                textboxValueAtCost.Text = System.Convert.ToString(valueatcost);
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Enter valid NAV and Unit values');", true);
        }

        protected void textboxPurchaseNAV_TextChanged(object sender, EventArgs e)
        {
            if (textboxPurchaseNAV.Text.Length > 0 && textboxUnits.Text.Length > 0)
            {
                double purchaseNAV = System.Convert.ToDouble(textboxPurchaseNAV.Text);
                double purchaseUnits = System.Convert.ToDouble(textboxUnits.Text);

                double valueatcost = (purchaseNAV * purchaseUnits);

                textboxValueAtCost.Text = System.Convert.ToString(valueatcost);
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Enter valid NAV and Unit values');", true);
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            //if (ViewState["MFHistoryTable"] != null)
            //{
            //    ((DataTable)ViewState["MFHistoryTable"]).Clear();
            //    ViewState["MFHistoryTable"] = null;
            //}
            //if (this.MasterPageFile.Contains("Site.Master"))
            //    Response.Redirect("~/openportfolio.aspx");
            //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
            //    Response.Redirect("~/mopenportfolio.aspx");
            //else
            Response.Redirect("~/mopenportfolioMF.aspx");
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            bool breturn = false;
            if (textboxPurchaseDate.Text.Length > 0 && textboxPurchaseNAV.Text.Length > 0 &&
                    textboxUnits.Text.Length > 0 && textboxValueAtCost.Text.Length > 0)

            {
                textboxPurchaseNAV_TextChanged(null, null);
                //Server.Transfer("~/openportfolio.aspx");
                try
                {
                    string portfolioRowId = Request.QueryString["portfoliorowid"].ToString();

                    breturn = DataManager.updateTransaction(Session["emailid"].ToString(), Session["ShortPortfolioNameMF"].ToString(), portfolioRowId,
                        Request.QueryString["schemecode"].ToString(),
                        Request.QueryString["purchasedate"].ToString(),
                        string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["purchasenav"].ToString())),
                        string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["purchaseunits"].ToString())),
                        string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["valueatcost"].ToString())),
                        FundNameSelectedValue, PurchaseDate,
                        string.Format("{0:0.0000}", System.Convert.ToDouble(PurchaseNAV)),
                        string.Format("{0:0.0000}", System.Convert.ToDouble(PurchaseUnits)),
                        string.Format("{0:0.0000}", System.Convert.ToDouble(ValueAtCost)));

                    //breturn = MFAPI.updateTransaction(Session["PortfolioNameMF"].ToString(), Request.QueryString["fundhouse"].ToString(),
                    //    Request.QueryString["fundname"].ToString(), Request.QueryString["schemecode"].ToString(),
                    //    Request.QueryString["purchasedate"].ToString(),
                    //    string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["purchasenav"].ToString())),
                    //    string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["purchaseunits"].ToString())),
                    //    string.Format("{0:0.0000}", System.Convert.ToDouble(Request.QueryString["valueatcost"].ToString())),
                    //    FundHouseSelected,FundName, PurchaseDate,
                    //    string.Format("{0:0.0000}", System.Convert.ToDouble(PurchaseNAV)),
                    //    string.Format("{0:0.0000}", System.Convert.ToDouble(PurchaseUnits)),
                    //    string.Format("{0:0.0000}", System.Convert.ToDouble(ValueAtCost)));
                }
                catch (Exception ex)
                {
                    //Response.Write("<script language=javascript>alert('" + msg + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + ex.Message + "');", true);
                }
                if (breturn)
                {
                    //if (this.MasterPageFile.Contains("Site.Master"))
                    //    Response.Redirect("~/openportfolio.aspx");
                    //else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                    //    Response.Redirect("~/mopenportfolio.aspx");
                    //else
                    Response.Redirect("~/mopenportfolioMF.aspx");
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('Error while updating the transaction. Please try again or hit back.')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.errorEditScript + "');", true);
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('All fields are mandatory.')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.errorAllFieldsMandatory + "');", true);

            }
        }

        protected void ddlFundName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundNameSelectedValue != "-1")
            {
                DataTable mfHistoryTable = null;

                textboxFundName.Text = FundNameSelected;
                textboxSchemeCode.Text = FundNameSelectedValue;
                mfHistoryTable = DataManager.getNAVRecordsTable(System.Convert.ToInt32(FundNameSelectedValue), fromDate: PurchaseDate, toDate: PurchaseDate);
                if ((mfHistoryTable != null) && (mfHistoryTable.Rows.Count > 0))
                {
                    textboxPurchaseNAV.Text = mfHistoryTable.DefaultView[0]["NET_ASSET_VALUE"].ToString();
                    textboxPurchaseNAV_TextChanged(null, null);
                }

                //if (ViewState["MFHistoryTable"] != null)
                //{
                //    DataTable mfHistoryTable = (DataTable)ViewState["MFHistoryTable"];
                //    mfHistoryTable.DefaultView.RowFilter = "SCHEME_NAME = '" + FundNameSelected + "'";
                //    if (mfHistoryTable.DefaultView.Count > 0)
                //    {
                //        textboxFundName.Text = FundNameSelected;
                //        textboxSchemeCode.Text = mfHistoryTable.DefaultView[0]["SCHEME_CODE"].ToString();
                //        textboxPurchaseNAV.Text = mfHistoryTable.DefaultView[0]["NET_ASSET_VALUE"].ToString();
                //        textboxPurchaseNAV_TextChanged(null, null);
                //    }
                //    else
                //    {
                //        textboxSchemeCode.Text = "";
                //        textboxPurchaseNAV.Text = "";
                //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('No data available for selected fund name. Please select another fund');", true);
                //    }

                //}
            }
        }

        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundHouseSelectedValue != "-1")
            {
                textboxFundHouse.Text = FundHouseSelected;


                //string folderPath = Server.MapPath("~/mfdata/");
                //if (Session["TestDataFolderMF"] != null)
                //{
                //    folderPath = Session["TestDataFolderMF"].ToString();
                //}

                ddlFundName.Items.Clear();
                if(LoadFundList() == true)
                {
                    textboxFundName.Text = "";
                    textboxSchemeCode.Text = "";
                    textboxPurchaseNAV.Text = "";
                    textboxValueAtCost.Text = "";
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('No data available for selected fund house. Please select another fund house');", true);
                    textboxFundHouse.Text = "";
                    textboxFundName.Text = "";
                    textboxSchemeCode.Text = "";
                    textboxPurchaseNAV.Text = "";
                    textboxValueAtCost.Text = "";
                }

                //ViewState["MFHistoryTable"] = null;
                //DataTable mfHistoryTable = MFAPI.getHistoryNAV(folderPath, FundHouseSelectedValue, PurchaseDate);
                //if ((mfHistoryTable != null) && (mfHistoryTable.Rows.Count > 0))
                //{
                //    mfHistoryTable = mfHistoryTable.DefaultView.ToTable(true, new string[] { "SCHEME_NAME", "SCHEME_CODE", "NET_ASSET_VALUE" });
                //    ViewState["MFHistoryTable"] = mfHistoryTable;

                //    ddlFundName.DataTextField = "SCHEME_NAME";
                //    ddlFundName.DataValueField = "SCHEME_NAME";
                //    ddlFundName.DataSource = mfHistoryTable;
                //    ddlFundName.DataBind();

                //    ListItem li = new ListItem("-- Select New Fund --", "-1");
                //    ddlFundName.Items.Insert(0, li);

                //    textboxFundName.Text = "";
                //    textboxSchemeCode.Text = "";
                //    textboxPurchaseNAV.Text = "";
                //    textboxValueAtCost.Text = "";
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('No data available for selected fund house. Please select another fund house');", true);
                //    textboxFundHouse.Text = "";
                //    textboxFundName.Text = "";
                //    textboxSchemeCode.Text = "";
                //    textboxPurchaseNAV.Text = "";
                //    textboxValueAtCost.Text = "";
                //}
            }
        }
    }
}