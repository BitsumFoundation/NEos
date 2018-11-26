# ![GitHub Logo](https://github.com/BitsumFoundation/NEos/raw/master/Eos.Small.png) .NET implemetation of eos.io


|   Library   |   Version   |   Downloads   |
|---------|:----------:|:----------:|
|   **Eos.Cryptography**        | [![NuGet](https://img.shields.io/nuget/v/Eos.Cryptography.svg)](https://www.nuget.org/packages/Eos.Cryptography)|[![NuGet](https://img.shields.io/nuget/dt/Eos.Cryptography.svg)](https://www.nuget.org/packages/Eos.Cryptography)|
|   **Eos.Api**        | [![NuGet](https://img.shields.io/nuget/v/Eos.Api.svg)](https://www.nuget.org/packages/Eos.Api)|[![NuGet](https://img.shields.io/nuget/dt/Eos.Api.svg)](https://www.nuget.org/packages/Eos.Api)|

### How to use:

#### Eos.Cryptography:

```csharp
using System;

namespace CryptographySample
{
    using Eos.Cryptography;

    class Program
    {
        static void Main(string[] args)
        {
            KeyPair keyPair = new KeyPair(KeyTypes.K1);
            string pvt = keyPair.PrivateKey.ToString();
            string pub = keyPair.PublicKey.ToString();

            Console.WriteLine(pvt);
            Console.WriteLine(pub);

            keyPair = new KeyPair("5KVG5zRbKPRfMttnL9YR1r4BgC3EVwXuLE3k72HtxLCFrbr2J6s");
            pvt = keyPair.PrivateKey.ToString();
            pub = keyPair.PublicKey.ToString();

            Console.WriteLine(pvt);
            Console.WriteLine(pub);

            string chainId = "038f4b0fc8ff18a4f0842a8f0564611f6e96e8535901dd45e43ac8691a1c4dca";
            string packed_trx = "bc9bea5bda2907ca858500000000019028530df99b9d4c000000572d3ccdcd019028530df99b9d4c00000000a8ed3232219028530df99b9d4ca05872f7488db33b01000000000000000242534d000000000000";

            string signature = keyPair.SignData(chainId, packed_trx);
            bool check = keyPair.VerifySignature(chainId, packed_trx, signature);

            Console.WriteLine(signature);
            Console.WriteLine($"Is signature right: {check}");

            Console.ReadLine();
        }
    }
}
```

#### Eos.Api:

```csharp
using System;

namespace NodeApiSample
{
    using Eos;
    using Eos.Api;

    class Program
    {
        static void Main(string[] args)
        {
            NodeApiClient node = new NodeApiClient("https://jungle2.cryptolions.io");
            string accountName = "dmitrychegod";

            DoTasks(node, accountName);

            Console.ReadLine();
        }

        static async void DoTasks(NodeApiClient node, string accountName)
        {
            var info = await node.GetInfo();
            Console.WriteLine("Blockchain Info:");
            Console.WriteLine(info.ToJson());

            var accountInfo = await node.GetAccount(accountName);
            Console.WriteLine("\nAccount Info:");
            Console.WriteLine(accountInfo.ToJson());

            var balance = await node.GetBalance(accountName);
            Console.WriteLine("\nAccount Balance:");
            Console.WriteLine(balance.ToJson());
        }
    }
}
```