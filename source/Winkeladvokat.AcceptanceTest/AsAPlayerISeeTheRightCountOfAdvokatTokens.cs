using Winkeladvokat.Tokens;

namespace Winkeladvokat
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class AsAPlayerISeeTheRightCountOfAdvokatTokens
    {
        private GameBoardViewModel testee;

        [Test]
        public void TwoAdvokatTokenArePlacedOnTheOppositeCornerFields()
        {
            this.testee = new GameBoardViewModel(new GameBoard(new FieldListFactory(), 2));
            this.testee.Loaded();

            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 0, 0, Colors.Red);
            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 7, 7, Colors.Blue);
        }

        [Test]
        public void ThreeAdvokatTokenArePlacedInTheRightFields()
        {
            this.testee = new GameBoardViewModel(new GameBoard(new FieldListFactory(), 3));
            this.testee.Loaded();

            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 0, 0, Colors.Red);
            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 0, 7, Colors.Green);
            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 7, 7, Colors.Blue);
        }

        [Test]
        public void FourAdvokatTokenArePlacedInTheRightFields()
        {
            this.testee = new GameBoardViewModel(new GameBoard(new FieldListFactory(), 4));
            this.testee.Loaded();

            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 0, 0, Colors.Red);
            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 0, 7, Colors.Green);
            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 7, 0, Colors.Yellow);
            this.VerifyPresenceOfAdvokatToken(this.testee.FieldViewModels, 7, 7, Colors.Blue);
        }

        private void VerifyPresenceOfAdvokatToken(IEnumerable<FieldViewModel> fields, int row, int column, Color tokenColor)
        {
            var token = fields.Single(f => f.Row == row && f.Column == column).Field.Token;
            token.Should().BeOfType<AdvocatToken>();
            token.Color.Should().Be(tokenColor);
        }
    }
}
