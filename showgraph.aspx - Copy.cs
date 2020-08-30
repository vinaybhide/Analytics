using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class showgraph : System.Web.UI.Page
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
               }
               else
               {
                   if (ViewState["GraphScript"] != null)
                       labelSelectedSymbol.Text = ViewState["GraphScript"].ToString();
               }
            }
            else
            {
                Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Response.Flush();
                //Response.Redirect("~/Default.aspx");
                Server.Transfer("~/Default.aspx");
            }
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (TextBoxSearch.Text.Length > 0)
            {
                DataTable resultTable = StockApi.symbolSearch(TextBoxSearch.Text, apiKey: Session["ApiKey"].ToString());

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
                    Response.Write("<script language=javascript>alert('" + common.noSymbolFound +"')</script>");
                }

            }
            else
            {
                Response.Write("<script language=javascript>alert('"+ common.noTextSearchSymbol +"')</script>");
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


        protected void buttonDaily_Click(object sender, EventArgs e)
        {
            string outputSize = ddlDaily_OutputSize.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;
            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/dailygraph.aspx" + "?script=" + scriptName + "&size=" + outputSize;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonIntraday_Click(object sender, EventArgs e)
        {
            string outputSize = ddlIntraday_outputsize.SelectedValue;
            string interval = ddlIntraday_Interval.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;
            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/intraday.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval=" + interval;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonEMA_Click(object sender, EventArgs e)
        {
            string interval = ddlEMA_Interval.SelectedValue;
            string period = textboxEMA_Period.Text;
            string seriesType = ddlEMA_Series.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/ema.aspx" + "?script=" + scriptName + "&interval=" + interval + "&period=" + period + "&seriestype=" + seriesType;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonSMA_Click(object sender, EventArgs e)
        {
            string interval = ddlSMA_Interval.SelectedValue;
            string period = textboxSMA_Period.Text;
            string seriesType = ddlSMA_Series.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/sma.aspx" + "?script=" + scriptName + "&interval=" + interval + "&period=" + period + "&seriestype=" + seriesType;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonAdx_Click(object sender, EventArgs e)
        {
            string interval = ddlAdx_Interval.SelectedValue;
            string period = textboxAdx_Period.Text;
            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/adx.aspx" + "?script=" + scriptName + "&interval=" + interval + "&period=" + period;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1000,height=1000,top=0");
                }
            }
        }

        protected void buttonRSI_Click(object sender, EventArgs e)
        {
            string interval = ddlRSI_Interval.SelectedValue;
            string period = textboxRSI_Period.Text;
            string seriestype = ddlRSI_Series.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/rsi.aspx" + "?script=" + scriptName + "&interval=" + interval + "&period=" + period + "&seriestype=" + seriestype;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonSTOCH_Click(object sender, EventArgs e)
        {
            string interval = ddlSTOCH_Interval.SelectedValue;
            string fastkperiod = textboxSTOCH_Fastkperiod.Text;
            string slowkperiod = textboxSTOCH_Slowkperiod.Text;
            string slowdperiod = textboxSTOCH_Slowdperiod.Text;
            string slowkmatype = ddlSTOCH_Slowkmatype.SelectedValue;
            string slowdmatype = ddlSTOCH_Slowdmatype.SelectedValue;

            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/stoch.aspx" + "?script=" + scriptName + "&interval=" + interval + "&fastkperiod=" + fastkperiod + "&slowkperiod=" + slowkperiod +
                                    "&slowdperiod=" + slowdperiod + "&slowkmatype=" + slowkmatype + "&slowdmatype=" + slowdmatype;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonMACD_Click(object sender, EventArgs e)
        {
            string interval = ddlMACD_Interval.SelectedValue;
            string seriestype = ddlMACD_Series.SelectedValue;
            string fastperiod = textboxMACD_FastPeriod.Text;
            string slowperiod = textboxMACD_SlowPeriod.Text;
            string signalperiod = textboxMACD_SignalPeriod.Text;

            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/macd.aspx" + "?script=" + scriptName + "&interval=" + interval + "&seriestype=" + seriestype + "&fastperiod=" + fastperiod +
                                    "&slowperiod=" + slowperiod + "&signalperiod=" + signalperiod;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonAroon_Click(object sender, EventArgs e)
        {
            string interval = ddlAroon_Interval.SelectedValue;
            string period = textboxAroon_Period.Text;
            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/aroon.aspx" + "?script=" + scriptName + "&interval=" + interval + "&period=" + period;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonBBands_Click(object sender, EventArgs e)
        {
            string interval = ddlBBands_Interval.SelectedValue;
            string period = textboxBBands_Period.Text;
            string seriestype = ddlBBands_Series.SelectedValue;
            string nbdevup = textboxBBands_NbdevUp.Text;
            string nbdevdn = textboxBBands_NbdevDn.Text;

            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/bbands.aspx" + "?script=" + scriptName + "&interval=" + interval + "&period=" + period + "&seriestype=" + seriestype +
                                            "&nbdevup=" + nbdevup + "&nbdevdn=" + nbdevdn;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }

        protected void buttonVWAPrice_Click(object sender, EventArgs e)
        {
            string interval = ddlVWAP_Interval.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "~/graphs/vwaprice.aspx" + "?script=" + scriptName + "&interval=" + interval;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=showgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=mshowgraph.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=2,width=1280,height=1024,top=0");
                }
            }
        }
    }
}