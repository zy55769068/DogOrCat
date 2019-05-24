using System;
using System.Collections.Generic;
using DogOrCat.ViewModels;
using Xamarin.Forms;

namespace DogOrCat.Views
{
    public partial class MainView : ContentPage
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
