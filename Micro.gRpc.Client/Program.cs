using Grpc.Net.Client;
using Micro.gRpc.Services;
using Micro.gRpc.Shared.Contracts;
using ProtoBuf.Grpc.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:7138");
var client = channel.CreateGrpcService<IUserService>();

var reply = await client.SayHelloAsync(
    new HelloRequest { Name = "GreeterClient" });

Console.WriteLine($"Greeting: {reply.Message}");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();