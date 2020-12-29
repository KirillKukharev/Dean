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
    public partial class StudentPageForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteCommand cmd;
        DataTable sTable;
        SQLiteDataAdapter sAdapter;

        public StudentPageForm()
        {
            InitializeComponent();

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            // запрет на изменение и добавление записей в datagridview1
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;

        }

        // выход из программы
        private void StudentPageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // переход на главную форму 
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            InfoPageForm infopageForm = new InfoPageForm();
            infopageForm.Show();
        }

        // переход на форму поиска  студента
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            FindStudInfoForm findastudent = new FindStudInfoForm();
            findastudent.Show();
        }

        // переход на форму добавления студента
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddStudent studadd = new AddStudent();
            studadd.Show();
        }

        private void StudentPageForm_Load(object sender, EventArgs e)
        {
            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // запрос на вывод групп в combobox
            cmd = new SQLiteCommand("SELECT (group_name) " +
                                    "FROM GroupsDb ;", con);
            SQLiteDataReader myreader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("group_name", typeof(string));
            dt.Load(myreader);
            comboBox4.ValueMember = "group_name";
            comboBox4.DataSource = dt;
            comboBox4.SelectedIndex = -1;

            // запрос на вывод формы обучения
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT (form_name) " +
                                                   "FROM foeDB ;", con);
            SQLiteDataReader myreader1 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("form_name", typeof(string));
            dt2.Load(myreader1);
            comboBox3.ValueMember = "form_name";
            comboBox3.DataSource = dt2;
            comboBox3.SelectedIndex = -1;

            // переименование столбцов в datagridview1
            dataGridView1.Columns[0].HeaderText = "Номер студенческого";
            dataGridView1.Columns[1].HeaderText = "Название группы";
            dataGridView1.Columns[2].HeaderText = "Имя";
            dataGridView1.Columns[3].HeaderText = "Фамилия";
            dataGridView1.Columns[4].HeaderText = "Отчество";
            dataGridView1.Columns[5].HeaderText = "Адрес";
            dataGridView1.Columns[6].HeaderText = "Год рождения";
            dataGridView1.Columns[7].HeaderText = "Номер телефона";
            dataGridView1.Columns[8].HeaderText = "Телефон матери";
            dataGridView1.Columns[9].HeaderText = "Телефон отца";
            dataGridView1.Columns[10].HeaderText = "Тип поступления";
            dataGridView1.Columns[11].HeaderText = "Год обучения";
            MaximizeBox = false;

            // запрос на вывод информации о студенте
            string querus = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                            "FROM StudDb " +
                            "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                            "JOIN foeDB ON StudDb.admission_type = foeDB.form_id ";
            sAdapter = new SQLiteDataAdapter(querus, con);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;

            // изменение стиля datagridview1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
        }

        // метод для отслеживания изменения  комбобокса с группами и вывода соответствующей группе информации в datagridview1
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int getind = comboBox4.SelectedIndex + 1;
            int getind2 = comboBox3.SelectedIndex + 1;
            string getval = textBox1.Text;

            // условия для вывода информации
            if (getind2 == 0 && getval.Equals(""))
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Group_num = '" + getind.ToString()  + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if (getind2 == 0 && getval != "")
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Group_num = '" + getind.ToString() + "' AND year_of_study = '" + getval  + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if (getind2 !=0 && getval.Equals(""))
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Group_num = '" + getind.ToString()  + "' and admission_type = '" + getind2.ToString() + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if (getind2 != 0 && getval != "")
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Group_num = '" + getind.ToString() + "' AND year_of_study = '" + getval + "' and admission_type = '" + getind2.ToString() + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }

        }

        // метод для отслеживания изменения  года обучения студента и вывода соответствующей  информации в datagridview1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int getind = comboBox4.SelectedIndex + 1;
            int getind2 = comboBox3.SelectedIndex + 1;
            string getval = textBox1.Text;
            if (getind == 0 && getind2 == 0)
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE  year_of_study = '" + getval + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if (getind == 0 && getind2 != 0)
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id  " +
                               "WHERE year_of_study = '" + getval + "' and admission_type = '" + getind2.ToString() + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if( getind !=0 && getind2 == 0)
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Group_num = '" + getind.ToString() + "' AND year_of_study = '" + getval + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if (getind !=0 && getind2 != 0)
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Group_num = '" + getind.ToString() + "' AND year_of_study = '" + getval + "' and admission_type = '" + getind2.ToString() + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
        }

        // метод для отслеживания изменения  комбобокса с условием поступления и вывода соответствующей  информации в datagridview1
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int getind = comboBox4.SelectedIndex + 1;
            int getind2 = comboBox3.SelectedIndex + 1;
            string getval = textBox1.Text;
            if (getind == 0 && getval.Equals(""))
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE admission_type = '" + getind2.ToString() + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if(getind == 0 && getval != "")
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE year_of_study = '" + getval + "' and admission_type = '" + getind2.ToString() + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if (getind != -1 && getval.Equals(""))
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Group_num = '" + getind.ToString() +  "' and admission_type = '" + getind2.ToString() + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if(getind != -1 && getval != "")
            {
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Group_num = '" + getind.ToString() + "' AND year_of_study = '" + getval + "' and admission_type = '" + getind2.ToString() + "';";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            
        }
    }
}
