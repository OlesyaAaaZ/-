using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Автомастерская
{
    public partial class заполнениеЗаявки : Form
    {
        int clientId;
        private const string connectionString = @"Data Source=ADCLG1;Initial Catalog=УП_419/1;Integrated Security=True";

        public заполнениеЗаявки(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId;
            LoadComboBoxData();
            SetupAutoComplete();
        }

        private void LoadComboBoxData()
        {
            LoadCarTypes();
            LoadCarModels();
            LoadProblems();
        }

        public class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }

            public override string ToString() => Text; // Это позволит отображать текст в комбобоксе
        }

        private void LoadCarTypes()
        {
            string query = "SELECT id_car_type, car_type FROM car_type";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxCarType.Items.Add(new ComboBoxItem
                    {
                        Value = (int)reader["id_car_type"],
                        Text = reader["car_type"].ToString()
                    });
                }
                reader.Close();
            }
        }

        private void LoadCarModels()
        {
            string query = "SELECT id_car_model, model FROM car_model";
            HashSet<string> uniqueCarModels = new HashSet<string>(); // Используем HashSet для хранения уникальных названий

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string model = reader["model"].ToString();
                    if (uniqueCarModels.Add(model)) // Добавляем только если элемент уникален
                    {
                        comboBoxCarModel.Items.Add(new ComboBoxItem
                        {
                            Value = (int)reader["id_car_model"],
                            Text = model
                        });
                    }
                }
                reader.Close();
            }
        }

        private void LoadProblems()
        {
            string query = "SELECT id_problem_descryption, problem_descryption FROM problem_descryption";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxProblem.Items.Add(new ComboBoxItem
                    {
                        Value = (int)reader["id_problem_descryption"],
                        Text = reader["problem_descryption"].ToString()
                    });
                }
                reader.Close();
            }
        }

        private void AddNewRequest()
        {
            // Проверяем, что выбраны все необходимые значения
            if (comboBoxCarModel.SelectedItem == null || comboBoxProblem.SelectedItem == null || comboBoxCarType.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите тип машины, модель автомобиля и описание проблемы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Выход из метода, если не выбраны необходимые параметры
            }

            var selectedCarType = (ComboBoxItem)comboBoxCarType.SelectedItem;
            var selectedCarModel = (ComboBoxItem)comboBoxCarModel.SelectedItem;
            var selectedProblem = (ComboBoxItem)comboBoxProblem.SelectedItem;

            int carModelId = GetOrCreateCarModel(selectedCarType.Value, selectedCarModel.Text);

            if (carModelId == -1)
            {
                MessageBox.Show("Не удалось сохранить модель автомобиля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Запрос для добавления заявки
            string query = "INSERT INTO request (client_id, car_model, id_problem_descryption, start_date,id_request_status ) " +
                           "VALUES (@clientId, @carModel, @problem, @startDate,@requestStatus)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@clientId", clientId); // Используем сохраненный ID клиента
                command.Parameters.AddWithValue("@carModel", carModelId); // Используем ID модели
                command.Parameters.AddWithValue("@problem", selectedProblem.Value); // Используем ID проблемы
                command.Parameters.AddWithValue("@startDate", DateTime.Now); // Устанавливаем текущую дату как дату начала

               command.Parameters.AddWithValue("@requestStatus", 3); // Например, ID статуса заявки (нужно определить)
              

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private int GetOrCreateCarModel(int carTypeId, string modelName)
        {
            // Проверяем, существует ли модель
            string checkModelQuery = "SELECT id_car_model FROM car_model WHERE model = @modelName AND id_car_type = @carTypeId";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(checkModelQuery, connection);
                command.Parameters.AddWithValue("@modelName", modelName);
                command.Parameters.AddWithValue("@carTypeId", carTypeId);
                connection.Open();

                var result = command.ExecuteScalar();
                if (result != null)
                {
                    return (int)result; // Модель уже существует, возвращаем ее ID
                }
            }

            // Если модель не существует, добавляем ее
            string insertModelQuery = "INSERT INTO car_model (model, id_car_type) VALUES (@modelName, @carTypeId); SELECT SCOPE_IDENTITY();";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertModelQuery, connection);
                command.Parameters.AddWithValue("@modelName", modelName);
                command.Parameters.AddWithValue("@carTypeId", carTypeId);
                connection.Open();

                try
                {
                    return Convert.ToInt32(command.ExecuteScalar()); // Возвращаем ID новой модели
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении модели: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Указываем, что произошла ошибка
                }
            }
        }

        private void SetupAutoComplete()
        {
            // Настройка автозаполнения для комбобоксов
            comboBoxCarType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxCarType.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboBoxCarModel.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxCarModel.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboBoxProblem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxProblem.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void просмотрЗаявокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Здесь можно добавить код для перехода к форме просмотра заявок
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewRequest();
                MessageBox.Show("Заявка успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // this.Close(); // Закрываем форму после добавления заявки, если необходимо
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении заявки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.OpenForms["заполнениеЗаявки"].Dispose();
            new заявкиКлиентов(clientId).Show();
        }

        private void заполнениеЗаявки_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Application.Exit();
            }
        }

        private void заполнениеЗаявки_Load(object sender, EventArgs e)
        {

        }
    }
}
