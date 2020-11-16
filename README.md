# Testing OpenTelemetry StackExchange.Redis instrumentation

**Summary:** The StackExchange.Redis instrumentation works when using the `Sdk.CreateTracerProviderBuilder()` syntax, but when using `ConfigureServices(IServiceCollection services)` in Startup.cs

Requirements to run this example: 

* Local Redis instance
* `testKey01` with a valid string value in Redis

```
$ docker run -d -p 6379:6379 redis

$ redis-cli
127.0.0.1:6379> SET testKey01 "this is a string value saved in Redis"
```

## Run console app (works as expected):

```
$ dotnet run --project ./OpenTelemetry.Redis.ConsoleApp
```

StackExchange.Redis instrumentation works correctly, and telemetry is written to the console output:

```
Activity.Id:          00-9d3f69931850ec42a4971457262f001a-96ced4535b073144-01
Activity.DisplayName: GET
Activity.Kind:        Client
Activity.StartTime:   2020-11-16T20:45:08.0931395Z
Activity.Duration:    00:00:00.0037115
Activity.TagObjects:
    otel.status_code: 0
    db.system: redis
    db.redis.flags: None
    db.statement: GET
    net.peer.name: localhost
    net.peer.port: 6379
    db.redis.database_index: 0
Activity.Events:
    Enqueued [11/16/2020 20:45:08 +00:00]
    Sent [11/16/2020 20:45:08 +00:00]
    ResponseReceived [11/16/2020 20:45:08 +00:00]
```

## Run API (StackExchange.Redis telemetry not working):

```
$ dotnet run --project ./OpenTelemetry.Redis.API
```

Two routes in API:

* `GET https://localhost:5001/http` (OpenTelemetry HTTP instrumentation)
* `GET https://localhost:5001/redis` (OpenTelemetry Redis instrumentation)

`GET https://localhost:5001/http` makes a `System.Net.HttpClient` call in the controller, and correctly writes the AspNetCore and Http telemetry to the console output:

```Activity.Id:          00-6bd8892b93af194691aced7eecfed433-848fbe7b9037104f-01
Activity.ParentId:    00-6bd8892b93af194691aced7eecfed433-b6550fc7689b0b41-01
Activity.DisplayName: HTTP GET
Activity.Kind:        Client
Activity.StartTime:   2020-11-16T20:48:54.2025164Z
Activity.Duration:    00:00:00.2107437
Activity.TagObjects:
    http.method: GET
    http.host: github.com
    http.url: https://github.com/
    http.status_code: 200
    otel.status_code: 0
    otel.status_description: OK

Activity.Id:          00-6bd8892b93af194691aced7eecfed433-b6550fc7689b0b41-01
Activity.DisplayName: Http
Activity.Kind:        Server
Activity.StartTime:   2020-11-16T20:48:54.1384931Z
Activity.Duration:    00:00:00.4028030
Activity.TagObjects:
    http.host: localhost:5001
    http.method: GET
    http.path: /http
    http.url: https://localhost:5001/http
    http.user_agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0
    http.route: Http
    http.status_code: 200
    otel.status_code: 0
```

`GET https://localhost:5001/redis` reads from the local Redis instance, but only writes the AspNetCore telemetry to the console output. There is no telemetry written for the StackExchange.Redis activity:

```
Activity.Id:          00-ae623fa811c40f49b53d7631dd965a5b-69613d8304b98f4e-01
Activity.DisplayName: Redis
Activity.Kind:        Server
Activity.StartTime:   2020-11-16T20:57:29.6511907Z
Activity.Duration:    00:00:00.0743082
Activity.TagObjects:
    http.host: localhost:5001
    http.method: GET
    http.path: /redis
    http.url: https://localhost:5001/redis
    http.user_agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0
    http.route: Redis
    http.status_code: 200
    otel.status_code: 0
```

**Expected behavior:** AspNetCore and StackExchange.Redis telemetry should be written to the console output. 