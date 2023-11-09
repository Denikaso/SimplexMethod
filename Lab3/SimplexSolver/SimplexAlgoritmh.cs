using SimplexSolverProject.SimplexSolver.Models;
using SimplexSolverProject.SimplexSolverApp.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexSolverProject.SimplexSolver
{

    internal class SimplexAlgoritmh
    {
        private const string fileName = "C:\\Уник\\Методы\\SimplexMethod\\results\\table";
        private LinearProgram linearProgram;
        private List<int> basisVariables;
        public List<string> tableFiles;
        private int iterationIndex;
        internal SimplexAlgoritmh(LinearProgram linearProgram)
        {
            this.linearProgram = linearProgram;
            basisVariables = new List<int>();
            tableFiles = new List<string>();
            iterationIndex = 0;
        }
        public void Solve()
        {
            // Основной цикл симплекс-алгоритма
            while (!IsOptimal())
            {
                iterationIndex++;
                // Выбор разрешающего столбца
                int pivotColumn = SelectPivotColumn();

                // Выбор разрешающей строки
                int pivotRow = SelectPivotRow(pivotColumn);

                // Переход к новому базису
                Pivot(pivotRow, pivotColumn);
                WriteSimplexTableToFile(fileName + iterationIndex);                
            }
        }

        private bool IsOptimal()
        {
            return linearProgram.objectiveFunctionCoefficients.All(coefficient => coefficient >= 0);
        }

        private int SelectPivotColumn()
        {
            // Выбор разрешающего столбца
            // Обычно это столбец с наименьшим положительным значением в строке целевой функции
            int pivotColumn = 0;
            double minValue = double.MaxValue;
            for (int i = 0; i < linearProgram.objectiveFunctionCoefficients.Count; i++)
            {
                if (linearProgram.objectiveFunctionCoefficients[i] < minValue)
                {
                    minValue = linearProgram.objectiveFunctionCoefficients[i];
                    pivotColumn = i;
                }
            }
            return pivotColumn;            
        }

        private int SelectPivotRow(int pivotColumn)
        {
            // Выбор разрешающей строки
            // Обычно это строка, в которой столбец с наименьшим положительным отношением к правой части
            int pivotRow = -1;
            double minRatio = double.MaxValue;
            for (int i = 0; i < linearProgram.constraintsB.Count; i++)
            {
                if (linearProgram.constraintsCoefficients[i][pivotColumn] > 0)
                {
                    double ratio = linearProgram.constraintsB[i] / linearProgram.constraintsCoefficients[i][pivotColumn];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }
            return pivotRow;
        }

        private void Pivot(int pivotRow, int pivotColumn)
        {
            basisVariables[pivotRow] = pivotColumn+1;            

            // Пересчет коэффициентов целевой функции
            double pivotValue = linearProgram.constraintsCoefficients[pivotRow][pivotColumn];
            // Для каждого элемента базисной строки
            for (int i = 0; i < linearProgram.constraintsCoefficients[pivotRow].Count; i++)
            {
                // Делим элемент на ведущий элемент
                linearProgram.constraintsCoefficients[pivotRow][i] /= pivotValue;                
            }
            linearProgram.constraintsB[pivotRow] /= pivotValue;
            double pivotColumnElement;
            // Для каждой строки, кроме базисной
            for (int i = 0; i < linearProgram.constraintsCoefficients.Count; i++)
            {
                if (i != pivotRow)
                {
                    pivotColumnElement = linearProgram.constraintsCoefficients[i][pivotColumn];
                    for (int j = 0; j < linearProgram.constraintsCoefficients[i].Count; j++)
                    {
                        linearProgram.constraintsCoefficients[i][j] -= linearProgram.constraintsCoefficients[pivotRow][j] * pivotColumnElement;
                    }
                    linearProgram.constraintsB[i] -= linearProgram.constraintsB[pivotRow] * pivotColumnElement;
                }
            }
            pivotColumnElement = linearProgram.objectiveFunctionCoefficients[pivotColumn];
            for (int i = 0;i < linearProgram.objectiveFunctionCoefficients.Count;i++)
            {
                linearProgram.objectiveFunctionCoefficients[i] -= linearProgram.constraintsCoefficients[pivotRow][i] * pivotColumnElement;                                
            }
            linearProgram.objectiveFunctionB -= linearProgram.constraintsB[pivotRow] * pivotColumnElement;
        }

        public void GetSolution(out List<double> solution, out double result)
        {
            solution = new List<double>(linearProgram.objectiveFunctionCoefficients.Count);
            result = 0;
            for (int i = 0; i < linearProgram.objectiveFunctionCoefficients.Count; i++)
            {
                solution.Add(double.MinValue); 
            }
            for (int i = 0; i < linearProgram.constraintsCoefficients.Count; i++)
            {
                for (int j = 0; j < linearProgram.constraintsCoefficients[i].Count; j++)
                {
                    if(basisVariables.Contains(j+1) && linearProgram.constraintsCoefficients[i][j] == 1)
                    {
                        solution[j] = linearProgram.constraintsB[i];
                    }
                }
            }
            for (int i = 0; i < linearProgram.objectiveFunctionCoefficients.Count; i++)
            {
                if (solution[i] == double.MinValue)
                {
                    solution[i] = 0;
                }
            }
            for(int i = 0;i < linearProgram.initialObjectiveFunctionCoefficients.Count; i++)
            {
                result += linearProgram.initialObjectiveFunctionCoefficients[i] * solution[i];
            }
        }
        public bool IsCanonical(LinearProgram linearProgram)
        {            
            int maxLenght = linearProgram.constraintsCoefficients.Max(list => list.Count);            
            List<bool> constraintsContainsSingle = new List<bool>(linearProgram.constraintsSigns.Count);
            for (int i = 0; i < linearProgram.constraintsCoefficients.Count;i++)
            {
                bool singleCoefficent = false;
                for (int j = 0;j < linearProgram.constraintsCoefficients[i].Count; j++)
                {
                    if (linearProgram.constraintsCoefficients[i][j] == 1)
                    {
                        singleCoefficent = true;
                        for (int k = 0; k < linearProgram.constraintsCoefficients.Count; k++)
                        {
                            if (k != i)
                            {
                                if (linearProgram.constraintsCoefficients[k][j] != 0)
                                    singleCoefficent = false;
                            }                            
                        }
                        if (singleCoefficent)
                        {
                            basisVariables.Add(j + 1);
                            break;
                        }                            
                    }                    
                }                
                constraintsContainsSingle.Add(singleCoefficent);
            }
            return linearProgram.constraintsCoefficients.Count == constraintsContainsSingle.Count(item => item == true);
        }
        public void CheckObjectiveFunction()
        {
            int basisVAriable;
            for(int i = 0; i < basisVariables.Count; i++)
            {
                basisVAriable = basisVariables[i] - 1;
                if (linearProgram.objectiveFunctionCoefficients[basisVAriable] != 0)
                {
                    for (int j = 0; j < linearProgram.constraintsCoefficients.Count; j++)
                    {
                        if (linearProgram.constraintsCoefficients[j][basisVAriable] == 1)
                        {
                            for (int k = 0; k < linearProgram.constraintsCoefficients[j].Count;k++)
                            {
                                if (linearProgram.constraintsCoefficients[j][k] != 0 && k != basisVAriable)
                                {
                                    linearProgram.objectiveFunctionCoefficients[k] += linearProgram.objectiveFunctionCoefficients[basisVAriable] * (-1 * linearProgram.constraintsCoefficients[j][k]);
                                }                                
                            }
                            linearProgram.objectiveFunctionB += linearProgram.constraintsB[j] * linearProgram.objectiveFunctionCoefficients[basisVAriable];
                            linearProgram.objectiveFunctionCoefficients[basisVAriable] = 0;
                        }
                    }
                }
            }
            linearProgram.objectiveFunctionB *= -1;
        }
        public void WriteSimplexTableToFile(string fileName)
        {
            int columnCount = linearProgram.constraintsCoefficients.Max(list => list.Count);
            int rowCount = basisVariables.Count;
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    StringBuilder header = new StringBuilder("       |");
                    for (int j = 0; j < columnCount; j++)
                    {
                        header.AppendFormat("   x{0}   |", j + 1);
                    }
                    header.AppendFormat("   RHS  |");

                    writer.WriteLine(header);

                    writer.WriteLine(new string('-', header.Length));
                    StringBuilder row;
                    int basisVariableIndex = 0;

                    foreach (int basisVariableKey in basisVariables)
                    {
                        row = new StringBuilder();
                        row.AppendFormat("x{0}     | ", basisVariableKey);

                        for (int j = 0; j < columnCount; j++)
                        {
                            row.AppendFormat("{0, 7}| ", linearProgram.constraintsCoefficients[basisVariableIndex][j]);
                        }
                        row.AppendFormat("{0, 7}| ", linearProgram.constraintsB[basisVariableIndex]);

                        writer.WriteLine(row);
                        basisVariableIndex++;
                    }

                    row = new StringBuilder("F      | ");
                    foreach (double objectiveFunctionCoefficient in linearProgram.objectiveFunctionCoefficients)
                    {
                        row.AppendFormat("{0, 7}| ", objectiveFunctionCoefficient);
                    }
                    row.AppendFormat("{0, 7}|", linearProgram.objectiveFunctionB);
                    writer.WriteLine(row);
                }
                tableFiles.Add(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при записи в файл: " + ex.Message);
            }
        }
    }
}
