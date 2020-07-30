using System;
using System.Collections.Generic;
using System.Data;
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
                }
                else
                {
                    if (ViewState["GraphScript"] != null)
                        labelSelectedSymbol.Text = ViewState["GraphScript"].ToString();
                }

            }
            else
            {
                Response.Redirect(".\\Default.aspx");
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
                DataTable resultTable = StockApi.symbolSearch(TextBoxSearch.Text);

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
                    Response.Write("<script language=javascript>alert('No matching symbols found')</script>");
                }

            }
            else
            {
                Response.Write("<script language=javascript>alert('Enter text in Search Stock to search stock symbol')</script>");
            }

        }
        protected void buttonVWAPIntra_Click(object sender, EventArgs e)
        {
            string outputSize = ddlIntraday_outputsize.SelectedValue;
            string interval_intra = ddlIntraday_Interval.SelectedValue;

            string interval_vwap = ddlVWAP_Interval.SelectedValue;
            string scriptName = labelSelectedSymbol.Text;

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "\\ vwap_intra.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval_intra=" + interval_intra + "&interval_vwap=" + interval_vwap;
                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
                }
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

            string url = "";
            if (scriptName.Length > 0)
            {
                url = "\\crossover.aspx" + "?script=" + scriptName + "&size=" + outputSize + "&interval1=" + interval1 + "&period1=" + period1 +
                        "&seriestype1=" + seriestype1 + "&interval2=" + interval2 + "&period2=" + period2 + "&seriestype2=" + seriestype2;

                if (this.MasterPageFile.Contains("Site.Master"))
                {
                    url += "&parent=advancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
                }
                else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                {
                    url += "&parent=madvancegraphs.aspx";
                    ResponseHelper.Redirect(Response, url, "_blank", "menubar=0,scrollbars=1,width=1000,height=1000,top=10");
                }
            }
        }
    }
}