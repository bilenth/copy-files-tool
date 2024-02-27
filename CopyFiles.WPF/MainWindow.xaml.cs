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
            string folderPath = Path.GetFullPath(tbFolderPath.Text);
            string oldValue = tbOldValue.Text;
            string newValue = tbNewValue.Text;

            var message = await CopyFiles.Core.CopyFiles.RunAsync(folderPath, oldValue, newValue);

            lblMessage.Content = message;
            lblMessage.Visibility = Visibility.Visible;
        }

    }
}
