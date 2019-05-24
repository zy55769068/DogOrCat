using System;
using System.Collections.Generic;
using DogOrCat.ViewModels;
using Xamarin.Forms;

namespace DogOrCat.Views
{
    public partial class ResultView : ContentPage
    {
        public ResultView(ResultViewModel resultviewModel)
        {
            InitializeComponent();
            BindingContext = resultviewModel;
        }
    }
}
