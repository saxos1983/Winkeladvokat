namespace Winkeladvokat
{
    using System.Collections.Generic;

    using Winkeladvokat.Move;

    public class MoveFactory : IMoveFactory
    {
        public IMove CreateByToken(Token token, IEnumerable<Field> gameBoardFields)
        {
            if (token is AdvocatToken)
            {
                return new Winkelzug(gameBoardFields);
            }

            return new Paragraphenzug(gameBoardFields);
        }
    }
}