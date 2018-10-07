﻿using System.Windows;

namespace Atarashii.GUI.Detector
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Main _main;

        public MainWindow()
        {
            InitializeComponent();
            _main = (Main) DataContext;
            _main.LogWindow = new LogWindow();
            _main.LogWindow.Show();
            _main.LogWindow.Left += Width * 1.5;
        }

        private void Detect(object sender, RoutedEventArgs e)
        {
            _main.DetectExecutablePath();
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            _main.CopyToClipboard();
        }
    }
}