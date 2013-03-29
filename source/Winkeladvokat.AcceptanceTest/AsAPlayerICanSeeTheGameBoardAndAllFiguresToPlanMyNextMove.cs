namespace Winkeladvokat
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class AsAPlayerICanSeeTheGameBoardAndAllFiguresToPlanMyNextMove
    {
        private GameBoardViewModel testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new GameBoardViewModel(new GameBoard(new FieldListFactory(), 4));
            this.testee.Loaded();
        }

        [Test]
        public void CellValuesAreDisplayed()
        {
            this.VerifyFieldValue(this.testee.FieldViewModels, 0, 0, 0);
            this.VerifyFieldValue(this.testee.FieldViewModels, 7, 7, 0);
            this.VerifyFieldValue(this.testee.FieldViewModels, 7, 0, 0);
            this.VerifyFieldValue(this.testee.FieldViewModels, 0, 7, 0);

            this.VerifyFieldValue(this.testee.FieldViewModels, 1, 1, 4);
            this.VerifyFieldValue(this.testee.FieldViewModels, 2, 2, 8);
            this.VerifyFieldValue(this.testee.FieldViewModels, 3, 3, 16);
            this.VerifyFieldValue(this.testee.FieldViewModels, 1, 6, 4);
            this.VerifyFieldValue(this.testee.FieldViewModels, 2, 5, 8);

            this.VerifyFieldValue(this.testee.FieldViewModels, 0, 1, 2);
            this.VerifyFieldValue(this.testee.FieldViewModels, 3, 7, 2);
            this.VerifyFieldValue(this.testee.FieldViewModels, 7, 5, 2);
        }

        [Test]
        public void AdvokatTokensAreDisplayedOnTheirStartPositions()
        {
            this.VerifyPresenceOfToken(this.testee.FieldViewModels, 0, 0);
            this.VerifyPresenceOfToken(this.testee.FieldViewModels, 7, 0);
            this.VerifyPresenceOfToken(this.testee.FieldViewModels, 0, 7);
            this.VerifyPresenceOfToken(this.testee.FieldViewModels, 7, 7);

            this.testee.FieldViewModels.Count(f => f.HasToken).Should().Be(4);
        }

        [Ignore]
        private void VerifyPresenceOfToken(IEnumerable<FieldViewModel> fields, int row, int column)
        {
            fields.Single(f => f.Row == row && f.Column == column).HasToken.Should().Be(true);
        }

        private void VerifyFieldValue(IEnumerable<FieldViewModel> fields, int row, int column, int expectedValue)
        {
            fields.Single(f => f.Row == row && f.Column == column).Value.Should().Be(expectedValue);
        }
    }
}
