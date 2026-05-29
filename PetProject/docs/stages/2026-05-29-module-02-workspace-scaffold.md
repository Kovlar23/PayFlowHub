# Stage Note: Module 2 Workspace Scaffold

## Learning Goal

Понять, как из концептуальной архитектурной карты перейти к первому реальному `.NET`-монорепозиторию так, чтобы кодовая база сразу учила границам ответственности, сборочной дисциплине и минимальной проверяемости.

## What Was Built

- Создано решение [`PayFlowHub.sln`](/D:/VS_projects/C#/PetProject/PetProject/PayFlowHub.sln) как первая материальная форма монорепозитория.
- Добавлены проекты [`PayFlowHub.Api`](/D:/VS_projects/C#/PetProject/PetProject/src/Api/PayFlowHub.Api/PayFlowHub.Api.csproj), [`PayFlowHub.BuildingBlocks`](/D:/VS_projects/C#/PetProject/PetProject/src/BuildingBlocks/PayFlowHub.BuildingBlocks/PayFlowHub.BuildingBlocks.csproj), [`PayFlowHub.Contracts`](/D:/VS_projects/C#/PetProject/PetProject/src/Contracts/PayFlowHub.Contracts/PayFlowHub.Contracts.csproj) и [`PayFlowHub.Modules.Payments.Domain`](/D:/VS_projects/C#/PetProject/PetProject/src/Modules/Payments/PayFlowHub.Modules.Payments.Domain/PayFlowHub.Modules.Payments.Domain.csproj).
- Добавлен исполняемый проверочный harness [`PayFlowHub.StructureChecks`](/D:/VS_projects/C#/PetProject/PetProject/tests/PayFlowHub.StructureChecks/PayFlowHub.StructureChecks.csproj), который валидирует учебные границы без внешних тестовых пакетов.
- Добавлены единые build-настройки в [`Directory.Build.props`](/D:/VS_projects/C#/PetProject/PetProject/Directory.Build.props) и базовые style-правила в [`.editorconfig`](/D:/VS_projects/C#/PetProject/PetProject/.editorconfig).
- Шаблонный `weatherforecast` заменён на учебный workspace API с endpoint'ами `/`, `/health` и `/course/workspace`.
- Добавлены [`docs/modules/02-workspace-and-monorepo.md`](/D:/VS_projects/C#/PetProject/PetProject/docs/modules/02-workspace-and-monorepo.md), ADR [`0001-module-02-monorepo-foundation.md`](/D:/VS_projects/C#/PetProject/PetProject/docs/adr/0001-module-02-monorepo-foundation.md), stage script [`Verify-Workspace.ps1`](/D:/VS_projects/C#/PetProject/PetProject/scripts/Verify-Workspace.ps1) и обновлён CI workflow.

## Why This Matters

Наивный старт обычно выглядит так: один `Web API` проект, несколько папок `Services`, `Models`, `Helpers`, потом быстрый рост хаоса.

Для финтех-системы это опасно, потому что:

- транспортные DTO начинают управлять бизнес-логикой;
- доменные правила прячутся внутри контроллеров;
- shared code становится свалкой случайных утилит;
- позже становится трудно отделить API, домен, интеграции и инфраструктуру.

Этот шаг делает обратное: он намеренно строит небольшой, но явно разложенный каркас, где студент с первого дня видит, что разные типы ответственности живут в разных проектах.

## Theory

### Что такое монорепозиторий в этом курсе

Монорепозиторий здесь — это один git-репозиторий, где живут несколько связанных проектов, собираемых одной solution.

Это не означает, что система уже стала микросервисной. На старте это ближе к `modular monolith with explicit boundaries`:

- один репозиторий;
- один основной исполняемый host;
- несколько логических и проектных границ;
- единые скрипты проверки и единый workflow разработки.

Такой подход полезен новичку, потому что сначала нужно понять форму системы и зависимостей, а не распыляться на сетевую сложность, инфраструктуру и deployment topology.

### Что такое `BuildingBlocks`

`BuildingBlocks` — это место для очень маленького количества общих примитивов и соглашений.

Оно нужно, чтобы:

- не дублировать одни и те же базовые константы и marker-типы;
- иметь явный слой для действительно cross-cutting вещей;
- не превращать `Contracts` или `Domain` в случайное shared-хранилище.

Trade-off:
если злоупотреблять этим проектом, он превращается в anti-pattern `SharedKernelDump`, куда начинают складывать всё подряд. Поэтому на старте он должен быть узким и скучным.

### Что такое `Contracts`

`Contracts` — это проект для DTO и transport-friendly record-типов.

Он нужен, чтобы ученик с самого начала видел разницу между:

- данными, которыми системы обмениваются;
- правилами, по которым живёт доменная модель.

Почему это важно:
в плохих системах доменные сущности напрямую сериализуются наружу, а публичный API начинает диктовать внутреннюю модель. Позже это мешает и версии API, и тестированию, и развитию домена.

### Что такое `Payments.Domain`

Сейчас это почти пустой проект с marker-типом и описанием ответственности.

Это специально.

Зачем создавать его так рано:

- закрепить место будущей доменной модели;
- показать, что домен будет отдельным смысловым слоем;
- не смешивать будущие `Aggregate`, `Entity`, `Value Object` и `Invariant` с API-кодом уже на уровне структуры решения.

Почему бизнес-логики пока нет:
потому что `Module 4` должен отдельно и последовательно объяснить domain model, а не прятать её кусками в ранних инфраструктурных шагах.

### Что такое `StructureChecks`

Вместо полноценного unit-test framework здесь используется простой консольный harness.

Он решает несколько задач:

- даёт автоматическую проверку уже сейчас;
- не зависит от внешних NuGet-пакетов в ограниченной среде;
- учит сначала формулировать сами инварианты структуры.

Что именно он проверяет:

- что stage metadata указывает на `Module 2`;
- что workspace overview содержит ожидаемые модули;
- что `Contracts` не зависит от `Api`;
- что `Payments.Domain` не зависит от `Contracts`.

Это не замена нормальному тестированию. Это учебная ступень.

### Почему API уже есть, но это ещё не Module 3

В [`Program.cs`](/D:/VS_projects/C#/PetProject/PetProject/src/Api/PayFlowHub.Api/Program.cs) есть простые endpoint'ы, но они не являются unified payments API.

Сейчас API нужен только как:

- исполняемая точка входа;
- способ увидеть, что solution реально запускается;
- средство показать связь между host-проектом и contract-проектом.

Мы ещё не учим:

- versioning;
- `Correlation ID`;
- `Idempotency Key`;
- payment request/response contracts.

Это осознанно отложено в `Module 3`, чтобы не смешивать темы.

## Code Walkthrough

### [`Directory.Build.props`](/D:/VS_projects/C#/PetProject/PetProject/Directory.Build.props)

Фиксирует общие правила сборки:

- `net10.0` как единый target framework;
- `Nullable` и `ImplicitUsings`;
- `TreatWarningsAsErrors`, чтобы дисциплина появлялась рано;
- `AnalysisLevel` и `EnforceCodeStyleInBuild`, чтобы style drift не накапливался незаметно.

Это важно, потому что в растущей системе локальные различия между проектами быстро становятся операционным долгом.

### [`src/BuildingBlocks/PayFlowHub.BuildingBlocks/DeveloperWorkflowDefaults.cs`](/D:/VS_projects/C#/PetProject/PetProject/src/BuildingBlocks/PayFlowHub.BuildingBlocks/DeveloperWorkflowDefaults.cs)

Содержит базовые константы текущего учебного этапа.

Зачем такие константы вообще нужны:

- скрипты и проверки получают единое место правды;
- stage metadata можно использовать повторно;
- проект начинает учить, что даже простые operational defaults лучше централизовать, чем размазывать по строкам.

### [`src/Contracts/PayFlowHub.Contracts/WorkspaceOverviewContract.cs`](/D:/VS_projects/C#/PetProject/PetProject/src/Contracts/PayFlowHub.Contracts/WorkspaceOverviewContract.cs)

Показывает ранний пример contract-first мышления.

Это ещё не payment contract, но уже полезная демонстрация того, что наружу стоит выдавать простые record-типы, а не внутренние implementation objects.

### [`src/Api/PayFlowHub.Api/Workspace/WorkspaceOverviewFactory.cs`](/D:/VS_projects/C#/PetProject/PetProject/src/Api/PayFlowHub.Api/Workspace/WorkspaceOverviewFactory.cs)

Создаёт обзор текущей топологии workspace.

Почему это хорошо как учебная ступень:

- код простой;
- зависимости видны явно;
- можно показать, как host собирает данные из других проектов, не ломая границы домена.

### [`tests/PayFlowHub.StructureChecks/Program.cs`](/D:/VS_projects/C#/PetProject/PetProject/tests/PayFlowHub.StructureChecks/Program.cs)

Это простейший executable test harness.

Он не использует атрибуты `[Fact]` и test runner'ы. Вместо этого он собирает список нарушений и завершает процесс с кодом `1`, если найдено хотя бы одно.

Почему это полезно для новичка:

- логика проверки прозрачна;
- легко увидеть причинно-следственную связь между правилом и падением;
- потом будет проще понять, что test framework лишь автоматизирует и масштабирует ту же идею.

### [`scripts/Verify-Workspace.ps1`](/D:/VS_projects/C#/PetProject/PetProject/scripts/Verify-Workspace.ps1)

Запускает `restore`, `build` и `StructureChecks`.

Это первый зачаток developer workflow:

- одна команда;
- воспроизводимая проверка;
- поведение, пригодное и локально, и в CI.

## How To Run

### Сборка решения

```powershell
Set-Location D:\VS_projects\C#\PetProject\PetProject
dotnet restore .\PayFlowHub.sln
dotnet build .\PayFlowHub.sln --no-restore
```

### Запуск API

```powershell
Set-Location D:\VS_projects\C#\PetProject\PetProject
dotnet run --project .\src\Api\PayFlowHub.Api\PayFlowHub.Api.csproj
```

После запуска открой:

- `http://localhost:5000/` или выведенный порт для обзора workspace;
- `http://localhost:5000/health` для health-style ответа;
- `http://localhost:5000/course/workspace` для списка учебных модулей решения.

### Запуск полной проверки этапа

```powershell
Set-Location D:\VS_projects\C#\PetProject\PetProject
.\scripts\Verify-CourseDocs.ps1
.\scripts\Verify-Workspace.ps1
```

## How To Verify

- Убедиться, что [`PayFlowHub.sln`](/D:/VS_projects/C#/PetProject/PetProject/PayFlowHub.sln) содержит `Api`, `BuildingBlocks`, `Contracts`, `Payments.Domain` и `StructureChecks`.
- Запустить `dotnet run --project .\src\Api\PayFlowHub.Api\PayFlowHub.Api.csproj` и получить JSON-ответ на `/`.
- Убедиться, что `/course/workspace` возвращает четыре модуля.
- Запустить `.\scripts\Verify-Workspace.ps1` и получить успешное завершение.
- Открыть [`0001-module-02-monorepo-foundation.md`](/D:/VS_projects/C#/PetProject/PetProject/docs/adr/0001-module-02-monorepo-foundation.md) и проверить, что архитектурное решение зафиксировано явно, а не только подразумевается кодом.

## Common Mistakes

- Решить, что каждый проект solution уже обязан быть отдельным deployable service.
- Начать складывать доменные правила в `Contracts`, потому что там уже есть record-типы.
- Превратить `BuildingBlocks` в универсальную папку для любого кода, который пока непонятно куда положить.
- Считать, что раз есть `/health`, значит API-дизайн уже начат по-настоящему.
- Игнорировать build warnings на раннем этапе и откладывать дисциплину на потом.

## Simpler Version vs Better Version

### Simpler Version

Текущая версия специально простая:

- один основной ASP.NET Core host;
- несколько небольших проектов;
- ручной, но воспроизводимый structure-check harness;
- без базы данных, сообщений, провайдеров и сложной domain model.

Почему это хорошо сейчас:

- новичок видит форму системы без инфраструктурного шума;
- можно запускать и читать код локально почти без барьеров;
- легче объяснить границы до того, как они начнут трещать под нагрузкой реального функционала.

### Better Version

Более сильная версия появится позже:

- `Module 3` добавит реальные public API contracts, versioning и idempotency-related заголовки;
- `Module 4` наполнит `Payments.Domain` агрегатами и инвариантами;
- `Module 14` заменит часть простых проверок на нормальную многоуровневую test strategy;
- поздние модули добавят persistence, messaging, saga orchestration и observability.

Почему она лучше:

- система станет ближе к production reality;
- проверки будут богаче и точнее;
- появится полноценная инженерная обратная связь по поведению, а не только по структуре.

Какие trade-offs появятся:

- станет больше кода и больше зависимостей;
- сборка и локальный запуск усложнятся;
- вырастет риск accidental complexity, если терять из виду исходные границы.

## What To Learn Next

Следующий логичный запуск должен оставаться последовательным и перейти к раннему `Module 3`:

- ввести первый public payment creation contract;
- объяснить `Correlation ID` и почему это не `Payment ID`;
- объяснить `Idempotency Key` и почему в финтехе это защита от двойного движения денег;
- начать формировать версионируемый unified REST API без смешивания его с доменной моделью.
