using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;
using Microsoft.AspNetCore.Mvc;
using PlantMonitorApi.Models;

namespace PlantMonitorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {

        public LineProtocolClient Client { get; set; }
        public PlantController(LineProtocolClient client)
        {
            Client = client;
        }

        [HttpGet]
        public async Task<PlantData> Get()
        {
            return new PlantData(){PlantId = "Jostein", Value = 50.5};
        }
        

        [HttpPost]
        public async Task Post([FromBody] PlantData value)
        {
            var cpuTime = new LineProtocolPoint(
                value.PlantId,
                new Dictionary<string, object>
                {
                    { "Value", value.Value },
                },
                new Dictionary<string, string>
                {
                    { "Name", value.PlantId }
                },
                DateTime.UtcNow);

            var payload = new LineProtocolPayload();
            payload.Add(cpuTime);
            var influxResult = await Client.WriteAsync(payload);
            if (!influxResult.Success)
                Console.WriteLine(influxResult.ErrorMessage);

        }
    }
}