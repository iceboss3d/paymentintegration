using System;

namespace PaymentIntegration.Utilities
{
    public class RefGen
    {
        public static int Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
    }
}
