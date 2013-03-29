namespace Winkeladvokat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class AsAPlayerIWantToBeNotifiedWhenTheGameIsFinished
    {
        private GameBoardViewModel gameBoardViewModel;

        private PerformMove performMove;

        [SetUp]
        public void SetUp()
        {
            this.gameBoardViewModel = new GameBoardViewModel(new GameBoard(new FieldListFactory(), 4));
            this.gameBoardViewModel.Loaded();
            this.performMove = new PerformMove(this.gameBoardViewModel);

            this.SetBoardPrecondition();
        }

        [Test]
        public void TheCurrentPlayerCannotMakeAWinkelZugThenTheGameEndsImmediately()
        {
            this.performMove.MakeWinkelzug(RedTokenStartField, RedTokenCornerField, RedTokenEndField);
            this.gameBoardViewModel.IsGameFinished.Should().Be(true);
        }

        [Test]
        public void ThePlayerWithTheMostPointsHasWonAfterTheGameEnded()
        {
            this.gameBoardViewModel.PlaceTokenCommand.Execute(this.gameBoardViewModel.FieldViewModels.Single(RedTokenStartField));
            this.gameBoardViewModel.MoveResultText.Should().Be("Spiel beendet!" + Environment.NewLine + "Gewinner:" + Environment.NewLine + "Blue" + Environment.NewLine + "Green");
        }

        private static bool RedTokenStartField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 0;
        }

        private static bool RedTokenCornerField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 2;
        }

        private static bool RedTokenEndField(FieldViewModel f)
        {
            return f.Row == 1 && f.Column == 2;
        }

        private static bool BlueTokenStartField(FieldViewModel f)
        {
            return f.Row == 7 && f.Column == 7;
        }

        private static bool BlueTokenCornerField(FieldViewModel f)
        {
            return f.Row == 1 && f.Column == 7;
        }

        private static bool BlueTokenEndField(FieldViewModel f)
        {
            return f.Row == 1 && f.Column == 0;
        }

        private static bool GreenTokenStartField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 7;
        }

        private static bool GreenTokenCornerField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 1;
        }

        private static bool GreenTokenEndField(FieldViewModel f)
        {
            return f.Row == 3 && f.Column == 1;
        }

        private void SetBoardPrecondition()
        {
            this.performMove.MakeWinkelzug(BlueTokenStartField, BlueTokenCornerField, BlueTokenEndField);
            this.performMove.MakeWinkelzug(GreenTokenStartField, GreenTokenCornerField, GreenTokenEndField);
        }
    }
}
