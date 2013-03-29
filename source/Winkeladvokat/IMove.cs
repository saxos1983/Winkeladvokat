namespace Winkeladvokat
{
    public interface IMove
    {
        bool IsFinished { get; }

        MoveResult PerformMove(Field field);
    }
}