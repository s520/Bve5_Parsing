﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Bve5Parser.MapGrammar.V2.AstNodes;

namespace Bve5Parser.MapGrammar.V2
{
	/// <summary>
	/// ASTノードの評価クラス定義
	/// </summary>
	/// <typeparam name="T">ASTノードの種類</typeparam>
	internal abstract class AstVisitor<T>
	{
		/// <summary>
		/// 変数管理
		/// </summary>
		protected VariableStore Store;

		/// <summary>
		/// エラー保持
		/// </summary>
		protected ICollection<ParseError> Errors;

		/// <summary>
		/// 新しいインスタンスを生成します。
		/// </summary>
		/// <param name="store"></param>
		/// <param name="errors"></param>
		protected AstVisitor(VariableStore store, ICollection<ParseError> errors)
		{
			Store = store;
			Errors = errors;
		}

		public abstract T Visit(RootNode node);
		public abstract T Visit(DistanceNode node);
		public abstract T Visit(Syntax1Node node);
		public abstract T Visit(Syntax2Node node);
		public abstract T Visit(Syntax3Node node);
		public abstract T Visit(LoadListNode node);
		public abstract T Visit(VarAssignNode node);
		public abstract T Visit(AdditionNode node);
		public abstract T Visit(SubtractionNode node);
		public abstract T Visit(MultiplicationNode node);
		public abstract T Visit(DivisionNode node);
		public abstract T Visit(UnaryNode node);
		public abstract T Visit(ModuloNode node);
		public abstract T Visit(AbsNode node);
		public abstract T Visit(Atan2Node node);
		public abstract T Visit(CeilNode node);
		public abstract T Visit(CosNode node);
		public abstract T Visit(ExpNode node);
		public abstract T Visit(FloorNode node);
		public abstract T Visit(LogNode node);
		public abstract T Visit(PowNode node);
		public abstract T Visit(RandNode node);
		public abstract T Visit(SinNode node);
		public abstract T Visit(SqrtNode node);
		public abstract T Visit(VarNode node);
		public abstract T Visit(NumberNode node);
		public abstract T Visit(StringNode node);
		public abstract T Visit(DistanceVariableNode node);

		/// <summary>
		/// 引数に与えられたASTノードを評価します。
		/// </summary>
		/// <param name="node">評価するASTノード</param>
		/// <returns>評価結果</returns>
		public T Visit(MapGrammarAstNodes node)
		{
			return Visit((dynamic) node);
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

		public EvaluateMapGrammarVisitor(VariableStore store, ICollection<ParseError> errors) : base(store, errors) { }

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

				if (Convert.ToDouble(node.Version.Text) < 2.0)
				{
					Errors.Add(node.CreateNewWarning(string.Format("バージョン：{0}はBve5Parserではサポートしていないバージョンです。", node.Version.Text)));
				}
			}

			if (node.Encoding != null)
			{
				evaluateData.Encoding = node.Encoding.text;
			}

