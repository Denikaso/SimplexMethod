using SimplexSolverProject.SimplexSolver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexSolverProject.SimplexSolver
{

    internal class SimplexAlgoritmh
    {
        private LinearProgram linearProgram;
        Dictionary<int, double> basisVariables;
        internal SimplexAlgoritmh(LinearProgram linearProgram)
        {
            this.linearProgram = linearProgram;
            basisVariables = new Dictionary<int, double>();
        }
        public bool IsCanonical(LinearProgram linearProgram)
        {            
            int maxLenght = linearProgram.constraintsCoefficients.Max(list => list.Count);            
            List<bool> constraintsContainsSingle = new List<bool>(linearProgram.constraintsSigns.Count);
            for (int i = 0; i < maxLenght; i++)
            {
                bool singleCoefficent = false;
                for (int j = 0; j < linearProgram.constraintsCoefficients.Count; j++) 
                {
                    if (linearProgram.constraintsCoefficients[j][i] != 0)
                    {
                        if (!singleCoefficent)
                        {
                            if(linearProgram.constraintsCoefficients[j][i] == 1)
                            {
                                singleCoefficent = true;                                
                            }                                
                        }                            
                        else
                        {
                            if (linearProgram.constraintsCoefficients[j][i] != 0)
                            {
                                singleCoefficent = false;
                                break;
                            }
                        }                            
                    }                    
                }
                constraintsContainsSingle.Add(singleCoefficent);
                if (singleCoefficent)                
                    basisVariables.Add(i+1, 1);               
            }
            return linearProgram.constraintsCoefficients.Count == constraintsContainsSingle.Count(item => item == true);
        }

        public void WriteSimplexTableToFile(string fileName)
        {
            int columnCount = linearProgram.constraintsCoefficients.Max(list=>list.Count);
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
                    header.AppendFormat("   RHS  |   Ratio   |");

                    writer.WriteLine(header);

                    writer.WriteLine(new string('-', header.Length));
                    StringBuilder row;
                    int basisVariableIndex = 0;

                    foreach (int basisVariableKey in basisVariables.Keys)
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
                    row.Append("      0|");
                    writer.WriteLine(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при записи в файл: " + ex.Message);
            }
        }

    }
}
