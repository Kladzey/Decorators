using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Kladzey.Decorators.Collections;
using Xunit;

namespace Kladzey.Decorators.Tests.Collections
{
    public class DictionaryDefaultValueDecoratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void AddOfDefaultValueForExistingKeyTest()
        {
            // Given
            var defaultValue = _fixture.Create<string>();
            var dictionary = _fixture.Create<Dictionary<int, string>>();
            var sut = new DictionaryDefaultValueDecorator<int, string>(dictionary, EqualityComparer<string>.Default, defaultValue);

            // When
            var action = sut.Invoking(s => s.Add(dictionary.Keys.First(), defaultValue));

            // Then
            action.Should().Throw<ArgumentException>().Where(e => e.ParamName == "key");
        }

        [Fact]
        public void GetNotExistingValueTest()
        {
            // Given
            var defaultValue = _fixture.Create<string>();
            var sut = new DictionaryDefaultValueDecorator<int, string>(new Dictionary<int, string>(), EqualityComparer<string>.Default, defaultValue);

            // When
            var result = sut[_fixture.Create<int>()];

            // Then
            result.Should().Be(defaultValue);
        }

        [Fact]
        public void SetDefaultValueTest()
        {
            // Given
            var defaultValue = _fixture.Create<string>();
            var key1 = _fixture.Create<int>();
            var key2 = _fixture.Create<int>();
            var dictionary = new Dictionary<int, string>
            {
                { key1, _fixture.Create<string>() },
                { key2, defaultValue }
            };
            var sut = new DictionaryDefaultValueDecorator<int, string>(dictionary, EqualityComparer<string>.Default, defaultValue);

            // When
            sut[key1] = defaultValue;

            // Then
            dictionary.Should().BeEmpty();
        }
    }
}
