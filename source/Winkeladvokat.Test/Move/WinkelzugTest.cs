namespace Winkeladvokat.Move
{
    using System.Collections.Generic;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class WinkelzugTest
    {
        private Winkelzug testee;

        [Test]
        public void RemovesAdvokatSteinWhenFirstMoveIsMade()
        {
            var field = new Field(0, 0) { Token = new AdvocatToken() };
            this.testee = new Winkelzug(new List<Field> { field });

            this.testee.PerformMove(field);

            field.Token.Should().BeNull();
        }

        [Test]
        public void PlacesParagraphensteinWhenSecondMoveIsMade()
        {
            var firstField = new Field(0, 0) { Token = new AdvocatToken() };
            var cornerField = new Field(1, 0);
            this.testee = new Winkelzug(new List<Field> { firstField, cornerField });

            this.testee.PerformMove(firstField);
            this.testee.PerformMove(cornerField);

            cornerField.Token.Should().BeOfType<ParagraphToken>();
        }

        [Test]
        public void PlacesAdvokatsteinWhenThirdMoveIsMade()
        {
            var firstField = new Field(0, 0) { Token = new AdvocatToken() };
            var cornerField = new Field(1, 0);
            var endField = new Field(1, 1);
            this.testee = new Winkelzug(new List<Field> { firstField, cornerField, endField });

            this.testee.PerformMove(firstField);
            this.testee.PerformMove(cornerField);
            this.testee.PerformMove(endField);

            endField.Token.Should().BeOfType<AdvocatToken>();
        }

        [Test]
        public void IsFinishedEqualsTrueWhenThirdMoveIsMade()
        {
            var firstField = new Field(0, 0) { Token = new AdvocatToken() };
            var cornerField = new Field(1, 0);
            var endField = new Field(1, 1);
            this.testee = new Winkelzug(new List<Field> { firstField, cornerField, endField });

            this.testee.PerformMove(firstField);
            this.testee.PerformMove(cornerField);
            this.testee.PerformMove(endField);

            this.testee.IsFinished.Should().BeTrue();
        }

        [Test]
        [TestCase(0, 0, 0, 2, 2, 2, true)]
        [TestCase(7, 0, 7, 3, 4, 3, true)]
        [TestCase(7, 7, 5, 7, 5, 2, true)]
        [TestCase(0, 7, 1, 7, 1, 6, true)]
        [TestCase(0, 0, 0, 1, 0, 2, false)]
        [TestCase(0, 0, 2, 2, 2, 5, false)]
        [TestCase(7, 7, 5, 6, 4, 3, false)]
        public void IsAsExpectedWhenThreeValidMovesWereMade(int startFieldRow, int startFieldColumn, int angleFieldRow, int angleFieldColumn, int endFieldRow, int endFieldColumn, bool expectedResult)
        {
            var startField = new Field(startFieldRow, startFieldColumn) { Token = new AdvocatToken() };
            var winkelField = new Field(angleFieldRow, angleFieldColumn);
            var endField = new Field(endFieldRow, endFieldColumn);
            this.testee = new Winkelzug(new List<Field> { startField, winkelField, endField });

            this.testee.PerformMove(startField);
            this.testee.PerformMove(winkelField);
            this.testee.PerformMove(endField);

            this.testee.IsValid.Should().Be(expectedResult);
        }
    }
}