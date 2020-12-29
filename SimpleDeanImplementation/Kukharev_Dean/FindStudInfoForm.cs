using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kukharev_Dean
{
    public partial class FindStudInfoForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection Scon;
        DataTable sTable;
        SQLiteDataAdapter sAdapter;

        public FindStudInfoForm()
        {
            InitializeComponent();

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            // запрет на изменение и добавление записей в datagridview1
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        // объявление списка для вывода примера вводимой информации
        List<string> personinfo = new List<string>(); 
        private void FindStudInfoForm_Load(object sender, EventArgs e)
        {
            printButton.Enabled = false;
            // заполнение combobox параметрами для поиска студента
            comboBox1.Items.AddRange(new string[] { "Номер студенческого","Группа", "Имя", "Фамилия",  "Отчество", "Адрес", "Год рождения",  "Номер телефона", "Телефон матери", "Телефон отца","Тип поступления","Год обучения"});
            personinfo.AddRange(new string[] { "1284574", "ПИЭ", "Кирилл", "Кухарев",  "Александрович", "Машиностроителей 22", "2001",  "89546321312", "89574875986", "89654875968","Бюджет", "3" });
            
            // инициализация подключения к бд
            Scon = new SQLiteConnection("Data Source=Dean.sqlite");
            Scon.Open();
            
            // запрос на вывод всей информации из таблицы StudDb
            sAdapter = new SQLiteDataAdapter("SELECT * " +
                                             "FROM StudDb ;", Scon);
            sTable = new DataTable();
            sAdapter.Fill(sTable);

            new SQLiteCommandBuilder(sAdapter);

            // запрет на ввод информации для поиска студента
            findpesonbox.Enabled = false;

            dataGridView1.DataSource = sTable;
            dataGridView1.Columns[0].Visible = false;

            // переименование столбцов для вывода информации в DataGridView1(можно было в sql запросе
            //  указать, однако пришлось бы перечислять все столбцы)
            dataGridView1.Columns[1].HeaderText = "Номер студенческого";
            dataGridView1.Columns[2].HeaderText = "Название группы";
            dataGridView1.Columns[3].HeaderText = "Имя";
            dataGridView1.Columns[4].HeaderText = "Фамилия";
            dataGridView1.Columns[5].HeaderText = "Отчество";
            dataGridView1.Columns[6].HeaderText = "Адрес";
            dataGridView1.Columns[7].HeaderText = "Год рождения";
            dataGridView1.Columns[8].HeaderText = "Номер телефона";
            dataGridView1.Columns[9].HeaderText = "Телефон матери";
            dataGridView1.Columns[10].HeaderText = "Телефон отца";
            dataGridView1.Columns[11].HeaderText = "Тип поступления";
            dataGridView1.Columns[12].HeaderText = "Год обучения";
            dataGridView1.Columns[2].Width = 70;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[7].Width = 60;
            dataGridView1.Columns[12].Width = 70;
            dataGridView1.Columns[6].Width = 140;
            comboBox1.SelectedIndex = 0;

            // запрет изменения размеров формы
            MaximizeBox = false;

            // изменение стиля datagridview1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;

        }

        // метод для вывода примера ввода информации при выборе соответствующего значения в combobox1
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            findpesonbox.Text = personinfo[comboBox1.SelectedIndex];
            findpesonbox.Enabled = true;
            printButton.Enabled = true;
        }

        // метод Enter и метод Leave для эмуляции placeholder
        private void findpesonbox_Enter(object sender, EventArgs e)
        {
            if (findpesonbox.Text == personinfo[comboBox1.SelectedIndex])
            {
                findpesonbox.Text = "";
                findpesonbox.ForeColor = Color.Black;
            }
        }

        private void findpesonbox_Leave(object sender, EventArgs e)
        {
            if (findpesonbox.Text == "")
            {
                findpesonbox.Text = personinfo[comboBox1.SelectedIndex];

                findpesonbox.ForeColor = Color.Silver;
            }
        }

        // метод вывода информации в DataGridView при изменении текста в textbox'e
        private void findpesonbox_TextChanged(object sender, EventArgs e)
        {
            SearchData(findpesonbox.Text.Trim());
            
        }

        // метод поиска информации о студенте 
        public void SearchData(string search)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                // запрос на вывод информации о студенте со значением студенческого search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE Stud_number LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                switch (search)
                {
                    case "ПИЭ":
                        // запрос на вывод информации о студенте со значением группы ПИЭ
                        string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                                       "FROM StudDb " +
                                       "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                       "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                                       "WHERE  Group_num = 3 ;";
                        sAdapter = new SQLiteDataAdapter(query, Scon);
                        sTable = new DataTable();
                        sAdapter.Fill(sTable);
                        dataGridView1.DataSource = sTable;
                        break;
                    case "ИВТ":
                        // запрос на вывод информации о студенте со значением группы ИВТ
                        string quer = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                                      "FROM StudDb " +
                                      "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                      "JOIN foeDB ON StudDb.admission_type = foeDB.form_id  " +
                                      "WHERE  Group_num = 1 ;";
                        sAdapter = new SQLiteDataAdapter(quer, Scon);
                        sTable = new DataTable();
                        sAdapter.Fill(sTable);
                        dataGridView1.DataSource = sTable;
                        break;
                    case "ИТ":
                        // запрос на вывод информации о студенте со значением группы ИВТ
                        string queri = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                                       "FROM StudDb " +
                                       "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                       "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                                       "WHERE  Group_num = 2 ;";
                        sAdapter = new SQLiteDataAdapter(queri, Scon);
                        sTable = new DataTable();
                        sAdapter.Fill(sTable);
                        dataGridView1.DataSource = sTable;
                        break;

                }
                   
            }
            if (comboBox1.SelectedIndex == 2)
            {
                // запрос на вывод информации о студенте со значением имени search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE first_name LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 3)
            {
                // запрос на вывод информации о студенте со значением фамилии search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE last_name LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 4)
            {
                // запрос на вывод информации о студенте со значением отчества search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE middle_name LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 5)
            {
                // запрос на вывод информации о студенте со значением адреса search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE address LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 6)
            {
                // запрос на вывод информации о студенте со значением дня рождения search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE birthday LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 7)
            {
                // запрос на вывод информации о студенте со значением номера телефона search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE phone_number LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 8)
            {
                // запрос на вывод информации о студенте со значением телефона мамы search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE mother_phone LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 9)
            {
                // запрос на вывод информации о студенте со значением телефона папы search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE father_phone LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            if (comboBox1.SelectedIndex == 10)
            {
                switch (search)
                {
                    case ("Бюджет"):
                        // запрос на вывод информации о студенте со значением формы обучения бюджет
                        string query1 = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                                        "FROM StudDb " +
                                        "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                        "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                                        "WHERE form_id = 1 ;";
                        sAdapter = new SQLiteDataAdapter(query1, Scon);
                        sTable = new DataTable();
                        sAdapter.Fill(sTable);
                        dataGridView1.DataSource = sTable;
                        break;

                    case ("Внебюджет"):
                        // запрос на вывод информации о студенте со значением формы обучения внебюджет
                        string query2 = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                                        "FROM StudDb " +
                                        "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                        "JOIN foeDB ON StudDb.admission_type = foeDB.form_id WHERE form_id = 2 ;";
                        sAdapter = new SQLiteDataAdapter(query2, Scon);
                        sTable = new DataTable();
                        sAdapter.Fill(sTable);
                        dataGridView1.DataSource = sTable;
                        break;

                }
            }
            if (comboBox1.SelectedIndex == 11)
            {
                // запрос на вывод информации о студенте со значением года обучения search
                string query = "SELECT StudDb.Stud_number, GroupsDb.group_name as 'Название группы',StudDb.first_name, StudDb.last_name, StudDb.middle_name, StudDb.address, StudDb.birthday, StudDb.phone_number, StudDb.mother_phone, StudDb.father_phone, foeDB.form_name as 'Тип поступления', StudDb.year_of_study " +
                               "FROM StudDb " +
                               "INNER JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                               "JOIN foeDB ON StudDb.admission_type = foeDB.form_id " +
                               "WHERE year_of_study LIKE '%" + search + "%' ;";
                sAdapter = new SQLiteDataAdapter(query, Scon);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }

        }

        // метод сохранения списка студентов из dataGridView1
        private void printButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Изображение .bmp | *.bmp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int width, height;
                width = panelstudent.Width;
                height = panelstudent.Height;
                Bitmap bmp = new Bitmap(width, height);
                panelstudent.DrawToBitmap(bmp, panelstudent.ClientRectangle);
                bmp.Save(sfd.FileName);
            }
          
        }

        // метод перехода на форму информации о студентах
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentPageForm studpage = new StudentPageForm();
            studpage.Show();
        }

        // выход из программы
        private void FindStudInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // метод перехода на форму изменения информации о студенте
        private void changestudent_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddStudInfoForm changeinfo = new AddStudInfoForm();
            changeinfo.Show();
            changestudent.Enabled = false;
        }
    }
}




