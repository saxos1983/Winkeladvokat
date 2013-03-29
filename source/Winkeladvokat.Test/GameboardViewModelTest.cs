namespace Winkeladvokat
{
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class GameboardViewModelTest
    {
        private GameBoardViewModel testee;
        private IGameBoard gameboard;

        [SetUp]
        public void Setup()
        {
            this.gameboard = A.Fake<IGameBoard>();
            this.testee = new GameBoardViewModel(this.gameboard);
        }

        [Test]
        public void HasListOfFieldViewModelsAfterConstruction()
        {
            A.CallTo(() => this.gameboard.Fields).Returns(new List<Field> { A.Fake<Field>() });
            this.testee.Loaded();

            this.testee.FieldViewModels.Count().Should().Be(1);
        }

        [Test]
        public void DisplaysMessageWhenInvalidMoveWasPerformed()
        {
            var fieldViewModel = A.Fake<FieldViewModel>();
            A.CallTo(() => this.gameboard.PlaceToken(fieldViewModel.Field)).Returns(new MoveResult("Invalid Move"));

            this.testee.PlaceTokenCommand.Execute(fieldViewModel);

            this.testee.MoveResultText.Should().Be("Invalid Move");
            this.testee.IsMessageVisible.Should().Be(true);
        }

        [Test]
        public void ScoreResultNotifyPropertyChangedMustHaveHappened_WhenMoveWasPerformed()
        {
            var fieldViewModel = A.Fake<FieldViewModel>();

            this.testee.MonitorEvents();

            this.testee.PlaceTokenCommand.Execute(fieldViewModel);

            this.testee.ShouldRaisePropertyChangeFor(t => t.Scores);
        }
    }
}
