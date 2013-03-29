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

        protected IEnumerable<Field> GameBoardFields
        {
            get
            {
                return this.gameBoardFields;
            }
        }

        public abstract MoveResult PerformMove(Field field);
    }
}
