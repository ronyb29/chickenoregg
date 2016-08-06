using System;
using System.Diagnostics;
using System.ServiceModel;
using ChickenOrEgg;
using WCF_Service;

namespace CORPOS
{
    class CorposService : StartableService
    {
        public CorposService()
            : base()
        {
            ServiceName = "TestService";

            CanHandlePowerEvent = false;
            CanHandleSessionChangeEvent = false;
            CanPauseAndContinue = false;
            CanShutdown = false;
            CanStop = true;
            AutoLog = true;
        }

        public new void Start(string[] args)
        {
            OnStart(args);
        }
        
        private static ServiceHost _wcfHost = null;
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Net.ServicePointManager.DefaultConnectionLimit = 5000;

            try
            {
                _wcfHost = new ServiceHost(typeof(Service1));
                _wcfHost.Faulted += Host_Faulted;
                _wcfHost.Open();

                foreach (var endpoint in _wcfHost.Description.Endpoints)
                {
                    EventLog.WriteEntry(string.Format("Listening on endpoint: {0} ({2})-[{3}]\nUsing contract: {1}\nURI: {4}", endpoint.Name, endpoint.Contract, endpoint.Address, endpoint.Binding.Name, endpoint.ListenUri));
                }
            }
            finally
            {
                if (_wcfHost.State == CommunicationState.Faulted)
                {
                    _wcfHost.Abort();
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            _wcfHost.Close();
        }

        void Host_Faulted(object sender, EventArgs e)
        {
            EventLog.WriteEntry(string.Format("Host Faulted\n\n\n{0}", e), EventLogEntryType.Error);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            EventLog.WriteEntry(string.Format("uncaught exception\n\n\n{0}", e.ExceptionObject), EventLogEntryType.Error);
        }
    }

}
