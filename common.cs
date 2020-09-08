using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Analytics
{
    public static class common
    {
        public static string noLogin = "You must login to the application to use this feature";
        public static string noUserMatch = "Incorrect Email Id or Password. Please try again or register for an account.";
        public static string userExists = "Email id already registered for another account. Please use different email id.";
        public static string noPortfolioName = "Please login to application and then select portfolio in which you want to add stock entry.";
        public static string noPortfolioNameToOpen = "Please select valid portfolio.";
        public static string noSymbolFound = "No matching stock symbols found";
        public static string noTextSearchSymbol = "Enter text in Search Stock to search stock symbol";
        public static string noStockSelectedToAdd = "Please search & then select script to add.";
        public static string noStockSelectedToShowGraph = "Please search & then select script to show graph.";
        public static string noStockSelectedToGetQuote = "Please search & then select script to add.";
        public static string noPortfolioSelectedToDelete = "Please select valid portfolio to delete.";
        public static string testFlagTrue = "To use this feature login to application with \'Off-line mode?\' option un-checked.";
        public static string noStockSelectedToDownload = "Please search & then select script to download data.";
        public static string noQuoteAvailable = "Not able to get quote from market at this time, please try again later.";
        public static string noValidNewPortfolioName = "Please enter valid portfolio name.";
        public static string noValidKey = "Enter valid key!";
        public static string noScriptSelectedInformationEntered = "Please make sure you have selected script and entered all information.";
        public static string errorEditScript = "Error while updating the transaction.Please try again or hit back.";
        public static string errorAllFieldsMandatory = "All fields are mandatory.";
        public static string registrationComplete = "Registration complete with free Alpha Vantage API key.You can now login to application.Free Alpha Vantage key has limitations. Please use Admin->Add Key to add your AlphaVantage API key.";
        public static string portfolioExists = "Portfolio already exists.";
        public static string noScriptsInPortfolio = "No scripts found in the portfolio";
        public static string noPortfolioSelected = "Please select portfolio from the list";
        public static string noTxnSelected = "Please select a transaction to delete.";

        /// <summary>
        /// Shows a basic MessageBox on the passed in page
        /// </summary>
        /// <param name="page">The Page object to show the message on</param>
        /// <param name="message">The message to show</param>
        /// <returns></returns>
        public static void ShowMessageBox(Page page, string message)
        {
            Type cstype = page.GetType();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = page.ClientScript;

            // Find the first unregistered script number
            int ScriptNumber = 0;
            bool ScriptRegistered = false;
            do
            {
                ScriptNumber++;
                ScriptRegistered = cs.IsStartupScriptRegistered(cstype, "PopupScript" + ScriptNumber);
            } while (ScriptRegistered == true);

            //Execute the new script number that we found
            cs.RegisterStartupScript(cstype, "PopupScript" + ScriptNumber, "alert('" + message + "');", true);
        }
    }
}