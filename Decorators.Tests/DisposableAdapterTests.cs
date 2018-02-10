using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Kladzey.Decorators.Tests
{
    public class DisposableAdapterTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void DisposeTest()
        {
            // Given
            var value = _fixture.Create<int>();
            var disposedValue = (int?)null;
            var sut = new DisposableAdapter<int>(value, v => disposedValue = v);

            // When
            sut.Dispose();

            // Then
            disposedValue.Should().Be(value);
        }

        [Fact]
        public void ValueTest()
        {
            // Given
            var value = _fixture.Create<int>();
            var sut = new DisposableAdapter<int>(value, v => { });

            // When
            var result = sut.Value;

            // Then
            result.Should().Be(value);
        }
    }
}
