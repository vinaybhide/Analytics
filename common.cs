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