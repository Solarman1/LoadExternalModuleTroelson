using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonSnappableTypes;
using System.Reflection;

namespace MyExtendableApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void snapInModuleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // let the choose assembly to user
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.FileName.Contains("CommonSnappableTypes"))
                    //CommnonSnappableTypes not have a rigging
                    MessageBox.Show("CommonSnappableTypes has no snap-ins!");
                else if (!LoadExternalModule(dlg.FileName))
                    //
                    MessageBox.Show("Nothing implements IAppFunctionality");
            }
        }

        private bool LoadExternalModule(string path)
        {
            bool foundSnapIn = false;
            Assembly theSnapInAsm = null;
            try
            {
                //Динамически загрузить выбранную сборку
                theSnapInAsm = Assembly.LoadFrom(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return foundSnapIn;
            }

            //Получить все совместные с IAppFunctionality классы в сборке
            var theClassTypes = from t in theSnapInAsm.GetTypes()
                                where t.IsClass &&
                                (t.GetInterface("IAppFunctionality") != null)
                                select t;

            foreach (Type t in theClassTypes)
            {
                foundSnapIn = true;

                //Использовать позднее связывание для создания экземпляра типа.

                IAppFunctionality itfApp =
                    (IAppFunctionality)theSnapInAsm.CreateInstance(t.FullName, true);
                itfApp.DoIt();
                lstLoadedSnapIns.Items.Add(t.FullName);
            }
            return foundSnapIn;
        }




        private void commonSnappableTypesdllToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
