using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using FluentAssertions.Execution;
using Kladzey.Wrappers.Collections;
using Xunit;

namespace Kladzey.Wrappers.Tests.Collections
{
    public class ReadOnlyDictionaryValuesAdapterTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ContainsShouldReturnFalseIfNotContainsTest(int key)
        {
            // Given
            var sut = new ReadOnlyDictionaryValuesAdapter<int, string, int>(
                new Dictionary<int, int>() { { 0, 0 } },
                v => v.ToString(CultureInfo.InvariantCulture));

            // When
            var result = ((ICollection<KeyValuePair<int, string>>)sut).Contains(new KeyValuePair<int, string>(key, null));

            // Then
            result.Should().BeFalse();
        }

        [Fact]
        public void IsReadOnlyTest()
        {
            // Given
            var sut = new ReadOnlyDictionaryValuesAdapter<int, string, int>(new Dictionary<int, int>(), v => v.ToString(CultureInfo.InvariantCulture));

            // When
            var result = ((ICollection<KeyValuePair<int, string>>)sut).IsReadOnly;

            // Then
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(0, true, "1")]
        [InlineData(1, false, null)]
        public void TryGetValueTest(int key, bool expectedResult, string expectedValue)
        {
            // Given
            var sut = new ReadOnlyDictionaryValuesAdapter<int, string, int>(
                new Dictionary<int, int>() { { 0, 1 } },
                v => v.ToString(CultureInfo.InvariantCulture));

            // When
            var result = sut.TryGetValue(key, out var value);

            // Then
            using (new AssertionScope())
            {
                result.Should().Be(expectedResult);
                value.Should().Be(expectedValue);
            }
        }
    }
}
