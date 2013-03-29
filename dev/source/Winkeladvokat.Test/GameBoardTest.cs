namespace Winkeladvokat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class GameBoardTest
    {
        private IFieldListFactory fieldListFactory;
        private IMoveFactory moveFactory;
        private GameBoard testee;

        private List<Field> board;

        [SetUp]
        public void Setup()
        {
            this.fieldListFactory = A.Fake<IFieldListFactory>();
            this.board = this.Create8x8Board();
            A.CallTo(() => this.fieldListFactory.Create(A<int>._, A<int>._)).Returns(this.board);
            this.moveFactory = A.Fake<IMoveFactory>();

            this.testee = new GameBoard(this.fieldListFactory, this.moveFactory, 4);
        }

        [Test]
        public void GameIsFinishedWhenTheCurrentAdvokatIsBlockedAndHeStartHisMoveWithAParagraphToken()
        {
            this.board.SelectByPosition(4, 4).Token = new AdvocatToken { Color = Colors.Red };

            this.board.SelectByPosition(3, 4).Token = new ParagraphToken { Color = Colors.Red };
            this.board.SelectByPosition(5, 4).Token = new ParagraphToken { Color = Colors.Blue };
            this.board.SelectByPosition(4, 3).Token = new ParagraphToken { Color = Colors.Blue };
            this.board.SelectByPosition(4, 5).Token = new ParagraphToken { Color = Colors.Blue };

            A.CallTo(() => this.fieldListFactory.Create(A<int>._, A<int>._)).Returns(this.board);

            this.testee = new GameBoard(this.fieldListFactory, 1);

            var result = this.testee.PlaceToken(this.board.SelectByPosition(3, 4));
            result.IsValid.Should().Be(false);
            result.Message.StartsWith("Spiel beendet!").Should().Be(true);
        }

        [Test]
        public void GameIsFinishedWhenTheCurrentAdvokatIsBlockedAndHeStartHisMoveWithAnAdvokatToken()
        {
            this.board.SelectByPosition(4, 4).Token = new AdvocatToken { Color = Colors.Red };
            this.board.SelectByPosition(3, 4).Token = new ParagraphToken();
            this.board.SelectByPosition(5, 4).Token = new ParagraphToken();
            this.board.SelectByPosition(4, 3).Token = new ParagraphToken();
            this.board.SelectByPosition(4, 5).Token = new ParagraphToken();

            A.CallTo(() => this.fieldListFactory.Create(A<int>._, A<int>._)).Returns(this.board);

            this.testee = new GameBoard(this.fieldListFactory, 1);

            var result = this.testee.PlaceToken(this.board.SelectByPosition(4, 4));
            result.IsValid.Should().Be(false);
            result.Message.StartsWith("Spiel beendet!").Should().Be(true);
        }

        [Test]
        public void FieldListPropertyIsLoadedFromFieldListFactory()
        {
            this.testee = new GameBoard(this.fieldListFactory, 4);

            this.testee.Fields.Count().Should().Be(64);
        }

        [Test]
        public void PlaceTokenCallsMoveFactoryWhenTokenIsNotNullAndCalledTheFirstTime()
        {
            var field = new Field(0, 0) { Token = new AdvocatToken() };
            this.board.SelectByPosition(4, 4).Token = new AdvocatToken();
            this.testee.PlaceToken(field);

            A.CallTo(() => this.moveFactory.CreateByToken(A<Token>._, A<IEnumerable<Field>>._)).MustHaveHappened();
        }

        [Test]
        public void PlaceTokenDelegatesMoveWhenTokenIsNotNullAndCalledTheFirstTime()
        {
            var move = A.Fake<IMove>();
            var selectedField = this.board.SelectByPosition(0, 0);
            selectedField.Token = new AdvocatToken();
            A.CallTo(() => this.moveFactory.CreateByToken(A<Token>._, A<IEnumerable<Field>>._)).Returns(move);

            this.testee.PlaceToken(selectedField);

            A.CallTo(() => move.PerformMove(selectedField)).MustHaveHappened();
        }

        [Test]
        public void PlaceTokenReturnsFailureWhenTokenIsNullAndCalledTheFirstTime()
        {
            var moveResult = this.testee.PlaceToken(new Field(0, 0));

            moveResult.IsValid.Should().BeFalse();
            moveResult.Message.Should().NotBeEmpty();
        }

        [Test]
        public void WinkelzugIsPossibleWhenNoOtherTokensArePresent()
        {
            this.board.SelectByPosition(4, 4).Token = new AdvocatToken();

            this.testee = new GameBoard(this.fieldListFactory, 1);

            this.testee.IsWinkelzugPossible(this.board.SelectByPosition(4, 4)).Should().Be(true);
        }

        [Test]
        public void WinkelZugIsPossibleWhenNoOtherTokenAndAdvokatIsInCorner()
        {
            this.board.SelectByPosition(0, 0).Token = new AdvocatToken();

            this.testee = new GameBoard(this.fieldListFactory, 1);

            this.testee.IsWinkelzugPossible(this.board.SelectByPosition(4, 4)).Should().Be(true);
        }

        [Test]
        public void NoWinkelzugPossibleWhenSurroundedByTokens()
        {
            this.board.SelectByPosition(4, 4).Token = new AdvocatToken();

            this.board.SelectByPosition(3, 4).Token = new ParagraphToken();
            this.board.SelectByPosition(5, 4).Token = new ParagraphToken();
            this.board.SelectByPosition(4, 3).Token = new ParagraphToken();
            this.board.SelectByPosition(4, 5).Token = new ParagraphToken();

            this.testee = new GameBoard(this.fieldListFactory, 1);

            this.testee.IsWinkelzugPossible(this.board.SelectByPosition(4, 4)).Should().Be(false);
        }

        [Test]
        public void WinkelzugPossibleWhenOnlyOneWinkelzugPossible()
        {
            this.board.SelectByPosition(4, 4).Token = new AdvocatToken();

            this.board.SelectByPosition(5, 4).Token = new ParagraphToken();
            this.board.SelectByPosition(4, 3).Token = new ParagraphToken();
            this.board.SelectByPosition(4, 5).Token = new ParagraphToken();

            this.board.SelectByPosition(5, 3).Token = new ParagraphToken();
            this.board.SelectByPosition(5, 6).Token = new ParagraphToken();
            this.board.SelectByPosition(6, 4).Token = new ParagraphToken();
            this.board.SelectByPosition(6, 5).Token = new ParagraphToken();

            this.testee = new GameBoard(this.fieldListFactory, 1);

            this.testee.IsWinkelzugPossible(this.board.SelectByPosition(4, 4)).Should().Be(true);
        }

        [Test]
        public void WinkelzugPossibleWhenOnlyOneWinkelzugPossible2()
        {
            this.board.SelectByPosition(4, 4).Token = new AdvocatToken();

            this.board.SelectByPosition(5, 4).Token = new ParagraphToken();
            this.board.SelectByPosition(4, 3).Token = new ParagraphToken();
            this.board.SelectByPosition(4, 5).Token = new ParagraphToken();

            this.board.SelectByPosition(3, 3).Token = new ParagraphToken();
            this.board.SelectByPosition(2, 3).Token = new ParagraphToken();
            this.board.SelectByPosition(1, 3).Token = new ParagraphToken();
            this.board.SelectByPosition(3, 5).Token = new ParagraphToken();
            this.board.SelectByPosition(2, 5).Token = new ParagraphToken();
            this.board.SelectByPosition(1, 6).Token = new ParagraphToken();
            this.board.SelectByPosition(0, 5).Token = new ParagraphToken();
            this.board.SelectByPosition(0, 4).Token = new ParagraphToken();

            this.testee = new GameBoard(this.fieldListFactory, 1);

            this.testee.IsWinkelzugPossible(this.board.SelectByPosition(4, 4)).Should().Be(true);
        }

        public void PlaceTokenDelegatesMoveWhenTokenIsNullAndCalledTheSecondTime()
        {
            var move = A.Fake<IMove>();
            var startField = new Field(0, 0) { Token = new AdvocatToken() };
            var endField = new Field(1, 0);
            A.CallTo(() => this.moveFactory.CreateByToken(A<Token>._, A<IEnumerable<Field>>._)).Returns(move);

            this.testee.PlaceToken(startField);
            this.testee.PlaceToken(endField);

            A.CallTo(() => move.PerformMove(A<Field>._)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void PlaceTokenCallsMoveFactoryAgainWhenPreviousMoveReportedIsFinished()
        {
            var move = A.Fake<IMove>();
            var startField = new Field(0, 0) { Token = new AdvocatToken() };
            var secondField = new Field(1, 0);
            var thirdField = new Field(1, 1) { Token = new AdvocatToken() };
            this.board.SelectByPosition(4, 4).Token = new AdvocatToken();
            A.CallTo(() => this.moveFactory.CreateByToken(A<Token>._, A<IEnumerable<Field>>._)).Returns(move);

            // CreateByToken is called here as no move has been set yet.
            this.testee.PlaceToken(startField);

            A.CallTo(() => move.IsFinished).Returns(true);
            this.testee.PlaceToken(secondField);

            // CreateByToken is called here because move has previously beent set to null.
            this.testee.PlaceToken(thirdField);

            A.CallTo(() => this.moveFactory.CreateByToken(A<Token>._, A<IEnumerable<Field>>._)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void GetCurrentScoreReturnsAnEmptyList_WhenTheFieldsAreEmpty()
        {
            var board = this.Create8x8Board();

            A.CallTo(() => this.fieldListFactory.Create(A<int>._, A<int>._)).Returns(board);

            this.testee = new GameBoard(this.fieldListFactory, 1);

            var currentScore = this.testee.GetCurrentScore();

            currentScore.Should().BeEmpty();
        }

        [Test]
        public void GetCurrentScoreReturnsAListOfZeroScores_WhenOnlyAdvocatTokensArePresent()
        {
            var board = this.Create8x8Board();

            board.SelectByPosition(0, 0).Token = new AdvocatToken();
            board.SelectByPosition(0, 7).Token = new AdvocatToken();
            board.SelectByPosition(7, 0).Token = new AdvocatToken();
            board.SelectByPosition(7, 7).Token = new AdvocatToken();

            A.CallTo(() => this.fieldListFactory.Create(A<int>._, A<int>._)).Returns(board);

            this.testee = new GameBoard(this.fieldListFactory, 4);

            var currentScore = this.testee.GetCurrentScore().ToList();

            currentScore.Should().OnlyContain(s => s.Score == 0);
            currentScore.Count().Should().Be(4);
        }

        [Test]
        public void GetCurrentScoreReturnsValueOfField_WhenOneParagraphTokenIsPresent()
        {
            var board = this.Create8x8Board();

            board.SelectByPosition(0, 0).Token = new AdvocatToken() { Color = Colors.Black };

            this.AddParagraphTokenToField(board, Colors.Black, 2, 0, 1);

            A.CallTo(() => this.fieldListFactory.Create(A<int>._, A<int>._)).Returns(board);

            this.testee = new GameBoard(this.fieldListFactory, 1);

            var currentScore = this.testee.GetCurrentScore().ToList();

            currentScore.Should().OnlyContain(s => s.Score == 2);
            currentScore.Count().Should().Be(1);
        }

        [Test]
        public void GetCurrentScoreReturnsSumOfFieldValuesPerColor_WhenSomeParagraphTokensArePresent()
        {
            var board = this.Create8x8Board();

            board.SelectByPosition(0, 0).Token = new AdvocatToken() { Color = Colors.Black };
            board.SelectByPosition(7, 0).Token = new AdvocatToken() { Color = Colors.White };
            board.SelectByPosition(0, 7).Token = new AdvocatToken() { Color = Colors.Blue };

            this.AddParagraphTokenToField(board, Colors.Black, 2, 0, 1);
            this.AddParagraphTokenToField(board, Colors.Black, 4, 1, 1);
            this.AddParagraphTokenToField(board, Colors.White, 2, 0, 6);
            this.AddParagraphTokenToField(board, Colors.White, 8, 4, 2);
            this.AddParagraphTokenToField(board, Colors.Blue, 2, 7, 2);
            this.AddParagraphTokenToField(board, Colors.Blue, 16, 4, 3);
            this.AddParagraphTokenToField(board, Colors.Blue, 8, 5, 2);

            A.CallTo(() => this.fieldListFactory.Create(A<int>._, A<int>._)).Returns(board);

            this.testee = new GameBoard(this.fieldListFactory, 1);

            var currentScore = this.testee.GetCurrentScore().ToList();

            currentScore.Should().Contain(s => s.Color == Colors.Black && s.Score == 6);
            currentScore.Should().Contain(s => s.Color == Colors.White && s.Score == 10);
            currentScore.Should().Contain(s => s.Color == Colors.Blue && s.Score == 26);
        }

        private void AddParagraphTokenToField(IEnumerable<Field> board, Color color, int value, int row, int column)
        {
            var field = board.SelectByPosition(row, column);
            field.Value = value;
            field.Token = new ParagraphToken() { Color = color };
        }

        private List<Field> Create8x8Board()
        {
            var tempBoard = new List<Field>();
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    tempBoard.Add(new Field(row, column));
                }
            }

            return tempBoard;
        }
    }
}