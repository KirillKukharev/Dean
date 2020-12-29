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
    public partial class KickForm : Form
    {
        // объявление переменных для взаимодействия с бд
        SQLiteConnection con;
        DataTable sTable;
        SQLiteDataAdapter sAdapter;
        public KickForm()
        {
            InitializeComponent();

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            // запрет на  добавление записей в datagridview1
            dataGridView1.AllowUserToAddRows = false;
            
        }

        // переход на форму должников 
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            DebtorsForm debtForm = new DebtorsForm();
            debtForm.Show();
        }

        private void KickForm_Load(object sender, EventArgs e)
        {
            // запрет изменения размеров формы
            MaximizeBox = false;

            // инициализация подключения к бд
            con = new SQLiteConnection("Data Source=Dean.sqlite");
            con.Open();

            // запрос на вывод студентов в dataGridView1, у которых больше трех задолженностей по предметам(включая зачеты)
            string querus = "SELECT DISTINCT StudDb.last_name Фамилия, StudDb.first_name Имя, GroupsDb.group_name Группа, StudDb.year_of_study Курс,  Незачеты AS 'Кол-во незачетов', Экзамены AS 'Кол-во не сданных экз.', Незачеты + Экзамены AS 'Всего'  " +
                            "FROM MarksAndOffsets " +
                            "JOIN StudDb ON MarksAndOffsets.USER_ID = StudDb.Id " +
                            "JOIN GroupsDb ON StudDb.Group_num = GroupsDb.group_id " +
                            "JOIN(SELECT USER_ID p1, COUNT(MarksAndOffsets.MarkOrOfset) Незачеты FROM MarksAndOffsets  WHERE MarksAndOffsets.MarkOrOfset  LIKE '%Неза%' GROUP BY p1) b1 ON MarksAndOffsets.USER_ID = b1.p1 " +
                            "JOIN(SELECT USER_ID p2, COUNT(MarksAndOffsets.MarkOrOfset) Экзамены FROM MarksAndOffsets  WHERE MarksAndOffsets.MarkOrOfset = 2 GROUP BY p2) b2 ON MarksAndOffsets.USER_ID = b2.p2; ";
            sAdapter = new SQLiteDataAdapter(querus, con);
            sTable = new DataTable();
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;
            dataGridView1.ReadOnly = true;

            // изменение стиля datagridview1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;

        }

        // выход из программы
        private void KickForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
