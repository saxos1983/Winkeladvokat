namespace Winkeladvokat.Move
{
    using System.Collections.Generic;
    using System.Windows.Media;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class ParagraphenzugTest
    {
        private Paragraphenzug testee;

        private GameBoardFactory gameBoardFactory;

        [SetUp]
        public void SetUp()
        {
            this.gameBoardFactory = new GameBoardFactory();
        }

        [TestCase("PTR[2,4];PTG[2,5]", "PZ=[2,4]->[3,5]", "PTR[2,4];PTG[2,5]", 8)]
        [TestCase("PTR[2,4];PTG[2,5];PTG[2,6]", "PZ=[2,4]->[2,7]", "PTR[2,4];PTG[2,5]", 8)]
        public void ParagraphenzugIsNotPossible_WhenParagraphensteinIsMovedDiagonally(string gameBoardData, string moves, string expectedResult, int boardDimension)
        {
            IList<Field> gameBoard = this.gameBoardFactory.Create(gameBoardData, boardDimension);
            this.testee = new Paragraphenzug(gameBoard);

            Moves moveHelper = new Moves(gameBoard);
            moveHelper.PerformMoves(moves);

            this.gameBoardFactory.VerifyFields(gameBoard, expectedResult);
        }        
        
        [Test]
        public void ParagraphenzugIsNotPossible_WhenParagraphensteinJumpsOverTwoFieldsHorizontally()
        {
            var startFieldOfMove = new Field(2, 4) { Token = new ParagraphToken { Color = Colors.Red } };
            var opponentsFirstParagraphToken = new Field(2, 5) { Token = new ParagraphToken { Color = Colors.Green } };
            var opponentsSecondParagraphToken = new Field(2, 6) { Token = new ParagraphToken { Color = Colors.Green } };
            var endFieldOfMove = new Field(2, 7);
            this.testee = new Paragraphenzug(new List<Field> { startFieldOfMove, opponentsFirstParagraphToken, opponentsSecondParagraphToken, endFieldOfMove });

            this.testee.PerformMove(startFieldOfMove);
            this.testee.PerformMove(endFieldOfMove);

            endFieldOfMove.Token.Should().BeNull();
            startFieldOfMove.Token.Should().BeOfType<ParagraphToken>();
            startFieldOfMove.Token.Color.Should().Be(Colors.Red);
            opponentsFirstParagraphToken.Token.Should().BeOfType<ParagraphToken>();
            opponentsFirstParagraphToken.Token.Color.Should().Be(Colors.Green);            
            opponentsFirstParagraphToken.Token.Should().BeOfType<ParagraphToken>();
            opponentsFirstParagraphToken.Token.Color.Should().Be(Colors.Green);
        }

        [Test]
        public void ParagraphenzugIsNotPossible_WhenParagraphensteinTriesToJumpOverOpponentsAdvokatstein()
        {
            var startFieldOfMove = new Field(2, 4) { Token = new ParagraphToken { Color = Colors.Red } };
            var opponentsAdvokatstein = new Field(2, 5) { Token = new AdvocatToken { Color = Colors.Green } };
            var endFieldOfMove = new Field(2, 6);
            this.testee = new Paragraphenzug(new List<Field> { startFieldOfMove, opponentsAdvokatstein, endFieldOfMove });

            this.testee.PerformMove(startFieldOfMove);
            this.testee.PerformMove(endFieldOfMove);

            endFieldOfMove.Token.Should().BeNull();
            startFieldOfMove.Token.Should().BeOfType<ParagraphToken>();
            startFieldOfMove.Token.Color.Should().Be(Colors.Red);
            opponentsAdvokatstein.Token.Should().BeOfType<AdvocatToken>();
            opponentsAdvokatstein.Token.Color.Should().Be(Colors.Green);
        }        
        
        [Test]
        public void ParagraphenzugIsNotPossible_WhenParagraphensteinTriesToJumpOverOwnParagraphenstein()
        {
            var startFieldOfMove = new Field(2, 4) { Token = new ParagraphToken { Color = Colors.Red } };
            var ownParagraphenstein = new Field(2, 5) { Token = new ParagraphToken { Color = Colors.Red } };
            var endFieldOfMove = new Field(2, 6);
            this.testee = new Paragraphenzug(new List<Field> { startFieldOfMove, ownParagraphenstein, endFieldOfMove });

            this.testee.PerformMove(startFieldOfMove);
            this.testee.PerformMove(endFieldOfMove);

            endFieldOfMove.Token.Should().BeNull();
            startFieldOfMove.Token.Should().BeOfType<ParagraphToken>();
            startFieldOfMove.Token.Color.Should().Be(Colors.Red);
            ownParagraphenstein.Token.Should().BeOfType<ParagraphToken>();
            ownParagraphenstein.Token.Color.Should().Be(Colors.Red);
        }

        [Test]
        public void RemoveParagraphensteinFromStartPosition_WhenFirstMoveIsMade()
        {
            var startField = new Field(2, 2) { Token = new ParagraphToken() };
            this.testee = new Paragraphenzug(new List<Field> { startField });

            this.testee.PerformMove(startField);

            startField.Token.Should().BeNull();
        }

        [Test]
        public void SetParagraphensteinToEndPosition_WhenLastMoveIsMade()
        {
            var startField = new Field(2, 2) { Token = new ParagraphToken { Color = Colors.Red } };
            var opponentField = new Field(2, 3) { Token = new ParagraphToken { Color = Colors.Green } };
            var endField = new Field(2, 4);
            this.testee = new Paragraphenzug(new List<Field> { startField, opponentField, endField });

            this.testee.PerformMove(startField);
            this.testee.PerformMove(endField);

            endField.Token.Should().BeOfType<ParagraphToken>();
            endField.Token.Color.Should().Be(Colors.Red);
        }

        [Test]
        public void RemoveParagraphenSteinFromOpponent_WhenItWasJumpedOver()
        {
            var startField = new Field(2, 2) { Token = new ParagraphToken { Color = Colors.Red } };
            var opponentField = new Field(2, 3) { Token = new ParagraphToken { Color = Colors.Green } };
            var endField = new Field(2, 4);
            this.testee = new Paragraphenzug(new List<Field> { startField, opponentField, endField });

            this.testee.PerformMove(startField);
            this.testee.PerformMove(endField);

            opponentField.Token.Should().BeNull();
        }

        // TODO: Possibility to make a Row test. Bit of code duplication right now.
        [Test]
        public void RemoveParagraphenSteinFromOpponent_WhenItWasJumpedOverVertically()
        {
            var startField = new Field(2, 2) { Token = new ParagraphToken { Color = Colors.Red } };
            var opponentField = new Field(3, 2) { Token = new ParagraphToken { Color = Colors.Green } };
            var endField = new Field(4, 2);
            this.testee = new Paragraphenzug(new List<Field> { startField, opponentField, endField });

            this.testee.PerformMove(startField);
            this.testee.PerformMove(endField);

            opponentField.Token.Should().BeNull();
        }
    }
}
