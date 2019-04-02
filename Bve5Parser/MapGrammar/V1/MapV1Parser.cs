using Antlr4.Runtime;
using Bve5Parser.MapGrammar.V1.ANTLR_SyntaxDefinitions;
using Bve5Parser.MapGrammar.V1.AstNodes;

namespace Bve5Parser.MapGrammar.V1
{
	/// <summary>
	/// MapGrammarの解析を行うクラス
	/// </summary>
	public class MapV1Parser : MapParser
	{
		/// <summary>
		/// 引数に与えられたMapGrammarの構文解析を行います。
		/// </summary>
		/// <param name="input">解析する文字列</param>
		public override MapData Parse(string input)
		{
			ParserErrors.Clear();

			var inputStream = new AntlrInputStream(input);
			var lexer = new MapV1GrammarLexer(inputStream);
			var commonTokenStream = new CommonTokenStream(lexer);
			var parser = new MapV1GrammarParser(commonTokenStream);

			parser.AddErrorListener(ErrorListener);
			ErrorListener.Errors.Clear();
			parser.ErrorHandler = new MapV1GrammarErrorStrategy();

			var cst = parser.root();
			var ast = new BuildAstVisitor().VisitRoot(cst);
			var data = (MapData)new EvaluateMapGrammarVisitor(ParserErrors).Visit(ast);

			return data;
		}
	}
}
