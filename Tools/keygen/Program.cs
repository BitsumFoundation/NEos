using System;

namespace keygen
{
    using Eos.Cryptography;

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                KeyPair keyPair = new KeyPair(KeyTypes.K1);

                Console.Clear();
                Console.WriteLine("EOS Key pair generator:\n");
                Console.WriteLine("K1 Key pair:");
                Console.WriteLine($"{keyPair.PrivateKey}\n{keyPair.PublicKey}\n");

                keyPair = new KeyPair(KeyTypes.R1);
                Console.WriteLine("R1 Key pair:");
                Console.WriteLine($"{keyPair.PrivateKey}\n{keyPair.PublicKey}\n");

                Console.Write("Press any key to generate new pairs or ESC to exit: ");

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) break;
            }

        }
    }
}
