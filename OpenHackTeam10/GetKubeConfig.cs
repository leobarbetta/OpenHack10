using k8s;
using Microsoft.Azure.Management.ContainerService.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.IO;
using System.Text;

namespace OpenHackTeam10
{
    public static class GetKubeConfig
    {
        public static IKubernetes GetKubernetes()
        {
            //string clientId = "bf36d86f-09b9-48ef-8c69-834ac7fcf3c6";
            //string clientSecret = "8edc77a2-02f9-4cc6-9fe5-3fc85a83eae9";
            //string tenantId = "1e6debca-08b7-4ccb-92f0-7a73e2f4da68";

            //string resourceGroupName = "OpenHackTeam10";
            //string clusterName = "minecraft-cluster-leonardo";

            //var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
            //        clientId,
            //        clientSecret,
            //        tenantId,
            //        AzureEnvironment.AzureGlobalCloud);

            //var azure = Azure
            //             .Configure()
            //             .Authenticate(credentials)
            //             .WithDefaultSubscription();

            //IKubernetesCluster kubernetesCluster = azure.KubernetesClusters.GetByResourceGroup(resourceGroupName, clusterName);

            //var buffer = kubernetesCluster.UserKubeConfigContent;
            //var configFile = Encoding.UTF8.GetString(buffer, 0, buffer.Length);


            //var streamConfig = new MemoryStream();
            //var writer = new StreamWriter(streamConfig);
            //writer.Write(configFile);
            //writer.Flush();
            //streamConfig.Position = 0;

            FileInfo configFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\config"));

            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(configFile);
            return new Kubernetes(config);
        }
    }
}
