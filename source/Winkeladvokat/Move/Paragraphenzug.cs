using System.Collections;
using Winkeladvokat.Tokens;

namespace Winkeladvokat.Move
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Paragraphenzug : MoveBase
    {
        private readonly IList<Field> moves;

        private Token playingParagraphToken;

        public Paragraphenzug(IEnumerable<Field> gameBoardFields)
            : base(gameBoardFields)
        {
            this.moves = new List<Field>();
        }

        public override bool IsFinished { get; protected set; }

        public override bool IsValid
        {
            get { return this.CheckIsValid(this.moves.ElementAt(0), this.moves.ElementAt(1)); }

            protected set { throw new NotImplementedException(); }
        }

        public override MoveResult PerformMove(Field field)
        {

            MoveResult result = MoveResult.CreateValidResult();
            if (this.MovedToPossibleEndPosition() && this.MoveCanBeContinued() && FinishMoveRequested(field))
            {
                this.IsFinished = true;
                this.ClearMoves();
                return result;
            }

            this.moves.Add(field);
            this.IsFinished = false;

            if (this.MovesFromStartPosition())
            {
                if (field.HasToken)
                {
                    this.playingParagraphToken = field.Token;
                    this.RemoveTokenFrom(this.moves.ElementAt(0));
                }
                else
                {
                    this.ClearMoves();
                }
            }
            else if (this.MovedToPossibleEndPosition())
            {
                var actualPosition = this.moves.Last();
                var previousPosition = this.moves.ElementAt(this.moves.Count - 2);

                if (this.CheckIsValid(previousPosition, actualPosition))
                {
                    this.SetPlayingParagraphTokenAt(actualPosition);
                    this.RemoveTokenFrom(previousPosition);
                    this.RemoveTokenFrom(this.GetJumpedOverField(previousPosition, actualPosition));
                }
                else
                {
                    this.SetPlayingParagraphTokenAt(previousPosition);
                    result = MoveResult.CreateInvalidResult("Paragraphenzug ist nicht gültig!");
                }

                if (!this.MoveCanBeContinued())
                {
                    this.IsFinished = true;
                    this.playingParagraphToken = new NoToken();
                    this.ClearMoves();
                }
            }

            return result;
        }

        private bool FinishMoveRequested(Field field)
        {
            return this.moves.Last().Equals(field);
        }

        public void ClearMoves()
        {
            this.moves.Clear();
        }

        private void SetPlayingParagraphTokenAt(Field destination)
        {
            destination.Token = new ParagraphToken { Color = this.playingParagraphToken.Color };
        }

        private bool CheckIsValid(Field startPosition, Field endPosition)
        {
            bool isValid = false;

            if (this.IsParagraphTokenHorizontallyOrVerticallyJumpingOverOneField(startPosition, endPosition))
            {
                var jumpedOverField = this.GetJumpedOverField(startPosition, endPosition);
                if (jumpedOverField != null 
                    && this.IsOnJumpedOverFieldAnOpponentsParagraphToken(jumpedOverField) 
                    && this.IsEndPositionValid(endPosition)
                    && this.IsAnyMovePositionDifferent())
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private bool IsAnyMovePositionDifferent()
        {
            foreach (var move in this.moves)
            {
                if (this.moves.Count(f => f.Equals(move)) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsOnJumpedOverFieldAnOpponentsParagraphToken(Field jumpedOverField)
        {
            return jumpedOverField.Token is ParagraphToken && this.playingParagraphToken.Color != jumpedOverField.Token.Color;
        }

        private bool IsEndPositionValid(Field endPosition)
        {
            return endPosition.Token is NoToken;
        }

        private bool IsParagraphTokenHorizontallyOrVerticallyJumpingOverOneField(
            Field startPosition, Field endPosition)
        {
            if (((startPosition.Row == endPosition.Row) && Math.Abs(startPosition.Column - endPosition.Column) == 2)
                || ((startPosition.Column == endPosition.Column) && Math.Abs(startPosition.Row - endPosition.Row) == 2))
            {
                return true;
            }

            return false;
        }

        private Field GetJumpedOverField(Field startPosition, Field endPosition)
        {
            Field result = null;

            if (startPosition.Row == endPosition.Row)
            {
                int higherColumn = Math.Max(startPosition.Column, endPosition.Column);
                result = this.GameBoardFields.SelectByPosition(startPosition.Row, higherColumn - 1);
            }

            if (startPosition.Column == endPosition.Column)
            {
                int higherRow = Math.Max(startPosition.Row, endPosition.Row);
                result = this.GameBoardFields.SelectByPosition(higherRow - 1, startPosition.Column);
            }

            return result;
        }

        private bool MovesFromStartPosition()
        {
            return this.moves.Count == 1;
        }

        private bool MovedToPossibleEndPosition()
        {
            return this.moves.Count >= 2;
        }

        private bool MoveCanBeContinued()
        {
            if (this.MovesFromStartPosition())
            {
                return true;
            }

            var lastMove = this.moves.Last();

            var nextToLastMove = this.moves.ElementAt(this.moves.Count - 2);

            // Go up, down, left and right (except the position you came from).
            var fieldUpwards = new Tuple<int, int>(lastMove.Row - 2, lastMove.Column);
            var fieldDownwards = new Tuple<int, int>(lastMove.Row + 2, lastMove.Column);
            var fieldToTheLeft = new Tuple<int, int>(lastMove.Row, lastMove.Column - 2);
            var fieldToTheRight = new Tuple<int, int>(lastMove.Row, lastMove.Column + 2);
            var positionsOfPossibleFieldsToJumpOn = new List<Tuple<int, int>> { fieldUpwards, fieldDownwards, fieldToTheLeft, fieldToTheRight };

            var candidatesToJumpOn = new List<Field>();

            foreach (var positionOfCandidate in positionsOfPossibleFieldsToJumpOn)
            {
                if (
                    this.GameBoardFields.Any(
                        f => f.Row == positionOfCandidate.Item1 && f.Column == positionOfCandidate.Item2) &&
                    (nextToLastMove.Row != positionOfCandidate.Item1 && nextToLastMove.Column != positionOfCandidate.Item2))
                {
                    candidatesToJumpOn.Add(this.GameBoardFields.SelectByPosition(positionOfCandidate.Item1, positionOfCandidate.Item2));
                }
            }

            if (!candidatesToJumpOn.Any())
            {
                return false;   // This shouldn't be possible. There will always be at least one candidate.
            }

            foreach (var fieldToJumpOn in candidatesToJumpOn)
            {
                var jumpedOverField = this.GetJumpedOverField(lastMove, fieldToJumpOn);
                if (!this.IsOnJumpedOverFieldAnOpponentsParagraphToken(jumpedOverField))
                {
                    return false;
                }
                if (!this.IsEndPositionValid(fieldToJumpOn))
                {
                    return false;
                }
            }

            return true;
        }
    }
}