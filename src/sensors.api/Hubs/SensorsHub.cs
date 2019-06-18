using production.models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace sensors.api
{

  //  [Authorize]
    public class SensorsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
        
            await base.OnConnectedAsync();
        }

        public void Ping()
        {
              this.Clients.Caller.SendAsync("Pong");
        }

        public void MeasureAdded(Measure measure)
        {
            this.Clients.Other.SendAsync("Measured", measure);

           // this.Clients.All.Except(this.Client.Caller).SendAsync("Measured", measure);

            // this.Clients.All.SendAsync("Measured", measure);
        }
    }



}