			foreach (var statement in node.StatementList)
			{
				var childData = Visit(statement);
				if (childData != null)
				{
					evaluateData.Statements.Add((SyntaxData)childData);
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
			nowDistance = Convert.ToDouble(Visit(node.Value));

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

            if (node.MapElementName == "include")
            {
                returnData.Arguments.Add("startindex", node.Start.StartIndex);
                returnData.Arguments.Add("stopindex", node.Stop.StopIndex);
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
		/// 構文タイプ3の評価
		/// </summary>
		/// <param name="node">Syntax3Node</param>
		/// <returns>解析結果のSyntaxDataクラス</returns>
		public override object Visit(Syntax3Node node)
		{
			//構文情報を登録する
			var returnData = new SyntaxData
			{
				Distance = nowDistance,
				MapElement = node.MapElementNames,
				Key = Visit(node.Key).ToString(),
				Function = node.FunctionName
			};

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
		/// 変数宣言ノードの評価
		/// </summary>
		/// <param name="node">変数宣言ノード</param>
		/// <returns>null</returns>
		public override object Visit(VarAssignNode node)
		{
			var val = Visit(node.Value);
			Store.SetVar(node.VarName, val);
			return null;
		}

		#region 数式ノードの評価

		/// <summary>
		/// 加算ノードの評価
		/// </summary>
		/// <param name="node">加算ノード</param>
		/// <returns>演算後の数値(Double)、もしくは文字列(String)</returns>
		public override object Visit(AdditionNode node)
		{
			var left = Visit(node.Left);
			var right = Visit(node.Right);

			if (left is string || right is string)
			{
				return left.ToString() + right.ToString(); // 文字列の結合
			}

			return Convert.ToDouble(left) + Convert.ToDouble(right);
		}

		/// <summary>
		/// 減算ノードの評価
		/// </summary>
		/// <param name="node">減算ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(SubtractionNode node)
		{
			var left = Visit(node.Left);
			var right = Visit(node.Right);

			if (left == null || right == null)
			{
				return null;
			}

			if (left is string || right is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'{0} - {1}'は有効な式ではありません。", left, right)));
				return null;
			}

			return Convert.ToDouble(left) - Convert.ToDouble(right);
		}

		/// <summary>
		/// 乗算ノードの評価
		/// </summary>
		/// <param name="node">乗算ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(MultiplicationNode node)
		{
			var left = Visit(node.Left);
			var right = Visit(node.Right);

			if (left == null || right == null)
			{
				return null;
			}

			if (left is string || right is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'{0} * {1}'は有効な式ではありません。", left, right)));
				return null;
			}

			return Convert.ToDouble(left) * Convert.ToDouble(right);
		}

		/// <summary>
		/// 除算ノードの評価
		/// </summary>
		/// <param name="node">除算ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(DivisionNode node)
		{
			var left = Visit(node.Left);
			var right = Visit(node.Right);

			if (left == null || right == null)
			{
				return null;
			}

			if (left is string || right is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'{0} / {1}'は有効な式ではありません。", left, right)));
				return null;
			}

			// TODO: 0除算対策
			return Convert.ToDouble(left) / Convert.ToDouble(right);
		}

		/// <summary>
		/// ユーナリ演算ノードの評価
		/// </summary>
		/// <param name="node">ユーナリ演算ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(UnaryNode node)
		{
			var inner = Visit(node.InnerNode);

			if (inner == null)
			{
				return null;
			}

			if (inner is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'- {0}'は有効な式ではありません。", inner)));
				return null;
			}

			return -Convert.ToDouble(Visit(node.InnerNode));
		}

		/// <summary>
		/// 剰余算ノードの評価
		/// </summary>
		/// <param name="node">剰余算ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(ModuloNode node)
		{
			var left = Visit(node.Left);
			var right = Visit(node.Right);

			if (left == null || right == null)
			{
				return null;
			}

			if (left is string || right is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'{0} % {1}'は有効な式ではありません。", left, right)));
				return null;
			}

			return Convert.ToDouble(left) % Convert.ToDouble(right);
		}

		/// <summary>
		/// 絶対値関数の評価
		/// </summary>
		/// <param name="node">絶対値関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(AbsNode node)
		{
			var value = Visit(node.Value);

			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'abs({0})'は有効な式ではありません。", value)));
				return null;
			}

			return Math.Abs(Convert.ToDouble(value));
		}

		/// <summary>
		/// Atan2関数の評価
		/// </summary>
		/// <param name="node">Atan2ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(Atan2Node node)
		{
			var y = Visit(node.Y);
			var x = Visit(node.X);

			if (y == null || x == null)
			{
				return null;
			}

			if (y is string || x is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'atan2({0}, {1})'は有効な式ではありません。", y, x)));
				return null;
			}

			return Math.Atan2(Convert.ToDouble(y), Convert.ToDouble(x));
		}

		/// <summary>
		/// 切り上げ関数の評価
		/// </summary>
		/// <param name="node">切り上げ関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(CeilNode node)
		{
			var value = Visit(node.Value);

			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'ceil({0})'は有効な式ではありません。", value)));
				return null;
			}

			return Math.Ceiling(Convert.ToDouble(value));
		}

		/// <summary>
		/// 余弦関数の評価
		/// </summary>
		/// <param name="node">余弦関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(CosNode node)
		{
			var value = Visit(node.Value);

			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'cos({0})'は有効な式ではありません。", value)));
				return null;
			}

			return Math.Cos(Convert.ToDouble(value));
		}

		/// <summary>
		/// 累乗関数の評価
		/// </summary>
		/// <param name="node">累乗関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(ExpNode node)
		{
			var value = Visit(node.Value);

			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'exp({0})'は有効な式ではありません。", value)));
				return null;
			}

			return Math.Exp(Convert.ToDouble(value));
		}

