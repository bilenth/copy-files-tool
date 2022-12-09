using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace DDDToolWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = tbFolderPath.Text;
            string oldValue = tbOldValue.Text;
            string newValue = tbNewValue.Text;

            try
            {
                if (File.Exists(folderPath))
                {
                    FileInfo oldFile = new FileInfo(folderPath);
                    await FileCopyAndReplaceAsync(oldFile.DirectoryName, oldFile, oldValue, newValue);
                }
                else if (Directory.Exists(folderPath))
                {
                    await FolderCopyAndReplaceAsync(new DirectoryInfo(folderPath), oldValue, newValue);
                }
                else
                {
                    Console.WriteLine("Folder Path no exists");
                }
                lblMessage.Content = "Successful.";
                lblMessage.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.ToString();
                lblMessage.Visibility = Visibility.Visible;
            }

        }

        public async Task FolderCopyAndReplaceAsync(DirectoryInfo oldFolder, string oldValue, string newValue)
        {
            string newFolderPath = oldFolder.FullName.Replace(oldValue, newValue);
            if (!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);
            }

            foreach (FileInfo subOldFile in oldFolder.EnumerateFiles())
            {
                await FileCopyAndReplaceAsync(newFolderPath, subOldFile, oldValue, newValue);
            }

            foreach (DirectoryInfo subOldFolder in oldFolder.GetDirectories())
            {
                await FolderCopyAndReplaceAsync(subOldFolder, oldValue, newValue);
            }
        }

        private static async Task FileCopyAndReplaceAsync(string targetFolder, FileInfo oldFile, string oldValue, string newValue)
        {
            string newFilePath = Path.Combine(targetFolder, oldFile.Name.Replace(oldValue, newValue));
            if (!File.Exists(newFilePath))
            {
                string oldText = await File.ReadAllTextAsync(oldFile.FullName);
                string newText = oldText.Replace(oldValue, newValue);

                await File.WriteAllTextAsync(newFilePath, newText);
            }
        }
    }
}
