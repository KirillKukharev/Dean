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
    public partial class AddMarksAndOffsetsForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteCommand cmd2;
        SQLiteDataReader da;
        SQLiteDataReader da2;

        public AddMarksAndOffsetsForm()
        {
            InitializeComponent();
            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void AddMarksAndOffsetsForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;
            
            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // задание запроса к бд
            cmd = new SQLiteCommand("SELECT (Subject_name) " +
                                    "FROM SubjectsDb;", con);

            // считывание данных
            da = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Subject_name", typeof(string));
            dt.Load(da);

            // вывод данных в combobox
            comboBox1.ValueMember = "Subject_name";
            comboBox1.DataSource = dt;
            comboBox1.SelectedIndex = -1;
        }

        // переход на форму оценок студентов
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudMarkForm marksForm = new StudMarkForm();
            marksForm.Show();
        }

        // выход из программы
        private void AddMarksAndOffsetsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // метод кнопки добавления оценки/ зачета
        private void button1_Click(object sender, EventArgs e)
        {
            // переменная для получения идентификатора студента
            string unic_ident = "";

            // переменная для считывания номера(индекса в combobox) предмета
            int getind = comboBox1.SelectedIndex + 1;

            // переменная для поиска поиска студента по студенческому
            string unic_id = textBox1.Text;
            

            // запрос для получения id студента из StudDb
            string getindexres = "SELECT StudDb.Id " +
                                 "FROM StudDb " +
                                 "WHERE StudDb.Stud_number = '" + unic_id + "';";
            cmd2 = new SQLiteCommand(getindexres, con);

            // присвоение значения id переменной unic_ident
            using (da2 = cmd2.ExecuteReader())
            {
                while (da2.Read())
                {
                    unic_ident = da2.GetValue(0).ToString();
                }
            }

            // запрос для проверки, что студент есть в бд
            string quer = "SELECT StudDb.Stud_number " +
                          "FROM StudDb " +
                          "WHERE StudDb.Stud_number ='" + unic_id + "' ;";
            cmd = new SQLiteCommand(quer, con);
            using (da = cmd.ExecuteReader())
            {
                // если студент есть в бд, то проверяем дальше, иначе выводим соответствующее сообщение
                if (da.HasRows)
                {
                    // если индекс предмета не пуст, проверяем дальше, иначе выводим сообщение
                    if (getind != 0)
                    {
                        // проверка, что поле с оценкой/зачетом не пусто
                        if (textBox3.Text != "")
                        {
                            // запрос для проверки, не был ли оценен студент раньше
                            string findpers = "SELECT MarksAndOffsets.USER_ID, MarksAndOffsets.SubjectName " +
                                              "FROM MarksAndOffsets " +
                                              "WHERE MarksAndOffsets.USER_ID ='"+ unic_ident + "' AND MarksAndOffsets.SubjectName ='" + getind + "' ;";

                            // запрос для вставки информации о сдаваемом студентом предмете
                            string resquery = "INSERT INTO MarksAndOffsets (USER_ID, SubjectName, MarkOrOfset, Date) VALUES ('" + unic_ident + "', '" + getind + "', '" + textBox3.Text + "','" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "') ;";
                            using (SQLiteCommand cmd3 = new SQLiteCommand(findpers, con)) 
                            {
                                using (SQLiteDataReader sdr2 = cmd3.ExecuteReader())
                                {
                                    // если уже есть информация о студенте, который сдавал соответствующий предмет, то выводим сообщение
                                    if (sdr2.HasRows)
                                    {
                                        MessageBox.Show("Студент уже получил оценку/зачет по данной дисциплине");
                                        return;
                                    }
                                    else
                                    {
                                        // иначе "вставляем" информацию в бд и выводим сообщение об успешной "вставке" 
                                        using (SQLiteCommand cmd4 = new SQLiteCommand(resquery, con))
                                        {
                                            try
                                            {
                                                da = cmd4.ExecuteReader();
                                                MessageBox.Show("Оценка/зачет у студента " + textBox1.Text + " успешно добавлена");
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                           
                            

                        }
                        else
                        {
                            MessageBox.Show("Заполните поле 'Оценка/Зачет'");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните поле 'Предмет'");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Студент с таким номером студенческого не найден", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
    }
}
