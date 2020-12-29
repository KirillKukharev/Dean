using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kukharev_Dean
{
    public partial class AcademicPerForm : Form
    {
        public AcademicPerForm()
        {
            InitializeComponent();
            // инициализация формы по центру экрана
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // метод перехода на главную форму
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            InfoPageForm infopageForm = new InfoPageForm();
            infopageForm.Show();
        }

        // метод для запрета изменения размеров формы
        private void AcademicPerForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        // метод для выхода из программы
        private void AcademicPerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // метод для перехода на форму оценок студентов
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudMarkForm marks = new StudMarkForm();
            marks.Show();
        }

        // метод для перехода на форму должников
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            DebtorsForm debtrs = new DebtorsForm();
            debtrs.Show();
        }

        // метод для перехода на форму стипендиатов
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            SuccessForm successForm = new SuccessForm();
            successForm.Show();
        }
    }
}