		/// <summary>
		/// 切り捨て関数の評価
		/// </summary>
		/// <param name="node">切り捨て関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(FloorNode node)
		{
			var value = Visit(node.Value);

			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'floor({0})'は有効な式ではありません。", value)));
				return null;
			}

			return Math.Floor(Convert.ToDouble(value));
		}

		/// <summary>
		/// 自然対数関数の評価
		/// </summary>
		/// <param name="node">自然対数関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(LogNode node)
		{
			var value = Visit(node.Value);

			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'log({0})'は有効な式ではありません。", value)));
				return null;
			}

			return Math.Log(Convert.ToDouble(value));
		}

		/// <summary>
		/// べき乗関数の評価
		/// </summary>
		/// <param name="node">べき乗関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(PowNode node)
		{
			var x = Visit(node.X);
			var y = Visit(node.Y);

			if (x == null || y == null)
			{
				return null;
			}

			if (x is string || y is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'pow({0}, {1})'は有効な式ではありません。", x, y)));
				return null;
			}

			return Math.Pow(Convert.ToDouble(x), Convert.ToDouble(y));
		}

		/// <summary>
		/// 乱数関数の評価
		/// </summary>
		/// <param name="node">乱数関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(RandNode node)
		{
			Random random = new Random();

            if (node.Value == null)
            {
                return random.NextDouble();
            }

			var value = Visit(node.Value);

			if (value == null)
			{
				return random.NextDouble();
			}

			if (value is string || Convert.ToInt32(value) < 0)
			{
				Errors.Add(node.CreateNewError(string.Format("'rand({0})'は有効な式ではありません。", value)));
				return null;
			}

			return random.Next(Convert.ToInt32(value));
		}

		/// <summary>
		/// 正弦関数の評価
		/// </summary>
		/// <param name="node">正弦関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(SinNode node)
		{
			var value = Visit(node.Value);

			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'sin({0})'は有効な式ではありません。", value)));
				return null;
			}

			return Math.Sin(Convert.ToDouble(value));
		}

		/// <summary>
		/// 平方根関数の評価
		/// </summary>
		/// <param name="node">平方根関数ノード</param>
		/// <returns>演算後の数値(Double)</returns>
		public override object Visit(SqrtNode node)
		{
			var value = Visit(node.Value);

			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				Errors.Add(node.CreateNewError(string.Format("'sqrt({0})'は有効な式ではありません。", value)));
				return null;
			}

			return Math.Sqrt(Convert.ToDouble(value));
		}

		/// <summary>
		/// 変数の評価
		/// </summary>
		/// <param name="node">変数ノード</param>
		/// <returns>変数に対応する値(Double)</returns>
		public override object Visit(VarNode node)
		{
			return Store.GetVar(node.Key);
		}

		/// <summary>
		/// 数値の評価
		/// </summary>
		/// <param name="node">数値ノード</param>
		/// <returns>数値(String)</returns>
		public override object Visit(NumberNode node)
		{
			return double.Parse(node.Value.Text, CultureInfo.InvariantCulture);
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

		/// <summary>
		/// Distance変数の評価
		/// 現在の距離程を返します。
		/// </summary>
		/// <param name="node">距離程変数ノード</param>
		/// <returns>現在の距離程(Double)</returns>
		public override object Visit(DistanceVariableNode node)
		{
			return nowDistance;
		}

		#endregion
	}
}
