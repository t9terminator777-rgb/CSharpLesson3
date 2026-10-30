public sealed class SavingsAccount : BankAccount
{
    public const decimal MinimumBalance = 1000m;

    public SavingsAccount(string owner) : base(owner)
    {
    }

    public override bool Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сумма снятия должна быть больше нуля.");
            return false;
        }

        if (Balance - amount < MinimumBalance)
        {
            Console.WriteLine(
                $"Снять нельзя: на сберегательном счёте должен остаться минимум {MinimumBalance:F2}.");
            return false;
        }

        Balance -= amount;
        Console.WriteLine($"Снято: {amount:F2}.");
        return true;
    }
}