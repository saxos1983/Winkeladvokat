namespace Winkeladvokat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;

    public class GameBoard : IGameBoard
    {
        private const int Size = 8;

        private readonly IFieldListFactory fieldListFactory;
        private readonly IMoveFactory moveFactory;

        private IMove currentMove;

        public GameBoard(IFieldListFactory fieldListFactory, int numberOfPlayers)
        {
            this.fieldListFactory = fieldListFactory;
            this.moveFactory = new MoveFactory();

            this.Fields = this.fieldListFactory.Create(Size, numberOfPlayers);
        }

        public GameBoard(IFieldListFactory fieldListFactory, IMoveFactory moveFactory, int numberOfPlayers)
            : this(fieldListFactory, numberOfPlayers)
        {
            this.moveFactory = moveFactory;
        }

        public IEnumerable<Field> Fields { get; private set; }

        public bool IsFinished { get; private set; }

        public IEnumerable<Color> Winners
        {
            get
            {
                int highestScore = this.GetCurrentScore().Max(s => s.Score);

                return this.GetCurrentScore().Where(s => s.Score == highestScore).Select(s => s.Color);
            }
        }

        public IEnumerable<ScoreResult> GetCurrentScore()
        {
            foreach (var advocatToken in this.Fields.Where(f => f.Token is AdvocatToken))
            {
                var advocatColor = advocatToken.Token.Color;
                var paragraphTokens = this.Fields.Where(f => f.Token is ParagraphToken && f.Token.Color == advocatColor);
                var score = paragraphTokens.Sum(t => t.Value);

                yield return new ScoreResult(advocatToken.Token.Color, score);
            }
        }

        public MoveResult PlaceToken(Field field)
        {
            if (this.currentMove == null)
            {
                if (field.Token == null)
                {
                    return MoveResult.CreateInvalidResult("Der erste Zug muss von einem Feld mit einem Spielstein stattfinden.");
                }

                var advokatTokenToTurn = this.Fields.First(t => t.Token is AdvocatToken && t.Token.Color == field.Token.Color);

                if (this.IsWinkelzugPossible(advokatTokenToTurn) == false)
                {
                    this.IsFinished = true;
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("Spiel beendet!" + Environment.NewLine + "Gewinner:");
                    foreach (var winner in this.Winners)
                    {
                        stringBuilder.Append(Environment.NewLine + winner.GetColorName());
                    }

                    return MoveResult.CreateInvalidResult(stringBuilder.ToString());
                }

                this.currentMove = this.moveFactory.CreateByToken(field.Token, this.Fields);
            }

            var moveResult = this.currentMove.PerformMove(field);

            if (this.currentMove.IsFinished)
            {
                this.currentMove = null;
            }

            return moveResult;
        }

        public bool IsWinkelzugPossible(Field currentAdvokatTokenPosition)
        {
            return this.CheckVertical(currentAdvokatTokenPosition, -1) ||
                this.CheckHorizontal(currentAdvokatTokenPosition, -1) ||
                this.CheckHorizontal(currentAdvokatTokenPosition, 1) ||
                this.CheckVertical(currentAdvokatTokenPosition, 1);
        }

        private static bool FieldExistsAndIsFree(Field f)
        {
            return f != null && f.Token == null;
        }

        private bool CheckVertical(Field currentAdvokatTokenPosition, int numberofColumnsToMoveDown)
        {
            var field = this.MoveVertical(currentAdvokatTokenPosition, numberofColumnsToMoveDown);
            var candoWinkelZug = false;
            if (FieldExistsAndIsFree(field))
            {
                var tempField = this.MoveHorizontal(field, -1);
                if (FieldExistsAndIsFree(tempField))
                {
                    return true;
                }

                tempField = this.MoveHorizontal(field, 1);
                if (FieldExistsAndIsFree(tempField))
                {
                    return true;
                }

                candoWinkelZug = this.CheckVertical(field, numberofColumnsToMoveDown);
            }

            if (candoWinkelZug)
            {
                return true;
            }

            return false;
        }

        private bool CheckHorizontal(Field currentAdvokatTokenPosition, int numberOfRowsToMoveRight)
        {
            var field = this.MoveHorizontal(currentAdvokatTokenPosition, numberOfRowsToMoveRight);
            bool canDoWinkelZug = false;
            if (FieldExistsAndIsFree(field))
            {
                var tempField = this.MoveVertical(field, -1);
                if (FieldExistsAndIsFree(tempField))
                {
                    return true;
                }

                tempField = this.MoveVertical(field, 1);
                if (FieldExistsAndIsFree(tempField))
                {
                    return true;
                }

                canDoWinkelZug = this.CheckHorizontal(field, 1);
            }

            if (canDoWinkelZug)
            {
                return true;
            }

            return false;
        }

        private Field MoveVertical(Field currentAdvokatTokenPosition, int rowsToMoveDown)
        {
            if ((currentAdvokatTokenPosition.Row > 0 && rowsToMoveDown < 0) || (currentAdvokatTokenPosition.Row < 7 && rowsToMoveDown > 0))
            {
                return this.Fields.SelectByPosition(currentAdvokatTokenPosition.Row + rowsToMoveDown, currentAdvokatTokenPosition.Column);
            }

            return null;
        }

        private Field MoveHorizontal(Field currentAdvokatTokenPosition, int columnsToMoveRight)
        {
            if ((currentAdvokatTokenPosition.Column > 0 && columnsToMoveRight < 0) || (currentAdvokatTokenPosition.Column < 7 && columnsToMoveRight > 0))
            {
                return this.Fields.SelectByPosition(currentAdvokatTokenPosition.Row, currentAdvokatTokenPosition.Column + columnsToMoveRight);
            }

            return null;
        }
    }
}