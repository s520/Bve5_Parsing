using System.Collections.Generic;
using Antlr4.Runtime;
using Bve5Parser.ScenarioGrammar.ANTLR_SyntaxDefinitions;

namespace Bve5Parser.ScenarioGrammar.AstNodes
{
	// ScenarioGrammarのAstNode定義

	/// <summary>
	/// AST親クラス
	/// </summary>
	internal abstract class ScenarioGrammarAstNodes { }

	/// <summary>
	/// ルートノード
	/// </summary>
	internal class RootNode : ScenarioGrammarAstNodes
	{
		public IToken Version { get; set; }
		public ScenarioGrammarParser.EncodingContext Encoding { get; set; }
		public List<ScenarioGrammarAstNodes> StatementList { get; set; }

		public RootNode()
		{
			StatementList = new List<ScenarioGrammarAstNodes>();
		}
	}

	/// <summary>
	/// ステートメント親ノード
	/// </summary>
	internal abstract class StatementNode : ScenarioGrammarAstNodes
	{
		public string StateName { get; set; }
	}

	/// <summary>
	/// 重み付けステートメント
	/// </summary>
	internal class WeightStateNode : StatementNode
	{
		public List<ScenarioGrammarAstNodes> PathList { get; set; }

		public WeightStateNode()
		{
			PathList = new List<ScenarioGrammarAstNodes>();
		}
	}

	/// <summary>
	/// テキストステートメント
	/// </summary>
	internal class TextStateNode : StatementNode
	{
		public string Text { get; set; }
	}

	/// <summary>
	/// 重み付けパス
	/// </summary>
	internal class WeightPathNode : ScenarioGrammarAstNodes
	{
		public string Path { get; set; }
		public double Weight { get; set; }
	}
}
