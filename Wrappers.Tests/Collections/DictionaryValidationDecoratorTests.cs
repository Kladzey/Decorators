using System;
using System.Collections.Generic;
using FluentAssertions;
using Kladzey.Wrappers.Collections;
using Xunit;

namespace Kladzey.Wrappers.Tests.Collections
{
    public class DictionaryValidationDecoratorTests
    {
        [Fact]
        public void AddingNotValidValueTest()
        {
            // Given
            var sut = new DictionaryValidationDecorator<int, string?>(
                new Dictionary<int, string?>(),
                (key, value) => value != null);

            // When
            var actions = new[]
            {
                sut.Invoking(s =>
                {
                    s[1] = null;
                }),
                sut.Invoking(s => s.Add(1, null)),
                sut.Invoking(s => s.Add(new KeyValuePair<int, string?>(1, null))),
            };

            // Then
            foreach (var action in actions)
            {
                action.Should().Throw<ArgumentException>().WithMessage("Key/value pair is not valid.");
            }
        }
    }
}
