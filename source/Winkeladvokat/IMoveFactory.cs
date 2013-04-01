using Winkeladvokat.Tokens;

namespace Winkeladvokat
{
    using System.Collections.Generic;

    public interface IMoveFactory
    {
        IMove CreateByToken(Token token, IEnumerable<Field> gameBoardFields);
    }
}