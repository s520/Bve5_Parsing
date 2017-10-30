﻿using System;
using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyTest.MapGrammars
{

    public sealed class Vars
    {
        private static Vars _varsInstance = new Vars();
        private Dictionary<string, double> vars;

        public static Vars GetInstance()
        {
            return _varsInstance;
        }

        private Vars()
        {
            vars = new Dictionary<string, double>();
        }

        /// <summary>
        /// 新たな変数を設定する
        /// </summary>
        /// <param name="name">変数名</param>
        /// <param name="val">変数の値</param>
        public void SetVar(string name, double val)
        {
            if (vars.ContainsKey(name))
                vars[name] = val;
            else
                vars.Add(name, val);
        }

        /// <summary>
        /// 変数の値を返す。変数が存在しない場合は0を返す
        /// </summary>
        /// <param name="name">変数名</param>
        /// <returns>変数の値</returns>
        public double GetVar(string name)
        {
            if (vars.ContainsKey(name))
                return vars[name];
            else
                return 0;
        }
    }

    /*
     * 以下AstNodeの定義達
     */

    public class StatementsNode : AstNode
    {
        public List<AstNode> Statements { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            if(Statements == null)
                Statements = new List<AstNode>();

            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            foreach (var node in nodes)
                Statements.Add(AddChild("Statements", node));
        }
    }

    public class StatementNode : AstNode
    {
        public AstNode Statement { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Statement = AddChild("Statement", nodes[0]);
        }
    }

    public class DistNode : AstNode
    {
        public List<AstNode> BasicStates { get; private set; }
        public AstNode Distance { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            if (BasicStates == null)
                BasicStates = new List<AstNode>();

            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Distance = AddChild("Distance", nodes[0]);
            if(nodes.Count > 1)
            {
                foreach (var node in nodes[1].ChildNodes)
                    BasicStates.Add(AddChild("BasicStates", node));
            }
        }
    }

    public class BasicStatesNode : AstNode
    {
        public List<AstNode> BasicState { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            if (BasicState == null)
                BasicState = new List<AstNode>();

            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            foreach (var node in nodes)
                BasicState.Add(AddChild("BasicState", node));
        }
    }

    public class BasicStateNode : AstNode
    {
        public AstNode MapElement { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            MapElement = AddChild("BasicState", nodes[0].ChildNodes[0]);
        }
    }

    public class MapElementNode : AstNode
    {
        public AstNode MapElement { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            MapElement = AddChild("MapElement", nodes[0]);
        }
    }

    public class CurveNode : AstNode
    {
        public AstNode CurveElement { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            CurveElement = AddChild("CurveElement", nodes[0]);
        }
    }

    public class Curve_SetGaugeNode : AstNode
    {
        public AstNode Gauge { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Gauge = AddChild("Gauge", nodes[2]);
        }
    }

    public class VarAssignNode : AstNode
    {
        public AstNode VarName { get; private set; }
        public AstNode VarValue { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();

            //変数名と変数値を取得して変数に登録
            VarNode var = (VarNode)nodes[0].AstNode;
            ExprNode exprNode = (ExprNode)nodes[2].AstNode;
            Vars vars = Vars.GetInstance();
            vars.SetVar(var.Name, exprNode.Value);

            VarName = AddChild("VarName:" + var.Name, nodes[0]);
            VarValue = AddChild("VarValue:" + exprNode.Value, nodes[2]);
        }
    }

    public class ExprNode : AstNode
    {
        public double Value { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            
            //演算を行いValueに代入する
            if(nodes.Count == 3)
            {
                //演算(term + op + expr)
                TermNode term = (TermNode)nodes[0].AstNode;
                double val1 = term.Value;
                string op = nodes[1].Term.ToString();
                ExprNode exprNode = (ExprNode)nodes[2].AstNode;
                double var2 = exprNode.Value;
                Value = exprNode.Calc(val1, exprNode.Value, op);
                AddChild("Expr:", nodes[2]);
            }
            else if (nodes[0].ToString().Equals("Expr"))
            {
                //括弧
                ExprNode exprNode = (ExprNode)nodes[0].AstNode;
                Value = exprNode.Value;
                AddChild("kakko", nodes[0]);
            }
            else
            {
                //Term単体
                TermNode term = (TermNode)nodes[0].AstNode;
                Value = term.Value;
                AddChild("Term:" + Value, nodes[0]);
            }
        }

        public double Calc(double val1, double val2, string op)
        {
            double result = 0;

            switch (op)
            {
                case "+":
                    result = val1 + val2;
                    break;
                case "-":
                    result = val1 - val2;
                    break;
                case "*":
                    result = val1 * val2;
                    break;
                case "/":
                    result = val1 / val2;
                    break;
                case "%":
                    result = val1 % val2;
                    break;
            }

            return result;
        }
    }

    public class TermNode : AstNode
    {
        public double Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();

            //Termの値を取得しValueに設定する
            if (nodes[0].ToString().Equals("Var"))
            {
                //変数
                VarNode varNode = (VarNode)nodes[0].AstNode;
                Vars vars = Vars.GetInstance();
                Value = vars.GetVar(varNode.Name);
                AddChild(varNode.Name + ":" + Value, nodes[0]);
            }
            else
            {
                //定数
                Value = double.Parse(nodes[0].Token.Value.ToString());
            }
        }
    }

    public class VarNode : AstNode
    {
        public string Name { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Name = nodes[0].Token.Value.ToString();
        }
    }
}
