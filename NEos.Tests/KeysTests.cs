using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NEos.Tests
{
    using Cryptography;
    using System.Diagnostics;
    using static Consts;

    [TestClass]
    public class KeysTests
    {
        [TestMethod]
        public void GenerateKeysTest()
        {
            PrivateKey pvt = new PrivateKey(KeyTypes.K1);
            PublicKey pub = new PublicKey(pvt);
            Trace.WriteLine($"{pvt}\n{pub}\n");

            pvt = new PrivateKey(KeyTypes.R1);
            pub = new PublicKey(pvt);
            Trace.WriteLine($"{pvt}\n{pub}");
        }

        [TestMethod]
        public void K1CustomKeyTest()
        {
            string pk = "5KQN22arZG2NuGYveHxS3YRyNazvNxQaC381UpspWG8nxVpCs3C";
            string expected = "EOS7gkQ28VCtq4YBEUvLiWsP2Ww16PWsAFNb2oiLk69g9QFkuTRuu";

            PrivateKey pvt = new PrivateKey(pk);
            PublicKey pub = new PublicKey(pvt);
            Assert.AreEqual(expected, pub.ToString());
        }

        [TestMethod]
        public void K1KeyTest()
        {
            PrivateKey pvt = new PrivateKey(K1PrivateKey);
            PublicKey pub = new PublicKey(pvt);
            Assert.AreEqual(K1PublicKey, pub.ToString());

            PublicKey pub_import = new PublicKey(K1PublicKey);

            Assert.AreEqual(pub.Value.ToHex(), pub_import.Value.ToHex());
        }

        [TestMethod]
        public void R1KeyTest()
        {
            PrivateKey pvt = new PrivateKey(R1PrivateKey);
            PublicKey pub = new PublicKey(pvt);
            Assert.AreEqual(R1PublicKey, pub.ToString());

            PublicKey pub_import = new PublicKey(R1PublicKey);

            Assert.AreEqual(pub.Value.ToHex(), pub_import.Value.ToHex());
        }

        [TestMethod]
        public void VerifyK1Signature()
        {
            string signature = "SIG_K1_KBW1jH37snqxqp35mqcCDD2bPiUXrGhBp23eSjhLdzgGKn9DeGkkmv4ZZidZ5ju5mpijkZ1fdQv4zs9myt2MPgpimgKWpT";
            string data = "bc9bea5bda2907ca858500000000019028530df99b9d4c000000572d3ccdcd019028530df99b9d4c00000000a8ed3232219028530df99b9d4ca05872f7488db33b01000000000000000242534d000000000000";

            PublicKey pub = new PublicKey("EOS5KcMqNEVmEBeEwhSxmFcqwDv8AQFZxE2C5SweACEDUCjnp3vGk");

            bool actual = pub.VerifySignature(JungleChainId, data, signature);

            Assert.IsTrue(actual);
        }
    }
}
