using System;
using Winkeladvokat.Tokens;

namespace Winkeladvokat.Move
{
    using System.Collections.Generic;
    using System.Linq;

    public class Winkelzug : MoveBase
    {
        private readonly IList<Field> moves;
        private Token playingAdvocatToken;

        public Winkelzug(IEnumerable<Field> gameBoardFields)
            : base(gameBoardFields)
        {
            this.moves = new List<Field>();
        }

        public override bool IsFinished { get; protected set; }
        public override bool IsValid { get; protected set; }

        public override MoveResult PerformMove(Field field)
        {
            MoveResult result = MoveResult.CreateValidResult();
            this.moves.Add(field);
            this.IsFinished = false;

            if (this.MovesFromStartPosition())
            {
                if (field.HasToken)
                {
                    this.IsValid = true;
                    this.playingAdvocatToken = field.Token;
                    this.moves.ElementAt(0).Token = new NoToken();
                }
                else
                {
                    this.IsValid = false;
                    this.ClearMoves();
                }
            }
            else if (this.MovedToCornerPosition())
            {
                if (this.CheckIsValid())
                {
                    this.IsValid = true;
                    this.moves.ElementAt(1).Token = new ParagraphToken {Color = this.playingAdvocatToken.Color};
                }
                else
                {
                    this.IsValid = false;
                    this.SetPlayingAdvocatTokenAt(this.moves[0]);
                    result = MoveResult.CreateInvalidResult("Winkelzug ist nicht gültig!");
                    this.ClearMoves();
                }
            }
            else if (this.MovedToEndPosition())
            {
                if (this.CheckIsValid())
                {
                    this.IsValid = true;
                    this.moves.ElementAt(2).Token = new AdvocatToken {Color = this.playingAdvocatToken.Color};
                }
                else
                {
                    this.IsValid = false;
                    this.SetPlayingAdvocatTokenAt(this.moves[0]);
                    this.RemoveTokenFrom(this.moves[1]);
                    result = MoveResult.CreateInvalidResult("Winkelzug ist nicht gültig!");
                }

                this.ClearMoves();
                this.IsFinished = true;
            }

            return result;
        }

        private void SetPlayingAdvocatTokenAt(Field destination)
        {
            destination.Token = new AdvocatToken { Color = this.playingAdvocatToken.Color };
        }

        public void ClearMoves()
        {
            this.moves.Clear();
        }

        private bool CheckIsValid()
        {
            return this.IsAdvocatTokensMovementOnDifferentFields() &&
                            this.IsAdvocatTokensMovementAngled() &&
                            this.IsAdvocatTokensMovementFreeOfAnyTokens();
        }

        private bool IsAdvocatTokensMovementAngled()
        {
            if (this.MovesFromStartPosition())
            {
                return true;
            }

            if (this.MovedToCornerPosition())
            {
                return this.moves[0].Row == this.moves[1].Row || this.moves[0].Column == this.moves[1].Column;
            }

            if (this.MovedToEndPosition())
            {
                return (this.moves[0].Row == this.moves[1].Row && this.moves[1].Column == this.moves[2].Column) ||
                       (this.moves[0].Column == this.moves[1].Column && this.moves[1].Row == this.moves[2].Row);
            }

            return false;
        }

        private bool IsAdvocatTokensMovementOnDifferentFields()
        {
            if (this.MovesFromStartPosition())
            {
                return true;
            }

            if (this.MovedToCornerPosition())
            {
                return !this.moves[0].Equals(this.moves[1]);
            } 

            if (this.MovedToEndPosition())
            {
                return  !this.moves[0].Equals(this.moves[2]) && 
                        !this.moves[0].Equals(this.moves[1]) &&
                        !this.moves[1].Equals(this.moves[2]);
            }

            return false;
        }

        private bool IsAdvocatTokensMovementFreeOfAnyTokens()
        {
            if (this.MovesFromStartPosition())
            {
                return true;
            }
            if (this.MovedToCornerPosition())
            {
                return this.FreeOfAnyTokensInBetween(this.moves[0], this.moves[1]) && !this.moves[1].HasToken;
            }
            if (this.MovedToEndPosition())
            {
                return this.FreeOfAnyTokensInBetween(this.moves[1], this.moves[2]) && !this.moves[2].HasToken;
            }
            return true;
        }

        private bool FreeOfAnyTokensInBetween(Field startField, Field endField)
        {
            var isHorizontalPath = startField.Row == endField.Row;

            if (isHorizontalPath)
            {
                int startPosition = Math.Min(startField.Column, endField.Column);
                int endPosition = Math.Max(startField.Column, endField.Column);

                for (int i = startPosition + 1; i < endPosition; i++)
                {
                    if (this.GameBoardFields.SelectByPosition(startField.Row, i).HasToken)
                    {
                        return false;
                    }
                }
            }
            else
            {
                int startPosition = Math.Min(startField.Row, endField.Row);
                int endPosition = Math.Max(startField.Row, endField.Row);

                for (int i = startPosition + 1; i < endPosition; i++)
                {
                    if (this.GameBoardFields.SelectByPosition(i, startField.Column).HasToken)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool MovesFromStartPosition()
        {
            return this.moves.Count == 1;
        }

        private bool MovedToCornerPosition()
        {
            return this.moves.Count == 2;
        }

        private bool MovedToEndPosition()
        {
            return this.moves.Count == 3;
        }
    }
}