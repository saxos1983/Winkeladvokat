namespace Winkeladvokat
{
    using System.Windows.Media;

    public class FieldViewModel : BaseViewModel
    {
        private readonly Field field;

        public FieldViewModel(Field field)
        {
            this.field = field;
        }

        public Field Field
        {
            get
            {
                return this.field;
            }
        }

        public bool HasToken
        {
            get
            {
                return this.field.Token != null;
            }
        }

        public int Value
        {
            get { return this.field.Value; }
        }

        public Color TokenColor
        {
            get
            {
                var token = this.field.Token;
                if (token != null)
                {
                    return token.Color;
                }

                return Colors.White;
            }
        }

        public int Column
        {
            get { return this.field.Column; }
        }

        public int Row
        {
            get { return this.field.Row; }
        }

        public string TokenSize
        {
            get
            {
                if (this.field.Token is AdvocatToken)
                {
                    return "8*";
                }

                return "4*";
            }
        }

        public string TokenName
        {
            get
            {
                if (this.field.Token is AdvocatToken)
                {
                    return "A";
                }

                return "§";
            }
        }
    }
}