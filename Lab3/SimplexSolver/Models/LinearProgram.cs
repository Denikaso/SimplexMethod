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
        // Коэффициенты целевой функции
        private List<double> objectiveFunctionCoefficients = new List<double>();
        public string Objective { get; private set; } // Максимизация ("max") по умолчанию

        // Ограничения
        private List<List<double>> constraintsCoefficients = new List<List<double>>();
        private List<string> constraintsSigns = new List<string>();
        private List<double> constraintsB = new List<double>();
        public void SetObjective(string objective)
        {
            // Установить выбор пользователя (максимизация или минимизация)
            Objective = objective;
        }

        public void SetObjectiveFunctionCoefficients(List<double> coefficients)
        {
            // Установить коэффициенты целевой функции
            objectiveFunctionCoefficients = coefficients;
        }

        public void AddConstraint(List<double> coefficients, string sign, double b)
        {
            // Добавить ограничение
            constraintsCoefficients.Add(coefficients);
            constraintsSigns.Add(sign);
            constraintsB.Add(b);
        }

    }
}
