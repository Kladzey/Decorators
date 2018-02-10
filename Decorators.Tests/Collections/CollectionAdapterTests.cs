using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Kladzey.Decorators.Collections;
using Xunit;

namespace Kladzey.Decorators.Tests.Collections
{
    public class CollectionAdapterTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ClearTest()
        {
            // Given
            var internalCollection = _fixture.CreateMany<(int Value, string ValueString)>().ToList();
            var sut = CreateSut(internalCollection);

            // When
            sut.Clear();

            // Then
            internalCollection.Should().BeEmpty();
        }

        [Fact]
        public void CopyToTest()
        {
            // Given
            var internalCollection = _fixture.CreateMany<(int Value, string ValueString)>().ToList();
            var sut = CreateSut(internalCollection);
            var targetArray = new int[internalCollection.Count];

            // When
            sut.CopyTo(targetArray, 0);

            // Then
            targetArray.Should().Equal(internalCollection.Select(i => i.Value));
        }

        [Fact]
        public void MainTest()
        {
            // Given
            var internalCollection = new List<(int Value, string ValueString)>();
            var sut = CreateSut(internalCollection);

            // When
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
            sut.Should().Equal(new[] { 1, 3, 2, 5 });
            internalCollection.Should().Equal(new[] { (1, "1"), (3, "3"), (2, "2"), (5, "5") });
        }

        private static CollectionAdpater<(int Value, string ValueString), int> CreateSut(ICollection<(int Value, string ValueString)> internalCollection)
        {
            return new CollectionAdpater<(int Value, string ValueString), int>(internalCollection, i => i.Value, v => (v, v.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
