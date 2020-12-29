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
    public partial class DebtorsForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader da;
        DataTable sTable;
        SQLiteDataAdapter sAdapter;

        public DebtorsForm()
        {
            InitializeComponent();
            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            // запрет на изменение и добавление записей в datagridview1
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        // метод для перехода на форму успеваемости
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AcademicPerForm acadForm = new AcademicPerForm();
            acadForm.Show();
        }

        // выход из программы
        private void DebtorsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void DebtorsForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // запрос на вывод всех групп в combobox1
            cmd = new SQLiteCommand("SELECT (group_name) " +
                                    "FROM GroupsDb", con);
            da = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("group_name", typeof(string));
            dt.Load(da);
            comboBox1.ValueMember = "group_name";
            comboBox1.DataSource = dt;
            comboBox1.SelectedIndex = -1;

            // запрос на вывод года обучения в combobox2
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT (Num_year) " +
                                                   "FROM YearOfStudy", con);
            SQLiteDataReader myreader1 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Num_year", typeof(string));
            dt2.Load(myreader1);
            comboBox2.ValueMember = "Num_year";
            comboBox2.DataSource = dt2;
            comboBox2.SelectedIndex = -1;

            // запрос на вывод данных о студенте в dataGridView1
            string querus = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                            "FROM MarksAndOffsets " +
                            "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                            "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                            "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                            "WHERE MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет' " +
                            "ORDER BY StudDb.last_name ";
            sAdapter = new SQLiteDataAdapter(querus, con);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;

            dataGridView1.Columns[0].HeaderText = "Фамилия";
            dataGridView1.Columns[1].HeaderText = "Имя";
            dataGridView1.Columns[2].HeaderText = "Название группы";
            dataGridView1.Columns[3].HeaderText = "Год обучения";
            dataGridView1.Columns[4].HeaderText = "Название предмета";
            dataGridView1.Columns[5].HeaderText = "Оценка/Зачет";
            dataGridView1.Columns[6].HeaderText = "Дата проведения";

            // изменение стиля datagridview1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;

        }

        // метод для сброса выбранных данных из combobox'ов
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // переменные для хранения индексов из combobox'ов
            int getind = comboBox1.SelectedIndex + 1;
            int getind2 = comboBox2.SelectedIndex + 1;
            
            // условия для вывода соответствующей информации о студентах в dataGridView1
            // в зависимости от заполнения Combobox1 и Combobox2
            if ((getind2 == 0) && (getind != 0))
            {
                string query = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                    "FROM MarksAndOffsets " +
                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                    "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                    "WHERE (MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет') AND GroupsDb.group_id ='" + getind + "' " +
                    "ORDER BY StudDb.last_name" ;
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 != 0) && (getind != 0))
            {
                string query = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                    "FROM MarksAndOffsets " +
                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                    "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                    "WHERE (MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет') AND GroupsDb.group_id ='" + getind + "' AND StudDb.year_of_study ='" + getind2 + "' " +
                    "ORDER BY StudDb.last_name";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 == 0) && (getind == 0))
            {
                string query = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                    "FROM MarksAndOffsets " +
                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                    "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                    "WHERE (MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет') " +
                    "ORDER BY StudDb.last_name";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 != 0) && (getind == 0))
            {
                string query = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                    "FROM MarksAndOffsets " +
                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                    "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                    "WHERE (MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет') AND StudDb.year_of_study ='" + getind2 + "' " +
                    "ORDER BY StudDb.last_name";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
        }

        // метод аналогичный comboBox1_SelectedIndexChanged для отслеживания изменений в comboBox2
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int getind = comboBox1.SelectedIndex + 1;
            int getind2 = comboBox2.SelectedIndex + 1;
            if ((getind2 == 0) && (getind != 0))
            {
                string query = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                    "FROM MarksAndOffsets " +
                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                    "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                    "WHERE (MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет') AND GroupsDb.group_id ='" + getind + "' " +
                    "ORDER BY StudDb.last_name";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 != 0) && (getind != 0))
            {
                string query = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                    "FROM MarksAndOffsets " +
                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                    "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                    "WHERE (MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет') AND GroupsDb.group_id ='" + getind + "' AND StudDb.year_of_study ='" + getind2 + "' " +
                    "ORDER BY StudDb.last_name";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 == 0) && (getind == 0))
            {
                string query = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                    "FROM MarksAndOffsets " +
                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                    "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                    "WHERE (MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет') " +
                    "ORDER BY StudDb.last_name";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 != 0) && (getind == 0))
            {
                string query = "SELECT StudDb.last_name, StudDb.first_name, GroupsDb.group_name, StudDb.year_of_study,SubjectsDb.Subject_name , MarksAndOffsets.MarkOrOfset, MarksAndOffsets.Date " +
                    "FROM MarksAndOffsets " +
                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                    "JOIN subjectsDb ON MarksAndOffsets.SubjectName = SubjectsDb.Sub_id " +
                    "WHERE (MarksAndOffsets.MarkOrOfset = 2 OR MarksAndOffsets.MarkOrOfset = 'Незачет') AND StudDb.year_of_study ='" + getind2 + "' " +
                    "ORDER BY StudDb.last_name";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
        }

        // метод для перехода на форму списков для отчисления
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            KickForm kickForm = new KickForm();
            kickForm.Show();
        }
    }
}
