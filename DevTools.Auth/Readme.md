# DevTools.Auth

## Описание:
Данная библиотека предназначена для интеграции с KeyCloak.
Проект использует версию .NET 8.0

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