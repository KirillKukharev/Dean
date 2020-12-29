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
    public partial class SuccessForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader da;
        DataTable sTable;
        SQLiteDataAdapter sAdapter;

        public SuccessForm()
        {
            InitializeComponent();

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            // запрет на изменение и добавление записей в datagridview1
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        // метод для прехода на форму успеваемости
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AcademicPerForm acadForm = new AcademicPerForm();
            acadForm.Show();
        }

        private void SuccessForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // запрос на вывод списка групп в combobox1
            cmd = new SQLiteCommand("SELECT (group_name) " +
                                    "FROM GroupsDb;", con);
            da = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("group_name", typeof(string));
            dt.Load(da);
            comboBox1.ValueMember = "group_name";
            comboBox1.DataSource = dt;
            comboBox1.SelectedIndex = -1;
            // запрос на вывод года обучения в combobox2
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT (Num_year) " +
                                                   "FROM YearOfStudy;", con);
            SQLiteDataReader myreader1 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Num_year", typeof(string));
            dt2.Load(myreader1);
            comboBox2.ValueMember = "Num_year";
            comboBox2.DataSource = dt2;
            comboBox2.SelectedIndex = -1;

            // запрос на вывод стипендии для успевающих студентов в datagridView1 
            string querus = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                            "FROM( " +
                                    "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1  " +
                                    "FROM MarksAndOffsets " +
                                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                                    "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                    "GROUP BY p1) b1 " +
                            "JOIN( " +
                                    "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                    "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                    "GROUP BY p2) b2 " +
                            "ON b1.p1 = b2.p2 " +
                            "WHERE p1 = p2 AND result1 = result2 " +
                            "UNION " +
                            "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                            "FROM( " +
                                    "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1 " +
                                    "FROM MarksAndOffsets  " +
                                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                    "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                    "GROUP BY d1) b1 " +
                                    "JOIN( " +
                                            "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                            "FROM MarksAndOffsets " +
                                            "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                            "GROUP BY d2) b2 " +
                                    "ON b1.d1 = b2.d2 " +
                                    "JOIN( " +
                                            "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                            "FROM MarksAndOffsets " +
                                            "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                            "GROUP BY d3) b3 " +
                                    "ON b1.d1 = b3.d3 " +
                            "UNION " +
                            "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                            "FROM( " +
                                    "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1 " +
                                    "FROM MarksAndOffsets  " +
                                    "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                                    "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                    "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                    "GROUP BY k1) b1 " +
                                    "JOIN( " +
                                            "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                            "FROM MarksAndOffsets " +
                                            "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                            "GROUP BY k3) b3  " +
                                    "ON b1.k1 = b3.k3 " +
                                    "JOIN( " +
                                            "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                            "FROM MarksAndOffsets " +
                                            "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                            "GROUP BY k4) b4  " +
                                    "ON b1.k1 = b4.k4 " +
                                    "WHERE result1 + result3 = result4 " +
                            "UNION " +
                            "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                            "FROM( " +
                                    "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1 " +
                                    "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                                    "WHERE StudDb.year_of_study = 1 AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                                    "GROUP BY c1);";
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

        // выход из программы
        private void SuccessForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // сброс фильтров
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        // вывод данных о студенте в datagridview согласно выбранной группы(отслеживание изменения группы) и году обучения 
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int getind = comboBox1.SelectedIndex + 1;
            int getind2 = comboBox2.SelectedIndex + 1;
            if ((getind2 == 0) && (getind != 0))
            {
                string query = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                "FROM( " +
                        "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind  " +
                        "FROM MarksAndOffsets " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                        "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p1) b1 " +
                "JOIN( " +
                        "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                        "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p2) b2 " +
                "ON b1.p1 = b2.p2 " +
                "WHERE p1 = p2 AND result1 = result2 AND ind = '"+ getind + "' " +
                "UNION " +
                "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind2 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY d1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d2) b2 " +
                        "ON b1.d1 = b2.d2 " +
                        "JOIN( " +
                                "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d3) b3 " +
                        "ON b1.d1 = b3.d3 " +
                "WHERE ind2 = '"+ getind + "' " +
                "UNION " +
                "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind3 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY k1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k3) b3  " +
                        "ON b1.k1 = b3.k3 " +
                        "JOIN( " +
                                "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                "FROM MarksAndOffsets " +
                                "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k4) b4  " +
                        "ON b1.k1 = b4.k4 " +
                "WHERE result1 + result3 = result4 AND ind3='"+ getind + "' " +
                "UNION " +
                "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1, GroupsDb.group_id ind4 " +
                        "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE StudDb.year_of_study = 1 AND ind4 = '" + getind + "' AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                        "GROUP BY c1);";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 != 0) && (getind != 0))
            {
                string query = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                "FROM( " +
                        "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind  " +
                        "FROM MarksAndOffsets " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                        "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p1) b1 " +
                "JOIN( " +
                        "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                        "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p2) b2 " +
                "ON b1.p1 = b2.p2 " +
                "WHERE p1 = p2 AND result1 = result2 AND ind = '" + getind + "' AND y1 = '"+ getind2 +"' " +
                "UNION " +
                "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind2 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY d1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d2) b2 " +
                        "ON b1.d1 = b2.d2 " +
                        "JOIN( " +
                                "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d3) b3 " +
                        "ON b1.d1 = b3.d3 " +
                "WHERE ind2 = '" + getind + "' AND y2 = '"+ getind2 +"' " +
                "UNION " +
                "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind3 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY k1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k3) b3  " +
                        "ON b1.k1 = b3.k3 " +
                        "JOIN( " +
                                "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                "FROM MarksAndOffsets " +
                                "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k4) b4  " +
                        "ON b1.k1 = b4.k4 " +
                "WHERE result1 + result3 = result4 AND ind3='" + getind + "' AND y3 = '"+ getind2 +"' " +
                "UNION " +
                "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1, GroupsDb.group_id ind4 " +
                        "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE StudDb.year_of_study = 1 AND ind4 = '" + getind + "' AND y4 = '"+ getind2 + "'  AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                        "GROUP BY c1);";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 == 0) && (getind == 0))
            {
                string query = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                "FROM( " +
                        "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind  " +
                        "FROM MarksAndOffsets " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                        "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p1) b1 " +
                "JOIN( " +
                        "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                        "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p2) b2 " +
                "ON b1.p1 = b2.p2 " +
                "WHERE p1 = p2 AND result1 = result2  " +
                "UNION " +
                "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind2 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY d1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d2) b2 " +
                        "ON b1.d1 = b2.d2 " +
                        "JOIN( " +
                                "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d3) b3 " +
                        "ON b1.d1 = b3.d3 " +
                "UNION " +
                "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind3 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY k1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k3) b3  " +
                        "ON b1.k1 = b3.k3 " +
                        "JOIN( " +
                                "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                "FROM MarksAndOffsets " +
                                "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k4) b4  " +
                        "ON b1.k1 = b4.k4 " +
                "WHERE result1 + result3 = result4  " +
                "UNION " +
                "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1, GroupsDb.group_id ind4 " +
                        "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE StudDb.year_of_study = 1  AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                        "GROUP BY c1);";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 != 0) && (getind == 0))
            {
                string query = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                "FROM( " +
                        "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind  " +
                        "FROM MarksAndOffsets " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                        "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p1) b1 " +
                "JOIN( " +
                        "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                        "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p2) b2 " +
                "ON b1.p1 = b2.p2 " +
                "WHERE p1 = p2 AND result1 = result2 AND y1 = '" + getind2 + "' " +
                "UNION " +
                "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind2 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY d1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d2) b2 " +
                        "ON b1.d1 = b2.d2 " +
                        "JOIN( " +
                                "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d3) b3 " +
                        "ON b1.d1 = b3.d3 " +
                "WHERE  y2 = '" + getind2 + "' " +
                "UNION " +
                "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind3 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY k1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k3) b3  " +
                        "ON b1.k1 = b3.k3 " +
                        "JOIN( " +
                                "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                "FROM MarksAndOffsets " +
                                "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k4) b4  " +
                        "ON b1.k1 = b4.k4 " +
                "WHERE result1 + result3 = result4 AND y3 = '" + getind2 + "' " +
                "UNION " +
                "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1, GroupsDb.group_id ind4 " +
                        "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE StudDb.year_of_study = 1 AND y4 = '" + getind2 + "'  AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                        "GROUP BY c1);";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
        }

        // вывод данных о студенте в datagridview согласно выбранной группы и году обучения (отслеживание изменения года обучения)
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int getind = comboBox1.SelectedIndex + 1;
            int getind2 = comboBox2.SelectedIndex + 1;
            if ((getind2 == 0) && (getind != 0))
            {
                string query = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                "FROM( " +
                        "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind  " +
                        "FROM MarksAndOffsets " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                        "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p1) b1 " +
                "JOIN( " +
                        "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                        "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p2) b2 " +
                "ON b1.p1 = b2.p2 " +
                "WHERE p1 = p2 AND result1 = result2 AND ind = '" + getind + "' " +
                "UNION " +
                "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind2 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY d1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d2) b2 " +
                        "ON b1.d1 = b2.d2 " +
                        "JOIN( " +
                                "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d3) b3 " +
                        "ON b1.d1 = b3.d3 " +
                "WHERE ind2 = '" + getind + "' " +
                "UNION " +
                "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind3 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY k1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k3) b3  " +
                        "ON b1.k1 = b3.k3 " +
                        "JOIN( " +
                                "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                "FROM MarksAndOffsets " +
                                "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k4) b4  " +
                        "ON b1.k1 = b4.k4 " +
                "WHERE result1 + result3 = result4 AND ind3='" + getind + "' " +
                "UNION " +
                "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1, GroupsDb.group_id ind4 " +
                        "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE StudDb.year_of_study = 1 AND ind4 = '" + getind + "' AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                        "GROUP BY c1);";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 != 0) && (getind != 0))
            {
                string query = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                "FROM( " +
                        "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind  " +
                        "FROM MarksAndOffsets " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                        "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p1) b1 " +
                "JOIN( " +
                        "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                        "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p2) b2 " +
                "ON b1.p1 = b2.p2 " +
                "WHERE p1 = p2 AND result1 = result2 AND ind = '" + getind + "' AND y1 = '" + getind2 + "' " +
                "UNION " +
                "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind2 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY d1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d2) b2 " +
                        "ON b1.d1 = b2.d2 " +
                        "JOIN( " +
                                "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d3) b3 " +
                        "ON b1.d1 = b3.d3 " +
                "WHERE ind2 = '" + getind + "' AND y2 = '" + getind2 + "' " +
                "UNION " +
                "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind3 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY k1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k3) b3  " +
                        "ON b1.k1 = b3.k3 " +
                        "JOIN( " +
                                "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                "FROM MarksAndOffsets " +
                                "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k4) b4  " +
                        "ON b1.k1 = b4.k4 " +
                "WHERE result1 + result3 = result4 AND ind3='" + getind + "' AND y3 = '" + getind2 + "' " +
                "UNION " +
                "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1, GroupsDb.group_id ind4 " +
                        "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE StudDb.year_of_study = 1 AND ind4 = '" + getind + "' AND y4 = '" + getind2 + "'  AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                        "GROUP BY c1);";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 == 0) && (getind == 0))
            {
                string query = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                "FROM( " +
                        "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind  " +
                        "FROM MarksAndOffsets " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                        "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p1) b1 " +
                "JOIN( " +
                        "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                        "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p2) b2 " +
                "ON b1.p1 = b2.p2 " +
                "WHERE p1 = p2 AND result1 = result2  " +
                "UNION " +
                "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind2 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY d1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d2) b2 " +
                        "ON b1.d1 = b2.d2 " +
                        "JOIN( " +
                                "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d3) b3 " +
                        "ON b1.d1 = b3.d3 " +
                "UNION " +
                "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind3 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY k1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k3) b3  " +
                        "ON b1.k1 = b3.k3 " +
                        "JOIN( " +
                                "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                "FROM MarksAndOffsets " +
                                "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k4) b4  " +
                        "ON b1.k1 = b4.k4 " +
                "WHERE result1 + result3 = result4  " +
                "UNION " +
                "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1, GroupsDb.group_id ind4 " +
                        "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE StudDb.year_of_study = 1  AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                        "GROUP BY c1);";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
            else if ((getind2 != 0) && (getind == 0))
            {
                string query = "SELECT DISTINCT l1 Фамилия, f1 Имя, g1 Группа, y1 as 'Год обучения', 4000 Стипендия  " +
                "FROM( " +
                        "SELECT StudDb.last_name l1, StudDb.first_name f1, GroupsDb.group_name g1, StudDb.year_of_study y1, USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind  " +
                        "FROM MarksAndOffsets " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id  " +
                        "WHERE(MarksAndOffsets.MarkOrOfset = '5' OR MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p1) b1 " +
                "JOIN( " +
                        "SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                        "FROM MarksAndOffsets WHERE(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND(CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY p2) b2 " +
                "ON b1.p1 = b2.p2 " +
                "WHERE p1 = p2 AND result1 = result2 AND y1 = '" + getind2 + "' " +
                "UNION " +
                "SELECT  DISTINCT l2 Фамилия, f2 Имя, g2 Группа, y2 as 'Год обучения', 3500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l2, StudDb.first_name f2, GroupsDb.group_name g2, StudDb.year_of_study y2, USER_ID d1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind2 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE(MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY d1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID d2, COUNT(MarksAndOffsets.MarkOrOfset) result2 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '5')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d2) b2 " +
                        "ON b1.d1 = b2.d2 " +
                        "JOIN( " +
                                "SELECT USER_ID d3, COUNT(MarksAndOffsets.MarkOrOfset) result3 " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY d3) b3 " +
                        "ON b1.d1 = b3.d3 " +
                "WHERE  y2 = '" + getind2 + "' " +
                "UNION " +
                "SELECT DISTINCT l3 Фамилия, f3 Имя, g3 Группа, y3 as 'Год обучения', 3000 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l3, StudDb.first_name f3, GroupsDb.group_name g3, StudDb.year_of_study y3, USER_ID k1, COUNT(MarksAndOffsets.MarkOrOfset) result1, GroupsDb.group_id ind3 " +
                        "FROM MarksAndOffsets  " +
                        "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                        "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE (MarksAndOffsets.MarkOrOfset LIKE '%Зач%')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                        "GROUP BY k1) b1 " +
                        "JOIN( " +
                                "SELECT USER_ID k3, COUNT(MarksAndOffsets.MarkOrOfset) result3  " +
                                "FROM MarksAndOffsets " +
                                "WHERE(MarksAndOffsets.MarkOrOfset = '4')  AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k3) b3  " +
                        "ON b1.k1 = b3.k3 " +
                        "JOIN( " +
                                "SELECT USER_ID k4, COUNT(MarksAndOffsets.MarkOrOfset) result4  " +
                                "FROM MarksAndOffsets " +
                                "WHERE (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) <= 8) AND (CAST((julianday('now', 'start of month') - julianday(Date, 'start of month')) / 30 as integer) >= 2) " +
                                "GROUP BY k4) b4  " +
                        "ON b1.k1 = b4.k4 " +
                "WHERE result1 + result3 = result4 AND y3 = '" + getind2 + "' " +
                "UNION " +
                "SELECT l4 Фамилия, f4 Имя, g4 Группа, y4 as 'Год обучения', 1500 Стипендия " +
                "FROM( " +
                        "SELECT StudDb.last_name l4, StudDb.first_name f4, GroupsDb.group_name g4, StudDb.year_of_study y4, StudDb.Id c1, GroupsDb.group_id ind4 " +
                        "FROM StudDb JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                        "WHERE StudDb.year_of_study = 1 AND y4 = '" + getind2 + "'  AND strftime('%m', date('now', 'start of month')) IN('01', '09', '10', '11', '12')  " +
                        "GROUP BY c1);";
                sAdapter = new SQLiteDataAdapter(query, con);
                sTable = new DataTable();
                sAdapter.Fill(sTable);
                dataGridView1.DataSource = sTable;
            }
        }

        // метод для сохранения студентов в виде картинки
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Изображение .bmp | *.bmp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int width, height;
                width = panel1.Width;
                height = panel1.Height;
                Bitmap bmp = new Bitmap(width, height);
                panel1.DrawToBitmap(bmp, panel1.ClientRectangle);
                bmp.Save(sfd.FileName);
            }
        }
    }
}
