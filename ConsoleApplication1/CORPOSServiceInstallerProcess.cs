using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;
using WCF_Service;


namespace CORPOS
{
    [RunInstaller(true)]
    public sealed class CORPOSServiceInstallerProcess : ServiceProcessInstaller
    {
        private static ServiceHost _serviceHost = new ServiceHost(typeof(Service1));

        public CORPOSServiceInstallerProcess()
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
