﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using TB.Shared;
using TB.Shared.Controls;
using WpfLibrary1.ViewModels;

namespace WpfLibrary1.Views
{
    /// <summary>
    /// ForegroundWindow.xaml 的交互逻辑
    /// </summary>
    [Widget("前台窗口", "23333")]
    public partial class ForegroundWindow : MyViewBase<ForegroundViewModel>
    {
        public ForegroundWindow()
        {
            InitializeComponent();
        }
    }
}
