using DateRecurrenceR.Core;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.PublicApi;

/// <summary>
/// Argument-validation cases (VAL-*) from <c>TEST_CASES.md</c>: the <c>Core</c> value types
/// reject out-of-range input at construction time.
/// </summary>
public sealed class PublicApiValidationTests
{
    [Theory] // VAL-INT-0
    [InlineData(0)]
    [InlineData(-1)]
    public void Interval_below_minimum_throws(int value) =>
        FluentActions.Invoking(() => new Interval(value)).Should().Throw<ArgumentOutOfRangeException>();

    [Theory] // VAL-DOM-0 / VAL-DOM-32
    [InlineData(0)]
    [InlineData(32)]
    public void DayOfMonth_out_of_range_throws(int value) =>
        FluentActions.Invoking(() => new DayOfMonth(value)).Should().Throw<ArgumentOutOfRangeException>();

    [Theory] // VAL-DOY-0 / VAL-DOY-367
    [InlineData(0)]
    [InlineData(367)]
    public void DayOfYear_out_of_range_throws(int value) =>
        FluentActions.Invoking(() => new DayOfYear(value)).Should().Throw<ArgumentOutOfRangeException>();

    [Theory] // VAL-MOY-0 / VAL-MOY-13
    [InlineData(0)]
    [InlineData(13)]
    public void MonthOfYear_out_of_range_throws(int value) =>
        FluentActions.Invoking(() => new MonthOfYear(value)).Should().Throw<ArgumentOutOfRangeException>();

    [Fact] // VAL-RANGE-NEG
    public void DateRange_with_negative_count_throws() =>
        FluentActions.Invoking(() => new DateRange(new DateOnly(2024, 1, 1), -1))
            .Should().Throw<ArgumentOutOfRangeException>();

    [Theory] // valid boundary values must not throw
    [InlineData(1)]
    [InlineData(31)]
    public void DayOfMonth_boundaries_are_accepted(int value) =>
        FluentActions.Invoking(() => new DayOfMonth(value)).Should().NotThrow();

    [Theory]
    [InlineData(1)]
    [InlineData(366)]
    public void DayOfYear_boundaries_are_accepted(int value) =>
        FluentActions.Invoking(() => new DayOfYear(value)).Should().NotThrow();
}
