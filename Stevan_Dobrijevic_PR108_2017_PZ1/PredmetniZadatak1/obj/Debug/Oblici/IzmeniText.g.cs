#pragma checksum "..\..\..\Oblici\IzmeniText.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7D01CE8EE8AD0B1DCB0C6C347E5F78A6F2817EFAEC5055DAFCF195EE931AAAB1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PredmetniZadatak1.Oblici;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace PredmetniZadatak1.Oblici {
    
    
    /// <summary>
    /// IzmeniText
    /// </summary>
    public partial class IzmeniText : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\Oblici\IzmeniText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelVelicina;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Oblici\IzmeniText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelBojaTexta;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\Oblici\IzmeniText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxVelicinaTextaIzmena;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\Oblici\IzmeniText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBoxBojaTextaIzmena;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Oblici\IzmeniText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonIzadjiIzmenaTexta;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Oblici\IzmeniText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonIzmeniIzmenaTexta;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Oblici\IzmeniText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelGreskaVelicinaTextaIzmena;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Oblici\IzmeniText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelGreskaBojaTextaIzmena;
        
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
            System.Uri resourceLocater = new System.Uri("/PredmetniZadatak1;component/oblici/izmenitext.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Oblici\IzmeniText.xaml"
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
            this.labelVelicina = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.labelBojaTexta = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.TextBoxVelicinaTextaIzmena = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.comboBoxBojaTextaIzmena = ((System.Windows.Controls.ComboBox)(target));
            
            #line 15 "..\..\..\Oblici\IzmeniText.xaml"
            this.comboBoxBojaTextaIzmena.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxBojaTextaIzmena_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.buttonIzadjiIzmenaTexta = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\Oblici\IzmeniText.xaml"
            this.buttonIzadjiIzmenaTexta.Click += new System.Windows.RoutedEventHandler(this.ButtonIzadjiTextIzmena_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.buttonIzmeniIzmenaTexta = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\Oblici\IzmeniText.xaml"
            this.buttonIzmeniIzmenaTexta.Click += new System.Windows.RoutedEventHandler(this.ButtonIzmeniTextIzmena_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.labelGreskaVelicinaTextaIzmena = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.labelGreskaBojaTextaIzmena = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

