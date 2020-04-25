﻿using GMap.NET.MapProviders;
using PZ2.Controller;
using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            DrawLine();
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
        private void DrawLine(List<AllPurpuseEntity> path)
        {
            //var ellipsesLine = XMLLoader.SetLine(path);
        }
        
        private void DrawCircle()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int num = 0;
            string elapsedTime = "";
            foreach (var allPurpuseEntity in DataContainers.Containers.AllPurpuseEntities)
            {
                // za BFS
                //var connectedNodes = BFSAlg.BFS(DataContainers.Containers.Graph, allPurpuseEntity);
                //var visitedNodes = BFSAlg.VisitedNodes(DataContainers.Containers.Graph, allPurpuseEntity);
                //foreach (var visitedNode in visitedNodes)
                //{
                //    var path = BFSAlg.Path(connectedNodes, allPurpuseEntity, visitedNode);
                //    if (path.Count > 1) // necemo da nista da radimo ako nema putanje
                //        DrawLine(path);
                //}
                //
                
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

                Canvas.SetLeft(ellipse, allPurpuseEntity.X * CanvasData().Item3);
                Canvas.SetTop(ellipse, allPurpuseEntity.Y * CanvasData().Item3);

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                            ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10);
                
            }

            //XMLLoader.DrawLine();
            elapsedTime += ".";
        }
        #endregion

        private Tuple<AllPurpuseEntity, AllPurpuseEntity> GetFSEnds(long firstId, long secondId)
        {
            AllPurpuseEntity first = null;
            bool foundedFist = false;
            AllPurpuseEntity second = null;
            bool foundedSecond = false;
            for (int i = 0; i < DataContainers.Containers.AllPurpuseEntities.Count; ++i)
            {
                if (foundedFist == true && foundedSecond == true) break;

                if (DataContainers.Containers.AllPurpuseEntities.ElementAt(i).Id == firstId)
                {
                    first = DataContainers.Containers.AllPurpuseEntities.ElementAt(i);
                    foundedFist = true;
                    continue;
                }

                if (DataContainers.Containers.AllPurpuseEntities.ElementAt(i).Id == secondId)
                {
                    second = DataContainers.Containers.AllPurpuseEntities.ElementAt(i);
                    foundedSecond = true;
                }
            }
            return new Tuple<AllPurpuseEntity, AllPurpuseEntity>(first, second);
        }

        private bool CheckCoords(int x, int y)
        {
            Coord coord = new Coord(x, y);
            if (!DataContainers.Containers.AllCord.Any(i => i.X == coord.X && i.Y == coord.Y))
            {
                DataContainers.Containers.AllCord.Add(coord);
                return true;
            }
            return false;
        }

        private void DrawEllipse(LineEntity line, int x, int y)
        {
            Ellipse ellipse = new Ellipse();
            ToolTip toolTip = new ToolTip();
            toolTip.Background = Brushes.Black;
            toolTip.Foreground = Brushes.ForestGreen;
            toolTip.Content = $"id: {line.Id}\n" +
                            $"name: {line.Name}";
            toolTip.Content += $"\nx : {x}\ny : {y}";
            ellipse.ToolTip = toolTip;
            ellipse.Width = CanvasData().Item3;
            ellipse.Height = CanvasData().Item3;
            ellipse.Fill = System.Windows.Media.Brushes.Black;
            ellipse.Stroke = System.Windows.Media.Brushes.Black;
            ellipse.StrokeThickness = 1;

            pz2Canvas.Children.Add(ellipse);

            Canvas.SetLeft(ellipse, x * CanvasData().Item3);
            Canvas.SetTop(ellipse, y * CanvasData().Item3);
        }

        public void DrawLine()
        {
            int num = 0;
            foreach (var line in DataContainers.Containers.GetLines)
            {
                ++num;
                long firstEnd = line.FirstEnd;
                long secondEnd = line.SecondEnd;

                var tupple = GetFSEnds(firstEnd, secondEnd);
                // ako node nije povezan
                if (tupple.Item1 == null || tupple.Item2 == null)
                    continue;

                int x1 = tupple.Item1.X;
                int y1 = tupple.Item1.Y;
                int x2 = tupple.Item2.X;
                int y2 = tupple.Item2.Y;

                // ako su u istoj koloni
                if (x1 == x2)
                {
                    if (y1 > y2)
                    {
                        for (int y = y1; y > y2; y--)
                        {
                            if (CheckCoords(x1, y))
                            {
                                DrawEllipse(line, x1, y);
                            }
                        }
                    }
                    else if (y1 < y2)
                    {
                        for (int y = 0; y < y2; y++)
                        {
                            if (CheckCoords(x1, y))
                            {
                                DrawEllipse(line, x1, y);
                            }
                        }
                    }
                }

                // ako su u istom redu
                if (y1 == y2)
                {
                    if (x1 > x2)
                    {
                        for (int x = x1; x > x2; x--)
                        {
                            if (CheckCoords(x, y1))
                            {
                                DrawEllipse(line, x, y1);
                            }
                        }
                    }
                    else if (x1 < x2)
                    {
                        for (int x = x1; x < x2; x++)
                        {
                            if (CheckCoords(x, y1))
                            {
                                DrawEllipse(line, x, y1);
                            }
                        }
                    }
                }

                // I kvadrant
                if (x1 < x2 && y1 > y2)
                {
                    int x = x1;
                    for (; x < x2; x++)
                    {
                        if (CheckCoords(x, y1))
                        {
                            DrawEllipse(line, x, y1);
                        }
                    }
                    for (int y = y1; y > y2; y--)
                    {
                        if (CheckCoords(x, y))
                        {
                            DrawEllipse(line, x, y);
                        }
                    }
                }

                // II kvadrant
                if (x1 > x2 && y1 > y2)
                {
                    int x = x1;
                    for (; x > x2; x--)
                    {
                        if (CheckCoords(x, y1))
                        {
                            DrawEllipse(line, x, y1);
                        }
                    }
                    for (int y = y1; y > y2; y--)
                    {
                        if (CheckCoords(x, y))
                        {
                            DrawEllipse(line, x, y);
                        }
                    }
                }

                // III kvadrant
                if (x1 > x2 && y1 < y2)
                {
                    int x = x1;
                    for (; x > x2; x--)
                    {
                        if (CheckCoords(x, y1))
                        {
                            DrawEllipse(line, x, y1);
                        }
                    }
                    for (int y = y1; y < y2; y++)
                    {
                        if (CheckCoords(x, y))
                        {
                            DrawEllipse(line, x, y);
                        }
                    }
                }

                // IV kvadrant
                if (x1 < x2 && y1 < y2)
                {
                    int x = x1;
                    for (; x < x2; x++)
                    {
                        if (CheckCoords(x, y1))
                        {
                            DrawEllipse(line, x, y1);
                        }
                    }
                    for (int y = y1; y < y2; y++)
                    {
                        if (CheckCoords(x, y))
                        {
                            DrawEllipse(line, x, y);
                        }
                    }
                }
            }
        }
    }
}
