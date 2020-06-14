using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticeTask8
{
    public partial class Form1 : Form
    {
        int[,] matrix;
        int size;
        TextBox[,] Matrix_Cells;
        Label component_label = null;
        int current_index = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Focus();
            SizeChoice.Visible = false;
            InputSize.Visible = false;
            InputLabel.Visible = false;
        }
        string[] Read_FromFile() // Чтение из файла
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открытие текстового файла";
            openFileDialog1.Filter = "Текстовые файлы|*.txt";
            openFileDialog1.InitialDirectory = "";
            string[] filelines = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                filelines = File.ReadAllLines(filename);
            }
            return filelines;
        }
        void Generate_Visual_Matrix(bool is_from_file) // Вывод матрицы на экран
        {
            Matrix_Cells = new TextBox[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Matrix_Cells[i, j] = new TextBox() // Каждый элемент матрицы представляется текстбоксом
                    {
                        Size = new Size(39, 34),
                        Location = new Point(19 + (39 + 5) * j, (137 + (34 + 5) * i)),
                        Name = "TextBox" + (i * size + j).ToString(),

                    };
                    if (is_from_file)
                    {
                        Matrix_Cells[i, j].Text = matrix[i, j].ToString();
                        Matrix_Cells[i, j].ReadOnly = true;
                    }
                    Controls.Add(Matrix_Cells[i, j]);
                    Matrix_Cells[i, j].KeyDown += new KeyEventHandler(this.KeyDown);
                    Matrix_Cells[i, j].Leave += new EventHandler(this.Leave);
                }
            }
            Controls.Find(Matrix_Cells[0, 0].Name, false)[0].Focus();
        }
        void Calculate_Components() // Подсчет компонент
        {
            List<List<int>> node_sets = new List<List<int>>(size); // Список списков, где i-тый список содержит номера вершин, с которыми соединена i-тая вершина
            int count_of_components = 0; // Количество компонент
            for(int i = 0; i < size; i++)
            {
                node_sets.Add(new List<int>());
                for(int j = 0; j < size; j++)
                {
                    if (matrix[i, j] == 1) // Между вершинами i и j есть ребро
                    {
                        node_sets[i].Add(j);
                    }
                }
            }
            if (node_sets.All(list => list.Count == 0)) count_of_components = size; // Если никакие вершины не соединены, то компонент столько же, сколько вершин
            else
            {
                string[] components_lines = new string[size]; // Строковое представление последовательности соединенных вершин
                for (int i = 0; i < size; i++)
                {
                    bool[] checked_nodes = new bool[size]; // Проверенные вершины
                    for (int j = 0; j < size; j++)
                    {
                        if (i != j && node_sets[i].Contains(j)) // Если j-тая вершина содержится в i-той и i не j
                        {
                            node_sets[i].AddRange(node_sets[j]); // Во все вершины j-того списка можно попасть из i-той вершины через j-тую
                            checked_nodes[j] = true; // Вершина проверена
                        }
                        else checked_nodes[j] = false;
                    }
                    node_sets[i] = node_sets[i].Distinct().OrderBy(elem => elem).ToList(); // Удаление повторов вершин и упорядочение
                    if (node_sets[i].Count == size) // i-тая вершина соединена со всеми
                    {
                        count_of_components = 1;
                        break;
                    }
                    else if (node_sets[i].Any(elem => (checked_nodes[elem] == false && elem != i))) // При добавлении вершин какая-то не была рассмотрена
                        --i;
                    else
                    {
                        components_lines[i] = string.Join(" ", node_sets[i]);
                    }
                }
                if (count_of_components != 1) count_of_components = components_lines.Distinct().Count(); // Количество компонент равно количеству уникальных последовательностей соединенных вершин
            }
            Print_Count(count_of_components); 
        }
        void Print_Count(int count_of_components) // Вывод количества вершин
        {
            component_label = new Label()
            {
                Text = "Количество компонент:" + count_of_components.ToString(),
                Font = new Font(InputLabel.Font.FontFamily, InputLabel.Font.Size, InputLabel.Font.Style),
                Size = new Size(Text.ToCharArray().Length * 50, 35),
                Location = new Point(Matrix_Cells[size - 1, 0].Location.X, Matrix_Cells[size - 1, 0].Location.Y + Matrix_Cells[size - 1, 0].Height + 20),
                Name = "Component_Label",
                BackColor = Color.Transparent,
                ForeColor = Color.Black
            };
            Controls.Add(component_label);
        }
        new void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Нажат энтер
            {
                Cell_Check(sender); // Проверка корректного ввода
                e.SuppressKeyPress = true;
            }
        }
        new void Leave(object sender, EventArgs e) 
        {
            Remove_Component_Label();
            bool found_textbox = false;
            for(int i = 0; i < size * size && !found_textbox; i++)
            {
                if (ActiveControl.Name == "TextBox" + i.ToString()) found_textbox = true; // Поиск активного текстбокса
            }
            if (found_textbox) // Если активен текстбокс матрицы
            {
                if (current_index < size * size) // Индекс текущего текстбокса не превышает индекс последнего элемента матрицы
                {
                    if (int.TryParse((sender as TextBox).Text, out matrix[current_index / size, current_index % size])) // Парс текста текстбокса в элемент матрицы
                    {
                        if (matrix[current_index / size, current_index % size] == 0 || matrix[current_index / size, current_index % size] == 1) // Если введено не 0 и не 1
                        {
                            current_index = int.Parse(string.Join(string.Empty, (ActiveControl as TextBox).Name.Where(ch => char.IsDigit(ch)).ToArray()));
                        }
                        else MessageBox.Show("Вводимое число должно быть 0 или 1!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Некорректный ввод числа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int index = int.Parse(string.Join(string.Empty, (sender as TextBox).Name.Where(ch => char.IsDigit(ch)).ToArray())); // Получение индекса активного текстбокса
                    if (int.TryParse((sender as TextBox).Text, out matrix[index / size, index % size])) // Парс текста текстбокса в элемент матрицы
                    {
                        if (matrix[index / size, index % size] != 0 && matrix[index / size, index % size] != 1) // Если введено не 0 и не 1
                        {
                            MessageBox.Show("Вводимое число должно быть 0 или 1!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        void Cell_Check(object sender)
        {
            Remove_Component_Label(); // Очистка результатов
            if (current_index < size * size) // Если текущий индекс не превосходит индекс последнего элемента матрицы
            {
                if (int.TryParse(Controls.Find("TextBox" + current_index.ToString(), false)[0].Text, out matrix[current_index / size, current_index % size])) // Парс текста текстбокса в элемент матрицы
                {
                    if (matrix[current_index / size, current_index % size] == 0 || matrix[current_index / size, current_index % size] == 1) // Если введено 0 или 1
                    {
                        current_index++; // Текущий индекс увеличивается
                        string next_name = "TextBox" + current_index.ToString();
                        if (Controls.ContainsKey(next_name)) // После текущего текстбокса есть еще текстбоксы
                        {
                            Controls.Find(next_name, false)[0].Focus(); // Переключение на следующий текстбокс
                        }
                        else
                        {
                            bool is_correct = Matrix_Check(is_from_file: false); // Корректное заполнение матрицы
                            if (is_correct)
                            {
                                Calculate_Components(); // Подсчет количества компонент

                            }
                            else MessageBox.Show("Матрица смежности задана неверно! Проверьте симметричность и нули на главной диагонали", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else MessageBox.Show("Вводимое число должно быть 0 или 1!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("Некорректный ввод числа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int index = int.Parse(string.Join(string.Empty, (sender as TextBox).Name.Where(ch => char.IsDigit(ch)).ToArray())); // Определение индекса текущего текстбокса
                if (int.TryParse((sender as TextBox).Text, out matrix[index / size, index % size])) // Парс текста текстбокса в элемент матрицы
                {
                    if (matrix[index / size, index % size] == 0 || matrix[index / size, index % size] == 1) // Если введено 0 или 1
                    {
                        bool is_correct = Matrix_Check(is_from_file: false); // Корректное заполнение матрицы
                        if (is_correct)
                        {
                            Calculate_Components(); // Подсчет количества компонент
                        }
                        else MessageBox.Show("Матрица смежности задана неверно! Проверьте симметричность и нули на главной диагонали", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else MessageBox.Show("Некорректный ввод числа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void Remove_Visual_Matrix() // Удаление матрицы с экрана
        {
            if (Matrix_Cells != null) // Если еще не удалена
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Controls.Remove(Matrix_Cells[i, j]);
                    }
                }
                Matrix_Cells = null; // Обнуление
                matrix = null;
                current_index = 0;
            }
        }
        public void Convert_From_File([Optional] string path) // Конвертация данных из файла в матрицу
        {
            string[] lines;
            if (path == string.Empty) lines = Read_FromFile(); // Считывание строк из файла
            else lines = File.ReadAllLines(path); // Считывание для тестов
            if (lines != null)
            {
                size = lines.Length; // Количество строк
                if (size <= 20 && size >= 1) // Количество строк от 1 до 20
                {
                    matrix = new int[size, size];
                    bool is_correct = true;
                    for (int i = 0; i < lines.Length && is_correct; i++)
                    {
                        lines[i] = string.Join("", lines[i].Split(',')); // Удаление запятых из строки
                        string tab = "\t";
                        string[] value_lines = lines[i].Split(' ',tab.ToCharArray()[0]); // Разбиение на подстроки
                        if (value_lines.Length != size) is_correct = false; // Количество подстрок (чисел) больше, чем количество строк, матрица не квадратная
                        for (int j = 0; j < size && is_correct; j++)
                        {
                            is_correct = (int.TryParse(value_lines[j], out matrix[i, j]) & (value_lines[j] == "0" || value_lines[j] == "1")); // Парс в элемент матрицы
                        }
                    }
                    if (!is_correct) // Некорректный ввод
                    {
                        matrix = null; // Обнуление
                        size = 0;
                        MessageBox.Show("Файл содержит некорректные данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        is_correct = Matrix_Check(is_from_file: true); // Корректность матрицы
                        if (is_correct) // Матрица корректна
                        {
                            InputLabel.Text = "Матрица из файла: ";
                            InputLabel.Visible = true;
                            Generate_Visual_Matrix(true); // Вывод матрицы на экран
                            Calculate_Components(); // Подсчет количества компонент
                        }
                        else MessageBox.Show("Матрица смежности задана неверно! Проверьте симметричность и нули на главной диагонали", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Размер матрицы должен быть от 1 до 20!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        bool Matrix_Check(bool is_from_file) // Проверка матрицы
        {
            bool is_correct = true;
            for (int i = 0; i < size && is_correct; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] != matrix[j, i]) // Проверка симметричности относительно главной диагонали
                    {
                        is_correct = false; // Если несимметрична, то ввод некорректный
                        if (!is_from_file) Controls.Find("TextBox" + (i * size + j).ToString(), false)[0].Focus();
                    }
                    else if (i == j && matrix[i, j] != 0) // Проверка, есть ли 1 на главной диагонали
                    {
                        is_correct = false; // Если есть, то ввод некорректный
                        if (!is_from_file) Controls.Find("TextBox" + (i * size + j).ToString(), false)[0].Focus();

                    }
                }
            }
            return is_correct;

        }
        void Remove_Component_Label() // Очистка результатов
        {
            if (component_label != null) // Если еще не удалено
            {
                Controls.Remove(component_label);
                component_label = null;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // Выбор пункта меню
        {
            InputLabel.Visible = false;
            if (comboBox1.SelectedItem.ToString() == "Ввод из файла")
            {
                // Очистка формы
                Remove_Visual_Matrix();
                Remove_Component_Label();
                SizeChoice.Visible = false;
                InputSize.Visible = false;
                InputLabel.Visible = false;
                Convert_From_File(); // Считывание из файла
            }
            else if (comboBox1.SelectedItem.ToString() == "Ввод вручную")
            {
                //Очистка формы
                Remove_Visual_Matrix();
                Remove_Component_Label();
                SizeChoice.Visible = true;
                InputSize.Visible = true;
                InputSize.Focus(); // Ввод размера матрицы
            }
            else if (comboBox1.SelectedItem.ToString() == "Генерация данных")
            {
                TestGenerator.Generate_Tests(); // Генерация тестов
                MessageBox.Show("Файлы входных данных сгенерированы", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void InputSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Нажат энтер
            {
                Size_Check(); // Проверка корректности введенного размера
                e.SuppressKeyPress = true;
            }

        }
        void Size_Check()
        {
            // Очистка матрицы и результатов
            Remove_Visual_Matrix();
            Remove_Component_Label();
            if (int.TryParse(InputSize.Text, out size) && size >= 1 && size <= 20) // Введено целое число от 1 до 20
            {
                InputLabel.Text = "Введите данные в таблицу:";
                InputLabel.Visible = true;
                matrix = new int[size, size];
                Generate_Visual_Matrix(false); // Вывод текстбоксов матрицы для заполнения
            }
            else MessageBox.Show("Введите натуральное число от 1 до 20!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}