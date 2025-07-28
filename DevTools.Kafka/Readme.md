# DevTools.Kafka

## Описание:
Данная библиотека предназначена для интеграции с Kafka.
Проект использует версию .NET 8.0

## Установка:
В конфигурационном файле appsettings.json необходимо добавить следующую стректуру:
```
"KafkaConsumerOptions":{
	"Server": "#{server_url}#",
	"Topic": "#{topic}#",
	"GroupId": "#{group_Id}#"
},
```