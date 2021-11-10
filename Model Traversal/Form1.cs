using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Model_Traversal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SldWorks swApp;

            swApp = new SolidWorks.Interop.sldworks.SldWorks();

            TraverseAssembly(swApp as SldWorks);
        }

        private void TraverseAssembly(SldWorks swApp)
        {
            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            swModel = (ModelDoc2)swApp.ActiveDoc;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent();

            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                var timer = new Stopwatch();
                timer.Start();
                string line = new string('-', 120);

                listView1.Items.Add(line);
                listView1.Items.Add("Reading model:");
                listView1.Items.Add(swModel.GetPathName());
                listView1.Items.Add(line);

                TraverseComponent(swRootComp, 0);

                timer.Stop();

                TimeSpan timeTaken = timer.Elapsed;
                listView1.Items.Add(line);
                listView1.Items.Add("model read in " + timeTaken.ToString(@"m\:ss\.f"));
            }
        }

        public void TraverseComponent(Component2 parent, int nLevel)
        {
            object[] childrenArray;
            
            childrenArray = (object[])parent.GetChildren();

            string padding = new string('-', 2*nLevel);

            foreach (Component2 child in childrenArray)
            {
                string row = padding + child.Name2;

                ListViewItem lvi = new ListViewItem(row);
                listView1.Items.Add(lvi);

                TraverseComponent(child, nLevel + 1);
            }
        }
    }
}
