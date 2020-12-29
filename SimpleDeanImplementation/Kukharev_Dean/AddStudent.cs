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
    public partial class AddStudent : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader da;

        public AddStudent()
        {
            InitializeComponent();
            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // переход на форму информации о студентах
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentPageForm studform = new StudentPageForm();
            studform.Show();
        }

        private void AddStudent_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // запрос на вывод названий групп
            cmd = new SQLiteCommand("SELECT (group_name) " +
                                    "FROM GroupsDb ;", con);

            // считывание данных
            da = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("group_name", typeof(string));
            dt.Load(da);
            comboBox1.ValueMember = "group_name";
            // заполнение combobox1
            comboBox1.DataSource = dt;

            // запрос на вывод формы обучения
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT (form_name) " +
                                                   "FROM foeDB ;", con);

            // считывание данных
            SQLiteDataReader myreader1 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("form_name", typeof(string));
            dt2.Load(myreader1);
            comboBox2.ValueMember = "form_name";
            // заполнение combobox2
            comboBox2.DataSource = dt2;

        }

        // выход из программы
        private void AddStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // метод для кнопки добавления студента
        private void button1_Click(object sender, EventArgs e)
        {
            // переменная для сравнения с типом int
            int g;

            // переменная для хранения значения textbox1
            string unic_studnum = textBox1.Text;

            // переменная для хранения значения textbox2
            string firstname = textBox2.Text;

            // переменная для хранения значения textbox3
            string lstname = textBox3.Text;

            // переменная для хранения значения textbox4
            string middlename = textBox4.Text;

            // переменная для хранения значения textbox5
            string address = textBox5.Text;

            // переменная для хранения значения textbox6
            string birthday = textBox6.Text;

            // переменная для хранения значения textbox7
            string phone = textBox7.Text;

            // переменная для хранения значения textbox8
            string motherphone = textBox8.Text;

            // переменная для хранения значения textbox9
            string fatherphone = textBox9.Text;

            // переменная для хранения значения textbox11
            string yearofstudy = textBox11.Text;

            // переменная для хранения индекса названия группы 
            int group = comboBox1.SelectedIndex + 1;

            // переменная для хранения индекса формы обучения
            int admtype = comboBox2.SelectedIndex + 1;
            string grouptype = group.ToString();
            string admisstype = admtype.ToString();

            // проверка студенческого на валидность
            if (unic_studnum.Equals("") || (textBox1.Text.Length != 7) || ((int.TryParse(textBox1.Text.ToString(), out g)) != true))
            {
                MessageBox.Show("Введите номер студенческого, его длина должна быть 7 цифр");
            }

            // проверка имени студента на пустоту
            else if (firstname.Equals(""))
            {
                MessageBox.Show("Введите имя учащегося");
            }

            // проверка фамилии студента на пустоту
            else if (lstname.Equals(""))
            {
                MessageBox.Show("Введите фамилию учащегося");
            }

            // проверка дня рождения студента на пустоту
            else if (birthday.Equals(""))
            {
                MessageBox.Show("Введите год рождения учащегося");
            }

            // проверка номера телефона студента на пустоту
            else if (phone.Equals(""))
            {
                MessageBox.Show("Введите номер телефона учащегося");
            }

            // проверка года поступления студента на валидность
            else if (yearofstudy.Equals("") || (int.TryParse(yearofstudy,out g) != true))
            {
                MessageBox.Show("Введите год поступления(целое число)");
            }
            else
            {
                string yearstudy = (int.Parse(DateTime.Now.Year.ToString()) - int.Parse(yearofstudy) + 1).ToString();
                if ((int.Parse(yearstudy) >5)||(int.Parse(yearstudy)<1))
                {
                    MessageBox.Show("Пожалуйта, проверьте правильность ввода даты поступления");
                }
                else
                {   // запрос для вставки данных о студенте
                    cmd = new SQLiteCommand("INSERT INTO StudDb (Stud_number,Group_num, first_name,last_name,middle_name,address,birthday,phone_number,mother_phone,father_phone, admission_type, year_of_study) VALUES ('" + unic_studnum + "','" + grouptype + "','" + firstname + "','"+ lstname + "','" + middlename + "','" + address + "','" + birthday + "','" + phone + "','" + motherphone + "','" + fatherphone + "','" + admisstype + "','" + yearstudy +  "');", con);
                    SQLiteDataReader mrd;
                    try
                    {
                        // вставка данных
                        mrd = cmd.ExecuteReader();
                        MessageBox.Show("Студент " + lstname + " успешно добавлен");
                    }
                    catch 
                    {
                        MessageBox.Show("Проверьте правильность введенных данных, возможно студент с такими параметрами (номер студенческого) уже существует");
                        return;
                    }
                }
            }
        }

        // очистка формы
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox11.Text = "";
        }
    }
}
