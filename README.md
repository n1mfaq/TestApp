# Назва проекту TestApp


## Технології

- ASP.NET Core
- Entity Framework Core
- Postgres ( Npgsql )
- xUnit / FluentAssertions / Moq
- Swagger
- Razor Pages
- JSON 

## NUGET

- Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
- Microsoft.AspNetCore.OpenApi
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.InMemory
- Microsoft.NET.Test.Sdk
- Moq
- Swashbuckle.AspNetCore
- xunit
- xunit.runner.visualstudio

### Вимоги

Версия .Net SDK 7
Postgres

### Запуск

При запуске ( http TestApp ) попадаем в swagger, есть 2 эндпоинта : deviceToken - Guid

- http://localhost:5222/api/experiment/button-color
- http://localhost:5222/api/experiment/price

Приклад 1:

Запит клієнта: GET: http://localhost:[PORT]/experiment/button-color?device-token=GUID1
JSON відповідь сервера: {key: "button_color", value: "#FF0000”}

Приклад 2 (другий запит того самого клієнта):

Запит клієнта: GET: http://localhost:[PORT]/experiment/button-color?device-token=GUID1
JSON відповідь сервера: {key: "button_color", value: "#FF0000"}

Приклад 3:

Запит клієнта: GET: http://localhost:[PORT]/experiment/button-color?device-token=GUID2
JSON відповідь сервера: {key: "button_color", value: "#00FF00"}

Приклад 4:

Запит клієнта: GET: http://localhost:[PORT]/experiment/price?device-token=GUID1
JSON відповідь сервера: {key: "button_color", value: "#FF0000”}

Приклад 5 (другий запит того самого клієнта):

Запит клієнта: GET: http://localhost:[PORT]/experiment/price?device-token=GUID2
JSON відповідь сервера: {key: "button_color", value: "#00FF00"}

Приклад 6:

Запит клієнта: GET: http://localhost:[PORT]/experiment/button-color?device-token=GUID3
JSON відповідь сервера: {key: "price", value: "10"}

http://localhost:5222/ExperimentStatistics - Страница статистики 

## Структура проекту

Точка входа Program.cs -> разбита на модули см. ProgramStartUp папка
Основной АПИ Контрллер и Контроллер для статистики в соответствующих папках

ApplicationDbContext.cs для бд

Кастомный Exception OptionChoosingException.cs в папке Exceptions

Папка для миграций

Папка для моделей - > Модель для View внутри

Папка для сервисов Services в которой IOptionChooserService / OptionChooserService для DI

Тестовый проект с xUnit и тестами для APi и Statistic Controller




