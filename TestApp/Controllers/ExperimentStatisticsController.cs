using Microsoft.AspNetCore.Mvc;
using TestApp.Models.DTO;

namespace TestApp.Controllers
{
    public class ExperimentStatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExperimentStatisticsController> _logger;

        public ExperimentStatisticsController(ApplicationDbContext context, ILogger<ExperimentStatisticsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                // Отримання статистики експериментів
                var experimentStatistics = _context.Experiments
                    .SelectMany(e => e.DeviceExperiments)
                    .AsEnumerable()
                    .GroupBy(de => de.ExperimentKey)
                    .Select(g => new ExperimentStatisticsViewModel
                    {
                        ExperimentKey = g.Key,
                        // Визначення кількості унікальних пристроїв для кожного експерименту
                        TotalDevices = g.Select(de => de.DeviceId).Distinct().Count(),
                        // Створення словника з розподілом опцій та їхньою кількістю
                        OptionsDistribution = g
                            .GroupBy(de => de.Option)
                            .ToDictionary(gr => gr.Key, gr => gr.Count())
                    })
                    .ToList();

                return View(experimentStatistics);
            }
            catch (Exception ex)
            {
                // Логування та обробка помилок
                _logger.LogError(ex, "An error occurred while processing experiment statistics.");

                // Повернення помилки сервера у вигляді сторінки з кодом 500
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}