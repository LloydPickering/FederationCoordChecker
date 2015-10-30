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
        private double minX, maxX, minY, maxY, minZ, maxZ;
        private Random rnd = new Random(DateTime.Now.Millisecond);

        //private double moveX, moveY;
        public ResultWindow(List<Result> ResultData)
        {
            InitializeComponent();
            dataGrid.ItemsSource = ResultData;

            minX = 0; maxX = 0; minY = 0; maxY = 0; minZ = 0; maxZ = 0;
            XYcanvas.Children.Clear();
            YZcanvas.Children.Clear();
            ResultData.ForEach(x => SetMinMax(x));

            //Get smallest dimension of canvas
            var cMin = Math.Min(this.Width, this.Height);
            //get largest dimension of model bounds
            var modelXYMax = Math.Max(maxX,maxY)-Math.Min(minX, minY);
            var modelYZMax = Math.Max(maxZ, maxY) - Math.Min(minZ, minY);
            var scaleXY = cMin / modelXYMax;
            var scaleYZ = cMin / modelYZMax;
            //scale *= 0.9; //reduce to 90% to fit in some margins


            //draw the rectangles
            ResultData.ForEach(x => DrawItem(x, scaleXY, scaleYZ));
        }
        private void SetMinMax(Result item)
        {
            var mix = item.CentrePointX - (item.ModelSizeX / 2);
            var max = item.CentrePointX + (item.ModelSizeX / 2);
            var miy = item.CentrePointY - (item.ModelSizeY / 2);
            var may = item.CentrePointY + (item.ModelSizeY / 2);
            var miz = item.CentrePointZ - (item.ModelSizeZ / 2);
            var maz = item.CentrePointZ + (item.ModelSizeZ / 2);

            minX = minX == 0 ? mix : Math.Min(minX, mix);
            maxX = maxX == 0 ? max : Math.Max(maxX, max);
            minY = minY == 0 ? miy : Math.Min(minY, miy);
            maxY = maxY == 0 ? may : Math.Max(maxY, may);
            minZ = minZ == 0 ? miz : Math.Min(minZ, miz);
            maxZ = maxZ == 0 ? maz : Math.Max(maxZ, maz);
        }
        private void DrawItem(Result item, Double ScaleXY, Double ScaleYZ)
        {
            var mix = item.CentrePointX - (item.ModelSizeX / 2);
            var max = item.CentrePointX + (item.ModelSizeX / 2);
            var miy = item.CentrePointY - (item.ModelSizeY / 2);
            var may = item.CentrePointY + (item.ModelSizeY / 2);
            var miz = item.CentrePointZ - (item.ModelSizeZ / 2);
            var maz = item.CentrePointZ + (item.ModelSizeZ / 2);

            var modelBrush = PickBrush();

            //XY
            var rXY = new Rectangle
            {
                Stroke = modelBrush,
                StrokeThickness = 2
            };

            rXY.SetValue(Canvas.LeftProperty, ((mix - minX) * ScaleXY));
            rXY.Width = item.ModelSizeX * ScaleXY;
            rXY.SetValue(Canvas.BottomProperty, ((miy-minY) * ScaleXY));
            rXY.Height = item.ModelSizeY * ScaleXY;
            
            XYcanvas.Children.Add(rXY);
            //YZ
            var rYZ = new Rectangle
            {
                Stroke = modelBrush,
                StrokeThickness = 2
            };

            rYZ.SetValue(Canvas.LeftProperty, ((miy - minY) * ScaleYZ));
            rYZ.Width = item.ModelSizeY * ScaleYZ;
            rYZ.SetValue(Canvas.BottomProperty, ((miz - minZ) * ScaleYZ));
            rYZ.Height = item.ModelSizeZ * ScaleYZ;

            YZcanvas.Children.Add(rYZ);
        }

        private Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Type brushesType = typeof(Brushes);

            var properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }
    }
}
