﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics
{
    public partial class getquoteadd : System.Web.UI.Page
    {
        public string Symbol
        {
            get
            {
                if (DropDownListStock.SelectedIndex >= 0)
                    return DropDownListStock.SelectedValue;
                else
                    return "";
            }
        }

        public string Price
        {
            get
            {
                return textboxPrice.Text;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["EmailId"] != null)
            //{
            //    Master.UserID = Session["emailid"].ToString();
            //}

            if (Session["PortfolioName"] != null)
            {
                //Master.Portfolio = Session["PortfolioName"].ToString();
                if (!IsPostBack)
                {
                    DropDownListStock.Items.Clear();
                }
            }
            else
            {
                Response.Redirect(".\\Default.aspx");
            }
        }
        protected void buttonAddStock_Click(object sender, EventArgs e)
        {
            //Server.Transfer("~/addnewscript.aspx");
            if(this.MasterPageFile.Contains("Site.Master"))
                Response.Redirect(".\\addnewscript.aspx?symbol=" + Symbol + "&price=" + Price);
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                Response.Redirect(".\\maddnewscript.aspx?symbol=" + Symbol + "&price=" + Price);
            else
                Response.Redirect(".\\addnewscript.aspx?symbol=" + Symbol + "&price=" + Price);
        }
        protected void buttonGoBack_Click(object sender, EventArgs e)
        {
            if (this.MasterPageFile.Contains("Site.Master"))
                Response.Redirect(".\\openportfolio.aspx");
            else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                Response.Redirect(".\\mopenportfolio.aspx");
            else
                Response.Redirect(".\\openportfolio.aspx");
        }
        protected void DropDownListStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListStock.SelectedIndex >= 0)
            {
                labelSelectedSymbol.Text = "Selected stock:" + DropDownListStock.SelectedValue;
                Session["ScriptName"] = DropDownListStock.SelectedValue;
            }
            else
            {
                labelSelectedSymbol.Text = "Please select stock to get quote for";
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
        protected void ButtonGetQuote_Click(object sender, EventArgs e)
        {
            string selectedSymbol = "";
            if (DropDownListStock.SelectedIndex >= 0)
            {
                string folderPath = Server.MapPath("~/scriptdata/");
                bool bIsTestOn = true;
                if (Session["IsTestOn"] != null)
                {
                    bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                }

                if (Session["TestDataFolder"] != null)
                {
                    folderPath = Session["TestDataFolder"].ToString();
                }

                selectedSymbol = DropDownListStock.SelectedValue;


                DataTable quoteTable = StockApi.globalQuote(folderPath, selectedSymbol, bIsTestOn);
                //column names = symbol,open,high,low,price,volume,latestDay,previousClose,change,changePercent
                if (quoteTable != null)
                {
                    textboxOpen.Text = quoteTable.Rows[0]["open"].ToString();
                    textboxHigh.Text = quoteTable.Rows[0]["high"].ToString();
                    textboxLow.Text = quoteTable.Rows[0]["low"].ToString();
                    textboxPrice.Text = quoteTable.Rows[0]["price"].ToString();
                    textboxVolume.Text = quoteTable.Rows[0]["volume"].ToString();
                    textboxLatestDay.Text = quoteTable.Rows[0]["latestDay"].ToString();
                    textboxPrevClose.Text = quoteTable.Rows[0]["previousClose"].ToString();
                    textboxChange.Text = quoteTable.Rows[0]["change"].ToString();
                    textboxChangePercent.Text = quoteTable.Rows[0]["changePercent"].ToString();
                }
                else
                {
                    Response.Write("<script language=javascript>alert('Not able to get quote at now, please try again later.')</script>");
                }
            }
            else
            {
                Response.Write("<script language=javascript>alert('Please select symbol to get quote for.')</script>");
            }

        }

    }
}