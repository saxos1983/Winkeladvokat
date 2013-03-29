namespace Winkeladvokat
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class AsAPlayerICanMoveMyParagraphensteinToEliminateTheParagraphensteinFromMyOpponent
    {
        private GameBoardViewModel gameBoardViewModel;

        private PerformMove performMove;

        [SetUp]
        public void SetUp()
        {
            this.gameBoardViewModel = new GameBoardViewModel(new GameBoard(new FieldListFactory(), 4));
            this.gameBoardViewModel.Loaded();
            this.performMove = new PerformMove(this.gameBoardViewModel);
        }

        [Test]
        public void ParagraphensteinRemovedFromOldPosition()
        {
            this.CreatePreconditionForParagraphenzug();
            this.MakeSimpleParagraphenzug(StartField, EndField);

            // ASSERT
            this.gameBoardViewModel.FieldViewModels.Single(StartField).Field.Token.Should().BeNull();
        }

        [Test]
        public void ParagraphensteinIsPlacedInNewPosition()
        {
            this.CreatePreconditionForParagraphenzug();
            this.MakeSimpleParagraphenzug(StartField, EndField);

            // Assert
            this.gameBoardViewModel.FieldViewModels.Single(EndField).Field.Token.Should().BeOfType<ParagraphToken>();
        }

        [Test]
        public void ParagraphensteinFromOpponentIsRemoved()
        {
            this.CreatePreconditionForParagraphenzug();
            this.MakeSimpleParagraphenzug(StartField, EndField);

            // Assert
            this.gameBoardViewModel.FieldViewModels.Single(GreenTokenCornerField).Field.Token.Should().BeNull();
        }

        private static bool EndField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 4;
        }

        private static bool StartField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 2;
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

        private static bool GreenTokenStartField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 7;
        }

        private static bool GreenTokenCornerField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 3;
        }

        private static bool GreenTokenEndField(FieldViewModel f)
        {
            return f.Row == 1 && f.Column == 3;
        }

        private void MakeSimpleParagraphenzug(
            Func<FieldViewModel, bool> startFieldSelector,
            Func<FieldViewModel, bool> endFieldSelector)
        {
            // ARRANGE
            var startField = this.gameBoardViewModel.FieldViewModels.Single(startFieldSelector);
            var endField = this.gameBoardViewModel.FieldViewModels.Single(endFieldSelector);

            // ACT
            this.gameBoardViewModel.PlaceTokenCommand.Execute(startField);
            this.gameBoardViewModel.PlaceTokenCommand.Execute(endField);
        }

        private void CreatePreconditionForParagraphenzug()
        {
            // Place red Paragraphenstein to position (0,2)
            this.performMove.MakeWinkelzug(RedTokenStartField, RedTokenCornerField, RedTokenEndField);

            // Place green Paragraphenstein to position (0,3)
            this.performMove.MakeWinkelzug(GreenTokenStartField, GreenTokenCornerField, GreenTokenEndField);
        }
    }
}
