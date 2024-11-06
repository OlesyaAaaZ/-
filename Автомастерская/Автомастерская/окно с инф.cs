using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Автомастерская
{
    public partial class окно_с_инф : Form
    {
        private const string name = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";
        int d;

        public окно_с_инф()
        {
            InitializeComponent();
        }
        public окно_с_инф(int d1)
        {
            d = d1;
            InitializeComponent();
            SqlConnection MyConnection = new SqlConnection(name);
            MyConnection.Open();
            string sql = "SELECT users.fio, user_type.type " +
                "FROM users JOIN user_type ON users.user_type = user_type.id_user_type " +
                "WHERE id_user = @id and users.user_type= user_type.id_user_type";

            using (SqlCommand cmd1 = new SqlCommand(sql, MyConnection))
            {
              
                cmd1.Parameters.AddWithValue("@id", d);

                using (SqlDataReader Reader1 = cmd1.ExecuteReader())
                {
                    while (Reader1.Read())
                    {
                        label_fio.Text = Reader1["fio"].ToString();
                        label_role.Text = Reader1["type"].ToString();
                    }
                }
            }

            MyConnection.Close();
         
        }

      

        
       

        private void label2_Click(object sender, EventArgs e)
        {
         
            Application.OpenForms["окно_с_инф"].Dispose();
            new авторизация().Show();

        }

        private void историяВходовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            история_входов история_Входов = new история_входов(d);
            история_Входов.ShowDialog();
            this.Hide();
        }

        private void окно_с_инф_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Application.Exit();
            }
        }

        private void buttonUser_Click(object sender, EventArgs e)
        {
            Application.OpenForms["окно_с_инф"].Dispose();
            new заявкиКлиентов(d).Show();
        }

       

        private void buttonAdmin_Click(object sender, EventArgs e)
        {
            формаЗаявок формаЗаявок = new формаЗаявок(d);
            формаЗаявок. ShowDialog();
            this.Hide();
        }

        private void окно_с_инф_Load(object sender, EventArgs e)
        {
            SqlConnection MyConnection = new SqlConnection(name);
            MyConnection.Open();
            string sql = $"SELECT user_type from users where id_user=\'{d}\'";
            SqlCommand cmd1 = new SqlCommand(sql, MyConnection);
            int k1 = Convert.ToInt32(cmd1.ExecuteScalar().ToString());
            MyConnection.Close();
            if (k1 == 4 || k1 == 2)
            {
                buttonAdmin.Visible = false;
                buttonUser.Visible = true;

                menuStrip1.Visible = false; //заказчик и механик
            }
            else
            {
               // button3.Visible = false; //оператор и менеджер
            }
        }
    }
}