namespace Kukharev_Dean
{
    partial class FindStudInfoForm
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
            this.findpesonbox = new System.Windows.Forms.TextBox();
            this.stundfindinfo = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.printButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelstudent = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.changestudent = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelstudent.SuspendLayout();
            this.SuspendLayout();
            // 
            // findpesonbox
            // 
            this.findpesonbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.findpesonbox.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.findpesonbox.Location = new System.Drawing.Point(481, 55);
            this.findpesonbox.Name = "findpesonbox";
            this.findpesonbox.Size = new System.Drawing.Size(200, 24);
            this.findpesonbox.TabIndex = 1;
            this.findpesonbox.TextChanged += new System.EventHandler(this.findpesonbox_TextChanged);
            this.findpesonbox.Enter += new System.EventHandler(this.findpesonbox_Enter);
            this.findpesonbox.Leave += new System.EventHandler(this.findpesonbox_Leave);
            // 
            // stundfindinfo
            // 
            this.stundfindinfo.AutoSize = true;
            this.stundfindinfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.stundfindinfo.Location = new System.Drawing.Point(200, 11);
            this.stundfindinfo.Name = "stundfindinfo";
            this.stundfindinfo.Size = new System.Drawing.Size(204, 18);
            this.stundfindinfo.TabIndex = 3;
            this.stundfindinfo.Text = "Выберите параметр поиска:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(481, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(200, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // printButton
            // 
            this.printButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.printButton.Location = new System.Drawing.Point(858, 8);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(208, 52);
            this.printButton.TabIndex = 4;
            this.printButton.Text = "Распечатать форму";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 18);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1217, 395);
            this.dataGridView1.TabIndex = 2;
            // 
            // panelstudent
            // 
            this.panelstudent.Controls.Add(this.dataGridView1);
            this.panelstudent.Location = new System.Drawing.Point(0, 123);
            this.panelstudent.Name = "panelstudent";
            this.panelstudent.Size = new System.Drawing.Size(1220, 423);
            this.panelstudent.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(200, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Введите значение:";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(26, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 33);
            this.button2.TabIndex = 29;
            this.button2.Text = "< Назад";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // changestudent
            // 
            this.changestudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.changestudent.Location = new System.Drawing.Point(858, 66);
            this.changestudent.Name = "changestudent";
            this.changestudent.Size = new System.Drawing.Size(208, 51);
            this.changestudent.TabIndex = 30;
            this.changestudent.Text = "Изменить запись о студенте";
            this.changestudent.UseVisualStyleBackColor = true;
            this.changestudent.Click += new System.EventHandler(this.changestudent_Click);
            // 
            // FindStudInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1227, 548);
            this.Controls.Add(this.changestudent);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.stundfindinfo);
            this.Controls.Add(this.findpesonbox);
            this.Controls.Add(this.panelstudent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FindStudInfoForm";
            this.Text = "FindStudInfoForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FindStudInfoForm_FormClosed);
            this.Load += new System.EventHandler(this.FindStudInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelstudent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox findpesonbox;
        private System.Windows.Forms.Label stundfindinfo;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelstudent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button changestudent;
    }
}