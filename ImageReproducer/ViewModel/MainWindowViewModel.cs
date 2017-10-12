using System;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using GeneticAlgorithm;


namespace ImageReproducer.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public RelayCommand OptionsCommand { get; private set; }
        public RelayCommand LoadCommand { get; private set; }
        public RelayCommand StartStopCommand { get; private set; }

        private ImageBox BestImg;

        private ConcurrentQueue<ThreadMessage> ImageQueue = new ConcurrentQueue<ThreadMessage>();
        private Thread ThreadMessageProcess;
        private GAParameters Params;
        private GAThreadHandle GAImageReproducerThreadHandler;
        private Thread GAThread;
        private CancellationTokenSource CancelTokenSource;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _ButtonStartStopName = "Start";
        public string ButtonStartStopName
        {
            get
            {
                return _ButtonStartStopName;
            }
            set
            {
                _ButtonStartStopName = value;
                RaisePropertyChanged(nameof(ButtonStartStopName));
            }
        }

        private bool _DisableButtons = false;
        public bool DisableButtons
        {
            get
            {
                return _DisableButtons;
            }
            set
            {
                _DisableButtons = value;
                RaisePropertyChanged(nameof(DisableButtons));
            }
        }

        private string _TargetImgPath = "";
        public string TargetImgPath
        {
            get
            {
                return _TargetImgPath;
            }
            set
            {
                _TargetImgPath = value;
                RaisePropertyChanged(nameof(TargetImgPath));
            }
        }

        private int _CurrentGeneration = 0;
        public int CurrentGeneration
        {
            get
            {
                return _CurrentGeneration;
            }
            set
            {
                _CurrentGeneration = value;
                RaisePropertyChanged(nameof(CurrentGeneration));
            }
        }

        private int _ImageWidth = 100;
        public int ImageWidth
        {
            get
            {
                return _ImageWidth;
            }
            set
            {
                _ImageWidth = value;
                RaisePropertyChanged(nameof(ImageWidth));
            }
        }

        private int _ImageHeight = 100;
        public int ImageHeight
        {
            get
            {
                return _ImageHeight;
            }
            set
            {
                _ImageHeight = value;
                RaisePropertyChanged(nameof(ImageHeight));
            }
        }

        private double _TotalElapseTime = 0;
        public double TotalElapseTime
        {
            get
            {
                return _TotalElapseTime;
            }
            set
            {
                _TotalElapseTime = value;
                RaisePropertyChanged(nameof(TotalElapseTime));
            }
        }

        private double _GenerationMeanTime = 0;
        public double GenerationMeanTime
        {
            get
            {
                return _GenerationMeanTime;
            }
            set
            {
                _GenerationMeanTime = value;
                RaisePropertyChanged(nameof(GenerationMeanTime));
            }
        }

        private int _BestFitness = 0;
        public int BestFitness
        {
            get
            {
                return _BestFitness;
            }
            set
            {
                _BestFitness = value;
                RaisePropertyChanged(nameof(BestFitness));
            }
        }

        public MainWindowViewModel()
        {
            OptionsCommand = new RelayCommand(Options);
            LoadCommand = new RelayCommand(Load);
            StartStopCommand = new RelayCommand(StartStop);
            Params = new GAParameters();
            SetDefaultParams();
        }

        private void SetDefaultParams()
        {
            Params.General.PopulationSize = 100;
            Params.General.AdnSize = 50;
            Params.General.GeneSize = 2;
            Params.General.InfiniteGeneration = false;
            Params.General.NumberOfGeneration = 1000;

            Params.Selection.SelectionMethod = GASelectionMethod.TournamentSelection;
            Params.Selection.PartOfPopulationSelected = 0.3;
            Params.Selection.KeepBest = true;
            Params.Selection.TournamentSize = 2;
            Params.Selection.NumberOfWinners = 1;

            Params.Reproduction.ReproductionProbability = 0.9;

            Params.Mutation.MutationProbability = 0.03;
        }

        public void SetBestImg(ImageBox Img)
        {
            this.BestImg = Img;
        }

        private void Options()
        {
            OptionsWindow Options = new OptionsWindow(ref Params);
            Options.ShowDialog();
        }

        private void Load()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".bmp";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                TargetImgPath = filename;

                Mat TargetMat = CvInvoke.Imread(_TargetImgPath, Emgu.CV.CvEnum.ImreadModes.Grayscale);
                ImageWidth = TargetMat.Width;
                ImageHeight = TargetMat.Height;
            }
        }

        private void StartStop()
        {
            if (ButtonStartStopName == "Start")
            {
                if (_TargetImgPath != "")
                {
                    CancelTokenSource = new CancellationTokenSource();

                    ThreadMessageProcess = new Thread(() => MessageProcessing(CancelTokenSource.Token));
                    ThreadMessageProcess.Start();

                    GAImageReproducerThreadHandler = new GAThreadHandle(Params, _TargetImgPath, ImageQueue, CancelTokenSource.Token);
                    GAThread = new Thread(new ThreadStart(GAImageReproducerThreadHandler.ThreadLoop));
                    GAThread.Start();

                    ButtonStartStopName = "Stop";
                } 
                else
                {
                    MessageBox.Show("Aucune image sélectionnée");
                }
            }
            else
            {
                CancelTokenSource.Cancel();
                GAThread.Join();
                ButtonStartStopName = "Start";
            }
        }
        
        private void MessageProcessing(CancellationToken cancelToken)
        {
            ThreadMessage CurrentMsg;

            Stopwatch timer = new Stopwatch();

            while (!cancelToken.IsCancellationRequested)
            {
                if (ImageQueue.TryDequeue(out CurrentMsg))
                {
                    if (!timer.IsRunning)
                        timer.Start();

                    if (CurrentMsg.Image != null && BestImg != null)
                    {
                        BestImg.Image = CurrentMsg.Image.ToImage<Bgr, Byte>();
                    }

                    CurrentGeneration = CurrentMsg.GenerationNumber;
                    BestFitness = CurrentMsg.BestFitness;
                    TotalElapseTime = timer.Elapsed.TotalSeconds;
                    if (CurrentGeneration > 1)
                        GenerationMeanTime = (double)timer.ElapsedMilliseconds / (CurrentGeneration-1);

                }

                if (!GAThread.IsAlive)
                    StartStop();
            }
            timer.Stop();
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }

}
