# atomapp

Прототип решения для системы формирования и контроля распоряжений с голосовым управлением.
Создавался в рамках участия в хакатоне Цифровой прорыв 2020 командой "Ферма Киви"

Приложение запускается и работает на Windows 2019 Server. Выбор ОС обусловлен использоваением Speech API встроенным в систему для распознавания семантики голосового ввода пользователя.

# демо
Прототип: https://bitly.com/kiwifarm
Видео: https://vimeo.com/484936329

 - примеры аудио сообщений можно найти в папке /examples

Используемые технологии:

 - .NET Core 3.1 - бизнес логика, api, web-сервис (https://github.com/dotnet/core, MIT)
 - Entity Framework Core - ORM для базы данных (https://github.com/dotnet/efcore, Apache 2.0)
 - PostgreSQL - движок БД сущностей (https://github.com/postgres/postgres, PostgreSQL, BSD-style)
 - React - фронтэнд (https://github.com/facebook/react, MIT)
 - Vosk – движок распознавания сырого текста (https://github.com/alphacep/vosk-api, Apache 2.0)
 - NAudio - библиотека для работы со звуковыми файлами (https://github.com/naudio/NAudio, MSPL)
 - Xamarin - платформа для разработки кросплатформенных приложения (https://github.com/xamarin/xamarin-android, MIT)
 
# описание проектов решения
Сервер приложения
 - Отдаёт браузеру статические файлы фронтэнда
 - Имеет REST API для внешних интеграция и фронтэнда
 - Обрабатывает звуковые команды пользователя
 - Управляет назначенными задачами
 
Основные сервисы backend:
* ```back/atomapp.api/services/SemanticService.cs``` - сервис извлечения семантики по правилам описаным в SRGS грамматиках (сам файл грамматики находится в папке back/grammars/gramar-tasks.xml). Так же вызывает скрипт на python для оффлайн распознавания сырого текста (с помощью библиотеки Vosk (https://alphacephei.com/vosk/)). В решении используется сокращённая модель, но можно использовать и полную для улучшения качества распознавания)
Есть возможность вызывать библиотеку Vosk без python, сразу из C#.
* ```back/atomapp.api/services/TaskService.cs``` - сервис данных задач```
* ```back/atomapp.api/services/WorkplaceService.cs``` - сервис данных подразделений и работников```
* ```back/atomapp.api/services/PythonRunner.cs``` - сервис для запуска python```
* ```back/atomapp.api/services/AudioConverterService.cs``` - сервис конвертирования аудиформатов. Нужен из-за того, что браузер присылает аудио, закодированное в Opus, семантический разбор SAPI работает с mp3, а Vosk работает с wav файлами```

# Установка
1. Установить Windows Server 2019, установить последние обновления
2. Установить Microsoft Speech Platform SDK (https://www.microsoft.com/en-us/download/details.aspx?id=27226)
3. Установить Speech Platform Runtinme (https://www.microsoft.com/en-us/download/details.aspx?id=27225)
4. Установить STT модуль для русского языка (https://www.microsoft.com/en-us/download/details.aspx?id=3971)
5. Установить PostgreSQL (https://www.postgresql.org/download/)
6. Установить Python 3.8 (https://www.python.org/downloads/)
7. Установить Vosk API (https://alphacephei.com/vosk/install)
8. Скопировать файлы на сервер, настроить папки в appsettings.Production.json сервиса
