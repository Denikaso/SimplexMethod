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
        internal SimplexAlgoritmh(LinearProgram linearProgram)
        {
            this.linearProgram = linearProgram;
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
                                singleCoefficent = true;
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
            }
            return linearProgram.constraintsCoefficients.Count == constraintsContainsSingle.Count(item => item == true);
        }
    }
}
