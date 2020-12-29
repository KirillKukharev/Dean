using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kukharev_Dean
{
    public partial class ParamOfExamAndCreditForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader da;

        public ParamOfExamAndCreditForm()
        {
            InitializeComponent();

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // выход из программы
        private void ParamOfExamAndCreditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // метод перехода на главную страницу
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            InfoPageForm infopageForm = new InfoPageForm();
            infopageForm.Show();
        }

        private void ParamOfExamAndCreditForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // запрос на вывод в combobox форму аттестации студентов
            cmd = new SQLiteCommand("SELECT PassName " +
                                    "FROM ExamOrCredit;", con);
            da = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PassName", typeof(string));
            dt.Load(da);
            typeComboBox.ValueMember = "PassName";
            typeComboBox.DataSource = dt;
            typeComboBox.SelectedIndex = -1;

            // запрет на выбор других параметров 
            periodBox.Enabled = false;
            groupone.Enabled = false;
            comboBox1.Enabled = false;
            chooseone.Enabled = false;
        }
        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // проверка, что ComboBox не пуст
            if (typeComboBox.Text != "")
            {
                periodBox.Enabled = true;
                
                // запрос на вывод периода аттестации
                SQLiteCommand cmd2 = new SQLiteCommand("SELECT Timepair " +
                                                       "FROM PeriodDb;", con);
                SQLiteDataReader myreader2 = cmd2.ExecuteReader();
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Timepair", typeof(string));
                dt2.Load(myreader2);
                periodBox.ValueMember = "Timepair";
                periodBox.DataSource = dt2;
                periodBox.SelectedIndex = -1;
            }
        }
        private void periodBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // проверка, что вид и период аттестации заполнены
            if ((typeComboBox.Text != "") && (periodBox.Text != ""))
            {
                
                groupone.Enabled = true;

                // запрос на вывод групп
                SQLiteCommand cmd3 = new SQLiteCommand("SELECT group_name " +
                                                       "FROM GroupsDb;", con);
                SQLiteDataReader myreader3 = cmd3.ExecuteReader();
                DataTable dt3 = new DataTable();
                dt3.Columns.Add("group_name", typeof(string));
                dt3.Load(myreader3);
                groupone.ValueMember = "group_name";
                groupone.DataSource = dt3;
                groupone.SelectedIndex = -1;
            }
            else
            {
                groupone.Enabled = false;
            }
        }

        private void groupone_SelectedIndexChanged(object sender, EventArgs e)
        {
            // проверка, что все остальные combobox заполнены
            if ((typeComboBox.Text != "") && (periodBox.Text != "") && (groupone.Text != "") && (comboBox2.Text !=""))
            {
                comboBox1.Enabled = true;

                // переменные для хранения выбранных параметров для ведомости
                int sem = periodBox.SelectedIndex + 1;
                string semester = sem.ToString();
                int cur = comboBox2.SelectedIndex + 1;
                string curyear = cur.ToString();
                int gr = groupone.SelectedIndex + 1;
                string group = gr.ToString();

                // запрос на вывод предметов
                SQLiteCommand cmd4 = new SQLiteCommand("SELECT DISTINCT SubjectsDb.Subject_name " +
                                                       "FROM ScheduleDb JOIN SubjectsDb on  ScheduleDb.Subject = SubjectsDb.Sub_id " +
                                                       "WHERE Groupname = '"+ group + "' and YearofStudy ='" + curyear + "' and Semestr = '"+ semester + "';", con);
                SQLiteDataReader myreader4 = cmd4.ExecuteReader();
                DataTable dt4 = new DataTable();
                dt4.Columns.Add("Subject_name", typeof(string));
                dt4.Load(myreader4);
                comboBox1.ValueMember = "Subject_name";
                comboBox1.DataSource = dt4;
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // проверка,что combobox заполнены
            if ((typeComboBox.Text != "") && (periodBox.Text != "") && (groupone.Text != ""))
            {
                int sem = periodBox.SelectedIndex + 1;
                string semester = sem.ToString();
                int cur = comboBox2.SelectedIndex + 1;
                string curyear = cur.ToString();
                int gr = groupone.SelectedIndex + 1;
                string group = gr.ToString();

                // запрос на вывод предметов
                SQLiteCommand cmd4 = new SQLiteCommand("SELECT DISTINCT SubjectsDb.Subject_name " +
                                                       "FROM ScheduleDb JOIN SubjectsDb on  ScheduleDb.Subject = SubjectsDb.Sub_id " +
                                                       "WHERE Groupname = '" + group + "' and YearofStudy ='" + curyear + "' and Semestr = '" + semester + "' ;", con);
                SQLiteDataReader myreader4 = cmd4.ExecuteReader();
                DataTable dt4 = new DataTable();
                dt4.Columns.Add("Subject_name", typeof(string));
                dt4.Load(myreader4);
                comboBox1.ValueMember = "Subject_name";
                comboBox1.DataSource = dt4;
                comboBox1.SelectedIndex = -1;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                chooseone.Enabled = true;
            }
            else
            {
                chooseone.Enabled = false;
            }
        }

        // переменные для передачи значений, выбранных в комбобоксах на форму ведомости
        public static string yer;
        public static string gro;
        public static string discipl;
        public static string teacher;

        private void chooseone_Click(object sender, EventArgs e)
        {
            // присвоение значений из combobox
            yer = comboBox2.Text;
            gro = groupone.Text;
            discipl = comboBox1.Text;

            // запрос для получения ФИО учителя
            string query = "SELECT DISTINCT Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2) || '.' " +
                           "FROM ScheduleDb " +
                           "JOIN Teachers ON ScheduleDb.Teacher = Teachers.Teacher_id " +
                           "JOIN GroupsDb ON ScheduleDb.Groupname = GroupsDb.group_id " +
                           "JOIN PeriodDb ON ScheduleDb.Semestr = PeriodDb.Id " +
                           "JOIN SubjectsDb ON ScheduleDb.Subject = SubjectsDb.Sub_id " +
                           "WHERE group_name = '" + groupone.Text + "' and Timepair = '" + periodBox.Text + "'and YearofStudy = '"+ comboBox2.Text + "' and Subject_name ='"+ comboBox1.Text + "' ;";
            SQLiteCommand cmd1 = new SQLiteCommand(query, con);
            SQLiteDataReader rer = cmd1.ExecuteReader();

            // если выбран зачет, то перенаправление на форму зачета, иначе на форму экзамена
            if (typeComboBox.SelectedIndex == 0)
            {
                while (rer.Read())
                {
                    teacher = rer.GetValue(0).ToString();
                }
                this.Hide();
                CreditForm cred = new CreditForm();
                cred.Show();     
                
            }
            else if (typeComboBox.SelectedIndex == 1)
            {
                while (rer.Read())
                {
                    teacher = rer.GetValue(0).ToString();
                }
                this.Hide();
                ExamForm exam = new ExamForm();
                exam.Show();
            }
            
        }
    }
}
