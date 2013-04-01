using Winkeladvokat.Tokens;

namespace Winkeladvokat
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;
    using FluentAssertions;

    public class GameBoardFactory : IGameBoardFactory
    {
        private IList<Field> gameBoard;

        /// <summary>
        /// Creates a list of fields according to the provided fields data <see cref="fieldsData" />
        /// </summary>
        /// <param name="fieldsData">A comma separated list of fields. Each field is defined with:
        /// - it's token type (PT = ParagraphToken, AT = AdvocatToken)
        /// - it's token color [optional] (R = Red, G = Green, Y = Yellow, B = Blue, Not specified or unknown = White)
        /// - it's position on the board enclosed by brackets and coordinates separated by a comma ([3,2] = 3rd Row, 2nd Column)
        /// E.g. PTR[3,2];AT[4,2] will create a Red ParagraphToken at Row 3 Column 2 and a AdvocatToken at Row 4, Column 2 with no color.</param>
        /// <param name="dimension">The dimension of the board. E.g. 8 creates a 8x8 board.</param>
        /// <returns>
        /// The converted field from the provided field data.
        /// </returns>
        public IList<Field> Create(string fieldsData, int dimension)
        {
            this.gameBoard = this.Create(dimension);

            IList<Field> fields = this.GetFields(fieldsData);

            foreach (var field in fields)
            {
                Field f = this.gameBoard.SelectByPosition(field.Row, field.Column);
                f.Value = field.Value;
                f.Token = field.Token;
            }

            return this.gameBoard;
        }

        public IList<Field> Create(int dimension)
        {
            var board = new List<Field>();
            for (int row = 0; row < dimension; row++)
            {
                for (int column = 0; column < dimension; column++)
                {
                    board.Add(new Field(row, column));
                }
            }

            return board;
        }

        public void VerifyFields(IList<Field> boardFields, string expectedFieldsData)
        {
            IList<Field> expectedFields = this.GetFields(expectedFieldsData);
            foreach (var expectedField in expectedFields)
            {
                var actualBoardField = boardFields.SelectByPosition(expectedField.Row, expectedField.Column);
                if (expectedField.Token == null)
                {
                    actualBoardField.ShouldHave().AllProperties().But(d => d.Token).EqualTo(expectedField);
                    actualBoardField.Token.Should().BeNull();
                }
                else
                {
                    actualBoardField.ShouldHave().AllProperties().EqualTo(expectedField);
                }
            }
        }

        public IList<Field> GetFields(string fieldsData)
        {
            IList<Field> result = new List<Field>();

            var fields = fieldsData.Split(';');

            foreach (var field in fields)
            {
                result.Add(this.GetField(field));
            }

            return result;
        }

        private Field GetField(string fieldData)
        {
            if (string.IsNullOrEmpty(fieldData) || (fieldData.Length < 7 && fieldData.Length > 8))
            {
                throw new ArgumentException("The provided field data is not valid.");
            }

            var fieldPositionStartIndex = fieldData.IndexOf('[') + 1;
            var fieldPositionEndIndex = fieldData.IndexOf(']') - 1;

            var row = int.Parse(fieldData.Substring(fieldPositionStartIndex, 1));
            var column = int.Parse(fieldData.Substring(fieldPositionEndIndex, 1));

            var result = new Field(row, column);

            switch (fieldData.Substring(0, 2))
            {
                case "AT":
                    result.Token = new AdvocatToken();
                    break;
                case "PT":
                    result.Token = new ParagraphToken();
                    break;
                case "NO":
                    return result;
                default:
                    throw new ArgumentException("Provided token type is invalid!");
            }

            var containsColor = fieldData.Substring(3, 1) == "[";

            if (containsColor)
            {
                switch (fieldData.Substring(2, 1))
                {
                    case "R":
                        result.Token.Color = Colors.Red;
                        break;
                    case "G":
                        result.Token.Color = Colors.Green;
                        break;
                    case "B":
                        result.Token.Color = Colors.Blue;
                        break;
                    case "Y":
                        result.Token.Color = Colors.Yellow;
                        break;
                }
            }

            return result;
        }
    }
}