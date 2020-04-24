using GMap.NET.MapProviders;
using PZ2.Controller;
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
using System.Windows.Media.Animation;
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

        private EventTrigger Enlarge10x(double height, double width)
        {
            // za sirinu
            DoubleAnimation doubleAnimation = new DoubleAnimation(width, width*10, new Duration(TimeSpan.FromMilliseconds(800)));
            doubleAnimation.AutoReverse = true; // vrati na pocetno
            doubleAnimation.AccelerationRatio = 0.5; // ubzanje
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Width"));
            
            // za visinu
            DoubleAnimation doubleAnimation2 = new DoubleAnimation(height, height * 10, new Duration(TimeSpan.FromMilliseconds(800)));
            doubleAnimation2.AutoReverse = true;
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath("Height"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            storyboard.Children.Add(doubleAnimation2);

            BeginStoryboard beginStoryboard = new BeginStoryboard();
            beginStoryboard.Storyboard = storyboard;

            EventTrigger eventTrigger = new EventTrigger();
            eventTrigger.RoutedEvent = MouseLeftButtonDownEvent; // na levi klik misa
            eventTrigger.Actions.Add(beginStoryboard);

            return eventTrigger;
        }

        #region Draw Circles
        private void DrawCircle()
        {
            int num = 0;
            foreach (var allPurpuseEntity in DataContainers.Containers.AllPurpuseEntities)
            {
                //if (allPurpuseEntity.Id == 41990 || allPurpuseEntity.Id == 41992)
                //if (allPurpuseEntity.TypeE == Model.EntityType.Node)
                {
                    ++num;
                    Ellipse ellipse = new Ellipse();
                    ToolTip toolTip = new ToolTip();
                    toolTip.Background = Brushes.Black;
                    toolTip.Foreground = Brushes.ForestGreen;
                    if (allPurpuseEntity.Entity is PowerEntity)
                    {
                        toolTip.Content = $"{allPurpuseEntity.TypeE.ToString()}\n" +
                                        $"id: {allPurpuseEntity.Entity.Id}\n" +
                                        $"name: {allPurpuseEntity.Entity.Name}";
                        if (allPurpuseEntity.Entity is SwitchEntity)
                            toolTip.Content += $"\nstatus: {(allPurpuseEntity.Entity as SwitchEntity).Status}";
                    }
                    toolTip.Content += $"\nx : {allPurpuseEntity.X}\ny : {allPurpuseEntity.Y}";
                    ellipse.ToolTip = toolTip;
                    ellipse.Width = CanvasData().Item3;
                    ellipse.Height = CanvasData().Item3;
                    ellipse.Fill = allPurpuseEntity.ColorBrush;
                    ellipse.Stroke = allPurpuseEntity.ColorBrush;
                    ellipse.StrokeThickness = 1;
                    // animation
                    ellipse.Triggers.Add(Enlarge10x(ellipse.Height, ellipse.Width));

                    pz2Canvas.Children.Add(ellipse);

                    Canvas.SetBottom(ellipse, allPurpuseEntity.X * CanvasData().Item3);
                    Canvas.SetLeft(ellipse, allPurpuseEntity.Y * CanvasData().Item3);
                }
                
            }
            
            //Graph graph = new Graph(DataContainers.Containers.AllPurpuseEntities, DataContainers.Containers.GetLines);
            //AllPurpuseEntity startVertex = DataContainers.Containers.AllPurpuseEntities.ElementAt(0);
            //var shortestPath = BFSAlg.ShortestPathFunction(graph, startVertex);
            //foreach (var vertex in DataContainers.Containers.AllPurpuseEntities)
            //{
            //    var nodes = shortestPath(vertex);
            //    if (nodes != null)
            //    {
            //        foreach (var node in nodes)
            //        {
            //            Ellipse ellipse = new Ellipse();

            //            ellipse.ToolTip = $"{node.TypeE.ToString()}\n" +
            //                $"name: {node.Entity.Name}\n" +
            //                $"id: {node.Entity.Id}";
            //            if (node.Entity is SwitchEntity) // ako je nasledjen tip SwitchEntity
            //            {
            //                ellipse.ToolTip += $"\nstatus: {(node.Entity as SwitchEntity).Status}";
            //            }
            //            ellipse.Width = CanvasData().Item3;
            //            ellipse.Height = CanvasData().Item3;
            //            ellipse.Stroke = node.ColorBrush;
            //            ellipse.StrokeThickness = 1;

            //            pz2Canvas.Children.Add(ellipse);

            //            Canvas.SetBottom(ellipse, node.X * CanvasData().Item3);
            //            Canvas.SetLeft(ellipse, node.Y * CanvasData().Item3);
            //        }
            //    }
            //}
        }
        #endregion

       
    }
}
