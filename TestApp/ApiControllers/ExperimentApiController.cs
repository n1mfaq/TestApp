using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.ApiControllers;

[ApiController]
[Route("api/experiment")]
public class ExperimentApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IOptionChooserService _optionChooserService;
    private readonly ILogger<ExperimentApiController> _logger;

    public ExperimentApiController(ApplicationDbContext context, IOptionChooserService optionChooserService,
        ILogger<ExperimentApiController> logger)
    {
        _context = context;
        _optionChooserService = optionChooserService;
        _logger = logger;
    }

    private async Task<IActionResult> GetExperimentResultAsync(Guid deviceToken, string experimentKey)
    {
        try
        {
            // Перевірка на null або порожній рядок для experimentKey
            experimentKey = experimentKey ??
                            throw new ArgumentNullException(nameof(experimentKey),
                                "Experiment key cannot be null or empty.");

            // Перевірка, чи існує вже результат для пристрою
            var existingExperimentDevice = await _context.DeviceExperiments
                .Include(x => x.Experiment)
                .FirstOrDefaultAsync(x => x.DeviceId == deviceToken);

            if (existingExperimentDevice != null)
                // Повертаємо існуючий результат експерименту
                return Ok(new ExperimentResult
                    { Key = existingExperimentDevice.Experiment?.Key, Value = existingExperimentDevice.Option });

            // Знаходимо експеримент за ключем
            var experiment = await _context.Experiments.FirstOrDefaultAsync(x => x.Key == experimentKey);

            if (experiment == null)
                // Повертаємо 404, якщо експеримент не знайдено
                return NotFound($"Experiment with key '{experimentKey}' not found.");

            // Генеруємо новий результат експерименту
            var newExperimentResult = new DeviceExperiment
            {
                ExperimentKey = experiment.Key,
                ExperimentId = experiment.Id,
                DeviceId = deviceToken,
                Option = _optionChooserService.ChooseRandomOption(experiment)
            };

            // Зберігаємо новий результат у базі даних
            _context.DeviceExperiments.Add(newExperimentResult);
            await _context.SaveChangesAsync();

            // Підготовка результату для відправлення клієнту
            var result = new ExperimentResult { Key = experiment.Key, Value = newExperimentResult.Option };

            // Повертаємо результат експерименту
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Обробка помилок та логування
            _logger.LogError(ex, "An error occurred while processing the experiment for device {DeviceToken}.",
                deviceToken);

            // Повертаємо помилку сервера
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("button-color")]
    public async Task<IActionResult> GetButtonColorExperiment([FromQuery] Guid deviceToken)
    {
        // Виклик методу обробки результату для конкретного експерименту
        return await GetExperimentResultAsync(deviceToken, "button_color");
    }

    [HttpGet("price")]
    public async Task<IActionResult> GetPriceExperiment([FromQuery] Guid deviceToken)
    {
        // Виклик методу обробки результату для конкретного експерименту
        return await GetExperimentResultAsync(deviceToken, "price");
    }
}