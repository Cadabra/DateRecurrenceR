using DateRecurrenceR.Core;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Collections;

public class UnionEnumeratorTests
{
    [Fact]
    public void JustTest()
    {
        // Arrange
        const int takeCount = 5;
        var dayOfYear = new DayOfYear(256);
        var interval = new Interval(1);
        var beginDate = DateOnly.MinValue;
        var fromDate = beginDate;

        var equivalentEnumerator = RecurrenceVal.Yearly(beginDate, fromDate, takeCount, dayOfYear, interval);
        var enumerator = RecurrenceVal.Yearly(beginDate, fromDate, takeCount, dayOfYear, interval);

        var res = RecurrenceVal.Union(enumerator, enumerator, enumerator, enumerator);
        res = RecurrenceVal.Union(res, enumerator, enumerator, enumerator);
        res = RecurrenceVal.Union(res, enumerator, enumerator);
        res = RecurrenceVal.Union(res, enumerator);

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
    public void JustTest2()
    {
        // Arrange
        const int takeCount = 5;
        var interval = new Interval(1);
        var beginDate = DateOnly.MinValue;
        var fromDate = beginDate;

        var enumerator1 = RecurrenceVal.Yearly(beginDate, fromDate, takeCount, new DayOfYear(1), interval);
        var enumerator2 = RecurrenceVal.Yearly(beginDate, fromDate, takeCount, new DayOfYear(2), interval);

        var res1 = RecurrenceVal.Union(enumerator1, enumerator1, enumerator1, enumerator1);
        var res2 = RecurrenceVal.Union(enumerator2, enumerator2, enumerator2, enumerator2);
        var res = RecurrenceVal.Union(res1, res2);
        res = RecurrenceVal.Union(res, res1, enumerator2, enumerator1);
        res = RecurrenceVal.Union(res, res2, enumerator1, enumerator2);

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
    public void JustTest3()
    {
        // Arrange
        const int takeCount = 5;
        var interval = new Interval(1);
        var beginDate = DateOnly.MinValue;
        var fromDate = beginDate;

        var enumerator1 = RecurrenceVal.Yearly(beginDate, fromDate, takeCount, new DayOfYear(1), interval);
        var enumerator2 = RecurrenceVal.Yearly(beginDate, fromDate, takeCount, new DayOfYear(2), interval);

        var res = RecurrenceVal.Union(enumerator1, enumerator2);

        // Act
        var list = new List<DateOnly>();

        while (enumerator1.MoveNext())
        {
        }

        while (enumerator2.MoveNext())
        {
        }

        while (res.MoveNext())
        {
            list.Add(res.Current);
        }

        //Assert
        list.Count.Should().Be(takeCount*2);
    }
}