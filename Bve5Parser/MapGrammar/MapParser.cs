using System.Collections.Generic;

namespace Bve5Parser.MapGrammar
{
    public abstract class MapParser
	{
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

        public abstract MapData Parse(string input);
    }
}
