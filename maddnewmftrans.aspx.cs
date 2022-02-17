using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics
{
    public partial class maddnewmftrans : System.Web.UI.Page
    {
        public bool SIPEnabled
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return checkboxSIPTrans.Checked;
            }
        }

        public string FromDate
        {
            get
            {
                //return textboxFundName.Text.Trim();
                if (textboxPurchaseDate.Text.Length > 0)
                {
                    return System.Convert.ToDateTime(textboxPurchaseDate.Text).ToShortDateString();
                }
                else
                {
                    return null;
                }
            }
        }
        public string SIPEndDate
        {
            get
            {
                //return textboxFundName.Text.Trim();
                if (SIPEnabled)
                {
                    if (textboxEndDate.Text.Length > 0)
                    {
                        return System.Convert.ToDateTime(textboxEndDate.Text).ToShortDateString();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
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

        public string FundHouseSelectedValue
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return ddlFundHouse.SelectedValue;
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

        public string SchemeCode
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return textboxSchemeCode.Text;
            }
        }
        public string PurchaseNAV
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return textboxPurchaseNAV.Text;
            }
        }
        public string SIPFrequency
        {
            get
            {
                return ddlSIPFrequency.SelectedValue;
            }
        }
        public string SIPAmount
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return textboxSIPAmt.Text.Trim();
            }
        }

        public string PurchaseUnits
        {
            get
            {
                return textboxUnits.Text.Trim();
            }
        }
        public string ValueAtCost
        {
            get
            {
                return textboxValueAtCost.Text;
            }
        }

        public string SIPDayOfMonth
        {
            get
            {
                return ddlDayOfMonth.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["PortfolioNameMF"] != null)
            //if(Session["MFPORTFOLIONAME"] != null)
            if (Session["MFPORTFOLIOMASTERROWID"] != null)
            {
                if (!IsPostBack)
                {
                    //ViewState["MFHistoryTable"] = null;
                    ViewState["FUNDLIST"] = null;
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

        protected void checkboxSIPTrans_CheckedChanged(object sender, EventArgs e)
        {

            if (SIPEnabled == true)
            {
                labelPurchaseDate.Text = "SIP Start Date:";
                textboxEndDate.ReadOnly = false;
                textboxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                ddlSIPFrequency.Enabled = true;
                ddlDayOfMonth.Enabled = true;
                textboxSIPAmt.ReadOnly = false;
                textboxSIPAmt.Text = "";

                textboxUnits.Text = "";
                textboxUnits.ReadOnly = true;
                textboxValueAtCost.Text = "";
                textboxValueAtCost.ReadOnly = true;
                textboxPurchaseNAV.Text = "";
                textboxPurchaseNAV.ReadOnly = true;
            }
            else
            {
                labelPurchaseDate.Text = "Purchase Date:";
                //textboxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                textboxEndDate.Text = "";
                textboxEndDate.ReadOnly = true;
                ddlSIPFrequency.Enabled = false;
                ddlDayOfMonth.Enabled = false;
                textboxSIPAmt.Text = "";
                textboxSIPAmt.ReadOnly = false;
                textboxUnits.Text = "";
                textboxUnits.ReadOnly = false;
                textboxValueAtCost.Text = "";
                textboxValueAtCost.ReadOnly = true;
                textboxPurchaseNAV.Text = "";
                textboxPurchaseNAV.ReadOnly = true;
            }

        }

        public void LoadFundHouseList()
        {
            DataManager dataMgr = new DataManager();
            DataTable fundHouseTable = dataMgr.getFundHouseTable();
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
            ViewState["FUNDLIST"] = null;
            DataManager dataMgr = new DataManager();
            DataTable mfSchemeTable = dataMgr.getSchemesTable(fundhousecode: System.Convert.ToInt32(FundHouseSelectedValue));
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
                ViewState["FUNDLIST"] = mfSchemeTable;
            }
            return breturn;
        }
        protected void textboxPurchaseDate_TextChanged(object sender, EventArgs e)
        {
            if (SIPEnabled == true)
            {
                labelPurchaseDate.Text = "SIP Start Date:";
                textboxEndDate.ReadOnly = false;
                textboxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                ddlSIPFrequency.Enabled = true;
                ddlDayOfMonth.Enabled = true;
                textboxSIPAmt.ReadOnly = false;
                textboxUnits.Text = "";
                textboxUnits.ReadOnly = true;
                textboxValueAtCost.Text = "";
                textboxValueAtCost.ReadOnly = true;
                textboxPurchaseNAV.Text = "";
                textboxPurchaseNAV.ReadOnly = true;
            }
            else
            {
                labelPurchaseDate.Text = "Purchase Date:";
                //textboxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                textboxEndDate.Text = "";
                textboxEndDate.ReadOnly = true;
                ddlSIPFrequency.Enabled = false;
                ddlDayOfMonth.Enabled = false;
                //textboxSIPAmt.Text = "";
                textboxSIPAmt.ReadOnly = false;
                textboxUnits.Text = "";
                textboxUnits.ReadOnly = false;
                textboxValueAtCost.Text = "";
                textboxValueAtCost.ReadOnly = true;
                textboxPurchaseNAV.Text = "";
                textboxPurchaseNAV.ReadOnly = true;

            }


            //string folderPath = Server.MapPath("~/mfdata/");
            //if (Session["TestDataFolderMF"] != null)
            //{
            //    folderPath = Session["TestDataFolderMF"].ToString();
            //}

            ddlFundHouse.Enabled = true;
            ddlFundHouse.Items.Clear();
            //ddlFundHouse.Items.AddRange(MFAPI.listFundHouseMaster);
            LoadFundHouseList();

            textboxSelectedFundHouse.Text = "";
            ddlFundName.Items.Clear();
            ddlFundName.Enabled = false;
            textboxSelectedFundName.Text = "";
            textboxSchemeCode.Text = "";
        }

        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundHouseSelectedValue != "-1")
            {
                textboxSelectedFundHouse.Text = FundHouseSelected;

                //string folderPath = Server.MapPath("~/mfdata/");
                //if (Session["TestDataFolderMF"] != null)
                //{
                //    folderPath = Session["TestDataFolderMF"].ToString();
                //}

                //ddlFundName.Enabled = true;
                //ddlFundName.Items.Clear();
                //ViewState["MFHistoryTable"] = null;

                //DataTable mfHistoryTable = MFAPI.getHistoryNAV(folderPath, FundHouseSelectedValue, FromDate);

                //if ((mfHistoryTable != null) && (mfHistoryTable.Rows.Count > 0))
                //{

                //    mfHistoryTable = mfHistoryTable.DefaultView.ToTable(true, new string[] { "SCHEME_NAME", "SCHEME_CODE", "NET_ASSET_VALUE" });
                //    ViewState["MFHistoryTable"] = mfHistoryTable;

                //    ddlFundName.DataTextField = "SCHEME_NAME";
                //    ddlFundName.DataValueField = "SCHEME_NAME";
                //    ddlFundName.DataSource = mfHistoryTable;
                //    ddlFundName.DataBind();

                //    ListItem li = new ListItem("-- Select Fund Name --", "-1");
                //    ddlFundName.Items.Insert(0, li);
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Fund data not found for selected fund house. Please select another fund house.');", true);
                //}

                ddlFundName.Enabled = true;
                ddlFundName.Items.Clear();
                textboxSchemeCode.Text = "";

                if (LoadFundList() == false)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Fund data not found for selected fund house. Please select another fund house.');", true);
                }

                textboxSelectedFundName.Text = "";
                textboxSchemeCode.Text = "";
                textboxPurchaseNAV.Text = "";
                textboxSIPAmt.Text = "";
                textboxUnits.Text = "";
                textboxValueAtCost.Text = "";
            }
            else
            {
                textboxSelectedFundHouse.Text = "";
                textboxSelectedFundName.Text = "";
                textboxSchemeCode.Text = "";
                textboxPurchaseNAV.Text = "";
                textboxSIPAmt.Text = "";
                textboxUnits.Text = "";
                textboxValueAtCost.Text = "";
            }
        }


        protected void ddlFundName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundNameSelectedValue != "-1")
            {
                DataTable mfHistoryTable = null;

                textboxSelectedFundName.Text = FundNameSelected;
                textboxSchemeCode.Text = FundNameSelectedValue;
                DataManager dataMgr = new DataManager();
                mfHistoryTable = dataMgr.getNAVRecordsTable(System.Convert.ToInt32(FundNameSelectedValue), fromDate: FromDate, toDate: FromDate);

                if ((mfHistoryTable != null) && (mfHistoryTable.Rows.Count > 0))
                {
                    if (SIPEnabled == false)
                    {
                        textboxPurchaseNAV.Text = mfHistoryTable.Rows[0]["NET_ASSET_VALUE"].ToString();
                    }

                }
                else
                {
                    textboxSelectedFundName.Text = "";
                    textboxSchemeCode.Text = "";
                    textboxPurchaseNAV.Text = "";
                }

                //if (ViewState["MFHistoryTable"] != null)
                //{
                //    mfHistoryTable = (DataTable)ViewState["MFHistoryTable"];
                //    mfHistoryTable.DefaultView.RowFilter = "SCHEME_NAME = '" + FundNameSelected + "'";
                //    if (mfHistoryTable.DefaultView.Count > 0)
                //    {
                //        //textboxSchemeCode.Text = mfHistoryTable.Rows[0]["SCHEME_CODE"].ToString();
                //        textboxSchemeCode.Text = mfHistoryTable.DefaultView[0]["SCHEME_CODE"].ToString();
                //        if (SIPEnabled == false)
                //        {
                //            //textboxPurchaseNAV.Text = mfHistoryTable.Rows[0]["NET_ASSET_VALUE"].ToString();
                //            textboxPurchaseNAV.Text = mfHistoryTable.DefaultView[0]["NET_ASSET_VALUE"].ToString();
                //        }
                //    }
                //    else
                //    {
                //        textboxSchemeCode.Text = "";
                //        textboxPurchaseNAV.Text = "";
                //    }
                //}

            }
            else
            {
                textboxSelectedFundName.Text = "";
                textboxSchemeCode.Text = "";
                textboxPurchaseNAV.Text = "";
            }
        }

        protected void textboxUnits_TextChanged(object sender, EventArgs e)
        {
            if (SIPEnabled == false)
            {
                try
                {
                    if ((PurchaseNAV.Length > 0) && (PurchaseUnits.Length > 0))
                    {
                        textboxValueAtCost.Text = string.Format("{0:0.0000}", (System.Convert.ToDouble(PurchaseNAV) * System.Convert.ToDouble(PurchaseUnits)));
                        textboxSIPAmt.Text = textboxValueAtCost.Text;
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select fund for which NAV information is available for specified date.');", true);
                    }
                }
                catch 
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please enter valid units.');", true);
                }
            }
        }

        protected void textboxSIPAmt_TextChanged(object sender, EventArgs e)
        {
            if (SIPEnabled == false)
            {
                try
                {
                    if ((SIPAmount.Length > 0) && (PurchaseNAV.Length > 0))
                    {
                        textboxUnits.Text = string.Format("{0:0.0000}", (System.Convert.ToDouble(SIPAmount)) / (System.Convert.ToDouble(PurchaseNAV)));
                        textboxValueAtCost.Text = string.Format("{0:0.0000}", (System.Convert.ToDouble(PurchaseNAV) * System.Convert.ToDouble(PurchaseUnits)));
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select fund for which NAV information is available for specified date.');", true);
                    }
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please enter valid units.');", true);
                }
            }
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool breturn = false;

                string portfolioName = Session["MFPORTFOLIONAME"].ToString();
                string portfolioRowId = Session["MFPORTFOLIOMASTERROWID"].ToString();
                DataManager dataMgr = new DataManager();

                if (SIPEnabled == true)
                {
                    if ((FromDate != null) && (SIPEndDate != null) && (SchemeCode.Length > 0) && (SIPAmount.Length > 0))
                    {
                        breturn = dataMgr.addNewSIP(Session["EMAILID"].ToString(), portfolioName, System.Convert.ToInt64(portfolioRowId), SchemeCode,
                                            System.Convert.ToDateTime(FromDate).ToShortDateString(),
                                            System.Convert.ToDateTime(SIPEndDate).ToShortDateString(),
                                            string.Format("{0:0.0000}", System.Convert.ToDouble(SIPAmount)), sipFrequency: SIPFrequency,
                                            monthday: SIPDayOfMonth);
                        if (breturn)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('SIP Transaction added successfully. Please click Back button to go back to portfolio page.');", true);

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while adding transaction. Please try again with different values ot click Back button to go back to portfolio page.');", true);
                        }

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please enter all information before adding SIP transaction.');", true);
                    }
                }
                else
                {
                    if ((FromDate.Length > 0) && (SchemeCode.Length > 0) && (PurchaseNAV.Length > 0) && (PurchaseUnits.Length > 0) && (ValueAtCost.Length > 0))
                    {
                        breturn = dataMgr.addNewTransaction(Session["EMAILID"].ToString(), portfolioName, SchemeCode,
                                            System.Convert.ToDateTime(FromDate).ToShortDateString(),
                                            string.Format("{0:0.0000}", System.Convert.ToDouble(PurchaseNAV)),
                                            string.Format("{0:0.0000}", System.Convert.ToDouble(PurchaseUnits)),
                                            string.Format("{0:0.0000}", System.Convert.ToDouble(ValueAtCost)), System.Convert.ToInt64(portfolioRowId));
                        if (breturn)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Transaction added successfully. Please click Back button to go back to portfolio page.');", true);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while adding transaction. Please try again with different values ot click Back button to go back to portfolio page.');", true);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please enter all information before adding transaction.');", true);
                    }
                }


                //string folderPath = Server.MapPath("~/mfdata/");
                //if (Session["TestDataFolderMF"] != null)
                //{
                //    folderPath = Session["TestDataFolderMF"].ToString();
                //}
                //string filename = Session["PortfolioNameMF"].ToString();
                //if (SIPEnabled == true)
                //{
                //    if ((FromDate != null) && (SIPEndDate != null) && (FundHouseSelectedValue.Equals("-1") == false) &&
                //        (FundNameSelectedValue.Equals("-1") == false) && (SchemeCode.Length > 0)
                //        && (SIPAmount.Length > 0))
                //    {
                //        DataTable fundNameTable = MFAPI.searchMFHistoryForSchemeName(folderPath, FundHouseSelectedValue, FromDate,
                //                            searchString: FundNameSelected, bExactMatch: true, mfHistoryTable: null, toDate: SIPEndDate);

                //        breturn = MFAPI.addNewSIP(folderPath, filename, FundHouseSelected, FundHouseSelectedValue, FundNameSelected, SchemeCode,
                //                            System.Convert.ToDateTime(FromDate).ToShortDateString(),
                //                            System.Convert.ToDateTime(SIPEndDate).ToShortDateString(),
                //                            string.Format("{0:0.0000}", System.Convert.ToDouble(SIPAmount)), sipFrequency: SIPFrequency, 
                //                            monthday:SIPDayOfMonth, historyNAVTable: fundNameTable);
                //        if (breturn)
                //        {
                //            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('SIP Transaction added successfully. Please click Back button to go back to portfolio page.');", true);

                //        }
                //        else
                //        {
                //            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while adding transaction. Please try again with different values ot click Back button to go back to portfolio page.');", true);
                //        }
                //    }
                //    else
                //    {
                //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please enter all information before adding SIP transaction.');", true);
                //    }
                //}
                //else
                //{
                //    if ((FromDate.Length > 0) && (FundHouseSelectedValue.Equals("-1") == false) &&
                //        (FundNameSelectedValue.Equals("-1") == false) && (SchemeCode.Length > 0)
                //        && (PurchaseNAV.Length > 0) && (PurchaseUnits.Length > 0) && (ValueAtCost.Length > 0))
                //    {
                //        breturn = MFAPI.addNewTransaction(filename, FundHouseSelected, FundNameSelected, SchemeCode,
                //                            System.Convert.ToDateTime(FromDate).ToShortDateString(),
                //                            string.Format("{0:0.0000}", System.Convert.ToDouble(PurchaseNAV)),
                //                            string.Format("{0:0.0000}", System.Convert.ToDouble(PurchaseUnits)),
                //                            string.Format("{0:0.0000}", System.Convert.ToDouble(ValueAtCost)));
                //        if (breturn)
                //        {
                //            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Transaction added successfully. Please click Back button to go back to portfolio page.');", true);
                //        }
                //        else
                //        {
                //            Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Error occurred while adding transaction. Please try again with different values ot click Back button to go back to portfolio page.');", true);
                //        }
                //    }
                //    else
                //    {
                //        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please enter all information before adding transaction.');", true);
                //    }

                //}

                if (breturn)
                {
                    checkboxSIPTrans.Checked = false;
                    labelPurchaseDate.Text = "Purchase Date:";
                    textboxPurchaseDate.Text = "";
                    textboxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    textboxEndDate.ReadOnly = true;
                    ddlSIPFrequency.Enabled = false;
                    ddlDayOfMonth.Enabled = false;
                    textboxSIPAmt.Text = "";
                    textboxSIPAmt.ReadOnly = false;
                    textboxUnits.Text = "";
                    textboxUnits.ReadOnly = true;
                    textboxValueAtCost.Text = "";
                    textboxValueAtCost.ReadOnly = true;
                    textboxPurchaseNAV.Text = "";
                    textboxPurchaseNAV.ReadOnly = true;
                    ddlFundHouse.Items.Clear();
                    ddlFundHouse.Enabled = false;
                    ddlFundName.Items.Clear();
                    ddlFundName.Enabled = false;
                    textboxSchemeCode.Text = "";
                    textboxSelectedFundHouse.Text = "";
                    textboxSelectedFundName.Text = "";
                    //((DataTable)ViewState["MFHistoryTable"]).Clear();
                    //ViewState["MFHistoryTable"] = null;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception occurred while adding transaction:" + ex.Message + "');", true);
            }
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            //if (ViewState["MFHistoryTable"] != null)
            //{
            //    ((DataTable)ViewState["MFHistoryTable"]).Clear();
            //    ViewState["MFHistoryTable"] = null;
            //}
            Response.Redirect("~/mopenportfolioMF.aspx");
        }

        protected void ddlSIPFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SIPFrequency.Equals("Monthly"))
            {
                ddlDayOfMonth.Enabled = true;
            }
            else
            {
                if (SIPFrequency.Equals("Daily"))
                {
                    ddlDayOfMonth.SelectedValue = "1";
                }
                else
                {
                    ddlDayOfMonth.SelectedValue = "7";
                }
                ddlDayOfMonth.Enabled = false;
            }
        }

        protected void buttonSearchFUndName_Click(object sender, EventArgs e)
        {
            if (ViewState["FUNDLIST"] != null)
            {
                DataTable mfFundList = (DataTable)ViewState["FUNDLIST"];
                StringBuilder filter = new StringBuilder();
                if (!(string.IsNullOrEmpty(textboxSelectedFundName.Text)))
                    filter.Append("SCHEMENAME Like '%" + textboxSelectedFundName.Text + "%'");
                DataView dv = mfFundList.DefaultView;
                dv.RowFilter = filter.ToString();

                //mfFundList.DefaultView.RowFilter = "SCHEMENAME like '%" + textboxSelectedFundName.Text + "%'";
                //if (mfFundList.DefaultView.Count > 0)
                if(dv.Count > 0)
                {
                    ddlFundName.Items.Clear();
                    ddlFundName.DataTextField = "SCHEMENAME";
                    ddlFundName.DataValueField = "SCHEMECODE";
                    ddlFundName.DataSource = dv;//mfFundList.DefaultView;
                    ddlFundName.DataBind();
                    ListItem li = new ListItem("-- Select Fund Name --", "-1");
                    ddlFundName.Items.Insert(0, li);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select fund house before searching for fund name');", true);
            }
        }
    }
}