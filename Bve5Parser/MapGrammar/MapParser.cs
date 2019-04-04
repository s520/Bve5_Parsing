using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bve5Parser.MapGrammar
{
	public abstract class MapParser
	{
		[Flags]
		public enum MapParserOption
		{
			/// <summary>
			/// オプション指定なし
			/// </summary>
			None = 0x001,

			/// <summary>
			/// パース中にInclude構文が出現した場合、指定されたファイルを再帰的にパースします。
			/// このフラグはParseメソッドにファイル名を指定した場合のみ有効です。
			/// </summary>
			ParseIncludeSyntaxRecursively = 0x002
		}

		/// <summary>
		/// 構文解析エラー
		/// </summary>
		public List<ParseError> ParserErrors { get; private set; }

		/// <summary>
		/// 構文解析のエラーを取得するリスナー
		/// </summary>
		public ParseErrorListener ErrorListener { get; set; }

		/// <summary>
		/// 構文解析器を初期化します。
		/// </summary>
		protected MapParser()
		{
			ParserErrors = new List<ParseError>();
			ErrorListener = new ParseErrorListener(ParserErrors);
		}

		/// <summary>Attempts to determine the System.Text.Encoding value for a given BVE5 file</summary>
		/// <param name="filePath">The file path</param>
		/// <returns>The detected encoding, or UTF-8 if this is not found</returns>
		internal Encoding DetermineFileEncoding(string filePath)
		{
			using (var reader = new StreamReader(filePath))
			{
				var firstLine = reader.ReadLine();

				if (firstLine == null)
				{
					return Encoding.UTF8;
				}

				var Header = firstLine.Split(':');

				if (Header.Length == 1)
				{
					return Encoding.UTF8;
				}

				var Arguments = Header[1].Split(',');

				try
				{
					return Encoding.GetEncoding(Arguments[0].ToLowerInvariant().Trim());
				}
				catch
				{
					return Encoding.UTF8;
				}
			}
		}

		internal abstract MapGrammarAstNodes ParseToAst(string input, string filePath = "");
		public abstract MapData Parse(string input);
		public abstract MapData Parse(string input, MapParserOption option);
		public abstract MapData ParseFromFile(string filePath);
		public abstract MapData ParseFromFile(string filePath, MapParserOption option);
	}
}
