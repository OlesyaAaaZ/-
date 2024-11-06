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

namespace Автомастерская
{
    public partial class регистрацияЗаявок : Form
    {
      
        private const string name = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";
        private DataTable заявкиTable;
        private int totalRecords;
        int d;
        public регистрацияЗаявок(int d1)
        {
            InitializeComponent();
            PopulateRequestIDComboBox();
            PopulateMechanicsComboBox();
            d = d1;
        }

       

        private void label2_Click_1(object sender, EventArgs e)
        {
            Application.OpenForms["регистрацияЗаявок"].Dispose();
            new формаЗаявок(d).Show();
        }

        private void регистрацияЗаявок_Load(object sender, EventArgs e)
        {
            string query = @"
                SELECT 
request.id_request AS [ID],
    request.start_date AS [Дата начала], 
    car_type.car_type AS [Тип машины], 
    car_model.model AS [Модель машины], 
    problem_descryption.problem_descryption AS [Проблема], 
    request_status.status_name AS [Статус], 
    request.completion_date AS [Дата окончания], 
    parts.parts AS [Запчасть], 
    mechanic.fio AS [Механик], 
    client.fio AS [Клиент]
FROM request
LEFT JOIN car_model ON request.car_model = car_model.id_car_model
LEFT JOIN car_type ON car_model.id_car_type = car_type.id_car_type
LEFT JOIN problem_descryption ON request.id_problem_descryption = problem_descryption.id_problem_descryption
LEFT JOIN request_status ON request.id_request_status = request_status.id_request_status
LEFT JOIN parts ON request.id_parts = parts.id_parts
LEFT JOIN users AS mechanic ON request.master_id = mechanic.id_user
LEFT JOIN users AS client ON request.client_id = client.id_user";

            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 14);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 14, System.Drawing.FontStyle.Bold);
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.ScrollBars = ScrollBars.Horizontal;

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                заявкиTable = new DataTable();
                adapter.Fill(заявкиTable);
                totalRecords = заявкиTable.Rows.Count;
                dataGridView1.DataSource = заявкиTable;
              
            }
        }
        private void PopulateRequestIDComboBox()
        {
            using (SqlConnection connection = new SqlConnection(name))
            {
                string query = "SELECT id_request FROM request";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBoxRequestID.DataSource = dt;
                comboBoxRequestID.DisplayMember = "id_request";
                comboBoxRequestID.ValueMember = "id_request";
            }
        }
        private void RegisterRequest()
        {
            if (comboBoxRequestID.SelectedValue == null)
            {
                MessageBox.Show("Выберите ID заявки.");
                return;
            }

            if (comboBoxMechanics.SelectedValue == null)
            {
                MessageBox.Show("Выберите мастера для заявки.");
                return;
            }

         
            if (!int.TryParse(comboBoxRequestID.SelectedValue.ToString(), out int requestId))
            {
                MessageBox.Show("Неверный формат ID заявки.");
                return;
            }

            if (!int.TryParse(comboBoxMechanics.SelectedValue.ToString(), out int mechanicId))
            {
                MessageBox.Show("Неверный формат ID мастера.");
                return;
            }

            string query = "UPDATE request SET master_id = @mechanicId WHERE id_request = @requestId";

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@mechanicId", mechanicId);
                command.Parameters.AddWithValue("@requestId", requestId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Мастер успешно добавлен к заявке.");
                        RefreshDataGridView(); 
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить заявку. Проверьте данные.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении мастера: " + ex.Message);
                }
            }
        }

        private void RefreshDataGridView()
        {
            string query = @"
        SELECT 
            r.id_request AS [ID заявки], 
            r.start_date AS [Дата начала], 
            cm.model AS [Модель машины], 
            pd.problem_descryption AS [Описание проблемы], 
            rs.status_name AS [Статус], 
            m.fio AS [ФИО мастера]
        FROM request r
        LEFT JOIN car_model cm ON r.car_model = cm.id_car_model
        LEFT JOIN problem_descryption pd ON r.id_problem_descryption = pd.id_problem_descryption
        LEFT JOIN request_status rs ON r.id_request_status = rs.id_request_status
        LEFT JOIN users m ON r.master_id = m.id_user";

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable); 
                dataGridView1.DataSource = dataTable; 
            }
        }
        private void PopulateMechanicsComboBox()
        {
            using (SqlConnection connection = new SqlConnection(name))
            {
                string query = "SELECT CAST(id_user AS VARCHAR) AS id_user, fio FROM users WHERE user_type = 2"; 

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBoxMechanics.DataSource = dt;
                comboBoxMechanics.DisplayMember = "fio";
                comboBoxMechanics.ValueMember = "id_user";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterRequest();
        }
    }
}
