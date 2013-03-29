using Machine.Specifications;

namespace Namespace
{
    using Winkeladvokat;
    using Winkeladvokat.Move;
    using System.Linq;
    using FluentAssertions;

    [Subject(typeof(Winkelzug), "Winkelzug is performed")]
    public class WhenAWinkelzugIsPerformed
    {
        static GameBoardViewModel gameBoardViewModel;
        static PerformMove performMove;

        Establish context = () =>
            {
                gameBoardViewModel = new GameBoardViewModel(new GameBoard(new FieldListFactory(), 4));
                performMove = new PerformMove(gameBoardViewModel);
            };

        Because of = () => { System.Console.WriteLine(""); };

        It should_remove_the_advokatenstein_from_the_old_position = () =>
            { 
                gameBoardViewModel.FieldViewModels.Single(f => f.Row == 0 && f.Column == 0).Field.Token.Should().BeNull();
            };
    }
}