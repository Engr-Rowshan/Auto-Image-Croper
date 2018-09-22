using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.IO;

namespace Auto_Image_Croper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _path;
        private FileSystemWatcher fw;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if(fw != null)
            {
                fw.Dispose();
                btnWatch.IsEnabled = false;
            }

            //This will show a directory selection dialog box
            using (var fdb = new FolderBrowserDialog())
            {
                fdb.Description = "Select the directory to watch";
                fdb.ShowNewFolderButton = true;
                
                DialogResult result = fdb.ShowDialog();

                if(result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fdb.SelectedPath))
                {
                    try
                    {
                        System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(fdb.SelectedPath);
                        _path = fdb.SelectedPath;
                        txtWatchPath.Text = _path;
                        btnWatch.IsEnabled = true;
                        btnWatch.Content = "Start Monitoring";
                    }
                    catch (UnauthorizedAccessException)
                    {
                        System.Windows.MessageBox.Show("Access Denied in that folder. Please choice another path.","Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                        btnWatch.IsEnabled = false;
                    }
                }

            }
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            if( ! String.IsNullOrEmpty(_path))
            {
                if(fw == null)
                {
                    fw = new FileSystemWatcher();
                    fw.Path = _path;
                    fw.Filter = "*.*";
                    fw.Created += new FileSystemEventHandler(NewFileFound);

                    fw.EnableRaisingEvents = true;

                    //UI Update
                    btnBrowse.IsEnabled = false;
                    btnWatch.Content = "Stop Monitoring";


                    lstLog.Items.Clear();
                    lstLog.Items.Add("Stated watching.");
                }
                else
                {
                    fw.EnableRaisingEvents = false;
                    fw.Dispose();
                    fw = null;

                    //UI Update
                    btnBrowse.IsEnabled = true;
                    btnWatch.Content = "Start Monitoring";
                    lstLog.Items.Add("Stopped watching.");

                }
            }
            else
            {
                System.Windows.MessageBox.Show("Select a valid folder first","Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewFileFound(object source, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                lstLog.Items.Add("New file added: " + e.Name);
            });
        }
    }
}
