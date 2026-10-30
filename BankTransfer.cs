public sealed class BankTransfer : IPaymentMethod
{
    private readonly decimal _amount;

    public BankTransfer(decimal amount)
    {
        _amount = amount;
    }

    public void Pay()
    {
        Console.WriteLine($"Банковский перевод выполнен. Сумма: {_amount:F2}.");
    }
}