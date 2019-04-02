using System;
using System.Collections.Generic;
using Bve5Parser.MapGrammar.V1.AstNodes;

namespace Bve5Parser.MapGrammar.V1
{
	/// <summary>
	/// ASTノードの評価クラス定義
	/// </summary>
	/// <typeparam name="T">ASTノードの種類</typeparam>
	internal abstract class AstVisitor<T>
	{
		/// <summary>
		/// エラー保持
		/// </summary>
		protected ICollection<ParseError> Errors;

		/// <summary>
		/// 新しいインスタンスを生成します。
		/// </summary>
		/// <param name="errors"></param>
		protected AstVisitor(ICollection<ParseError> errors)
		{
			Errors = errors;
		}

		public abstract T Visit(RootNode node);
		public abstract T Visit(DistanceNode node);
		public abstract T Visit(Syntax1Node node);
		public abstract T Visit(Syntax2Node node);
		public abstract T Visit(LoadListNode node);
		public abstract T Visit(StringNode node);

		/// <summary>
		/// 引数に与えられたASTノードを評価します。
		/// </summary>
		/// <param name="node">評価するASTノード</param>
		/// <returns>評価結果</returns>
		public T Visit(MapGrammarAstNodes node)
		{
			return Visit((dynamic)node);
		}
	}

	/// <summary>
	/// ASTノードの評価手続きクラス
	/// </summary>
	internal class EvaluateMapGrammarVisitor : AstVisitor<object>
	{
		/// <summary>
		/// 評価結果
		/// </summary>
		private MapData evaluateData;

		/// <summary>
		/// 現在評価中の距離程
		/// </summary>
		private double nowDistance;

		public EvaluateMapGrammarVisitor(ICollection<ParseError> errors) : base(errors) { }

		/// <summary>
		/// ルートノードの評価
		/// </summary>
		/// <param name="node">ルートノード</param>
		/// <returns>解析結果のMapData</returns>
		public override object Visit(RootNode node)
		{
			evaluateData = new MapData();

			if (node.Version != null)
			{
				evaluateData.Version = node.Version.Text;

				if (Convert.ToDouble(node.Version.Text) >= 2.0)
				{
					Errors.Add(node.CreateNewWarning(string.Format("バージョン：{0}はBve5Parserではサポートしていないバージョンです。", node.Version.Text)));
				}
			}

			foreach (var statement in node.StatementList)
			{
				var childData = Visit(statement);
				if (childData != null)
				{
					evaluateData.Statements.Add((SyntaxData) childData);
				}
			}

			return evaluateData;
		}

		/// <summary>
		/// 距離程の評価
		/// </summary>
		/// <param name="node">距離程ノード</param>
		/// <returns>null</returns>
		public override object Visit(DistanceNode node)
		{
			nowDistance = Convert.ToDouble(node.Value.Text);

			return null;
		}

		/// <summary>
		/// 構文タイプ1の評価
		/// </summary>
		/// <param name="node">Syntax1Node</param>
		/// <returns>解析結果のSyntaxDataクラス</returns>
		public override object Visit(Syntax1Node node)
		{
			//構文情報を登録する
			var returnData = new SyntaxData
			{
				Distance = nowDistance,
				MapElement = new string[1]
			};
			returnData.MapElement[0] = node.MapElementName;
			returnData.Function = node.FunctionName;

			foreach (var key in node.Arguments.Keys)
			{
				if (node.Arguments[key] != null)
				{
					returnData.Arguments.Add(key, Visit(node.Arguments[key]));
				}
				else
				{
					returnData.Arguments.Add(key, null);
				}
			}

			return returnData;
		}

		/// <summary>
		/// 構文タイプ2の評価
		/// </summary>
		/// <param name="node">Syntax2Node</param>
		/// <returns>解析結果のSyntaxDataクラス</returns>
		public override object Visit(Syntax2Node node)
		{
			//構文情報を登録する
			var returnData = new SyntaxData
			{
				Distance = nowDistance,
				MapElement = new string[1]
			};
			returnData.MapElement[0] = node.MapElementName;
			returnData.Key = Visit(node.Key).ToString();
			returnData.Function = node.FunctionName;

			foreach (var key in node.Arguments.Keys)
			{
				if (node.Arguments[key] != null)
				{
					returnData.Arguments.Add(key, Visit(node.Arguments[key]));
				}
				else
				{
					returnData.Arguments.Add(key, null);
				}
			}

			return returnData;
		}

		/// <summary>
		/// リストファイルノードの評価
		/// リストファイルの参照パスを追加する
		/// </summary>
		/// <param name="node">リストファイルノード</param>
		/// <returns>null</returns>
		public override object Visit(LoadListNode node)
		{
			if (node.Path == null)
			{
				Errors.Add(node.CreateNewError("ファイルパスが指定されていません。"));
			}
			else
			{
				switch (node.MapElementName)
				{
					case "structure":
						evaluateData.StructureListPath = Visit(node.Path).ToString();
						break;
					case "station":
						evaluateData.StationListPath = Visit(node.Path).ToString();
						break;
					case "signal":
						evaluateData.SignalListPath = Visit(node.Path).ToString();
						break;
					case "sound":
						evaluateData.SoundListPath = Visit(node.Path).ToString();
						break;
					case "sound3d":
						evaluateData.Sound3DListPath = Visit(node.Path).ToString();
						break;
				}
			}

			return null;
		}

		/// <summary>
		/// 文字列の評価
		/// </summary>
		/// <param name="node">文字列ノード</param>
		/// <returns>文字列(String)</returns>
		public override object Visit(StringNode node)
		{
			return node.Value.text;
		}
	}
}
