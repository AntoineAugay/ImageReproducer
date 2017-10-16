using System;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using GeneticAlgorithm;


namespace ImageReproducer.ViewModel
{
    class OptionsWindowViewModel : INotifyPropertyChanged
    {
        public RelayCommand ApplyCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private Regex RegThreeDigits;

        private GAParameters Params;
        private GAEvaluatorParameters EvaluatorParams;

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

        public ObservableCollection<string> ItemsSelectionComboBox { get; set; }

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

        // Interpretation Gene Tab

        public ObservableCollection<string> ItemsColorComboBox { get; set; }

        private string _SelectedItemColorComboBox;
        public string SelectedItemColorComboBox
        {
            get
            {
                return _SelectedItemColorComboBox;
            }
            set
            {
                ApplyBtnEnable = true;
                _SelectedItemColorComboBox = value;
                int TmpGeneSize = 2;
                if (_SelectedItemColorComboBox == "GrayScale")
                    TmpGeneSize++;
                if (IsSizeVariable)
                    TmpGeneSize +=2;
                GeneSize = TmpGeneSize;
                InfoGene = InfoBuilder(SelectedItemColorComboBox, IsSizeVariable);
                RaisePropertyChanged(nameof(SelectedItemColorComboBox));
            }
        }

        public ObservableCollection<string> ItemsFormComboBox { get; set; }

        private string _SelectedItemFormComboBox;
        public string SelectedItemFormComboBox
        {
            get
            {
                return _SelectedItemFormComboBox;
            }
            set
            {
                ApplyBtnEnable = true;
                _SelectedItemFormComboBox = value;
                RaisePropertyChanged(nameof(SelectedItemFormComboBox));
            }
        }

        private bool _IsSizeVariable;
        public bool IsSizeVariable
        {
            get
            {
                return _IsSizeVariable;
            }
            set
            {
                ApplyBtnEnable = true;
                _IsSizeVariable = value;
                int TmpGeneSize = 2;
                if (_IsSizeVariable)
                {
                    GridFixedSizeVisibility    = Visibility.Hidden;
                    GridVariableSizeVisibility = Visibility.Visible;
                    TmpGeneSize += 2;
                }
                else
                {
                    GridFixedSizeVisibility = Visibility.Visible;
                    GridVariableSizeVisibility = Visibility.Hidden;
                }
                if (SelectedItemColorComboBox == "GrayScale")
                    TmpGeneSize++;
                GeneSize = TmpGeneSize;
                InfoGene = InfoBuilder(SelectedItemColorComboBox, IsSizeVariable);
                RaisePropertyChanged(nameof(IsSizeVariable));
            }
        }

        private Visibility _GridVariableSizeVisibility;
        public Visibility GridVariableSizeVisibility
        {
            get
            {
                return _GridVariableSizeVisibility;
            }
            set
            {
                ApplyBtnEnable = true;
                _GridVariableSizeVisibility = value;
                RaisePropertyChanged(nameof(GridVariableSizeVisibility));
            }
        }

        private Visibility _GridFixedSizeVisibility;
        public Visibility GridFixedSizeVisibility
        {
            get
            {
                return _GridFixedSizeVisibility;
            }
            set
            {
                ApplyBtnEnable = true;
                _GridFixedSizeVisibility = value;
                RaisePropertyChanged(nameof(GridFixedSizeVisibility));
            }
        }

        private string _SizeMinGene = "1";
        public string SizeMinGene
        {
            get
            {
                return _SizeMinGene;
            }
            set
            {
                Match Result = RegThreeDigits.Match(value);
                if (Result.Success && (int.Parse(_SizeMaxGene) >= int.Parse(value)))
                {
                    ApplyBtnEnable = true;
                    _SizeMinGene = value;
                    RaisePropertyChanged(nameof(SizeMinGene));
                }
            }
        }

        private string _SizeMaxGene = "1";
        public string SizeMaxGene
        {
            get
            {
                return _SizeMaxGene;
            }
            set
            {
                Match Result = RegThreeDigits.Match(value);
                if (Result.Success && (int.Parse(_SizeMinGene) <= int.Parse(value)))
                {
                    ApplyBtnEnable = true;
                    _SizeMaxGene = value;
                    RaisePropertyChanged(nameof(SizeMaxGene));
                }
            }
        }

        private string _HeightGene = "1";
        public string HeightGene
        {
            get
            {
                return _HeightGene;
            }
            set
            {
                Match Result = RegThreeDigits.Match(value);
                if (Result.Success)
                {
                    ApplyBtnEnable = true;
                    _HeightGene = value;
                    RaisePropertyChanged(nameof(HeightGene));
                }
            }
        }

        private string _WidthGene = "1";
        public string WidthGene
        {
            get
            {
                return _WidthGene;
            }
            set
            {
                Match Result = RegThreeDigits.Match(value);
                if (Result.Success)
                {
                    ApplyBtnEnable = true;
                    _WidthGene = value;
                    RaisePropertyChanged(nameof(WidthGene));
                }
            }
        }

