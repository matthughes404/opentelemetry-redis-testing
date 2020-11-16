using System;

using OpenTelemetry.Trace;
using StackExchange.Redis;

namespace OpenTelemetry.Redis.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConnectionMultiplexer.Connect("localhost:6379");

            Sdk.CreateTracerProviderBuilder()
                .AddRedisInstrumentation(connection, opt => opt.FlushInterval = TimeSpan.FromSeconds(1))
                .AddConsoleExporter()
                .Build();

            var db = connection.GetDatabase();

            var msg = db.StringGet("testKey01");
            Console.WriteLine(msg);
        }
    }
}
