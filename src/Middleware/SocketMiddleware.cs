using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace cvx.lct.vot.api.Services
{
    public class SocketMiddleware : Hub
    {

        public SocketMiddleware()
        {
        }

        /*public async Task PullAsync()
        {
            var entity = await _jobService.ListParamsAsync();
            await Clients.Caller.SendAsync("PullAsync", entity);
        }*/

    }
}