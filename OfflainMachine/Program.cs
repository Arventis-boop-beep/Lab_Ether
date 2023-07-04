using Nethereum.Signer;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Nethereum.Hex.HexConvertors.Extensions;


namespace OfflineMachine
{
	class Program
	{
		private static Random random = new Random();
		
		//Функция для получения рандомного ключа
		public static string GetRandomString(int length)
		{
			const string chars = "0123456789abcdef";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
		static void Main(string[] args)
		{
			//Получим рандомный приватный ключ
			String privateKeyHex = "0x" + GetRandomString(64);
			var privateKeyBytes = privateKeyHex.HexToByteArray();

			// Создадим объект EthECKey из приватного ключа, чтоб получить публичный
			var privateKey = new EthECKey(privateKeyHex);

			// Получим публичный ключ
			byte[] publicKey = privateKey.GetPubKey();
			string publicKeyHex = "0x" + BitConverter.ToString(publicKey).Replace("-", "");

			//Получим хэш публичного ключа и адрес
			byte[] publicKeyHash = SHA256.HashData(publicKey);
			String address = "0x" + publicKeyHash.ToHex().Substring(24);

			Console.WriteLine("Private Key: " + privateKeyHex);
			Console.WriteLine("Public Key: " + publicKeyHex);
			Console.WriteLine("Address: " + address);

			StreamReader stream1 = new StreamReader("../../../../OnlineMachine/bin/Debug/net5.0/transaction_in.txt");
			String from = address;
			String to = stream1.ReadLine();
			String nonce = stream1.ReadLine();
			String value = stream1.ReadLine();
			stream1.Close();

			Console.WriteLine(from);
			Console.WriteLine(to);
			Console.WriteLine(nonce);
			Console.WriteLine(value);

			//var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7"; //32 byte hex string
			// random generation of private key
			// compute address from with generated private key:
			// private key => public key => address (last 20 bytes of SHA256 of public key)

			var signer = new EthereumMessageSigner();
			String message = $"{from} {to} {nonce} {value}";
			String signature = signer.EncodeUTF8AndSign(message, new EthECKey(privateKeyHex));

			StreamWriter stream2 = new StreamWriter("./transaction_out.txt");
			stream2.WriteLine(signature);
			stream2.Close();
		}
	}
}
