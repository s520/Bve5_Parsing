using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bve5Parser.MapGrammar.V2;
using NUnit.Framework;

namespace Bve5ParserTests
{
	[TestFixture]
	public class MapV2GrammarTests
	{
		private readonly MapV2Parser Parser = new MapV2Parser();
		private readonly StringBuilder Builder = new StringBuilder();

		[SetUp]
		public void SetUp()
		{
			Builder.Clear();
			Builder.AppendLine("BveTs Map 2.02:utf-8");
		}

		[TestCase("2.02", "utf-8")]
		[TestCase("2.00", "shift_jis")]
		[TestCase("1.00", "")]
		public void HeaderTest(string version, string encoding)
		{
			Builder.Clear();
			Builder.AppendLine("BveTs Map " + version + ":" + encoding);
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual(version, data.Version);
			Assert.AreEqual(encoding, data.Encoding);
		}

		[Test]
		public void IncludeTest()
		{
			Builder.AppendLine("include 'submap.txt';");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("include", data.Statements[0].MapElement[0]);
			object path, startIndex, stopIndex;
			data.Statements[0].Arguments.TryGetValue("path", out path);
            data.Statements[0].Arguments.TryGetValue("startindex", out startIndex);
            data.Statements[0].Arguments.TryGetValue("stopindex", out stopIndex);
			Assert.AreEqual("submap.txt", path);
            var left = Builder.ToString().Substring(0, Convert.ToInt32(startIndex));
            var right = Builder.ToString().Substring(Convert.ToInt32(stopIndex) + 1, Builder.ToString().Length - (Convert.ToInt32(stopIndex) + 1));
            var replaceLeft = Regex.Replace(left, @"i\s*n\s*c\s*l\s*u\s*d\s*e\s*\z", string.Empty, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var replaceRight = Regex.Replace(right, @"^\s*;", string.Empty, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

		[Test]
		public void CurveTest()
		{
			Builder.AppendLine("Curve.SetGauge(1.067);");
			Builder.AppendLine("Curve.Gauge(1.067);");
			Builder.AppendLine("Curve.SetCenter(.5);");
			Builder.AppendLine("Curve.SetFunction(1);");
			Builder.AppendLine("Curve.BeginTransition();");
			Builder.AppendLine("Curve.Begin(300);");
			Builder.AppendLine("Curve.BeginCircular(300, 0.105);");
			Builder.AppendLine("Curve.End();");
			Builder.AppendLine("Curve.Interpolate(300, 0.105);");
			Builder.AppendLine("Curve.Change(300);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("curve",data.Statements[0].MapElement[0]);
				Assert.AreEqual("setgauge", data.Statements[0].Function);
				object value;
				data.Statements[0].Arguments.TryGetValue("value", out value);
				Assert.AreEqual(1.067, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("curve", data.Statements[1].MapElement[0]);
				Assert.AreEqual("gauge", data.Statements[1].Function);
				object value;
				data.Statements[1].Arguments.TryGetValue("value", out value);
				Assert.AreEqual(1.067, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("curve", data.Statements[2].MapElement[0]);
				Assert.AreEqual("setcenter", data.Statements[2].Function);
				object value;
				data.Statements[2].Arguments.TryGetValue("x", out value);
				Assert.AreEqual(0.5, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("curve", data.Statements[3].MapElement[0]);
				Assert.AreEqual("setfunction", data.Statements[3].Function);
				object value;
				data.Statements[3].Arguments.TryGetValue("id", out value);
				Assert.AreEqual(1, Convert.ToInt32(value));
			}
			{
				Assert.AreEqual("curve", data.Statements[4].MapElement[0]);
				Assert.AreEqual("begintransition", data.Statements[4].Function);
			}
			{
				Assert.AreEqual("curve", data.Statements[5].MapElement[0]);
				Assert.AreEqual("begin", data.Statements[5].Function);
				object radius;
				data.Statements[5].Arguments.TryGetValue("radius", out radius);
				Assert.AreEqual(300, Convert.ToDouble(radius));
			}
			{
				Assert.AreEqual("curve", data.Statements[6].MapElement[0]);
				Assert.AreEqual("begincircular", data.Statements[6].Function);
				object radius, cant;
				data.Statements[6].Arguments.TryGetValue("radius", out radius);
				data.Statements[6].Arguments.TryGetValue("cant", out cant);
				Assert.AreEqual(300, Convert.ToDouble(radius));
				Assert.AreEqual(0.105, Convert.ToDouble(cant));
			}
			{
				Assert.AreEqual("curve", data.Statements[7].MapElement[0]);
				Assert.AreEqual("end", data.Statements[7].Function);
			}
			{
				Assert.AreEqual("curve", data.Statements[8].MapElement[0]);
				Assert.AreEqual("interpolate", data.Statements[8].Function);
				object radius, cant;
				data.Statements[8].Arguments.TryGetValue("radius", out radius);
				data.Statements[8].Arguments.TryGetValue("cant", out cant);
				Assert.AreEqual(300, Convert.ToDouble(radius));
				Assert.AreEqual(0.105, Convert.ToDouble(cant));
			}
			{
				Assert.AreEqual("curve", data.Statements[9].MapElement[0]);
				Assert.AreEqual("change", data.Statements[9].Function);
				object radius;
				data.Statements[9].Arguments.TryGetValue("radius", out radius);
				Assert.AreEqual(300, Convert.ToDouble(radius));
			}
		}

		[Test]
		public void GradientTest()
		{
			Builder.AppendLine("Gradient.BeginTransition();");
			Builder.AppendLine("Gradient.Begin(25);");
			Builder.AppendLine("Gradient.BeginConst(25);");
			Builder.AppendLine("Gradient.End();");
			Builder.AppendLine("Gradient.Interpolate(25);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("gradient", data.Statements[0].MapElement[0]);
				Assert.AreEqual("begintransition", data.Statements[0].Function);
			}
			{
				Assert.AreEqual("gradient", data.Statements[1].MapElement[0]);
				Assert.AreEqual("begin", data.Statements[1].Function);
				object value;
				data.Statements[1].Arguments.TryGetValue("gradient", out value);
				Assert.AreEqual(25, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("gradient", data.Statements[2].MapElement[0]);
				Assert.AreEqual("beginconst", data.Statements[2].Function);
				object value;
				data.Statements[2].Arguments.TryGetValue("gradient", out value);
				Assert.AreEqual(25, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("gradient", data.Statements[3].MapElement[0]);
				Assert.AreEqual("end", data.Statements[3].Function);
			}
			{
				Assert.AreEqual("gradient", data.Statements[4].MapElement[0]);
				Assert.AreEqual("interpolate", data.Statements[4].Function);
				object value;
				data.Statements[4].Arguments.TryGetValue("gradient", out value);
				Assert.AreEqual(25, Convert.ToDouble(value));
			}
		}

		[Test]
		public void TrackTest()
		{
			Builder.AppendLine("Track[1].X.Interpolate(6.25, 1000);");
			Builder.AppendLine("Track['A'].Y.Interpolate(6.25);");
			Builder.AppendLine("Track['b'].Position(6.25, .25, 1000, 400);");
			Builder.AppendLine("Track[1].Cant.SetGauge(1.067);");
			Builder.AppendLine("Track[1].Gauge(1.067);");
			Builder.AppendLine("Track[1].Cant.SetCenter(.5);");
			Builder.AppendLine("Track[1].Cant.SetFunction(1);");
			Builder.AppendLine("Track[1].Cant.BeginTransition();");
			Builder.AppendLine("Track[1].Cant.Begin(0.105);");
			Builder.AppendLine("Track[1].Cant.End();");
			Builder.AppendLine("Track[1].Cant.Interpolate(0.105);");
			Builder.AppendLine("Track[1].Cant(0.105);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("track", data.Statements[0].MapElement[0]);
				Assert.AreEqual("1", data.Statements[0].Key);
				Assert.AreEqual("x", data.Statements[0].MapElement[1]);
				Assert.AreEqual("interpolate", data.Statements[0].Function);
				object x, radius;
				data.Statements[0].Arguments.TryGetValue("x", out x);
				data.Statements[0].Arguments.TryGetValue("radius", out radius);
				Assert.AreEqual(6.25, Convert.ToDouble(x));
				Assert.AreEqual(1000, Convert.ToDouble(radius));
			}
			{
				Assert.AreEqual("track", data.Statements[1].MapElement[0]);
				Assert.AreEqual("A", data.Statements[1].Key);
				Assert.AreEqual("y", data.Statements[1].MapElement[1]);
				Assert.AreEqual("interpolate", data.Statements[1].Function);
				object y;
				data.Statements[1].Arguments.TryGetValue("y", out y);
				Assert.AreEqual(6.25, Convert.ToDouble(y));
			}
			{
				Assert.AreEqual("track", data.Statements[2].MapElement[0]);
				Assert.AreEqual("b", data.Statements[2].Key);
				Assert.AreEqual("position", data.Statements[2].Function);
				object x, y, radiusH, radiusV;
				data.Statements[2].Arguments.TryGetValue("x", out x);
				data.Statements[2].Arguments.TryGetValue("y", out y);
				data.Statements[2].Arguments.TryGetValue("radiush", out radiusH);
				data.Statements[2].Arguments.TryGetValue("radiusv", out radiusV);
				Assert.AreEqual(6.25, Convert.ToDouble(x));
				Assert.AreEqual(0.25, Convert.ToDouble(y));
				Assert.AreEqual(1000, Convert.ToDouble(radiusH));
				Assert.AreEqual(400, Convert.ToDouble(radiusV));
			}
			{
				Assert.AreEqual("track", data.Statements[3].MapElement[0]);
				Assert.AreEqual("1", data.Statements[3].Key);
				Assert.AreEqual("cant", data.Statements[3].MapElement[1]);
				Assert.AreEqual("setgauge", data.Statements[3].Function);
				object value;
				data.Statements[3].Arguments.TryGetValue("gauge", out value);
				Assert.AreEqual(1.067, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("track", data.Statements[4].MapElement[0]);
				Assert.AreEqual("1", data.Statements[4].Key);
				Assert.AreEqual("gauge", data.Statements[4].Function);
				object value;
				data.Statements[4].Arguments.TryGetValue("gauge", out value);
				Assert.AreEqual(1.067, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("track", data.Statements[5].MapElement[0]);
				Assert.AreEqual("1", data.Statements[5].Key);
				Assert.AreEqual("cant", data.Statements[5].MapElement[1]);
				Assert.AreEqual("setcenter", data.Statements[5].Function);
				object value;
				data.Statements[5].Arguments.TryGetValue("x", out value);
				Assert.AreEqual(0.5, Convert.ToDouble(value));
			}
			{
				Assert.AreEqual("track", data.Statements[6].MapElement[0]);
				Assert.AreEqual("1", data.Statements[6].Key);
				Assert.AreEqual("cant", data.Statements[6].MapElement[1]);
				Assert.AreEqual("setfunction", data.Statements[6].Function);
				object value;
				data.Statements[6].Arguments.TryGetValue("id", out value);
				Assert.AreEqual(1, Convert.ToInt32(value));
			}
			{
				Assert.AreEqual("track", data.Statements[7].MapElement[0]);
				Assert.AreEqual("1", data.Statements[7].Key);
				Assert.AreEqual("cant", data.Statements[7].MapElement[1]);
				Assert.AreEqual("begintransition", data.Statements[7].Function);
			}
			{
				Assert.AreEqual("track", data.Statements[8].MapElement[0]);
				Assert.AreEqual("1", data.Statements[8].Key);
				Assert.AreEqual("cant", data.Statements[8].MapElement[1]);
				Assert.AreEqual("begin", data.Statements[8].Function);
				object cant;
				data.Statements[8].Arguments.TryGetValue("cant", out cant);
				Assert.AreEqual(0.105, Convert.ToDouble(cant));
			}
			{
				Assert.AreEqual("track", data.Statements[9].MapElement[0]);
				Assert.AreEqual("1", data.Statements[9].Key);
				Assert.AreEqual("cant", data.Statements[9].MapElement[1]);
				Assert.AreEqual("end", data.Statements[9].Function);
			}
			{
				Assert.AreEqual("track", data.Statements[10].MapElement[0]);
				Assert.AreEqual("1", data.Statements[10].Key);
				Assert.AreEqual("cant", data.Statements[10].MapElement[1]);
				Assert.AreEqual("interpolate", data.Statements[10].Function);
				object cant;
				data.Statements[10].Arguments.TryGetValue("cant", out cant);
				Assert.AreEqual(0.105, Convert.ToDouble(cant));
			}
			{
				Assert.AreEqual("track", data.Statements[11].MapElement[0]);
				Assert.AreEqual("1", data.Statements[11].Key);
				Assert.AreEqual("cant", data.Statements[11].Function);
				object cant;
				data.Statements[11].Arguments.TryGetValue("cant", out cant);
				Assert.AreEqual(0.105, Convert.ToDouble(cant));
			}
		}

		[Test]
		public void StructureTest()
		{
			Builder.AppendLine("Structure.Load('Structures\\List.txt');");
			Builder.AppendLine("Structure['Ballast'].Put('A', 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 3, 5);");
			Builder.AppendLine("Structure['Ballast'].Put0('A', 3, 5);");
			Builder.AppendLine("Structure['Ballast'].PutBetween('A', 'B', 1);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("Structures\\List.txt", data.StructureListPath);
			}
			{
				Assert.AreEqual("structure", data.Statements[0].MapElement[0]);
				Assert.AreEqual("Ballast", data.Statements[0].Key);
				Assert.AreEqual("put", data.Statements[0].Function);
				object trackKey, x, y, z, rx, ry, rz, tilt, span;
				data.Statements[0].Arguments.TryGetValue("trackkey", out trackKey);
				data.Statements[0].Arguments.TryGetValue("x", out x);
				data.Statements[0].Arguments.TryGetValue("y", out y);
				data.Statements[0].Arguments.TryGetValue("z", out z);
				data.Statements[0].Arguments.TryGetValue("rx", out rx);
				data.Statements[0].Arguments.TryGetValue("ry", out ry);
				data.Statements[0].Arguments.TryGetValue("rz", out rz);
				data.Statements[0].Arguments.TryGetValue("tilt", out tilt);
				data.Statements[0].Arguments.TryGetValue("span", out span);
				Assert.AreEqual("A", Convert.ToString(trackKey));
				Assert.AreEqual(1.1, Convert.ToDouble(x));
				Assert.AreEqual(2.2, Convert.ToDouble(y));
				Assert.AreEqual(3.3, Convert.ToDouble(z));
				Assert.AreEqual(4.4, Convert.ToDouble(rx));
				Assert.AreEqual(5.5, Convert.ToDouble(ry));
				Assert.AreEqual(6.6, Convert.ToDouble(rz));
				Assert.AreEqual(3, Convert.ToInt32(tilt));
				Assert.AreEqual(5, Convert.ToDouble(span));
			}
			{
				Assert.AreEqual("structure", data.Statements[1].MapElement[0]);
				Assert.AreEqual("Ballast", data.Statements[1].Key);
				Assert.AreEqual("put0", data.Statements[1].Function);
				object trackKey, tilt, span;
				data.Statements[1].Arguments.TryGetValue("trackkey", out trackKey);
				data.Statements[1].Arguments.TryGetValue("tilt", out tilt);
				data.Statements[1].Arguments.TryGetValue("span", out span);
				Assert.AreEqual("A", Convert.ToString(trackKey));
				Assert.AreEqual(3, Convert.ToInt32(tilt));
				Assert.AreEqual(5, Convert.ToDouble(span));
			}
			{
				Assert.AreEqual("structure", data.Statements[2].MapElement[0]);
				Assert.AreEqual("Ballast", data.Statements[2].Key);
				Assert.AreEqual("putbetween", data.Statements[2].Function);
				object trackKey1, trackKey2, flag;
				data.Statements[2].Arguments.TryGetValue("trackkey1", out trackKey1);
				data.Statements[2].Arguments.TryGetValue("trackkey2", out trackKey2);
				data.Statements[2].Arguments.TryGetValue("flag", out flag);
				Assert.AreEqual("A", Convert.ToString(trackKey1));
				Assert.AreEqual("B", Convert.ToString(trackKey2));
				Assert.AreEqual(1, Convert.ToInt32(flag));
			}
		}

		[Test]
		public void RepeaterTest()
		{
			Builder.AppendLine("Repeater['RailR'].Begin('A', 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 3, 5, 5, 'RailR0', 'RailR1', );");
			Builder.AppendLine("Repeater['RailR'].Begin0('A', 3, 5, 5, 'RailR0', 'RailR1');");
			Builder.AppendLine("Repeater['RailR'].End();");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("repeater", data.Statements[0].MapElement[0]);
				Assert.AreEqual("RailR", data.Statements[0].Key);
				Assert.AreEqual("begin", data.Statements[0].Function);
				object trackKey, x, y, z, rx, ry, rz, tilt, span, interval, key1, key2;
				data.Statements[0].Arguments.TryGetValue("trackkey", out trackKey);
				data.Statements[0].Arguments.TryGetValue("x", out x);
				data.Statements[0].Arguments.TryGetValue("y", out y);
				data.Statements[0].Arguments.TryGetValue("z", out z);
				data.Statements[0].Arguments.TryGetValue("rx", out rx);
				data.Statements[0].Arguments.TryGetValue("ry", out ry);
				data.Statements[0].Arguments.TryGetValue("rz", out rz);
				data.Statements[0].Arguments.TryGetValue("tilt", out tilt);
				data.Statements[0].Arguments.TryGetValue("span", out span);
				data.Statements[0].Arguments.TryGetValue("interval", out interval);
				data.Statements[0].Arguments.TryGetValue("key1", out key1);
				data.Statements[0].Arguments.TryGetValue("key2", out key2);
				Assert.AreEqual("A", Convert.ToString(trackKey));
				Assert.AreEqual(1.1, Convert.ToDouble(x));
				Assert.AreEqual(2.2, Convert.ToDouble(y));
				Assert.AreEqual(3.3, Convert.ToDouble(z));
				Assert.AreEqual(4.4, Convert.ToDouble(rx));
				Assert.AreEqual(5.5, Convert.ToDouble(ry));
				Assert.AreEqual(6.6, Convert.ToDouble(rz));
				Assert.AreEqual(3, Convert.ToInt32(tilt));
				Assert.AreEqual(5, Convert.ToDouble(span));
				Assert.AreEqual(5, Convert.ToDouble(interval));
				Assert.AreEqual("RailR0", Convert.ToString(key1));
				Assert.AreEqual("RailR1", Convert.ToString(key2));
			}
			{
				Assert.AreEqual("repeater", data.Statements[1].MapElement[0]);
				Assert.AreEqual("RailR", data.Statements[1].Key);
				Assert.AreEqual("begin0", data.Statements[1].Function);
				object trackKey, tilt, span, interval, key1, key2;
				data.Statements[1].Arguments.TryGetValue("trackkey", out trackKey);
				data.Statements[1].Arguments.TryGetValue("tilt", out tilt);
				data.Statements[1].Arguments.TryGetValue("span", out span);
				data.Statements[1].Arguments.TryGetValue("interval", out interval);
				data.Statements[1].Arguments.TryGetValue("key1", out key1);
				data.Statements[1].Arguments.TryGetValue("key2", out key2);
				Assert.AreEqual("A", Convert.ToString(trackKey));
				Assert.AreEqual(3, Convert.ToInt32(tilt));
				Assert.AreEqual(5, Convert.ToDouble(span));
				Assert.AreEqual(5, Convert.ToDouble(interval));
				Assert.AreEqual("RailR0", Convert.ToString(key1));
				Assert.AreEqual("RailR1", Convert.ToString(key2));
			}
			{
				Assert.AreEqual("repeater", data.Statements[2].MapElement[0]);
				Assert.AreEqual("RailR", data.Statements[2].Key);
				Assert.AreEqual("end", data.Statements[2].Function);
			}
		}

		[Test]
		public void BackgroundTest()
		{
			Builder.AppendLine("Background.Change('Bg');");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("background", data.Statements[0].MapElement[0]);
			Assert.AreEqual("change", data.Statements[0].Function);
			object value;
			data.Statements[0].Arguments.TryGetValue("structurekey", out value);
			Assert.AreEqual("Bg", Convert.ToString(value));
		}

		[Test]
		public void StationTest()
		{
			Builder.AppendLine("Station.Load('Station.txt');");
			Builder.AppendLine("Station['sta1'].Put(1, 2, 3);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("Station.txt", data.StationListPath);
			}
			{
				Assert.AreEqual("station", data.Statements[0].MapElement[0]);
				Assert.AreEqual("sta1", data.Statements[0].Key);
				Assert.AreEqual("put", data.Statements[0].Function);
				object door, margin1, margin2;
				data.Statements[0].Arguments.TryGetValue("door", out door);
				data.Statements[0].Arguments.TryGetValue("margin1", out margin1);
				data.Statements[0].Arguments.TryGetValue("margin2", out margin2);
				Assert.AreEqual(1, Convert.ToInt32(door));
				Assert.AreEqual(2, Convert.ToDouble(margin1));
				Assert.AreEqual(3, Convert.ToDouble(margin2));
			}
		}

		[Test]
		public void SectionTest()
		{
			Builder.AppendLine("Section.Begin(1, 2, 3);");
			Builder.AppendLine("Section.BeginNew(1, 2, 3);");
			Builder.AppendLine("Section.SetSpeedLimit(25, 40, 70);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("section", data.Statements[0].MapElement[0]);
				Assert.AreEqual("begin", data.Statements[0].Function);
				object signal0, signal1, signal2;
				data.Statements[0].Arguments.TryGetValue("signal0", out signal0);
				data.Statements[0].Arguments.TryGetValue("signal1", out signal1);
				data.Statements[0].Arguments.TryGetValue("signal2", out signal2);
				Assert.AreEqual(1, Convert.ToInt32(signal0));
				Assert.AreEqual(2, Convert.ToInt32(signal1));
				Assert.AreEqual(3, Convert.ToInt32(signal2));
			}
			{
				Assert.AreEqual("section", data.Statements[1].MapElement[0]);
				Assert.AreEqual("beginnew", data.Statements[1].Function);
				object signal0, signal1, signal2;
				data.Statements[1].Arguments.TryGetValue("signal0", out signal0);
				data.Statements[1].Arguments.TryGetValue("signal1", out signal1);
				data.Statements[1].Arguments.TryGetValue("signal2", out signal2);
				Assert.AreEqual(1, Convert.ToInt32(signal0));
				Assert.AreEqual(2, Convert.ToInt32(signal1));
				Assert.AreEqual(3, Convert.ToInt32(signal2));
			}
			{
				Assert.AreEqual("section", data.Statements[2].MapElement[0]);
				Assert.AreEqual("setspeedlimit", data.Statements[2].Function);
				object v0, v1, v2;
				data.Statements[2].Arguments.TryGetValue("v0", out v0);
				data.Statements[2].Arguments.TryGetValue("v1", out v1);
				data.Statements[2].Arguments.TryGetValue("v2", out v2);
				Assert.AreEqual(25, Convert.ToDouble(v0));
				Assert.AreEqual(40, Convert.ToDouble(v1));
				Assert.AreEqual(70, Convert.ToDouble(v2));
			}
		}

		[Test]
		public void SignalTest()
		{
			Builder.AppendLine("Signal.Load('Signal.csv');");
			Builder.AppendLine("Signal.SpeedLimit(25, 40, 70);");
			Builder.AppendLine("Signal['sig1'].Put(1, 'A', 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 3, 5);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("Signal.csv", data.SignalListPath);
			}
			{
				Assert.AreEqual("signal", data.Statements[0].MapElement[0]);
				Assert.AreEqual("speedlimit", data.Statements[0].Function);
				object v0, v1, v2;
				data.Statements[0].Arguments.TryGetValue("v0", out v0);
				data.Statements[0].Arguments.TryGetValue("v1", out v1);
				data.Statements[0].Arguments.TryGetValue("v2", out v2);
				Assert.AreEqual(25, Convert.ToDouble(v0));
				Assert.AreEqual(40, Convert.ToDouble(v1));
				Assert.AreEqual(70, Convert.ToDouble(v2));
			}
			{
				Assert.AreEqual("signal", data.Statements[1].MapElement[0]);
				Assert.AreEqual("sig1", data.Statements[1].Key);
				Assert.AreEqual("put", data.Statements[1].Function);
				object section, trackKey, x, y, z, rx, ry, rz, tilt, span;
				data.Statements[1].Arguments.TryGetValue("section", out section);
				data.Statements[1].Arguments.TryGetValue("trackkey", out trackKey);
				data.Statements[1].Arguments.TryGetValue("x", out x);
				data.Statements[1].Arguments.TryGetValue("y", out y);
				data.Statements[1].Arguments.TryGetValue("z", out z);
				data.Statements[1].Arguments.TryGetValue("rx", out rx);
				data.Statements[1].Arguments.TryGetValue("ry", out ry);
				data.Statements[1].Arguments.TryGetValue("rz", out rz);
				data.Statements[1].Arguments.TryGetValue("tilt", out tilt);
				data.Statements[1].Arguments.TryGetValue("span", out span);
				Assert.AreEqual(1, Convert.ToInt32(section));
				Assert.AreEqual("A", Convert.ToString(trackKey));
				Assert.AreEqual(1.1, Convert.ToDouble(x));
				Assert.AreEqual(2.2, Convert.ToDouble(y));
				Assert.AreEqual(3.3, Convert.ToDouble(z));
				Assert.AreEqual(4.4, Convert.ToDouble(rx));
				Assert.AreEqual(5.5, Convert.ToDouble(ry));
				Assert.AreEqual(6.6, Convert.ToDouble(rz));
				Assert.AreEqual(3, Convert.ToInt32(tilt));
				Assert.AreEqual(5, Convert.ToDouble(span));
			}
		}

		[Test]
		public void BeaconTest()
		{
			Builder.AppendLine("Beacon.Put(1, 2, 3);");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("beacon", data.Statements[0].MapElement[0]);
			Assert.AreEqual("put", data.Statements[0].Function);
			object type, section, sendData;
			data.Statements[0].Arguments.TryGetValue("type", out type);
			data.Statements[0].Arguments.TryGetValue("section", out section);
			data.Statements[0].Arguments.TryGetValue("senddata", out sendData);
			Assert.AreEqual(1, Convert.ToInt32(type));
			Assert.AreEqual(2, Convert.ToInt32(section));
			Assert.AreEqual(3, Convert.ToInt32(sendData));
		}

		[Test]
		public void SpeedLimitTest()
		{
			Builder.AppendLine("SpeedLimit.Begin(70);");
			Builder.AppendLine("SpeedLimit.End();");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("speedlimit", data.Statements[0].MapElement[0]);
				Assert.AreEqual("begin", data.Statements[0].Function);
				object v;
				data.Statements[0].Arguments.TryGetValue("v", out v);
				Assert.AreEqual(70, Convert.ToDouble(v));
			}
			{
				Assert.AreEqual("speedlimit", data.Statements[1].MapElement[0]);
				Assert.AreEqual("end", data.Statements[1].Function);
			}
		}

		[Test]
		public void PreTrainTest()
		{
			Builder.AppendLine("PreTrain.Pass('13:17:40');");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("pretrain", data.Statements[0].MapElement[0]);
			Assert.AreEqual("pass", data.Statements[0].Function);
			object time;
			data.Statements[0].Arguments.TryGetValue("time", out time);
			Assert.AreEqual("13:17:40", Convert.ToString(time));
		}

		[Test]
		public void LightTest()
		{
			Builder.AppendLine("Light.Ambient(0.1, 0.2, 0.3);");
			Builder.AppendLine("Light.Diffuse(0.1, 0.2, 0.3);");
			Builder.AppendLine("Light.Direction(0.1, 0.2);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("light", data.Statements[0].MapElement[0]);
				Assert.AreEqual("ambient", data.Statements[0].Function);
				object red, green, blue;
				data.Statements[0].Arguments.TryGetValue("red", out red);
				data.Statements[0].Arguments.TryGetValue("green", out green);
				data.Statements[0].Arguments.TryGetValue("blue", out blue);
				Assert.AreEqual(0.1, Convert.ToDouble(red));
				Assert.AreEqual(0.2, Convert.ToDouble(green));
				Assert.AreEqual(0.3, Convert.ToDouble(blue));
			}
			{
				Assert.AreEqual("light", data.Statements[1].MapElement[0]);
				Assert.AreEqual("diffuse", data.Statements[1].Function);
				object red, green, blue;
				data.Statements[1].Arguments.TryGetValue("red", out red);
				data.Statements[1].Arguments.TryGetValue("green", out green);
				data.Statements[1].Arguments.TryGetValue("blue", out blue);
				Assert.AreEqual(0.1, Convert.ToDouble(red));
				Assert.AreEqual(0.2, Convert.ToDouble(green));
				Assert.AreEqual(0.3, Convert.ToDouble(blue));
			}
			{
				Assert.AreEqual("light", data.Statements[2].MapElement[0]);
				Assert.AreEqual("direction", data.Statements[2].Function);
				object pitch, yaw;
				data.Statements[2].Arguments.TryGetValue("pitch", out pitch);
				data.Statements[2].Arguments.TryGetValue("yaw", out yaw);
				Assert.AreEqual(0.1, Convert.ToDouble(pitch));
				Assert.AreEqual(0.2, Convert.ToDouble(yaw));
			}
		}

		[Test]
		public void FogTest()
		{
			Builder.AppendLine("Fog.Interpolate(50);");
			Builder.AppendLine("Fog.Set(50, 0.1, 0.2, 0.3);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("fog", data.Statements[0].MapElement[0]);
				Assert.AreEqual("interpolate", data.Statements[0].Function);
				object density;
				data.Statements[0].Arguments.TryGetValue("density", out density);
				Assert.AreEqual(50, Convert.ToDouble(density));
			}
			{
				Assert.AreEqual("fog", data.Statements[1].MapElement[0]);
				Assert.AreEqual("set", data.Statements[1].Function);
				object density, red, green, blue;
				data.Statements[1].Arguments.TryGetValue("density", out density);
				data.Statements[1].Arguments.TryGetValue("red", out red);
				data.Statements[1].Arguments.TryGetValue("green", out green);
				data.Statements[1].Arguments.TryGetValue("blue", out blue);
				Assert.AreEqual(50, Convert.ToDouble(density));
				Assert.AreEqual(0.1, Convert.ToDouble(red));
				Assert.AreEqual(0.2, Convert.ToDouble(green));
				Assert.AreEqual(0.3, Convert.ToDouble(blue));
			}
		}

		[Test]
		public void DrawDistanceTest()
		{
			Builder.AppendLine("DrawDistance.Change(200);");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("drawdistance", data.Statements[0].MapElement[0]);
			Assert.AreEqual("change", data.Statements[0].Function);
			object value;
			data.Statements[0].Arguments.TryGetValue("value", out value);
			Assert.AreEqual(200, Convert.ToDouble(value));
		}

		[Test]
		public void CabIlluminanceTest()
		{
			Builder.AppendLine("CabIlluminance.Interpolate();");
			Builder.AppendLine("CabIlluminance.Set(1);");
			var data = Parser.Parse(Builder.ToString());
			{
				Assert.AreEqual("cabilluminance", data.Statements[0].MapElement[0]);
				Assert.AreEqual("interpolate", data.Statements[0].Function);
			}
			{
				Assert.AreEqual("cabilluminance", data.Statements[1].MapElement[0]);
				Assert.AreEqual("set", data.Statements[1].Function);
				object value;
				data.Statements[1].Arguments.TryGetValue("value", out value);
				Assert.AreEqual(1, Convert.ToDouble(value));
			}
		}

		[Test]
		public void IrregularityTest()
		{
			Builder.AppendLine("Irregularity.Change(1.1, 2.2, 3.3, 4.4, 5.5, 6.6);");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("irregularity", data.Statements[0].MapElement[0]);
			Assert.AreEqual("change", data.Statements[0].Function);
			object x, y, r, lx, ly, lr;
			data.Statements[0].Arguments.TryGetValue("x", out x);
			data.Statements[0].Arguments.TryGetValue("y", out y);
			data.Statements[0].Arguments.TryGetValue("r", out r);
			data.Statements[0].Arguments.TryGetValue("lx", out lx);
			data.Statements[0].Arguments.TryGetValue("ly", out ly);
			data.Statements[0].Arguments.TryGetValue("lr", out lr);
			Assert.AreEqual(1.1, Convert.ToDouble(x));
			Assert.AreEqual(2.2, Convert.ToDouble(y));
			Assert.AreEqual(3.3, Convert.ToDouble(r));
			Assert.AreEqual(4.4, Convert.ToDouble(lx));
			Assert.AreEqual(5.5, Convert.ToDouble(ly));
			Assert.AreEqual(6.6, Convert.ToDouble(lr));
		}

		[Test]
		public void AdhesionTest()
		{
			Builder.AppendLine("Adhesion.Change(1.1, 2.2, 3.3);");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("adhesion", data.Statements[0].MapElement[0]);
			Assert.AreEqual("change", data.Statements[0].Function);
			object a, b, c;
			data.Statements[0].Arguments.TryGetValue("a", out a);
			data.Statements[0].Arguments.TryGetValue("b", out b);
			data.Statements[0].Arguments.TryGetValue("c", out c);
			Assert.AreEqual(1.1, Convert.ToDouble(a));
			Assert.AreEqual(2.2, Convert.ToDouble(b));
			Assert.AreEqual(3.3, Convert.ToDouble(c));
		}

		[Test]
		public void SoundTest()
		{
			Builder.AppendLine("Sound.Load('sound.csv');");
			Builder.AppendLine("Sound['sound1'].Play();");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("sound.csv", data.SoundListPath);
			Assert.AreEqual("sound", data.Statements[0].MapElement[0]);
			Assert.AreEqual("sound1", data.Statements[0].Key);
			Assert.AreEqual("play", data.Statements[0].Function);
		}

		[Test]
		public void Sound3dTest()
		{
			Builder.AppendLine("Sound3D.Load('sound3d.csv');");
			Builder.AppendLine("Sound3D['sound1'].Put(1.1, 2.2);");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("sound3d.csv", data.Sound3DListPath);
			Assert.AreEqual("sound3d", data.Statements[0].MapElement[0]);
			Assert.AreEqual("sound1", data.Statements[0].Key);
			Assert.AreEqual("put", data.Statements[0].Function);
			object x, y;
			data.Statements[0].Arguments.TryGetValue("x", out x);
			data.Statements[0].Arguments.TryGetValue("y", out y);
			Assert.AreEqual(1.1, Convert.ToDouble(x));
			Assert.AreEqual(2.2, Convert.ToDouble(y));
		}

		[Test]
		public void RollingNoiseTest()
		{
			Builder.AppendLine("RollingNoise.Change(1);");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("rollingnoise", data.Statements[0].MapElement[0]);
			Assert.AreEqual("change", data.Statements[0].Function);
			object index;
			data.Statements[0].Arguments.TryGetValue("index", out index);
			Assert.AreEqual(1, Convert.ToInt32(index));
		}

		[Test]
		public void FlangeNoiseTest()
		{
			Builder.AppendLine("FlangeNoise.Change(1);");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("flangenoise", data.Statements[0].MapElement[0]);
			Assert.AreEqual("change", data.Statements[0].Function);
			object index;
			data.Statements[0].Arguments.TryGetValue("index", out index);
			Assert.AreEqual(1, Convert.ToInt32(index));
		}

		[Test]
		public void JointNoiseTest()
		{
			Builder.AppendLine("JointNoise.Play(1);");
			var data = Parser.Parse(Builder.ToString());
			Assert.AreEqual("jointnoise", data.Statements[0].MapElement[0]);
			Assert.AreEqual("play", data.Statements[0].Function);
			object index;
			data.Statements[0].Arguments.TryGetValue("index", out index);
			Assert.AreEqual(1, Convert.ToInt32(index));
		}
	}
}
