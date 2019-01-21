using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Kladzey.Decorators.Collections;
using Moq;
using Xunit;

namespace Kladzey.Decorators.Tests.Collections
{
    public class CollectionAdapterWithDisposingTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void AddIsFailedTest()
        {
            // Given
            var mock = new Mock<IDisposableValue<int>>();

            var exception = _fixture.Create<Exception>();

            var internalCollectionMock = new Mock<ICollection<IDisposableValue<int>>>();
            internalCollectionMock.Setup(c => c.Add(It.IsAny<IDisposableValue<int>>())).Throws(exception);

            var sut = new CollectionAdapterWithDisposing<IDisposableValue<int>, int>(
                internalCollectionMock.Object,
                i => i.Value,
                v => mock.Object);

            // When
            var thrownException = sut.Invoking(s => s.Add(_fixture.Create<int>())).Should().Throw<Exception>().Which;

            // Then
            thrownException.Should().BeSameAs(exception);
            mock.Verify(v => v.Dispose());
        }

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
            var sut = new CollectionAdapterWithDisposing<IDisposableValue<int>, int>(
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
            var sut = new CollectionAdapterWithDisposing<IDisposableValue<int>, int>(
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
