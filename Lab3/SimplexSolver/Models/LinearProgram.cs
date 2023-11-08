using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SimplexSolverProject.SimplexSolver.Models
{
    internal class LinearProgram
    {
        public List<double> objectiveFunctionCoefficients = new List<double>();
        public string Objective { get; private set; }

        public List<List<double>> constraintsCoefficients = new List<List<double>>();
        public List<string> constraintsSigns = new List<string>();
        public List<double> constraintsB = new List<double>();
        public void SetObjective(string objective)
        {
            Objective = objective;
        }

        public void SetObjectiveFunctionCoefficients(List<double> coefficients)
        {
            objectiveFunctionCoefficients = coefficients;
        }

        public void AddConstraint(List<double> coefficients, string sign, double b)
        {
            constraintsCoefficients.Add(coefficients);
            constraintsSigns.Add(sign);
            constraintsB.Add(b);
        }
        public void ToStandardForm()
        {
            // 1. Если задача максимизируется, инвертируем коэффициенты целевой функции.
            if (Objective == "max")
            {
                for (int i = 0; i < objectiveFunctionCoefficients.Count; i++)
                {
                    objectiveFunctionCoefficients[i] *= -1;
                }
                Objective = "min";
            }

            // 2. Добавление дополнительных переменных для ограничений с >= и <=
            int additionalVariableCount = 0; // Счетчик дополнительных переменных
            for (int i = 0; i < constraintsCoefficients.Count; i++)
            {                
                if (constraintsSigns[i] == ">=")
                {
                    // Добавляем дополнительные переменные и пересчитываем коэффициенты
                    for (int j = 0; j < additionalVariableCount; j++)
                    {                                                     
                            constraintsCoefficients[i].Add(0); // Дополнительные переменные с коэффициентами 0
                    }
                    constraintsCoefficients[i].Add(-1); // Дополнительная переменная с коэффициентом -1
                    objectiveFunctionCoefficients.Add(0);
                    additionalVariableCount++;
                }
                else if (constraintsSigns[i] == "<=")
                {
                    // Добавляем дополнительные переменные и пересчитываем коэффициенты
                    for (int j = 0; j < additionalVariableCount; j++)
                    {
                        constraintsCoefficients[i].Add(0); // Дополнительные переменные с коэффициентами 0
                    }
                    constraintsCoefficients[i].Add(1); // Дополнительная переменная с коэффициентом 1
                    objectiveFunctionCoefficients.Add(0);
                    additionalVariableCount++;
                }
            }
            // Обработка ограничений со знаком "="
            for (int i = 0; i < constraintsCoefficients.Count; i++)
            {
                int maxLenght = constraintsCoefficients.Max(list => list.Count);
                int variance = maxLenght - constraintsCoefficients[i].Count;
                if (variance > 0)
                {
                    // Добавляем дополнительные переменные и пересчитываем коэффициенты
                    for (int j = 0; j < variance; j++)
                    {
                        constraintsCoefficients[i].Add(0); // Дополнительные переменные с коэффициентами 0                        
                    }
                }
            }
        }
    }
}
