using GMap.NET.MapProviders;
using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Tuple<int, int, int> canvasData;
        public MainWindow()
        {
            InitializeComponent();
            //WindowStartupLocation = WindowStartupLocation.CenterScreen;
            #region SetData
            int circleNodeWH = 5;
            canvasData = new Tuple<int, int, int>(
                (int)(pz2Canvas.Width / circleNodeWH), 
                (int)(pz2Canvas.Height / circleNodeWH),
                circleNodeWH);
            #endregion

            Controller.XMLLoader.LoadXml();

            Controller.XMLLoader.SetAll();

            DrawCircle();
        }

        /// <summary>
        /// Funkcija koja samo prosledjuje velicinu matrice na osnovu velicine
        /// canvas-a podaljenoj velicini kruga
        /// </summary>
        /// <returns>
        /// Item1 = Width
        /// Item2 = Height
        /// Item3 = circleNodeWH
        /// </returns>
        public static Tuple<int, int, int> CanvasData() => canvasData;

        #region Draw Circles
        private void DrawCircle()
        {
            for (int i = 0; i < CanvasData().Item1; i++)
            {
                for (int j = 0; j < CanvasData().Item2; j++)
                {
                    if (DataContainers.Containers.EntityMatrix[i, j] != null)
                    {
                        Ellipse ellipse = new Ellipse();

                        if (DataContainers.Containers.EntityMatrix[i, j].Entity != null)
                        {
                            PowerEntity pE = DataContainers.Containers.EntityMatrix[i, j].Entity;
                            ellipse.ToolTip = $"{DataContainers.Containers.EntityMatrix[i, j].TypeE.ToString()}\n" +
                                $"name: {pE.Name}\n" +
                                $"id: {pE.Id}";
                            if (pE is SwitchEntity) // ako je nasledjen tip SwitchEntity
                            {
                                ellipse.ToolTip += $"\nstatus: {(pE as SwitchEntity).Status}";
                            }
                        }
                        else
                        {
                            LineEntity lE = DataContainers.Containers.EntityMatrix[i, j].LineEntity;
                            ellipse.ToolTip = $"{DataContainers.Containers.EntityMatrix[i, j].TypeE.ToString()}\n" +
                               $"name: {lE.Name}\n" +
                               $"id: {lE.Id}";
                        }

                        ellipse.Width = CanvasData().Item3;
                        ellipse.Height = CanvasData().Item3;
                        ellipse.Stroke = DataContainers.Containers.EntityMatrix[i, j].ColorBrush;
                        ellipse.StrokeThickness = 1;
                        // ako je LineEntity onda je obojena elipsa
                        if (DataContainers.Containers.EntityMatrix[i, j].Entity == null)
                            ellipse.Fill = DataContainers.Containers.EntityMatrix[i, j].ColorBrush;
                        

                        pz2Canvas.Children.Add(ellipse);

                        Canvas.SetBottom(ellipse, i * CanvasData().Item3);
                        Canvas.SetLeft(ellipse, j * CanvasData().Item3);
                    }
                }
            }
        }
        #endregion
    }
}
