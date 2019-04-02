using Antlr4.Runtime;
using Bve5Parser.ScenarioGrammar.ANTLR_SyntaxDefinitions;
using Bve5Parser.ScenarioGrammar.AstNodes;

namespace Bve5Parser.ScenarioGrammar
{
	/// <summary>
	/// ScenarioGrammarの解析を行うクラス
	/// </summary>
	public class ScenarioParser
	{
		/// <summary>
		/// 構文解析のエラーを取得するリスナー
		/// </summary>
		public ParseErrorListener ErrorListener { get; set; }

		/// <summary>
		/// ScenarioGrammarの構文解析器を初期化します。
		/// </summary>
		public ScenarioParser()
		{
			ErrorListener = new ParseErrorListener();
		}

		/// <summary>
		/// 引数に与えられたScenarioGrammarの構文解析を行います。
		/// </summary>
		/// <param name="input">解析する文字列</param>
		/// <returns>解析結果</returns>
		public ScenarioData Parse(string input)
		{
			var inputStream = new AntlrInputStream(input);
			var lexer = new ScenarioGrammarLexer(inputStream);
			var commonTokenStream = new CommonTokenStream(lexer);
			var parser = new ScenarioGrammarParser(commonTokenStream);

			parser.AddErrorListener(ErrorListener);

			var cst = parser.root();
			var ast = new BuildAstVisitor().VisitRoot(cst);
			var data = (ScenarioData)new EvaluateScenarioGrammarVisitor().Visit(ast);

			return data;
		}
	}
}
