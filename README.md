## TrainLists
Этот проект явлется решением тестового задания компании ...

**Для запуска проекта необходимо сделать следующее**

1. **Клонируйте проект**
git clone https://github.com/lidiya-fabrichnaja/TrainLists.git
  
2. **Отредактируйте строку подключения для вашего экземляра сервера баз данных**
в файле ../TrainLists.WebApi/appsettings.json

3. **Запустите приложение**
dotnet run --project TrainLists.WebApi/TrainLists.WebApi.csproj

4. **Для работы с api необходим JWT токен**
Для получения токена искользуйте метод ***Login*** контоллера ***User***:
```
curl -k 'POST' \
  'https://localhost:7092/api/User/Login' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{   
  "login": "admin",
  "password": "12345"
}'
```   
пример запроса с JWT токеном
```   
curl -k 'GET' \
  'https://localhost:7092/api/Report/ExportJson?trainNumber=2236' \
  -H 'accept: text/plain' \
  -H 'Authorization: bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJleHAiOjE2ODAyOTQyODcsImlzcyI6IkxfRmFicmljaG5heWEiLCJhdWQiOiJXZWJBcGkifQ.AI0P1ZTsg0ADVe3mX4AFy-V08xHtOTJO1vrqu-2Q3oE'   
```

5. **Загрузите данные в бд используя соответсвующий контроллер api**

## Описание котроллеров
•	    [POST] /api/User/Login - Авторизация пользователя
•	    [POST] /api/ExchangeData/ImportXml - загрузка данных в БД из xml файла в формате заказчика
•	    [GET]  /api/Report/ExportXlsx - выгрузка отчета в формате xlsx
•	    [GET]  /api/Report/ExportJson - выгрузка отчета в формате json

В данном проекте используется Swagger https://localhost:7092/index.html.
