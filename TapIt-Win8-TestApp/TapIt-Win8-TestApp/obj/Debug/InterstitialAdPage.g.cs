﻿

#pragma checksum "D:\TapIt-Windows8\TapIt\TapIt-Win8-TestApp\TapIt-Win8-TestApp\InterstitialAdPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "43D4823C462BA61C289C2449F4ABDAD9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TapIt_Win8_TestApp
{
    partial class InterstitialAdPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 39 "..\..\InterstitialAdPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.loadBtn_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 40 "..\..\InterstitialAdPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.showBtn_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 27 "..\..\InterstitialAdPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.BackBtn_Tapped;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


