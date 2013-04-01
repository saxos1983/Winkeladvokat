using Winkeladvokat.Tokens;

namespace Winkeladvokat.Move
{
    using System.Collections.Generic;
    using System.Windows.Media;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class ParagraphenzugTest
    {
        [TestCase("IsNotPossible_WhenParagraphTokenIsMovedDiagonally",              "PTR[2,4];PTG[2,5]",            "PZ=[2,4]->[3,5]", "PTR[2,4];PTG[2,5];NO[3,5]", 8)]
        [TestCase("IsNotPossible_WhenParagraphTokenJumpsOverTwoFieldsHorizontally", "PTR[2,4];PTG[2,5];PTG[2,6]",   "PZ=[2,4]->[2,7]", "PTR[2,4];PTG[2,5];PTG[2,6];NO[2,7]", 8)]
        [TestCase("IsNotPossible_WhenParagraphTokenJumpsOverOpponentsAdvocatToken", "PTR[2,4];ATG[2,5]",            "PZ=[2,4]->[2,6]", "PTR[2,4];PTG[2,5];NO[2,6]", 8)]
        [TestCase("IsNotPossible_WhenParagraphTokenJumpsOverOwnParagraphToken",     "PTR[2,4];PTR[2,5]",            "PZ=[2,4]->[2,6]", "PTR[2,4];PTR[2,5];NO[2,6]", 8)]
        [TestCase("ParagraphenTokenRemovedFromStartPosition_WhenFirstMoveIsMade",   "PT[2,2]",                      "PZ=[2,2]",         "NO[2,2]", 8)]
        [TestCase("ParagraphTokenSetToEndPosition_WhenLastMoveIsMade",              "PTR[2,2];PTG[2,3]",            "PZ=[2,2]->[2,4]", "PTR[2,4]", 8)]
        [TestCase("OpponentsParagraphTokenIsRemoved_WhenJumpedOverHorizontally",    "PTR[2,2];PTG[2,3]", "PZ=[2,2]->[2,4]", "NO[2,3]", 8)]
        [TestCase("OpponentsParagraphTokenIsRemoved_WhenJumpedOverVertically",      "PTR[2,2];PTG[3,2]", "PZ=[2,2]->[4,2]", "NO[3,2]", 8)]
        [TestCase("TwoOpponentsParagraphTokensAreRemoved_WhenMoveIsChained", "PTG[0,1];PTR[3,0];ATR[3,4];PTB[4,0];PTB[4,7];PTG[5,1];ATG[5,7];ATB[7,0]", "PZ=[3,0]->[5,0]->[5,2]", "PTG[0,1];NO[3,0];NO[5,0];ATR[3,4];NO[4,0];PTB[4,7];NO[5,1];ATG[5,7];ATB[7,0];PTR[5,2]", 8)]
        public void Paragraphenzug(string name, string gameBoardData, string moves, string expectedResult, int boardDimension)
        {
            var gameBoardFactory = new GameBoardFactory();
            IList<Field> gameBoard = gameBoardFactory.Create(gameBoardData, boardDimension);

            Moves moveHelper = new Moves(gameBoard);
            moveHelper.PerformMoves(moves);

            gameBoardFactory.VerifyFields(gameBoard, expectedResult);
        }   
    }
}