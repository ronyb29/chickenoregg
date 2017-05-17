using ChickenOrEgg;

namespace ServiceExample
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var sr = new ServiceRunner<ServiceExample>();
            return sr.Main(args);
        }
    }
}