        private string _InfoGene;
        public string InfoGene
        {
            get
            {
                return _InfoGene;
            }
            set
            {
                ApplyBtnEnable = true;
                _InfoGene = value;
                RaisePropertyChanged(nameof(InfoGene));
            }
        }

        private string InfoBuilder(string Color, bool IsSizeVariable)
        {
            string Info = "";
            Info += "(x, y";
            if (IsSizeVariable)
                Info += ", sizeMin, sizeMax";
            if (Color == "GrayScale")
                Info += ", Color";
            Info += ")";
            return Info;
        }


        public OptionsWindowViewModel()
        {
            ApplyCommand = new RelayCommand(Apply);
            ItemsSelectionComboBox = new ObservableCollection<string>();
            Enum.GetNames(typeof(GASelectionMethod)).ToList().ForEach( str => {
                ItemsSelectionComboBox.Add(str);
            });

            ItemsColorComboBox = new ObservableCollection<string>();
            Enum.GetNames(typeof(GAColorType)).ToList().ForEach(str => {
                ItemsColorComboBox.Add(str);
            });

            ItemsFormComboBox = new ObservableCollection<string>();
            Enum.GetNames(typeof(GAGeneForm)).ToList().ForEach(str => {
                ItemsFormComboBox.Add(str);
            });

            RegThreeDigits = new Regex(@"\d{1,3}");
        }

        public void SetParams(ref GAParameters Params, ref GAEvaluatorParameters EvaluatorParams)
        {
            this.Params = Params;
            this.EvaluatorParams = EvaluatorParams;

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

            SelectedItemColorComboBox = Enum.GetName(typeof(GAColorType), EvaluatorParams.Interpretation.Color);
            SelectedItemFormComboBox  = Enum.GetName(typeof(GAGeneForm) , EvaluatorParams.Interpretation.Form);
            IsSizeVariable = EvaluatorParams.Interpretation.IsSizeVariable;
            SizeMaxGene = EvaluatorParams.Interpretation.MaxSize.ToString();
            SizeMinGene = EvaluatorParams.Interpretation.MinSize.ToString();
            HeightGene  = EvaluatorParams.Interpretation.Height.ToString();
            WidthGene   = EvaluatorParams.Interpretation.Width.ToString();

            ApplyBtnEnable = false;
        }

        public void Apply()
        {
            Params.General.PopulationSize = _PopulationSize;
            Params.General.AdnSize = _AdnSize;
            Params.General.GeneSize = _GeneSize;
            Params.General.NumberOfGeneration = _NumberOfGenerations;

            switch (_SelectedItemSelectionComboBox)
            {
                case "TournamentSelection":
                    Params.Selection.SelectionMethod = GASelectionMethod.TournamentSelection;
                    break;
                case "WheelSelection":
                    Params.Selection.SelectionMethod = GASelectionMethod.WheelSelection;
                    break;
                default:
                    Params.Selection.SelectionMethod = GASelectionMethod.TournamentSelection;
                    break;
            }
            Params.Selection.PartOfPopulationSelected = (double)_SelectedPartPop / 100;
            Params.Selection.KeepBest = _KeepBestInd;
            Params.Selection.TournamentSize = TournamentSize;
            Params.Selection.NumberOfWinners = NumberOfWinners;

            Params.Reproduction.ReproductionProbability = (double) _ReproductionProb / 100;

            Params.Mutation.MutationProbability = _MutationProb / 100;

            switch (SelectedItemColorComboBox)
            {
                case "BlackAndWhite":
                    EvaluatorParams.Interpretation.Color = GAColorType.BlackAndWhite;
                    break;
                case "GrayScale":
                    EvaluatorParams.Interpretation.Color = GAColorType.GrayScale;
                    break;
                default:
                    EvaluatorParams.Interpretation.Color = GAColorType.BlackAndWhite;
                    break;
            }
            switch (SelectedItemFormComboBox)
            {
                case "Rectangle":
                    EvaluatorParams.Interpretation.Form = GAGeneForm.Rectangle;
                    break;
                case "Ellipse":
                    EvaluatorParams.Interpretation.Form = GAGeneForm.Ellipse;
                    break;
                default:
                    EvaluatorParams.Interpretation.Form = GAGeneForm.Rectangle;
                    break;
            }
            EvaluatorParams.Interpretation.IsSizeVariable = IsSizeVariable;
            EvaluatorParams.Interpretation.MaxSize = int.Parse(SizeMaxGene);
            EvaluatorParams.Interpretation.MinSize = int.Parse(SizeMinGene);
            EvaluatorParams.Interpretation.Width   = int.Parse(WidthGene);
            EvaluatorParams.Interpretation.Height  = int.Parse(HeightGene);
            ApplyBtnEnable = false;
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }

}
