using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.AttemptQuestions;

public class Order : ValueObject
{
    private Order(int value) => Value = value;

    public int Value { get; }

    public static implicit operator int(Order order) => order.Value;

    public static Order Create(int order)
    {
        Ensure.MoreThan(order, -1, "Order must be greater than or equal to 0.", nameof(order));

        return new Order(order);
    }

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
} 