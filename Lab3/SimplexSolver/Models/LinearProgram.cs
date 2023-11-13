using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SimplexSolverProject.SimplexSolver.Models
{
    internal class LinearProgram
    {
        public List<double> objectiveFunctionCoefficients = new List<double>();
        public List<double> initialObjectiveFunctionCoefficients = new List<double>();
        public string Objective { get; private set; }

        public List<List<double>> constraintsCoefficients = new List<List<double>>();
        public List<string> constraintsSigns = new List<string>();
        public List<double> constraintsB = new List<double>();
        public double objectiveFunctionB;
        public void SetObjective(string objective)
        {
            Objective = objective;
        }

        public void SetObjectiveFunctionCoefficients(List<double> coefficients)
        {
            objectiveFunctionCoefficients = coefficients;
            initialObjectiveFunctionCoefficients.AddRange(coefficients);
        }

        public void AddConstraint(List<double> coefficients, string sign, double b)
        {
            constraintsCoefficients.Add(coefficients);
            constraintsSigns.Add(sign);
            constraintsB.Add(b);
        }
        public void ToStandardForm()
        {            
            if (Objective == "max")
            {
                for (int i = 0; i < objectiveFunctionCoefficients.Count; i++)
                {
                    objectiveFunctionCoefficients[i] *= -1;
                }
                Objective = "min";
            }
            
            int additionalVariableCount = 0; 
            for (int i = 0; i < constraintsCoefficients.Count; i++)
            {                
                if (constraintsSigns[i] == ">=")
                {                    
                    for (int j = 0; j < additionalVariableCount; j++)
                    {                                                     
                            constraintsCoefficients[i].Add(0); 
                    }
                    constraintsCoefficients[i].Add(-1); 
                    objectiveFunctionCoefficients.Add(0);
                    additionalVariableCount++;
                }
                else if (constraintsSigns[i] == "<=")
                {                    
                    for (int j = 0; j < additionalVariableCount; j++)
                    {
                        constraintsCoefficients[i].Add(0); 
                    }
                    constraintsCoefficients[i].Add(1); 
                    objectiveFunctionCoefficients.Add(0);
                    additionalVariableCount++;
                }
            }            
            for (int i = 0; i < constraintsCoefficients.Count; i++)
            {
                int maxLenght = constraintsCoefficients.Max(list => list.Count);
                int variance = maxLenght - constraintsCoefficients[i].Count;
                if (variance > 0)
                {                    
                    for (int j = 0; j < variance; j++)
                    {
                        constraintsCoefficients[i].Add(0);                         
                    }
                }
            }
            for (int i = 0; i < constraintsCoefficients.Count; i++)
            {
                if (constraintsB[i] < 0)
                {
                    for(int j = 0; j < constraintsCoefficients[i].Count; j++)
                    {
                        constraintsCoefficients[i][j] *= -1;                        
                    }
                    constraintsB[i] *= -1;
                }
            }
        }
    }
}
