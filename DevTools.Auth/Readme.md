# DevTools.Auth

## Описание:
Данная библиотека предназначена для интеграции с KeyCloak.
<br>Позволяет производить авторизацию для методов и контроллеров через атрибуты.

## Установка:
В конфигурационном файле appsettings.json необходимо добавить следующую структуру:
``` json
"AuthOptions":{
	"Url": "#{url}#",
	"Realm": "#{realm}#",
	"ClientId": "#{client_id}#",
	"ClientSecret": "#{client_secret}#",
	"UseGwtAuthorization": "#{use_gwt_authorization}#"
},
```

## Пример использования:
``` C#
builder.Services.AddAuth(configuration, "KeyCloakAuth");
.
.
[Route("[action]")]
[HttpPost]
[Authorize(AuthenticationSchemes = "KeyCloakAuth")]
public async Task<IActionResult> SendToKafka(SendToKafkaModel sendToKafkaModel)
```