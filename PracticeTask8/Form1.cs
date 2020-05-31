using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
        string[] Read_FromFile()
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
        void Generate_Visual_Matrix(bool is_from_file)
        {
            Matrix_Cells = new TextBox[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Matrix_Cells[i, j] = new TextBox()
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
        void Calculate_Components()
        {
            string[] node_sets = new string[size];
            int count_of_components = 0;
            for(int i = 0; i < size; i++)
            {
                node_sets[i] = "";
                for(int j = 0; j < size; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        node_sets[i] += j.ToString();
                    }
                }
            }
            if (node_sets.All(str => str == "")) count_of_components = size;
            else
            {
                for (int i = 0; i < size; i++)
                {
                    bool[] checked_nodes = new bool[size];
                    for (int j = 0; j < size; j++)
                    {
                        if (i != j && node_sets[i].Contains(j.ToString()))
                        {
                            node_sets[i] += node_sets[j];
                            checked_nodes[j] = true;
                        }
                        else checked_nodes[j] = false;
                    }
                    node_sets[i] = string.Concat(node_sets[i].Distinct().OrderBy(ch => ch));
                    if (node_sets[i].Any(ch => (checked_nodes[(ch - '0')] == false && (ch - '0') != i)))
                        --i;
                }
                count_of_components = node_sets.Distinct().Count();
            }
            Print_Count(count_of_components); 
        }
        void Print_Count(int count_of_components)
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
            if (e.KeyCode == Keys.Enter)
            {
                Cell_Check(sender);
                e.SuppressKeyPress = true;
            }
        }
        new void Leave(object sender, EventArgs e)
        {
            Remove_Component_Label();
            bool found_textbox = false;
            for(int i = 0; i < size * size && !found_textbox; i++)
            {
                if (ActiveControl.Name == "TextBox" + i.ToString()) found_textbox = true;
            }
            if (found_textbox)
            {
                if (current_index < size * size)
                {
                    if (int.TryParse((sender as TextBox).Text, out matrix[current_index / size, current_index % size]))
                    {
                        if (matrix[current_index / size, current_index % size] == 0 || matrix[current_index / size, current_index % size] == 1)
                        {
                            current_index = int.Parse(string.Join(string.Empty, (ActiveControl as TextBox).Name.Where(ch => char.IsDigit(ch)).ToArray()));
                        }
                        else MessageBox.Show("Вводимое число должно быть 0 или 1!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show("Некорректный ввод числа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int index = int.Parse(string.Join(string.Empty, (sender as TextBox).Name.Where(ch => char.IsDigit(ch)).ToArray()));
                    if (int.TryParse((sender as TextBox).Text, out matrix[index / size, index % size]))
                    {
                        if (matrix[index / size, index % size] != 0 && matrix[index / size, index % size] != 1)
                        {
                            MessageBox.Show("Вводимое число должно быть 0 или 1!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        void Cell_Check(object sender)
        {
            Remove_Component_Label();
            if (current_index < size * size)
            {
                if (int.TryParse(Controls.Find("TextBox" + current_index.ToString(), false)[0].Text, out matrix[current_index / size, current_index % size]))
                {
                    if (matrix[current_index / size, current_index % size] == 0 || matrix[current_index / size, current_index % size] == 1)
                    {
                        current_index++;
                        string next_name = "TextBox" + current_index.ToString();
                        if (Controls.ContainsKey(next_name))
                        {
                            Controls.Find(next_name, false)[0].Focus();
                        }
                        else
                        {
                            bool is_correct = Matrix_Check(is_from_file: false);
                            if (is_correct)
                            {
                                Calculate_Components();

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
                int index = int.Parse(string.Join(string.Empty, (sender as TextBox).Name.Where(ch => char.IsDigit(ch)).ToArray()));
                if (int.TryParse((sender as TextBox).Text, out matrix[index / size, index % size]))
                {
                    if (matrix[index / size, index % size] == 0 || matrix[index / size, index % size] == 1)
                    {
                        bool is_correct = Matrix_Check(is_from_file: false);
                        if (is_correct)
                        {
                            Calculate_Components();
                        }
                        else MessageBox.Show("Матрица смежности задана неверно! Проверьте симметричность и нули на главной диагонали", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else MessageBox.Show("Некорректный ввод числа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void Remove_Visual_Matrix()
        {
            if (Matrix_Cells != null)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Controls.Remove(Matrix_Cells[i, j]);
                    }
                }
                Matrix_Cells = null;
                matrix = null;
                current_index = 0;
            }
        }
        void Convert_From_File()
        {
            string[] lines = Read_FromFile();
            if (lines != null)
            {
                size = lines.Length;
                if (size <= 20 && size >= 1)
                {
                    matrix = new int[size, size];
                    bool is_correct = true;
                    for (int i = 0; i < lines.Length && is_correct; i++)
                    {
                        lines[i] = string.Join("", lines[i].Split(','));
                        string tab = "\t";
                        string[] value_lines = lines[i].Split(' ',tab.ToCharArray()[0]);
                        if (value_lines.Length != size) is_correct = false;
                        for (int j = 0; j < size && is_correct; j++)
                        {
                            is_correct = (int.TryParse(value_lines[j], out matrix[i, j]) && (value_lines[j] == "0" || value_lines[j] == "1"));
                        }
                    }
                    if (!is_correct)
                    {
                        matrix = null;
                        size = 0;
                        MessageBox.Show("Файл содержит некорректные данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        is_correct = Matrix_Check(is_from_file: true);
                        if (is_correct)
                        {
                            InputLabel.Text = "Матрица из файла: ";
                            InputLabel.Visible = true;
                            Generate_Visual_Matrix(true);
                            Calculate_Components();
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
        bool Matrix_Check(bool is_from_file)
        {
            bool is_correct = true;
            for (int i = 0; i < size && is_correct; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] != matrix[j, i])
                    {
                        is_correct = false;
                        if (!is_from_file) Controls.Find("TextBox" + (i * size + j).ToString(), false)[0].Focus();
                    }
                    else if (i == j && matrix[i, j] != 0)
                    {
                        is_correct = false;
                        if (!is_from_file) Controls.Find("TextBox" + (i * size + j).ToString(), false)[0].Focus();

                    }
                }
            }
            return is_correct;

        }
        void Remove_Component_Label()
        {
            if (component_label != null)
            {
                Controls.Remove(component_label);
                component_label = null;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            InputLabel.Visible = false;
            if (comboBox1.SelectedItem.ToString() == "Ввод из файла")
            {
                Remove_Visual_Matrix();
                Remove_Component_Label();
                SizeChoice.Visible = false;
                InputSize.Visible = false;
                InputLabel.Visible = false;
                Convert_From_File();
            }
            else if (comboBox1.SelectedItem.ToString() == "Ввод вручную")
            {
                Remove_Visual_Matrix();
                Remove_Component_Label();
                SizeChoice.Visible = true;
                InputSize.Visible = true;
                InputSize.Focus();
            }
            else if (comboBox1.SelectedItem.ToString() == "Генерация данных")
            {
                TestGenerator.Generate_Tests();
                MessageBox.Show("Файлы входных данных сгенерированы", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void InputSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Size_Check();
                e.SuppressKeyPress = true;
            }

        }
        void Size_Check()
        {
            Remove_Visual_Matrix();
            Remove_Component_Label();
            if (int.TryParse(InputSize.Text, out size) && size >= 1 && size <= 20)
            {
                InputLabel.Text = "Введите данные в таблицу:";
                InputLabel.Visible = true;
                matrix = new int[size, size];
                Generate_Visual_Matrix(false);
            }
            else MessageBox.Show("Введите натуральное число от 1 до 20!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}