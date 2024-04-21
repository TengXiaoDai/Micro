using Micro.gRpc.Shared.Contracts;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.gRpc.Services
{
    public class UserService : IUserService
    {
        public async Task<HelloReply> SayHelloAsync(HelloRequest request, CallContext context = default)
        {
            return new HelloReply()
            {
                Message = "哈哈哈"
            };
        }
    }
}
