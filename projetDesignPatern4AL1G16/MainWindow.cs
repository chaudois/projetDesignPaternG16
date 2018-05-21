using System;
using System.Windows.Forms;

namespace projetDesignPatern4AL1G16
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            ScrollBar scrollBar = new VScrollBar();
            scrollBar.Dock = DockStyle.Right;
            ActiveForm.Controls.Add(scrollBar);
            ActiveForm.AutoScroll = true;
            for (int i = 0; i < 50; i++)
            {
                Button boutonName = new Button();
                boutonName.Text = "test " + i;
                boutonName.AutoSize = true;
                boutonName.Anchor = AnchorStyles.Right;
                boutonName.Anchor = AnchorStyles.Left;
                boutonName.Size = new System.Drawing.Size(300, 25);
                boutonName.Location = new System.Drawing.Point(0, spaceBetweenContacts * nbContactDisplayed);
                ActiveForm.Controls.Add(boutonName);
                nbContactDisplayed++;
            }
        }
    }
}
