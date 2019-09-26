namespace MyNotepad
{
    partial class CustomTheme
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
            this.MenuForeColorButton = new System.Windows.Forms.Button();
            this.MenuBgColorButton = new System.Windows.Forms.Button();
            this.TextBoxBgColorButton = new System.Windows.Forms.Button();
            this.TextBoxForeColorButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MenuForeColorButton
            // 
            this.MenuForeColorButton.Location = new System.Drawing.Point(185, 33);
            this.MenuForeColorButton.Name = "MenuForeColorButton";
            this.MenuForeColorButton.Size = new System.Drawing.Size(139, 32);
            this.MenuForeColorButton.TabIndex = 1;
            this.MenuForeColorButton.Text = "MenuForeColor";
            this.MenuForeColorButton.UseVisualStyleBackColor = true;
            this.MenuForeColorButton.Click += new System.EventHandler(this.MenuForeColorButton_Click);
            // 
            // MenuBgColorButton
            // 
            this.MenuBgColorButton.Location = new System.Drawing.Point(30, 33);
            this.MenuBgColorButton.Name = "MenuBgColorButton";
            this.MenuBgColorButton.Size = new System.Drawing.Size(139, 32);
            this.MenuBgColorButton.TabIndex = 2;
            this.MenuBgColorButton.Text = "MenuBgColor";
            this.MenuBgColorButton.UseVisualStyleBackColor = true;
            this.MenuBgColorButton.Click += new System.EventHandler(this.MenuBgColorButton_Click);
            // 
            // TextBoxBgColorButton
            // 
            this.TextBoxBgColorButton.Location = new System.Drawing.Point(30, 98);
            this.TextBoxBgColorButton.Name = "TextBoxBgColorButton";
            this.TextBoxBgColorButton.Size = new System.Drawing.Size(139, 32);
            this.TextBoxBgColorButton.TabIndex = 3;
            this.TextBoxBgColorButton.Text = "TextBoxBgColor";
            this.TextBoxBgColorButton.UseVisualStyleBackColor = true;
            this.TextBoxBgColorButton.Click += new System.EventHandler(this.TextBoxBgColorButton_Click);
            // 
            // TextBoxForeColorButton
            // 
            this.TextBoxForeColorButton.Location = new System.Drawing.Point(185, 98);
            this.TextBoxForeColorButton.Name = "TextBoxForeColorButton";
            this.TextBoxForeColorButton.Size = new System.Drawing.Size(139, 32);
            this.TextBoxForeColorButton.TabIndex = 4;
            this.TextBoxForeColorButton.Text = "TextBoxForeColor";
            this.TextBoxForeColorButton.UseVisualStyleBackColor = true;
            this.TextBoxForeColorButton.Click += new System.EventHandler(this.TextBoxForeColorButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(124, 155);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(111, 26);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "Close";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // CustomTheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 217);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.TextBoxForeColorButton);
            this.Controls.Add(this.TextBoxBgColorButton);
            this.Controls.Add(this.MenuBgColorButton);
            this.Controls.Add(this.MenuForeColorButton);
            this.Name = "CustomTheme";
            this.Text = "CustomTheme";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MenuForeColorButton;
        private System.Windows.Forms.Button MenuBgColorButton;
        private System.Windows.Forms.Button TextBoxBgColorButton;
        private System.Windows.Forms.Button TextBoxForeColorButton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.Button OKButton;

    }
}