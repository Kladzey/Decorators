using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kladzey.Decorators.Collections;
using Moq;
using Xunit;

namespace Kladzey.Decorators.Tests.Collections
{
    public class DictionaryKeysToCollectionAdapterWithDisposingTests
    {
        [Fact]
        public void AddIsFailedTest()
        {
            // Given
            var mock = new Mock<IDisposableValue<int>>();

            var exception = new Exception();

            var internalDictionaryMock = new Mock<IDictionary<int, IDisposableValue<int>>>();
            internalDictionaryMock.Setup(c => c.Add(It.IsAny<int>(), It.IsAny<IDisposableValue<int>>()))
                .Throws(exception);

            var sut = new DictionaryKeysToCollectionAdapterWithDisposing<int, IDisposableValue<int>>(
                internalDictionaryMock.Object,
                v => mock.Object);

            // When
            var thrownException = sut.Invoking(s => s.Add(1)).Should().Throw<Exception>().Which;

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
            var internalDictionary = mocks.Select(m => m.Object).ToDictionary(v => v.Value);
            var sut = new DictionaryKeysToCollectionAdapterWithDisposing<int, IDisposableValue<int>>(
                internalDictionary,
                v => { throw new Exception(); });

            // When
            sut.Clear();

            // Then
            internalDictionary.Should().BeEmpty();
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
            var internalCollection = mocks.Select(m => m.Object).ToDictionary(v => v.Value);
            var sut = new DictionaryKeysToCollectionAdapterWithDisposing<int, IDisposableValue<int>>(
                internalCollection,
                v => { throw new Exception(); });

            // When
            var removeResult = sut.Remove(0);

            // Then
            removeResult.Should().BeTrue();
            internalCollection.Should().BeEquivalentTo(new Dictionary<int, IDisposableValue<int>>
                {{mocks[1].Object.Value, mocks[1].Object}});
            mocks[0].Verify(v => v.Dispose());
            mocks[1].Verify(v => v.Dispose(), Times.Never());
        }
    }
}
