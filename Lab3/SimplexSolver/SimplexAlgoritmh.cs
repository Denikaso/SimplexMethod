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
        private SimplexTable simplexTable;
        internal SimplexAlgoritmh(LinearProgram linearProgram)
        {
            this.linearProgram = linearProgram;
        }        
    }
}
