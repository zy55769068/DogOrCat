namespace DogOrCat.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new DogOrCat.App());
        }
    }
}