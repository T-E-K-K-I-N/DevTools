# DevTools.Kafka

## Описание:
Данная библиотека предназначена для интеграции с Kafka.
Проект использует версию .NET 8.0

## Установка:
В конфигурационном файле appsettings.json необходимо добавить следующую структуру:
``` json
"Kafka":{
	"ConsumerOptions":{
		"Server": "#{server_url}#",
		"Topic": "#{topic}#",
		"GroupId": "#{group_Id}#"
	},
	"ProducerOptions":{
		"Server": "#{server_url}#",
		"Topic": "#{topic}#",
		"Partitioner": "#{partitioner}#",
		"Partition": "#{partition}#",
		"AutoCreateTopics": "#{auto_create_topics}#",
		"Acks": "#{acks}#"
	}
},
```
