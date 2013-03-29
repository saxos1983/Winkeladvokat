namespace Winkeladvokat
{
    using System.Collections.Generic;

    public interface IFieldListFactory
    {
        IEnumerable<Field> Create(int size, int numberOfPlayers);
    }
}