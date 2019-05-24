using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace DogOrCat.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        public static INavigation Navigation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}