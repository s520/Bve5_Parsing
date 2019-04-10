﻿using Antlr4.Runtime;
using Bve5_Parsing.Attributes;
using System.Linq;
using System.Collections.Generic;
using static Bve5_Parsing.MapGrammar.SyntaxDefinitions.MapGrammarParser;
using System.Reflection;
using System;
using Antlr4.Runtime.Tree;
using System.Collections.ObjectModel;

namespace Bve5_Parsing.MapGrammar.AstNodes
{
    /**
     * AstNodeの定義
     */

    /// <summary>
    /// AST親クラス
    /// </summary>
	public abstract class MapGrammarAstNodes
    {
        public IToken Start { get; set; }
        public IToken Stop { get; set; }

        public MapGrammarAstNodes(IToken start, IToken stop)
        {
            Start = start;
            Stop = stop;
        }

        public ParseError CreateNewWarning(string msg)
        {
            return new ParseError(ParseErrorLevel.Warning, Start.Line, Start.Column, msg);
        }

        public ParseError CreateNewError(string msg)
        {
            return new ParseError(ParseErrorLevel.Error, Start.Line, Start.Column, msg);
        }
    }

    /// <summary>
    /// ルートノード
    /// </summary>
    public class RootNode : MapGrammarAstNodes
    {

        public List<MapGrammarAstNodes> StatementList { get; set; }
        public IToken Version { get; set; }
        public EncodingContext Encoding { get; set; }
        public RootNode(IToken start, IToken stop): base(start, stop)
        {
            StatementList = new List<MapGrammarAstNodes>();
        }
    }

    /// <summary>
    /// 距離程ノード
    /// </summary>
    public class DistanceNode : MapGrammarAstNodes
    {
        public MapGrammarAstNodes Value { get; set; }

        public DistanceNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// Includeディレクティブノード
    /// </summary>
    public class IncludeNode : MapGrammarAstNodes
    {
        public MapGrammarAstNodes FilePath { get; set; }

        public IncludeNode(IToken start, IToken stop) : base(start, stop) { }
    }

    #region ステートメント AST Nodes

    /// <summary>
    /// 構文ASTノードのベースとなるノード
    /// </summary>
    public abstract class SyntaxNode : MapGrammarAstNodes
    {
        public abstract MapElementName ElementName { get; }

        public abstract MapFunctionName FunctionName { get; }

        /// <summary>
        /// キーを指定する構文か？
        /// </summary>
        public bool HasKey => GetType().GetProperty("Key") != null;

        public SyntaxNode(IToken start, IToken stop) : base(start, stop) { }

        /// <summary>
        /// アトリビュートからASTノードの引数を取得します。
        /// </summary>
        /// <returns></returns>
        protected internal IEnumerable<PropertyInfo> GetAllArguments()
        {
            return GetType().GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attr = p.GetCustomAttributes(typeof(ArgumentAttribute), true)
                        .Select(a => a as ArgumentAttribute)
                        .Where(a => a != null)
                })
                .Where(p => p.Attr.Any())
                .Select(p => p.Property)
                ;
        }

