namespace Winkeladvokat
{
    using System.Collections.Generic;

    public interface IGameBoard
    {
        IEnumerable<Field> Fields { get; }

        bool IsFinished { get; }

        IEnumerable<ScoreResult> GetCurrentScore();

        MoveResult PlaceToken(Field field);
    }
}