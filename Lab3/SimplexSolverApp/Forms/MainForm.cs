using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplexSolverProject.SimplexSolverApp.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            titleLabel.Location = new Point(15, 20);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            selectVariablesComboBox.Select();
        }

        private void onwardButton_Click(object sender, EventArgs e)
        {
            // Проверяем, что в комбо боксах выбраны значения
            if (selectVariablesComboBox.SelectedIndex != -1 && selectConstraintsСomboBox.SelectedIndex != -1)
            {
                // Значения выбраны, можно перейти к следующей форме
                int numberOfVariables = int.Parse(selectVariablesComboBox.SelectedItem.ToString());
                int numberOfConstraints = int.Parse(selectConstraintsСomboBox.SelectedItem.ToString());

                // Создаем новую форму для ввода коэффициентов
                inputForm inputForm = new inputForm(numberOfVariables, numberOfConstraints);

                // Открываем новую формуму 
                inputForm.Show();

                // Закрываем текущую форму (если это необходимо)
                this.Hide();
            }
            else
            {
                // Выводим сообщение об ошибке, так как не выбраны значения в ComboBox'ах
                MessageBox.Show("Пожалуйста, выберите количество переменных и ограничений.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
