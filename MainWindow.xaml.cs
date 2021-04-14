﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Xml;

namespace Thimblerigger
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thimble[] thimbles = new Thimble[3];
        private int sizeThimble = 120;
        private static Random random = new Random();
        private int difficulty = 1000 / 3;
        private int time = 1000;
        private int ballIndex;
        private DispatcherTimer dispatcherTimer;
        private int count = 0;
        private int requiredCount;
        public MainWindow()
        {
            InitializeComponent();

            menu.Visibility = Visibility.Visible;

            menu.playButton.Click += playButton_Click;
            menu.settingsButton.Click += settingsButton_Click;
            menu.exitButton.Click += exitButton_Click;

            settings.easy.Checked += RadioButton_Checked;
            settings.normal.Checked += RadioButton_Checked;
            settings.hard.Checked += RadioButton_Checked;
            settings.menuButton.Click += menuButton_Click;
            
            victory.playButton.Click += playButton_Click;
            victory.menuButton.Click += menuButton_Click;

            defeat.playButton.Click += playButton_Click;
            defeat.menuButton.Click += menuButton_Click;


            addThimbles();
        }

        private void addThimbles()
        {
            for (int i = 0; i < thimbles.Length; i++)
            {
                thimbles[i] = new Thimble();
                thimbles[i].Width = thimbles[i].Height = sizeThimble;
                thimbles[i].MouseLeftButtonDown += thimbles_MouseLeftButtonDown;

                Canvas.SetLeft(thimbles[i], sizeThimble * i);
                Canvas.SetTop(thimbles[i], 0);
                canvas.Children.Add(thimbles[i]);
            }
            ballIndex = random.Next(0, 3);
            thimbles[ballIndex].ball = true;
            thimbles[ballIndex].thimble.Opacity = 0.5;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void clickToContinue_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickToContinue.Visibility = Visibility.Collapsed;
            //for (int i = 0; i < thimbles.Length; i++)
            //{
            //    double temp = (thimbles[i].Width + 1) * i;

            //    if (horizontal == true) Moving.MoveTo(thimbles[i], temp, 0);
            //    else Moving.MoveTo(canvas, 0, temp);
            //}

            //horizontal = !horizontal;
            thimbles[ballIndex].thimble.Opacity = 1;

            count = 0;
            requiredCount = random.Next(10, 20);

            dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(difficulty) };
            dispatcherTimer.Tick += Timer_Tick;
            dispatcherTimer.Start();
        }

        void swapElements(int elementA, int elementB)
        {
            animation(elementA, elementB);
            animation(elementB, elementA);

            Thimble temp = thimbles[elementA];
            thimbles[elementA] = thimbles[elementB];
            thimbles[elementB] = temp;
        }

        void animation(int elementA, int elementB)
        {
            TranslateTransform translateTransformA = new TranslateTransform();
            thimbles[elementA].RenderTransform = translateTransformA;
            DoubleAnimation animationA = new DoubleAnimation(thimbles[elementA].position, thimbles[elementA].position + sizeThimble * (elementB - elementA), TimeSpan.FromMilliseconds(difficulty));
            thimbles[elementA].position = thimbles[elementA].position + sizeThimble * (elementB - elementA);
            translateTransformA.BeginAnimation(TranslateTransform.XProperty, animationA);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int a = random.Next(0, 3);
            int b = random.Next(0, 3);

            while (true)
            {
                if (a == b)
                {
                    a = random.Next(0, 3);
                    b = random.Next(0, 3);

                }
                else
                    break;
            }
            swapElements(a, b);

            //if ((a = random.Next(0, 3)) == (b = random.Next(0, 3)))
            //{
            //    a = random.Next(0, 3);
            //    b = random.Next(0, 3);

            //    Console.WriteLine(a + " " + b);
            //}
            //else
            //    swapElements(a, b);


            if (count == requiredCount)
            {
                dispatcherTimer.Stop();
                Thimble.finish = true;
            }
            else
                count++;
        }
        private void thimbles_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Thimble)sender).ball && Thimble.finish)
                victoryEvent((Thimble)sender);
            else if (!((Thimble)sender).ball && Thimble.finish)
                defeatEvent();
        }

        private void defeatEvent()
        {
            defeat.Visibility = Visibility.Visible;
        }

        private void victoryEvent(Thimble thimble)
        {
            thimble.thimble.Opacity = 0.5;
            victory.Visibility = Visibility.Visible;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            addThimbles();

            clickToContinue.Visibility = Visibility.Visible;

            menu.Visibility = Visibility.Collapsed;
            victory.Visibility = Visibility.Collapsed;
            defeat.Visibility = Visibility.Collapsed;
        }

        private void menuButton_Click(object sender, RoutedEventArgs e)
        {
            settings.Visibility = Visibility.Collapsed;
            victory.Visibility = Visibility.Collapsed;
            defeat.Visibility = Visibility.Collapsed;

            menu.Visibility = Visibility.Visible;
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Collapsed;
            settings.Visibility = Visibility.Visible;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(((RadioButton)sender).Content);
            switch(((RadioButton)sender).Content)
            {
                case "Easy":
                    difficulty = time / 2;
                    break;
                case "Normal":
                    difficulty = time / 3;
                    break;
                case "Hard":
                    difficulty = time / 4;
                    break;
            }
        }
    }
}
