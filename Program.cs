using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

internal static class Program
{
    private const string LogFileName = "log.txt";

    private static async Task Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        WriteLoginEvent();
        Console.WriteLine("Журнал после запуска программы:");
        ShowEventLog();

        while (true)
        {
            ShowMenu();
            int choice = InputHelper.ReadIntInRange("Выбери номер задания: ", 0, 6);
            Console.WriteLine();

            switch (choice)
            {
                case 0:
                    Console.WriteLine("Программа завершена.");
                    return;
                case 1:
                    RunBankAccountDemo();
                    break;
                case 2:
                    RunPaymentDemo();
                    break;
                case 3:
                    RunOrderDemo();
                    break;
                case 4:
                    ShowEventLog();
                    break;
                case 5:
                    await DownloadReportAsync();
                    break;
                case 6:
                    await UpdateSourcesAsync();
                    break;
            }

            Console.WriteLine();
            Console.Write("Нажми Enter, чтобы вернуться в меню...");
            Console.ReadLine();
            Console.WriteLine();
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("=== Практические задачи по C# ===");
        Console.WriteLine("1. Абстрактный класс: банковский счёт");
        Console.WriteLine("2. Интерфейс: способы оплаты");
        Console.WriteLine("3. Исключения: заказ товара");
        Console.WriteLine("4. Файловая система: журнал событий");
        Console.WriteLine("5. Асинхронная загрузка отчёта");
        Console.WriteLine("6. Параллельное обновление данных (задача 7 в PDF)");
        Console.WriteLine("0. Выход");
    }

    private static void RunBankAccountDemo()
    {
        Console.WriteLine("=== Банковский счёт ===");
        Console.WriteLine("1. Сберегательный счёт");
        Console.WriteLine("2. Расчётный счёт");

        int accountType = InputHelper.ReadIntInRange("Выбери тип счёта: ", 1, 2);
        string owner = InputHelper.ReadRequiredText("Имя владельца: ");

        BankAccount account = accountType == 1
            ? new SavingsAccount(owner)
            : new CheckingAccount(owner);

        Console.WriteLine("Сначала пополним счёт.");
        decimal depositAmount = InputHelper.ReadPositiveDecimal("Сумма пополнения: ");
        account.Deposit(depositAmount);

        Console.WriteLine("Теперь попробуем снять деньги.");
        decimal withdrawalAmount = InputHelper.ReadPositiveDecimal("Сумма снятия: ");
        account.Withdraw(withdrawalAmount);
        account.DisplayInfo();
    }

    private static void RunPaymentDemo()
    {
        Console.WriteLine("=== Оплата покупки ===");
        Console.WriteLine("1. Банковская карта");
        Console.WriteLine("2. PayPal");
        Console.WriteLine("3. Банковский перевод");

        int choice = InputHelper.ReadIntInRange("Выбери способ оплаты: ", 1, 3);
        decimal amount = InputHelper.ReadPositiveDecimal("Сумма покупки: ");

        IPaymentMethod paymentMethod = choice switch
        {
            1 => new CreditCard(amount),
            2 => new PayPal(amount),
            _ => new BankTransfer(amount)
        };

        paymentMethod.Pay();
    }

    private static void RunOrderDemo()
    {
        Console.WriteLine("=== Заказ товара ===");

        while (true)
        {
            try
            {
                Console.Write("Количество товара: ");
                string quantityText = Console.ReadLine() ?? "";
                int quantity = int.Parse(quantityText, CultureInfo.InvariantCulture);

                Console.Write("Цена за один товар (например, 125,50): ");
                string priceText = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
                decimal price = decimal.Parse(
                    priceText,
                    NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture);

                if (quantity <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(quantity),
                        "Количество должно быть больше нуля.");
                }

                if (price <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(price),
                        "Цена должна быть больше нуля.");
                }

                decimal total = quantity * price;
                Console.WriteLine($"Итоговая сумма заказа: {total:F2}");
                return;
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введи целое количество и числовую цену.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка: введённое число слишком большое.");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine($"Ошибка: {exception.Message}");
            }

            Console.WriteLine("Попробуй ввести данные ещё раз.");
        }
    }

    private static void WriteLoginEvent()
    {
        string logPath = Path.Combine(Directory.GetCurrentDirectory(), LogFileName);
        string message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Запуск программы и вход пользователя{Environment.NewLine}";

        try
        {
            File.AppendAllText(logPath, message, Encoding.UTF8);
        }
        catch (IOException)
        {
            Console.WriteLine("Не удалось записать событие в журнал.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет разрешения на запись в файл журнала.");
        }
    }

    private static void ShowEventLog()
    {
        string logPath = Path.Combine(Directory.GetCurrentDirectory(), LogFileName);

        if (!File.Exists(logPath))
        {
            Console.WriteLine("Журнал пока пуст.");
            return;
        }

        try
        {
            string logContents = File.ReadAllText(logPath, Encoding.UTF8);
            Console.WriteLine("=== Журнал событий ===");
            Console.WriteLine(logContents);
        }
        catch (IOException)
        {
            Console.WriteLine("Не удалось прочитать журнал событий.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет разрешения на чтение файла журнала.");
        }
    }

    private static async Task DownloadReportAsync()
    {
        Console.WriteLine("Подключаемся к серверу и загружаем отчёт...");
        await Task.Delay(2000);

        string report = $"Отчёт о продажах{Environment.NewLine}" +
                        $"Дата формирования: {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}" +
                        "Продано товаров: 125" + Environment.NewLine +
                        "Выручка: 87500,00";

        string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "report.txt");

        try
        {
            await File.WriteAllTextAsync(reportPath, report, Encoding.UTF8);
            Console.WriteLine($"Загрузка завершена. Отчёт сохранён в файл: {reportPath}");
        }
        catch (IOException)
        {
            Console.WriteLine("Не удалось сохранить отчёт в файл.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет разрешения на запись отчёта.");
        }
    }

    private static async Task UpdateSourcesAsync()
    {
        Console.WriteLine("Обновляем данные из нескольких источников одновременно...");

        Task<string> currencyTask = LoadSourceAsync("Курсы валют", "1 USD = 87,50 KGS", 1600);
        Task<string> newsTask = LoadSourceAsync("Новости", "Новых сообщений: 3", 2200);
        Task<string> weatherTask = LoadSourceAsync("Погода", "Бишкек: +18 °C, ясно", 1200);

        string[] results = await Task.WhenAll(currencyTask, newsTask, weatherTask);

        Console.WriteLine("Все операции завершены. Результаты:");
        foreach (string result in results)
        {
            Console.WriteLine($"- {result}");
        }
    }

    private static async Task<string> LoadSourceAsync(string sourceName, string result, int delayMilliseconds)
    {
        Console.WriteLine($"Загружаем источник: {sourceName}...");
        await Task.Delay(delayMilliseconds);
        return $"{sourceName}: {result}";
    }
}