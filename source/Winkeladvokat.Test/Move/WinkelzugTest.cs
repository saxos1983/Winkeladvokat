using Winkeladvokat.Tokens;

namespace Winkeladvokat.Move
{
    using System.Collections.Generic;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class WinkelzugTest
    {
        private Winkelzug testee;

        [TestCase("RemovesAdvocatToken_WhenFirstMoveIsMade", "AT[2,2]", "WZ=[2,2]->[2,5]->[4,5]", "NO[2,2]", 8)]
        [TestCase("PlacesParagraphToken_WhenSecondMoveIsMade", "ATG[0,0]", "WZ=[0,0]->[0,5]->[4,5]", "PTG[0,5]", 8)]
        [TestCase("PlacesParagraphToken_WhenLastMoveIsMade", "ATG[0,0]", "WZ=[0,0]->[0,5]->[4,5]", "ATG[4,5]", 8)]
        [TestCase("PlacesAdvocatToken_WhenMovedCorrectly", "PTR[0,3];ATR[3,3];ATB[4,4];PTB[4,7]", "WZ=[4,4]->[4,0]->[0,0]", "ATB[0,0];PTR[0,3];ATR[3,3];PTB[4,0];PTB[4,7]", 8)]
        [TestCase("DoesNotPlaceAdvocatToken_WhenMovedNotCorrectly", "PTR[0,3];ATR[3,3];ATB[4,4];PTB[4,7]", "WZ=[4,4]->[4,7]->[0,7]", "PTR[0,3];ATR[3,3];ATB[4,4];PTB[4,7]", 8)]
        [TestCase("DoesNotPlaceAdvocatToken_WhenMovedNotCorrectly", "PTR[0,3];PTR[0,6];ATR[2,6];PTR[3,4];ATB[4,4];PTB[4,7]", "WZ=[4,4]->[2,4]->[2,5]", "PTR[0,3];PTR[0,6];ATR[2,6];PTR[3,4];ATB[4,4];PTB[4,7]", 8)]
        [TestCase("DoesNotPlaceAdvocatToken_WhenMovedIntoAdvocatToken", "PTR[3,0];ATR[3,3];ATB[4,5];PTB[4,7]", "WZ=[4,5]->[3,5]->[3,3]", "PTR[3,0];ATR[3,3];ATB[4,5];PTB[4,7]", 8)]
        public void Winkelzug(string name, string gameBoardData, string moves, string expectedResult, int boardDimension)
        {
            var gameBoardFactory = new GameBoardFactory();
            IList<Field> gameBoard = gameBoardFactory.Create(gameBoardData, boardDimension);

            Moves moveHelper = new Moves(gameBoard);
            moveHelper.PerformMoves(moves);

            gameBoardFactory.VerifyFields(gameBoard, expectedResult);
        }

        [TestCase("AT[0,0]", "WZ=[0,0]->[1,0]->[1,1]", 8, true)]
        [TestCase("AT[0,0]", "WZ=[0,0]->[1,0]", 8, false)]
        public void IsFinishedEqualsTrueWhenThirdMoveIsMade(string gameBoardData, string moves, int boardDimension, bool expectedResultForIsFinished)
        {
            var gameBoardFactory = new GameBoardFactory();
            IList<Field> gameBoard = gameBoardFactory.Create(gameBoardData, boardDimension);

            Moves moveHelper = new Moves(gameBoard);
            moveHelper.PerformMoves(moves);

            moveHelper.IsActualMoveFinished.Should().Be(expectedResultForIsFinished);
        }

        [TestCase("AT[0,0]", "WZ=[0,0]->[0,2]->[2,2]", 8, true)]
        [TestCase("AT[7,0]", "WZ=[7,0]->[7,3]->[4,3]", 8, true)]
        [TestCase("AT[7,7]", "WZ=[7,7]->[5,7]->[5,2]", 8, true)]
        [TestCase("AT[0,7]", "WZ=[0,7]->[1,7]->[1,6]", 8, true)]
        [TestCase("AT[0,0]", "WZ=[0,0]->[0,1]->[0,2]", 8, false)]
        [TestCase("AT[0,0]", "WZ=[0,0]->[2,2]->[2,5]", 8, false)]
        [TestCase("AT[7,7]", "WZ=[7,7]->[5,6]->[4,3]", 8, false)]
        [TestCase("PTR[0,3];ATR[3,3];ATB[4,4];PTB[4,7]", "WZ=[3,3]->[4,3]->[4,4]", 8, false)]
        [TestCase("PTR[0,3];ATR[3,3];ATB[4,4];PTB[4,7]", "WZ=[3,3]->[1,3]->[1,2]", 8, true)]
        [TestCase("PTR[0,3];ATR[3,3];ATB[4,4];PTB[4,7]", "WZ=[4,4]->[4,0]->[0,0]", 8, true)]
        [TestCase("PTR[0,3];ATR[3,3];ATG[3,4];PTG[3,7];ATB[4,4];PTB[4,7];ATY[7,0]", "WZ=[4,4]->[3,4]->[3,5]", 8, false)]
        [TestCase("PTR[3,0];ATR[3,3];ATB[4,4];PTB[4,7]", "WZ=[3,3]->[3,7]->[5,7]", 8, false)]
        [TestCase("PTR[3,0];ATR[3,3];ATB[4,4];PTB[4,7]", "WZ=[4,4]->[4,0]->[3,0]", 8, false)]
        public void IsAsExpectedWhenWinkelzugIsMade(string gameBoardData, string moves, int boardDimension, bool expectedResultForIsValidMove)
        {
            var gameBoardFactory = new GameBoardFactory();
            IList<Field> gameBoard = gameBoardFactory.Create(gameBoardData, boardDimension);

            Moves moveHelper = new Moves(gameBoard);
            moveHelper.PerformMoves(moves);

            moveHelper.IsActualMoveValid.Should().Be(expectedResultForIsValidMove);
        }
    }
}