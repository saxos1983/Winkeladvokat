namespace Winkeladvokat
{
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class MoveResultTest
    {
        [Test]
        public void IsValidEqualsTrue_WhenCreatingWithoutMessage()
        {
            var testee = MoveResult.CreateValidResult();

            testee.IsValid.Should().BeTrue();
        }

        [Test]
        public void IsValidEqualsFalse_WhenCreatingWithMessage()
        {
            var testee = MoveResult.CreateInvalidResult("Error");

            testee.IsValid.Should().BeFalse();
        }

        [Test]
        public void MessageIsAvailable_WhenCreatingWithMessage()
        {
            var testee = MoveResult.CreateInvalidResult("Error");

            testee.Message.Should().Be("Error");
        }
    }
}