# АБП НТИ (студенты)

## Для проверки проекта:

### Уже запущенное приложение

Мы выгрузили наш проект в AzureWebApp ([ссылка](https://elite-logistic-app.azurewebsites.net/)).

### Запустить самостоятельно

Если у нас закончились токены на бесплатный хостинг приложения, то мы также подготовили вариант запуска проекта в
ручную, с использованием локальной базы данных (inMemory) для удобства проверки. Для этого необходимо установить ПО:

1. Yarn, npm, node.js
2. .Net Core 3.1 (SDK)

Наше приложение является клиент-серверным. Состоит из двух основных фреймворков, которые необходимо собрать отдельно.

1. Для сбора клиентской части (frontend):
    * Перейдите в папку _/AppExample.WebUI/client-app_
    * Загрузити зависимости в папке с помощью команды:
        ```
        yarn install
        ```
    * Выполните команду
        ```
        yarn build
        ```
      В конце билда будут сгенерированы .js файлы (папка bundles (_AppExample.WebUI/wwwroot/bundles_)) для
      подключения в страницу приложения. Клиентская часть не может работать без серверной (пункт 2 - запуск серверной
      части)


2. Для билда серверной части выполните команды в папке _AppExample.WebUI_

   ```
   dotnet restore
   dotnet build
   ```

3. Для запуска скомпилированных приложений в папке _AppExample.WebUI_ выполните команду:
   ```
   dotnet run
   ```
   Будет запущено на локальной бд (inMemory), на локальном адресе с портом 5001 (порт можно поменять в
   launchSettings.json) `https://localhost:5005/`

## Код проекта

* [Github](https://github.com/Viewshka/AppExamples)

## Для проверки отчетности:

1. Вся основная отчетность в призентации **ELITE_отчет_КоржовВыползов.ppt**. Это аналог презентации проекта-проделанной
   работы
2. БП, БД и другая требуемая отчетность о проделанной работе продублирована в папках.

## Для проверки функционала:

1. Пароли для пользователей в файле _ДляАвторизации.txt_ (git у нас открытый)
2. Все новые регистрируемые пользователи получают роль "Клиент"
3. Курьеров, менеджеров, клиентов, гостей может быть сколько угодно, работа с заказами разделена по персоналям.

## Команда проекта:

1. Коржов Александр Алексеевич (для связи [@KorzhovAlexander](https://t.me/KorzhovAlexander))
2. Выползов Юрий Вячеславович