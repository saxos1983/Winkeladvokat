namespace Winkeladvokat
{
    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class FieldViewModelTest
    {
        private FieldViewModel testee;
        private Field field;

        [SetUp]
        public void Setup()
        {
            this.field = A.Fake<Field>();
            this.testee = new FieldViewModel(this.field);
        }

        [Test]
        public void HasTokenWhenTokenInFieldIsNotNull()
        {
            this.field.Token = A.Fake<Token>();

            this.testee.HasToken.Should().Be(true);
        }
    }
}
