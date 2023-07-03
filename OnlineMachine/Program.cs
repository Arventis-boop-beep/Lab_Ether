using System;
using Nethereum.Accounts;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using System.IO;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.NonceServices;
using Nethereum.Web3;

namespace OnlineMachine
{
	class Program
	{
		public static String apiToken = "d88243da67d24bd29963d6c6b2972a8b";

		/// <summary>
		/// from, to, value => file, which contains unsigned tx
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			String endpoint = $"https://goerli.infura.io/v3/{apiToken}";
			var web3 = new Web3(endpoint);

			String from = args[0];
			String to = args[1];
			UInt64 value = Convert.ToUInt64(args[2]);

			var nonceService = new InMemoryNonceService(from, web3.Client);
			var nonceRequest = web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(from);
			nonceRequest.Wait();
			var nonceRequest1 = nonceService.GetNextNonceAsync();
			nonceRequest1.Wait();
			var nonce = nonceRequest1.Result;

			StreamWriter writer = new StreamWriter("./transaction_in.txt");
			writer.WriteLine(from);
			writer.WriteLine(to);
			writer.WriteLine(nonce);
			writer.WriteLine(value);
			writer.Close();

			Console.WriteLine("Please run procces at offline machine and press any key");
			Console.ReadKey();

			StreamReader streamReader = new StreamReader("../../../../OfflainMachine/bin/Debug/net5.0/transaction_out.txt");
			String signature = streamReader.ReadLine();
			streamReader.Close();

			Console.WriteLine(signature);
		}
	}
}
