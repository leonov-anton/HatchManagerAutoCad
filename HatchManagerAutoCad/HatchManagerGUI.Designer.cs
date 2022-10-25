namespace HatchManagerAutoCad
{
    partial class HatchManagerGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.comboBoxChapter = new System.Windows.Forms.ComboBox();
            this.labelChapter = new System.Windows.Forms.Label();
            this.listBoxDomain = new System.Windows.Forms.ListBox();
            this.labelDomain = new System.Windows.Forms.Label();
            this.listBoxGroupe = new System.Windows.Forms.ListBox();
            this.labelGroupe = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelPic = new System.Windows.Forms.Label();
            this.dataGridViewHatchData = new System.Windows.Forms.DataGridView();
            this.HatchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PatName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Layer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Img = new System.Windows.Forms.DataGridViewImageColumn();
            this.buttonChange = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHatchData)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxChapter
            // 
            this.comboBoxChapter.FormattingEnabled = true;
            this.comboBoxChapter.Location = new System.Drawing.Point(20, 20);
            this.comboBoxChapter.Name = "comboBoxChapter";
            this.comboBoxChapter.Size = new System.Drawing.Size(200, 21);
            this.comboBoxChapter.TabIndex = 0;
            this.comboBoxChapter.SelectedIndexChanged += new System.EventHandler(this.chapter_comboBox_SelectedIndexChanged);
            // 
            // labelChapter
            // 
            this.labelChapter.AutoSize = true;
            this.labelChapter.Location = new System.Drawing.Point(17, 4);
            this.labelChapter.Name = "labelChapter";
            this.labelChapter.Size = new System.Drawing.Size(47, 13);
            this.labelChapter.TabIndex = 1;
            this.labelChapter.Text = "Раздел:";
            // 
            // listBoxDomain
            // 
            this.listBoxDomain.FormattingEnabled = true;
            this.listBoxDomain.Location = new System.Drawing.Point(20, 60);
            this.listBoxDomain.Name = "listBoxDomain";
            this.listBoxDomain.Size = new System.Drawing.Size(200, 95);
            this.listBoxDomain.TabIndex = 2;
            this.listBoxDomain.SelectedIndexChanged += new System.EventHandler(this.listBoxDomain_SelectedIndexChanged);
            // 
            // labelDomain
            // 
            this.labelDomain.AutoSize = true;
            this.labelDomain.Location = new System.Drawing.Point(17, 44);
            this.labelDomain.Name = "labelDomain";
            this.labelDomain.Size = new System.Drawing.Size(53, 13);
            this.labelDomain.TabIndex = 3;
            this.labelDomain.Text = "Область:";
            // 
            // listBoxGroupe
            // 
            this.listBoxGroupe.FormattingEnabled = true;
            this.listBoxGroupe.Location = new System.Drawing.Point(20, 175);
            this.listBoxGroupe.Name = "listBoxGroupe";
            this.listBoxGroupe.Size = new System.Drawing.Size(200, 95);
            this.listBoxGroupe.TabIndex = 4;
            this.listBoxGroupe.SelectedIndexChanged += new System.EventHandler(this.listBoxGroupe_SelectedIndexChanged);
            // 
            // labelGroupe
            // 
            this.labelGroupe.AutoSize = true;
            this.labelGroupe.Location = new System.Drawing.Point(17, 158);
            this.labelGroupe.Name = "labelGroupe";
            this.labelGroupe.Size = new System.Drawing.Size(45, 13);
            this.labelGroupe.TabIndex = 5;
            this.labelGroupe.Text = "Группа:";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(20, 295);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(200, 200);
            this.pictureBox.TabIndex = 6;
            this.pictureBox.TabStop = false;
            // 
            // labelPic
            // 
            this.labelPic.AutoSize = true;
            this.labelPic.Location = new System.Drawing.Point(17, 279);
            this.labelPic.Name = "labelPic";
            this.labelPic.Size = new System.Drawing.Size(121, 13);
            this.labelPic.TabIndex = 7;
            this.labelPic.Text = "Пример изображения:";
            // 
            // dataGridViewHatchData
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewHatchData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewHatchData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHatchData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HatchName,
            this.Description,
            this.PatName,
            this.Layer,
            this.Img});
            this.dataGridViewHatchData.Location = new System.Drawing.Point(238, 20);
            this.dataGridViewHatchData.Name = "dataGridViewHatchData";
            this.dataGridViewHatchData.Size = new System.Drawing.Size(524, 474);
            this.dataGridViewHatchData.TabIndex = 8;
            this.dataGridViewHatchData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewHatchData_CellContentClick);
            // 
            // HatchName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.HatchName.DefaultCellStyle = dataGridViewCellStyle2;
            this.HatchName.HeaderText = "Имя";
            this.HatchName.Name = "HatchName";
            this.HatchName.ReadOnly = true;
            // 
            // Description
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Description.DefaultCellStyle = dataGridViewCellStyle3;
            this.Description.HeaderText = "Описание";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 150;
            // 
            // PatName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PatName.DefaultCellStyle = dataGridViewCellStyle4;
            this.PatName.HeaderText = "Имя образца";
            this.PatName.Name = "PatName";
            this.PatName.ReadOnly = true;
            this.PatName.Width = 50;
            // 
            // Layer
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Layer.DefaultCellStyle = dataGridViewCellStyle5;
            this.Layer.HeaderText = "Слой";
            this.Layer.Name = "Layer";
            this.Layer.ReadOnly = true;
            // 
            // Img
            // 
            this.Img.HeaderText = "Вид";
            this.Img.Name = "Img";
            this.Img.ReadOnly = true;
            this.Img.Width = 80;
            // 
            // buttonChange
            // 
            this.buttonChange.Location = new System.Drawing.Point(692, 510);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(70, 30);
            this.buttonChange.TabIndex = 9;
            this.buttonChange.Text = "Изменить";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(607, 510);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(70, 30);
            this.buttonNew.TabIndex = 10;
            this.buttonNew.Text = "Новая";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // HatchManagerGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 553);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.dataGridViewHatchData);
            this.Controls.Add(this.labelPic);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.labelGroupe);
            this.Controls.Add(this.listBoxGroupe);
            this.Controls.Add(this.labelDomain);
            this.Controls.Add(this.listBoxDomain);
            this.Controls.Add(this.labelChapter);
            this.Controls.Add(this.comboBoxChapter);
            this.Name = "HatchManagerGUI";
            this.Text = "Менеджер Штриховок";
            this.Load += new System.EventHandler(this.HatchManagerGUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHatchData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.ComboBox comboBoxChapter;
        private System.Windows.Forms.Label labelChapter;
        private System.Windows.Forms.ListBox listBoxDomain;
        private System.Windows.Forms.Label labelDomain;
        private System.Windows.Forms.ListBox listBoxGroupe;
        private System.Windows.Forms.Label labelGroupe;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelPic;
        private System.Windows.Forms.DataGridView dataGridViewHatchData;
        private System.Windows.Forms.DataGridViewTextBoxColumn HatchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Layer;
        private System.Windows.Forms.DataGridViewImageColumn Img;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.Button buttonNew;
    }
}