using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using Bve5Parser.MapGrammar.V1.ANTLR_SyntaxDefinitions;

namespace Bve5Parser.MapGrammar.V1.AstNodes
{
	/// <summary>
	/// CSTを巡回してASTを作成するVisitorクラス
	/// </summary>
	internal class BuildAstVisitor : MapV1GrammarParserBaseVisitor<MapGrammarAstNodes>
	{
		/// <summary>
		/// ルートの巡回
		/// ステートメントの集合をノードに追加する
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>ルートASTノード</returns>
		public override MapGrammarAstNodes VisitRoot([NotNull] MapV1GrammarParser.RootContext context)
		{
			var node = new RootNode(context.Start, context.Stop)
			{
				Version = context.version
			};

			foreach (var state in context.statement())
			{
				MapGrammarAstNodes child = base.Visit(state);

				if (child != null)
				{
					node.StatementList.Add(child);
				}
			}

			return node;
		}

		#region ステートメントVisitors

		/// <summary>
		/// 距離程ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>距離程ASTノード</returns>
		public override MapGrammarAstNodes VisitDistState([NotNull] MapV1GrammarParser.DistStateContext context)
		{
			return Visit(context.distance());
		}

		/// <summary>
		/// 平面曲線ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitCurveState([NotNull] MapV1GrammarParser.CurveStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.curve());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 縦曲線ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitGradientState([NotNull] MapV1GrammarParser.GradientStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.gradient());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 他軌道ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitTrackState([NotNull] MapV1GrammarParser.TrackStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.track());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// ストラクチャステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitStructureState([NotNull] MapV1GrammarParser.StructureStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.structure());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 連続ストラクチャステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitRepeaterState([NotNull] MapV1GrammarParser.RepeaterStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.repeater());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 背景ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitBackgroundState([NotNull] MapV1GrammarParser.BackgroundStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.background());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 停車場ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitStationState([NotNull] MapV1GrammarParser.StationStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.station());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 閉そくステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSectionState([NotNull] MapV1GrammarParser.SectionStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.section());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 信号機ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSignalState([NotNull] MapV1GrammarParser.SignalStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.signal());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 地上子ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitBeaconState([NotNull] MapV1GrammarParser.BeaconStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.beacon());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 速度制限ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSpeedLimitState([NotNull] MapV1GrammarParser.SpeedLimitStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.speedLimit());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 先行列車ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitPreTrainState([NotNull] MapV1GrammarParser.PreTrainStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.preTrain());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 光源ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitLightState([NotNull] MapV1GrammarParser.LightStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.light());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 霧効果ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitFogState([NotNull] MapV1GrammarParser.FogStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.fog());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 運転台の明るさステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitCabIlluminanceState([NotNull] MapV1GrammarParser.CabIlluminanceStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.cabIlluminance());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 軌道変位ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitIrregularityState([NotNull] MapV1GrammarParser.IrregularityStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.irregularity());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 粘着特性ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitAdhesionState([NotNull] MapV1GrammarParser.AdhesionStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.adhesion());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 音ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSoundState([NotNull] MapV1GrammarParser.SoundStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.sound());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 固定音源ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSound3dState([NotNull] MapV1GrammarParser.Sound3dStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.sound3d());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 走行音ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitRollingNoiseState([NotNull] MapV1GrammarParser.RollingNoiseStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.rollingNoise());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// フランジきしり音ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitFlangeNoiseState([NotNull] MapV1GrammarParser.FlangeNoiseStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.flangeNoise());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 分岐器通過音ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitJointNoiseState([NotNull] MapV1GrammarParser.JointNoiseStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.jointNoise());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 他列車ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitTrainState([NotNull] MapV1GrammarParser.TrainStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.train());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// レガシー関数ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitLegacyState([NotNull] MapV1GrammarParser.LegacyStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.legacy());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		#endregion

		#region マップ構文Visitors

		/// <summary>
		/// 距離程の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>距離程ASTノード</returns>
		public override MapGrammarAstNodes VisitDistance([NotNull] MapV1GrammarParser.DistanceContext context)
		{
			return new DistanceNode(context.Start, context.Stop)
			{
				Value = context.dist
			};
		}

