using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analytics.Graphs
{
    public partial class standardgraphs : System.Web.UI.MasterPage
    {
        public HiddenField panelWidth
        {
            get
            {
                // Return the textbox on the master page
                return this.panelWidthM;
            }
        }

        public HiddenField panelHeight
        {
            get
            {
                // Return the textbox on the master page
                return this.panelHeightM;
            }
        }

        public Label headingtext
        {
            get
            {
                // Return the textbox on the master page
                return this.headingtextM;
            }
        }

        public TextBox textboxFromDate
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxFromDateM;
            }
        }

        public TextBox textboxToDate
        {
            get
            {
                // Return the textbox on the master page
                return this.textboxToDateM;
            }
        }
        public BulletedList bulletedlistDesc
        {
            get
            {
                // Return the textbox on the master page
                return this.bulletedlistDescM;
            }
        }
        public CheckBoxList checkboxlistLines
        {
            get
            {
                // Return the textbox on the master page
                return this.checkboxlistLinesM;
            }
        }
        public Button buttonDesc
        {
            get
            {
                // Return the textbox on the master page
                return this.buttonDesc;
            }
        }
        public Button buttonShowGrid
        {
            get
            {
                // Return the textbox on the master page
                return this.buttonShowGridM;
            }
        }

        public delegate void DoEventShowGraph();
        public event DoEventShowGraph OnDoEventShowGraph;

        public delegate void DoEventShowGrid();
        public event DoEventShowGrid OnDoEventShowGrid;

        public delegate void DoEventToggleDesc();
        public event DoEventToggleDesc OnDoEventToggleDesc;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void buttonShowGraph_Click(object sender, EventArgs e)
        {
            if (OnDoEventShowGraph != null)
            {
                OnDoEventShowGraph();
            }
        }

        protected void buttonShowGrid_Click(object sender, EventArgs e)
        {
            if (OnDoEventShowGrid != null)
            {
                OnDoEventShowGrid();
            }
        }

        protected void buttonDescM_Click(object sender, EventArgs e)
        {
            if (OnDoEventToggleDesc != null)
            {
                OnDoEventToggleDesc();
            }
        }
    }
}