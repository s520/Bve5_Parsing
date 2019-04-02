using System.Collections.Generic;
using Antlr4.Runtime;
using Bve5Parser.MapGrammar.V2.ANTLR_SyntaxDefinitions;
using Bve5Parser.MapGrammar.V2.AstNodes;

namespace Bve5Parser.MapGrammar.V2
{
	/// <summary>
	/// MapGrammarの解析を行うクラス
	/// </summary>
	public class MapV2Parser : MapParser
	{
		/// <summary>
		/// マップ構文の変数管理用
		/// </summary>
		public VariableStore Store { get; set; }

		/// <summary>
		/// 構文解析器を初期化します。
		/// </summary>
		public MapV2Parser()
        {
			Store = new VariableStore();
		}

		/// <summary>
		/// 引数に与えられたMapGrammarの構文解析を行います。
		/// </summary>
		/// <param name="input">解析する文字列</param>
		public override MapData Parse(string input)
		{
			Store.ClearVar();
			ParserErrors.Clear();

			var inputStream = new AntlrInputStream(input);
			var lexer = new MapV2GrammarLexer(inputStream);
			var commonTokenStream = new CommonTokenStream(lexer);
			var parser = new MapV2GrammarParser(commonTokenStream);

			parser.AddErrorListener(ErrorListener);
			ErrorListener.Errors.Clear();
			parser.ErrorHandler = new MapV2GrammarErrorStrategy();

			var cst = parser.root();
			var ast = new BuildAstVisitor().VisitRoot(cst);
			var data = (MapData)new EvaluateMapGrammarVisitor(Store, ParserErrors).Visit(ast);

			return data;
		}
	}
}
