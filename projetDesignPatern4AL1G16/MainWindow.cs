﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DAL;
using DTO;
namespace WinForm
{
    public partial class MainWindow : Form
    {
        private const int SPACE_BETWEEN_CONTROLS = 50;


        ContactSQL contactSQL = new ContactSQL();
        int nbControlsAddedMainPanel = 0;
        int nbControlsAddedInfoPanel = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            DisplayMain();
        }
        /// <summary>
        /// ajoute tout les contact contenues en base au main panel ainsi que le formualire d'ajout
        /// </summary>
        private void DisplayMain()
        {
            var contacts = contactSQL.GetAll();

            foreach (var item in contacts)//ajoute tout les contact à la fenetre principal
            {
                Button boutonName = new Button();
                boutonName.Text = item.firstName + " " + item.lastName;
                boutonName.AutoSize = true;
                boutonName.Anchor = AnchorStyles.Right;
                boutonName.Anchor = AnchorStyles.Left;
                boutonName.Size = new Size(150, 25);
                boutonName.Location = new Point(0, SPACE_BETWEEN_CONTROLS * nbControlsAddedMainPanel);
                boutonName.Click += (e, s) =>
                {
                    DisplayInfo(item);
                };
                mainPanel.Controls.Add(boutonName);

                Button boutonRemove = new Button();
                
                boutonRemove.BackgroundImage= global::projetDesignPatern4AL1G16.Properties.Resources.Remove_icon;
                boutonRemove.Size = new Size(new Point(25, 25));
                boutonRemove.BackgroundImageLayout = ImageLayout.Stretch;
                boutonRemove.Click += (s,e)=> { RemoveContact( item); };
                
                boutonRemove.Location = new Point(310, SPACE_BETWEEN_CONTROLS * nbControlsAddedMainPanel);

                mainPanel.SetFlowBreak(boutonRemove,true);
                mainPanel.Controls.Add(boutonRemove);
                nbControlsAddedMainPanel++;
 
            }
            AddFormToMainPanel();

        }
        /// <summary>
        /// ajoute toutes les textbox au panel info
        /// </summary>
        /// <param name="item">contact qui preremplira les textbox et qui sera update à l'appui du bouton</param>
        private void DisplayInfo(ContactDTO item)
        {
            InfoPanel.Controls.Clear();
            TextBox textBoxInfoFirstName = SetupTextBoxInfoPanel("FirstName");
            TextBox textBoxInfoLastName = SetupTextBoxInfoPanel("LastName");
            TextBox textBoxInfoAddress = SetupTextBoxInfoPanel("Adresse");
            TextBox textBoxInfoMail = SetupTextBoxInfoPanel("Mail");
            TextBox textBoxInfoTel = SetupTextBoxInfoPanel("Tel");

            if (item.firstName != "") textBoxInfoFirstName.Text = item.firstName;
            if (item.lastName != "") textBoxInfoLastName.Text = item.lastName;
            if (item.mail != "") textBoxInfoMail.Text = item.mail;
            if (item.phoneNum != "") textBoxInfoTel.Text = item.phoneNum;
            if (item.adresse != "") textBoxInfoAddress.Text = item.adresse;

            Button boutonUpdate = new Button();
            boutonUpdate.Text = "Mettre à jour";
            boutonUpdate.AutoSize = true;
            boutonUpdate.Location = new Point(0, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            boutonUpdate.Click += (e, s) =>
            {
                ContactDTO newContact = new ContactDTO
                {
                    id = item.id,
                    firstName = textBoxInfoFirstName.Text == "FirstName" ? "" : textBoxInfoFirstName.Text,
                    lastName = textBoxInfoLastName.Text == "LastName" ? "" : textBoxInfoLastName.Text,
                    adresse = textBoxInfoAddress.Text == "Adresse" ? "" : textBoxInfoAddress.Text,
                    mail = textBoxInfoMail.Text == "Mail" ? "" : textBoxInfoMail.Text,
                    phoneNum = textBoxInfoTel.Text == "Tel" ? "" : textBoxInfoTel.Text
                };
                contactSQL.update(newContact);
                mainPanel.Controls.Clear();
                InfoPanel.Controls.Clear();
                DisplayInfo(newContact);
                DisplayMain();
            };
            InfoPanel.Controls.Add(boutonUpdate);


        }
        /// <summary>
        /// creer un textbox avec un placeholder
        /// </summary>
        /// <param name="text">text du placeholder</param>
        /// <returns></returns>
        private TextBox SetupPlaceHolder(string text)
        {
            TextBox textbox = new TextBox();
            textbox.Text = text;
            textbox.ForeColor = Color.Gray;


            textbox.Enter += (t, args) =>
            {
                textbox.ForeColor = Color.Black;

                if (textbox.Text == text)
                {
                    textbox.Text = "";
                }
            };
            textbox.Leave += (t, args) =>
            {
                if (textbox.Text == "")
                {
                    textbox.Text = text;
                    textbox.ForeColor = Color.Gray;

                }
            };
            return textbox;
        }
        /// <summary>
        /// creer et ajoute un textbox sur l'infoPanel
        /// </summary>
        /// <param name="text">text à afficher sur le textbox</param>
        /// <returns></returns>
        private TextBox SetupTextBoxInfoPanel(string text)
        {
            TextBox textbox = SetupPlaceHolder(text);
            textbox.Location = new Point(0, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            nbControlsAddedInfoPanel++;
            InfoPanel.Controls.Add(textbox);
            InfoPanel.SetFlowBreak(textbox, true);
            return textbox;
        }
        /// <summary>
        /// supprime le contact de la base de donné et du panel
        /// </summary>
        /// <param name="item">contact à supprimer</param>
        private void RemoveContact( ContactDTO item)
        {
            contactSQL.remove(item.id);
            mainPanel.Controls.Clear();
            InfoPanel.Controls.Clear();
            DisplayMain();

        }
        /// <summary>
        /// creer un textbox et l'insert dans le mainPanel
        /// </summary>
        /// <param name="nomTextBox">text à afficher dans le placeholder du textBox</param>
        /// <returns></returns>
        private TextBox setupTextBoxMainPanel(string nomTextBox)
        {
            TextBox textbox = SetupPlaceHolder(nomTextBox);
            textbox.Location = new Point(0, SPACE_BETWEEN_CONTROLS * nbControlsAddedMainPanel);
            nbControlsAddedMainPanel++;
            mainPanel.Controls.Add(textbox);
            mainPanel.SetFlowBreak(textbox, true);
            return textbox;
        }
        /// <summary>
        /// ajoute le formulaire d'ajout au mainPanel
        /// </summary>
        private void AddFormToMainPanel()
        {
            TextBox textboxFirstName = setupTextBoxMainPanel("FirstName");
            TextBox textboxLastName  = setupTextBoxMainPanel("LastName");
            TextBox textboxAdresse = setupTextBoxMainPanel("Address");
            TextBox textboxMail = setupTextBoxMainPanel("Mail");
            TextBox textboxPhoneNum = setupTextBoxMainPanel("Tel");
            
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
                      DisplayMain();
                  }
              };
            mainPanel.Controls.Add(boutonValider);
 

        }
    }
}
