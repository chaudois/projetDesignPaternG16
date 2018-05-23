using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DAL;
using DTO;
namespace WinForm
{
    public partial class MainWindow : Form
    {
        ContactSQL contactSQL = new ContactSQL();
        private int nbControlsAdded = 0;
        private int spaceBetweenControls = 50;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AddContactListToView()
        {
            IEnumerable<ContactDTO> contacts = contactSQL.GetAll();

            foreach (var item in contacts)
            {
                Button boutonName = new Button();
                boutonName.Text = item.firstName + " " + item.lastName;
                boutonName.AutoSize = true;
                boutonName.Anchor = AnchorStyles.Right;
                boutonName.Anchor = AnchorStyles.Left;
                boutonName.Size = new Size(150, 25);
                boutonName.Location = new Point(0, spaceBetweenControls * nbControlsAdded);
                mainPanel.Controls.Add(boutonName);

                Button boutonRemove = new Button();
                
                boutonRemove.BackgroundImage= global::projetDesignPatern4AL1G16.Properties.Resources.Remove_icon;
                boutonRemove.Size = new Size(new Point(25, 25));
                boutonRemove.BackgroundImageLayout = ImageLayout.Stretch;
                boutonRemove.Click += (s,e)=> { RemoveContact( item); };
                
                boutonRemove.Location = new Point(310, spaceBetweenControls * nbControlsAdded);
                mainPanel.SetFlowBreak(boutonRemove,true);
                mainPanel.Controls.Add(boutonRemove);
                nbControlsAdded++;
 
            }
            AddAddButtonToView();

        }

        private void RemoveContact( ContactDTO item)
        {
            contactSQL.remove(item.id);
            mainPanel.Controls.Clear();
            AddContactListToView();

        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            AddContactListToView();
        }
        private TextBox setupTextBox(string nomBouton)
        {
            TextBox textbox = new TextBox();
            textbox.Location = new Point(0, spaceBetweenControls * nbControlsAdded);
            nbControlsAdded++;
            textbox.Text = nomBouton;
            textbox.ForeColor = Color.Gray;
            textbox.Enter += (t, args) =>
            {
                textbox.ForeColor = Color.Black;

                if (textbox.Text == nomBouton)
                {
                    textbox.Text = "";
                }
            };
            textbox.Leave += (t, args) =>
            {
                if (textbox.Text == "")
                {
                    textbox.Text = nomBouton;
                    textbox.ForeColor = Color.Gray;

                }
            };
            mainPanel.Controls.Add(textbox);
            mainPanel.SetFlowBreak(textbox, true);
            return textbox;
        }
        private void AddAddButtonToView()
        {
            TextBox textboxFirstName = setupTextBox("FirstName");
            TextBox textboxLastName  = setupTextBox("LastName");
            TextBox textboxAdresse = setupTextBox("Address");
            TextBox textboxMail = setupTextBox("Mail");
            TextBox textboxPhoneNum = setupTextBox("Tel");
            
            Button boutonValider = new Button();
            boutonValider.Text = "Ajouter";
            boutonValider.Click += (bouton, args) =>
              {
                  if ((textboxFirstName.Text != "FirstName" && textboxFirstName.Text != "")
                  || (textboxLastName.Text != "LastName" && textboxLastName.Text != ""))// il faut au moins un nom ou un prenom
                  {
                      contactSQL.Add(new ContactDTO
                      {
                          firstName = (textboxFirstName.Text != "FirstName" && textboxFirstName.Text != "") ? textboxFirstName.Text : "",
                          lastName = (textboxLastName.Text != "LastName" && textboxLastName.Text != "") ? textboxLastName.Text : "",
                          adresse = (textboxAdresse.Text != "Address" && textboxFirstName.Text != "") ? textboxAdresse.Text : "",
                          mail = (textboxMail.Text != "Mail" && textboxMail.Text != "") ? textboxMail.Text : "",
                          phoneNum = (textboxPhoneNum.Text != "Tel" && textboxPhoneNum.Text != "") ? textboxPhoneNum.Text : ""

                      });
                      mainPanel.Controls.Clear();
                      AddContactListToView();
                  }
              };
            mainPanel.Controls.Add(boutonValider);
 

        }
    }
}
