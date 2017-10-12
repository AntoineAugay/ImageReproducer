using System;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using GeneticAlgorithm;


namespace ImageReproducer.ViewModel
{
    class OptionsWindowViewModel : INotifyPropertyChanged
    {
        public RelayCommand ApplyCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private GAParameters Params;

        public ObservableCollection<string> ItemsSelectionComboBox { get; set; }
        
        private bool _ApplyBtnEnable = false;
        public bool ApplyBtnEnable
        {
            get
            {
                return _ApplyBtnEnable;
            }
            set
            {
                _ApplyBtnEnable = value;
                RaisePropertyChanged(nameof(ApplyBtnEnable));
            }
        }


        // General tab
        private int _PopulationSize = 10;
        public int PopulationSize
        {
            get
            {
                return _PopulationSize;
            }
            set
            {
                ApplyBtnEnable = true;
                _PopulationSize = value;
                RaisePropertyChanged(nameof(PopulationSize));
            }
        }

        private int _AdnSize = 10;
        public int AdnSize
        {
            get
            {
                return _AdnSize;
            }
            set
            {
                ApplyBtnEnable = true;
                _AdnSize = value;
                RaisePropertyChanged(nameof(AdnSize));
            }
        }

        private int _GeneSize = 1;
        public int GeneSize
        {
            get
            {
                return _GeneSize;
            }
            set
            {
                ApplyBtnEnable = true;
                _GeneSize = value;
                RaisePropertyChanged(nameof(GeneSize));
            }
        }

        private int _NumberOfGenerations = 1;
        public int NumberOfGenerations
        {
            get
            {
                return _NumberOfGenerations;
            }
            set
            {
                ApplyBtnEnable = true;
                _NumberOfGenerations = value;
                RaisePropertyChanged(nameof(NumberOfGenerations));
            }
        }

        // Selection tab
        private int _SelectedPartPop = 20;
        public int SelectedPartPop
        {
            get
            {
                return _SelectedPartPop;
            }
            set
            {
                ApplyBtnEnable = true;
                _SelectedPartPop = value;
                RaisePropertyChanged(nameof(SelectedPartPop));
            }
        }

        private bool _KeepBestInd = true;
        public bool KeepBestInd
        {
            get
            {
                return _KeepBestInd;
            }
            set
            {
                ApplyBtnEnable = true;
                _KeepBestInd = value;
                RaisePropertyChanged(nameof(KeepBestInd));
            }
        }

        private string _SelectedItemSelectionComboBox;
        public string SelectedItemSelectionComboBox
        {
            get
            {
                return _SelectedItemSelectionComboBox;
            }
            set
            {
                ApplyBtnEnable = true;
                _SelectedItemSelectionComboBox = value;
                
                switch (_SelectedItemSelectionComboBox)
                {
                    case "WheelSelection":
                        DisableMethodSelectionOptions();
                        WheelOptionsEnable = Visibility.Visible;
                        break;
                    case "TournamentSelection":
                        DisableMethodSelectionOptions();
                        TournamentOptionsEnable = Visibility.Visible;
                        break;
                }

                RaisePropertyChanged(nameof(SelectedItemSelectionComboBox));
            }
        }

        private int _TournamentSize = 2;
        public int TournamentSize
        {
            get
            {
                return _TournamentSize;
            }
            set
            {
                ApplyBtnEnable = true;
                _TournamentSize = value;
                if (value <= _NumberOfWinners)
                {
                    NumberOfWinners -= 1;
                }
                RaisePropertyChanged(nameof(TournamentSize));
            }
        }

        private int _NumberOfWinners = 1;
        public int NumberOfWinners
        {
            get
            {
                return _NumberOfWinners;
            }
            set
            {
                _NumberOfWinners = value;
                ApplyBtnEnable = true;
                if (value >= _TournamentSize)
                {
                    TournamentSize += 1;
                }
                RaisePropertyChanged(nameof(NumberOfWinners));
            }
        }

        private Visibility _TournamentOptionsEnable = Visibility.Hidden;
        public Visibility TournamentOptionsEnable
        {
            get
            {
                return _TournamentOptionsEnable;
            }
            set
            {
                _TournamentOptionsEnable = value;
                RaisePropertyChanged(nameof(TournamentOptionsEnable));
            }
        }

        private Visibility _WheelOptionsEnable = Visibility.Hidden;
        public Visibility WheelOptionsEnable
        {
            get
            {
                return _WheelOptionsEnable;
            }
            set
            {
                _WheelOptionsEnable = value;
                RaisePropertyChanged(nameof(WheelOptionsEnable));
            }
        }

        private void DisableMethodSelectionOptions()
        {
            WheelOptionsEnable = Visibility.Hidden;
            TournamentOptionsEnable = Visibility.Hidden;
        }

        // Reproduction tab
        private int _ReproductionProb = 10;
        public int ReproductionProb
        {
            get
            {
                return _ReproductionProb;
            }
            set
            {
                ApplyBtnEnable = true;
                _ReproductionProb = value;
                RaisePropertyChanged(nameof(ReproductionProb));
            }
        }

        // Mutation Tab
        private double _MutationProb = 0.0;
        public double MutationProb
        {
            get
            {
                return _MutationProb;
            }
            set
            {
                ApplyBtnEnable = true;
                _MutationProb = value;
                RaisePropertyChanged(nameof(MutationProb));
            }
        }


        public OptionsWindowViewModel()
        {
            ApplyCommand = new RelayCommand(Apply);
            ItemsSelectionComboBox = new ObservableCollection<string>();
            Enum.GetNames(typeof(GASelectionMethod)).ToList().ForEach( str => {
                ItemsSelectionComboBox.Add(str);
            });
        }

        public void SetParams(ref GAParameters Params)
        {
            this.Params = Params;

            PopulationSize = Params.General.PopulationSize;
            AdnSize = Params.General.AdnSize;
            GeneSize = Params.General.GeneSize;
            NumberOfGenerations = Params.General.NumberOfGeneration;

            SelectedPartPop = (int)(Params.Selection.PartOfPopulationSelected * 100);
            KeepBestInd = Params.Selection.KeepBest;
            SelectedItemSelectionComboBox = Enum.GetName(typeof(GASelectionMethod), Params.Selection.SelectionMethod);
            TournamentSize = Params.Selection.TournamentSize;
            NumberOfWinners = Params.Selection.NumberOfWinners;

            ReproductionProb = (int)(Params.Reproduction.ReproductionProbability * 100);

            MutationProb = Params.Mutation.MutationProbability*100;

            ApplyBtnEnable = false;
        }

        public void Apply()
        {
            Params.General.PopulationSize = _PopulationSize;
            Params.General.AdnSize = _AdnSize;
            Params.General.GeneSize = _GeneSize;
            Params.General.NumberOfGeneration = _NumberOfGenerations;

            Params.Selection.SelectionMethod = GASelectionMethod.TournamentSelection;
            Params.Selection.PartOfPopulationSelected = (double)_SelectedPartPop / 100;
            Params.Selection.KeepBest = _KeepBestInd;
            Params.Selection.TournamentSize = TournamentSize;
            Params.Selection.NumberOfWinners = NumberOfWinners;

            Params.Reproduction.ReproductionProbability = (double) _ReproductionProb / 100;

            Params.Mutation.MutationProbability = _MutationProb / 100;
            ApplyBtnEnable = false;
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }

}
