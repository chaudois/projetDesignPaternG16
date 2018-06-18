using System;
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
        private const int TEXT_INFO_WIDTH=70;
        private SqliteManager SqliteManager = new SqliteManager();

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
                    DisplayInfo(item, boutonName);
                };
                mainPanel.Controls.Add(boutonName);

                Button boutonClone = new Button();
                Button boutonRemove = new Button();

                boutonClone.BackgroundImage = projetDesignPatern4AL1G16.Properties.Resources.arrow_return_left;
                boutonClone.Size = new Size(new Point(25, 25));
                boutonClone.BackgroundImageLayout = ImageLayout.Stretch;
                boutonClone.Click += (s, e) => { CloneContact(item); };
                boutonClone.Anchor = AnchorStyles.Left;

                boutonRemove.BackgroundImage = projetDesignPatern4AL1G16.Properties.Resources.Remove_icon;
                boutonRemove.Size = new Size(new Point(25, 25));
                boutonRemove.BackgroundImageLayout = ImageLayout.Stretch;
                boutonRemove.Click += (s, e) => { RemoveContact(item); };
                boutonRemove.Anchor = AnchorStyles.Left;

                boutonClone.Location = new Point(310, SPACE_BETWEEN_CONTROLS * nbControlsAddedMainPanel);
                boutonRemove.Location = new Point(350, SPACE_BETWEEN_CONTROLS * nbControlsAddedMainPanel);

                mainPanel.Controls.Add(boutonClone);
                mainPanel.Controls.Add(boutonRemove);

                mainPanel.SetFlowBreak(boutonRemove, true);
                nbControlsAddedMainPanel++;

            }
            AddFormToMainPanel();
        }

        private void CloneContact(ContactDTO item)
        {
            new ContactSQL().Add((ContactDTO) item.Clone());
            mainPanel.Controls.Clear();
            DisplayMain();
        }

        /// <summary>
        /// ajoute toutes les textbox au panel info
        /// </summary>
        /// <param name="contact">contact qui preremplira les textbox et qui sera update à l'appui du bouton</param>
        private void DisplayInfo(ContactDTO contact,Button origin)
        {
            InfoPanel.Controls.Clear();
            Button boutonUpdate = new Button();
            TextBox boxNewName = SetupPlaceHolder("nouveau champ");
            TextBox boxNewValue = new TextBox();
            Button boutonAddField = new Button();

            //ajoute les textbox qui contiendront les champs du contact, avec nom et prenom par default
            Dictionary<string,TextBox> textBoxInfo = new Dictionary<string,TextBox>
            {
                {"FirstName", SetupTextBoxInfoPanel("FirstName",contact.firstName) },
                {"LastName", SetupTextBoxInfoPanel("LastName",contact.lastName) }
            };
            foreach (var field in contact.fields)
            {
                textBoxInfo.Add(field.name,SetupTextBoxInfoPanel(field.name,field.value));
            }

            //creer et place les textbox d'ajout de champs pour ce contact

            boxNewName.Location = new Point(0, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            boxNewValue.Location = new Point(TEXT_INFO_WIDTH, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            InfoPanel.SetFlowBreak(boxNewValue,true);
            nbControlsAddedInfoPanel++;
            boxNewName.Hide();
            boxNewValue.Hide();

            boutonAddField.Text = "Ajouter";
            boutonAddField.Location= new Point(TEXT_INFO_WIDTH, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
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
            boutonUpdate.Location = new Point(TEXT_INFO_WIDTH*2, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
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
                    if(item!="FirstName" && item != "LastName")
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
                DisplayInfo(newContact,null);
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
        private TextBox SetupTextBoxInfoPanel(string name, string value)
        {
            TextBox textbox = new TextBox();
            Label label = new Label();

            textbox.Text = value;
            textbox.Name = name;
            label.Text = name;
            label.Width = TEXT_INFO_WIDTH;
            label.AutoEllipsis = true;
            label.Location = new Point(0, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);
            textbox.Location = new Point(TEXT_INFO_WIDTH, nbControlsAddedInfoPanel * SPACE_BETWEEN_CONTROLS);


            InfoPanel.Controls.Add(label);
            InfoPanel.Controls.Add(textbox);

            nbControlsAddedInfoPanel++;
            InfoPanel.SetFlowBreak(textbox, true);

            return textbox;
        }
        /// <summary>
        /// supprime le contact de la base de donné et du panel
        /// </summary>
        /// <param name="item">contact à supprimer</param>
        private void RemoveContact(ContactDTO item)
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
    }
}
