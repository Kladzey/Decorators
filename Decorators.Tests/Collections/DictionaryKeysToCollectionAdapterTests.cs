using System.Collections.Generic;
using System.Globalization;
using AutoFixture;
using FluentAssertions;
using Kladzey.Decorators.Collections;
using Xunit;

namespace Kladzey.Decorators.Tests.Collections
{
    public class DictionaryKeysToCollectionAdapterTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ClearTest()
        {
            // Given
            var dictionary = _fixture.Create<Dictionary<int, string>>();
            var sut = CreateSut(dictionary);

            // When
            sut.Clear();

            // Then
            dictionary.Should().BeEmpty();
        }

        [Fact]
        public void CopyToTest()
        {
            // Given
            var dictionary = _fixture.Create<Dictionary<int, string>>();
            var sut = CreateSut(dictionary);
            var targetArray = new int[dictionary.Count];

            // When
            sut.CopyTo(targetArray, 0);

            // Then
            targetArray.Should().BeEquivalentTo(dictionary.Keys);
        }

        [Fact]
        public void MainTest()
        {
            // Given
            var dictionary = new Dictionary<int, string>();
            var sut = CreateSut(dictionary);

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
            dictionary.Should().BeEquivalentTo(new Dictionary<int, string>()
            {
                { 1, "1" },
                { 3, "3" },
                { 2, "2" },
                { 5, "5" }
            });
        }

        private static DictionaryKeysToCollectionAdapter<int, string> CreateSut(IDictionary<int, string> dictionary)
        {
            return new DictionaryKeysToCollectionAdapter<int, string>(dictionary, key => key.ToString(CultureInfo.InvariantCulture));
        }
    }
}
