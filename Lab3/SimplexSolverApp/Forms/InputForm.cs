using SimplexSolverProject.SimplexSolver.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplexSolverProject.SimplexSolverApp.Forms
{
    public partial class InputForm : Form
    {
        int _numberOfVariables;
        int _numberOfConstraints;
        private string selectedObjective;
        Panel panel;
        public InputForm(int numberOfVariables, int numberOfConstraints)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this._numberOfVariables = numberOfVariables;
            this._numberOfConstraints = numberOfConstraints;
            this.Text = "Ввод данных";
            this.Icon = Properties.Resources.SolutionIcon;

            int formWidth = 350 + (numberOfVariables * 95);
            int formHeight = 260 + (numberOfConstraints * 45);
            this.Size = new Size(formWidth, formHeight);

            panel = new Panel();
            panel.Size = new Size(formWidth, formHeight);
            this.Controls.Add(panel);

            ComboBox comboObjective = new ComboBox();
            comboObjective.Name = "comboObjective";
            comboObjective.SelectedIndexChanged += ComboObjective_SelectedIndexChanged;
            comboObjective.Items.Add("max");
            comboObjective.Items.Add("min");
            comboObjective.SelectedIndex = 0;
            comboObjective.Size = new Size(100, 20);
            comboObjective.Location = new Point(10, 10);
            comboObjective.DropDownStyle = ComboBoxStyle.DropDownList;
            panel.Controls.Add(comboObjective);
            comboObjective.TabIndex = 0;

            for (int i = 0; i < numberOfVariables; i++)
            {
                Label labelVariable = new Label();
                labelVariable.Text = "x" + (i + 1);
                labelVariable.Size = new Size(30, 25);
                labelVariable.Location = new Point(10 + i * 120, 50);
                labelVariable.Font = new Font(labelVariable.Font.FontFamily, 11);
                panel.Controls.Add(labelVariable);

                TextBox textBoxVariable = new TextBox();
                textBoxVariable.Name = "textBoxVariable" + i;
                textBoxVariable.Size = new Size(100, 20);
                textBoxVariable.Location = new Point(10 + i * 120, 80);
                panel.Controls.Add(textBoxVariable);
                textBoxVariable.TabIndex = i + 1;
            }

            Label labelConstraints = new Label();
            labelConstraints.Text = "Ограничения";
            labelConstraints.Size = new Size(150, 25);
            labelConstraints.Location = new Point((formWidth - labelConstraints.Width) / 2, 120);
            labelConstraints.Font = new Font(labelConstraints.Font.FontFamily, 11);
            panel.Controls.Add(labelConstraints);

            int tabIndex = numberOfVariables + 1;

            for (int i = 0; i < numberOfConstraints; i++)
            {
                for (int j = 0; j < numberOfVariables; j++)
                {
                    TextBox textBoxConstraint = new TextBox();
                    textBoxConstraint.Name = "textBoxConstraint" + i + "Variable" + j;
                    textBoxConstraint.Size = new Size(100, 20);
                    textBoxConstraint.Location = new Point(10 + j * 120, 160 + i * 40);
                    textBoxConstraint.Font = new Font(textBoxConstraint.Font.FontFamily, 11);
                    panel.Controls.Add(textBoxConstraint);
                    textBoxConstraint.TabIndex = tabIndex;
                    tabIndex++;
                }

                ComboBox comboSign = new ComboBox();
                comboSign.Name = "comboSign" + i;
                comboSign.Items.Add(">=");
                comboSign.Items.Add("<=");
                comboSign.Items.Add("=");
                comboSign.SelectedIndex = 0;
                comboSign.Size = new Size(50, 20);
                comboSign.Location = new Point(10 + (numberOfVariables * 120), 160 + i * 40);
                comboSign.DropDownStyle = ComboBoxStyle.DropDownList;
                panel.Controls.Add(comboSign);
                comboSign.TabIndex = tabIndex;
                tabIndex++;

                TextBox textBoxB = new TextBox();
                textBoxB.Name = "textBoxB" + i;
                textBoxB.Size = new Size(100, 20);
                textBoxB.Location = new Point(10 + (numberOfVariables * 120) + 60, 160 + i * 40);
                textBoxB.Font = new Font(textBoxB.Font.FontFamily, 11);
                panel.Controls.Add(textBoxB);
                textBoxB.TabIndex = tabIndex;
                tabIndex++;
            }
            Button calculateButton = new Button();
            calculateButton.Name = "calculateButton";
            calculateButton.Text = "Рассчитать";
            calculateButton.Size = new Size(145, 37);
            int xPosition = (formWidth - calculateButton.Width) / 2;
            int yPosition = 130 + numberOfConstraints * 40 + 40;
            calculateButton.Location = new Point(xPosition, yPosition);
            calculateButton.Font = new Font(calculateButton.Font.FontFamily, 11, FontStyle.Bold);
            panel.Controls.Add(calculateButton);
            calculateButton.TabIndex = tabIndex;
            calculateButton.Click += CalculateButton_Click;
        }
        private void CalculateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Собрать данные с формы и создать экземпляр LinearProgram
                LinearProgram linearProgram = new LinearProgram();

                // Собрать данные и передать их в linearProgram
                List<double> objectiveCoefficients = new List<double>();
                for (int i = 0; i < _numberOfVariables; i++)
                {
                    string variableName = "textBoxVariable" + i;
                    if (panel.Controls.ContainsKey(variableName) && panel.Controls[variableName] is TextBox textBox)
                    {
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            // Обработка ошибки: поле пусто
                            MessageBox.Show("Поле для коэффициента целевой функции x" + (i + 1) + " пусто", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (double.TryParse(textBox.Text, out double coefficient))
                        {
                            objectiveCoefficients.Add(coefficient);
                        }
                        else
                        {
                            // Обработка ошибки: некорректный ввод
                            MessageBox.Show("Некорректный ввод коэффициента целевой функции для x" + (i + 1), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                linearProgram.SetObjectiveFunctionCoefficients(objectiveCoefficients);
                linearProgram.SetObjective(selectedObjective);

                // Повторите этот процесс для ограничений (коэффициенты, знаки и свободные члены)
                for (int i = 0; i < _numberOfConstraints; i++)
                {
                    List<double> constraintCoefficients = new List<double>();
                    for (int j = 0; j < _numberOfVariables; j++)
                    {
                        string variableName = "textBoxConstraint" + i + "Variable" + j;
                        if (panel.Controls.ContainsKey(variableName) && panel.Controls[variableName] is TextBox textBox)
                        {
                            if (string.IsNullOrWhiteSpace(textBox.Text))
                            {
                                // Обработка ошибки: поле пусто
                                MessageBox.Show("Поле для коэффициента ограничения " + (i + 1) + " x" + (j + 1) + " пусто", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (double.TryParse(textBox.Text, out double coefficient))
                            {
                                constraintCoefficients.Add(coefficient);
                            }
                            else
                            {
                                // Обработка ошибки: некорректный ввод
                                MessageBox.Show("Некорректный ввод коэффициента ограничения " + (i + 1) + " для x" + (j + 1), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    string signName = "comboSign" + i;
                    if (panel.Controls.ContainsKey(signName) && panel.Controls[signName] is ComboBox comboBox)
                    {
                        string constraintSign = comboBox.SelectedItem.ToString();

                        string bName = "textBoxB" + i;
                        if (panel.Controls.ContainsKey(bName) && panel.Controls[bName] is TextBox bTextBox)
                        {
                            if (string.IsNullOrWhiteSpace(bTextBox.Text))
                            {
                                // Обработка ошибки: поле пусто
                                MessageBox.Show("Поле для свободного члена ограничения " + (i + 1) + " пусто", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (double.TryParse(bTextBox.Text, out double constraintB))
                            {
                                // Добавьте ограничение в модель LinearProgram
                                linearProgram.AddConstraint(constraintCoefficients, constraintSign, constraintB);
                            }
                            else
                            {
                                // Обработка ошибки: некорректный ввод
                                MessageBox.Show("Некорректный ввод свободного члена для ограничения " + (i + 1), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                ResultForm resultForm = new ResultForm(linearProgram);
                resultForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при расчете: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ComboObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            selectedObjective = comboBox.SelectedItem.ToString();
        }

        private void inputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
