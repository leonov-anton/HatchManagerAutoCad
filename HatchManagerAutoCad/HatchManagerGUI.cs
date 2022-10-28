using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HatchManagerAutoCad
{
    public partial class HatchManagerGUI : Form
    {

        Sqliter db = new Sqliter();

        private string chapterName { get; set; }
        private string domainName { get; set; }
        private string groupeName { get; set; }
        private string imgDirPath { get; set; }

        public HatchManagerGUI()
        {
            InitializeComponent();
        }

        private void updateChapters()
        {
            foreach (string chapt in db.getChapters())
                comboBoxChapter.Items.Add(chapt);
            if (comboBoxChapter.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(chapterName))
                {
                    comboBoxChapter.Text = chapterName;
                    chapterName = null;
                }
                else
                    comboBoxChapter.SelectedIndex = 0;
            }
        }

        private void updateDomains()
        {
            listBoxDomain.Items.Clear();
            foreach (string domain in db.getDomains((string)comboBoxChapter.SelectedItem))
                listBoxDomain.Items.Add(domain);
            if (listBoxDomain.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(domainName))
                {
                    listBoxDomain.Text = domainName;
                    domainName = null;
                }
                else
                    listBoxDomain.SelectedIndex = 0;
            }
        }

        private void updateGroups()
        {
            listBoxGroupe.Items.Clear();
            foreach (string group in db.getGroups((string)listBoxDomain.SelectedItem, (string)comboBoxChapter.SelectedItem))
                listBoxGroupe.Items.Add(group);
            if (listBoxGroupe.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(groupeName))
                {
                    listBoxGroupe.Text = groupeName;
                    groupeName = null;
                }
                else
                    listBoxGroupe.SelectedIndex = 0;
            }
        }

        private void updateHatchs()
        {
            dataGridViewHatchData.ClearSelection();
            dataGridViewHatchData.Rows.Clear();
            pictureBox.Image = null;
            int n = 0;
            foreach (List<string> hatchData in db.getHatchsData((string)listBoxGroupe.SelectedItem))
            {
                dataGridViewHatchData.Rows.Add(hatchData[0], hatchData[1], hatchData[2], hatchData[3]);
                try
                {
                    Bitmap img = new Bitmap($"{imgDirPath}\\{hatchData[4]}.png");
                    dataGridViewHatchData.Rows[n].Cells[4].Value = img;
                }
                catch { }
                dataGridViewHatchData.Rows[n].Height = 60;
                n++;
            }
            if (dataGridViewHatchData.Rows.Count > 0)
                dataGridViewHatchData.Rows[0].Selected = true;
        }

        private void updatePictureBox()
        {
            if (dataGridViewHatchData.Rows.Count > 0)
                pictureBox.Image = (Image)dataGridViewHatchData.SelectedRows[0].Cells[4].Value;
        }

        private void HatchManagerGUI_Load(object sender, EventArgs e)
        {
            string[] userPath = db.getUserDir(Environment.UserName);
            chapterName = userPath[0];
            domainName = userPath[1];
            groupeName = userPath[2];
            string imgRelisePath = @"G:\BIM\01_BIM Library\02_CIVIL3D\01_AUTOCAD\04_ШТРИХОВКИ\01_БАЗА ДАННЫХ\landscape";
            if (Directory.Exists(imgRelisePath))
                imgDirPath = imgRelisePath;
            else
                imgDirPath = @"D:\YandexDisk\C#_projects\AutoCad\HatchManagerAutoCad\HatchManagerAutoCad\bin\Debug\base\landscape";
            updateChapters();
        }

        private void chapter_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDomains();
        }

        private void listBoxDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGroups();
        }

        private void listBoxGroupe_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateHatchs();
        }

        private void dataGridViewHatchData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            updatePictureBox();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            HatchManager hatchManager = new HatchManager(db.getHatchData((string)dataGridViewHatchData.SelectedRows[0].Cells[0].Value, 
                                                        (string)listBoxGroupe.SelectedItem));
            hatchManager.CreateNewHatch();
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            HatchManager hatchManager = new HatchManager(db.getHatchData((string)dataGridViewHatchData.SelectedRows[0].Cells[0].Value, 
                                                        (string)listBoxGroupe.SelectedItem));
            hatchManager.ChangeHatch();
        }

        private void buttonSetOD_Click(object sender, EventArgs e)
        {
            HatchManager hatchManager = new HatchManager(db.getHatchData((string)dataGridViewHatchData.SelectedRows[0].Cells[0].Value, 
                                                        (string)listBoxGroupe.SelectedItem));
            hatchManager.SetOdataTable();
        }

        private void HatchManagerGUI_FormClosing(Object sender, FormClosingEventArgs e)
        {
            db.setUserPath(Environment.UserName, (string)listBoxGroupe.SelectedItem);
        }
    }
}
