namespace Winkeladvokat
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for GameBoardView.xaml
    /// </summary>
    public partial class GameBoardView
    {
        public GameBoardView(int numberOfPlayers)
        {
            this.InitializeComponent();
            this.DataContext = new GameBoardViewModel(new GameBoard(new FieldListFactory(), numberOfPlayers));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ((GameBoardViewModel)this.DataContext).Loaded();
        }
    }
}
