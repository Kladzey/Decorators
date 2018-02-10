using System;
using System.Collections.Generic;
using AutoFixture;
using Decorators.Tests.TestUtils;
using FluentAssertions;
using Kladzey.Decorators.Collections;
using Xunit;

namespace Decorators.Tests.Collections
{
    public class DictionaryAccessDecoratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void DisabledTest()
        {
            // Given
            var sut = new DictionaryAccessDecorator<int, string>(_fixture.Create<Dictionary<int, string>>(), () => false);

            // When
            var actions = new[]
            {
                sut.InvokingFunc(s => s.Count),
                sut.InvokingFunc(s => s.IsReadOnly),
                sut.InvokingFunc(s => s.Keys),
                sut.InvokingFunc(s => s.Values),
                sut.InvokingFunc(s => s[_fixture.Create<int>()]),
                sut.Invoking(s => s[_fixture.Create<int>()] = _fixture.Create<string>()),
                sut.Invoking(s => s.Add(_fixture.Create<int>(), _fixture.Create<string>())),
                sut.Invoking(s => s.Add(_fixture.Create<KeyValuePair<int, string>>())),
                sut.Invoking(s => s.Clear()),
                sut.Invoking(s => s.Contains(_fixture.Create<KeyValuePair<int, string>>())),
                sut.Invoking(s => s.ContainsKey(_fixture.Create<int>())),
                sut.Invoking(s => s.CopyTo(_fixture.Create<KeyValuePair<int, string>[]>(), 0)),
                sut.Invoking(s => s.GetEnumerator()),
                sut.Invoking(s => s.Remove(_fixture.Create<int>())),
                sut.Invoking(s => s.Remove(_fixture.Create<KeyValuePair<int, string>>())),
                sut.Invoking(s => s.TryGetValue(_fixture.Create<int>(), out var value))
            };
            foreach (var action in actions)
            {
                action.Should().Throw<InvalidOperationException>().WithMessage("Access to object is disabled.");
            }
        }

        [Fact]
        public void EnabledTest()
        {
            // Given
            var dictionary = _fixture.Create<Dictionary<int, string>>();

            // When
            var sut = new DictionaryAccessDecorator<int, string>(dictionary, () => true);

            // Then
            sut.Should().BeEquivalentTo(dictionary);
        }
    }
}
