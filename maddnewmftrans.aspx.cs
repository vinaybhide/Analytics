﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                return System.Convert.ToDateTime(textboxPurchaseDate.Text).ToShortDateString();
            }
        }
        public string SIPEndDate
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return System.Convert.ToDateTime(textboxEndDate.Text).ToShortDateString();
            }
        }

        public string FundHouseSelected
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PortfolioNameMF"] != null)
            {
                if (!IsPostBack)
                {
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
                textboxSIPAmt.ReadOnly = false;
                textboxUnits.Text = "";
                textboxUnits.ReadOnly = true;
                textboxValueAtCost.Text = "";
                textboxValueAtCost.ReadOnly = true;
            }
            else
            {
                labelPurchaseDate.Text = "Purchase Date:";
                textboxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                textboxEndDate.ReadOnly = true;
                ddlSIPFrequency.Enabled = false;
                textboxSIPAmt.Text = "";
                textboxSIPAmt.ReadOnly = true;
                textboxUnits.Text = "";
                textboxUnits.ReadOnly = false;
                textboxValueAtCost.Text = "";
                textboxValueAtCost.ReadOnly = true;
            }
        }

        protected void textboxPurchaseDate_TextChanged(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/mfdata/");
            if (Session["TestDataFolderMF"] != null)
            {
                folderPath = Session["TestDataFolderMF"].ToString();
            }

            DataTable mfMasterTable = MFAPI.getMFNAVForDate(folderPath, FromDate);

            DataTable fundHouseTable = MFAPI.getFundHouses(folderPath, searchString: null, bExactMatch: false, mfMasterTable: mfMasterTable);
            ddlFundHouse.Items.Clear();
            ddlFundHouse.DataTextField = "MF_COMP_NAME";
            ddlFundHouse.DataValueField = "MF_COMP_NAME";
            ddlFundHouse.DataSource = fundHouseTable;
            ddlFundHouse.DataBind();
            ListItem li = new ListItem("Select Fund House", "-1");
            ddlFundHouse.Items.Insert(0, li);

            ddlFundName.Items.Clear();
            textboxSchemeCode.Text = "";
            textboxPurchaseNAV.Text = "";
            textboxSIPAmt.Text = "";
            textboxUnits.Text = "";
            textboxValueAtCost.Text = "";
        }

        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundHouseSelected != "-1")
            {
                textboxSelectedFundHouse.Text = FundHouseSelected;

                string folderPath = Server.MapPath("~/mfdata/");
                if (Session["TestDataFolderMF"] != null)
                {
                    folderPath = Session["TestDataFolderMF"].ToString();
                }

                DataTable mfMasterTable = MFAPI.getMFNAVForDate(folderPath, FromDate);
                DataTable fundNameTable = MFAPI.getALLMFforFundHouse(folderPath, searchString: FundHouseSelected, bExactMatch: true, mfMasterTable: mfMasterTable);
                ddlFundName.Items.Clear();
                if ((fundNameTable != null) && (fundNameTable.Rows.Count > 0))
                {
                    ddlFundName.DataTextField = "SCHEME_NAME";
                    ddlFundName.DataValueField = "SCHEME_NAME";
                    ddlFundName.DataSource = fundNameTable;
                    ddlFundName.DataBind();

                    ListItem li = new ListItem("Select Fund Name", "-1");
                    ddlFundName.Items.Insert(0, li);

                }
            }
            else
            {
                textboxSelectedFundHouse.Text = "";
            }
        }


        protected void ddlFundName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundNameSelected != "-1")
            {
                textboxSelectedFundName.Text = FundNameSelected;
                string folderPath = Server.MapPath("~/mfdata/");
                if (Session["TestDataFolderMF"] != null)
                {
                    folderPath = Session["TestDataFolderMF"].ToString();
                }
                DataTable mfMasterTable = MFAPI.getMFNAVForDate(folderPath, FromDate);
                if (mfMasterTable != null)
                {
                    DataTable fundNameTable = MFAPI.searchMFMaster(folderPath, searchString: FundNameSelected, bExactMatch: true, mfMasterTable: mfMasterTable);
                    if ((fundNameTable != null) && (fundNameTable.Rows.Count > 0))
                    {
                        //MF_TYPE;MF_COMP_NAME;SCHEME_CODE;ISIN_Div_Payout_ISIN_Growth;ISIN_Div_Reinvestment;SCHEME_NAME;NET_ASSET_VALUE;DATE
                        textboxSchemeCode.Text = fundNameTable.Rows[0]["SCHEME_CODE"].ToString();
                        if (SIPEnabled == false)
                        {
                            textboxPurchaseNAV.Text = fundNameTable.Rows[0]["NET_ASSET_VALUE"].ToString();
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Fund information not available for specified purchase date.');", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Fund information not available for specified purchase date.');", true);
                }
            }
            else
            {
                textboxSelectedFundName.Text = "";
            }
        }

        protected void textboxUnits_TextChanged(object sender, EventArgs e)
        {
            if (SIPEnabled == false)
            {
                try
                {
                    if (PurchaseNAV.Length > 0)
                    {
                        textboxValueAtCost.Text = string.Format("{0:0.0000}", (System.Convert.ToDouble(PurchaseNAV) * System.Convert.ToDouble(PurchaseUnits)));
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select fund for which NAV information is available for specified date.');", true);
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please enter valid units.');", true);
                }
            }
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool breturn;
                string folderPath = Server.MapPath("~/mfdata/");
                if (Session["TestDataFolderMF"] != null)
                {
                    folderPath = Session["TestDataFolderMF"].ToString();
                }
                string filename = Session["PortfolioNameMF"].ToString();
                if (SIPEnabled == true)
                {
                    if ((FromDate.Length > 0) && (SIPEndDate.Length > 0) && (FundHouseSelected.Length > 0) && (FundNameSelected.Length > 0) && (SchemeCode.Length > 0)
                        && (SIPAmount.Length > 0))
                    {
                        breturn = MFAPI.addNewSIPTransactionSet(folderPath, filename, FundHouseSelected, FundNameSelected, SchemeCode,
                                    System.Convert.ToDateTime(FromDate).ToShortDateString(),
                                    System.Convert.ToDateTime(SIPEndDate).ToShortDateString(), SIPAmount, SIPFrequency);
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
                    if ((FromDate.Length > 0) && (FundHouseSelected.Length > 0) && (FundNameSelected.Length > 0) && (SchemeCode.Length > 0)
                        && (PurchaseNAV.Length > 0) && (PurchaseUnits.Length > 0) && (ValueAtCost.Length > 0))
                    {
                        breturn = MFAPI.addNewTransaction(filename, FundHouseSelected, FundNameSelected, SchemeCode, 
                                            System.Convert.ToDateTime(FromDate).ToShortDateString(), PurchaseNAV, PurchaseUnits, ValueAtCost);
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
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Exception occurred while adding transaction:" + ex.Message + "');", true);
            }
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/mopenportfolioMF.aspx");
        }
    }
}