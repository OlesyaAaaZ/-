using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Data.SqlClient;
using System.Windows.Forms;
using ClassLibrary1;

namespace Автомастерская
{
    public partial class авторизация : Form
    {
        private const string name = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";
        private int loginAttempts = 0;
        private int remainingAttemptsAfterBlock = 1;
        private string captcha;
        private int blockTime = 180;
        public авторизация()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            labelcaptcha.Visible = false;
            textBoxcaptcha.Visible = false;
            pictureBoxcaptcha.Visible = false;
            pictureBoxreplay.Visible = false;
            loginTimer.Interval = 1000;
            loginTimer.Tick += LoginTimer_Tick;
        }

        private void GenerateCaptcha()
        {
            captcha = Class1.GenerateCaptcha(pictureBoxcaptcha);
            labelcaptcha.Visible = true;
            textBoxcaptcha.Visible = true;
            pictureBoxcaptcha.Visible = true;
            pictureBoxreplay.Visible = true;
        }



        private bool Authenticate(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(name))
            {
                connection.Open();
                string query = $"SELECT COUNT(*) FROM users WHERE login =\'{textBox1.Text}\' AND password =\'{textBox2.Text}\'"; 
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);
                    int result = (int)command.ExecuteScalar();
                    return result > 0;
                }
            }
        }
        private void LogLoginAttempt(string username, bool isSuccess)
        {
            using (SqlConnection connection = new SqlConnection(name))
            {
                connection.Open();
                string query = "INSERT INTO LoginHistory (AttemptTime, Login, IsSuccess) VALUES (@AttemptTime, @Login, @IsSuccess)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AttemptTime", DateTime.Now);
                    command.Parameters.AddWithValue("@Login", username);
                    command.Parameters.AddWithValue("@IsSuccess", isSuccess);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool value1 = false;
                string sql;
                if (textBox1.Text != "" && textBox2.Text != "")
                {

                    SqlConnection MyConnection = new SqlConnection(name);
                    MyConnection.Open();
                    if (textBox1.Text != "" && textBox2.Text != "")
                    {
                        sql = $"SELECT id_user FROM users where login=\'{textBox1.Text}\' and password=\'{textBox2.Text}\'";
                        SqlCommand sqlCommand = new SqlCommand(sql, MyConnection);
                        string d = sqlCommand.ExecuteScalar() == null ? string.Empty : sqlCommand.ExecuteScalar().ToString();
                        if (d != "")
                        {
                            value1 = true;
                            this.Hide();
                            new окно_с_инф(Convert.ToInt32(d)).Show();
                        }
                    }
                }

                        if (loginAttempts >= 3)
                {
                    MessageBox.Show("Вы превышаете максимальное количество попыток. Перезапустите приложение.", "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }

                if (loginAttempts >= 1 && textBoxcaptcha.Text != captcha)
                {
                    MessageBox.Show("Неверная капча. Попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GenerateCaptcha();
                    LogLoginAttempt(textBox1.Text,  false);
                    return;
                }

                if (Authenticate(textBox1.Text, textBox2.Text))
                {
                    
                   LogLoginAttempt(textBox1.Text,  true);
                    
                }
                else
                {
                    loginAttempts++;
                    LogLoginAttempt(textBox1.Text, false);
                    if (loginAttempts == 1)
                    {
                        MessageBox.Show("Неправильный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        labelcaptcha.Visible = true;
                        textBoxcaptcha.Visible = true;
                        pictureBoxcaptcha.Visible = true;
                        pictureBoxreplay.Visible = true;
                        GenerateCaptcha();
                    }
                    else if (loginAttempts <= 2)
                    {
                        MessageBox.Show("Неправильный логин, пароль или капча. Попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        GenerateCaptcha(); 
                    }
                    else if (loginAttempts == 3)
                    {
                        MessageBox.Show("Вы превысили количество попыток. Активируется блокировка.", "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        StartBlockTimer();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click_AfterBlock(object sender, EventArgs e)
        {

            try
            {
                if (loginAttempts >= 1 && textBoxcaptcha.Text != captcha)
                {
                    MessageBox.Show("Неверная капча. Попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GenerateCaptcha();
                    return;
                }
                if (Authenticate(textBox1.Text, textBox2.Text))
                {
                    MessageBox.Show("Авторизация успешна!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
              
                }
                else
                {
                    DialogResult result = MessageBox.Show("Вы превысили количество попыток. Перезапустите приложение для повторного входа.", "Блокировка", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        Application.Restart(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
        private void pictureBoxreplay_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }
        private void StartBlockTimer()
        {
            blockTime = 18;   
            loginTimer.Start();
            button1.Enabled = false;
            MessageBox.Show("Доступ временно заблокирован. Повторите попытку через 3 минуты.", "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void LoginTimer_Tick(object sender, EventArgs e)
        {
            blockTime--;
            if (blockTime <= 0)
            {
                loginTimer.Stop();
                button1.Enabled = true;   
                loginAttempts = 0;        
                MessageBox.Show("Вы можете повторить попытку входа.", "Блокировка снята", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Click -= button1_Click;
                button1.Click += button1_Click_AfterBlock;
            }
        }

        private void авторизация_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Application.Exit();
            }
        }
    }
}