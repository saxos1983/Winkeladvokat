namespace Winkeladvokat
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow
    {
        public StartWindow()
        {
            this.InitializeComponent();
        }

        private void ButtonStartGameClick(object sender, RoutedEventArgs e)
        {
            int numberOfPlayers = this.NumberOfPlayers.SelectedIndex + 2;
            var board = new GameBoardView(numberOfPlayers);
            board.Show();
            this.Close();
        }
    }
}
