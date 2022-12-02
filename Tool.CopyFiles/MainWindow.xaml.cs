using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                    await FileCopyAndReplaceAsync(new FileInfo(folderPath), oldValue, newValue);
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

        public static async System.Threading.Tasks.Task FolderCopyAndReplaceAsync(DirectoryInfo oldFolder, string oldValue, string newValue)
        {
            string newFolderPath = oldFolder.FullName.Replace(oldValue, newValue);
            if (!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);
            }

            foreach (FileInfo nextFile in oldFolder.EnumerateFiles())
            {
                await FileCopyAndReplaceAsync(nextFile, oldValue, newValue);
            }

            foreach (DirectoryInfo nextFolder in oldFolder.GetDirectories())
            {
                await FolderCopyAndReplaceAsync(nextFolder, oldValue, newValue);
            }
        }

        private static async Task FileCopyAndReplaceAsync(FileInfo nextFile, string oldValue, string newValue)
        {
            string newFilePath = nextFile.FullName.Replace(oldValue, newValue);
            if (!File.Exists(newFilePath))
            {
                string oldText = await File.ReadAllTextAsync(nextFile.FullName);
                string newText = oldText.Replace(oldValue, newValue);

                await File.WriteAllTextAsync(newFilePath, newText);
            }
        }
    }
}
