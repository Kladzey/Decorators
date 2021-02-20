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
        private readonly List<(int Value, string ValueString)> internalCollection;
        private readonly CollectionAdapter<(int Value, string ValueString), int> sut;

        public CollectionAdapterTests()
        {
            internalCollection = new List<(int Value, string ValueString)> {(1, "1"), (2, "2"), (3, "3"),};
            sut = new CollectionAdapter<(int Value, string ValueString), int>(
                internalCollection,
                i => i.Value,
                v => (v, v.ToString(CultureInfo.InvariantCulture)));
        }

        [Fact]
        public void ClearTest()
        {
            // When
            sut.Clear();

            // Then
            internalCollection.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0, 3, 4)]
        [InlineData(1, 3, 3)]
        [InlineData(3, 3, -1)]
        [InlineData(3, 3, 1)]
        [InlineData(3, 3, 3)]
        public void CopyToShouldFailOnIndexOutOfRangeTest(int internalCollectionSize, int targetArraySize,
            int arrayIndex)
        {
            // Given
            internalCollection.Clear();
            internalCollection.AddRange(Enumerable.Range(1, internalCollectionSize)
                .Select(v => (v, v.ToString(CultureInfo.InvariantCulture))));
            var targetArray = new int[targetArraySize];

            // When
            var act = sut.Invoking(s => s.CopyTo(targetArray, arrayIndex));

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
            internalCollection.Clear();
            var targetArray = new int[3];

            // When
            sut.CopyTo(targetArray, arrayIndex);

            // Then
            targetArray.Should().AllBeEquivalentTo(0);
        }

        [Fact]
        public void CopyToTest()
        {
            // Given
            var targetArray = new int[internalCollection.Count];

            // When
            sut.CopyTo(targetArray, 0);

            // Then
            targetArray.Should().Equal(internalCollection.Select(i => i.Value));
        }

        [Fact]
        public void MainTest()
        {
            // When
            sut.Clear();
            sut.Add(1);
            sut.Add(3);
            sut.Add(2);
            sut.Add(4);
            sut.Add(5);
            var removedResult = sut.Remove(4);
            var notRemovedResult = sut.Remove(6);
            var containsResult = sut.Contains(2);
            var notContainsResult = sut.Contains(4);

            // Then
            sut.Count.Should().Be(4);
            sut.IsReadOnly.Should().BeFalse();
            containsResult.Should().BeTrue();
            notContainsResult.Should().BeFalse();
            removedResult.Should().BeTrue();
            notRemovedResult.Should().BeFalse();
            sut.Should().Equal(1, 3, 2, 5);
            internalCollection.Should().Equal((1, "1"), (3, "3"), (2, "2"), (5, "5"));
        }

        [Fact]
        public void RemoveShouldReturnFalseIfItemNotExistTest()
        {
            // When
            var result = sut.Remove(4);

            // Then
            using (new AssertionScope())
            {
                result.Should().BeFalse();
                internalCollection.Should().BeEquivalentTo((1, "1"), (2, "2"), (3, "3"));
            }
        }

        [Fact]
        public void RemoveTest()
        {
            // When
            var result = sut.Remove(2);

            // Then
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                internalCollection.Should().BeEquivalentTo((1, "1"), (3, "3"));
            }
        }
    }
}
