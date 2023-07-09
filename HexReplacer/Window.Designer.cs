namespace Hex_replacer
{
    partial class Window
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            source = new Label();
            sourceDirectoryBox = new TextBox();
            buttonSource = new Button();
            buttonOutput = new Button();
            outputDirectoryBox = new TextBox();
            output = new Label();
            hexSourceInput = new TextBox();
            hexSource = new Label();
            hexEditInput = new TextBox();
            hexEdit = new Label();
            replaceButton = new Button();
            counterText = new Label();
            StyleIndicator = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)StyleIndicator).BeginInit();
            SuspendLayout();
            // 
            // source
            // 
            source.AutoSize = true;
            source.Location = new Point(79, 57);
            source.Name = "source";
            source.Size = new Size(54, 20);
            source.TabIndex = 0;
            source.Text = "Source";
            // 
            // sourceDirectoryBox
            // 
            sourceDirectoryBox.AllowDrop = true;
            sourceDirectoryBox.BorderStyle = BorderStyle.FixedSingle;
            sourceDirectoryBox.Location = new Point(139, 54);
            sourceDirectoryBox.Name = "sourceDirectoryBox";
            sourceDirectoryBox.ReadOnly = true;
            sourceDirectoryBox.Size = new Size(309, 27);
            sourceDirectoryBox.TabIndex = 1;
            sourceDirectoryBox.DragDrop += InputTextBox_DragDrop;
            sourceDirectoryBox.DragEnter += InputTextBox_DragEnter;
            // 
            // buttonSource
            // 
            buttonSource.BackColor = Color.Red;
            buttonSource.FlatAppearance.BorderColor = Color.Brown;
            buttonSource.FlatAppearance.BorderSize = 0;
            buttonSource.FlatStyle = FlatStyle.Flat;
            buttonSource.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSource.Location = new Point(468, 53);
            buttonSource.Name = "buttonSource";
            buttonSource.Size = new Size(94, 29);
            buttonSource.TabIndex = 2;
            buttonSource.Text = "Locate";
            buttonSource.UseVisualStyleBackColor = false;
            buttonSource.Click += FolderPicker_Click;
            // 
            // buttonOutput
            // 
            buttonOutput.FlatStyle = FlatStyle.System;
            buttonOutput.Location = new Point(468, 120);
            buttonOutput.Name = "buttonOutput";
            buttonOutput.Size = new Size(94, 29);
            buttonOutput.TabIndex = 5;
            buttonOutput.Text = "Locate";
            buttonOutput.UseVisualStyleBackColor = true;
            buttonOutput.Click += FolderPicker_Click;
            // 
            // outputDirectoryBox
            // 
            outputDirectoryBox.AllowDrop = true;
            outputDirectoryBox.BorderStyle = BorderStyle.FixedSingle;
            outputDirectoryBox.Location = new Point(139, 121);
            outputDirectoryBox.Name = "outputDirectoryBox";
            outputDirectoryBox.ReadOnly = true;
            outputDirectoryBox.Size = new Size(309, 27);
            outputDirectoryBox.TabIndex = 4;
            outputDirectoryBox.DragDrop += InputTextBox_DragDrop;
            outputDirectoryBox.DragEnter += InputTextBox_DragEnter;
            // 
            // output
            // 
            output.AutoSize = true;
            output.Location = new Point(78, 124);
            output.Name = "output";
            output.Size = new Size(55, 20);
            output.TabIndex = 3;
            output.Text = "Output";
            // 
            // hexSourceInput
            // 
            hexSourceInput.AllowDrop = true;
            hexSourceInput.BorderStyle = BorderStyle.FixedSingle;
            hexSourceInput.Location = new Point(139, 189);
            hexSourceInput.Name = "hexSourceInput";
            hexSourceInput.Size = new Size(309, 27);
            hexSourceInput.TabIndex = 7;
            hexSourceInput.Text = "00 03 00 04 00 63 73 6E 77 5F 69 64 30 00";
            // 
            // hexSource
            // 
            hexSource.AutoSize = true;
            hexSource.Location = new Point(41, 192);
            hexSource.Name = "hexSource";
            hexSource.Size = new Size(92, 20);
            hexSource.TabIndex = 6;
            hexSource.Text = "Hex Original";
            // 
            // hexEditInput
            // 
            hexEditInput.BorderStyle = BorderStyle.FixedSingle;
            hexEditInput.Location = new Point(139, 258);
            hexEditInput.Name = "hexEditInput";
            hexEditInput.Size = new Size(309, 27);
            hexEditInput.TabIndex = 10;
            hexEditInput.Text = "00 03 00 04 00 63 73 62 77 5F 69 64 30 00";
            // 
            // hexEdit
            // 
            hexEdit.AutoSize = true;
            hexEdit.Location = new Point(7, 261);
            hexEdit.Name = "hexEdit";
            hexEdit.Size = new Size(126, 20);
            hexEdit.TabIndex = 9;
            hexEdit.Text = "Hex Replacement";
            // 
            // replaceButton
            // 
            replaceButton.FlatStyle = FlatStyle.System;
            replaceButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            replaceButton.Location = new Point(226, 339);
            replaceButton.Name = "replaceButton";
            replaceButton.Size = new Size(153, 30);
            replaceButton.TabIndex = 11;
            replaceButton.Text = "Start Replacement";
            replaceButton.UseVisualStyleBackColor = true;
            replaceButton.Click += ReplaceButton_Click;
            // 
            // counterText
            // 
            counterText.AutoSize = true;
            counterText.Location = new Point(240, 316);
            counterText.Name = "counterText";
            counterText.Size = new Size(139, 20);
            counterText.TabIndex = 15;
            counterText.Text = "588 out of 588 files.";
            counterText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // StyleIndicator
            // 
            StyleIndicator.BackColor = SystemColors.Control;
            StyleIndicator.BackgroundImage = Properties.Resources.moon;
            StyleIndicator.BackgroundImageLayout = ImageLayout.Stretch;
            StyleIndicator.InitialImage = Properties.Resources.moon;
            StyleIndicator.Location = new Point(530, 339);
            StyleIndicator.Name = "StyleIndicator";
            StyleIndicator.Size = new Size(32, 32);
            StyleIndicator.TabIndex = 18;
            StyleIndicator.TabStop = false;
            StyleIndicator.WaitOnLoad = true;
            StyleIndicator.Click += DarkModeToggle_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(592, 398);
            Controls.Add(StyleIndicator);
            Controls.Add(counterText);
            Controls.Add(replaceButton);
            Controls.Add(hexEditInput);
            Controls.Add(hexEdit);
            Controls.Add(hexSourceInput);
            Controls.Add(hexSource);
            Controls.Add(buttonOutput);
            Controls.Add(outputDirectoryBox);
            Controls.Add(output);
            Controls.Add(buttonSource);
            Controls.Add(sourceDirectoryBox);
            Controls.Add(source);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "Hex Replacer Tool";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)StyleIndicator).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label source;
        private TextBox sourceDirectoryBox;
        private Button buttonSource;
        private Button buttonOutput;
        private TextBox outputDirectoryBox;
        private Label output;
        private TextBox hexSourceInput;
        private Label hexSource;
        private TextBox hexEditInput;
        private Label hexEdit;
        private Button replaceButton;
        private Label counterText;
        private PictureBox StyleIndicator;
    }
}