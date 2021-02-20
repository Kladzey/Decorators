using System;
using System.Collections.Generic;
using FluentAssertions;
using Kladzey.Wrappers.Collections;
using Xunit;

namespace Kladzey.Wrappers.Tests.Collections
{
    public class DictionaryDefaultValueDecoratorTests
    {
        [Fact]
        public void AddShouldNotAddDefaultValueButAnywayShouldThrowExceptionIfItemExistTest()
        {
            // Given
            var dictionary = new Dictionary<int, string> {{1, "1"}};
            var sut = new DictionaryDefaultValueDecorator<int, string>(
                dictionary,
                EqualityComparer<string>.Default,
                "default value");

            // When
            var action = sut.Invoking(s => s.Add(1, "default value"));

            // Then
            action.Should().Throw<ArgumentException>().WithMessage("Duplicate key*").Which.ParamName.Should().Be("key");
        }

        [Fact]
        public void GetShouldReturnDefaultValueIfItemNotExistTest()
        {
            // Given
            var sut = new DictionaryDefaultValueDecorator<int, string>(
                new Dictionary<int, string>(),
                EqualityComparer<string>.Default,
                "default value");

            // When
            var result = sut[1];

            // Then
            result.Should().Be("default value");
        }

        [Fact]
        public void SetShouldRemoveItemIfValueIsDefaultTest()
        {
            // Given
            var dictionary = new Dictionary<int, string> {{1, "1"}, {2, "default value"}};
            var sut = new DictionaryDefaultValueDecorator<int, string>(dictionary, EqualityComparer<string>.Default,
                "default value");

            // When
            sut[1] = "default value";

            // Then
            sut.Should().BeEmpty();
            dictionary.Should().BeEmpty();
        }
    }
}
