using DateRecurrenceR.Collections;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Collections;

[TestSubject(typeof(EmptyEnumerator))]
public class EmptyEnumeratorTests
{
    [Fact]
    public void MoveNext_always_returns_false()
    {
        var sut = new EmptyEnumerator();

        sut.MoveNext().Should().BeFalse();
        sut.MoveNext().Should().BeFalse();
    }

    [Fact]
    public void Current_is_the_default_date()
    {
        var sut = new EmptyEnumerator();

        sut.Current.Should().Be(default(DateOnly));
    }

    [Fact]
    public void Reset_throws_NotSupportedException()
    {
        var sut = new EmptyEnumerator();

        var act = () => sut.Reset();

        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Dispose_can_be_called_safely()
    {
        var sut = new EmptyEnumerator();

        var act = () => sut.Dispose();

        act.Should().NotThrow();
    }
}
