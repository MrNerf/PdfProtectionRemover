using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace PdfProtectionRemoverGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string[]? _fileNamesStrings;

        public MainWindow()
        {
            InitializeComponent();
            _fileNamesStrings = null;
        }

        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            FilesListBox.Items.Clear();
            
            var openFileDialog = PdfOpenFileDialog();

            if (openFileDialog.ShowDialog() != true)
                return;

            _fileNamesStrings = openFileDialog.FileNames;
            
            foreach (var fileName in openFileDialog.FileNames) FilesListBox.Items.Add(Path.GetFileName(fileName));
            RemoveProtectionButton.IsEnabled = true;
        }
        
        private async void RemoveProtectionButton_Click(object sender, RoutedEventArgs e)
        {
            StatusBar.Value = 0.0;

            var saveFileDialog = PdfSaveFileDialog();
            if (saveFileDialog.ShowDialog() != true)
                return;

            if (_fileNamesStrings == null) return;
            for (var cnt = 0; cnt < _fileNamesStrings.Length; ++cnt)
            {
                await ExecuteTask(_fileNamesStrings[cnt], Path.Combine(Path.GetDirectoryName(saveFileDialog.FileName) ?? string.Empty, 
                                                                                                              "U_" + (string)FilesListBox.Items[cnt]));
                StatusBar.Value += 100.0 / _fileNamesStrings.Length;
            }
        }

        private static OpenFileDialog PdfOpenFileDialog() => new OpenFileDialog()
        {
            CheckFileExists = true,
            CheckPathExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            DefaultExt = "*.pdf",
            Filter = "PDF files (*.pdf) | *.pdf",
            Multiselect = true,
            Title = "Выберете защищенные pdf файлы"
        };

        private static SaveFileDialog PdfSaveFileDialog() => new SaveFileDialog()
        {
            Title = "Выберите директорию для сохранения",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            FileName = "FileName will generate automatically",
            DefaultExt = ".pdf",
            Filter = "PDF files (*.pdf) | *.pdf"
            
        };

        private static Task ExecuteTask(string inputFileName, string outputFileName) => Task.Run(() => PdfSecurityRemover.SecurityRemove(inputFileName, outputFileName));
    }
}
