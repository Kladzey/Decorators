using System;
using System.Collections.Generic;
using FluentAssertions;
using Kladzey.Wrappers.Collections;
using Moq;
using Xunit;

namespace Kladzey.Wrappers.Tests.Collections
{
    public class DictionaryAccessDecoratorTests
    {
        public static IEnumerable<object[]> DisabledTestCases
        {
            get
            {
                yield return Func(s => s.Count);
                yield return Func(s => s.IsReadOnly);
                yield return Func(s => s.Keys);
                yield return Func(s => s.Values);
                yield return Func(s => s[1]);
                yield return Action(s => s[1] = "1");
                yield return Action(s => s.Add(1, "1"));
                yield return Action(s => s.Add(new KeyValuePair<int, string>(1, "1")));
                yield return Action(s => s.Clear());
                yield return Action(s => s.Contains(new KeyValuePair<int, string>(1, "1")));
                yield return Action(s => s.ContainsKey(0));
                yield return Action(s => s.CopyTo(new KeyValuePair<int, string>[1], 0));
                yield return Action(s => s.GetEnumerator());
                yield return Action(s => s.Remove(1));
                yield return Action(s => s.Remove(new KeyValuePair<int, string>(1, "1")));
                yield return Action(s => s.TryGetValue(1, out _));

                static object[] Action(Action<DictionaryAccessDecorator<int, string>> act) => new object[] {act};
                static object[] Func<T>(Func<DictionaryAccessDecorator<int, string>, T> act) => Action(s => act(s));
            }
        }

        [Theory]
        [MemberData(nameof(DisabledTestCases))]
        public void DisabledTest(Action<DictionaryAccessDecorator<int, string>> sutAction)
        {
            // Given
            var dictionaryMock = new Mock<IDictionary<int, string>>();
            var sut = new DictionaryAccessDecorator<int, string>(dictionaryMock.Object, () => false);

            // When
            var act = sut.Invoking(sutAction);

            // Then
            act.Should().Throw<InvalidOperationException>().WithMessage("Access to object is disabled.");
            dictionaryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void EnabledTest()
        {
            // Given
            var dictionary = new Dictionary<int, string> {{0, "0"}, {1, "1"}, {2, "2"},};

            // When
            var sut = new DictionaryAccessDecorator<int, string>(dictionary, () => true);

            // Then
            sut.Should().BeEquivalentTo(dictionary);
        }
    }
}
