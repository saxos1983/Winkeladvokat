namespace Winkeladvokat.Move
{
    using System.Collections.Generic;
    using System.Linq;

    public class Winkelzug : MoveBase
    {
        private readonly IList<Field> moves;
        private Token advokatToken;
        private bool isValid;

        public Winkelzug(IEnumerable<Field> gameBoardFields)
            : base(gameBoardFields)
        {
            this.moves = new List<Field>();
        }

        public override bool IsFinished { get; protected set; }

        public bool IsValid
        {
            get
            {
                return this.isValid;
            }
        }

        public override MoveResult PerformMove(Field field)
        {
            this.moves.Add(field);
            this.isValid = false;

            if (this.MovesFromStartPosition())
            {
                if (field.Token != null)
                {
                    this.advokatToken = field.Token;
                    this.moves.ElementAt(0).Token = null;
                }
                else
                {
                    this.ClearMoves();
                }
            }
            else if (this.MovedToCornerPosition())
            {
                this.moves.ElementAt(1).Token = new ParagraphToken { Color = this.advokatToken.Color };
            }
            else if (this.MovedToEndPosition())
            {
                this.moves.ElementAt(2).Token = new AdvocatToken { Color = this.advokatToken.Color };
                this.isValid = this.CheckIsValid();
                this.ClearMoves();
                this.IsFinished = true;
            }

            return MoveResult.CreateValidResult();
        }

        public void ClearMoves()
        {
            this.moves.Clear();
        }

        private bool CheckIsValid()
        {
            return this.moves.Count() == 3 &&
                 ((this.moves[0].Row == this.moves[1].Row && this.moves[1].Column == this.moves[2].Column) ||
                 (this.moves[0].Column == this.moves[1].Column && this.moves[1].Row == this.moves[2].Row));
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
