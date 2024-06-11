using DateRecurrenceR.Collections;
using FluentAssertions;

namespace Cadabra.CS.Date.Tests.Unit.Collections;

public sealed class DailyEnumeratorLimitByCountTests : EnumeratorLimitByCountTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void MoveNext_sets_current_with_expected_interval(int expectedInterval)
    {
        // Arrange
        var sut = new DailyEnumeratorLimitByCount(DateOnly.FromDateTime(DateTime.Now), 2, expectedInterval);
        var dayDif = 0;

        // Act
        sut.MoveNext();
        dayDif = sut.Current.DayNumber;
        sut.MoveNext();
        dayDif = sut.Current.DayNumber - dayDif;

        // Assert
        dayDif.Should().Be(expectedInterval);
    }

    public override void MoveNext_returns_true_expected_times(int expectedCount)
    {
        // Arrange
        var sut = new DailyEnumeratorLimitByCount(DateOnly.FromDateTime(DateTime.Now), expectedCount, 1);
        var counter = 0;

        // Act
        while (sut.MoveNext()) counter++;

        // Assert
        counter.Should().Be(expectedCount);
    }

    public override void MoveNext_returns_false_after_last_element()
    {
        // Arrange
        var sut = new DailyEnumeratorLimitByCount(DateOnly.FromDateTime(DateTime.Now), 0, 1);
        var moveNext = false;

        // Act
        for (var i = 0; i < 10; i++)
            if (!moveNext)
                moveNext = sut.MoveNext();

        // Assert
        moveNext.Should().Be(false);
    }

    public override void MoveNext_returns_false_after_DateMax()
    {
        // Arrange
        var sut = new DailyEnumeratorLimitByCount(DateOnly.MaxValue, 1, 1);

        // Act
        sut.MoveNext();
        var moveNext = sut.MoveNext();

        // Assert
        moveNext.Should().Be(false);
    }
}