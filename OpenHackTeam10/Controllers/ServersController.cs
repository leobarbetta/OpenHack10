using k8s;
using Microsoft.AspNetCore.Mvc;

namespace OpenHackTeam10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);

            var servers = client.ListAPIService1();

            return Ok(servers);
        }
    }
}