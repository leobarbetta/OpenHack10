using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace OpenHackTeam10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PodsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            IKubernetes client = GetKubeConfig.GetKubernetes();

            V1PodList list = client.ListNamespacedPod("default");

            List<RootObject> pods = new List<RootObject>();

            foreach (var item in list.Items)
            {
                pods.Add(new RootObject
                {
                    Name = item?.Metadata?.Name,
                    Endpoints = new Endpoints
                    {
                        Minecraft = $"{item?.Status?.PodIP}: {item?.Spec?.Containers?.FirstOrDefault().Ports?.FirstOrDefault(p => p.ContainerPort == 25565).ContainerPort}",
                        Rcon = $"{item?.Status?.PodIP}: {item?.Spec?.Containers?.FirstOrDefault().Ports?.FirstOrDefault(p => p.ContainerPort == 25575).ContainerPort}"
                    }
                });
            }

            //return Ok(list.Items.FirstOrDefault());
            return Ok(pods);

        }

        [HttpPost]
        public IActionResult Post()
        {
            IKubernetes client = GetKubeConfig.GetKubernetes();
            V1PodList list = client.ListNamespacedPod("default");

            var pod = new V1Pod
            {
                Metadata = new V1ObjectMeta { Name = "teste-api", },
                Spec = new V1PodSpec
                {
                    Containers = new List<V1Container>()
                    {
                        new V1Container{
                            Image= "containersopenhackteam10.azurecr.io/minecraft-server:v1.0",
                            Name= "teste-api",
                            Ports = new List<V1ContainerPort> { new V1ContainerPort { ContainerPort = 25565 }, new V1ContainerPort { ContainerPort = 25575 } }
                        }

                    },
                    ImagePullSecrets = new List<V1LocalObjectReference>
                    {
                        new V1LocalObjectReference
                        {
                            Name = "acr-auth"
                        }
                    }
                }
            };

            client.CreateNamespacedPod(pod, "default");

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            IKubernetes client = GetKubeConfig.GetKubernetes();
            V1PodList list = client.ListNamespacedPod("default");

            var testeDelete = new V1DeleteOptions();
            client.DeleteNamespacedPod(testeDelete, "teste-api", "default");

            return Ok();
        }
    }


    public class Endpoints
    {
        public string Minecraft { get; set; }
        public string Rcon { get; set; }
    }

    public class RootObject
    {
        public string Name { get; set; }
        public Endpoints Endpoints { get; set; }
    }
}