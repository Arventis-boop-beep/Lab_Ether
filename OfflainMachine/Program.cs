using Nethereum.Signer;
using System;
using Nethereum.Web3;
using System.IO;
using Nethereum.Web3.Accounts;

namespace OfflineMachine
{
	class Program
	{
		static void Main(string[] args)
		{
			StreamReader stream1 = new StreamReader("../../../../OnlineMachine/bin/Debug/net5.0/transaction_in.txt");
			String from = stream1.ReadLine();
			String to = stream1.ReadLine();
			String nonce = stream1.ReadLine();
			String value = stream1.ReadLine();
			stream1.Close();

			Console.WriteLine(from);
			Console.WriteLine(to);
			Console.WriteLine(nonce);
			Console.WriteLine(value);

			var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7"; //32 byte hex string
			// random generation of private key
			// compute address from with generated private key:
			// private key => public key => address (last 20 bytes of SHA256 of public key)

			var signer = new EthereumMessageSigner();
			String message = $"{from} {to} {nonce} {value}";
			String signature = signer.EncodeUTF8AndSign(message, new EthECKey(privateKey));

			StreamWriter stream2 = new StreamWriter("./transaction_out.txt");
			stream2.WriteLine(signature);
			stream2.Close();
		}
	}
}
