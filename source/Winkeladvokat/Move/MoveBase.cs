using Winkeladvokat.Tokens;

namespace Winkeladvokat.Move
{
    using System.Collections.Generic;

    public abstract class MoveBase : IMove
    {
        private readonly IEnumerable<Field> gameBoardFields;

        protected MoveBase(IEnumerable<Field> gameBoardFields)
        {
            this.gameBoardFields = gameBoardFields;
        }

        public abstract bool IsFinished { get; protected set; }
        // TODO: IsValid and CheckIsValid are misleading. Try to refactor.
        public abstract bool IsValid { get; protected set; }

        protected IEnumerable<Field> GameBoardFields
        {
            get
            {
                return this.gameBoardFields;
            }
        }

        public abstract MoveResult PerformMove(Field field);

        protected void RemoveTokenFrom(Field fieldToRemoveToken)
        {
            if (fieldToRemoveToken != null)
            {
                fieldToRemoveToken.Token = new NoToken();
            }
        }
    }
}
