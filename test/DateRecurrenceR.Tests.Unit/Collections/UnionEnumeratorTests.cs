using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Collections;

public class UnionEnumeratorTests
{
    [Fact]
    public void JustTest()
    {
        // Arrange
        const int takeCount = 5;
        const int dayOfYear = 256;
        const int interval = 1;
        var beginDate = DateOnly.MinValue;
        var fromDate = beginDate;

        var enumerator = Recurrence.Yearly(beginDate, fromDate, takeCount, dayOfYear, interval);

        var res = Recurrence.Union(enumerator, enumerator, enumerator, enumerator);
        res = Recurrence.Union(res, enumerator, enumerator, enumerator);
        res = Recurrence.Union(res, enumerator, enumerator);
        res = Recurrence.Union(res, enumerator);

        // Act
        var list = new List<DateOnly>();
        while (res.MoveNext())
        {
            list.Add(res.Current);
        }

        //Assert
        list.Count.Should().Be(takeCount);
    }
    
    [Fact]
    public void JustTest2()
    {
        // Arrange
        const int takeCount = 5;
        const int dayOfYear = 256;
        const int interval = 1;
        var beginDate = DateOnly.MinValue;
        var fromDate = beginDate;

        var enumerator1 = Recurrence.Yearly(beginDate, fromDate, takeCount, 1, interval);
        var enumerator2 = Recurrence.Yearly(beginDate, fromDate, takeCount, 2, interval);

        var res1 = Recurrence.Union(enumerator1, enumerator1, enumerator1, enumerator1);
        var res2 = Recurrence.Union(enumerator2, enumerator2, enumerator2, enumerator2);
        var res = Recurrence.Union(res1, res2);
        res = Recurrence.Union(res1, enumerator2, enumerator1);
        res = Recurrence.Union(res2, enumerator1, enumerator2);

        // Act
        var list = new List<DateOnly>();
        while (res.MoveNext())
        {
            list.Add(res.Current);
        }

        //Assert
        list.Count.Should().Be(takeCount);
    }
}