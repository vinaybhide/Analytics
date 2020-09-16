using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class advancegraphs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmailId"] != null)
            {
                if (!IsPostBack)
                {
                    if (Session["ScriptName"] != null)
                    {
                        ViewState["GraphScript"] = Session["ScriptName"];
                    }
                    else
                    {
                        ViewState["GraphScript"] = null;
                    }

                    if (ViewState["GraphScript"] != null)
                        labelSelectedSymbol.Text = ViewState["GraphScript"].ToString();
                    else
                        labelSelectedSymbol.Text = "";

                    if (Session["PortfolioFolder"] != null)
                    {
                        string folder = Session["PortfolioFolder"].ToString();
                        string[] filelist = Directory.GetFiles(folder, "*.xml");

                        //int lstwidth = 0;
                        if (filelist.Length > 0)
                        {
                            ddlPortfolios.Items.Clear();
                            ListItem li = new ListItem("Select Portfolio", "-1");
                            ddlPortfolios.Items.Insert(0, li);

                            foreach (string filename in filelist)
                            {
                                string portfolioName = filename.Remove(0, filename.LastIndexOf('\\') + 1);
                                ListItem filenameItem = new ListItem(portfolioName, filename);
                                ddlPortfolios.Items.Add(filenameItem);
                            }
                        }
                        else
                        {
                            ButtonSearchPortfolio.Enabled = false;
                            ddlPortfolios.Enabled = false;
                        }
                    }
                }
                else
                {
                    if (ViewState["GraphScript"] != null)
                        labelSelectedSymbol.Text = ViewState["GraphScript"].ToString();
                }

            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }

        }

        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListStock.SelectedValue != "-1")
            {
                ViewState["GraphScript"] = DropDownListStock.SelectedValue;
                labelSelectedSymbol.Text = DropDownListStock.SelectedValue;
            }
            else
            {
                ViewState["GraphScript"] = "Search & Select script";
                labelSelectedSymbol.Text = "Search & Select script";
            }
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (TextBoxSearch.Text.Length > 0)
            {
                //DataTable resultTable = StockApi.symbolSearch(TextBoxSearch.Text, apiKey: Session["ApiKey"].ToString());
                DataTable resultTable = StockApi.symbolSearchAltername(TextBoxSearch.Text, apiKey: Session["ApiKey"].ToString());

                if (resultTable != null)
                {
                    DropDownListStock.DataTextField = "Name";
                    DropDownListStock.DataValueField = "Symbol";
                    DropDownListStock.DataSource = resultTable;
                    DropDownListStock.DataBind();
                    ListItem li = new ListItem("Select Stock", "-1");
                    DropDownListStock.Items.Insert(0, li);
                    ViewState["GraphScript"] = "Search & Select script";
                    labelSelectedSymbol.Text = "Search & Select script";
                }
                else
                {
                    //Response.Write("<script language=javascript>alert('"+ common.noSymbolFound + "')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noSymbolFound + "');", true);
                }

            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noTextSearchSymbol +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noTextSearchSymbol + "');", true);
            }

        }

        protected void ButtonSearchPortfolio_Click(object sender, EventArgs e)
        {
            if (ddlPortfolios.SelectedIndex >= 0)
            {
                DropDownListStock.Items.Clear();
                DropDownListStock.DataSource = null;
                ListItem li = new ListItem("Select Stock", "-1");
                DropDownListStock.Items.Insert(0, li);

                //Session["PortfolioName"] = ddlPortfolios.SelectedValue;
                //Session["ShortPortfolioName"] = ddlPortfolios.SelectedItem.Text;

                string[] scriptList = StockApi.getScriptFromPortfolioFile(ddlPortfolios.SelectedValue);
                if (scriptList != null)
                {
                    foreach (string script in scriptList)
                    {
                        li = new ListItem(script, script);
                        DropDownListStock.Items.Add(li);
                    }
                    labelSelectedSymbol.Text = "Selected stock: ";
                    ViewState["GraphScript"] = "Selected stock:";
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noScriptsInPortfolio + "');", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noPortfolioSelected + "');", true);
            }
        }

        protected void buttonVWAPIntra_Click(object sender, EventArgs e)
        {
            string outputSize = ddlIntraday_outputsize.SelectedValue;
            string interval_intra = ddlIntraday_Interval.SelectedValue;

            string interval_vwap = ddlVWAP_Interval.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                //url = "\\ vwap_intra.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval_intra=" + interval_intra + "&interval_vwap=" + interval_vwap;
                url = "~/advgraphs/vwap_intra.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval_intra=" + interval_intra + "&interval_vwap=" + interval_vwap;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph +"')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonCrossover_Click(object sender, EventArgs e)
        {
            string outputSize = ddlDaily_OutputSize.SelectedValue;
            string interval1 = ddlSMA1_Interval.SelectedValue;
            string period1 = textboxSMA1_Period.Text;
            string seriestype1 = ddlSMA1_Series.SelectedValue;
            string interval2 = ddlSMA2_Interval.SelectedValue;
            string period2 = textboxSMA2_Period.Text;
            string seriestype2 = ddlSMA2_Series.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/crossover.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval1=" + interval1 + "&period1=" + period1 +
                        "&seriestype1=" + seriestype1 + "&interval2=" + interval2 + "&period2=" + period2 + "&seriestype2=" + seriestype2;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonMACD_EMA_Daily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlMACDEMADaily_outputsize.SelectedValue;
            string interval = ddlMACDEMADail_interval1.SelectedValue;
            string seriestype = ddlMACDEMADail_seriestype1.SelectedValue;
            string fastperiod = textboxMACDEMADaily_fastperiod.Text;
            string slowperiod = textboxMACDEMADaily_slowperiod.Text;
            string signalperiod = textboxMACDEMADaily_signalperiod.Text;
            string scriptName = labelSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/macdemadaily.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval=" + interval + "&seriestype=" + seriestype +
                    "&fastperiod=" + fastperiod + "&slowperiod=" + slowperiod + "&signalperiod=" + signalperiod;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }

        }

        protected void buttonRSIDaily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlRSIDaily_Outputsize.SelectedValue;
            string interval = ddlRSIDaily_Interval.SelectedValue;
            string period = textboxRSIDaily_Period.Text;
            string seriestype = ddlRSIDaily_SeriesType.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/rsidaily.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval=" + interval + "&period=" + period +
                        "&seriestype=" + seriestype;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonBBandsDaily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlBBandsDaily_Outputsize.SelectedValue;
            string interval = ddlBBandsDaily_Interval.SelectedValue;
            string period = textboxBBandsDaily_Period.Text;
            string seriestype = ddlBBandsDaily_SeriesType.SelectedValue;
            string nbdevup = textboxBBandsDaily_NbdevUp.Text;
            string nbdevdn = textboxBBandsDaily_NbdevDn.Text;
            string scriptName = labelSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/bbandsdaily.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval=" + interval + "&period=" + period +
                        "&seriestype=" + seriestype + "&nbdevup=" + nbdevup + "&nbdevdn=" + nbdevdn;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonStochDaily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlStochDaily_OutuputSize.SelectedValue;
            string interval = ddlStochDaily_Interval.SelectedValue;
            string fastkperiod = textboxSTOCHDaily_Fastkperiod.Text;
            string slowkperiod = textboxSTOCHDaily_Slowkperiod.Text;
            string slowdperiod = textboxSTOCHDaily_Slowdperiod.Text;
            string slowkmatype = ddlSTOCHDaily_Slowkmatype.SelectedValue;
            string slowdmatype = ddlSTOCHDaily_Slowdmatype.SelectedValue;
            string rsi_interval = ddlStochDaily_Interval.SelectedValue;
            string rsi_period = textboxStochDailyRSI_Period.Text;
            string rsi_seriestype = ddlStochDailyRSI_SeriesType.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/stochdaily.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval=" + interval + 
                    "&fastkperiod=" + fastkperiod + "&slowkperiod=" + slowkperiod + "&slowdperiod=" + slowdperiod +
                        "&slowkmatype=" + slowkmatype + "&slowdmatype=" + slowkmatype + "&rsiinterval=" + rsi_interval + "&rsiperiod=" + rsi_period +
                        "&rsiseriestype=" + rsi_seriestype;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

        protected void buttonDMI_Click(object sender, EventArgs e)
        {
            string outputSize = ddlDMIDaily_Outputsize.SelectedValue;
            string intervalDX = ddlDMIDX_Interval.SelectedValue;
            string periodDX = textboxDMIDX_Period.Text;
            string interval_minusdi = ddlDMIMINUSDI_Interval.SelectedValue;
            string period_minusdi = textboxDMIMINUSDI_Period.Text;
            string interval_plusdi = ddlDMIPLUSDI_Interval.SelectedValue;
            string period_plusdi = textboxDMIPLUSDI_Interval.Text;
            string interval_adx = ddlDMIADX_Interval.SelectedValue;
            string period_adx = textboxDMIADX_Period.Text;

            string scriptName = labelSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/dx.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&intervaldx=" + intervalDX + "&perioddx=" + periodDX +
                    "&intervalminusdi=" + interval_minusdi + "&periodminusdi=" + period_minusdi + "&intervalplusdi=" + interval_plusdi +
                        "&periodplusdi=" + period_plusdi + "&intervaladx=" + interval_adx + "&periodadx=" + period_adx;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }
        protected void buttonPrice_Click(object sender, EventArgs e)
        {
            string outputSize = ddlPrice_Outputsize.SelectedValue;
            string interval_minusdm = ddlPriceMINUSDMI_Interval.SelectedValue;
            string period_minusdm = textboxPriceMINUSDMI.Text;
            string interval_plusdm = ddlPricePLUSDMI.SelectedValue;
            string period_plusdm = textboxPricePlusDMI.Text;

            string scriptName = labelSelectedSymbol.Text;

            string url;
            if (scriptName.Length > 0)
            {
                url = "~/advgraphs/dmi.aspx" + "?script=" + scriptName + "&size=" + outputSize + 
                    "&intervalminusdm=" + interval_minusdm + "&periodminusdm=" + period_minusdm + "&intervalplusdm=" + interval_plusdm +
                        "&periodplusdm=" + period_plusdm;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
            else
            {
                //Response.Write("<script language=javascript>alert('" + common.noStockSelectedToShowGraph + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noStockSelectedToShowGraph + "');", true);
            }
        }

    }
}