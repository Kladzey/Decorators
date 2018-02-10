using System;
using System.Linq;
using FluentAssertions;
using Kladzey.Decorators;
using Kladzey.Decorators.Collections;
using Moq;
using Xunit;

namespace Decorators.Tests.Collections
{
    public class CollectionAdapterWithDisposingTests
    {
        [Fact]
        public void ClearTest()
        {
            // Given
            var mocks = Enumerable.Range(0, 5)
                .Select(i =>
                {
                    var mock = new Mock<IDisposableValue<int>>();
                    mock.Setup(x => x.Value).Returns(i);
                    return mock;
                })
                .ToList();
            var internalCollection = mocks.Select(m => m.Object).ToList();
            var sut = new CollectionAdpaterWithDisposing<IDisposableValue<int>, int>(
                internalCollection,
                i => i.Value,
                v =>
                {
                    throw new Exception();
                });

            // When
            sut.Clear();

            // Then
            internalCollection.Should().BeEmpty();
            foreach (var mock in mocks)
            {
                mock.Verify(v => v.Dispose());
            }
        }

        [Fact]
        public void RemoveTest()
        {
            // Given
            var mocks = Enumerable.Range(0, 2)
                .Select(i =>
                {
                    var mock = new Mock<IDisposableValue<int>>();
                    mock.Setup(x => x.Value).Returns(i);
                    return mock;
                })
                .ToList();
            var internalCollection = mocks.Select(m => m.Object).ToList();
            var sut = new CollectionAdpaterWithDisposing<IDisposableValue<int>, int>(
                internalCollection,
                i => i.Value,
                v =>
                {
                    throw new Exception();
                });

            // When
            var removeResult = sut.Remove(0);

            // Then
            removeResult.Should().BeTrue();
            internalCollection.Should().BeEquivalentTo(new[] { mocks[1].Object });
            mocks[0].Verify(v => v.Dispose());
            mocks[1].Verify(v => v.Dispose(), Times.Never());
        }
    }
}
