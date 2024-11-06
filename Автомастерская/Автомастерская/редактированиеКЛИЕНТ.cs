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
    public partial class редактированиеКЛИЕНТ : Form
    {
        private const string name = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";
        int clientId;
        public редактированиеКЛИЕНТ(int clientId)
        {
            this.clientId = clientId;
            InitializeComponent();
            LoadClientRequests();
            LoadRequestIds();
            LoadCarTypes();         // Загружаем все типы машин
            LoadCarModels();        // Загружаем все модели машин
            LoadProblemDescriptions();

        }
        private void LoadCarTypes()
        {
            string query = "SELECT car_type FROM car_type";

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string carType = reader["car_type"].ToString();
                    if (!comboBox2.Items.Contains(carType)) // Проверка на дублирование
                    {
                        comboBox2.Items.Add(carType);
                    }
                }
                reader.Close();
            }

            // Включаем автозавершение
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void LoadCarModels()
        {
            string query = "SELECT model FROM car_model";

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string model = reader["model"].ToString();
                    if (!comboBox3.Items.Contains(model)) // Проверка на дублирование
                    {
                        comboBox3.Items.Add(model);
                    }
                }
                reader.Close();
            }

            // Включаем автозавершение
            comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void LoadProblemDescriptions()
        {
            string query = "SELECT problem_descryption FROM problem_descryption";

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string problem = reader["problem_descryption"].ToString();
                    if (!comboBox4.Items.Contains(problem)) // Проверка на дублирование
                    {
                        comboBox4.Items.Add(problem);
                    }
                }
                reader.Close();
            }

            // Включаем автозавершение
            comboBox4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox4.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        private void LoadRequestIds()
        {
            string query = "SELECT id_request FROM request WHERE client_id = @clientId";

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["id_request"].ToString());
                }
                reader.Close();
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
                cmd.Parameters.AddWithValue("@userId", clientId);
                userType = Convert.ToInt32(cmd.ExecuteScalar());
            }
            string query;
            if (userType == 4) // Предполагается, что 1 — это клиент
            {
                query = "SELECT r.id_request,r.start_date, cm.model, pd.problem_descryption, rs.status_name " +
                        "FROM request r " +
                        "LEFT JOIN car_model cm ON r.car_model = cm.id_car_model " +
                        "LEFT JOIN problem_descryption pd ON r.id_problem_descryption = pd.id_problem_descryption " +
                        "LEFT JOIN request_status rs ON r.id_request_status = rs.id_request_status " +
                        "WHERE r.client_id = @clientId";
            }
            else if (userType == 2) // Предполагается, что 2 — это механик
            {
                query = "SELECT r.start_date, cm.model, pd.problem_descryption, rs.status_name " +
                        "FROM request r " +
                        "JOIN car_model cm ON r.car_model = cm.id_car_model " +
                        "JOIN problem_descryption pd ON r.id_problem_descryption = pd.id_problem_descryption " +
                        "JOIN request_status rs ON r.id_request_status = rs.id_request_status " +
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
                command.Parameters.AddWithValue("@clientId", clientId);
                command.Parameters.AddWithValue("@masterId", clientId);// Используем ID клиента
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // Привязываем данные к DataGridView
            }
        }


        private void редактированиеКЛИЕНТ_Load(object sender, EventArgs e)
        {
            SqlConnection MyConnection = new SqlConnection(name);
            MyConnection.Open();
            string sql = $"SELECT user_type from users where id_user=\'{clientId}\'";
            SqlCommand cmd1 = new SqlCommand(sql, MyConnection);
            string k1 = cmd1.ExecuteScalar().ToString();
            MyConnection.Close();

            LoadClientRequests();
        }


        private void редактированиеКЛИЕНТ_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Application.Exit();
            }
        }

        private void label6_Click_1(object sender, EventArgs e)
        {
            Application.OpenForms["редактированиеКЛИЕНТ"].Dispose();
            new заявкиКлиентов(clientId).Show();
        }

        private void редактированиеКЛИЕНТ_Load_1(object sender, EventArgs e)
        {
            SqlConnection MyConnection = new SqlConnection(name);
            MyConnection.Open();
            string sql = $"SELECT user_type from users where id_user=\'{clientId}\'";
            SqlCommand cmd1 = new SqlCommand(sql, MyConnection);
            string k1 = cmd1.ExecuteScalar().ToString();
            MyConnection.Close();

            LoadClientRequests();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                // Проверяем, выбран ли ID заявки в первом ComboBox
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Выберите ID заявки.");
                    return;
                }

                int selectedRequestId = int.Parse(comboBox1.SelectedItem.ToString());

                string query = @"SELECT cr.car_type, cm.model, pd.problem_descryption, c.message
      FROM request r
      LEFT JOIN car_model cm ON r.car_model = cm.id_car_model
      LEFT JOIN car_type cr ON cm.id_car_type = cr.id_car_type
      LEFT JOIN problem_descryption pd ON r.id_problem_descryption = pd.id_problem_descryption
      LEFT JOIN comments c ON r.id_request = c.id_request
      WHERE r.id_request = @requestId";

                using (SqlConnection connection = new SqlConnection(name))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@requestId", selectedRequestId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Присваиваем значения другим ComboBox-ам
                        comboBox2.Text = reader["car_type"].ToString();       // Тип машины
                        comboBox3.Text = reader["model"].ToString();          // Модель машины
                        comboBox4.Text = reader["problem_descryption"].ToString(); // Проблема
                    }
                    else
                    {
                        MessageBox.Show("Данные по выбранной заявке не найдены.");
                    }

                    reader.Close();
                }
            }
        }
        private int GetCarTypeId(string carType)
        {
            string query = "SELECT id_car_type FROM car_type WHERE car_type = @carType";
            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@carType", carType);
                connection.Open();

                object result = command.ExecuteScalar();
                return result != null ? (int)result : -1; // Возвращаем -1, если не найден
            }
        }

        private int GetCarModelId(string model)
        {
            string query = "SELECT id_car_model FROM car_model WHERE model = @model";
            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@model", model);
                connection.Open();

                object result = command.ExecuteScalar();
                return result != null ? (int)result : -1; // Возвращаем -1, если не найден
            }
        }

        private int GetProblemId(string problem)
        {
            string query = "SELECT id_problem_descryption FROM problem_descryption WHERE problem_descryption = @problem";
            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@problem", problem);
                connection.Open();

                object result = command.ExecuteScalar();
                return result != null ? (int)result : -1; // Возвращаем -1, если не найден
            }
        }
        private void UpdateRequest(int requestId, string carType, string model, string problem)
        {
            // Получаем ID типа машины, модели и проблемы
            int carTypeId = GetCarTypeId(carType);
            int modelId = GetCarModelId(model);
            int problemId = GetProblemId(problem);

            if (carTypeId == -1 || modelId == -1 || problemId == -1)
            {
                MessageBox.Show("Некорректные данные. Пожалуйста, проверьте значения.");
                return;
            }

            // Обновляем данные в таблице request
            string updateRequestQuery = @"UPDATE request 
                                   SET car_model = @modelId, 
                                       id_problem_descryption = @problemId 
                                   WHERE id_request = @requestId";

            using (SqlConnection connection = new SqlConnection(name))
            {
                SqlCommand command = new SqlCommand(updateRequestQuery, connection);
                command.Parameters.AddWithValue("@modelId", modelId);
                command.Parameters.AddWithValue("@problemId", problemId);
                command.Parameters.AddWithValue("@requestId", requestId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Теперь обновляем тип машины в таблице car_model, если он изменился
                    string updateCarModelQuery = @"UPDATE car_model 
                                            SET id_car_type = @carTypeId 
                                            WHERE id_car_model = @modelId";

                    SqlCommand updateModelCommand = new SqlCommand(updateCarModelQuery, connection);
                    updateModelCommand.Parameters.AddWithValue("@carTypeId", carTypeId);
                    updateModelCommand.Parameters.AddWithValue("@modelId", modelId);

                    int modelRowsAffected = updateModelCommand.ExecuteNonQuery();

                    if (modelRowsAffected > 0)
                    {
                        MessageBox.Show("Данные успешно обновлены.");
                        RefreshDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить тип машины в таблице моделей.");
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось обновить данные в заявке. Проверьте правильность выбранных значений.");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите ID заявки для редактирования.");
                return;
            }

            int selectedRequestId = int.Parse(comboBox1.SelectedItem.ToString());

            // Получаем значения из ComboBox
            string carType = comboBox2.Text; // Тип машины
            string model = comboBox3.Text;    // Модель машины
            string problem = comboBox4.Text;   // Проблема

            // Вызываем метод обновления
            UpdateRequest(selectedRequestId, carType, model, problem);
        }
        private void RefreshDataGridView()
        {
            LoadClientRequests(); // Загрузка данных заново в DataGridView
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