        /// <summary>
        /// アトリビュートからASTノードの省略不可能な引数を取得します。
        /// </summary>
        /// <returns></returns>
        protected internal IEnumerable<PropertyInfo> GetNonOptionalArguments()
        {
            return GetType().GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attr = p.GetCustomAttributes(typeof(ArgumentAttribute), true)
                        .Select(a => a as ArgumentAttribute)
                        .Where(a => a != null)
                })
                .Where(p => p.Attr.Any() && false == p.Attr.Single().Optional)
                .Select(p => p.Property)
                ;
        }

        /// <summary>
        /// アトリビュートからASTノードの省略可能引数を取得します。
        /// </summary>
        /// <returns></returns>
        protected internal IEnumerable<PropertyInfo> GetOptionalArguments()
        {
            return GetType().GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attr = p.GetCustomAttributes(typeof(ArgumentAttribute), true)
                        .Select(a => a as ArgumentAttribute)
                        .Where(a => a != null)
                })
                .Where(p => p.Attr.Any() && p.Attr.Single().Optional)
                .Select(p => p.Property)
                ;
        }

        /// <summary>
        /// メタ情報から対応するAstを生成して返します。
        /// MapGrammarParserの定義ファイル更新時に使えなくなる可能性が高いため、更新時は注意して下さい。
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="ctx"></param>
        /// <param name="elementName"></param>
        /// <param name="funcName"></param>
        /// <param name="element2Name"></param>
        /// <returns></returns>
        protected internal static SyntaxNode CreateSyntaxAstNode(BuildAstVisitor visitor, ParserRuleContext ctx, MapElementName elementName, string funcName, string element2Name = null)
        {
            // 関数名は必ず2文字以上あるので
            if (funcName.Length < 2)
                throw new ArgumentOutOfRangeException("関数名が短すぎます。");

            // ASTのインスタンス取得
            var astClassName = elementName.GetStringValue() + char.ToUpper(funcName[0]) + funcName.Substring(1).ToLower() + "Node";
            
            if (element2Name != null)
            {
                var ele2 = element2Name.Length <= 1 ? element2Name.ToUpper() : char.ToUpper(element2Name[0]) + element2Name.Substring(1).ToLower();
                astClassName = elementName.GetStringValue() + ele2 + char.ToUpper(funcName[0]) + funcName.Substring(1).ToLower() + "Node";
            }
            var astClassType = Type.GetType(typeof(MapGrammarAstNodes).Namespace + "." + astClassName);

            if (astClassType == null)
                throw new NotSupportedException("対応するAstNodeの取得に失敗しました。");

            var instanceArg = new object[] { ctx.Start, ctx.Stop };
            var node = Activator.CreateInstance(astClassType, instanceArg) as SyntaxNode;

            // キー
            if (node.HasKey)
            {
                var key = ctx.GetType().GetField("key").GetValue(ctx) as IParseTree;
                node.GetType().GetProperty("Key").SetValue(node, visitor.Visit(key), null);
            }

            // 引数の取得
            foreach (var arg in node.GetNonOptionalArguments())
            {
                var argCtxInfo = ctx.GetType().GetField(arg.Name.ToLower());
                var argCtx = argCtxInfo?.GetValue(ctx);
                if (argCtx != null)
                {
                    arg.SetValue(node, visitor.Visit(argCtx as IParseTree), null);
                    continue;
                }

                // Gradientに関しては特別対応
                // Graident -> GradientArgs
                var argGCtxInfo = ctx.GetType().GetField(arg.Name.ToLower() + "Args");
                var argGCtx = argGCtxInfo?.GetValue(ctx);
                if (argGCtx != null)
                {
                    arg.SetValue(node, visitor.Visit(argGCtx as IParseTree), null);
                    continue;
                }

                throw new NotSupportedException($"引数：{arg.Name}に対応するContextの取得に失敗しました。");
            }

            // 省略可能引数の取得
            foreach(var arg in node.GetOptionalArguments())
            {
                var argCtxInfo = ctx.GetType().GetField(arg.Name.ToLower());
                var argCtx = argCtxInfo?.GetValue(ctx);
                if (argCtx != null)
                    arg.SetValue(node, visitor.Visit(argCtx as IParseTree), null);
                else
                {
                    // "引数名" + Eのコンテキスト名に対応させる
                    var argECtxInfo = ctx.GetType().GetField(arg.Name.ToLower() + "E");
                    var argECtx = argECtxInfo?.GetValue(ctx);
                    if (argECtx != null)
                        arg.SetValue(node, visitor.Visit(argECtx as IParseTree), null);

                    // Graident -> GradientArgsE
                    var argGECtxInfo = ctx.GetType().GetField(arg.Name.ToLower() + "ArgsE");
                    var argGECtx = argGECtxInfo?.GetValue(ctx);
                    if (argGECtx != null)
                        arg.SetValue(node, visitor.Visit(argGECtx as IParseTree), null);
                }
            }

            return node;
        }

        /// <summary>
        /// ASTノードのメタ情報からSyntaxDataを生成して返します。
        /// </summary>
        /// <param name="evaluator"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public SyntaxData CreateSyntaxData(EvaluateMapGrammarVisitor evaluator, double distance)
        {
            var funcName = FunctionName.GetStringValue();
            var syntax = new SyntaxData();
            syntax.Distance = distance;

            // TODO: 構文タイプの判定を属性でやりたい
            if (funcName.Contains("."))
            {
                //Syntax3
                syntax.MapElement = new string[2]
                {
                    ElementName.GetStringValue().ToLower(),
                    funcName.Substring(0, funcName.IndexOf(".")).ToLower()
                };
            }else
            {
                syntax.MapElement = new string[1]
                {
                    ElementName.GetStringValue().ToLower()
                };
            }

            if (HasKey)
            {
                var key = GetType().GetProperty("Key").GetValue(this, null) as MapGrammarAstNodes;
                syntax.Key = evaluator.Visit(key).ToString();
            }
                
            syntax.Function = funcName.Substring(funcName.IndexOf(".") + 1).ToLower();
            foreach(var argument in GetAllArguments())
            {
                var val = argument.GetValue(this, null) as MapGrammarAstNodes;
                if (val == null)
                    continue;

                syntax.SetArg(argument.Name.ToLower(), evaluator.Visit(val));
            }

            return syntax;
        }
    }

    /// <summary>
    /// Repeater.Beginの手動対応
    /// 可変長のstructureKeyへ対応する
    /// </summary>
    public partial class RepeaterBeginNode
    {
        protected List<MapGrammarAstNodes> _structureKeys = new List<MapGrammarAstNodes>();

        public ReadOnlyCollection<MapGrammarAstNodes> StructureKeys => _structureKeys.AsReadOnly();

        public void AddStructureKey(MapGrammarAstNodes strKey)
        {
            _structureKeys.Add(strKey);
        }
    }

    /// <summary>
    /// Repeater.Begin0の手動対応
    /// 可変長のstructureKeyへ対応する
    /// </summary>
    public partial class RepeaterBegin0Node
    {
        protected List<MapGrammarAstNodes> _structureKeys = new List<MapGrammarAstNodes>();

        public ReadOnlyCollection<MapGrammarAstNodes> StructureKeys => _structureKeys.AsReadOnly();

        public void AddStructureKey(MapGrammarAstNodes strKey)
        {
            _structureKeys.Add(strKey);
        }
    }

    /// <summary>
    /// Section.Beginの手動対応
    /// 可変長のsignalIndexへ対応する
    /// </summary>
    public partial class SectionBeginNode
    {
        protected List<MapGrammarAstNodes> _signalIndexes = new List<MapGrammarAstNodes>();

        public ReadOnlyCollection<MapGrammarAstNodes> SignalIndexes => _signalIndexes.AsReadOnly();

        public void AddSignalIndex(MapGrammarAstNodes sigIdx)
        {
            _signalIndexes.Add(sigIdx);
        }
    }

    /// <summary>
    /// Section.Setspeedlimitの手動対応
    /// 可変長の速度制限へ対応する
    /// </summary>
    public partial class SectionSetspeedlimitNode
    {
        protected List<MapGrammarAstNodes> _speedLimits = new List<MapGrammarAstNodes>();

        public ReadOnlyCollection<MapGrammarAstNodes> SpeedLimits => _speedLimits.AsReadOnly();

        // TODO: NULLの可能性もある
        public void AddSpeedLimit(MapGrammarAstNodes spdLmt)
        {
            _speedLimits.Add(spdLmt);
        }
    }

    /// <summary>
    /// ステートメントノード1 MapElement.Function(Args)
    /// </summary>
    public class Syntax1Node : MapGrammarAstNodes
    {
        public string MapElementName { get; set; }
        public string FunctionName { get; set; }
        public Dictionary<string, MapGrammarAstNodes> Arguments { get; set; }

        public Syntax1Node(IToken start, IToken stop): base(start, stop)
        {
            Arguments = new Dictionary<string, MapGrammarAstNodes>();
        }
    }

    /// <summary>
    /// ステートメントノード2 MapElement[key].Function(Args)
    /// </summary>
    public class Syntax2Node : MapGrammarAstNodes
    {
        public string MapElementName { get; set; }
        public MapGrammarAstNodes Key { get; set; }
        public string FunctionName { get; set; }
        public Dictionary<string, MapGrammarAstNodes> Arguments { get; set; }

        public Syntax2Node(IToken start, IToken stop): base(start, stop)
        {
            Arguments = new Dictionary<string, MapGrammarAstNodes>();
        }
    }

    /// <summary>
    /// ステートメントノード3 MapElement[key].MapElement.Function(Args)
    /// </summary>
    public class Syntax3Node : MapGrammarAstNodes
    {
        // TODO: タプルの方がいい？
        public string[] MapElementNames { get; set; }
        public MapGrammarAstNodes Key { get; set; }
        public string FunctionName { get; set; }
        public Dictionary<string, MapGrammarAstNodes> Arguments { get; set; }

        public Syntax3Node(IToken start, IToken stop): base(start, stop)
        {
            Arguments = new Dictionary<string, MapGrammarAstNodes>();
            MapElementNames = new string[2];
        }
    }

    /// <summary>
    /// リストファイルロードノード
    /// </summary>
    public class LoadListNode : MapGrammarAstNodes
    {
        public string MapElementName { get; set; }
        public StringContext Path { get; set; }

        public LoadListNode(IToken start, IToken stop): base(start, stop) { }
    }

    #endregion ステートメント AST Nodes


    #region 変数宣言 AST Nodes

    /// <summary>
    /// 変数宣言ノード
    /// </summary>
    public class VarAssignNode : MapGrammarAstNodes
    {
        public string VarName { get; set; }
        public MapGrammarAstNodes Value { get; set; }

        public VarAssignNode(IToken start, IToken stop): base(start, stop) { }
    }
    #endregion 変数宣言 AST Nodes

    #region 数式 AST Nodes

    /// <summary>
    /// 二項演算数式ノード
    /// </summary>
    public abstract class InfixExpressionNode : MapGrammarAstNodes
    {
        public MapGrammarAstNodes Left { get; set; }
        public MapGrammarAstNodes Right { get; set; }

        public InfixExpressionNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// 加算ノード
    /// </summary>
    public class AdditionNode : InfixExpressionNode
    {
        public AdditionNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// 減算ノード
    /// </summary>
    public class SubtractionNode : InfixExpressionNode
    {
        public SubtractionNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// 乗算ノード
    /// </summary>
    public class MultiplicationNode : InfixExpressionNode
    {
        public MultiplicationNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// 除算ノード
    /// </summary>
    public class DivisionNode : InfixExpressionNode
    {
        public DivisionNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// 剰余算ノード
    /// </summary>
    public class ModuloNode : InfixExpressionNode
    {
        public ModuloNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// ユーナリ演算ノード
    /// </summary>
    public class UnaryNode : MapGrammarAstNodes
    {
        public MapGrammarAstNodes InnerNode { get; set; }

        public UnaryNode(IToken start, IToken stop): base(start, stop) { }
    }

    #region 数学関数ノード

    /// <summary>
    /// 数学関数親ノード
    /// </summary>
    public abstract class SingleArgFunctionNode : MapGrammarAstNodes
    {
        public MapGrammarAstNodes Value { get; set; }

        public SingleArgFunctionNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// ABSノード
    /// </summary>
    public class AbsNode : SingleArgFunctionNode
    {
        public AbsNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// ATAN2ノード
    /// </summary>
    public class Atan2Node : MapGrammarAstNodes
    {
        public MapGrammarAstNodes Y { get; set; }
        public MapGrammarAstNodes X { get; set; }

        public Atan2Node(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// CEILノード
    /// </summary>
    public class CeilNode : SingleArgFunctionNode
    {
        public CeilNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// COSノード
    /// </summary>
    public class CosNode : SingleArgFunctionNode
    {
        public CosNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// EXPノード
    /// </summary>
    public class ExpNode : SingleArgFunctionNode
    {
        public ExpNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// FLOORノード
    /// </summary>
    public class FloorNode : SingleArgFunctionNode
    {
        public FloorNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// LOGノード
    /// </summary>
    public class LogNode : SingleArgFunctionNode
    {
        public LogNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// POWノード
    /// </summary>
    public class PowNode : MapGrammarAstNodes
    {
        public MapGrammarAstNodes X { get; set; }
        public MapGrammarAstNodes Y { get; set; }

        public PowNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// RANDノード
    /// </summary>
    public class RandNode : SingleArgFunctionNode
    {
        public RandNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// SINノード
    /// </summary>
    public class SinNode : SingleArgFunctionNode
    {
        public SinNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// SQRTノード
    /// </summary>
    public class SqrtNode : SingleArgFunctionNode
    {
        public SqrtNode(IToken start, IToken stop): base(start, stop) { }
    }

    #endregion 数学関数ノード

    /// <summary>
    /// 数値ノード
    /// </summary>
    public class NumberNode : MapGrammarAstNodes
    {
        public IToken Value { get; set; }

        public NumberNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// 距離変数ノード
    /// </summary>
    public class DistanceVariableNode : MapGrammarAstNodes
    {
        public DistanceVariableNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// 文字列ノード
    /// </summary>
    public class StringNode : MapGrammarAstNodes
    {
        public StringContext Value { get; set; }

        public StringNode(IToken start, IToken stop): base(start, stop) { }
    }

    /// <summary>
    /// 変数ノード
    /// </summary>
    public class VarNode : MapGrammarAstNodes
    {
        public string Key { get; set; }

        public VarNode(IToken start, IToken stop) : base(start, stop) { }

    }
    #endregion 数式 AST Nodes
}