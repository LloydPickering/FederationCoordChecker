using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using Xbim.ModelGeometry.Scene;

namespace XbimFederationChecker
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

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.SelectedPath = textBox.Text;
            dlg.ShowNewFolderButton = true;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox.Text = dlg.SelectedPath;
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            var dir = textBox.Text;
            if (!Directory.Exists(dir))
            {
                MessageBox.Show("Please choose a Directory that exists before clicking Start");
                return;
            }

            var worker = new BackgroundWorker {
                WorkerReportsProgress = true
            };
            worker.ProgressChanged += delegate (object s, ProgressChangedEventArgs args)
            {
                status.Text = String.Format("{0}% {1}", args.ProgressPercentage, (string)args.UserState);
            };
            worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
            {
                var Results = args.Result as List<Result>;
                var win = new ResultWindow(Results);
                win.Show();
            };

            worker.DoWork += Process;
            worker.RunWorkerAsync(dir);
            
        }
        void Process(object w, DoWorkEventArgs args)
        {
            var worker = w as BackgroundWorker;
            var dir = args.Argument as string;
            var dirInfo = new DirectoryInfo(dir);

            var Results = new List<Result>();

            foreach (var f in dirInfo.EnumerateFiles("*.*").Where(s => s.Extension == ".ifc" || s.Extension == ".ifczip"))
            {
                worker.ReportProgress(0, String.Format("Starting Processing of {0}", f.Name));
                using (var m = new Xbim.IO.XbimModel())
                {
                    m.CreateFrom(f.FullName, System.IO.Path.ChangeExtension(f.FullName, ".xbim"), worker.ReportProgress, true, true);
                    var m3d = new Xbim.ModelGeometry.Scene.Xbim3DModelContext(m);
                    m3d.CreateContext(XbimGeometry.Interfaces.XbimGeometryType.PolyhedronBinary, worker.ReportProgress);
                    var region = m3d.GetLargestRegion();
                    Results.Add(new Result(f.Name, region));
                }
                worker.ReportProgress(0, String.Format("Completed Processing of {0}", f.Name));
            }
            worker.ReportProgress(100, "Processing Complete.");

            args.Result = Results;
        }
    }
}
