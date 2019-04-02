using System.Globalization;
using Antlr4.Runtime.Misc;
using Bve5Parser.ScenarioGrammar.ANTLR_SyntaxDefinitions;

namespace Bve5Parser.ScenarioGrammar.AstNodes
{
	/// <summary>
	/// CSTを巡回してASTを作成するVisitorクラス
	/// </summary>
	internal class BuildAstVisitor : ScenarioGrammarParserBaseVisitor<ScenarioGrammarAstNodes>
	{
		/// <summary>
		/// ルートの巡回
		/// ステートメントの集合をノードに追加する
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>ルートASTノード</returns>
		public override ScenarioGrammarAstNodes VisitRoot([NotNull] ScenarioGrammarParser.RootContext context)
		{
			var node = new RootNode
			{
				Version = context.version,
				Encoding = context.encoding()
			};

			//ステートメントの追加
			foreach (var state in context.statement())
			{
				ScenarioGrammarAstNodes child = base.Visit(state);
				if (child != null)
				{
					node.StatementList.Add(child);
				}
			}

			return node;
		}

		#region ステートメントの巡回

		/// <summary>
		/// 路線ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>重み付けファイルパスASTノード</returns>
		public override ScenarioGrammarAstNodes VisitRouteState([NotNull] ScenarioGrammarParser.RouteStateContext context)
		{
			var node = new WeightStateNode
			{
				StateName = context.stateName.Text
			};

			foreach (var weight in context.weight_path())
			{
				ScenarioGrammarAstNodes child = base.Visit(weight);
				if (child != null)
				{
					node.PathList.Add(child);
				}
			}

			return node;
		}

		/// <summary>
		/// 車両ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>重み付けファイルパスASTノード</returns>
		public override ScenarioGrammarAstNodes VisitVehicleState([NotNull] ScenarioGrammarParser.VehicleStateContext context)
		{
			var node = new WeightStateNode
			{
				StateName = context.stateName.Text
			};

			foreach (var weight in context.weight_path())
			{
				ScenarioGrammarAstNodes child = base.Visit(weight);
				if (child != null)
				{
					node.PathList.Add(child);
				}
			}

			return node;
		}

		/// <summary>
		/// タイトルステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>テキストASTノード</returns>
		public override ScenarioGrammarAstNodes VisitTitleState([NotNull] ScenarioGrammarParser.TitleStateContext context)
		{
			return new TextStateNode { StateName = context.stateName.Text, Text = context.@string().text };
		}

		/// <summary>
		/// サムネイルステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>テキストASTノード</returns>
		public override ScenarioGrammarAstNodes VisitImageState([NotNull] ScenarioGrammarParser.ImageStateContext context)
		{
			return new TextStateNode { StateName = context.stateName.Text, Text = context.@string().text };
		}

		/// <summary>
		/// 路線名ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>テキストASTノード</returns>
		public override ScenarioGrammarAstNodes VisitRouteTitleState([NotNull] ScenarioGrammarParser.RouteTitleStateContext context)
		{
			return new TextStateNode { StateName = context.stateName.Text, Text = context.@string().text };
		}

		/// <summary>
		/// 車両名ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>テキストASTノード</returns>
		public override ScenarioGrammarAstNodes VisitVehicleTitleState([NotNull] ScenarioGrammarParser.VehicleTitleStateContext context)
		{
			return new TextStateNode { StateName = context.stateName.Text, Text = context.@string().text };
		}

		/// <summary>
		/// 作者名ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>テキストASTノード</returns>
		public override ScenarioGrammarAstNodes VisitAuthorState([NotNull] ScenarioGrammarParser.AuthorStateContext context)
		{
			return new TextStateNode { StateName = context.stateName.Text, Text = context.@string().text };
		}

		/// <summary>
		/// コメントステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>テキストASTノード</returns>
		public override ScenarioGrammarAstNodes VisitCommentState([NotNull] ScenarioGrammarParser.CommentStateContext context)
		{
			return new TextStateNode { StateName = context.stateName.Text, Text = context.@string().text };
		}

		#endregion ステートメントの巡回

		/// <summary>
		/// 重み付けファイルパスの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>WeightPathASTノード</returns>
		public override ScenarioGrammarAstNodes VisitWeight_path([NotNull] ScenarioGrammarParser.Weight_pathContext context)
		{
			var node = new WeightPathNode
			{
				Path = context.path.Text.Trim(), // ファイルパス前後の空白は削除する
				Weight = context.weight == null ? 1.0 : double.Parse(context.weight.Text, CultureInfo.InvariantCulture) // Weightの取得
			};

			return node;
		}
	}
}
