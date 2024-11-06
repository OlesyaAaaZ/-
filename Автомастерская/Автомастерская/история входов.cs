using System;
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
    public partial class история_входов : Form
    {
        int d;
        private const string name = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";
        public история_входов(int d1)
        {
            InitializeComponent();
          
            d = d1;
        }


        private void LoadHistory(string filter = "")
        {
            using (SqlConnection connection = new SqlConnection(name))
            {
                connection.Open();
                string query = "SELECT AttemptTime,Login, IsSuccess FROM LoginHistory";


                if (!string.IsNullOrEmpty(filter))
                {
                    query += " WHERE Login LIKE @filter";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        command.Parameters.AddWithValue("@filter", "%" + filter + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["AttemptTime"].HeaderText = "Время попытки";
                    dataGridView1.Columns["Login"].HeaderText = "Логин";
                    dataGridView1.Columns["IsSuccess"].HeaderText = "Успешая/Ошибочная попытка";
                }
            }
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 14);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 14, System.Drawing.FontStyle.Bold);
            dataGridView1.ScrollBars = ScrollBars.Vertical;
        }


        private void label2_Click(object sender, EventArgs e)
        {
            Application.OpenForms["история_входов"].Dispose();
            new окно_с_инф(d).Show();
        }

        private void история_входов_Load(object sender, EventArgs e)
        {
            LoadHistory();
        }

        private void история_входов_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Application.Exit();
            }
        }

        private void buttonsort_Click_1(object sender, EventArgs e)
        {
            LoadHistory();
            DataTable dataTable = (DataTable)dataGridView1.DataSource;
            dataTable.DefaultView.Sort = "AttemptTime DESC";
            dataGridView1.DataSource = dataTable;
        }

        private void buttonfilter_Click(object sender, EventArgs e)
        {
            string filter = textBox1.Text.Trim();
            LoadHistory(filter);
        }
    }
}
