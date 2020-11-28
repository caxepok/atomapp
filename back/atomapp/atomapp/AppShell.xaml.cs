using atomapp.ViewModels;
using atomapp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace atomapp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MakeRecordPage), typeof(MakeRecordPage));
        }

    }
}
