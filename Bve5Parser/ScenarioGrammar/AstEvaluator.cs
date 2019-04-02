using System.Collections.Generic;
using Bve5Parser.ScenarioGrammar.AstNodes;

namespace Bve5Parser.ScenarioGrammar
{
	/// <summary>
	/// ASTノードの評価クラス定義
	/// </summary>
	/// <typeparam name="T">ASTノードの種類</typeparam>
	internal abstract class AstVisitor<T>
	{
		public abstract T Visit(RootNode node);
		public abstract T Visit(WeightStateNode node);
		public abstract T Visit(TextStateNode node);
		public abstract T Visit(WeightPathNode node);

		/// <summary>
		/// 引数に与えられたASTノードを評価します。
		/// </summary>
		/// <param name="node">評価するASTノード</param>
		/// <returns>評価結果</returns>
		public T Visit(ScenarioGrammarAstNodes node)
		{
			return Visit((dynamic)node);
		}
	}

	/// <summary>
	/// ASTノードの評価手続きクラス
	/// </summary>
	internal class EvaluateScenarioGrammarVisitor : AstVisitor<object>
	{
		/// <summary>
		/// 評価結果
		/// </summary>
		private ScenarioData evaluateData;

		/// <summary>
		/// ルートノードの評価
		/// </summary>
		/// <param name="node">ルートノード</param>
		/// <returns>解析結果のScenarioData</returns>
		public override object Visit(RootNode node)
		{
			evaluateData = new ScenarioData
			{
				Route = new List<FilePath>(),
				Vehicle = new List<FilePath>()
			};

			if (node.Version != null)
			{
				evaluateData.Version = node.Version.Text;
			}

			if (node.Encoding != null)
			{
				evaluateData.Encoding = node.Encoding.text;
			}

			//ステートメントの巡回(出力は各ステートメントごとに行う)
			foreach (var statement in node.StatementList)
			{
				Visit(statement);
			}

			return evaluateData;
		}

		/// <summary>
		/// 重み付けステートメントノードの評価
		/// </summary>
		/// <param name="node">重み付けステートメントノード</param>
		/// <returns>戻り値なし</returns>
		public override object Visit(WeightStateNode node)
		{
			switch (node.StateName.ToLowerInvariant())
			{
				case "route":
					foreach (var path in node.PathList)
					{
						evaluateData.Route.Add((FilePath)Visit(path));
					}
					break;
				case "vehicle":
					foreach (var path in node.PathList)
					{
						evaluateData.Vehicle.Add((FilePath)Visit(path));
					}
					break;
			}

			return null;
		}

		/// <summary>
		/// テキストステートメントノードの評価
		/// </summary>
		/// <param name="node">テキストステートメントノード</param>
		/// <returns>戻り値なし</returns>
		public override object Visit(TextStateNode node)
		{
			switch (node.StateName.ToLowerInvariant())
			{
				case "image":
					evaluateData.Image = node.Text;
					break;
				case "title":
					evaluateData.Title = node.Text;
					break;
				case "routetitle":
					evaluateData.RouteTitle = node.Text;
					break;
				case "vehicletitle":
					evaluateData.VehicleTitle = node.Text;
					break;
				case "author":
					evaluateData.Author = node.Text;
					break;
				case "comment":
					evaluateData.Comment = node.Text;
					break;
			}

			return null;
		}

		/// <summary>
		/// 重み付けファイルパスノードの評価
		/// </summary>
		/// <param name="node">重み付けファイルパスノード</param>
		/// <returns>ファイルパス構造体</returns>
		public override object Visit(WeightPathNode node)
		{
			FilePath filePath = new FilePath
			{
				Value = node.Path,
				Weight = node.Weight
			};

			return filePath;
		}
	}
}
