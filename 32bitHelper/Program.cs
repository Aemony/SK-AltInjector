using System;

namespace _32bitHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Int32.TryParse(args[0], out int processID))
            {
                NativeMethods.InjectDLL(processID);
            }
        }
    }
}
