using SimplexSolverProject.SimplexSolverApp.Forms;

namespace SimplexSolverProject.SimplexSolverApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}