using System.IO;
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
		/// 引数に与えられたマップ構文の構文解析を行い、ASTを生成します。
		/// </summary>
		/// <param name="input">解析するマップ構文を表す文字列</param>
		/// <param name="filePath">解析するマップ構文のファイルパス</param>
		/// <returns></returns>
		internal override MapGrammarAstNodes ParseToAst(string input, string filePath = "")
		{
			var inputStream = new AntlrInputStream(input);
			var lexer = new MapV1GrammarLexer(inputStream);
			var commonTokenStream = new CommonTokenStream(lexer);
			var parser = new MapV1GrammarParser(commonTokenStream);

			parser.AddErrorListener(ErrorListener);
			ErrorListener.Errors.Clear();
			parser.ErrorHandler = new MapV1GrammarErrorStrategy(filePath);

			var cst = parser.root();
			var ast = new BuildAstVisitor().VisitRoot(cst);

			return ast;
		}

		/// <summary>
		/// 引数に与えられたMapGrammarの構文解析を行います。
		/// </summary>
		/// <param name="input">解析する文字列</param>
		public override MapData Parse(string input)
		{
			ParserErrors.Clear();

			var ast = ParseToAst(input);
			var data = (MapData)new EvaluateMapGrammarVisitor(ParserErrors).Visit(ast);

			return data;
		}

		/// <summary>
		/// 引数に与えられたマップ構文文字列の構文解析と評価を行い、MapDataを生成します。
		/// </summary>
		/// <param name="input">解析するマップ構文を表す文字列</param>
		/// <param name="option"></param>
		/// <returns></returns>
		public override MapData Parse(string input, MapParserOption option)
		{
			// TODO: 現在は特にオプション無し
			return Parse(input);
		}

		/// <summary>
		/// 引数に与えられたマップ構文ファイルの構文解析と評価を行い、MapDataを生成します。
		/// </summary>
		/// <param name="filePath">解析するマップ構文のファイルパス</param>
		/// <returns></returns>
		public override MapData ParseFromFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new IOException();  // TODO
			}

			ParserErrors.Clear();

			var encoding = DetermineFileEncoding(filePath);
			var ast = ParseToAst(File.ReadAllText(filePath, encoding), filePath);
			var data = (MapData)new EvaluateMapGrammarVisitor(ParserErrors).Visit(ast);

			return data;
		}

		/// <summary>
		/// 引数に与えられたマップ構文ファイルの構文解析と評価を行い、MapDataを生成します。
		/// </summary>
		/// <param name="filePath">解析するマップ構文のファイルパス</param>
		/// <param name="option"></param>
		/// <returns></returns>
		public override MapData ParseFromFile(string filePath, MapParserOption option)
		{
			// TODO: 現在は特にオプション無し
			return ParseFromFile(filePath);
		}
	}
}
