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

        public override MoveResult PerformMove(Field field)
        {
            MoveResult result = MoveResult.CreateValidResult();
            this.moves.Add(field);
            var startPosition = this.moves.ElementAt(0);
            this.IsFinished = false;

            if (this.MovesFromStartPosition())
            {
                if (field.Token != null)
                {
                    this.playingParagraphToken = field.Token;
                    this.RemoveTokenFrom(startPosition);
                }
                else
                {
                    this.ClearMoves();
                }
            }
            else if (this.MovedToEndPosition())
            {
                var endPosition = this.moves.ElementAt(1);

                if (this.CheckIsValid(startPosition, endPosition))
                {
                    this.SetPlayingParagraphTokenAt(endPosition);
                    this.RemoveTokenFrom(this.GetJumpedOverField(startPosition, endPosition));
                }
                else
                {
                    this.SetPlayingParagraphTokenAt(startPosition);
                    result = MoveResult.CreateInvalidResult("Paragraphenzug ist nicht gültig!");
                }

                this.IsFinished = true;
                this.playingParagraphToken = null;
                this.ClearMoves();
            }

            return result;
        }

        public void ClearMoves()
        {
            this.moves.Clear();
        }

        private void SetPlayingParagraphTokenAt(Field destination)
        {
            destination.Token = new ParagraphToken { Color = this.playingParagraphToken.Color };
        }

        private void RemoveTokenFrom(Field fieldToRemoveToken)
        {
            if (fieldToRemoveToken != null)
            {
                fieldToRemoveToken.Token = null;
            }
        }

        private bool CheckIsValid(Field startPosition, Field endPosition)
        {
            bool isValid = false;

            if (this.IsParagraphTokenHorizontallyOrVerticallyJumpingOverOneField(startPosition, endPosition))
            {
                var jumpedOverField = this.GetJumpedOverField(startPosition, endPosition);
                if (jumpedOverField != null 
                    && this.IsOnJumpedOverFieldAnOpponentsParagraphToken(jumpedOverField) 
                    && this.IsEndPositionValid(endPosition))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private bool IsOnJumpedOverFieldAnOpponentsParagraphToken(Field jumpedOverField)
        {
            return jumpedOverField.Token is ParagraphToken && this.playingParagraphToken.Color != jumpedOverField.Token.Color;
        }

        private bool IsEndPositionValid(Field endPosition)
        {
            return endPosition.Token == null;
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

        private bool MovedToEndPosition()
        {
            return this.moves.Count == 2;
        }
    }
}
