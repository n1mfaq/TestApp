using TestApp.Exceptions;
using TestApp.Models;

namespace TestApp.Services;

public interface IOptionChooserService
{
    string ChooseRandomOption(Experiment experiment);
}

public class OptionChooserService : IOptionChooserService
{
    private readonly ILogger<OptionChooserService> _logger;

    public OptionChooserService(ILogger<OptionChooserService> logger)
    {
        _logger = logger;
    }

    public string ChooseRandomOption(Experiment experiment)
    {
        try
        {
            // Перевірка на нуль та наявність опцій для експерименту
            if (experiment == null || experiment.Options == null || !experiment.Options.Any())
                throw new ArgumentException("Experiment or its options are null or empty");

            // Створення списку опцій для подальшого використання
            var optionList = experiment.Options.ToList();

            // Логіка вибору опції в залежності від типу експерименту
            if (experiment.Key == "button_color")
            {
                // Логіка для button_color
                var randomIndex = new Random().Next(optionList.Count);
                return optionList[randomIndex];
            }

            if (experiment.Key == "price")
            {
                // Логіка для price з ймовірностями
                var randomNumber = new Random().Next(100);
                if (randomNumber < 75)
                    return "10";
                if (randomNumber < 85)
                    return "20";
                if (randomNumber < 90)
                    return "50";
                return "5";
            }

            // Якщо ключ невідомий, викидаємо виняток
            throw new OptionChoosingException($"Unknown experiment key: {experiment.Key}", "UnknownExperimentKey",
                null);
        }
        catch (OptionChoosingException ex)
        {
            // Обробка винятку опційного вибору
            _logger.LogError(ex, $"Option choosing error. Error Code: {ex.ErrorCode}");
            throw; // Перевищення винятку дозволяє передати його вище по стеку виклику.
        }
        catch (Exception ex)
        {
            // Інші загальні помилки можна також обробляти тут.
            _logger.LogError(ex, "An error occurred");
            throw;
        }
    }
}