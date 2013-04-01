using System;
using System.Windows.Media;

namespace Winkeladvokat.Tokens
{
    [Serializable]
    public abstract class Token
    {
        public Color Color { get; set; }

        public override int GetHashCode()
        {
            return this.Color.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var f = obj as Token;
            if (f == null)
            {
                return false;
            }

            return f.Color.Equals(this.Color);
        }
    }
}