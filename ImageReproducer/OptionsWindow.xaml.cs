using System.Windows;
using ImageReproducer.ViewModel;
using GeneticAlgorithm;

namespace ImageReproducer
{
    /// <summary>
    /// Logique d'interaction pour OptionWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow(ref GAParameters Params, ref GAEvaluatorParameters EvaluatorParams)
        {
            InitializeComponent();
            var OptionsViewModel = DataContext as OptionsWindowViewModel;
            OptionsViewModel.SetParams(ref Params, ref EvaluatorParams);
        }
    }
}
