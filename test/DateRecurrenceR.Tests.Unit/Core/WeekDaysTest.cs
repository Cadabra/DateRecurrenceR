using System.Collections;
using DateRecurrenceR.Core;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(WeekDays))]
public class WeekDaysTest
{
#if NET8_0_OR_GREATER
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void GetCountOfSelected_returns_1(int index)
    {
        // Arrange
        var sut = new WeekDaysArray {[index] = true};

        // Act
        var countOfSelected = sut.GetCountOfSelected();

        // Assert
        Assert.Equal(1, countOfSelected);
    }
    
    [Fact]
    public void GetCountOfSelected_returns_7()
    {
        // Arrange
        var sut = new WeekDaysArray {[0] = true, [1] = true, [2] = true, [3] = true, [4] = true, [5] = true, [6] =
 true};

        // Act
        var countOfSelected = sut.GetCountOfSelected();

        // Assert
        Assert.Equal(7, countOfSelected);
    }

    [Fact]
    public void METHOD()
    {
        WeekDaysArray weekDaysArray = default;
        var combinationCount = Math.Pow(2, weekDaysArray.Length);

        for (var i = 1; i <= combinationCount; i++)
        {
            var bytes = new BitArray([i]);
            weekDaysArray[0] = bytes[0];
            weekDaysArray[1] = bytes[1];
            weekDaysArray[2] = bytes[2];
            weekDaysArray[3] = bytes[3];
            weekDaysArray[4] = bytes[4];
            weekDaysArray[5] = bytes[5];
            weekDaysArray[6] = bytes[5];
        }
    }
#endif

    [Theory]
    [InlineData(DayOfWeek.Sunday, 1 << 0)]
    [InlineData(DayOfWeek.Monday, 1 << 1)]
    [InlineData(DayOfWeek.Tuesday, 1 << 2)]
    [InlineData(DayOfWeek.Wednesday, 1 << 3)]
    [InlineData(DayOfWeek.Thursday, 1 << 4)]
    [InlineData(DayOfWeek.Friday, 1 << 5)]
    [InlineData(DayOfWeek.Saturday, 1 << 6)]
    public void ToFlag_returns_correct_value_for_each_day(DayOfWeek dayOfWeek, int expectedFlag)
    {
        // Arrange
        var sut = new WeekDays(dayOfWeek);

        // Act
        var flag = sut.ToFlag();

        //Assert
        Assert.Equal(expectedFlag, flag);
    }

    [Fact]
    public void ToFlag_returns_correct_value_for_all_days()
    {
        // Arrange
        var expectedFlag = 127;
        var sut = new WeekDays(true, true, true, true, true, true, true);

        // Act
        var flag = sut.ToFlag();

        //Assert
        Assert.Equal(expectedFlag, flag);
    }
}