		/// <summary>
		/// 平面曲線の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitCurve([NotNull] MapV1GrammarParser.CurveContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "curve",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Gauge(value)
				case MapV1GrammarLexer.GAUGE:
					node.Arguments.Add("value", Visit(context.value));
					break;
				// BeginTransition()
				case MapV1GrammarLexer.BEGIN_TRANSITION:
					break;
				// BeginCircular(radius, cant)
				case MapV1GrammarLexer.BEGIN_CIRCULAR:
					node.Arguments.Add("radius", Visit(context.radius));
					node.Arguments.Add("cant", Visit(context.cant));
					break;
				// End()
				case MapV1GrammarLexer.END:
					break;
			}

			return node;
		}

		/// <summary>
		/// 縦曲線の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitGradient([NotNull] MapV1GrammarParser.GradientContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "gradient",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// BeginTransition()
				case MapV1GrammarLexer.BEGIN_TRANSITION:
					break;
				// BeginConst(gradient)
				case MapV1GrammarLexer.BEGIN_CONST:
					node.Arguments.Add("gradient", Visit(context.gradientArgs));
					break;
				// End()
				case MapV1GrammarLexer.END:
					break;
			}

			return node;
		}

		/// <summary>
		/// 他軌道の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitTrack([NotNull] MapV1GrammarParser.TrackContext context)
		{
			// 全て構文タイプ2
			var node = new Syntax2Node(context.Start, context.Stop)
			{
				MapElementName = "track",
				Key = Visit(context.key),
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Position(x, y, radiusH?, radiusV?)
				case MapV1GrammarLexer.POSITION:
					node.Arguments.Add("x", Visit(context.x));
					node.Arguments.Add("y", Visit(context.y));
					if (context.radiusH != null)
					{
						node.Arguments.Add("radiush", Visit(context.radiusH));

						if (context.radiusV != null)
						{
							node.Arguments.Add("radiusv", Visit(context.radiusV));
						}
					}
					break;
				// Cant(cant)
				case MapV1GrammarLexer.CANT:
					node.Arguments.Add("cant", Visit(context.cant));
					break;
				// Gauge(gauge)
				case MapV1GrammarLexer.GAUGE:
					node.Arguments.Add("gauge", Visit(context.gauge));
					break;
			}

			return node;
		}

		/// <summary>
		/// ストラクチャの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitStructure([NotNull] MapV1GrammarParser.StructureContext context)
		{
			// Load(filePath)
			if (context.func.Type == MapV1GrammarLexer.LOAD)
			{
				return new LoadListNode(context.Start, context.Stop)
				{
					MapElementName = "structure",
					Path = Visit(context.path)
				};
			}

			// Load(filePath)以外は構文タイプ2
			var node = new Syntax2Node(context.Start, context.Stop)
			{
				MapElementName = "structure",
				Key = Visit(context.key),
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Put(trackkey, x, y, z, rx, ry, rz, tilt, span)
				case MapV1GrammarLexer.PUT:
					node.Arguments.Add("trackkey", Visit(context.trackKey));
					node.Arguments.Add("x", Visit(context.x));
					node.Arguments.Add("y", Visit(context.y));
					node.Arguments.Add("z", Visit(context.z));
					node.Arguments.Add("rx", Visit(context.rx));
					node.Arguments.Add("ry", Visit(context.ry));
					node.Arguments.Add("rz", Visit(context.rz));
					node.Arguments.Add("tilt", Visit(context.tilt));
					node.Arguments.Add("span", Visit(context.span));
					break;
				// Put0(trackkey, tilt, span)
				case MapV1GrammarLexer.PUT0:
					node.Arguments.Add("trackkey", Visit(context.trackKey));
					node.Arguments.Add("tilt", Visit(context.tilt));
					node.Arguments.Add("span", Visit(context.span));
					break;
				// PutBetween(trackkey1, trackkey2)
				case MapV1GrammarLexer.PUTBETWEEN:
					node.Arguments.Add("trackkey1", Visit(context.trackKey1));
					node.Arguments.Add("trackkey2", Visit(context.trackKey2));
					break;
			}

			return node;
		}

		/// <summary>
		/// 連続ストラクチャの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitRepeater([NotNull] MapV1GrammarParser.RepeaterContext context)
		{
			// 構文タイプ2
			var node = new Syntax2Node(context.Start, context.Stop)
			{
				MapElementName = "repeater",
				Key = Visit(context.key),
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Begin(trackkey, x, y, z, rx, ry, rz, tilt, span, interval, key+)
				case MapV1GrammarLexer.BEGIN:
					node.Arguments.Add("trackkey", Visit(context.trackKey));
					node.Arguments.Add("x", Visit(context.x));
					node.Arguments.Add("y", Visit(context.y));
					node.Arguments.Add("z", Visit(context.z));
					node.Arguments.Add("rx", Visit(context.rx));
					node.Arguments.Add("ry", Visit(context.ry));
					node.Arguments.Add("rz", Visit(context.rz));
					node.Arguments.Add("tilt", Visit(context.tilt));
					node.Arguments.Add("span", Visit(context.span));
					node.Arguments.Add("interval", Visit(context.interval));

					for (int i = 0; i < context.exprArgs().Length; i++)
					{
						node.Arguments.Add("key" + (i + 1), Visit(context.exprArgs()[i]));
					}
					break;
				// Begin(trackkey, tilt, span, interval, key+)
				case MapV1GrammarLexer.BEGIN0:
					node.Arguments.Add("trackkey", Visit(context.trackKey));
					node.Arguments.Add("tilt", Visit(context.tilt));
					node.Arguments.Add("span", Visit(context.span));
					node.Arguments.Add("interval", Visit(context.interval));

					for (int i = 0; i < context.exprArgs().Length; i++)
					{
						node.Arguments.Add("key" + (i + 1), Visit(context.exprArgs()[i]));
					}
					break;
				// End()
				case MapV1GrammarLexer.END:
					break;
			}

			return node;
		}

		/// <summary>
		/// 背景の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitBackground([NotNull] MapV1GrammarParser.BackgroundContext context)
		{
			// 構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "background",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(structurekey)
			if (context.func.Type == MapV1GrammarLexer.CHANGE)
			{
				node.Arguments.Add("structurekey", Visit(context.structureKey));
			}

			return node;
		}

		/// <summary>
		/// 停車場の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitStation([NotNull] MapV1GrammarParser.StationContext context)
		{
			// Load(filePath)
			if (context.func.Type == MapV1GrammarLexer.LOAD)
			{
				return new LoadListNode(context.Start, context.Stop)
				{
					MapElementName = "station",
					Path = Visit(context.path)
				};
			}

			// Load(filePath)以外は構文タイプ2
			var node = new Syntax2Node(context.Start, context.Stop)
			{
				MapElementName = "station",
				Key = Visit(context.key),
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Put(door, margin1, margin2)
			if (context.func.Type == MapV1GrammarLexer.PUT)
			{
				node.Arguments.Add("door", Visit(context.door));
				node.Arguments.Add("margin1", Visit(context.margin1));
				node.Arguments.Add("margin2", Visit(context.margin2));
			}

			return node;
		}

		/// <summary>
		/// 閉そくの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSection([NotNull] MapV1GrammarParser.SectionContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "section",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// BeginNew(signalN+)
			if (context.func.Type == MapV1GrammarLexer.BEGIN_NEW)
			{
				node.Arguments.Add("signal0", Visit(context.nullableExpr()));

				for (int i = 0; i < context.exprArgs().Length; i++)
				{
					node.Arguments.Add("signal" + (i + 1), Visit(context.exprArgs()[i]));
				}
			}

			return node;
		}

		/// <summary>
		/// 信号機の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSignal([NotNull] MapV1GrammarParser.SignalContext context)
		{
			string funcName = context.func.Text.ToLowerInvariant();

			// Load(filePath)
			if (context.func.Type == MapV1GrammarLexer.LOAD)
			{
				return new LoadListNode(context.Start, context.Stop)
				{
					MapElementName = "signal",
					Path = Visit(context.path)
				};
			}

			// Signal.SpeedLimit(vN+)
			if (context.func.Type == MapV1GrammarLexer.SPEEDLIMIT)
			{
				// 構文タイプ1
				var node = new Syntax1Node(context.Start, context.Stop)
				{
					MapElementName = "signal",
					FunctionName = funcName
				};

				// 引数の登録
				node.Arguments.Add("v0", Visit(context.nullableExpr()[0]));

				for (int i = 0; i < context.exprArgs().Length; i++)
				{
					node.Arguments.Add("v" + (i + 1), Visit(context.exprArgs()[i]));
				}

				return node;
			}

			// Put(section, trackkey, x, y, z?, rx?, ry?, rz?, tilt?, span?)
			if (context.func.Type == MapV1GrammarLexer.PUT)
			{
				// 構文タイプ2
				var node = new Syntax2Node(context.Start, context.Stop)
				{
					MapElementName = "signal",
					Key = Visit(context.key),
					FunctionName = funcName
				};

				// 引数の登録
				node.Arguments.Add("section", Visit(context.sectionArgs));
				node.Arguments.Add("trackkey", Visit(context.trackKey));
				node.Arguments.Add("x", Visit(context.x));
				node.Arguments.Add("y", Visit(context.y));

				if (context.z != null)
				{
					node.Arguments.Add("z", Visit(context.z));
					node.Arguments.Add("rx", Visit(context.rx));
					node.Arguments.Add("ry", Visit(context.ry));
					node.Arguments.Add("rz", Visit(context.rz));
					node.Arguments.Add("tilt", Visit(context.tilt));
					node.Arguments.Add("span", Visit(context.span));
				}

				return node;
			}

			return null;
		}

		/// <summary>
		/// 地上子の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitBeacon([NotNull] MapV1GrammarParser.BeaconContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "beacon",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Put(type, section, senddata)
			if (context.func.Type == MapV1GrammarLexer.PUT)
			{
				node.Arguments.Add("type", Visit(context.type));
				node.Arguments.Add("section", Visit(context.sectionArgs));
				node.Arguments.Add("senddata", Visit(context.sendData));
			}

			return node;
		}

		/// <summary>
		/// 速度制限の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSpeedLimit([NotNull] MapV1GrammarParser.SpeedLimitContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "speedlimit",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Begin(v)
				case MapV1GrammarLexer.BEGIN:
					node.Arguments.Add("v", Visit(context.v));
					break;
				// End()
				case MapV1GrammarLexer.END:
					break;
			}

			return node;
		}

		/// <summary>
		/// 先行列車の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitPreTrain([NotNull] MapV1GrammarParser.PreTrainContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "pretrain",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Pass(time)
			if (context.func.Type == MapV1GrammarLexer.PASS)
			{
				node.Arguments.Add("time", Visit(context.nullableExpr()));
			}

			return node;
		}

		/// <summary>
		/// 光源の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitLight([NotNull] MapV1GrammarParser.LightContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "light",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Ambient(red, green, blue)
				case MapV1GrammarLexer.AMBIENT:
					node.Arguments.Add("red", Visit(context.red));
					node.Arguments.Add("green", Visit(context.green));
					node.Arguments.Add("blue", Visit(context.blue));
					break;
				// Diffuse(red, green, blue)
				case MapV1GrammarLexer.DIFFUSE:
					node.Arguments.Add("red", Visit(context.red));
					node.Arguments.Add("green", Visit(context.green));
					node.Arguments.Add("blue", Visit(context.blue));
					break;
				// Direction(pitch, yaw)
				case MapV1GrammarLexer.DIRECTION:
					node.Arguments.Add("pitch", Visit(context.pitch));
					node.Arguments.Add("yaw", Visit(context.yaw));
					break;
			}

			return node;
		}

		/// <summary>
		/// 霧効果の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitFog([NotNull] MapV1GrammarParser.FogContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "fog",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Set(density, red, green, blue)
			if (context.func.Type == MapV1GrammarLexer.SET)
			{
				node.Arguments.Add("density", Visit(context.density));
				node.Arguments.Add("red", Visit(context.red));
				node.Arguments.Add("green", Visit(context.green));
				node.Arguments.Add("blue", Visit(context.blue));
			}

			return node;
		}

		/// <summary>
		/// 運転台の明るさの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitCabIlluminance([NotNull] MapV1GrammarParser.CabIlluminanceContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "cabilluminance",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Set(value)
			if (context.func.Type == MapV1GrammarLexer.SET)
			{
				node.Arguments.Add("value", Visit(context.value));
			}

			return node;
		}

		/// <summary>
		/// 軌道変位の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitIrregularity([NotNull] MapV1GrammarParser.IrregularityContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "irregularity",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(x, y, r, lx, ly, lr)
			if (context.func.Type == MapV1GrammarLexer.CHANGE)
			{
				node.Arguments.Add("x", Visit(context.x));
				node.Arguments.Add("y", Visit(context.y));
				node.Arguments.Add("r", Visit(context.r));
				node.Arguments.Add("lx", Visit(context.lx));
				node.Arguments.Add("ly", Visit(context.ly));
				node.Arguments.Add("lr", Visit(context.lr));
			}

			return node;
		}

		/// <summary>
		/// 粘着特性の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitAdhesion([NotNull] MapV1GrammarParser.AdhesionContext context)
		{
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "adhesion",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(a, b?, c?)
			if (context.func.Type == MapV1GrammarLexer.CHANGE)
			{
				node.Arguments.Add("a", Visit(context.a));

				if (context.b != null)
				{
					node.Arguments.Add("b", Visit(context.b));
					node.Arguments.Add("c", Visit(context.c));
				}
			}

			return node;
		}

		/// <summary>
		/// 音の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSound([NotNull] MapV1GrammarParser.SoundContext context)
		{
			// Load(filePath)
			if (context.func.Type == MapV1GrammarLexer.LOAD)
			{
				return new LoadListNode(context.Start, context.Stop)
				{
					MapElementName = "sound",
					Path = Visit(context.path)
				};
			}

			// Load(filePath)以外は構文タイプ2
			// Play()
			var node = new Syntax2Node(context.Start, context.Stop)
			{
				MapElementName = "sound",
				Key = Visit(context.key),
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			return node;
		}

		/// <summary>
		/// 固定音源の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSound3d([NotNull] MapV1GrammarParser.Sound3dContext context)
		{
			// Load(filePath)
			if (context.func.Type == MapV1GrammarLexer.LOAD)
			{
				return new LoadListNode(context.Start, context.Stop)
				{
					MapElementName = "sound3d",
					Path = Visit(context.path)
				};
			}
			// Load(filePath)以外は構文タイプ2
			var node = new Syntax2Node(context.Start, context.Stop)
			{
				MapElementName = "sound3d",
				Key = Visit(context.key),
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Put(x, y)
			if (context.func.Type == MapV1GrammarLexer.PUT)
			{
				node.Arguments.Add("x", Visit(context.x));
				node.Arguments.Add("y", Visit(context.y));
			}

			return node;
		}

		/// <summary>
		/// 走行音の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitRollingNoise([NotNull] MapV1GrammarParser.RollingNoiseContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "rollingnoise",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(index)
			if (context.func.Type == MapV1GrammarLexer.CHANGE)
			{
				node.Arguments.Add("index", Visit(context.index));
			}

			return node;
		}

		/// <summary>
		/// フランジきしり音の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitFlangeNoise([NotNull] MapV1GrammarParser.FlangeNoiseContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "flangenoise",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(index)
			if (context.func.Type == MapV1GrammarLexer.CHANGE)
			{
				node.Arguments.Add("index", Visit(context.index));
			}

			return node;
		}

		/// <summary>
		/// 分岐器通過音の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitJointNoise([NotNull] MapV1GrammarParser.JointNoiseContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "jointnoise",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Play(index)
			if (context.func.Type == MapV1GrammarLexer.PLAY)
			{
				node.Arguments.Add("index", Visit(context.index));
			}

			return node;
		}

		/// <summary>
		/// 他列車の巡回
		/// </summary>
		/// <param name="context"></param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitTrain([NotNull] MapV1GrammarParser.TrainContext context)
		{
			// Add(trainkey, filePath, trackkey?, direction?)
			if (context.func.Type == MapV1GrammarLexer.ADD)
			{
				// 構文タイプ1
				var node = new Syntax1Node(context.Start, context.Stop)
				{
					MapElementName = "train",
					FunctionName = context.func.Text.ToLowerInvariant()
				};

				// 引数の登録
				node.Arguments.Add("trainkey", Visit(context.trainKey));
				node.Arguments.Add("filepath", Visit(context.path));

				if (context.trackKey != null)
				{
					node.Arguments.Add("trackkey", Visit(context.trackKey));
					node.Arguments.Add("direction", Visit(context.direction));
				}

				return node;
			}
			else
			{
				// 構文タイプ2
				var node = new Syntax2Node(context.Start, context.Stop)
				{
					MapElementName = "train",
					Key = Visit(context.key),
					FunctionName = context.func.Text.ToLowerInvariant()
				};

				// 引数の登録
				switch (context.func.Type)
				{
					// SetTrack(trackkey, direction)
					case MapV1GrammarLexer.SET_TRACK:
						node.Arguments.Add("trackkey", Visit(context.trackKey));
						node.Arguments.Add("direction", Visit(context.direction));
						break;
					// Enable(time)
					case MapV1GrammarLexer.ENABLE:
						node.Arguments.Add("time", Visit(context.time));
						break;
					// Stop(decelerate, stopTime, accelerate, speed)
					case MapV1GrammarLexer.STOP:
						node.Arguments.Add("decelerate", Visit(context.decelerate));
						node.Arguments.Add("stoptime", Visit(context.stopTime));
						node.Arguments.Add("accelerate", Visit(context.accelerate));
						node.Arguments.Add("speed", Visit(context.speed));
						break;
				}

				return node;
			}
		}

		/// <summary>
		/// レガシー関数の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitLegacy([NotNull] MapV1GrammarParser.LegacyContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "legacy",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Fog(start, end, red, green, blue)
				case MapV1GrammarLexer.FOG:
					node.Arguments.Add("start", Visit(context.startArg));
					node.Arguments.Add("end", Visit(context.endArg));
					node.Arguments.Add("red", Visit(context.red));
					node.Arguments.Add("green", Visit(context.green));
					node.Arguments.Add("blue", Visit(context.blue));
					break;
				// Curve(radius, cant)
				case MapV1GrammarLexer.CURVE:
					node.Arguments.Add("radius", Visit(context.radius));
					node.Arguments.Add("cant", Visit(context.cant));
					break;
				// Pitch(rate)
				case MapV1GrammarLexer.PITCH:
					node.Arguments.Add("rate", Visit(context.rate));
					break;
				// Turn(slope)
				case MapV1GrammarLexer.TURN:
					node.Arguments.Add("slope", Visit(context.slope));
					break;
			}

			return node;
		}

		#endregion

		#region 数式と変数Visitors

		/// <summary>
		/// 数式の連続引数Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitExprArgs([NotNull] MapV1GrammarParser.ExprArgsContext context)
		{
			return Visit(context.nullableExpr());
		}

		/// <summary>
		/// null許容数式Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitNullableExpr([NotNull] MapV1GrammarParser.NullableExprContext context)
		{
			// null
			if (context.ChildCount == 0 || context.nullSyntax != null)
			{
				//return new NumberNode { Value = 0 };
				return null;
			}

			return Visit(context.expr());
		}

		/// <summary>
		/// 文字列Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitStringExpr([NotNull] MapV1GrammarParser.StringExprContext context)
		{
			return new StringNode(context.Start, context.Stop) { Value = context.str };
		}

		/// <summary>
		/// 文字列Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitString([NotNull] MapV1GrammarParser.StringContext context)
		{
			return new StringNode(context.Start, context.Stop) { Value = context };
		}

		#endregion
	}
}
