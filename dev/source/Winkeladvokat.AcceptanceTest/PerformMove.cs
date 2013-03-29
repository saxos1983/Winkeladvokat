namespace Winkeladvokat
{
    using System;
    using System.Linq;

    public class PerformMove
    {
        private readonly GameBoardViewModel gameBoardViewModel;

        public PerformMove(GameBoardViewModel gameBoardViewModel)
        {
            this.gameBoardViewModel = gameBoardViewModel;
        }

        public void MakeWinkelzug(
            Func<FieldViewModel, bool> startFieldSelector,
            Func<FieldViewModel, bool> cornerFieldSelector,
            Func<FieldViewModel, bool> endFieldSelector)
        {
            // ARRANGE
            var startField = this.gameBoardViewModel.FieldViewModels.Single(startFieldSelector);
            var cornerField = this.gameBoardViewModel.FieldViewModels.Single(cornerFieldSelector);
            var endField = this.gameBoardViewModel.FieldViewModels.Single(endFieldSelector);

            // ACT
            this.gameBoardViewModel.PlaceTokenCommand.Execute(startField);
            this.gameBoardViewModel.PlaceTokenCommand.Execute(cornerField);
            this.gameBoardViewModel.PlaceTokenCommand.Execute(endField);
        }
    }
}
