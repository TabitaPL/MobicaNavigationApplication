﻿#pragma checksum "C:\Users\Agusia\documents\visual studio 2010\Projects\Mobica\Mobica\OptionsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5D29DD9ED219849FAB8D43E63FD23755"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.2012
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Mobica {
    
    
    public partial class OptionsPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock RouteModeLabel;
        
        internal System.Windows.Controls.RadioButton DrivingRadioButton;
        
        internal System.Windows.Controls.RadioButton WalkingRadioButton;
        
        internal System.Windows.Controls.Button SaveButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Mobica;component/OptionsPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.RouteModeLabel = ((System.Windows.Controls.TextBlock)(this.FindName("RouteModeLabel")));
            this.DrivingRadioButton = ((System.Windows.Controls.RadioButton)(this.FindName("DrivingRadioButton")));
            this.WalkingRadioButton = ((System.Windows.Controls.RadioButton)(this.FindName("WalkingRadioButton")));
            this.SaveButton = ((System.Windows.Controls.Button)(this.FindName("SaveButton")));
        }
    }
}
