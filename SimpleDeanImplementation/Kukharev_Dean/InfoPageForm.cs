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
    public partial class InfoPageForm : Form
    {
        SQLiteConnection con;
        public InfoPageForm()
        {
            InitializeComponent();

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            examscredits.Text = '\u25B6'.ToString();
            shedule.Text = '\u25B6'.ToString();
            studentsettings.Text = '\u25B6'.ToString();
            workers.Text = '\u25B6'.ToString();
            academicperform.Text = '\u25B6'.ToString();
            Subjects.Text = '\u25B6'.ToString();

        }

        // выход из программы
        private void InfoPageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // метод перехода на форму информации о студентах
        private void studentsettings_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentPageForm studpageform = new StudentPageForm();
            studpageform.Show();
        }

        // метод перехода на форму заполнения ведомостей
        private void examscredits_Click(object sender, EventArgs e)
        {
            this.Hide();
            ParamOfExamAndCreditForm paramexamcredit = new ParamOfExamAndCreditForm();
            paramexamcredit.Show();
        }

        // метод перехода на форму расписания занятий
        private void shedule_Click(object sender, EventArgs e)
        {
            this.Hide();
            ScheduleForm schedules = new ScheduleForm();
            schedules.Show();
        }

        // метод перехода на форму успеваемости
        private void academicperform_Click(object sender, EventArgs e)
        {
            this.Hide();
            AcademicPerForm academ = new AcademicPerForm();
            academ.Show();
        }

        // метод перехода на форму вкладышей для дипломов
        private void Subjects_Click(object sender, EventArgs e)
        {
            this.Hide();
            DiplomaForm dipl = new DiplomaForm();
            dipl.Show();
        }

        private void InfoPageForm_Load(object sender, EventArgs e)
        {
            // запрет на изменение размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();
            SQLiteCommand cmd;

            // переменная для проверки, делали ли мы увеличение курса или нет
            string plug = "";

            // переменная для определения текущего месяца(увеличение курса у студентов происходит в августе)
            string currmonth = DateTime.Now.Month.ToString();

            // если текущий месяц - август, тогда проверяем, было ли обновление курса у студента
            if (currmonth == "8")
            {
                // запрос на проверку обновления курса студента
                string queru = "SELECT * FROM Plug;";
                using (SQLiteCommand cmd2 = new SQLiteCommand(queru, con))
                {
                    using (SQLiteDataReader da = cmd2.ExecuteReader())
                    {
                        if (da.HasRows)
                        {
                            while (da.Read())
                            {
                                plug = da.GetValue(0).ToString();
                            }
                        }
                    }
                }

                // если обновления не было, то
                if (plug == "0")
                {
                    // запрос на увеличение курса студента на 1 в таблице StudDb
                    string inccourse = "UPDATE StudDb SET year_of_study = year_of_study + 1;";
                    using (cmd = new SQLiteCommand(inccourse, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // запрос на вставку информации о студенте, который учился последний год(выпуск) в таблицу GraduateDb
                    string updatedb = "INSERT INTO GraduateDb (StudNumber, GroupNum, Last_name, First_name, Middle_name, Phone, YearOfIssue) SELECT Stud_number, Group_num, last_name,first_name,middle_name, phone_number, date('now') FROM StudDb WHERE year_of_study = 5;";
                    using (cmd = new SQLiteCommand(updatedb, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // запрос на удаление студентов(выпускников) из таблицы StudDb
                    string deletedb = "DELETE FROM StudDb WHERE year_of_study = 5;";
                    using (cmd = new SQLiteCommand(deletedb, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // удаление из таблицы оценок студентов, которые выпустились 
                    string deletedb2 = "DELETE FROM MarksAndOffsets WHERE USER_ID NOT IN (SELECT StudDb.Id FROM StudDb);";
                    using (cmd = new SQLiteCommand(deletedb2, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // обновление значения change в таблице Plug для того, чтобы несколько раз не увеличивать курс учащихся
                    string incr = "UPDATE Plug SET Change = Change + 1;";
                    using (cmd = new SQLiteCommand(incr, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // если текущий месяц не август, то уменьшаем значение в Plug , если оно не было изменено ранее
            if (currmonth != "8")
            {
                using (con = new SQLiteConnection("Data Source=Dean.sqlite"))
                {
                    con.Open();

                    // запрос на выбор текущего значения заглушки
                    string queru = "SELECT Change FROM Plug;";
                    cmd = new SQLiteCommand(queru, con);
                    using (SQLiteDataReader da = cmd.ExecuteReader())
                    {
                        if (da.HasRows)
                        {
                            while (da.Read())
                            {
                                plug = da.GetValue(0).ToString();
                            }
                            // если значение заглушки 1, то понижаем ее
                            if (plug == "1")
                            {
                                // уменьшить на 1
                                string decr = "UPDATE Plug SET Change = Change - 1;";
                                using (cmd = new SQLiteCommand(decr, con))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }

        }

        private void workers_Click(object sender, EventArgs e)
        {
            this.Hide();
            TeachersForm teacher = new TeachersForm();
            teacher.Show();
        }
    }
}