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
        private double minX, maxX, minY, maxY;
        private double moveX, moveY;
        public ResultWindow(List<Result> ResultData)
        {
            InitializeComponent();
            dataGrid.ItemsSource = ResultData;

            minX = 0; maxX = 0; minY = 0; maxY = 0;
            canvas.Children.Clear();
            ResultData.ForEach(x => SetMinMax(x));

            //Get smallest dimension of canvas
            var cMin = Math.Min(this.Width, this.Height);
            //get largest dimension of model bounds
            var modelMax = Math.Max(maxX,maxY)-Math.Min(minX, minY);
            var scale = cMin / modelMax;
            scale *= 0.9; //reduce to 90% to fit in some margins

            //draw the rectangles
            ResultData.ForEach(x => DrawItem(x, scale));
        }
        private void SetMinMax(Result item)
        {
            var mix = item.CentrePointX - (item.ModelSizeX / 2);
            var max = item.CentrePointX + (item.ModelSizeX / 2);
            var miy = item.CentrePointY - (item.ModelSizeY / 2);
            var may = item.CentrePointY + (item.ModelSizeY / 2);

            minX = minX == 0 ? mix : Math.Min(minX, mix);
            maxX = maxX == 0 ? max : Math.Max(maxX, max);
            minY = minY == 0 ? miy : Math.Min(minY, miy);
            maxY = maxY == 0 ? may : Math.Max(maxY, may);

            moveX = (this.Width / 2) - (maxX - minX);
            moveY = (this.Height / 2) - (maxY - minY);
        }
        private void DrawItem(Result item, Double Scale)
        {
            var mix = item.CentrePointX - (item.ModelSizeX / 2);
            var max = item.CentrePointX + (item.ModelSizeX / 2);
            var miy = item.CentrePointY - (item.ModelSizeY / 2);
            var may = item.CentrePointY + (item.ModelSizeY / 2);

            var r = new Rectangle
            {
                Stroke = Brushes.LightBlue,
                StrokeThickness = 2
            };
            r.SetValue(Canvas.BottomProperty, moveY - (miy * Scale));
            r.SetValue(Canvas.LeftProperty, moveX - (mix * Scale));
            r.Width = item.ModelSizeY * Scale;
            r.Height = item.ModelSizeX * Scale;
            
            canvas.Children.Add(r);
        }
    }
}
