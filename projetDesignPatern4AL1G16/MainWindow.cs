﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DAL;
using DTO;
using BO;

namespace WinForm
{
    public partial class MainWindow : Form
    {
        private const int SPACE_BETWEEN_CONTROLS = 50;
        private const int TEXT_INFO_WIDTH = 70;
        private SqliteManager SqliteManager = new SqliteManager();
        private List<CheckBox> checkBox = new List<CheckBox>();
        ContactSQL contactSQL = new ContactSQL();
        int nbControlsAddedMainPanel = 0;
        int nbControlsAddedInfoPanel = 0;
        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// ajoute tout les contact contenues en base au main panel ainsi que le formualire d'ajout
        /// </summary>
        private void DisplayMain()
        {
            mainPanel.Controls.Clear();
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
                    DisplayInfo(item.id, boutonName);
                };
                mainPanel.Controls.Add(boutonName);


                Button boutonClone = new Button();
                boutonClone.BackgroundImage = projetDesignPatern4AL1G16.Properties.Resources.arrow_return_left;
                boutonClone.Size = new Size(new Point(25, 25));
                boutonClone.BackgroundImageLayout = ImageLayout.Stretch;
                boutonClone.Click += (s, e) => { CloneContact(item); };
                boutonClone.Anchor = AnchorStyles.Left;
                boutonClone.Location = new Point(310, SPACE_BETWEEN_CONTROLS * nbControlsAddedMainPanel);
                mainPanel.Controls.Add(boutonClone);

                Button boutonRemove = new Button();
                boutonRemove.BackgroundImage = projetDesignPatern4AL1G16.Properties.Resources.Remove_icon;
                boutonRemove.Size = new Size(new Point(25, 25));
                boutonRemove.BackgroundImageLayout = ImageLayout.Stretch;
                boutonRemove.Click += (s, e) => { RemoveContact(item); };
                boutonRemove.Anchor = AnchorStyles.Left;
                boutonRemove.Location = new Point(350, SPACE_BETWEEN_CONTROLS * nbControlsAddedMainPanel);
                mainPanel.Controls.Add(boutonRemove);



