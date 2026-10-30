public sealed class CheckingAccount : BankAccount
{
    public const decimal ServiceFee = 50m;

    public CheckingAccount(string owner) : base(owner)
    {
    }

    public override bool Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сумма снятия должна быть больше нуля.");
            return false;
        }

        decimal totalToDeduct = amount + ServiceFee;
        if (Balance < totalToDeduct)
        {
            Console.WriteLine(
                $"Недостаточно денег: при снятии дополнительно списывается комиссия {ServiceFee:F2}.");
            return false;
        }

        Balance -= totalToDeduct;
        Console.WriteLine($"Снято: {amount:F2}. Комиссия: {ServiceFee:F2}.");
        return true;
    }
}