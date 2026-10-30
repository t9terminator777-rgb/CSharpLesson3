public abstract class BankAccount
{
    public string Owner { get; }
    public decimal Balance { get; protected set; }

    protected BankAccount(string owner)
    {
        Owner = owner;
        Balance = 0;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сумма пополнения должна быть больше нуля.");
            return;
        }

        Balance += amount;
        Console.WriteLine($"Счёт пополнен на {amount:F2}.");
    }

    public abstract bool Withdraw(decimal amount);

    public void DisplayInfo()
    {
        Console.WriteLine($"Владелец: {Owner}");
        Console.WriteLine($"Тип счёта: {GetType().Name}");
        Console.WriteLine($"Текущий баланс: {Balance:F2}");
    }
}