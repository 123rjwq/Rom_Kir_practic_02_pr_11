using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace pr_11
{
    public partial class SudokuForm : Form
    {
        // Массив цветов для блоков 3x3 в судоку
        private Color[,] blockColors = {
            { Color.Red, Color.Orange, Color.Yellow },
            { Color.Green, Color.Blue, Color.Indigo },
            { Color.Violet, Color.Gray, Color.White }
        };

        // Двумерный массив текстовых полей для судоку
        private TextBox[,] sudokuTextBoxes;

        public SudokuForm()
        {
            // Инициализация текстовых полей для судоку
            InitializeSudokuTextBoxes();
            // Установка цвета для всех блоков 3x3
            ColorAllSudokuBlocks();

            // Создание кнопки для генерации судоку
            CreateGenerateButton();
            // Создание кнопки для очистки полей судоку
            CreateClearButton();
            // Создание кнопки для сохранения судоку в файл
            SaveButton();
            // Создание кнопки для загрузки судоку из файла
            LoadSudokuButton();

            // Установка размеров формы
            this.Width = 500;
            this.Height = 500;
        }

        // Инициализация текстовых полей для судоку
        private void InitializeSudokuTextBoxes()
        {
            sudokuTextBoxes = new TextBox[9, 9];
            int textBoxSize = 30;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Size = new System.Drawing.Size(textBoxSize, textBoxSize);
                    textBox.Location = new System.Drawing.Point(col * (textBoxSize + 3), row * (textBoxSize + 3));
                    textBox.TextAlign = HorizontalAlignment.Center;

                    textBox.ReadOnly = true;
                    textBox.Multiline = true;
                    textBox.Font = new Font(textBox.Font.FontFamily, 16);

                    this.Controls.Add(textBox);
                    sudokuTextBoxes[row, col] = textBox;
                }
            }
        }

        // Установка цвета для блока 3x3 в судоку
        private void ColorSudokuBlock(int blockRow, int blockCol)
        {
            for (int row = blockRow * 3; row < (blockRow + 1) * 3; row++)
            {
                for (int col = blockCol * 3; col < (blockCol + 1) * 3; col++)
                {
                    sudokuTextBoxes[row, col].BackColor = blockColors[blockRow, blockCol];
                    sudokuTextBoxes[row, col].BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        // Установка цвета для всех блоков 3x3 в судоку
        private void ColorAllSudokuBlocks()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    ColorSudokuBlock(row, col);
                }
            }
        }

        // Создание кнопки для генерации судоку
        private void CreateGenerateButton()
        {
            Button generateButton = new Button();
            generateButton.Text = "Сгенерировать судоку";
            generateButton.Size = new Size(150, 30);
            generateButton.Location = new Point(300, 9);
            generateButton.Click += new EventHandler(GenerateSudokuButton_Click);

            this.Controls.Add(generateButton);
        }

        // Создание кнопки для очистки полей судоку
        private void CreateClearButton()
        {
            Button clearButton = new Button();
            clearButton.Text = "Очистить поля";
            clearButton.Size = new Size(150, 30);
            clearButton.Location = new Point(300, 50);
            clearButton.Click += new EventHandler(ClearSudokuFieldsButton_Click);

            this.Controls.Add(clearButton);
        }

        // Создание кнопки для сохранения судоку в файл
        private void SaveButton()
        {
            Button btnSave = new Button();
            btnSave.Text = "Сохранить судоку";
            btnSave.Size = new Size(150, 30);
            btnSave.Location = new Point(300, 90);
            btnSave.Click += new EventHandler(btnSaveSudoku_Click);

            this.Controls.Add(btnSave);
        }

        // Создание кнопки для загрузки судоку из файла
        private void LoadSudokuButton()
        {
            Button btnLoad = new Button();
            btnLoad.Text = "Загрузить судоку";
            btnLoad.Size = new Size(150, 30);
            btnLoad.Location = new Point(300, 130);
            btnLoad.Click += new EventHandler(btnLoadSudoku_Click);

            this.Controls.Add(btnLoad);
        }

        // Генерирование судоку
        private void GenerateSudoku()
        {
            int[,] sudoku = new int[9, 9];
            GenerateSudokuRecursively(sudoku);
            DisplaySudoku(sudoku);
        }

        // Рекурсивный метод для генерации судоку
        private bool GenerateSudokuRecursively(int[,] sudoku)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (sudoku[row, col] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsSafe(sudoku, row, col, num))
                            {
                                sudoku[row, col] = num;
                                if (GenerateSudokuRecursively(sudoku))
                                    return true;
                                sudoku[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        // Проверка, безопасно ли установить число в данную ячейку
        private bool IsSafe(int[,] sudoku, int row, int col, int num)
        {
            return !UsedInRow(sudoku, row, num) && !UsedInCol(sudoku, col, num) && !UsedInBox(sudoku, row - row % 3, col - col % 3, num);
        }

        // Проверка, использовалось ли число в данной строке
        private bool UsedInRow(int[,] sudoku, int row, int num)
        {
            for (int col = 0; col < 9; col++)
            {
                if (sudoku[row, col] == num)
                    return true;
            }
            return false;
        }

        // Проверка, использовалось ли число в данном столбце
        private bool UsedInCol(int[,] sudoku, int col, int num)
        {
            for (int row = 0; row < 9; row++)
            {
                if (sudoku[row, col] == num)
                    return true;
            }
            return false;
        }

        // Проверка, использовалось ли число в данном блоке 3x3
        private bool UsedInBox(int[,] sudoku, int boxStartRow, int boxStartCol, int num)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (sudoku[row + boxStartRow, col + boxStartCol] == num)
                        return true;
                }
            }
            return false;
        }

        // Отображение сгенерированного судоку
        private void DisplaySudoku(int[,] sudoku)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    sudokuTextBoxes[row, col].Text = sudoku[row, col].ToString();
                }
            }
        }

        // Обработчик события кнопки "Сгенерировать судоку"
        private void GenerateSudokuButton_Click(object sender, EventArgs e)
        {
            GenerateSudoku();
            MessageBox.Show("Судоку сгенерировано!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обработчик события кнопки "Очистить поля"
        private void ClearSudokuFieldsButton_Click(object sender, EventArgs e)
        {
            ClearAllSudokuFields();
            MessageBox.Show("Поля очищены!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Очистка всех полей судоку
        private void ClearAllSudokuFields()
        {
            foreach (TextBox textBox in sudokuTextBoxes)
            {
                textBox.Text = "";
            }
        }

        // Обработчик события для сохранения судоку в файл
        private void btnSaveSudoku_Click(object sender, EventArgs e)
        {
            // Создание диалогового окна для выбора файла для сохранения
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";

            // Отображение диалогового окна и проверка, выбран ли файл
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Использование StreamWriter для записи данных в файл
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    // Запись каждой ячейки судоку в файл, разделяя значения пробелами
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            writer.Write(sudokuTextBoxes[i, j].Text + " ");
                        }
                        writer.WriteLine(); // Переход на новую строку после записи каждой строки судоку
                    }
                }
                // Отображение сообщения о том, что судоку сохранено
                MessageBox.Show("Судоку сохранено!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Обработчик события для загрузки судоку из файла
        private void btnLoadSudoku_Click(object sender, EventArgs e)
        {
            // Создание диалогового окна для выбора файла для загрузки
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";

            // Отображение диалогового окна и проверка, выбран ли файл
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Использование StreamReader для чтения данных из файла
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    // Чтение каждой строки из файла и обновление соответствующих ячеек судоку
                    for (int i = 0; i < 9; i++)
                    {
                        string line = reader.ReadLine();
                        if (line != null)
                        {
                            string[] numbers = line.Split(' ');
                            for (int j = 0; j < 9; j++)
                            {
                                sudokuTextBoxes[i, j].Text = numbers[j];
                            }
                        }
                    }
                }
                // Отображение сообщения о том, что судоку загружено
                MessageBox.Show("Судоку загружено!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
