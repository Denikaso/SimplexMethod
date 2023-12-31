﻿using SimplexSolverProject.SimplexSolver;
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
        private const string fileName = "C:\\Уник\\Методы\\SimplexMethod\\results\\table";
        private LinearProgram linearProgram;
        private SimplexAlgoritmh simplexAlgoritmh;        
        private int standartPanelWidth, standartPanelHeight, canonicalPanelWidht, canonicalPanelHeight;
        private int tableLinksPanelWidht, tableLinksPanelHeight, resultsPanelWidht, resultpanelHeight;
        public int currentIteration;
        FlowLayoutPanel tableLinksPanel;
        List<double> solution;
        double result;
        internal ResultForm(LinearProgram linearProgram)
        {
            InitializeComponent();
            this.linearProgram = linearProgram;
            this.AutoSize = true;
            this.Name = "Форма результатов";
            this.MaximizeBox = false;            
            linearProgram.ToStandardForm();
            simplexAlgoritmh = new SimplexAlgoritmh(linearProgram);
            simplexAlgoritmh.isCanonical = simplexAlgoritmh.IsCanonical(linearProgram);
            currentIteration = 0;

            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = Properties.Resources.SolutionIcon;

            DisplayStandardForm();
            tableLinksPanel = CreateTableLinkPanel();
            simplexAlgoritmh.CheckObjectiveFunction();
            DisplayCanonicalForm();
            if (!simplexAlgoritmh.isCanonical)
            {
                simplexAlgoritmh.GetAuxiliaryObjectiveFunction();
                simplexAlgoritmh.WriteSimplexTableToFile(fileName + "0");
                simplexAlgoritmh.Solve();
                GenerateSimplexTablesLinks(simplexAlgoritmh.tableFiles);
                if (simplexAlgoritmh.isInfinitySolution)
                {
                    solution = null;
                    result = 0;
                    DisplayResults(solution, result);
                }
                else
                {
                    simplexAlgoritmh.GetSolution(out solution, out result);
                    DisplayResults(solution, result);
                }
            }
            else
            {                
                simplexAlgoritmh.WriteSimplexTableToFile(fileName + "0");
                simplexAlgoritmh.Solve();
                GenerateSimplexTablesLinks(simplexAlgoritmh.tableFiles);
                simplexAlgoritmh.GetSolution(out solution, out result);
                DisplayResults(solution, result);
            }
            int width = standartPanelWidth + Math.Max(canonicalPanelWidht, resultsPanelWidht) + 120;
            int height = standartPanelHeight + Math.Max(tableLinksPanelHeight, resultpanelHeight) + 100;
            this.Size = new Size(width, height);
            this.PerformLayout();            
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

            Font canonicalFont = new Font(FontFamily.GenericSansSerif, 11);

            Label canonicalFormLabel = new Label();
            canonicalFormLabel.Text = "Каноническая форма:";
            canonicalFormLabel.Location = new Point(6, 10);
            canonicalFormLabel.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
            canonicalFormLabel.AutoSize = true;
            canonicalPanel.Controls.Add(canonicalFormLabel);

            string canonicalInfo;
            if (simplexAlgoritmh.isCanonical)
            {
                canonicalInfo = "Задача в канонической форме";
            }
            else
            {
                canonicalInfo = "Задача НЕ в канонической форме.\nПрименяется двухэтапный симплекс метод.";
            }

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
        public FlowLayoutPanel CreateTableLinkPanel()
        {
            tableLinksPanel = new FlowLayoutPanel();
            tableLinksPanel.FlowDirection = FlowDirection.TopDown;
            tableLinksPanel.Location = new Point(30, standartPanelHeight + 40);
            tableLinksPanel.AutoSize = true;
            this.Controls.Add(tableLinksPanel);
            return tableLinksPanel;
        }
        public void GenerateSimplexTablesLinks(List<string> tableFiles)
        {
            for (int i = 0; i < tableFiles.Count; i++)
            {
                int currentIndex = i; 
                LinkLabel linkLabel = new LinkLabel();
                linkLabel.Text = "Открыть симплекс-таблицу для итерации " + i;
                linkLabel.Location = new Point(10, 100 + i * 40);
                linkLabel.Height = 60;
                linkLabel.AutoSize = true;
                linkLabel.LinkClicked += (sender, e) =>
                {
                    System.Diagnostics.Process.Start("notepad.exe", tableFiles[currentIndex]);
                };
                tableLinksPanel.Controls.Add(linkLabel);
                tableLinksPanel.PerformLayout();
            }
            tableLinksPanelWidht = tableLinksPanel.Width;
            tableLinksPanelHeight = tableLinksPanel.Height;
        }
        private void DisplayResults(List<double> solution, double result)
        {
            FlowLayoutPanel resultsPanel = new FlowLayoutPanel();
            resultsPanel.FlowDirection = FlowDirection.TopDown;
            resultsPanel.Location = new Point(standartPanelWidth + 80, canonicalPanelHeight + 10);
            resultsPanel.AutoSize = true;
            this.Controls.Add(resultsPanel);

            Label resultsHeader = new Label();
            resultsHeader.Text = "Результаты решения задачи:";
            resultsHeader.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
            resultsHeader.AutoSize = true;
            resultsPanel.Controls.Add(resultsHeader);
            if(simplexAlgoritmh.zeroingIteration != -1)
            {
                Label zeroingLabel = new Label();
                zeroingLabel.Text = "Вспомогательная функция \nобнулилась после " + simplexAlgoritmh.zeroingIteration + " итерации";
                zeroingLabel.Font = new Font(FontFamily.GenericSansSerif, 11);
                zeroingLabel.AutoSize = true;
                resultsPanel.Controls.Add(zeroingLabel);
            }
            Label resultLabel = new Label();
            if(simplexAlgoritmh.isNotSolution)
            {
                resultLabel.Text = "\nНевозможно получить канонический вид.\nНет решения";
                resultLabel.Font = new Font(FontFamily.GenericSansSerif, 11);
                resultLabel.AutoSize = true;
                resultsPanel.Controls.Add(resultLabel);
            }
            else if (solution == null)
            {
                resultLabel.Text = "\nF = -ꝏ";
                resultLabel.Font = new Font(FontFamily.GenericSansSerif, 11);
                resultLabel.AutoSize = true;
                resultsPanel.Controls.Add(resultLabel);
            }
            else
            {
                Label solutionLabel = new Label();
                solutionLabel.Text = "\nКоординаты точки: [" + string.Join("; ", solution) + "]";
                solutionLabel.Font = new Font(FontFamily.GenericSansSerif, 11);
                solutionLabel.AutoSize = true;
                resultsPanel.Controls.Add(solutionLabel);

                resultLabel.Text = "F = " + result;
                resultLabel.Font = new Font(FontFamily.GenericSansSerif, 11);
                resultLabel.AutoSize = true;
                resultsPanel.Controls.Add(resultLabel);
            }            
            resultsPanel.PerformLayout();
            resultsPanelWidht = resultsPanel.Width;
            resultpanelHeight = resultsPanel.Height;
        }


        private void ResultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {                
                string fullPath = fileName;
                
                string directoryPath = Path.GetDirectoryName(fullPath);                
                
                foreach (string filePath in Directory.GetFiles(directoryPath))
                {
                    File.Delete(filePath);
                }   
            }
            catch (Exception ex)
            {                
                MessageBox.Show("Произошла ошибка при удалении файлов: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
