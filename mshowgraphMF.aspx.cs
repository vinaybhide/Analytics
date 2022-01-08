using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace Analytics
{
    public partial class mshowgraphMF : System.Web.UI.Page
    {
        public string FundHouseSelected
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return ddlFundHouse.Items[ddlFundHouse.SelectedIndex].Text;
            }
        }

        public string FundHouseSelectedCode
        {
            get
            {
                //return textboxFundName.Text.Trim();
                return ddlFundHouse.Items[ddlFundHouse.SelectedIndex].Value;
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

        public string dateFrom
        {
            get
            {
                return System.Convert.ToDateTime(textboxFromDateM.Text).ToString("yyyy-MM-dd");
            }
        }

        public string dateTo
        {
            get
            {
                return System.Convert.ToDateTime(textboxToDateM.Text).ToString("yyyy-MM-dd");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["MFPORTFOLIOMASTERROWID"] = null;
            Session["MFPORTFOLIONAME"] = null;

            if (Session["EMAILID"] != null)
            {
                if (!IsPostBack)
                {
                    ViewState["MFMasterTable"] = null;
                    ViewState["MFSchemeTable"] = null;
                    ddlFundHouse.Enabled = true;
                    ddlFundHouse.Items.Clear();

                    //ddlFundHouse.Items.AddRange(MFAPI.listFundHouseMaster);
                    LoadFundHouseList();
                    ddlFundName.Items.Clear();
                    ddlFundName.Enabled = false;
                    textboxSchemeCode.Text = "";

                    textboxFromDateM.Text = DateTime.Today.AddDays(-90).ToString("yyyy-MM-dd");
                    textboxToDateM.Text= DateTime.Today.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                //Response.Flush();
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                //Response.Redirect("~/Default.aspx");
                Server.Transfer("~/Default.aspx");
            }
        }

        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundHouseSelectedValue != "-1")
            {
                //ViewState["MFMasterTable"] = null;
                ViewState["MFSchemeTable"] = null;

                ddlFundName.Enabled = true;
                ddlFundName.Items.Clear();
                textboxSchemeCode.Text = "";

                DataManager dataMgr = new DataManager();
                DataTable mfSchemeTable = dataMgr.getSchemesTable(fundhousecode: System.Convert.ToInt32(FundHouseSelectedCode));
                if ((mfSchemeTable != null) && (mfSchemeTable.Rows.Count > 0))
                {
                    //columns... SCHEME_TYPE.ID, SCHEME_TYPE.TYPE, FUNDHOUSE.FUNDHOUSECODE, FUNDHOUSE.NAME, SCHEMES.SCHEMECODE, SCHEMES.SCHEMENAME
                    ddlFundName.DataTextField = "SCHEMENAME";
                    ddlFundName.DataValueField = "SCHEMECODE";
                    ddlFundName.DataSource = mfSchemeTable;
                    ddlFundName.DataBind();

                    ListItem li = new ListItem("-- Select Fund Name --", "-1");
                    ddlFundName.Items.Insert(0, li);
                    ViewState["MFSchemeTable"] = mfSchemeTable;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Fund data not found for selected fund house. Please select another fund house.');", true);
            }
        }

        protected void ddlFundName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FundNameSelectedValue != "-1")
            {
                textboxSchemeCode.Text = FundNameSelectedValue;
            }
        }

        protected void buttonDaily_Click(object sender, EventArgs e)
        {
            string url = "";
            if (FundNameSelectedValue.Length > 0)
            {
                url = "~/graphs/dailygraphMF.aspx" + "?fundhousecode=" + FundHouseSelectedValue + "&schemecode=" + FundNameSelectedValue + "&schemetypeid=" + "-1" + 
                             "&fromdate=" + dateFrom + "&todate=" + dateTo;

                if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraphMF.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noFundSelected + "');", true);
            }
        }

        public void LoadFundHouseList()
        {
            ViewState["MFSchemeTable"] = null;
            DataManager dataMgr = new DataManager();
            DataTable fundHouseTable = dataMgr.getFundHouseTable();
            if((fundHouseTable != null) && (fundHouseTable.Rows.Count > 0))
            {
                // Columns - FUNDHOUSECODE, NAME
                ddlFundHouse.DataTextField = "NAME";
                ddlFundHouse.DataValueField = "FUNDHOUSECODE";
                ddlFundHouse.DataSource = fundHouseTable;
                ddlFundHouse.DataBind();
            }
        }

        protected void buttonRSI_Click(object sender, EventArgs e)
        {
            string url = "";
            if (FundNameSelectedValue.Length > 0)
            {
                url = "~/graphs/rsiMF.aspx" + "?fundhousecode=" + FundHouseSelectedValue + "&schemecode=" + FundNameSelectedValue + "&schemetypeid=" + "-1" +
                             "&fromdate=" + dateFrom + "&todate=" + dateTo + "&period=" + textboxRSI_Period.Text.ToString();

                if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraphMF.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noFundSelected + "');", true);
            }
        }

        protected void buttonSearchFUndName_Click(object sender, EventArgs e)
        {
            if (ViewState["MFSchemeTable"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Please select fund house and then search for fund name');", true);
            }
            else
            {
                DataTable fundNameTable = (DataTable)ViewState["MFSchemeTable"];
                StringBuilder filter = new StringBuilder();
                if (!(string.IsNullOrEmpty(textboxSelectedFundName.Text)))
                    filter.Append("SCHEMENAME Like '%" + textboxSelectedFundName.Text + "%'");
                DataView dv = fundNameTable.DefaultView;
                dv.RowFilter = filter.ToString();
                if (dv.Count > 0)
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
        }
    }
}