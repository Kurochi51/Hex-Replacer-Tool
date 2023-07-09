using System.IO;
using System.Security.AccessControl;
using System.Text;

namespace Hex_replacer
{
    public partial class Window : Form
    {
        private int counter;
        private int totalFiles;
        private bool isDarkModeEnabled;

        private List<string> failedFiles = new();
        private readonly List<string> prefixes = new()
        {
        "weapon",
        "human",
        "demihuman",
        "monster",
        "equipment",
        "accessory"
        };

        public Window()
        {
            InitializeComponent();
            AttachEventHandlersToTextBoxes();
            this.Icon = Properties.Resources.Icon;
            isDarkModeEnabled = true;
            SetDarkMode(isDarkModeEnabled);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            counterText.Text = string.Empty;
        }

        private void FolderPicker_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                if (object.ReferenceEquals(sender, buttonSource))
                {
                    sourceDirectoryBox.Text = folderBrowserDialog.SelectedPath;
                    if (!string.IsNullOrEmpty(sourceDirectoryBox.Text))
                    {
                        totalFiles = Directory.EnumerateFiles(sourceDirectoryBox.Text, "*", SearchOption.AllDirectories).Count();
                        counter = 0;
                        counterText.Text = $"{counter} out of {totalFiles} files";
                        counterText.Left += (replaceButton.Left + (replaceButton.Width / 2)) - (counterText.Left + (counterText.Width / 2));
                    }
                }
                if (object.ReferenceEquals(sender, buttonOutput))
                {
                    outputDirectoryBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void ReplaceButton_Click(object sender, EventArgs e)
        {
            if (counter != 0)
            {
                counter = 0;
                if (!string.IsNullOrEmpty(sourceDirectoryBox.Text))
                {
                    Invoke(new Action(() =>
                    {
                        counterText.Text = $"{counter} out of {totalFiles} files";
                        counterText.Left += (replaceButton.Left + (replaceButton.Width / 2)) - (counterText.Left + (counterText.Width / 2));
                    }));
                }
                else
                {
                    Invoke(new Action(() => counterText.Text = string.Empty));
                }
            }
            string directory = sourceDirectoryBox.Text.Trim();
            string output = outputDirectoryBox.Text.Trim();
            string hexSequence = hexSourceInput.Text.Trim();
            string newHexSequence = hexEditInput.Text.Trim();
            hexSequence = hexSequence.Replace(" ", "").Replace(",", "").Replace("0x", "");
            newHexSequence = newHexSequence.Replace(" ", "").Replace(",", "").Replace("0x", "");


            // Validate input
            if (string.IsNullOrEmpty(directory) || !(Directory.Exists(directory) || File.Exists(directory)))
            {
                MessageBox.Show("Please enter a valid source path.", "Invalid Source", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(output) || !Directory.Exists(output) || Directory.GetFiles(output).Length != 0)
            {
                if (Directory.GetFiles(output).Length != 0)
                {
                    DialogResult result = MessageBox.Show("Output folder isn't empty! Do you wish to continue?", "Invalid Output", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel || result == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid output path.", "Invalid Output", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (string.IsNullOrEmpty(hexSequence) || !IsValidHexSequence(hexSequence))
            {
                MessageBox.Show("Please enter a valid hex sequence.", "Invalid Hex Sequence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(newHexSequence) || !IsValidHexSequence(newHexSequence))
            {
                MessageBox.Show("Please enter a valid new hex sequence.", "Invalid New Hex Sequence", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Convert hex sequences to byte arrays
            byte[] hexSequenceBytes = HexStringToByteArray(hexSequence);
            byte[] newHexSequenceBytes = HexStringToByteArray(newHexSequence);

            // Find and replace hex sequence
            Task.Run(() =>
            {
                FindAndReplaceHexSequence(directory, output, hexSequenceBytes, newHexSequenceBytes);
                if (failedFiles.Count == 0 || failedFiles == null)
                {
                    MessageBox.Show("Hex replacement complete.", "Replacement Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Invoke(new Action(() =>
                    {
                        replaceButton.Text = "Start Replacement";
                        replaceButton.Enabled = true;
                    }));
                    counter = 0;
                }
                else if (failedFiles.Count != 0 && failedFiles.Count == totalFiles)
                {
                    foreach (var file in failedFiles)
                    {
                        File.Delete(file);
                    }
                    MessageBox.Show("Hex replacement failed.", "Replacement failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Invoke(new Action(() =>
                    {
                        replaceButton.Text = "Start Replacement";
                        replaceButton.Enabled = true;
                    }));
                    counter = 0;
                }
                else
                {
                    StringBuilder message = new();
                    message.AppendLine("Hex replacement partially incompleted.");
                    message.AppendLine($"{failedFiles.Count} files didn't contain the original sequence.");
                    foreach (var file in failedFiles)
                    {
                        bool prefixFound = false;
                        foreach (string prefix in prefixes)
                        {
                            int prefixIndex = file.IndexOf(prefix);
                            if (prefixIndex != -1 && prefixIndex + prefix.Length + 1 < file.Length)
                            {
                                string path;
                                if (file[prefixIndex + prefix.Length] == Path.DirectorySeparatorChar)
                                {
                                    path = file.Substring(prefixIndex + prefix.Length + 1);
                                }

                                else
                                {
                                    path = file.Substring(prefixIndex + prefix.Length);
                                }
                                message.AppendLine(path);
                                prefixFound = true;
                                break;
                            }
                        }
                        if (!prefixFound)
                        {
                            message.AppendLine(file);
                        }
                        File.Delete(file);
                        string parentDirectory = Path.GetDirectoryName(file);
                        while (!string.IsNullOrEmpty(parentDirectory) && Directory.GetFiles(parentDirectory).Length == 0 && Directory.GetDirectories(parentDirectory).Length == 0)
                        {
                            Directory.Delete(parentDirectory);
                            parentDirectory = Path.GetDirectoryName(parentDirectory);
                        }

                    }
                    failedFiles.Clear();
                    MessageBox.Show(message.ToString(), "Replacement incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Invoke(new Action(() =>
                    {
                        replaceButton.Text = "Start Replacement";
                        replaceButton.Enabled = true;
                    }));
                }
            });
        }

        private void FindAndReplaceHexSequence(string directory, string outputFolder, byte[] hexSequenceBytes, byte[] newHexSequenceBytes)
        {
            // If a file was used via drag-and-drop
            if (File.Exists(directory))
            {
                counter++;
                Invoke(new Action(() =>
                {
                    counterText.Text = $"{counter} out of {totalFiles} files";
                    counterText.Left += (replaceButton.Left + (replaceButton.Width / 2)) - (counterText.Left + (counterText.Width / 2));
                    replaceButton.Text = "In progress";
                    replaceButton.Enabled = false;
                }));
                FileInfo fileInfo = new(directory);
                byte[] content;
                content = File.ReadAllBytes(directory);
                DirectoryInfo directoryInfo = new(Path.GetDirectoryName(directory));
                string relativePath = fileInfo.FullName.Substring(directoryInfo.FullName.Length + 1);
                string outputFilePath = Path.Combine(outputFolder, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                byte[] updatedContent = FindAndReplace(content, hexSequenceBytes, newHexSequenceBytes, outputFilePath);
                File.WriteAllBytes(outputFilePath, updatedContent);
                SetFileAccessControl(outputFilePath);
            }
            // If a folder was used
            else if (Directory.Exists(directory))
            {
                foreach (var file in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
                {
                    counter++;
                    Invoke(new Action(() =>
                    {
                        counterText.Text = $"{counter} out of {totalFiles} files";
                        counterText.Left += (replaceButton.Left + (replaceButton.Width / 2)) - (counterText.Left + (counterText.Width / 2));
                        replaceButton.Text = "In progress";
                        replaceButton.Enabled = false;
                    }));
                    FileInfo fileInfo = new(file);
                    DirectoryInfo directoryInfo = new(directory);
                    byte[] content;
                    content = File.ReadAllBytes(file);
                    string relativePath = fileInfo.FullName.Substring(directoryInfo.FullName.Length + 1);
                    string outputFilePath = Path.Combine(outputFolder, relativePath);
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                    byte[] updatedContent = FindAndReplace(content, hexSequenceBytes, newHexSequenceBytes, outputFilePath);
                    File.WriteAllBytes(outputFilePath, updatedContent);
                    SetFileAccessControl(outputFilePath);
                }
            }
        }

        private byte[] FindAndReplace(byte[] content, byte[] hexSequenceBytes, byte[] newHexSequenceBytes, string file)
        {
            int sequence = 0;
            for (int i = 0; i < content.Length - hexSequenceBytes.Length + 1; i++)
            {
                bool found = true;
                for (int j = 0; j < hexSequenceBytes.Length; j++)
                {
                    if (content[i + j] != hexSequenceBytes[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    // Replace the hex sequence
                    for (int j = 0; j < newHexSequenceBytes.Length; j++)
                    {
                        content[i + j] = newHexSequenceBytes[j];
                    }
                    i += newHexSequenceBytes.Length - 1;
                    sequence++;
                }
            }

            if (sequence == 0)
            {
                failedFiles.Add(file);
            }

            return content;
        }

        private static byte[] HexStringToByteArray(string hexString)
        {
            int numBytes = hexString.Length / 2;
            byte[] bytes = new byte[numBytes];
            for (int i = 0; i < numBytes; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        private static bool IsValidHexSequence(string hexSequence)
        {
            if (hexSequence.Length % 2 != 0)
                return false;

            for (int i = 0; i < hexSequence.Length; i++)
            {
                if (!Uri.IsHexDigit(hexSequence[i]))
                    return false;
            }

            return true;
        }

        private static void SetFileAccessControl(string filePath)
        {
            // Get the file's existing access control settings
            FileInfo fileInfo = new(filePath);
            FileSecurity fileSecurity = fileInfo.GetAccessControl();

            // Set desired access control settings (e.g., full control for the current user)
            fileSecurity.SetAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl, AccessControlType.Allow));

            // Apply the modified access control settings to the file
            fileInfo.SetAccessControl(fileSecurity);
        }

        private void InputTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void InputTextBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                        string path = files[0];
                        if (sender == sourceDirectoryBox)
                        {
                            sourceDirectoryBox.Text = path;
                            if (File.Exists(path))
                            {
                                if (!string.IsNullOrEmpty(sourceDirectoryBox.Text))
                                {
                                    totalFiles = 1;
                                    counter = 0;
                                    counterText.Text = $"{counter} out of {totalFiles} files";
                                    counterText.Left += (replaceButton.Left + (replaceButton.Width / 2)) - (counterText.Left + (counterText.Width / 2));
                                }
                            }
                            else if (Directory.Exists(path))
                            {
                                if (!string.IsNullOrEmpty(sourceDirectoryBox.Text))
                                {
                                    totalFiles = Directory.EnumerateFiles(sourceDirectoryBox.Text, "*", SearchOption.AllDirectories).Count();
                                    counter = 0;
                                    counterText.Text = $"{counter} out of {totalFiles} files";
                                    counterText.Left += (replaceButton.Left + (replaceButton.Width / 2)) - (counterText.Left + (counterText.Width / 2));
                                }
                            }
                        }
                        else if (sender == outputDirectoryBox)
                            outputDirectoryBox.Text = path;
                    }
                }
            }
        }
        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.ForeColor = SystemColors.WindowText;
            }
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.ForeColor = SystemColors.GrayText;
            }
        }

        private void AttachEventHandlersToTextBoxes()
        {
            hexSourceInput.GotFocus += TextBox_GotFocus;
            hexSourceInput.LostFocus += TextBox_LostFocus;
            hexSourceInput.TextChanged += TextBox_TextChanged;
            hexSourceInput.Tag = "Enter lookup hex...";
            hexSourceInput.ForeColor = SystemColors.GrayText;
            hexSourceInput.Text = hexSourceInput.Tag.ToString();

            hexEditInput.GotFocus += TextBox_GotFocus;
            hexEditInput.LostFocus += TextBox_LostFocus;
            hexEditInput.TextChanged += TextBox_TextChanged;
            hexEditInput.Tag = "Enter altered hex...";
            hexEditInput.ForeColor = SystemColors.GrayText;
            hexEditInput.Text = hexEditInput.Tag.ToString();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            SetDarkMode(isDarkModeEnabled);
        }

        private void SetDarkMode(bool enabled)
        {
            if (enabled)
            {
                StyleIndicator.BackgroundImage = Properties.Resources.sun;
                StyleIndicator.BackColor = Color.Transparent;
                this.BackColor = Color.FromArgb(30, 30, 30);
                this.ForeColor = Color.White;

                foreach (Control control in this.Controls)
                {
                    if (control is Button button)
                    {
                        button.BackColor = Color.FromArgb(97, 97, 97);
                        button.ForeColor = Color.White;
                        button.FlatStyle = FlatStyle.Flat;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatAppearance.BorderColor = Color.White;
                    }
                    if (control is TextBox textBox)
                    {
                        textBox.BackColor = Color.FromArgb(40, 40, 40);
                        textBox.ForeColor = Color.White;
                    }
                }
            }
            else
            {
                // Restore default light mode colors
                StyleIndicator.BackgroundImage = Properties.Resources.moon;
                StyleIndicator.BackColor = Color.Transparent;
                this.BackColor = SystemColors.Window;
                this.ForeColor = SystemColors.WindowText;

                foreach (Control control in this.Controls)
                {
                    if (control is Button button)
                    {
                        button.BackColor = Color.FromArgb(230, 230, 230);
                        button.ForeColor = SystemColors.WindowText;
                        button.FlatStyle = FlatStyle.Flat;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatAppearance.BorderColor = Color.Black;
                    }
                    if (control is TextBox textBox)
                    {
                        textBox.BackColor = SystemColors.Window;
                        textBox.ForeColor = SystemColors.WindowText;
                    }
                }
            }
        }

        private void DarkModeToggle_Click(object sender, EventArgs e)
        {
            isDarkModeEnabled = !isDarkModeEnabled;
            SetDarkMode(isDarkModeEnabled);
        }
    }
}