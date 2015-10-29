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
using System.Windows.Shapes;
using Xbim.ModelGeometry.Scene;

namespace XbimFederationChecker
{
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(List<Result> ResultData)
        {
            InitializeComponent();
            dataGrid.ItemsSource = ResultData;
            //var r = new XbimRegion();
            //r.Centre.X
        }
    }
}
