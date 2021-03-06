﻿using System;
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
using Gamology.UserInterface.Utils.Helpers;
using Microsoft.Practices.Unity;

namespace Gamology.UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameEditorMainWindow : Window
    {
        public GameEditorMainWindow()
        {
            InitializeComponent();
            this.DataContext = ContainerHelper.Container.Resolve<GameEditorMainWindowViewModel>();
        }
    }
}
