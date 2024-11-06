using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Автомастерская
{
    public partial class заявкиКлиентов : Form
    {
        private const string connectionString = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";
        int d;
        int k = 0;
        DataTable ds;
        public заявкиКлиентов(int d1)
        {
            InitializeComponent();
            this.d = d1;

            LoadClientRequests();
        }


      
        private void LoadClientRequests()
        {
            int userType;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT user_type FROM users WHERE id_user = @userId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@userId", d);
                userType = Convert.ToInt32(cmd.ExecuteScalar());
            }
            string query;
            if (userType == 4) 
            {
                query = "SELECT r.start_date AS [Дата начала], " +
            "cm.model AS [Модель], " +
            "pd.problem_descryption AS [Описание проблемы], " +
            "rs.status_name AS [Статус], " +
            "c.message AS [Комментарий] " +
            "FROM request r " +
            "LEFT JOIN car_model cm ON r.car_model = cm.id_car_model " +
            "LEFT JOIN problem_descryption pd ON r.id_problem_descryption = pd.id_problem_descryption " +
            "LEFT JOIN request_status rs ON r.id_request_status = rs.id_request_status " +
            "LEFT JOIN comments c ON r.id_request = c.id_request " +
            "WHERE r.client_id = @clientId";

                label3.Visible=false;
                label4.Visible=false;
            }
            else if (userType == 2)
            {
                query = "SELECT r.start_date AS [Дата начала], " +
             "cm.model AS [Модель], " +
             "pd.problem_descryption AS [Описание проблемы], " +
             "rs.status_name AS [Статус], " +
             "c.message AS [Комментарий] " +
             "FROM request r " +
             "JOIN car_model cm ON r.car_model = cm.id_car_model " +
             "JOIN problem_descryption pd ON r.id_problem_descryption = pd.id_problem_descryption " +
             "JOIN request_status rs ON r.id_request_status = rs.id_request_status " +
             "LEFT JOIN comments c ON r.id_request = c.id_request " +
             "WHERE r.master_id = @masterId";

                menuStrip1.Visible=false;
            }
            else
            {
               
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
              command.Parameters.AddWithValue("@clientId", d);
                command.Parameters.AddWithValue("@masterId", d);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; 
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void заявкиКлиентов_Load(object sender, EventArgs e)
        {
            SqlConnection MyConnection = new SqlConnection(connectionString);
            MyConnection.Open();
            string sql = $"SELECT user_type from users where id_user=\'{d}\'";
            SqlCommand cmd1 = new SqlCommand(sql, MyConnection);
            string k1 = cmd1.ExecuteScalar().ToString();
            MyConnection.Close();

            LoadClientRequests();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.OpenForms["заявкиКлиентов"].Dispose();
            new окно_с_инф(d).Show();
        }

        private void заявкиКлиентов_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Application.Exit();
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.OpenForms["заявкиКлиентов"].Dispose();
            new заполнениеЗаявки(d).Show();
        }

        private void отредактироватьЗаявкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.OpenForms["заявкиКлиентов"].Dispose();
            new редактированиеКЛИЕНТ(d).Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.OpenForms["заявкиКлиентов"].Dispose();
            new редкатированиеМЕХАНИК(d).Show();
        }
    }
}
