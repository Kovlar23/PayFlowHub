# Workspace Setup

## Что уже есть

- `git`
- `.NET SDK 10.0.203`
- `.NET SDK 9.0.102`
- `node 22.18.0`
- `npm 10.9.3`
- `docker` CLI
- `docker compose`
- `kubectl`
- `java 17`

## Что требует исправления

1. Docker daemon сейчас не запущен.
2. `npm` из PowerShell блокируется execution policy, но работает через `cmd`.
3. `gh` не установлен.
4. `protoc` не установлен как системная утилита.
5. `docker` не может прочитать `C:\Users\Administrator\.docker\config.json`.

## Как подготовить пространство

1. Запустить Docker Desktop и убедиться, что `docker ps` отвечает без ошибок.
2. Исправить доступ к `C:\Users\Administrator\.docker\config.json` или пересоздать файл конфигурации Docker.
3. Поставить GitHub CLI (`gh`) и пройти `gh auth login`.
4. Если хотите использовать `npm` прямо в PowerShell, поменять execution policy для текущего пользователя или вызывать `npm` через `cmd /c npm ...`.
5. Для gRPC и protobuf в .NET системный `protoc` не обязателен, если использовать `Grpc.Tools` как NuGet dependency, но наличие локального `protoc` бывает полезно для ручной отладки контрактов.
6. Решить, где будут храниться секреты для локальной разработки: как минимум `.env`, `dotnet user-secrets` или отдельный секретный стор для infra-сервисов.

## Минимум для старта проекта

- работающий `git`
- работающий `.NET SDK`
- работающий `node` и `npm`
- работающий Docker daemon
- доступ к GitHub

## Рекомендуемый локальный workflow

1. Код и решения хранить в этом репозитории.
2. Инфраструктуру поднимать через `docker compose`.
3. Системные зависимости не ставить вручную, если их можно запускать в контейнерах.
4. Реальные PSP не подключать; использовать mock providers.
