using k8s;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace OpenHackTeam10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            IKubernetes client = GetKubeConfig.GetKubernetes();

            var serversRest = client.ListNamespacedService("default");

            List<Server> servers = new List<Server>();

            foreach (var item in serversRest.Items)
            {
                Server server = new Server
                {
                    Name = item.Metadata.Name,
                    IP = item.Status?.LoadBalancer?.Ingress?.FirstOrDefault()?.Ip
                };

                foreach (var port in item.Spec.Ports)
                {
                    server.Ports.Add(new Port
                    {
                        Name = port.Name,
                        PortNumber = port.Port.ToString()
                    });
                }

                servers.Add(server);
            }

            return Ok(servers);
        }
    }

    public class Server
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public List<Port> Ports { get; set; } = new List<Port>();
    }

    public class Port
    {
        public string Name { get; set; }
        public string PortNumber { get; set; }
    }
}