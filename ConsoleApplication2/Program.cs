using ChickenOrEgg;
using CORPOS;

namespace ChickenOrEgg
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var sr = new ServiceRunner<CorposService>();
            return sr.main(args);
        }
    }
}
