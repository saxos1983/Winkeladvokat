namespace Winkeladvokat
{
    using System.Collections.Generic;

    public interface IGameBoardFactory
    {
        /// <summary>
        /// Creates a game board initialized with fields from the provided fields data <see cref="fieldsData" />
        /// </summary>
        /// <param name="fieldsData">A comma separated list of fields. Each field is defined with:
        /// - it's token type (PT = ParagraphToken, AT = AdvocatToken)
        /// - it's token color [optional] (R = Red, G = Green, Y = Yellow, B = Blue)
        /// - it's position on the board enclosed by brackets and coordinates separated by a comma ([3,2] = 3rd Row, 2nd Column)
        /// E.g. PTR[3,2];AT[4,2] will create a Red ParagraphToken at Row 3 Column 2 and a AdvocatToken at Row 4, Column 2 with no color.</param>
        /// <param name="dimension">The dimension of the board. E.g. 8 creates a 8x8 board.</param>
        /// <returns>
        /// A game board with a given dimension initialized with fields from the provided field data.
        /// </returns>
        IList<Field> Create(string fieldsData, int dimension);

        /// <summary>
        /// Creates a game board with the provided dimension.
        /// </summary>
        /// <param name="dimension">The dimension of the board. E.g. 8 creates a 8x8 board.</param>
        /// <returns>
        /// A game board with a given dimension initialized with fields from the provided field data.        
        /// </returns>
        IList<Field> Create(int dimension);
    }
}