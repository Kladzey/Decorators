using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Kladzey.Decorators.Collections;
using Xunit;

namespace Kladzey.Decorators.Tests.Collections
{
    public class DictionaryValidationDecoratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void AddingNotValidValueTest()
        {
            // Given
            var sut = new DictionaryValidationDecorator<int, string>(_fixture.Create<Dictionary<int, string>>(), (key, value) => value != null);

            // When
            var actions = new[]
            {
                sut.Invoking(s => s[_fixture.Create<int>()] = null),
                sut.Invoking(s => s.Add(_fixture.Create<int>(), null)),
                sut.Invoking(s => s.Add(new KeyValuePair<int, string>(_fixture.Create<int>(), null)))
            };

            // Then
            foreach (var action in actions)
            {
                action.Should().Throw<ArgumentException>().WithMessage("Key/value pair is not valid.");
            }
        }
    }
}
