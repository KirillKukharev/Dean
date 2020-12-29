using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kukharev_Dean
{
    public partial class StudDiplomaInfoForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection Scon;
        DataTable sTable;
        SQLiteDataAdapter sAdapter;

        public StudDiplomaInfoForm()
        {
            InitializeComponent();

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            // запрет на изменение и добавление записей в datagridview1
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void StudDiplomaInfoForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            Scon = new SQLiteConnection("Data Source=Dean.sqlite");
            Scon.Open();
            string stud_num = label4.Text;

            // запрос на вывод предмета и оценки/зачета в datagridview 
            sAdapter = new SQLiteDataAdapter("SELECT SubjectsDb.Subject_name 'Название предмета', MarksAndOffsets.MarkOrOfset 'Оценка/Зачет'" +
                                             "FROM MarksAndOffsets " +
                                             "JOIN SubjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                                             "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                                             "WHERE StudDb.Stud_number ='"+ stud_num + "' ;", Scon);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;

            // изменение стиля DataGridView1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
        }

        // метод сохранения datagridview в виде картинки
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Изображение .bmp | *.bmp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int width, height;
                width = panel1.Width;
                height = panel1.Height;
                Bitmap bmp = new Bitmap(width, height);
                panel1.DrawToBitmap(bmp, panel1.ClientRectangle);
                bmp.Save(sfd.FileName);
            }
        }
    }
}
