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
    public partial class ScheduleForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader da;

        public ScheduleForm()
        {
            InitializeComponent();
            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // метод для перехода на главную страницу
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            InfoPageForm infopage = new InfoPageForm();
            infopage.Show();
        }

        // выход из программы
        private void ScheduleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // запрос на вывод всех групп
            cmd = new SQLiteCommand("SELECT (group_name) " +
                                    "FROM GroupsDb ;", con);
            da = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("group_name", typeof(string));
            dt.Load(da);
            comboBox1.ValueMember = "group_name";
            comboBox1.DataSource = dt;

            // запрос на вывод семестра
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT (Timepair) " +
                                                   "FROM PeriodDb ;", con);
            SQLiteDataReader myreader1 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Timepair", typeof(string));
            dt2.Load(myreader1);
            comboBox2.ValueMember = "Timepair";
            comboBox2.DataSource = dt2;

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            // запрет редактирования расписания
            // понедельник
            textBox2.ReadOnly = true;
            textBox32.ReadOnly = true;
            textBox62.ReadOnly = true;

            textBox3.ReadOnly = true;
            textBox33.ReadOnly = true;
            textBox64.ReadOnly = true;

            textBox4.ReadOnly = true;
            textBox34.ReadOnly = true;
            textBox65.ReadOnly = true;

            textBox5.ReadOnly = true;
            textBox35.ReadOnly = true;
            textBox66.ReadOnly = true;

            textBox6.ReadOnly = true;
            textBox36.ReadOnly = true;
            textBox67.ReadOnly = true;

            // вторник
            textBox7.ReadOnly = true;
            textBox37.ReadOnly = true;
            textBox68.ReadOnly = true;

            textBox8.ReadOnly = true;
            textBox38.ReadOnly = true;
            textBox69.ReadOnly = true;

            textBox9.ReadOnly = true;
            textBox39.ReadOnly = true;
            textBox70.ReadOnly = true;

            textBox10.ReadOnly = true;
            textBox40.ReadOnly = true;
            textBox71.ReadOnly = true;

            textBox11.ReadOnly = true;
            textBox41.ReadOnly = true;
            textBox72.ReadOnly = true;

            // среда
            textBox12.ReadOnly = true;
            textBox42.ReadOnly = true;
            textBox73.ReadOnly = true;

            textBox13.ReadOnly = true;
            textBox43.ReadOnly = true;
            textBox74.ReadOnly = true;

            textBox14.ReadOnly = true;
            textBox44.ReadOnly = true;
            textBox75.ReadOnly = true;

            textBox15.ReadOnly = true;
            textBox45.ReadOnly = true;
            textBox76.ReadOnly = true;

            textBox16.ReadOnly = true;
            textBox46.ReadOnly = true;
            textBox77.ReadOnly = true;

            // четверг
            textBox17.ReadOnly = true;
            textBox47.ReadOnly = true;
            textBox78.ReadOnly = true;

            textBox19.ReadOnly = true;
            textBox18.ReadOnly = true;
            textBox79.ReadOnly = true;

            textBox21.ReadOnly = true;
            textBox20.ReadOnly = true;
            textBox80.ReadOnly = true;

            textBox49.ReadOnly = true;
            textBox48.ReadOnly = true;
            textBox81.ReadOnly = true;

            textBox51.ReadOnly = true;
            textBox50.ReadOnly = true;
            textBox82.ReadOnly = true;

            // пятница 

            textBox52.ReadOnly = true;
            textBox22.ReadOnly = true;
            textBox83.ReadOnly = true;

            textBox24.ReadOnly = true;
            textBox23.ReadOnly = true;
            textBox84.ReadOnly = true;

            textBox26.ReadOnly = true;
            textBox25.ReadOnly = true;
            textBox85.ReadOnly = true;

            textBox54.ReadOnly = true;
            textBox53.ReadOnly = true;
            textBox86.ReadOnly = true;

            textBox56.ReadOnly = true;
            textBox55.ReadOnly = true;
            textBox87.ReadOnly = true;

            // суббота 
            textBox28.ReadOnly = true;
            textBox27.ReadOnly = true;
            textBox88.ReadOnly = true;

            textBox30.ReadOnly = true;
            textBox29.ReadOnly = true;
            textBox89.ReadOnly = true;

            textBox57.ReadOnly = true;
            textBox31.ReadOnly = true;
            textBox90.ReadOnly = true;

            textBox59.ReadOnly = true;
            textBox58.ReadOnly = true;
            textBox91.ReadOnly = true;

            textBox61.ReadOnly = true;
            textBox60.ReadOnly = true;
            textBox92.ReadOnly = true;



        }

        // запрос на вывод расписания
        private void button1_Click(object sender, EventArgs e)
        {
            // проверка, что параметры для вывода расписания заполнены
            if ((textBox1.Text == "") || (textBox63.Text == "") || (comboBox1.SelectedIndex == -1) ||  (comboBox2.SelectedIndex == -1))
            {
                MessageBox.Show("Не заполнено одно из полей: название группы, семестр, половина группы или год обучения");
            }
            else
            {
                // проверка года обучения на валидность
                if ((textBox1.Text != "2") && (textBox1.Text != "1") && (textBox1.Text != "3") && (textBox1.Text != "4"))
                {
                    MessageBox.Show("Год обучения должен быть валидным (от 1 до 4)");
                }
                // проверка половины группы на валидность
                else if((textBox63.Text != "1")&& (textBox63.Text != "2") && (textBox63.Text != "3") && (textBox63.Text != "4"))
                {
                    MessageBox.Show("Половина группы должна быть валидной (от 1 до 4)");
                }
                else
                {

                    //понедельник

                    // переменная для хранения индекса группы
                    int getind = comboBox1.SelectedIndex + 1;

                    // переменная для хранения индекса семестра
                    int getind2 = comboBox2.SelectedIndex + 1;

                    // запросы для вывода предметов, учителей, номеров кабинетов по расписанию в понедельник
                    string onereq = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id WHERE Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 1 and LessonNum = 1";
                    string tworeq = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id WHERE Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 1 and LessonNum = 2";
                    string threereq = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id WHERE Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 1 and LessonNum = 3";
                    string fourreq = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id WHERE Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 1 and LessonNum = 4";
                    string fivereq = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id WHERE Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 1 and LessonNum = 5";

                    // урок 1
                    using (SQLiteCommand query1 = new SQLiteCommand(onereq, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox2.Text = da1.GetValue(0).ToString();
                                    textBox32.Text = da1.GetValue(1).ToString();
                                    textBox62.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox2.Text = "";
                                textBox32.Text = "";
                                textBox62.Text = "";
                            }
                        }
                    }
                    // урок 2
                    using (SQLiteCommand query1 = new SQLiteCommand(tworeq, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox3.Text = da1.GetValue(0).ToString();
                                    textBox33.Text = da1.GetValue(1).ToString();
                                    textBox64.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox3.Text = "";
                                textBox33.Text = "";
                                textBox64.Text = "";
                            }
                        }
                    }

                    // урок 3
                    using (SQLiteCommand query1 = new SQLiteCommand(threereq, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox4.Text = da1.GetValue(0).ToString();
                                    textBox34.Text = da1.GetValue(1).ToString();
                                    textBox65.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox4.Text = "";
                                textBox34.Text = "";
                                textBox65.Text = "";
                            }
                        }
                    }

                    // урок 4
                    using (SQLiteCommand query1 = new SQLiteCommand(fourreq, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox5.Text = da1.GetValue(0).ToString();
                                    textBox35.Text = da1.GetValue(1).ToString();
                                    textBox66.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox5.Text = "";
                                textBox35.Text = "";
                                textBox66.Text = "";
                            }
                        }
                    }

                    // урок 5
                    using (SQLiteCommand query1 = new SQLiteCommand(fivereq, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox6.Text = da1.GetValue(0).ToString();
                                    textBox36.Text = da1.GetValue(1).ToString();
                                    textBox67.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox6.Text = "";
                                textBox36.Text = "";
                                textBox67.Text = "";
                            }
                        }
                    }

                    // вторник

                    string onereq2 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 2 and LessonNum = 1";
                    string tworeq2 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 2 and LessonNum = 2";
                    string threereq2 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 2 and LessonNum = 3";
                    string fourreq2 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 2 and LessonNum = 4";
                    string fivereq2 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 2 and LessonNum = 5";

                    // урок 1
                    using (SQLiteCommand query1 = new SQLiteCommand(onereq2, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox7.Text = da1.GetValue(0).ToString();
                                    textBox37.Text = da1.GetValue(1).ToString();
                                    textBox68.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox7.Text = "";
                                textBox37.Text = "";
                                textBox68.Text = "";
                            }
                        }
                    }

                    // урок 2
                    using (SQLiteCommand query1 = new SQLiteCommand(tworeq2, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox8.Text = da1.GetValue(0).ToString();
                                    textBox38.Text = da1.GetValue(1).ToString();
                                    textBox69.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox8.Text = "";
                                textBox38.Text = "";
                                textBox69.Text = "";
                            }
                        }
                    }

                    // урок 3
                    using (SQLiteCommand query1 = new SQLiteCommand(threereq2, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox9.Text = da1.GetValue(0).ToString();
                                    textBox39.Text = da1.GetValue(1).ToString();
                                    textBox70.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox9.Text = "";
                                textBox39.Text = "";
                                textBox70.Text = "";
                            }
                        }
                    }

                    // урок 4
                    using (SQLiteCommand query1 = new SQLiteCommand(fourreq2, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox10.Text = da1.GetValue(0).ToString();
                                    textBox40.Text = da1.GetValue(1).ToString();
                                    textBox71.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox10.Text = "";
                                textBox40.Text = "";
                                textBox71.Text = "";
                            }
                        }
                    }

                    // урок 5
                    using (SQLiteCommand query1 = new SQLiteCommand(fivereq2, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox11.Text = da1.GetValue(0).ToString();
                                    textBox41.Text = da1.GetValue(1).ToString();
                                    textBox72.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox11.Text = "";
                                textBox41.Text = "";
                                textBox72.Text = "";
                            }
                        }
                    }

                    // среда
                    string onereq3 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 3 and LessonNum = 1";
                    string tworeq3 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 3 and LessonNum = 2";
                    string threereq3 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 3 and LessonNum = 3";
                    string fourreq3 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 3 and LessonNum = 4";
                    string fivereq3 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 3 and LessonNum = 5";

                    // урок 1
                    using (SQLiteCommand query1 = new SQLiteCommand(onereq3, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox12.Text = da1.GetValue(0).ToString();
                                    textBox42.Text = da1.GetValue(1).ToString();
                                    textBox73.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox12.Text = "";
                                textBox42.Text = "";
                                textBox73.Text = "";
                            }
                        }
                    }

                    // урок 2
                    using (SQLiteCommand query1 = new SQLiteCommand(tworeq3, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox13.Text = da1.GetValue(0).ToString();
                                    textBox43.Text = da1.GetValue(1).ToString();
                                    textBox74.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox13.Text = "";
                                textBox43.Text = "";
                                textBox74.Text = "";
                            }
                        }
                    }

                    // урок 3
                    using (SQLiteCommand query1 = new SQLiteCommand(threereq3, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox14.Text = da1.GetValue(0).ToString();
                                    textBox44.Text = da1.GetValue(1).ToString();
                                    textBox75.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox14.Text = "";
                                textBox44.Text = "";
                                textBox75.Text = "";
                            }
                        }
                    }

                    // урок 4
                    using (SQLiteCommand query1 = new SQLiteCommand(fourreq3, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox15.Text = da1.GetValue(0).ToString();
                                    textBox45.Text = da1.GetValue(1).ToString();
                                    textBox76.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox15.Text = "";
                                textBox45.Text = "";
                                textBox76.Text = "";
                            }
                        }
                    }

                    // урок 5
                    using (SQLiteCommand query1 = new SQLiteCommand(fivereq3, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox16.Text = da1.GetValue(0).ToString();
                                    textBox46.Text = da1.GetValue(1).ToString();
                                    textBox77.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox16.Text = "";
                                textBox46.Text = "";
                                textBox77.Text = "";
                            }
                        }
                    }

                    //четверг 
                    string onereq4 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 4 and LessonNum = 1";
                    string tworeq4 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 4 and LessonNum = 2";
                    string threereq4 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 4 and LessonNum = 3";
                    string fourreq4 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 4 and LessonNum = 4";
                    string fivereq4 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 4 and LessonNum = 5";

                    // урок 1
                    using (SQLiteCommand query1 = new SQLiteCommand(onereq4, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox17.Text = da1.GetValue(0).ToString();
                                    textBox47.Text = da1.GetValue(1).ToString();
                                    textBox78.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox17.Text = "";
                                textBox47.Text = "";
                                textBox78.Text = "";
                            }
                        }
                    }

                    // урок 2
                    using (SQLiteCommand query1 = new SQLiteCommand(tworeq4, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox19.Text = da1.GetValue(0).ToString();
                                    textBox18.Text = da1.GetValue(1).ToString();
                                    textBox79.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox19.Text = "";
                                textBox18.Text = "";
                                textBox79.Text = "";
                            }
                        }
                    }

                    // урок 3
                    using (SQLiteCommand query1 = new SQLiteCommand(threereq4, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox21.Text = da1.GetValue(0).ToString();
                                    textBox20.Text = da1.GetValue(1).ToString();
                                    textBox80.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox21.Text = "";
                                textBox20.Text = "";
                                textBox80.Text = "";
                            }
                        }
                    }

                    // урок 4
                    using (SQLiteCommand query1 = new SQLiteCommand(fourreq4, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox49.Text = da1.GetValue(0).ToString();
                                    textBox48.Text = da1.GetValue(1).ToString();
                                    textBox81.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox49.Text = "";
                                textBox48.Text = "";
                                textBox81.Text = "";
                            }
                        }
                    }

                    // урок 5
                    using (SQLiteCommand query1 = new SQLiteCommand(fivereq4, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox51.Text = da1.GetValue(0).ToString();
                                    textBox50.Text = da1.GetValue(1).ToString();
                                    textBox82.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox51.Text = "";
                                textBox50.Text = "";
                                textBox82.Text = "";
                            }
                        }
                    }

                    // пятница
                    string onereq5 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 5 and LessonNum = 1";
                    string tworeq5 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 5 and LessonNum = 2";
                    string threereq5 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 5 and LessonNum = 3";
                    string fourreq5 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 5 and LessonNum = 4";
                    string fivereq5 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 5 and LessonNum = 5";

                    // урок 1
                    using (SQLiteCommand query1 = new SQLiteCommand(onereq5, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox52.Text = da1.GetValue(0).ToString();
                                    textBox22.Text = da1.GetValue(1).ToString();
                                    textBox83.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox52.Text = "";
                                textBox22.Text = "";
                                textBox83.Text = "";
                            }
                        }
                    }

                    // урок 2
                    using (SQLiteCommand query1 = new SQLiteCommand(tworeq5, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox24.Text = da1.GetValue(0).ToString();
                                    textBox23.Text = da1.GetValue(1).ToString();
                                    textBox84.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox24.Text = "";
                                textBox23.Text = "";
                                textBox84.Text = "";
                            }
                        }
                    }

                    // урок 3
                    using (SQLiteCommand query1 = new SQLiteCommand(threereq5, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox26.Text = da1.GetValue(0).ToString();
                                    textBox25.Text = da1.GetValue(1).ToString();
                                    textBox85.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox26.Text = "";
                                textBox25.Text = "";
                                textBox85.Text = "";
                            }
                        }
                    }

                    // урок 4
                    using (SQLiteCommand query1 = new SQLiteCommand(fourreq5, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox54.Text = da1.GetValue(0).ToString();
                                    textBox53.Text = da1.GetValue(1).ToString();
                                    textBox86.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox54.Text = "";
                                textBox53.Text = "";
                                textBox86.Text = "";
                            }
                        }
                    }

                    // урок 5
                    using (SQLiteCommand query1 = new SQLiteCommand(fivereq5, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox56.Text = da1.GetValue(0).ToString();
                                    textBox55.Text = da1.GetValue(1).ToString();
                                    textBox87.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox56.Text = "";
                                textBox55.Text = "";
                                textBox87.Text = "";
                            }
                        }
                    }

                    // суббота
                    string onereq6 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 6 and LessonNum = 1";
                    string tworeq6 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 6 and LessonNum = 2";
                    string threereq6 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 6 and LessonNum = 3";
                    string fourreq6 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 6 and LessonNum = 4";
                    string fivereq6 = "SELECT SubjectsDb.Subject_name, Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.', ScheduleDb.Classromm From ScheduleDb JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id JOIN GroupsDb On ScheduleDb.Groupname = GroupsDb.group_id JOIN PeriodDb On ScheduleDb.Semestr = PeriodDb.Id Where Groupname = '" + getind.ToString() + "' and PairOfGroup = '" + textBox63.Text + "' and Semestr = '" + getind2.ToString() + "' and YearofStudy = '" + textBox1.Text + "' and DayOfWeek = 6 and LessonNum = 5";

                    // урок 1
                    using (SQLiteCommand query1 = new SQLiteCommand(onereq6, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox28.Text = da1.GetValue(0).ToString();
                                    textBox27.Text = da1.GetValue(1).ToString();
                                    textBox88.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox28.Text = "";
                                textBox27.Text = "";
                                textBox88.Text = "";
                            }
                        }
                    }

                    // урок 2
                    using (SQLiteCommand query1 = new SQLiteCommand(tworeq6, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox30.Text = da1.GetValue(0).ToString();
                                    textBox29.Text = da1.GetValue(1).ToString();
                                    textBox89.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox30.Text = "";
                                textBox29.Text = "";
                                textBox89.Text = "";
                            }
                        }
                    }

                    // урок 3
                    using (SQLiteCommand query1 = new SQLiteCommand(threereq6, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox57.Text = da1.GetValue(0).ToString();
                                    textBox31.Text = da1.GetValue(1).ToString();
                                    textBox90.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox57.Text = "";
                                textBox31.Text = "";
                                textBox90.Text = "";
                            }
                        }
                    }

                    // урок 4
                    using (SQLiteCommand query1 = new SQLiteCommand(fourreq6, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox59.Text = da1.GetValue(0).ToString();
                                    textBox58.Text = da1.GetValue(1).ToString();
                                    textBox91.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox59.Text = "";
                                textBox58.Text = "";
                                textBox91.Text = "";
                            }
                        }
                    }

                    // урок 5
                    using (SQLiteCommand query1 = new SQLiteCommand(fivereq6, con))
                    {
                        using (SQLiteDataReader da1 = query1.ExecuteReader())
                        {
                            if (da1.HasRows)
                            {
                                while (da1.Read())
                                {
                                    textBox61.Text = da1.GetValue(0).ToString();
                                    textBox60.Text = da1.GetValue(1).ToString();
                                    textBox92.Text = da1.GetValue(2).ToString();
                                }
                            }
                            else
                            {
                                textBox61.Text = "";
                                textBox60.Text = "";
                                textBox92.Text = "";
                            }
                        }
                    }


                }
            }
        }

        // метод сохранения расписания в виде картинки
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Изображение .bmp | *.bmp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int width, height;
                width = panel7.Width;
                height = panel7.Height;
                Bitmap bmp = new Bitmap(width, height);
                panel7.DrawToBitmap(bmp, panel7.ClientRectangle);
                bmp.Save(sfd.FileName);
            }
        }
    }
}
