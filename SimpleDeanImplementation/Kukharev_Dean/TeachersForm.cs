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
    public partial class TeachersForm : Form
    {
        SQLiteConnection con;
        DataTable sTable;
        SQLiteDataAdapter sAdapter;
        public TeachersForm()
        {
            InitializeComponent();

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        // метод перехода на главную форму
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            InfoPageForm infopage = new InfoPageForm();
            infopage.Show();
        }

        private void TeachersForm_Load(object sender, EventArgs e)
        {
            // запрет на изменение размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // запрос на вывод  фамилии и 1 буквы имени учителей 
            string teachers = "SELECT Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2)  || '.'  AS 'ФИ учителя', Teachers.TeacherQualification AS 'Квалификация учителя' " +
                             "FROM Teachers " +
                             "ORDER BY \"ФИО учителя\";";
            using (sAdapter = new SQLiteDataAdapter(teachers, con))
            {
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }

            // ограничение длины вводимых данных для добавления учителя
            textBox1.MaxLength = 50;
            textBox2.MaxLength = 50;
            textBox3.MaxLength = 50;

            // изменение стиля datagridview1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;

        }

        // выход из программы
        private void TeachersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // метод вывода учителей при изменении текста в textbox1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            string teachsurn = textBox1.Text;

            // запрос на вывод учителей по введенной информации в textbox1
            string teacher = "SELECT Teachers.Teacher_lastname || ' ' || SUBSTR(Teachers.Teacher_name,0,2)  || '.'  AS 'ФИО учителя', Teachers.TeacherQualification AS 'Квалификация учителя' " +
                             "FROM Teachers " +
                             "WHERE Teachers.Teacher_name LIKE '%" + teachsurn + "%' OR Teachers.Teacher_lastname LIKE  '%" + teachsurn + "%'" +
                             "ORDER BY \"ФИО учителя\"";
            using (sAdapter = new SQLiteDataAdapter(teacher, con))
            {
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
        }

        // метод добавления учителя в бд
        private void button1_Click(object sender, EventArgs e)
        {
            // проверка, что имя и фамилия не пусты
            if ((textBox2.Text != "") && (textBox4.Text != ""))
            {
                string surn = textBox2.Text;
                string name = textBox4.Text;
                string graduate = textBox3.Text;

                // запрос на поиск одинаковых записей имени и фамилии в таблице
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT Teachers.Teacher_lastname, Teachers.Teacher_name " +
                                                      "FROM Teachers " +
                                                      "WHERE Teachers.Teacher_lastname= '"+ surn + "' AND Teachers.Teacher_name='" + name + "';", con))
                {
                    using (SQLiteDataReader sdr = cmd.ExecuteReader())
                    {
                        // если записи нашлись, то выводим соответствующее сообщение
                        if (sdr.HasRows)
                        {
                            MessageBox.Show("Учитель с указанными данными уже существует, \n  убедитесь в правильности введенных данных ", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            // если ученое звание не указано, переприсваиваем переменную graduate
                            if (graduate == "")
                            {
                                graduate = "Нет информации";

                                // запрос на вставку учителя в бд
                                string inserteach = "INSERT INTO Teachers  (Teacher_name, TeacherQualification, Teacher_lastname) VALUES ('" + name + "','" + graduate + "','" + surn + "');";
                                using(SQLiteCommand cmd1 = new SQLiteCommand(inserteach,con))
                                {
                                    SQLiteDataReader mrd;
                                    try
                                    {
                                        // вставка данных
                                        mrd = cmd1.ExecuteReader();
                                        MessageBox.Show("Учитель успешно добавлен");
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Проверьте правильность введенных данных");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                // запрос на вставку учителя в бд
                                string inserteach = "INSERT INTO Teachers  (Teacher_name, TeacherQualification, Teacher_lastname) VALUES ('" + name + "','" + graduate + "','" + surn + "');";
                                using (SQLiteCommand cmd1 = new SQLiteCommand(inserteach, con))
                                {
                                    SQLiteDataReader mrd;
                                    try
                                    {
                                        // вставка данных
                                        mrd = cmd1.ExecuteReader();
                                        MessageBox.Show("Учитель успешно добавлен");
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Проверьте правильность введенных данных");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                        
            }
            else
            {
                MessageBox.Show("Не заполнено одно из полей: ФИ учителя или ученое звание!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
        }
    }
}
