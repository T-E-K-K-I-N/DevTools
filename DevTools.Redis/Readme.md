# DevTools.Redis

## Описание:
Библиотека для интеграции Redis в .NET. Проект использует версию .NET 8.0

## Установка:
В конфигурационном файле appsettings.json необходимо добавить следующую структуру:
``` json
"Redis":{
	"Host": "#{host}#",
	"Port": "#{port}#",
	"Username": "#{username}#",
	"Password": "#{password}#",
	"Database": "#{database}#",
	"KeysPrefix": "#{keys_prefix}#",
	"DefaultExpirationSeconds": "#{default_expiration_seconds}#"
},
```
