namespace Winkeladvokat
{
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class AsAPlayerICanMakeOnlyValidMoves
    {
        private GameBoardViewModel gameBoardViewModel;

        private PerformMove performMove;

        //// TODO: Jumping over a ADS should not be possible
        //// TODO: Jumping over own Paragraphenstein should not be possible
        //// TODO: Jumping over own ADS should not be possible

        [SetUp]
        public void SetUp()
        {
            this.gameBoardViewModel = new GameBoardViewModel(new GameBoard(new FieldListFactory(), 4));
            this.gameBoardViewModel.Loaded();
            this.performMove = new PerformMove(this.gameBoardViewModel);
        }
    }
}
