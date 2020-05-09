using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Kladzey.Wrappers.Collections;
using Xunit;

namespace Kladzey.Wrappers.Tests.Collections
{
    public class CollectionAdapterTests
    {
        private readonly List<(int Value, string ValueString)> _internalCollection;
        private readonly CollectionAdapter<(int Value, string ValueString), int> _sut;

        public CollectionAdapterTests()
        {
            _internalCollection = new List<(int Value, string ValueString)>
            {
                (1, "1"),
                (2, "2"),
                (3, "3"),
            };
            _sut = new CollectionAdapter<(int Value, string ValueString), int>(
                _internalCollection,
                i => i.Value,
                v => (v, v.ToString(CultureInfo.InvariantCulture)));
        }

        [Fact]
        public void ClearTest()
        {
            // When
            _sut.Clear();

            // Then
            _internalCollection.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0, 3, 4)]
        [InlineData(1, 3, 3)]
        [InlineData(3, 3, -1)]
        [InlineData(3, 3, 1)]
        [InlineData(3, 3, 3)]
        public void CopyToShouldFailOnIndexOutOfRangeTest(int internalCollectionSize, int targetArraySize, int arrayIndex)
        {
            // Given
            _internalCollection.Clear();
            _internalCollection.AddRange(Enumerable.Range(1, internalCollectionSize).Select(v => (v, v.ToString(CultureInfo.InvariantCulture))));
            var targetArray = new int[targetArraySize];

            // When
            var act = _sut.Invoking(s => s.CopyTo(targetArray, arrayIndex));

            // Then
            act.Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("arrayIndex");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CopyToShouldNotChangeTargetArrayIfInternalIsEmptyTest(int arrayIndex)
        {
            // Given
            _internalCollection.Clear();
            var targetArray = new int[3];

            // When
            _sut.CopyTo(targetArray, arrayIndex);

            // Then
            targetArray.Should().AllBeEquivalentTo(0);
        }

        [Fact]
        public void CopyToTest()
        {
            // Given
            var targetArray = new int[_internalCollection.Count];

            // When
            _sut.CopyTo(targetArray, 0);

            // Then
            targetArray.Should().Equal(_internalCollection.Select(i => i.Value));
        }

        [Fact]
        public void MainTest()
        {
            // When
            _sut.Clear();
            _sut.Add(1);
            _sut.Add(3);
            _sut.Add(2);
            _sut.Add(4);
            _sut.Add(5);
            var removedResult = _sut.Remove(4);
            var notRemovedResult = _sut.Remove(6);
            var containsResult = _sut.Contains(2);
            var notContainsResult = _sut.Contains(4);

            // Then
            _sut.Count.Should().Be(4);
            _sut.IsReadOnly.Should().BeFalse();
            containsResult.Should().BeTrue();
            notContainsResult.Should().BeFalse();
            removedResult.Should().BeTrue();
            notRemovedResult.Should().BeFalse();
            _sut.Should().Equal(1, 3, 2, 5);
            _internalCollection.Should().Equal((1, "1"), (3, "3"), (2, "2"), (5, "5"));
        }

        [Fact]
        public void RemoveShouldReturnFalseIfItemNotExistTest()
        {
            // When
            var result = _sut.Remove(4);

            // Then
            using (new AssertionScope())
            {
                result.Should().BeFalse();
                _internalCollection.Should().BeEquivalentTo((1, "1"), (2, "2"), (3, "3"));
            }
        }

        [Fact]
        public void RemoveTest()
        {
            // When
            var result = _sut.Remove(2);

            // Then
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                _internalCollection.Should().BeEquivalentTo((1, "1"), (3, "3"));
            }
        }
    }
}
