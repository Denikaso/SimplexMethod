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
        bool isCanonical;
        int standartPanelWidth, standartPanelHeight, canonicalPanelWidht, canonicalPanelHeight;
        internal ResultForm(LinearProgram linearProgram)
        {
            InitializeComponent();
            this.linearProgram = linearProgram;
            linearProgram.ToStandardForm();
            simplexAlgoritmh = new SimplexAlgoritmh(linearProgram);
            isCanonical = simplexAlgoritmh.IsCanonical(linearProgram);
            
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = Properties.Resources.SolutionIcon;

            DisplayStandardForm();
            DisplayCanonicalForm();

            int width = standartPanelWidth + canonicalPanelWidht + 120;
            int height = standartPanelHeight + canonicalPanelHeight + 50;
            this.Size = new Size(width, height);
        }

        private void DisplayStandardForm()
        {            
            FlowLayoutPanel standardPanel = new FlowLayoutPanel();
            standardPanel.FlowDirection = FlowDirection.TopDown; 
            standardPanel.Location = new Point(30, 10);
            standardPanel.AutoSize = true; 
            this.Controls.Add(standardPanel);

            Font font = new Font(FontFamily.GenericSansSerif, 11);
            
            Label standardFormLabel = new Label();
            standardFormLabel.Text = "Стандартная форма:";
            standardFormLabel.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
            standardFormLabel.AutoSize = true;
            standardPanel.Controls.Add(standardFormLabel);
            
            Label objectiveHeaderLabel = new Label();
            objectiveHeaderLabel.Text = "Целевая функция:";
            objectiveHeaderLabel.Font = font;
            objectiveHeaderLabel.AutoSize = true;
            standardPanel.Controls.Add(objectiveHeaderLabel);

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
            objectiveLabel.Font = font;
            objectiveLabel.AutoSize = true;
            standardPanel.Controls.Add(objectiveLabel);

            Label constraintHeaderLabel = new Label();
            constraintHeaderLabel.Text = "Ограничения:";
            constraintHeaderLabel.Font = font;
            constraintHeaderLabel.AutoSize = true;
            standardPanel.Controls.Add(constraintHeaderLabel);

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
                            coefficient = Math.Abs(coefficient);
                        }
                    }
                    constraintText += $"{coefficient}x{j + 1}";
                }
                constraintText += $" = {linearProgram.constraintsB[i]}";

                Label constraintLabel = new Label();
                constraintLabel.Text = constraintText;
                constraintLabel.Font = font;
                constraintLabel.AutoSize = true;
                standardPanel.Controls.Add(constraintLabel);
            }

            standardPanel.PerformLayout();
            standartPanelWidth = standardPanel.Width;
            standartPanelHeight = standardPanel.Height;
        }

        private void DisplayCanonicalForm()
        {
            FlowLayoutPanel canonicalPanel = new FlowLayoutPanel();
            canonicalPanel.Location = new Point(standartPanelWidth + 80, 10);
            canonicalPanel.FlowDirection = FlowDirection.TopDown;
            canonicalPanel.AutoSize = true;
            this.Controls.Add(canonicalPanel);

            Font canonicalFont = new Font(FontFamily.GenericSansSerif, 10);

            Label canonicalFormLabel = new Label();
            canonicalFormLabel.Text = "Каноническая форма:";
            canonicalFormLabel.Location = new Point(6, 10);
            canonicalFormLabel.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);            
            canonicalFormLabel.AutoSize = true;
            canonicalPanel.Controls.Add(canonicalFormLabel);

            string canonicalInfo = isCanonical ? "Задача в канонической форме" : "Задача НЕ в канонической форме";

            Label canonicalInfoLabel = new Label();
            canonicalInfoLabel.Text = canonicalInfo;
            canonicalInfoLabel.Location = new Point(10, 40);
            canonicalInfoLabel.Font = canonicalFont;            
            canonicalInfoLabel.AutoSize = true;            
            canonicalPanel.Controls.Add(canonicalInfoLabel);

            canonicalPanel.PerformLayout();
            canonicalPanelWidht = canonicalPanel.Width;
            canonicalPanelHeight = canonicalPanel.Height;
        }


        private void ResultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
