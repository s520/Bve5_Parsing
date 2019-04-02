using System.Collections.Generic;
using Antlr4.Runtime;
using Bve5Parser.MapGrammar.V1.ANTLR_SyntaxDefinitions;

namespace Bve5Parser.MapGrammar.V1.AstNodes
{
	// AstNodeの定義

	/// <summary>
	/// ルートノード
	/// </summary>
	internal class RootNode : MapGrammarAstNodes
	{
		public List<MapGrammarAstNodes> StatementList { get; set; }
		public IToken Version { get; set; }
		public RootNode(IToken start, IToken stop) : base(start, stop)
		{
			StatementList = new List<MapGrammarAstNodes>();
		}
	}

	/// <summary>
	/// 距離程ノード
	/// </summary>
	internal class DistanceNode : MapGrammarAstNodes
	{
		public IToken Value { get; set; }

		public DistanceNode(IToken start, IToken stop) : base(start, stop) { }
	}

	#region ステートメント AST Nodes

	/// <summary>
	/// ステートメントノード1 MapElement.Function(Args)
	/// </summary>
	internal class Syntax1Node : MapGrammarAstNodes
	{
		public string MapElementName { get; set; }
		public string FunctionName { get; set; }
		public Dictionary<string, MapGrammarAstNodes> Arguments { get; set; }

		public Syntax1Node(IToken start, IToken stop) : base(start, stop)
		{
			Arguments = new Dictionary<string, MapGrammarAstNodes>();
		}
	}

	/// <summary>
	/// ステートメントノード2 MapElement[key].Function(Args)
	/// </summary>
	internal class Syntax2Node : MapGrammarAstNodes
	{
		public string MapElementName { get; set; }
		public MapGrammarAstNodes Key { get; set; }
		public string FunctionName { get; set; }
		public Dictionary<string, MapGrammarAstNodes> Arguments { get; set; }

		public Syntax2Node(IToken start, IToken stop) : base(start, stop)
		{
			Arguments = new Dictionary<string, MapGrammarAstNodes>();
		}
	}

	/// <summary>
	/// リストファイルロードノード
	/// </summary>
	internal class LoadListNode : MapGrammarAstNodes
	{
		public string MapElementName { get; set; }
		public MapGrammarAstNodes Path { get; set; }

		public LoadListNode(IToken start, IToken stop) : base(start, stop) { }
	}

	#endregion

	/// <summary>
	/// 文字列ノード
	/// </summary>
	internal class StringNode : MapGrammarAstNodes
	{
		public MapV1GrammarParser.StringContext Value { get; set; }

		public StringNode(IToken start, IToken stop) : base(start, stop) { }
	}
}
