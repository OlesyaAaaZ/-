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
using System.Windows.Forms.VisualStyles;

namespace Автомастерская
{
    public partial class редкатированиеМЕХАНИК : Form
    {

        private const string name = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";
        int d;
        public редкатированиеМЕХАНИК(int d1)
        {
            InitializeComponent();
            d = d1;
            LoadClientRequests();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            using (SqlConnection connection = new SqlConnection(name))
            {
                connection.Open();

                string requestIdQuery = "SELECT id_request FROM request WHERE master_id = @masterId";
                SqlDataAdapter requestIdAdapter = new SqlDataAdapter(requestIdQuery, connection);
                SqlCommand requestIdCommand = new SqlCommand(requestIdQuery, connection);
                requestIdCommand.Parameters.AddWithValue("@masterId", d); // Используем ID текущего механика
                requestIdAdapter.SelectCommand = requestIdCommand;

                DataTable requestIdTable = new DataTable();
                requestIdAdapter.Fill(requestIdTable);
                comboBox1.DataSource = requestIdTable;
                comboBox1.DisplayMember = "id_request";
                comboBox1.ValueMember = "id_request";

                // Загрузка статусов
                string statusQuery = "SELECT status_name FROM request_status";
                SqlDataAdapter statusAdapter = new SqlDataAdapter(statusQuery, connection);
                DataTable statusTable = new DataTable();
                statusAdapter.Fill(statusTable);
                comboBox2.DataSource = statusTable;
                comboBox2.DisplayMember = "status_name";
                comboBox2.ValueMember = "status_name";

               

                // Загрузка запчастей
                string partsQuery = "SELECT parts FROM parts";
                SqlDataAdapter partsAdapter = new SqlDataAdapter(partsQuery, connection);
                DataTable partsTable = new DataTable();
                partsAdapter.Fill(partsTable);
                comboBox4.DataSource = partsTable;
                comboBox4.DisplayMember = "parts";
                comboBox4.ValueMember = "parts";
            }
        }
        private void LoadClientRequests()
        {
            int userType;
            using (SqlConnection connection = new SqlConnection(name))
            {
                connection.Open();
                string sql = "SELECT user_type FROM users WHERE id_user = @userId";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@userId", d);
                userType = Convert.ToInt32(cmd.ExecuteScalar());
            }
            string query;
            if (userType == 4) // Предполагается, что 1 — это клиент
            {
                query = "SELECT r.start_date, cm.model, pd.problem_descryption,r.completion_date, rs.status_name, c.message " +
                        "FROM request r " +
                        "LEFT JOIN car_model cm ON r.car_model = cm.id_car_model " +
                        "LEFT JOIN problem_descryption pd ON r.id_problem_descryption = pd.id_problem_descryption " +
                        "LEFT JOIN request_status rs ON r.id_request_status = rs.id_request_status " +
                        "LEFT JOIN comments c ON r.id_request = c.id_request " +
                        "WHERE r.client_id = @clientId";

                
            }
            else if (userType == 2) // Предполагается, что 2 — это механик
            {
                query = "SELECT r.id_request, r.start_date, cm.model, pd.problem_descryption,r.completion_date, rs.status_name, c.message, p.parts  " +
                        "FROM request r " +
                        "JOIN car_model cm ON r.car_model = cm.id_car_model " +
                        "JOIN problem_descryption pd ON r.id_problem_descryption = pd.id_problem_descryption " +
                        "JOIN request_status rs ON r.id_request_status = rs.id_request_status " +
                          "LEFT JOIN comments c ON r.id_request = c.id_request " +
                           "LEFT JOIN parts p ON r.id_parts = p.id_parts " +
                        "WHERE r.master_id = @masterId"; // Извлекаем заявки для механика

               
            }
            else
            {
                // Неизвестный тип пользователя, выход из метода
                return;
            }
            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", d);
                command.Parameters.AddWithValue("@masterId", d);// Используем ID клиента
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // Привязываем данные к DataGridView
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.OpenForms["редкатированиеМЕХАНИК"].Dispose();
            new заявкиКлиентов(d).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(name))
            {
                connection.Open();

                // Получаем данные из ComboBox и TextBox
                int selectedRequestId = (int)comboBox1.SelectedValue;
                string selectedStatus = comboBox2.SelectedValue.ToString();
                string selectedPart = comboBox4.SelectedValue.ToString();
                string comment = textBox1.Text;

                // Проверяем, существует ли запись с данным ID
                string checkQuery = "SELECT COUNT(*) FROM request WHERE id_request = @requestId";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@requestId", selectedRequestId);
                int exists = (int)checkCommand.ExecuteScalar();

                if (exists > 0)
                {
                    // Подготовка для обновления
                    string updateQuery = @"
            UPDATE request 
            SET id_request_status = (SELECT id_request_status FROM request_status WHERE status_name = @status),
                id_parts = (SELECT id_parts FROM parts WHERE parts = @parts),
                completion_date = CASE 
                    WHEN @status = 'Готова к выдаче' THEN GETDATE() 
                    WHEN @status IS NULL THEN NULL
                    ELSE completion_date 
                END
            WHERE id_request = @requestId";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@status", selectedStatus);
                    updateCommand.Parameters.AddWithValue("@parts", selectedPart);
                    updateCommand.Parameters.AddWithValue("@requestId", selectedRequestId);
                    updateCommand.ExecuteNonQuery();

                    // Обновление комментария в таблице comments
                    string updateCommentQuery = @"
            UPDATE comments 
            SET message = @comment 
            WHERE id_request = @requestId"; // Убедитесь, что у вас есть логика для выбора правильного комментария

                    SqlCommand updateCommentCommand = new SqlCommand(updateCommentQuery, connection);
                    updateCommentCommand.Parameters.AddWithValue("@comment", comment);
                    updateCommentCommand.Parameters.AddWithValue("@requestId", selectedRequestId);
                    updateCommentCommand.ExecuteNonQuery();

                    MessageBox.Show("Запись обновлена и комментарий изменен!");

                    // Перезагрузка данных в DataGridView
                    LoadClientRequests(); // Перезагружаем данные
                }
                else
                {
                    MessageBox.Show("Запись не найдена.");
                }
            }
        }
    }

}
