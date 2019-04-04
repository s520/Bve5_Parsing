using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bve5Parser.MapGrammar.V1;
using NUnit.Framework;

namespace Bve5ParserTests
{
	[TestFixture]
	public class MapV1GrammarTests
	{
		private readonly MapV1Parser Parser = new MapV1Parser();
		private readonly StringBuilder Builder = new StringBuilder();

		[SetUp]
		public void SetUp()
		{
			Builder.Clear();
			Builder.AppendLine("BveTs Map 1.00");
		}

		[Test]
		public void CurveTest()
		{
			Builder.AppendLine("100;");
			Builder.AppendLine("Curve.Gauge(1.067);");
			Builder.AppendLine("Curve.BeginTransition();");
			Builder.AppendLine("Curve.BeginCircular(300, 0.105);");
			Builder.AppendLine("Curve.End();");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("curve", data.Statements[0].MapElement[0]);
				Assert.AreEqual("gauge", data.Statements[0].Function);
				object value;
				data.Statements[0].Arguments.TryGetValue("value", out value);
				Assert.AreEqual(1.067, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("curve", data.Statements[1].MapElement[0]);
				Assert.AreEqual("begintransition", data.Statements[1].Function);
			}
			{
				Assert.AreEqual("curve", data.Statements[2].MapElement[0]);
				Assert.AreEqual("begincircular", data.Statements[2].Function);
				object radius, cant;
				data.Statements[2].Arguments.TryGetValue("radius", out radius);
				data.Statements[2].Arguments.TryGetValue("cant", out cant);
				Assert.AreEqual(300, Convert.ToDouble(radius));
				Assert.AreEqual(0.105, Convert.ToDouble(cant));
			}
			{
				Assert.AreEqual("curve", data.Statements[3].MapElement[0]);
				Assert.AreEqual("end", data.Statements[3].Function);
			}
		}

	}
}
