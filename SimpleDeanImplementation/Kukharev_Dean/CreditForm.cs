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
    public partial class CreditForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteDataAdapter sAdapter;
        DataTable sTable;

        public CreditForm()
        {
            InitializeComponent();
            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            // запрет на изменение и добавление записей в datagridview1
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        // выход из программы
        private void ExamOrCreditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void CreditForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // вывод в textbox3 курса
            textBox3.Text = ParamOfExamAndCreditForm.yer;

            // вывод в textbox4 группы
            textBox4.Text = ParamOfExamAndCreditForm.gro;
            // вывод в textbox2 предмета
            textBox2.Text = ParamOfExamAndCreditForm.discipl;
            // вывод в textbox6 учителя
            textBox6.Text = ParamOfExamAndCreditForm.teacher;

            // запрос на вывод студентов из группы и курса, выбранного на форме ParamOfExamAndCreditForm
            sAdapter = new SQLiteDataAdapter("SELECT StudDb.Stud_number as 'Номер студенческого',StudDb.last_name as 'Фамилия студента',  StudDb.first_name as 'Имя студента',StudDb.middle_name as 'Отчество студента', \"\" as 'Зачтено / Незачтено',\"\" as 'Подпись предпод.' " +
                                             "FROM StudDb " +
                                             "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                                             "WHERE group_name = '" + textBox4.Text + "' and year_of_study = '" + textBox3.Text + "' ;", con);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;

            // редактирование размеров столбцов в datagridview1
            dataGridView1.Columns[1].Width = 130;
            dataGridView1.Columns[2].Width = 105;
            dataGridView1.Columns[3].Width = 127;
            dataGridView1.Columns[4].Width = 140;
            dataGridView1.Columns[5].Width = 80;

            // изменение стиля datagridview1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
        }

        // переход на главную форму
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ParamOfExamAndCreditForm parametersForm = new ParamOfExamAndCreditForm();
            parametersForm.Show();
        }

        // вывод информации о студентах сдающих зачет в зависимости от текста в поле "Курс"
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            sAdapter = new SQLiteDataAdapter("SELECT StudDb.Stud_number as 'Номер студенческого',StudDb.last_name as 'Фамилия студента',  StudDb.first_name as 'Имя студента',StudDb.middle_name as 'Отчество студента', \"\" as 'Зачтено / Незачтено',\"\" as 'Подпись предпод.' " +
                                             "FROM StudDb " +
                                             "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                             "WHERE group_name = '" + textBox4.Text + "' and year_of_study = '" + textBox3.Text + "' ;", con);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;
        }

        // вывод информации о студентах сдающих зачет в зависимости от текста в поле "Группа"
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            sAdapter = new SQLiteDataAdapter("SELECT StudDb.Stud_number as 'Номер студенческого',StudDb.last_name as 'Фамилия студента',  StudDb.first_name as 'Имя студента',StudDb.middle_name as 'Отчество студента', \"\" as 'Зачтено / Незачтено',\"\" as 'Подпись предпод.' " +
                                             "FROM StudDb " +
                                             "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                             "WHERE group_name = '" + textBox4.Text + "' and year_of_study = '" + textBox3.Text + "' ;", con);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;
        }

        // метод сохранения datagridView1
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
