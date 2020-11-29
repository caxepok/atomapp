using atomapp.Models;
using atomapp.Services;
using atomapp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atomapp.Views
{
    public partial class MakeRecordPage : ContentPage
    {
        public Record Item { get; set; }

        public MakeRecordPage()
        {
            InitializeComponent();
            BindingContext = new MakeRecordPageViewModel(new ApiService());
        }
    }
}