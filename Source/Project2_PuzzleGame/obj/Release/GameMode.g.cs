﻿#pragma checksum "..\..\GameMode.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "99C63EBA9B2065BAA93AC6EF68F3659382D46A9E781D943F25F7FA255E91CC47"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Project2_PuzzleGame;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Project2_PuzzleGame {
    
    
    /// <summary>
    /// GameMode
    /// </summary>
    public partial class GameMode : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\GameMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton Unlimited;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\GameMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton Three_Minutes;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\GameMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton EasyMode;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\GameMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton MediumMode;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\GameMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton DifficultMode;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\GameMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton UserMode;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\GameMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox UserMode_TextBox;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\GameMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OK;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Project2_PuzzleGame;component/gamemode.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\GameMode.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Unlimited = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 2:
            this.Three_Minutes = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 3:
            this.EasyMode = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.MediumMode = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.DifficultMode = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.UserMode = ((System.Windows.Controls.RadioButton)(target));
            
            #line 18 "..\..\GameMode.xaml"
            this.UserMode.Checked += new System.Windows.RoutedEventHandler(this.UserMode_Checked);
            
            #line default
            #line hidden
            
            #line 18 "..\..\GameMode.xaml"
            this.UserMode.Unchecked += new System.Windows.RoutedEventHandler(this.UserMode_Unchecked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.UserMode_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.OK = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\GameMode.xaml"
            this.OK.Click += new System.Windows.RoutedEventHandler(this.OK_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

