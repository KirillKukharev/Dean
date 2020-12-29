using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Data.SqlClient;
using System.Data.SQLite;
using System.Globalization;

namespace Kukharev_Dean
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            this.passtextbox.AutoSize = false;

            // фиксирование формы на центре экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            this.passtextbox.Size = new Size(this.passtextbox.Size.Width, 39);

        }

        // выход из программы
        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // метод enter и метод leave для эмуляции placeholder логина
        private void logintextbox_Enter(object sender, EventArgs e)
        {
            if (logintextbox.Text == "Логин")
            {
                logintextbox.Text = "";

                logintextbox.ForeColor = Color.Black;
            }
        }

        private void logintextbox_Leave(object sender, EventArgs e)
        {
            if (logintextbox.Text == "")
            {
                logintextbox.Text = "Логин";

                logintextbox.ForeColor = Color.Silver;
            }
        }

        // метод enter и метод leave для эмуляции placeholder пароля
        private void passtextbox_Enter(object sender, EventArgs e)
        {
            if (passtextbox.Text == "Пароль")
            {
                passtextbox.Text = "";

                passtextbox.ForeColor = Color.Black;
            }
        }

        private void passtextbox_Leave(object sender, EventArgs e)
        {
            if (passtextbox.Text == "")
            {
                passtextbox.Text = "Пароль";

                passtextbox.ForeColor = Color.Silver;
            }
        }

        // методы для возможности передвигать форму авторизации мышкой
        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {

            // переменные для хранения введенной информации в поля логин и пароль
            string logins = logintextbox.Text;
            string passwords = passtextbox.Text;

            // инициализация подключения к бд
            using (SQLiteConnection con = new SQLiteConnection("Data Source=Dean.sqlite"))
            {
                con.Open();



                //запрос для сравнения введенных с клавиатуры данных с данными из бд
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT Login, Password " +
                                                      "FROM LogDb " +
                                                      "WHERE Login=@log AND Password=@pass ;", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@log", logins);
                    cmd.Parameters.AddWithValue("@pass", passwords);

                    using (SQLiteDataReader sdr = cmd.ExecuteReader())
                    {
                        // проверка, что строка с указанными логином и паролем есть в бд
                        if (sdr.HasRows)
                        {
                            sdr.Close();
                            con.Close();
                            MessageBox.Show("Добро пожаловать", "Добро пожаловать", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            
                            InfoPageForm infoPage = new InfoPageForm();
                            infoPage.Show();
                        }
                        else
                        {
                            DialogResult dialogResult = MessageBox.Show("Отказано в доступе", "Внимание", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            if (dialogResult == DialogResult.Retry)
                            {
                                return;
                            }
                            else if (dialogResult == DialogResult.Cancel)
                            {
                                this.Close();
                            }
                            con.Close();
                        }
                    }
                }

            }

        }
    }
}
