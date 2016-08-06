using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;


namespace CORPOS
{
    [RunInstaller(true)]
    public sealed class CorposServiceInstallerProcess : ServiceProcessInstaller
    {
        private static ServiceHost _serviceHost = new ServiceHost(typeof(Service1));

        public CorposServiceInstallerProcess()
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
