using System.Windows;
using ImageReproducer.ViewModel;

namespace ImageReproducer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            var MainViewModel = DataContext as MainWindowViewModel;
            MainViewModel.SetBestImg(BestImg);
        }

    }

}
