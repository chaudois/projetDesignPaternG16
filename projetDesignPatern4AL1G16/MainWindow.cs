using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DAL;
using DTO;
namespace WinForm
{
    public partial class MainWindow : Form
    {
        private SqliteManager SQL = new SqliteManager();
        private int nbContactDisplayed = 0;
        private int spaceBetweenContacts = 35;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            ScrollBar scrollBar = new VScrollBar();
            scrollBar.Dock = DockStyle.Right;
            ActiveForm.AutoScroll = true;
            IEnumerable<ContactDTO> contacts = new ContactSQL().GetAll();

            foreach (var item in contacts)
            {
                Button boutonName = new Button();
                boutonName.Text = item.firstName+" "+item.lastName;
                boutonName.AutoSize = true;
                boutonName.Anchor = AnchorStyles.Right;
                boutonName.Anchor = AnchorStyles.Left;
                boutonName.Size = new System.Drawing.Size(300, 25);
                boutonName.Location = new System.Drawing.Point(0, spaceBetweenContacts * nbContactDisplayed);
                mainPanel.Controls.Add(boutonName);
                nbContactDisplayed++;
            }
        }
    }
}
