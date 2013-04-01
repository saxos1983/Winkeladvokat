namespace Winkeladvokat.Move
{
    using System;
    using System.Collections.Generic;

    public class Moves
    {
        private IList<Field> gameBoardFields;

        private MoveBase actualMove;

        public Moves(IList<Field> gameBoardFields)
        {
            this.gameBoardFields = gameBoardFields;
        }

        public IList<Field> PerformMoves(string movesData)
        {
            string[] moveDataTokens = movesData.Split('=');
            this.actualMove = this.GetMoveHandler(moveDataTokens[0]);
            IList<Field> fieldsForMovement = this.ParseGameBoardFields(moveDataTokens[1]);

            foreach (var actualFieldForMove in fieldsForMovement)
            {
                this.actualMove.PerformMove(actualFieldForMove);
            }

            return this.gameBoardFields;
        }

        public bool IsActualMoveFinished { get { return this.actualMove.IsFinished; } }
        public bool IsActualMoveValid { get { return this.actualMove.IsValid; } }

        private IList<Field> ParseGameBoardFields(string movesData)
        {
            var result = new List<Field>();
            string[] moves = movesData.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var move in moves)
            {
                var fieldPositionStartIndex = move.IndexOf('[') + 1;
                var fieldPositionEndIndex = move.IndexOf(']') - 1;

                var row = int.Parse(move.Substring(fieldPositionStartIndex, 1));
                var column = int.Parse(move.Substring(fieldPositionEndIndex, 1));

                result.Add(this.gameBoardFields.SelectByPosition(row, column));
            }

            return result;
        }

        private MoveBase GetMoveHandler(string movesType)
        {
            switch (movesType)
            {
                case "PZ":
                    return new Paragraphenzug(this.gameBoardFields);
                case "WZ":
                    return new Winkelzug(this.gameBoardFields);
                default:
                    return null;
            }
        }
    }
}
