using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kladzey.Decorators.Collections;
using Moq;
using Xunit;

namespace Kladzey.Decorators.Tests.Collections
{
    public class CollectionAdapterWithDisposingTests
    {
        [Fact]
        public void AddShouldDisposeItemOnExceptionTest()
        {
            // Given
            var mock = new Mock<IDisposableValue<int>>();

            var exception = new Exception();

            var internalCollectionMock = new Mock<ICollection<IDisposableValue<int>>>();
            internalCollectionMock.Setup(c => c.Add(It.IsAny<IDisposableValue<int>>())).Throws(exception);

            var sut = new CollectionAdapterWithDisposing<IDisposableValue<int>, int>(
                internalCollectionMock.Object,
                i => i.Value,
                v => mock.Object);

            // When
            var thrownException = sut.Invoking(s => s.Add(0)).Should().Throw<Exception>().Which;

            // Then
            thrownException.Should().BeSameAs(exception);
            mock.Verify(v => v.Dispose());
        }

        [Fact]
        public void ClearTest()
        {
            // Given
            var originalCollection = Enumerable.Range(1, 5)
                .Select(i => Mock.Of<IDisposableValue<int>>())
                .ToList();
            var internalCollection = originalCollection.ToList();
            var sut = new CollectionAdapterWithDisposing<IDisposableValue<int>, int>(
                internalCollection,
                i => i.Value,
                v => throw new Exception("This should not be called."));

            // When
            sut.Clear();

            // Then
            internalCollection.Should().BeEmpty();
            foreach (var item in originalCollection)
            {
                Mock.Get(item).Verify(v => v.Dispose());
            }
        }

        [Fact]
        public void RemoveTest()
        {
            // Given
            var originalCollection = Enumerable.Range(1, 2)
                .Select(i => Mock.Of<IDisposableValue<int>>(v => v.Value == i))
                .ToList();
            var internalCollection = originalCollection.ToList();
            var sut = new CollectionAdapterWithDisposing<IDisposableValue<int>, int>(
                internalCollection,
                i => i.Value,
                v => throw new Exception("This should not be called."));

            // When
            var removeResult = sut.Remove(1);

            // Then
            removeResult.Should().BeTrue();
            internalCollection.Select(v => v.Value).Should().BeEquivalentTo(2);
            Mock.Get(originalCollection[0]).Verify(v => v.Dispose());
            Mock.Get(originalCollection[1]).Verify(v => v.Dispose(), Times.Never());
        }
    }
}
