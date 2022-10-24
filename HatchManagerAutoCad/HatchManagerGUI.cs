﻿using Autodesk.AutoCAD.DatabaseServices.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HatchManagerAutoCad
{
    public partial class HatchManagerGUI : Form
    {

        Sqliter db = new Sqliter();

        public HatchManagerGUI()
        {
            InitializeComponent();
        }

        private void updateChapters()
        {
            foreach (string chapt in db.getChapters())
                comboBoxChapter.Items.Add(chapt);
            if (comboBoxChapter.Items.Count > 0)
                comboBoxChapter.SelectedIndex = 0;
        }

        private void updateDomains()
        {
            listBoxDomain.Items.Clear();
            foreach (string domain in db.getDomains(comboBoxChapter.SelectedText))
                listBoxDomain.Items.Add(domain);
            if (listBoxDomain.Items.Count > 0)
                listBoxDomain.SelectedIndex = 0;
        }

        private void updateGroups()
        {
            listBoxGroupe.Items.Clear();
            foreach (string group in db.getGroups(listBoxDomain.SelectedItem.ToString()))
                listBoxGroupe.Items.Add(group);
            if (listBoxGroupe.Items.Count > 0)
                listBoxGroupe.SelectedIndex = 0;
        }

        private void updateHatchs()
        {
            dataGridViewHatchData.ClearSelection();
            dataGridViewHatchData.Rows.Clear();
            pictureBox.Image = null;
            int n = 0;
            foreach (List<string> hatchData in db.getHatchsData(listBoxDomain.SelectedItem.ToString()))
            {
                dataGridViewHatchData.Rows.Add(hatchData[0], hatchData[1], hatchData[2], hatchData[3]);
                Bitmap img = new Bitmap(Application.StartupPath + $"{hatchData[4]}.png");
                dataGridViewHatchData.Rows[n].Cells[4].Value = img;
            }
        }

        private void updatePictureBox()
        {
            if (dataGridViewHatchData.Rows.Count > 0)
                pictureBox.Image = (Image)dataGridViewHatchData.SelectedRows[0].Cells[4].Value;
        }

        private void HatchManagerGUI_Load(object sender, EventArgs e)
        {
            updateChapters();
            updateDomains();
            updateGroups();
            updateHatchs();
        }

        private void chapter_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDomains();
            updateGroups();
            updateHatchs();
        }

        private void listBoxDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGroups();
            updateHatchs();
        }

        private void listBoxGroupe_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateHatchs();
        }

        private void dataGridViewHatchData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            updatePictureBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}