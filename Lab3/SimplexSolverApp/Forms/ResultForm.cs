using SimplexSolverProject.SimplexSolver;
using SimplexSolverProject.SimplexSolver.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplexSolverProject.SimplexSolverApp.Forms
{
    public partial class ResultForm : Form
    {
        private LinearProgram linearProgram;
        private SimplexAlgoritmh simplexAlgoritmh;
        internal ResultForm(LinearProgram linearProgram)
        {
            InitializeComponent();
            this.linearProgram = linearProgram;
            simplexAlgoritmh = new SimplexAlgoritmh(linearProgram);
            linearProgram.ToStandardForm();

            // Вычислите размеры формы на основе количества ограничений и коэффициентов целевой функции
            int width = 470 + (linearProgram.constraintsCoefficients.Count * 60); ; // Ширина формы
            int height = 180 + (linearProgram.constraintsCoefficients.Count * 40); // Начальная высота + (количество ограничений * высота элемента)

            this.Size = new Size(width, height);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = Properties.Resources.SolutionIcon;

            DisplayStandardForm();
        }

        private void DisplayStandardForm()
        {
            // Создаем панель для отображения элементов
            Panel panel = new Panel();
            panel.Location = new Point(10, 10);
            panel.Size = new Size(700, 700);
            this.Controls.Add(panel);

            // Устанавливаем общий размер шрифта
            Font font = new Font(FontFamily.GenericSansSerif, 11);
            Size size;

            // Заголовок "Стандартная форма:"
            Label standardFormLabel = new Label();
            standardFormLabel.Text = "Стандартная форма:";
            standardFormLabel.Location = new Point(6, 10);
            standardFormLabel.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold); // Устанавливаем жирный шрифт
            size = TextRenderer.MeasureText(standardFormLabel.Text, standardFormLabel.Font);
            standardFormLabel.Size = size;
            panel.Controls.Add(standardFormLabel);

            // Заголовок для целевой функции
            Label objectiveHeaderLabel = new Label();
            objectiveHeaderLabel.Text = "Целевая функция:";
            objectiveHeaderLabel.Location = new Point(10, 10);
            objectiveHeaderLabel.Font = font;
            objectiveHeaderLabel.Width = 200;
            size = TextRenderer.MeasureText(objectiveHeaderLabel.Text, objectiveHeaderLabel.Font);
            objectiveHeaderLabel.Size = size;
            panel.Controls.Add(objectiveHeaderLabel);

            // Отобразите целевую функцию
            string objectiveFunction = "F = ";
            double coefficient = linearProgram.objectiveFunctionCoefficients[0];
            objectiveFunction += $"{coefficient}x{1}";
            for (int i = 1; i < linearProgram.objectiveFunctionCoefficients.Count; i++)
            {
                coefficient = linearProgram.objectiveFunctionCoefficients[i];
                if (coefficient >= 0)
                {
                    objectiveFunction += $" +{coefficient}x{i + 1}";
                }
                else
                {
                    objectiveFunction += $" {coefficient}x{i + 1} ";
                }                
            }
            objectiveFunction += $" -> {linearProgram.Objective}";

            Label objectiveLabel = new Label();
            objectiveLabel.Text = objectiveFunction;
            objectiveLabel.Location = new Point(10, 40);
            objectiveLabel.Font = font;
            size = TextRenderer.MeasureText(objectiveLabel.Text, objectiveLabel.Font);
            objectiveLabel.Size = size;

            panel.Controls.Add(objectiveLabel);

            // Заголовок для ограничений
            Label constraintHeaderLabel = new Label();
            constraintHeaderLabel.Text = "Ограничения:";
            constraintHeaderLabel.Location = new Point(10, 70);
            constraintHeaderLabel.Font = font;
            size = TextRenderer.MeasureText(constraintHeaderLabel.Text, constraintHeaderLabel.Font);
            constraintHeaderLabel.Size = size;
            panel.Controls.Add(constraintHeaderLabel);

            // Отобразите ограничения
            for (int i = 0; i < linearProgram.constraintsCoefficients.Count; i++)
            {
                string constraintText = $"Ограничение {i + 1}: ";
                for (int j = 0; j < linearProgram.constraintsCoefficients[i].Count; j++)
                {
                    coefficient = linearProgram.constraintsCoefficients[i][j];
                    if (j > 0)
                    {
                        if (coefficient >= 0)
                        {
                            constraintText += " + ";
                        }
                        else
                        {
                            constraintText += " - ";
                            coefficient = Math.Abs(coefficient); // Берем абсолютное значение
                        }
                    }
                    constraintText += $"{coefficient}x{j + 1}";
                }
                constraintText += $" = {linearProgram.constraintsB[i]}";

                Label constraintLabel = new Label();
                constraintLabel.Text = constraintText;
                constraintLabel.Location = new Point(10, 100 + i * 30);
                constraintLabel.Font = font;
                size = TextRenderer.MeasureText(constraintLabel.Text, constraintLabel.Font);
                constraintLabel.Size = size;
                panel.Controls.Add(constraintLabel);
            }
        }

        private void ResultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
