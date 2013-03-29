namespace Winkeladvokat
{
    using System.Globalization;

    public class Field
    {
        public Field(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public Token Token { get; set; }

        public int Value { get; set; }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public override string ToString()
        {
            return this.Value.ToString(CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Token != null ? this.Token.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ this.Value;
                hashCode = (hashCode * 397) ^ this.Row;
                hashCode = (hashCode * 397) ^ this.Column;
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var f = obj as Field;
            if (f == null)
            {
                return false;
            }

            return this.Row == f.Row && this.Column == f.Column && this.Value == f.Value && this.Token.Equals(f.Token);
        }
    }
}