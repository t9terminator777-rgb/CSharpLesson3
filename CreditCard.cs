public sealed class CreditCard : IPaymentMethod
{
    private readonly decimal _amount;

    public CreditCard(decimal amount)
    {
        _amount = amount;
    }

    public void Pay()
    {
        Console.WriteLine($"Оплата картой выполнена. Сумма: {_amount:F2}.");
    }
}