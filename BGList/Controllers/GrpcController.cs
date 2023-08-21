using BGList.Constants;
using BGList.gRPC;

using Grpc.Core;
using Grpc.Net.Client;

using Microsoft.AspNetCore.Mvc;

namespace BGList.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class GrpcController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<BoardGameResponse> GetBoardGame(int id)
        {
            using var channel = GrpcChannel
            .ForAddress(BGListConstants.CGRPC_HOST_PORT);
            var client = new gRPC.Grpc.GrpcClient(channel);
            var response = await client.GetBoardGameAsync(
            new BoardGameRequest { Id = id });
            return response;
        }

        [HttpPost]
        public async Task<BoardGameResponse> UpdateBoardGame(string token, int id, string name)
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {token}");

            using var channel = GrpcChannel
            .ForAddress(BGListConstants.CGRPC_HOST_PORT);
            var client = new gRPC.Grpc.GrpcClient(channel);
            var response = await client.UpdateBoardGameAsync(
            new UpdateBoardGameRequest
            {
                Id = id,
                Name = name
            },
            headers);
            return response;
        }
    }
}
