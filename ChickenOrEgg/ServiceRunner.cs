using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace ChickenOrEgg
{
    public class ServiceRunner<T> where T : StartableService, new()
    {

        private const string HelpStr = "USAGE:\n" +
                                       "-install:       Install the service\n" +
                                       "-uninstall:     Uninstall the service\n" +
                                       "-console:       Run a service instance in a cosole\n" +
                                       "-help:          This dialog\n" +
                                       "\n" +
                                       "Press any key to exit.";

        void ParseArgs(IDictionary<string, bool> dict, IEnumerable<string> args)
        {
            foreach (var arg in args.Where(arg => dict.Keys.Contains(arg)))
                dict[arg] = true;
        }

        public int Main(string[] args)
        {
            var options = new Dictionary<string, bool> //Dictionary lookup is faster than array
            {
                { "-install", false },
                { "-uninstall", false },
                { "-console", false },
                { "-help", false },
            };

            ParseArgs(options, args);

            if (options["-help"])
                ShowHelp();

            if (options["-uninstall"])
                Uninstall(args);

            if (options["-install"])
                Install(args);

            if (options["-console"])
                RunConsole(args);

            if (options["-install"] || options["-uninstall"])
            {
                Console.ReadLine();
                return 0;
            }

            ShowHelp(false);

            ServiceBase[] services = { new T() };
            ServiceBase.Run(services);
            return 0;

        }

        private void RunConsole(string[] args)
        {
            try
            {
                var service = new T();
                Console.WriteLine("Starting...");
                service.Start(args);
                Console.WriteLine("Running...");
                //Console.ReadLine();
                //service.Stop();
                while (true) { System.Threading.Thread.Sleep(2000); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\nERROR:");
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private static void ShowHelp(bool wait = true)
        {
            Console.WriteLine(HelpStr);
            if (wait)
                Console.ReadLine();
        }

        static void Uninstall(string[] args)
        {
            Install(args, true);
        }

        static void Install(string[] args, bool undo = false)
        {
            Console.WriteLine(undo ? "uninstalling" : "installing");
            using (var inst = new AssemblyInstaller(typeof(T).Assembly, args))
            {
                IDictionary state = new Hashtable();

                inst.UseNewContext = true;

                if (undo)
                    inst.Uninstall(state);
                else
                {
                    try
                    {
                        inst.Install(state);
                        inst.Commit(state);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                        inst.Rollback(state);
                    }
                }

                Console.WriteLine(undo ? "uninstalling completed!" : "installing completed!");
            }
        }

    }
}