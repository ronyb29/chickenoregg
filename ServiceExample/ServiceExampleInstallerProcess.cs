using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;
using WCF_Service;

namespace ServiceExample
{
    [RunInstaller(true)]
    public sealed class ServiceExampleInstallerProcess : ServiceProcessInstaller
    {
        private static ServiceHost _serviceHost = new ServiceHost(typeof(Service1));

        public ServiceExampleInstallerProcess()
        {
            Account = ServiceAccount.NetworkService;
            Username = null;
            Password = null;
            
            string serviceName = "TestService";

            Installers.Add(new ServiceInstaller
            {
                ServiceName = serviceName,
                DisplayName = serviceName,
                Description = "",
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true,
            });
        }
    }
}
