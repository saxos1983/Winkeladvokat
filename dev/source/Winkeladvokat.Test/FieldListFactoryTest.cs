namespace Winkeladvokat
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class FieldListFactoryTest
    {
        private FieldListFactory testee;

        [SetUp]
        public void Setup()
        {
            this.testee = new FieldListFactory();
        }

        [Test]
        public void CanCreateFieldList()
        {
            var fieldList = this.testee.Create(1, 1);

            fieldList.Should().NotBeNull();
        }

        [TestCase(1, 1)]
        [TestCase(2, 4)]
        [TestCase(8, 64)]
        public void NumberOfFieldsEqualsTheSquareOfTheGivenSize(int size, int expectedNumberOfFields)
        {
            var fieldList = this.testee.Create(size, 1);

            fieldList.Count().Should().Be(expectedNumberOfFields);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 1)]
        [TestCase(2, 1, 0)]
        [TestCase(3, 1, 1)]
        public void RowsAndColumnsArePrefilledCorrectly(int index, int row, int column)
        {
            var fieldList = this.testee.Create(2, 1).ToList();

            fieldList.ElementAt(0).Row.Should().Be(0);
            fieldList.ElementAt(0).Column.Should().Be(0);
        }

        [Test]
        public void ValuesArePrefilledCorrectlyForA3X3Board()
        {
            var threeTimesThreeGameBoard = new[]
            {
                new[] { 0, 2, 0 },
                new[] { 2, 4, 2 },
                new[] { 0, 2, 0 }
            };

            var fieldList = this.testee.Create(3, 1).ToList();

            this.CheckFieldValues(3, threeTimesThreeGameBoard, fieldList);
        }

        [Test]
        public void ValuesArePrefilledCorrectlyForA4X4Board()
        {
            var fourTimesFourGameBoard = new[]
            {
                new[] { 0, 2, 2, 0 },
                new[] { 2, 4, 4, 2 },
                new[] { 2, 4, 4, 2 },
                new[] { 0, 2, 2, 0 }
            };

            var fieldList = this.testee.Create(4, 1).ToList();
            this.CheckFieldValues(4, fourTimesFourGameBoard, fieldList);
        }

        [Test]
        public void ThereAreAdvocatTokensOppositeCornersWhenPlayerCountEquals2()
        {
            var fieldList = this.testee.Create(4, 2).ToList();

            fieldList.SelectByPosition(0, 0).Token.Should().BeOfType<AdvocatToken>();
            fieldList.SelectByPosition(3, 3).Token.Should().BeOfType<AdvocatToken>();
        }

        [Test]
        public void ThereAreAdvocatTokensOppositeCornersWhenPlayerCountEquals3()
        {
            var fieldList = this.testee.Create(4, 3).ToList();

            fieldList.SelectByPosition(0, 0).Token.Should().BeOfType<AdvocatToken>();
            fieldList.SelectByPosition(0, 3).Token.Should().BeOfType<AdvocatToken>();
            fieldList.SelectByPosition(3, 3).Token.Should().BeOfType<AdvocatToken>();
        }

        [Test]
        public void ThereAreAdvocatTokensInAllCornersWhenPlayerCountEquals4()
        {
            var fieldList = this.testee.Create(4, 4).ToList();

            fieldList.SelectByPosition(0, 0).Token.Should().BeOfType<AdvocatToken>();
            fieldList.SelectByPosition(3, 3).Token.Should().BeOfType<AdvocatToken>();
            fieldList.SelectByPosition(0, 3).Token.Should().BeOfType<AdvocatToken>();
            fieldList.SelectByPosition(3, 0).Token.Should().BeOfType<AdvocatToken>();
        }

        private void CheckFieldValues(int size, int[][] expectedValues, IList<Field> fieldList)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    fieldList.Single(f => f.Row == i && f.Column == j).Value.Should().Be(expectedValues[i][j], string.Format("{0}/{1}", i, j));
                }
            }
        }
    }
}