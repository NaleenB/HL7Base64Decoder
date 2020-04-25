using HL7Base64Decoder.ProcessingLogic;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace HL7Base64Decoder
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private void btn_InputPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                lbl_InputPath.Content = fbd.SelectedPath;
                string[] files = Directory.GetFiles(fbd.SelectedPath);
                tb_AppOutput.Text = (files.Length + " files found in input path." + Environment.NewLine);
            }
        }

        private void btn_OutputPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                lbl_OutputPath.Content = fbd.SelectedPath;
            }
        }

        private void btn_DecodeSave_Click(object sender, RoutedEventArgs e)
        {
            string inputPath = (string)lbl_InputPath.Content;
            string outputPath = (string)lbl_OutputPath.Content;

            if (string.IsNullOrEmpty(inputPath) || string.IsNullOrEmpty(outputPath))
            {
                tb_AppOutput.AppendText("Input Path and Output Path cannot be empty." + Environment.NewLine);
                return;
            }

            string[] files = Directory.GetFiles(inputPath);
            if (files.Length == 0)
            {
                tb_AppOutput.AppendText("No files found in input path." + Environment.NewLine);
                return;
            }

            string outputLog;
            FileHandler.ProcessFiles(inputPath, outputPath, out outputLog);

            tb_AppOutput.AppendText(outputLog);
        }
    }
}
