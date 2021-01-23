using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace k8s.Extensions
{
    public static class ConfigMapExtensions
    {
        public static IConfigurationBuilder AddJsonKubeConfigMap(this IConfigurationBuilder builder, string nameAndKey, string @namespace)
        {
            return builder.AddJsonKubeConfigMap(nameAndKey, @namespace, nameAndKey);
        }

        public static IConfigurationBuilder AddJsonKubeConfigMap(this IConfigurationBuilder builder, string name, string @namespace, string key)
        {
            var config = KubernetesClientConfiguration.IsInCluster() ?
                KubernetesClientConfiguration.InClusterConfig() :
                KubernetesClientConfiguration.BuildDefaultConfig();
            IKubernetes client = new Kubernetes(config);

            var json = client.ReadNamespacedConfigMap(name, @namespace)?.Data[key];
            if (!string.IsNullOrEmpty(json))
            {
                builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json)));
            }

            return builder;
        }
    }
}
