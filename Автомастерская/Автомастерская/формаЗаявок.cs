using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Автомастерская
{
    public partial class формаЗаявок : Form
    {
       
        private const string name = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";
        private DataTable заявкиTable;
        private int totalRecords;
        int d;

        public формаЗаявок(int d1)
        {
            InitializeComponent();
            d = d1;

            LoadData();
            txtFilter.TextChanged += txtFilter_TextChanged;
        }
        private void LoadData()
        {
            string query = @"
                SELECT 
    request.start_date AS [Дата начала], 
    car_type.car_type AS [Тип машины], 
    car_model.model AS [Модель машины], 
    problem_descryption.problem_descryption AS [Проблема], 
    request_status.status_name AS [Статус], 
    request.completion_date AS [Дата окончания], 
    parts.parts AS [Запчасть], 
    mechanic.fio AS [Механик], 
    client.fio AS [Клиент],
 comments.message AS [Комментарий]
FROM request
LEFT JOIN car_model ON request.car_model = car_model.id_car_model
LEFT JOIN car_type ON car_model.id_car_type = car_type.id_car_type
LEFT JOIN problem_descryption ON request.id_problem_descryption = problem_descryption.id_problem_descryption
LEFT JOIN request_status ON request.id_request_status = request_status.id_request_status
LEFT JOIN parts ON request.id_parts = parts.id_parts
LEFT JOIN users AS mechanic ON request.master_id = mechanic.id_user
LEFT JOIN users AS client ON request.client_id = client.id_user
LEFT JOIN comments ON request.id_request = comments.id_request";

            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 14);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 14, System.Drawing.FontStyle.Bold);
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.ScrollBars = ScrollBars. Horizontal;

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                заявкиTable = new DataTable();
                adapter.Fill(заявкиTable);
                totalRecords = заявкиTable.Rows.Count;
                dataGridView1.DataSource = заявкиTable;
                UpdateRecordCount();
            }
        }
        private void UpdateRecordCount()
        {
            labelRecordCount.Text = $"{dataGridView1.Rows.Count} из {totalRecords}";
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }
        private void FilterData()
        {
            string filter = "";
            string[] keywords = txtFilter.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var keyword in keywords)
            {
                if (filter.Length > 0)
                {
                    filter += " AND "; 

                }
                DateTime dateValue;
                if (DateTime.TryParse(keyword, out dateValue))
                {
                    filter += $"[Дата начала] = '{dateValue:yyyy-MM-dd}' OR " +
                              $"[Дата окончания] = '{dateValue:yyyy-MM-dd}'"; 
                }
                else
                { 

                    filter += $"[Тип машины] LIKE '%{keyword}%' OR " +
                        $"[Модель машины] LIKE '%{keyword}%' OR " +
                          $"[Статус] LIKE '%{keyword}%' OR " +
                          $"[Проблема] LIKE '%{keyword}%' OR " +
                          $"[Запчасть] LIKE '%{keyword}%' OR " +
                          $"[Механик] LIKE '%{keyword}%' OR " +
                          $"[Клиент] LIKE '%{keyword}%'";
                }
            }

            DataView dataView = new DataView(заявкиTable);
            dataView.RowFilter = filter;

            dataGridView1.DataSource = dataView;
            UpdateRecordCount();
        }


        private void label2_Click(object sender, EventArgs e)
        {
            Application.OpenForms["формаЗаявок"].Dispose();
            new окно_с_инф(d).Show();
        }

        private void формаЗаявок_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Application.Exit();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.OpenForms["формаЗаявок"].Dispose();
            new регистрацияЗаявок(d).Show();
        }
    }
}
