using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.Topics;

public class Order : ValueObject
{
    public const int MinLimit = 0;

    private Order(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Order order) => order.Value;

    public static Order Create(int order)
    {
        Ensure.MoreThan(order, MinLimit, "The order is too small.", nameof(order));

        return new Order(order.ToString());
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
} 