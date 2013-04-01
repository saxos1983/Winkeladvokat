using Winkeladvokat.Tokens;

namespace Winkeladvokat
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class AsAPlayerICanMoveMyAdvokatensteinToPerformAWinkelzug
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
        public void AdvokatensteinRemovedFromOldPosition()
        {
            this.performMove.MakeWinkelzug(StartField, CornerField, EndField);

            // ASSERT
            this.gameBoardViewModel.FieldViewModels.Single(StartField).Field.HasToken.Should().BeFalse();
        }

        [Test]
        public void ParagraphensteinPlacedInCorner()
        {
            this.performMove.MakeWinkelzug(StartField, CornerField, EndField);

            // Assert
            this.gameBoardViewModel.FieldViewModels.Single(CornerField).Field.Token.Should().BeOfType<ParagraphToken>();
        }

        [Test]
        public void AdvokatensteinSetToNewPosition()
        {
            this.performMove.MakeWinkelzug(StartField, CornerField, EndField);

            // Assert
            this.gameBoardViewModel.FieldViewModels.Single(EndField).Field.Token.Should().BeOfType<AdvocatToken>();
        }

        private static bool EndField(FieldViewModel f)
        {
            return f.Row == 4 && f.Column == 3;
        }

        private static bool CornerField(FieldViewModel f)
        {
            return f.Row == 4 && f.Column == 0;
        }

        private static bool StartField(FieldViewModel f)
        {
            return f.Row == 0 && f.Column == 0;
        }
    }
}