﻿using FluentAssertions;
using Xunit;

namespace Kladzey.Decorators.Tests
{
    public class DisposableAdapterTests
    {
        [Fact]
        public void DisposeTest()
        {
            // Given
            int? disposedValue = null;
            var sut = new DisposableAdapter<int>(1, v => disposedValue = v);

            // When
            sut.Dispose();

            // Then
            disposedValue.Should().Be(1);
        }

        [Fact]
        public void ValueTest()
        {
            // Given
            var sut = new DisposableAdapter<int>(1, v => { });

            // When
            var result = sut.Value;

            // Then
            result.Should().Be(1);
        }
    }
}