                mainPanel.SetFlowBreak(boutonRemove, true);
                nbControlsAddedMainPanel++;

            }
            AddFormToMainPanel();
        }
        private void DisplayExportImport()
        {
            panelExport.Controls.Clear();
            var contacts = contactSQL.GetAll();
            foreach (var item in contacts)//ajoute tout les contact à la fenetre principal
            {

                CheckBox wantToExport = new CheckBox();
                wantToExport.Text = "";
                wantToExport.Size = new Size(20, 20);
                wantToExport.Name = item.id.ToString();
                checkBox.Add(wantToExport);

                Label labelName = new Label();
                labelName.Text = item.firstName + " " + item.lastName;
                labelName.Anchor = AnchorStyles.Left;
                labelName.Margin = new Padding(0, 3, 3, 3);


                panelExport.Controls.Add(wantToExport);
                panelExport.Controls.Add(labelName);

                panelExport.SetFlowBreak(labelName, true);
                nbControlsAddedMainPanel++;

            }

            Button buttonExportJSON = new Button();
            buttonExportJSON.Text = "Exporter JSON";
            buttonExportJSON.AutoSize = true;
            buttonExportJSON.Click += (e, s) =>
            {
                List<ContactDTO> listToExport = new List<ContactDTO>();
                foreach (var CB in checkBox)
                {
                    if (CB.Checked)
                    {
                        listToExport.Add(new ContactSQL().Get(int.Parse(CB.Name)));
                    }
                }
                new Export(new JSONWork(), listToExport).ExportContact();
 
            };

            Button buttonExportCSV = new Button();
            buttonExportCSV.Text = "Exporter CSV";
            buttonExportCSV.AutoSize = true;
            buttonExportCSV.Click += (e, s) =>
            {
                List<ContactDTO> listToExport = new List<ContactDTO>();
                foreach (var CB in checkBox)
                {
                    if (CB.Checked) 
                    {
                        listToExport.Add(new ContactSQL().Get(int.Parse(CB.Name)));
                    }
                }
                new Export(new CSVWork(), listToExport).ExportContact();

            };

            Button buttonImport = new Button();
            buttonImport.Click += (e, s) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog.FileName))
                    {
                        string dataToImport = sr.ReadToEnd();
                        List<ContactDTO> result = new Import(dataToImport).ImportContact();
                        foreach (var item in result)
                        {
                            new ContactSQL().Add(item);
                        }
                    }
                }
                DisplayExportImport();
            };
            buttonImport.Text = "Importer";

            panelExport.Controls.Add(buttonExportJSON);
            panelExport.Controls.Add(buttonExportCSV);
            panelExport.Controls.Add(buttonImport);
        }
        private void CloneContact(ContactDTO item)
        {
            new ContactSQL().Add((ContactDTO)item.Clone());
            mainPanel.Controls.Clear();
            DisplayMain();
        }

        /// <summary>
        /// ajoute toutes les textbox au panel info
        /// </summary>
        /// <param name="contact">contact qui preremplira les textbox et qui sera update à l'appui du bouton</param>
        private void DisplayInfo(int idcontact, Button origin)
        {
            ContactDTO contact = new ContactSQL().Get(idcontact);
            InfoPanel.Controls.Clear();
            Button boutonUpdate = new Button();
            TextBox boxNewName = SetupPlaceHolder("nouveau champ");
            TextBox boxNewValue = new TextBox();
            Button boutonAddField = new Button();

            //ajoute les textbox qui contiendront les champs du contact, avec nom et prenom par default
            Dictionary<string, TextBox> textBoxInfo = new Dictionary<string, TextBox>();


            foreach (FieldDTO field in contact)
            {
                try
                {
                    textBoxInfo.Add(field.name, SetupTextBoxInfoPanel(field));

                }
                catch
                {

                }
            }

            //creer et place les textbox d'ajout de champs pour ce contact
            boxNewName.Location = new Point(0, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            boxNewValue.Location = new Point(TEXT_INFO_WIDTH, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            InfoPanel.SetFlowBreak(boxNewValue, true);
            nbControlsAddedInfoPanel++;
            boxNewName.Hide();
            boxNewValue.Hide();

            boutonAddField.Text = "Ajouter";
            boutonAddField.Location = new Point(TEXT_INFO_WIDTH, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            boutonAddField.Click += (e, s) =>
            {
                boxNewName.Show();
                boxNewValue.Show();
                boutonAddField.Hide();
                nbControlsAddedInfoPanel++;

                boutonUpdate.Location = new Point(TEXT_INFO_WIDTH * 2, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            };

            InfoPanel.Controls.Add(boxNewName);
            InfoPanel.Controls.Add(boxNewValue);
            InfoPanel.Controls.Add(boutonAddField);


            //creer et place le bouton qui enregistre les modif faites sur le contact
            boutonUpdate.Text = "Enregistrer";
            boutonUpdate.AutoSize = true;
            boutonUpdate.Location = new Point(TEXT_INFO_WIDTH * 2, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            boutonUpdate.Click += (e, s) =>
            {
                ContactDTO newContact = new ContactDTO
                {
                    id = contact.id,
                    firstName = textBoxInfo["FirstName"].Text == "FirstName" ? "" : textBoxInfo["FirstName"].Text,
                    lastName = textBoxInfo["LastName"].Text == "LastName" ? "" : textBoxInfo["LastName"].Text
                };
                foreach (var item in textBoxInfo.Keys)
                {
                    if (item != "FirstName" && item != "LastName")
                    {

                        newContact.fields.Add(new FieldDTO
                        {
                            idContact = newContact.id,
                            name = item,
                            value = textBoxInfo[item].Text
                        });
                    }
                }
                if (boxNewValue.Text != "" && boxNewName.Text != "nouveau champ")
                {
                    newContact.fields.Add(new FieldDTO
                    {
                        idContact = newContact.id,
                        name = boxNewName.Text,
                        value = boxNewValue.Text
                    });
                    new FieldSQL().Add(new FieldDTO
                    {
                        idContact = newContact.id,
                        name = boxNewName.Text,
                        value = boxNewValue.Text
                    });
                }


                contactSQL.update(newContact);
                InfoPanel.Controls.Clear();
                DisplayInfo(newContact.id, null);
                if (origin != null)
                {
                    origin.Text = newContact.firstName + " " + newContact.lastName;
                }
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
        /// <param name="name">text à afficher sur le textbox</param>
        /// <returns></returns>
        private TextBox SetupTextBoxInfoPanel(FieldDTO field)
        {
            TextBox textbox = new TextBox();
            Label label = new Label();

            textbox.Text = field.value;
            textbox.Name = field.name;
            label.Text = field.name;
            label.Width = TEXT_INFO_WIDTH;
            label.AutoEllipsis = true;
            label.Location = new Point(0, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            textbox.Location = new Point(TEXT_INFO_WIDTH, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);



            Button boutonRemove = new Button();
            boutonRemove.BackgroundImage = projetDesignPatern4AL1G16.Properties.Resources.Remove_icon;
            boutonRemove.Size = new Size(new Point(25, 25));
            boutonRemove.BackgroundImageLayout = ImageLayout.Stretch;
            boutonRemove.Click += (s, e) =>
            {
                new FieldSQL().remove(field);
                InfoPanel.Controls.Clear();
                DisplayInfo(field.idContact, null);
            };
            boutonRemove.Anchor = AnchorStyles.Left;
            boutonRemove.Location = new Point(TEXT_INFO_WIDTH * 2, SPACE_BETWEEN_CONTROLS * nbControlsAddedMainPanel);



            InfoPanel.Controls.Add(label);
            InfoPanel.Controls.Add(textbox);
            InfoPanel.Controls.Add(boutonRemove);

            nbControlsAddedInfoPanel++;
            InfoPanel.SetFlowBreak(boutonRemove, true);

            return textbox;
        }
        /// <summary>
        /// supprime le contact de la base de donné et du panel
        /// </summary>
        /// <param name="item">contact à supprimer</param>
        private void RemoveContact(ContactDTO item)
        {
            contactSQL.remove(item);
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
            TextBox textboxLastName = setupTextBoxMainPanel("LastName");
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
                          fields = new List<FieldDTO>
                          {
                              new FieldDTO
                              {
                                  name="Adresse",
                                  value=(textboxAdresse.Text != "Address" && textboxFirstName.Text != "") ? textboxAdresse.Text : ""
                              },new FieldDTO
                              {
                                  name="Mail",
                                  value= (textboxMail.Text != "Mail" && textboxMail.Text != "") ? textboxMail.Text : ""
                              },new FieldDTO
                              {
                                  name="Tel",
                                  value=(textboxPhoneNum.Text != "Tel" && textboxPhoneNum.Text != "") ? textboxPhoneNum.Text : ""
                              }

                          }

                      });
                      mainPanel.Controls.Clear();
                      DisplayMain();
                  }
              };
            mainPanel.Controls.Add(boutonValider);


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs eve)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                DisplayMain();

            }
            else
            {
                DisplayExportImport();
            }
        }

        private void tabControl1_Enter(object sender, EventArgs e)
        {
            DisplayMain();
        }
    }
}
