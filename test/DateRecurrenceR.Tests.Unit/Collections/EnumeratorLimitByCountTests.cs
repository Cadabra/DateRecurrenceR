namespace DateRecurrenceR.Tests.Unit.Collections;

public abstract class EnumeratorLimitByCountTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public virtual void MoveNext_returns_true_expected_times(int expectedCount)
    {
        Assert.Fail("");
    }

    [Fact]
    public virtual void MoveNext_returns_false_after_last_element()
    {
        Assert.Fail("");
    }

    [Fact]
    public virtual void MoveNext_returns_false_after_DateMax()
    {
        Assert.Fail("");
    }
}