namespace SimplexSolverProject.SimplexSolverApp.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            titleLabel = new Label();
            selectVariablesLabel = new Label();
            selectConstraintsLabel = new Label();
            selectVariablesComboBox = new ComboBox();
            selectConstraintsСomboBox = new ComboBox();
            onwardButton = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            titleLabel.Location = new Point(10, 15);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(568, 50);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Выберите количество переменных и количество ограничений\r\n\r\n";
            // 
            // selectVariablesLabel
            // 
            selectVariablesLabel.AutoSize = true;
            selectVariablesLabel.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            selectVariablesLabel.Location = new Point(19, 84);
            selectVariablesLabel.Name = "selectVariablesLabel";
            selectVariablesLabel.Size = new Size(214, 25);
            selectVariablesLabel.TabIndex = 1;
            selectVariablesLabel.Text = "Количество переменных";
            // 
            // selectConstraintsLabel
            // 
            selectConstraintsLabel.AutoSize = true;
            selectConstraintsLabel.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            selectConstraintsLabel.Location = new Point(19, 144);
            selectConstraintsLabel.Name = "selectConstraintsLabel";
            selectConstraintsLabel.Size = new Size(219, 25);
            selectConstraintsLabel.TabIndex = 2;
            selectConstraintsLabel.Text = "Количество ограничений";
            // 
            // selectVariablesComboBox
            // 
            selectVariablesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            selectVariablesComboBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            selectVariablesComboBox.FormattingEnabled = true;
            selectVariablesComboBox.Items.AddRange(new object[] { "2", "3", "4", "5" });
            selectVariablesComboBox.Location = new Point(269, 84);
            selectVariablesComboBox.Name = "selectVariablesComboBox";
            selectVariablesComboBox.Size = new Size(76, 33);
            selectVariablesComboBox.TabIndex = 0;
            selectVariablesComboBox.Tag = "";
            // 
            // selectConstraintsСomboBox
            // 
            selectConstraintsСomboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            selectConstraintsСomboBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            selectConstraintsСomboBox.FormattingEnabled = true;
            selectConstraintsСomboBox.Items.AddRange(new object[] { "2", "3", "4", "5" });
            selectConstraintsСomboBox.Location = new Point(269, 141);
            selectConstraintsСomboBox.Name = "selectConstraintsСomboBox";
            selectConstraintsСomboBox.Size = new Size(76, 33);
            selectConstraintsСomboBox.TabIndex = 1;
            selectConstraintsСomboBox.Tag = "";
            // 
            // onwardButton
            // 
            onwardButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            onwardButton.Location = new Point(419, 96);
            onwardButton.Name = "onwardButton";
            onwardButton.Size = new Size(104, 58);
            onwardButton.TabIndex = 2;
            onwardButton.Text = "Далее";
            onwardButton.UseVisualStyleBackColor = true;
            onwardButton.Click += onwardButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(598, 216);
            Controls.Add(onwardButton);
            Controls.Add(selectConstraintsСomboBox);
            Controls.Add(selectVariablesComboBox);
            Controls.Add(selectConstraintsLabel);
            Controls.Add(selectVariablesLabel);
            Controls.Add(titleLabel);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Меню";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label selectVariablesLabel;
        private Label selectConstraintsLabel;
        private ComboBox selectVariablesComboBox;
        private ComboBox selectConstraintsСomboBox;
        private Button onwardButton;
    }
}