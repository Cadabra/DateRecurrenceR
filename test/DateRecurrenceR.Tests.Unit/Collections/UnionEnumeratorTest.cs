using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Collections;

[TestSubject(typeof(UnionEnumerator))]
public class UnionEnumeratorTest
{
    [Fact]
    public void Union_two_recurrences()
    {
        // Arrange
        const int takeCount = 5;
        var interval = new Interval(1);
        var beginDate = DateOnly.MinValue;
        var fromDate = beginDate;

        var enumerator1 = Recurrence.Yearly(beginDate, fromDate, takeCount, new DayOfYear(1), interval);
        var enumerator2 = Recurrence.Yearly(beginDate, fromDate, takeCount, new DayOfYear(2), interval);

        var res = Recurrence.Union(enumerator1, enumerator2);

        // Act
        var list = new List<DateOnly>();

        while (res.MoveNext())
        {
            list.Add(res.Current);
        }

        //Assert
        list.Count.Should().Be(takeCount * 2);
    }

    [Fact]
    public void Union_one_recurrence_multiple_times()
    {
        // Arrange
        const int takeCount = 5;
        var dayOfYear = new DayOfYear(256);
        var interval = new Interval(1);
        var beginDate = DateOnly.MinValue;
        var fromDate = beginDate;

        var equivalentEnumerator = Recurrence.Yearly(beginDate, fromDate, takeCount, dayOfYear, interval);
        var enumerator = Recurrence.Yearly(beginDate, fromDate, takeCount, dayOfYear, interval);

        var res = Recurrence.Union(enumerator, enumerator, enumerator, enumerator);
        res = Recurrence.Union(res, enumerator, enumerator, enumerator);
        res = Recurrence.Union(res, enumerator, enumerator);
        res = Recurrence.Union(res, enumerator);

        // Act
        var equivalentList = new List<DateOnly>();
        while (equivalentEnumerator.MoveNext())
        {
            equivalentList.Add(equivalentEnumerator.Current);
        }

        var list = new List<DateOnly>();
        while (res.MoveNext())
        {
            list.Add(res.Current);
        }

        //Assert
        list.Count.Should().Be(takeCount);
        list.Should().BeEquivalentTo(equivalentList);
    }

    [Fact]
    public void Union_two_recurrence_multiple_times()
    {
        // Arrange
        const int takeCount = 5;
        var interval = new Interval(1);
        var beginDate = DateOnly.MinValue;
        var fromDate = beginDate;

        var enumerator1 = Recurrence.Yearly(beginDate, fromDate, takeCount, new DayOfYear(1), interval);
        var enumerator2 = Recurrence.Yearly(beginDate, fromDate, takeCount, new DayOfYear(2), interval);

        var res1 = Recurrence.Union(enumerator1, enumerator1, enumerator1, enumerator1);
        var res2 = Recurrence.Union(enumerator2, enumerator2, enumerator2, enumerator2);
        var res = Recurrence.Union(res1, res2);
        res = Recurrence.Union(res, res1, enumerator2, enumerator1);
        res = Recurrence.Union(res, res2, enumerator1, enumerator2);

        // Act
        var list = new List<DateOnly>();
        while (res.MoveNext())
        {
            list.Add(res.Current);
        }

        //Assert
        list.Count.Should().Be(takeCount * 2);
    }
}