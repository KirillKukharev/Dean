using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kukharev_Dean
{
    public partial class AddStudInfoForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader da;
        public AddStudInfoForm()
        {
            InitializeComponent();
            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void AddStudInfoForm_Load(object sender, EventArgs e)
        {
            // запрет на редактирование полей о студенте
            button1.Enabled = false;
            textBox2.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            textBox11.Enabled = false;
            textBox13.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;

            // запрет изменения размеров формы
            MaximizeBox = false;
        }
        private void findbutton_Click(object sender, EventArgs e)
        {
            // переменная для сравнения номера студенческого с типом int
            int g;

            // проверка введенного студенческого на валидность
            if ((textBox1.Text.Length != 7) || ((int.TryParse(textBox1.Text.ToString(), out g)) != true))
            {
                MessageBox.Show("Длина студенческого билета должна быть равна 7, либо введено неправильное значение.");
                button1.Enabled = false;
                textBox2.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox13.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                MaximizeBox = false;
            }
            else
            {
                // инициализация подключения
                con = new SQLiteConnection("Data Source=Dean.sqlite");
                con.Open();

                // запрос на выбор данных студента с введенным с клавиатуры студенческим
                cmd = new SQLiteCommand("SELECT Stud_number, Group_num, first_name, last_name, middle_name, address, birthday, phone_number, mother_phone, father_phone, admission_type, year_of_study " +
                                        "FROM StudDb " +
                                        "WHERE Stud_number =@Studnumber ;", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Studnumber", int.Parse(textBox1.Text));
                da = cmd.ExecuteReader();

                // проверка на наличие студента в базе данных
                if (da.HasRows)
                {
                    // разрешение на редактирование полей
                    button1.Enabled = true;
                    textBox2.Enabled = true;
                    textBox4.Enabled = true;
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    textBox7.Enabled = true;
                    textBox8.Enabled = true;
                    textBox9.Enabled = true;
                    textBox10.Enabled = true;
                    textBox11.Enabled = true;
                    textBox13.Enabled = true;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;

                    // запрос на выбор всех групп для вывода в combobox1
                    SQLiteCommand cmd1 = new SQLiteCommand("SELECT (group_name) " +
                                                           "FROM GroupsDb ;", con);
                    SQLiteDataReader myreader = cmd1.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("group_name", typeof(string));
                    dt.Load(myreader);
                    comboBox1.ValueMember = "group_name";
                    comboBox1.DataSource = dt;

                    // запрос на вывод форм обучения в combobox2
                    SQLiteCommand cmd2 = new SQLiteCommand("SELECT (form_name) " +
                                                           "FROM foeDB ;", con);
                    SQLiteDataReader myreader1 = cmd2.ExecuteReader();
                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("form_name", typeof(string));
                    dt2.Load(myreader1);
                    comboBox2.ValueMember = "form_name";
                    comboBox2.DataSource = dt2;

                    while (da.Read())
                    {
                        // заполнение полей на форме данными о студенте, взятыми из бд
                        textBox2.Text = da.GetValue(0).ToString();
                        textBox4.Text = da.GetValue(2).ToString();
                        textBox5.Text = da.GetValue(3).ToString();
                        textBox6.Text = da.GetValue(4).ToString();
                        textBox7.Text = da.GetValue(5).ToString();
                        textBox8.Text = da.GetValue(6).ToString();
                        textBox9.Text = da.GetValue(7).ToString();
                        textBox10.Text = da.GetValue(8).ToString();
                        textBox11.Text = da.GetValue(9).ToString();
                        textBox13.Text = da.GetValue(11).ToString();
                        if (da.GetValue(1).ToString() == "1")
                        {
                            comboBox1.SelectedValue = "ИВТ";
                        }
                        if (da.GetValue(1).ToString() == "2")
                        {
                            comboBox1.SelectedValue = "ИТ";
                        }
                        if (da.GetValue(1).ToString() == "3")
                        {
                            comboBox1.SelectedValue = "ПИЭ";
                        }

                        if (da.GetValue(10).ToString() == "1")
                        {
                            comboBox2.SelectedValue = "Бюджет";
                        }
                        if (da.GetValue(10).ToString() == "2")
                        {
                            comboBox2.SelectedValue = "Внебюджет";
                        }
                    }
                    da.Close();
                }
                else
                {
                    MessageBox.Show("Студента с таким студенческим билетом не существует");
                }
            }
        }

        // метод для обновления данных о студенте
        private void button1_Click(object sender, EventArgs e)
        {
                // переменные для вставки данных о студенте в базу данных
                string checkstudnumb = textBox1.Text;
                string studnum = textBox2.Text;
                string firstname = textBox4.Text;
                string lastname = textBox5.Text;
                string middlename = textBox6.Text;
                string addressss = textBox7.Text;
                string birthday = textBox8.Text;
                string phone = textBox9.Text;
                string momphone = textBox10.Text;
                string faphone = textBox11.Text;
                string yearstud = textBox13.Text;

                // проверки, что поля о студенте не пусты
                if (studnum.Equals(""))
                {
                    MessageBox.Show("Введите номер студенческого");
                }
                else if (firstname.Equals(""))
                {
                    MessageBox.Show("Введите имя учащегося");
                }
                else if (lastname.Equals(""))
                {
                    MessageBox.Show("Введите фамилию учащегося");
                }
                else if (birthday.Equals(""))
                {
                    MessageBox.Show("Введите год рождения учащегося");
                }
                else if (phone.Equals(""))
                {
                    MessageBox.Show("Введите номер телефона учащегося");
                }
                else if (yearstud.Equals(""))
                {
                    MessageBox.Show("Введите номер курса учащегося");
                }
                else
                {
                    // переменные для хранения индекса группы и типа поступления
                    int ge = comboBox1.SelectedIndex + 1;
                    int ge2 = comboBox2.SelectedIndex + 1;
                    string bg = ge.ToString();
                    string bg2 = ge2.ToString();

                    // запрос для обновления данных о студенте
                    string query = "UPDATE StudDb SET Stud_number = '" + studnum + "', Group_num = '" + bg + "', first_name = '" + firstname + "', last_name = '" + lastname + "', middle_name = '" + middlename + "', address = '" + addressss + "', birthday = '" + birthday + "', phone_number = '" + phone + "', mother_phone='" + momphone + "', father_phone = '" + faphone + "', admission_type = '" + bg2 + "', year_of_study = '" + yearstud + "' WHERE Stud_number = '" + checkstudnumb + "' ;";
                    cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно сохранены");
                }
        }

        // выход из программы
        private void AddStudInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // переход на форму информации о студентах
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FindStudInfoForm studpage = new FindStudInfoForm();
            studpage.Show();
        }
    }
}
