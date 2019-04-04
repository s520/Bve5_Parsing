using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bve5Parser.ScenarioGrammar;
using NUnit.Framework;

namespace Bve5ParserTests
{
	[TestFixture]
	public class ScenarioGrammarTests
	{
		private readonly ScenarioParser Parser = new ScenarioParser();

		[Test]
		public void ParseTest()
		{
			const string version = "2.00";
			const string encoding = "shift_jis";
			const string route0_path = "1234\\map0.txt";
			const string route0_weight = "1.0";
			const string route1_path = "5678\\map1.txt";
			const string route1_weight = ".5";
			const string vehicle = "abc/Vehicle.txt";
			const string title = "test";

			var builder = new StringBuilder();
			builder.AppendLine("BveTs Scenario " + version+ ":" + encoding);
			builder.AppendLine();
			builder.AppendLine("Route = " + route0_path + " * " + route0_weight + " | " + route1_path + " * " + route1_weight);
			builder.AppendLine("vehicle=" + vehicle);
			builder.AppendLine("Title=" + title);

			var data = Parser.Parse(builder.ToString());
			Assert.AreEqual(version, data.Version);
			Assert.AreEqual(encoding, data.Encoding);
			Assert.AreEqual(route0_path, data.Route[0].Value);
			Assert.AreEqual(double.Parse(route0_weight), data.Route[0].Weight);
			Assert.AreEqual(route1_path, data.Route[1].Value);
			Assert.AreEqual(double.Parse(route1_weight), data.Route[1].Weight);
			Assert.AreEqual(vehicle, data.Vehicle[0].Value);
			Assert.AreEqual(1.0, data.Vehicle[0].Weight);
			Assert.AreEqual(title, data.Title);
		}
	}
}
