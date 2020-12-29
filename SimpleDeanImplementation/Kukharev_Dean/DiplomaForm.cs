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
    public partial class DiplomaForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection Scon;
        DataTable sTable;
        SQLiteDataAdapter sAdapter;

        public DiplomaForm()
        {
            InitializeComponent();
            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            // запрет на  добавление записей в datagridview1
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        // метод для перехода на главную форму
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            InfoPageForm infform = new InfoPageForm();
            infform.Show();
        }

        private void DiplomaForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;
            
            // инициализация подключения к базе данных
            Scon = new SQLiteConnection("Data Source=Dean.sqlite");
            Scon.Open();

            // запрос на вывод информации о студентах в datagridView1
            sAdapter = new SQLiteDataAdapter("SELECT DISTINCT StudDb.Stud_number  'Номер студенческого', StudDb.last_name 'Фамилия', StudDb.first_name 'Имя', GroupsDb.group_name  'Название группы'" +
                                             "FROM MarksAndOffsets " +
                                             "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                                             "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                             "WHERE StudDb.year_of_study = 4;", Scon);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;

            // изменение стиля datagridview1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
        }

        // выход из программы
        private void DiplomaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // метод для поиска студентов по фамилии
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string surname = textBox1.Text;
            // запрос студентов по введенной информации о их фамилии
            string query = "SELECT DISTINCT StudDb.Stud_number 'Номер студенческого', StudDb.last_name 'Фамилия', StudDb.first_name 'Имя', GroupsDb.group_name 'Название группы'" +
                           "FROM MarksAndOffsets " +
                           "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                           "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                           "WHERE StudDb.last_name LIKE '%"+ surname + "%' AND StudDb.year_of_study = 4;";
            sAdapter = new SQLiteDataAdapter(query, Scon);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;
        }

        // метод для перехода на форму вкладыша для диплома
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            StudDiplomaInfoForm studdipl = new StudDiplomaInfoForm();
            studdipl.label2.Text = this.dataGridView1.CurrentRow.Cells[3].Value.ToString();
            studdipl.label3.Text = this.dataGridView1.CurrentRow.Cells[2].Value.ToString();
            studdipl.label4.Text = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            studdipl.label6.Text = this.dataGridView1.CurrentRow.Cells[1].Value.ToString();
            studdipl.ShowDialog();
        }

        // метод для вывода студентов, "идущих" на красный диплом
        private void button1_Click(object sender, EventArgs e)
        {
            string diploms = "SELECT StudDb.Stud_number 'Номер студенческого', StudDb.last_name 'Фамилия', StudDb.first_name 'Имя', GroupsDb.group_name 'Название группы' " +
                             "FROM MarksAndOffsets " +
                             "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                             "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                             "JOIN( " +
                             "SELECT USER_ID as Result FROM MarksAndOffsets " +
                             "JOIN( " +
                             "SELECT creditos " +
                             "FROM( " +
                             "SELECT DISTINCT USER_ID creditos, COUNT(MarksAndOffsets.MarkOrOfset) as NUMs2 " +
                             "FROM  MarksAndOffsets " +
                             "GROUP BY creditos) t1 " +
                             "JOIN( " +
                             "SELECT USER_ID, COUNT(MarksAndOffsets.MarkOrOfset) as NUMs1 " +
                             "FROM  MarksAndOffsets " +
                             "WHERE(MarksAndOffsets.MarkOrOfset = 'Зачет')  OR(MarksAndOffsets.MarkOrOfset != 'Незачет') " +
                             "GROUP BY USER_ID) t2 " +
                             "ON t1.creditos = t2.USER_ID " +
                             "WHERE NUMs2 = NUMs1) b1 " +
                             "ON MarksAndOffsets.USER_ID = b1.creditos " +
                             "JOIN( " +
                             "SELECT mork " +
                             "FROM( " +
                             "SELECT USER_ID mork, COUNT(MarksAndOffsets.MarkOrOfset) as Marksall " +
                             "FROM  MarksAndOffsets " +
                             "WHERE(MarksAndOffsets.MarkOrOfset NOT LIKE '%ачет%') " +
                             "GROUP BY mork) p1 " +
                             "JOIN( " +
                             "SELECT USER_ID, COUNT(MarksAndOffsets.MarkOrOfset) as Marksand " +
                             "FROM  MarksAndOffsets " +
                             "WHERE(MarksAndOffsets.MarkOrOfset NOT LIKE '%ачет%') AND(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset = '4') " +
                             "GROUP BY USER_ID) p2 " +
                             "ON p1.mork = p2.USER_ID " +
                             "WHERE Marksall = Marksand) b2 " +
                             "ON MarksAndOffsets.USER_ID = b2.mork " +
                             "JOIN( " +
                             "SELECT fivees " +
                             "FROM( " +
                             "SELECT USER_ID fivees, COUNT(MarksAndOffsets.MarkOrOfset) as AllMarks " +
                             "FROM  MarksAndOffsets WHERE(MarksAndOffsets.MarkOrOfset NOT LIKE '%ачет%') " +
                             "GROUP BY fivees) v1 " +
                             "JOIN( " +
                             "SELECT USER_ID, COUNT(MarksAndOffsets.MarkOrOfset) as Fives " +
                             "FROM  MarksAndOffsets " +
                             "WHERE(MarksAndOffsets.MarkOrOfset NOT LIKE '%ачет%') AND(MarksAndOffsets.MarkOrOfset = '5') " +
                             "GROUP BY USER_ID) v2 " +
                             "ON v1.fivees = v2.USER_ID " +
                             "WHERE CAST(Fives as REAL) / CAST(AllMarks as REAL) * 100 >= 75) b3 " +
                             "ON MarksAndOffsets.USER_ID = b3.fivees " +
                             "GROUP BY Result " +
                             ") " +
                             "WHERE StudDb.Id = Result  AND StudDb.year_of_study = 4 " +
                             "GROUP BY StudDb.Stud_number;";
            sAdapter = new SQLiteDataAdapter(diploms, Scon);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;
        }

        // метод для сброса фильтров
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            sAdapter = new SQLiteDataAdapter("SELECT DISTINCT StudDb.Stud_number 'Номер студенческого', StudDb.last_name 'Фамилия', StudDb.first_name 'Имя', GroupsDb.group_name 'Название группы'" +
                                             "FROM MarksAndOffsets " +
                                             "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                                             "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                             "WHERE StudDb.year_of_study = 4;", Scon);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;
        }
    }
}
