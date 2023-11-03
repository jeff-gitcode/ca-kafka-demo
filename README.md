# CA Kafka Demo

## Tech Stack

- [x] ca
- [x] kafka
- [x] Kafkdrop
-

```javascript
$ dotnet new sln -o ca-kafka-demo
$ cd ca-kafka-demo

# create ca
$ dotnet new classlib -o Domain
$ dotnet new classlib -o Application
$ dotnet new classlib -o Infrastructure
$ dotnet new classlib -o Presentation
$ dotnet new webapi -o WebApi

# update reference
$ dotnet add .\Application\ reference .\Domain\
$ dotnet add .\Infrastructure\ reference .\Application\
$ dotnet add .\Presentation\ reference .\Application\
$ dotnet add .\WebApi\ reference .\Presentation\
$ dotnet add .\WebApi\ reference .\Infrastructure\
$ dotnet add .\WebApi\ reference .\Application\

# add to sln
$ dotnet sln add (ls -r **//*.csproj)

$ dotnet build ca-kafka-demo.sln
$ dotnet run --project .\WebApi\

# apply kafka
$ dotnet add .\ Install-Package Confluent.Kafka
$ dotnet add .\ Install-Package Confluent.SchemaRegistry.Serdes.Avro

# kafka server
# update daemon.json, set "experimental": true
$ docker-compose up -d
$ docker-compose ps
$ docker-compose stop

# prune image
$ docker image prune -a
# delete all containers
$ docker rm -f $(docker ps -a -q)
# delete all volumes
$ docker volume rm $(docker volume ls -q)

```
