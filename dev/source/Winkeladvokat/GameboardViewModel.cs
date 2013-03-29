namespace Winkeladvokat
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Media;
    using System.Timers;
    using System.Windows.Input;
    using System.Windows.Media;

    public class GameBoardViewModel : BaseViewModel
    {
        private readonly IGameBoard gameBoard;
        private ObservableCollection<FieldViewModel> fieldViewModels;
        private bool isMessageVisible;
        private string moveResultText;

        private RelayCommand<FieldViewModel> placeTokenCommand;
        private Timer timer;

        public GameBoardViewModel(IGameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
        }

        public ObservableCollection<FieldViewModel> FieldViewModels
        {
            get
            {
                return this.fieldViewModels;
            }

            set
            {
                this.fieldViewModels = value;
                this.NotifyPropertyChanged(() => this.FieldViewModels);
            }
        }

        public ICommand PlaceTokenCommand
        {
            get
            {
                if (this.placeTokenCommand == null)
                {
                    this.placeTokenCommand = new RelayCommand<FieldViewModel>(this.PlaceToken) { IsEnabled = true };
                }

                return this.placeTokenCommand;
            }
        }

        public ObservableCollection<ScoreResult> Scores 
        {
            get
            {
                return new ObservableCollection<ScoreResult>(this.gameBoard.GetCurrentScore());
            }
        }

        public bool IsMessageVisible
        {
            get
            {
                return this.isMessageVisible;
            }

            set
            {
                this.isMessageVisible = value;
                this.NotifyPropertyChanged(() => this.IsMessageVisible);
            }
        }

        public string MoveResultText
        {
            get
            {
                return this.moveResultText;
            }

            private set
            {
                this.moveResultText = value;
                this.NotifyPropertyChanged(() => this.MoveResultText);
            }
        }

        public bool IsGameFinished
        {
            get
            {
                return this.gameBoard.IsFinished;
            }
        }

        public void Loaded()
        {
            var viewModels = new ObservableCollection<FieldViewModel>();
            foreach (var field in this.gameBoard.Fields)
            {
                viewModels.Add(new FieldViewModel(field));
            }

            this.FieldViewModels = viewModels;
            this.NotifyPropertyChanged(() => this.Scores);
        }

        private void PlaceToken(FieldViewModel fieldViewModel)
        {
            MoveResult moveResult = this.gameBoard.PlaceToken(fieldViewModel.Field);
            var fields = this.FieldViewModels;
            this.FieldViewModels = null;
            this.FieldViewModels = fields;
            if (!moveResult.IsValid)
            {
                this.IsMessageVisible = true;
                this.MoveResultText = moveResult.Message;

                var player = new SoundPlayer("WebServiceUnavailable.WAV");
                player.Play();
                this.timer = new Timer(3000);
                this.timer.Elapsed += this.MessageDisplayElapsed;
                this.timer.Start();
            }

            // Todo: Implement IsFinished in MoveResult and do NotifyPropertyChanged only when it is true.
            this.NotifyPropertyChanged(() => this.Scores);
        }

        private void MessageDisplayElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.MoveResultText = string.Empty;
            this.IsMessageVisible = false;
            this.timer.Stop();
        }
    }
}