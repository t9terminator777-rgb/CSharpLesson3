public sealed class PayPal : IPaymentMethod
{
    private readonly decimal _amount;

    public PayPal(decimal amount)
    {
        _amount = amount;
    }

    public void Pay()
    {
        Console.WriteLine($"Оплата через PayPal выполнена. Сумма: {_amount:F2}.");
    }
}