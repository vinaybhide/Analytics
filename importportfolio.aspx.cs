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
    public partial class importportfolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["EmailId"] == null) || (Session["PortfolioFolder"] == null))
            {
                //Response.Write("<script language=javascript>alert('" + common.noLogin + "')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('" + common.noLogin + "');", true);
                Response.Redirect("~/Default.aspx");
            }

            if (!IsPostBack)
            {
                ViewState["CSVData"] = null;
                ViewState["CSVGridData"] = null;
                ViewState["CSVColData"] = null;
                ViewState["MappedData"] = null;
                ViewState["UploadedFileName"] = null;
            }

        }

        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable csvTable, csvColTable;

                if (FileUploadCSV.HasFile)
                {
                    // Get the file extension
                    string fileExtension = System.IO.Path.GetExtension(FileUploadCSV.FileName);

                    if (fileExtension.ToLower() != ".csv")
                    {
                        labelMessage.ForeColor = System.Drawing.Color.Red;
                        labelMessage.Text = "Only text file files with .csv extension are allowed";
                    }
                    else
                    {
                        // Get the file size
                        int fileSize = FileUploadCSV.PostedFile.ContentLength;
                        // If file size is greater than 2 MB
                        if (fileSize > 2097152)
                        {
                            labelMessage.ForeColor = System.Drawing.Color.Red;
                            labelMessage.Text = "File size cannot be greater than 2 MB";
                        }
                        else
                        {
                            // Upload the file
                            //string fileName = Session["PortfolioFolder"].ToString() + "\\" + textboxPortfolioName.Text + ".xml";

                            //FileUploadCSV.SaveAs(Server.MapPath("~/Uploads/" + FileUploadCSV.FileName));
                            ViewState["UploadedFileName"] = Session["PortfolioFolder"].ToString() + "\\" + FileUploadCSV.FileName;
                            FileUploadCSV.SaveAs(Session["PortfolioFolder"].ToString() + "\\" + FileUploadCSV.FileName);
                            labelMessage.ForeColor = System.Drawing.Color.Green;
                            labelMessage.Text = "File uploaded successfully. Please map the fields below to create portfolio.";

                            Stream receiveStream = FileUploadCSV.FileContent;
                            StreamReader reader = null;
                            reader = new StreamReader(receiveStream);
                            //string fileData = reader.ReadToEnd();
                            //textboxMessage.Text = fileData;
                            csvTable = StockApi.readCSV(reader);
                            if (csvTable != null)
                            {
                                ViewState["CSVData"] = csvTable;
                                ViewState["CSVGridData"] = csvTable;

                                //if(GridViewData.Columns.Count > 0)
                                //{
                                //    GridViewData.Columns.Clear();
                                //}
                                //foreach (DataColumn col in csvTable.Columns)
                                //{
                                //    BoundField newcol = new BoundField();
                                //    newcol.DataField = col.ColumnName;
                                //    newcol.HeaderText = col.ColumnName;

                                //    GridViewData.Columns.Add(newcol);
                                //}
                                //GridViewData.DataSource = csvTable;
                                //GridViewData.DataBind();

                                csvColTable = StockApi.readColumnsFromCSVTable(csvTable);
                                if (csvColTable != null)
                                {
                                    ViewState["CSVColData"] = csvColTable;
                                    ddlSourceCols.Items.Clear();
                                    foreach (DataRow row in csvColTable.Rows)
                                    {
                                        ListItem li = new ListItem(row[0].ToString(), row[0].ToString());
                                        ddlSourceCols.Items.Add(li);
                                    }
                                    GridViewMapping.DataSource = csvColTable;
                                    GridViewMapping.DataBind();
                                }
                                else
                                {
                                    labelMessage.ForeColor = System.Drawing.Color.Red;
                                    labelMessage.Text = "Problem while mapping source columns. Please select correct CSV file.";
                                    buttonMapSelected.Enabled = false;
                                }

                                buttonMapSelected.Enabled = true;
                            }
                            else
                            {
                                labelMessage.ForeColor = System.Drawing.Color.Red;
                                labelMessage.Text = "Problem while reading data. Please select correct CSV file.";
                                buttonMapSelected.Enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    labelMessage.ForeColor = System.Drawing.Color.Red;
                    labelMessage.Text = "Please select a file";
                }
            }
            catch (Exception ex)
            {
                labelMessage.Text = ex.Message;
            }
        }
        protected void buttonBack_Click(object sender, EventArgs e)
        {
            if (Session["PortfolioFolder"] != null)
            {
                string folder = Session["PortfolioFolder"].ToString();
                if ((Directory.GetFiles(folder, "*")).Length > 0)
                {
                    //Server.Transfer("~/openportfolio.aspx");
                    if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/selectportfolio.aspx");
                    else if (this.MasterPageFile.Contains("Site.Mobile.Master"))
                        Response.Redirect("~/mselectportfolio.aspx");
                    else
                        //Response.Redirect("~/selectportfolio.aspx");
                        Response.Redirect("~/Default.aspx");
                }
                else
                {
                    if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/newportfolio.aspx");
                    else if (this.MasterPageFile.Contains("Site.Master"))
                        Response.Redirect("~/mnewportfolio.aspx");
                    else
                        //Response.Redirect("~/newportfolio.aspx");
                        Response.Redirect("~/Default.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        //protected void GridViewData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridViewData.PageIndex = e.NewPageIndex;
        //    GridViewData.DataSource = (DataTable)ViewState["CSVGridData"];
        //    GridViewData.DataBind();
        //}

        protected void GridViewMapping_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMapping.PageIndex = e.NewPageIndex;
            GridViewMapping.DataSource = (DataTable)ViewState["CSVColData"];
            GridViewMapping.DataBind();
        }

        protected void GridViewMapped_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMapped.PageIndex = e.NewPageIndex;
            GridViewMapped.DataSource = (DataTable)ViewState["MappedData"];
            GridViewMapped.DataBind();
        }

        protected void buttonMapSelected_Click(object sender, EventArgs e)
        {
            DataTable csvColTable;
            string expression;
            DataRow[] filteredRows;

            string selectedSourceCol, selectedTargetCol;
            if (ViewState["CSVColData"] != null)
            {
                selectedSourceCol = ddlSourceCols.SelectedValue;
                selectedTargetCol = ddlTargetCols.SelectedValue;

                csvColTable = (DataTable)ViewState["CSVColData"];
                expression = "SourceCol = '" + selectedSourceCol + "'";
                filteredRows = csvColTable.Select(expression);
                if ((filteredRows != null) && (filteredRows.Length > 0))
                {
                    filteredRows[0]["TargetCol"] = selectedTargetCol;
                    csvColTable.AcceptChanges();
                }
                ViewState["CSVColData"] = csvColTable;

                GridViewMapping.DataSource = csvColTable;
                GridViewMapping.DataBind();
            }
        }

        protected void buttonConvert_Click(object sender, EventArgs e)
        {
            DataTable csvColTable;
            DataTable csvTable;
            string filename;
            string exchangeCode;
            try
            {
                csvColTable = (DataTable)ViewState["CSVColData"];
                csvTable = (DataTable)ViewState["CSVData"];

                if ((csvColTable == null) || (csvColTable.Rows.Count <= 0) || (csvTable == null) || (csvTable.Rows.Count <= 0))
                {
                    labelMessage.Text = "Problem while fetching data from imported file. Please try importing file again.";
                }
                else
                {
                    //first remove all columns that are not mapped from the csvtable
                    foreach (DataRow row in csvColTable.Rows)
                    {
                        if (row["TargetCol"].ToString().Length <= 0)
                        {
                            csvTable.Columns.Remove(row["SourceCol"].ToString());
                        }
                    }
                    csvTable.AcceptChanges();
                    //ViewState["CSVData"] = csvTable;
                    //ViewState["UploadedFileName"] = Session["PortfolioFolder"].ToString() + "\\" + FileUploadCSV.FileName;
                    filename = ViewState["UploadedFileName"].ToString();
                    filename = filename.ToLower().Replace(".csv", ".xml");
                    if (File.Exists(filename))
                    {
                        labelMessage.Text = "Data will be appended to existing file: " + filename;
                    }
                    //int i = 1;
                    //while ((File.Exists(filename)) || (i <= 20))
                    //{
                    //    if (i == 1)
                    //    {
                    //        filename = filename.Replace(".xml", "_" + i.ToString() + ".xml");
                    //    }
                    //    else
                    //    {
                    //        filename = filename.Replace("_" + (i - 1).ToString() + ".xml", "_" + i + ".xml");
                    //    }
                    //    i++;
                    //    if(i == 20)
                    //    {
                    //        labelMessage.Text = "Data will be appended to existing file:" + filename;
                    //    }
                    //}
                    if (ddlCountry.SelectedIndex >= 0)
                    {
                        exchangeCode = ddlCountry.SelectedValue;
                    }
                    else
                    {
                        exchangeCode = ".BSE";
                    }
                    if (StockApi.convertTableToPortfolio(filename, csvTable, csvColTable, exchangeCode, apiKey: Session["ApiKey"].ToString()))
                    {
                        labelMessage.Text = "Data uploaded to file - " + filename;

                        string folderPath = Server.MapPath("~/scriptdata/");
                        bool bIsTestOn = true;
                        if (Session["TestDataFolder"] != null)
                        {
                            folderPath = Session["TestDataFolder"].ToString();
                        }
                        if (Session["IsTestOn"] != null)
                        {
                            bIsTestOn = System.Convert.ToBoolean(Session["IsTestOn"]);
                        }

                        DataTable dt = StockApi.getPortfolioTable(folderPath, filename, false, bIsTestOn, apiKey: Session["ApiKey"].ToString());
                        if(dt != null)
                        {
                            GridViewMapped.DataSource = dt;
                            GridViewMapped.DataBind();
                            ViewState["MappedData"] = dt;
                        }
                    }
                    else
                    {
                        labelMessage.Text = "Failed to uploaded data to file - " + filename;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}