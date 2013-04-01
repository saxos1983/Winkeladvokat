using Winkeladvokat.Tokens;

namespace Winkeladvokat
{
    using System.Collections.Generic;

    using FluentAssertions;
    using NUnit.Framework;
    using Winkeladvokat.Move;

    [TestFixture]
    public class MoveFactoryTest
    {
        private MoveFactory testee;

        [SetUp]
        public void Setup()
        {
            this.testee = new MoveFactory();
        }

        [Test]
        public void MustCreateWinkelzug_WhenFieldContainsAdvocatToken()
        {
            var field = new Field(0, 0) { Token = new AdvocatToken() };
            
            var move = this.testee.CreateByToken(field.Token, new List<Field> { field });

            move.Should().BeOfType<Winkelzug>();
        }

        [Test]
        public void MustCreateParagraphenzug_WhenFieldContainsParagraphToken()
        {
            var field = new Field(1, 0) { Token = new ParagraphToken() };

            var move = this.testee.CreateByToken(field.Token, new List<Field> { field });

            move.Should().BeOfType<Paragraphenzug>();
        }
    }
}