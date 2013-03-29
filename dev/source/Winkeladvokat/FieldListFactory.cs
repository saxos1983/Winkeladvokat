namespace Winkeladvokat
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    public class FieldListFactory : IFieldListFactory
    {
        private readonly List<Field> fields;

        public FieldListFactory()
        {
            this.fields = new List<Field>();
        }

        public IEnumerable<Field> Create(int size, int numberOfPlayers)
        {
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    this.fields.Add(new Field(row, column));
                }
            }

            this.FillFieldValues(size);
            this.PlaceAdvocatTokens(size, numberOfPlayers);

            return this.fields;
        }

        private void PlaceAdvocatTokens(int size, int numberOfPlayers)
        {
            this.fields.Single(f => f.Row == 0 && f.Column == 0).Token = new AdvocatToken { Color = Colors.Red };
            this.fields.Single(f => f.Row == size - 1 && f.Column == size - 1).Token = new AdvocatToken { Color = Colors.Blue };

            if (numberOfPlayers >= 3)
            {
                this.fields.Single(f => f.Row == 0 && f.Column == size - 1).Token = new AdvocatToken { Color = Colors.Green };
            }

            if (numberOfPlayers == 4)
            {
                this.fields.Single(f => f.Row == size - 1 && f.Column == 0).Token = new AdvocatToken { Color = Colors.Yellow };
            }
        }

        private void FillFieldValues(int size)
        {
            int value = 2;
            for (int i = 0; i < size / 2; i++)
            {
                int actualIndex = i;
                int oppositeIndex = size - actualIndex - 1;

                foreach (var field in this.fields.Where(f => f.Row == actualIndex || f.Row == oppositeIndex || f.Column == actualIndex || f.Column == oppositeIndex))
                {
                    if ((field.Row >= actualIndex && field.Row <= oppositeIndex) && (field.Column >= actualIndex && field.Column <= oppositeIndex))
                    {
                        field.Value = value;    
                    }
                }

                value = value * 2;
            }

            if (size % 2 == 1)
            {
                this.fields.Single(f => f.Row == (size / 2) && f.Column == (size / 2)).Value = value;
            }

            foreach (var field in this.fields.Where(f => 
                                                    (f.Row == 0 && f.Column == 0) || 
                                                    (f.Row == 0 && f.Column == size - 1) || 
                                                    (f.Row == size - 1 && f.Column == 0) || 
                                                    (f.Row == size - 1 && f.Column == size - 1)))
            {
                field.Value = 0;
            }
        }
    }
}