using System;
using Antlr4.Runtime.Misc;
using Bve5Parser.MapGrammar.V2.ANTLR_SyntaxDefinitions;

namespace Bve5Parser.MapGrammar.V2.AstNodes
{
	/// <summary>
	/// CSTを巡回してASTを作成するVisitorクラス
	/// </summary>
	internal class BuildAstVisitor : MapV2GrammarParserBaseVisitor<MapGrammarAstNodes>
	{
		/// <summary>
		/// ルートの巡回
		/// ステートメントの集合をノードに追加する
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>ルートASTノード</returns>
		public override MapGrammarAstNodes VisitRoot([NotNull] MapV2GrammarParser.RootContext context)
		{
			var node = new RootNode(context.Start, context.Stop)
			{
				Version = context.version,
				Encoding = context.encoding()
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
		public override MapGrammarAstNodes VisitDistState([NotNull] MapV2GrammarParser.DistStateContext context)
		{
			return Visit(context.distance());
		}

		/// <summary>
		/// インクルードステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitIncludeState([NotNull] MapV2GrammarParser.IncludeStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.include());
			}
			catch (NullReferenceException)
			{
				node = null;
			}

			return node;
		}

		/// <summary>
		/// 平面曲線ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitCurveState([NotNull] MapV2GrammarParser.CurveStateContext context)
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
		public override MapGrammarAstNodes VisitGradientState([NotNull] MapV2GrammarParser.GradientStateContext context)
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
		public override MapGrammarAstNodes VisitTrackState([NotNull] MapV2GrammarParser.TrackStateContext context)
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
		public override MapGrammarAstNodes VisitStructureState([NotNull] MapV2GrammarParser.StructureStateContext context)
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
		public override MapGrammarAstNodes VisitRepeaterState([NotNull] MapV2GrammarParser.RepeaterStateContext context)
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
		public override MapGrammarAstNodes VisitBackgroundState([NotNull] MapV2GrammarParser.BackgroundStateContext context)
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
		public override MapGrammarAstNodes VisitStationState([NotNull] MapV2GrammarParser.StationStateContext context)
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
		public override MapGrammarAstNodes VisitSectionState([NotNull] MapV2GrammarParser.SectionStateContext context)
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
		public override MapGrammarAstNodes VisitSignalState([NotNull] MapV2GrammarParser.SignalStateContext context)
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
		public override MapGrammarAstNodes VisitBeaconState([NotNull] MapV2GrammarParser.BeaconStateContext context)
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
		public override MapGrammarAstNodes VisitSpeedLimitState([NotNull] MapV2GrammarParser.SpeedLimitStateContext context)
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
		public override MapGrammarAstNodes VisitPreTrainState([NotNull] MapV2GrammarParser.PreTrainStateContext context)
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
		public override MapGrammarAstNodes VisitLightState([NotNull] MapV2GrammarParser.LightStateContext context)
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
		public override MapGrammarAstNodes VisitFogState([NotNull] MapV2GrammarParser.FogStateContext context)
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
		/// 風景描画距離ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitDrawDistanceState([NotNull] MapV2GrammarParser.DrawDistanceStateContext context)
		{
			MapGrammarAstNodes node;

			try
			{
				node = Visit(context.drawDistance());
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
		public override MapGrammarAstNodes VisitCabIlluminanceState([NotNull] MapV2GrammarParser.CabIlluminanceStateContext context)
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
		public override MapGrammarAstNodes VisitIrregularityState([NotNull] MapV2GrammarParser.IrregularityStateContext context)
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
		public override MapGrammarAstNodes VisitAdhesionState([NotNull] MapV2GrammarParser.AdhesionStateContext context)
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
		public override MapGrammarAstNodes VisitSoundState([NotNull] MapV2GrammarParser.SoundStateContext context)
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
		public override MapGrammarAstNodes VisitSound3dState([NotNull] MapV2GrammarParser.Sound3dStateContext context)
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
		public override MapGrammarAstNodes VisitRollingNoiseState([NotNull] MapV2GrammarParser.RollingNoiseStateContext context)
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
		public override MapGrammarAstNodes VisitFlangeNoiseState([NotNull] MapV2GrammarParser.FlangeNoiseStateContext context)
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
		public override MapGrammarAstNodes VisitJointNoiseState([NotNull] MapV2GrammarParser.JointNoiseStateContext context)
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
		public override MapGrammarAstNodes VisitTrainState([NotNull] MapV2GrammarParser.TrainStateContext context)
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
		public override MapGrammarAstNodes VisitLegacyState([NotNull] MapV2GrammarParser.LegacyStateContext context)
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

		/// <summary>
		/// 変数宣言ステートメントの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>変数宣言ASTノード</returns>
		public override MapGrammarAstNodes VisitVarAssignState([NotNull] MapV2GrammarParser.VarAssignStateContext context)
		{
			return base.Visit(context.varAssign());
		}

		#endregion

		#region マップ構文Visitors

		/// <summary>
		/// 距離程の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>距離程ASTノード</returns>
		public override MapGrammarAstNodes VisitDistance([NotNull] MapV2GrammarParser.DistanceContext context)
		{
			return new DistanceNode(context.Start, context.Stop)
			{
				Value = Visit(context.expr())
			};
		}

		/// <summary>
		/// インクルードの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitInclude([NotNull] MapV2GrammarParser.IncludeContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "include",
				FunctionName = ""
			};

			// 引数の登録
			node.Arguments.Add("path", Visit(context.path));

			return node;
		}

		/// <summary>
		/// 平面曲線の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitCurve([NotNull] MapV2GrammarParser.CurveContext context)
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
				// SetGauge(value)
				// Gauge(value)
				case MapV2GrammarLexer.SET_GAUGE:
				case MapV2GrammarLexer.GAUGE:
					node.Arguments.Add("value", Visit(context.value));
					break;
				// SetCenter(x)
				case MapV2GrammarLexer.SET_CENTER:
					node.Arguments.Add("x", Visit(context.x));
					break;
				// SetFunction(id)
				case MapV2GrammarLexer.SET_FUNCTION:
					node.Arguments.Add("id", Visit(context.id));
					break;
				// BeginTransition()
				case MapV2GrammarLexer.BEGIN_TRANSITION:
					break;
				// Begin(radius, cant?)
				// BeginCircular(radius, cant?)
				case MapV2GrammarLexer.BEGIN:
				case MapV2GrammarLexer.BEGIN_CIRCULAR:
					node.Arguments.Add("radius", Visit(context.radius));

					if (context.cant != null)
					{
						node.Arguments.Add("cant", Visit(context.cant));
					}
					break;
				// End()
				case MapV2GrammarLexer.END:
					break;
				// Interpolate(radius?, cant?)
				case MapV2GrammarLexer.INTERPOLATE:
					if (context.radiusE != null)
					{
						node.Arguments.Add("radius", Visit(context.radiusE));
					}
					else if (context.radius != null)
					{
						node.Arguments.Add("radius", Visit(context.radius));

						if (context.cant != null)
						{
							node.Arguments.Add("cant", Visit(context.cant));
						}
					}
					else
					{
						// TODO: 引数なし
					}

					break;
				// Change(radius)
				case MapV2GrammarLexer.CHANGE:
					node.Arguments.Add("radius", Visit(context.radius));
					break;
			}

			return node;
		}

		/// <summary>
		/// 縦曲線の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitGradient([NotNull] MapV2GrammarParser.GradientContext context)
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
				case MapV2GrammarLexer.BEGIN_TRANSITION:
					break;
				// Begin(gradient)
				// BeginConst(gradient)
				case MapV2GrammarLexer.BEGIN:
				case MapV2GrammarLexer.BEGIN_CONST:
					node.Arguments.Add("gradient", Visit(context.gradientArgs));
					break;
				// End()
				case MapV2GrammarLexer.END:
					break;
				// Interpolate(gradient?)
				case MapV2GrammarLexer.INTERPOLATE:
					if (context.gradientArgsE != null)
					{
						node.Arguments.Add("gradient", Visit(context.gradientArgsE));
					}
					else
					{
						// TODO: 引数なし
					}
					break;
			}

			return node;
		}

		/// <summary>
		/// 他軌道の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitTrack([NotNull] MapV2GrammarParser.TrackContext context)
		{
			MapGrammarAstNodes key = Visit(context.key);

			if (context.element != null)
			{
				// 構文タイプ3
				var node = new Syntax3Node(context.Start, context.Stop);
				node.MapElementNames[0] = "track";
				node.MapElementNames[1] = context.element.Text.ToLowerInvariant();
				node.Key = key;
				node.FunctionName = context.func.Text.ToLowerInvariant();

				// 引数の登録
				switch (context.func.Type)
				{
					case MapV2GrammarLexer.INTERPOLATE:
						if (node.MapElementNames[1].Equals("cant"))
						{
							// Cant.Interpolate(cant?)
							if (context.cant != null)
							{
								node.Arguments.Add("cant", Visit(context.cant));
							}
							else
							{
								// TODO: 引数なし
							}
						}
						else
						{
							// (X | Y).Interpolate(x?, radius?)
							if (context.xE != null)
							{
								node.Arguments.Add(node.MapElementNames[1], Visit(context.xE));
							}
							else if (context.x != null)
							{
								node.Arguments.Add(node.MapElementNames[1], Visit(context.x));

								if (context.radius != null)
								{
									node.Arguments.Add("radius", Visit(context.radius));
								}
							}
							else
							{
								// TODO: 引数なし
							}
						}

						break;
					// Cant.SetCenter(x)
					case MapV2GrammarLexer.SET_CENTER:
						node.Arguments.Add("x", Visit(context.x));
						break;
					// Cant.SetGauge(gauge)
					case MapV2GrammarLexer.SET_GAUGE:
						node.Arguments.Add("gauge", Visit(context.gauge));
						break;
					// Cant.SetFunction(id)
					case MapV2GrammarLexer.SET_FUNCTION:
						node.Arguments.Add("id", Visit(context.id));
						break;
					// Cant.BeginTransition()
					case MapV2GrammarLexer.BEGIN_TRANSITION:
						break;
					// Cant.Begin(cant)
					case MapV2GrammarLexer.BEGIN:
						node.Arguments.Add("cant", Visit(context.cant));
						break;
					// Cant.End()
					case MapV2GrammarLexer.END:
						break;
				}

				return node;
			}
			else
			{
				// 構文タイプ2
				var node = new Syntax2Node(context.Start, context.Stop)
				{
					MapElementName = "track",
					Key = key,
					FunctionName = context.func.Text.ToLowerInvariant()
				};

				// 引数の登録
				switch (context.func.Type)
				{
					// Position(x, y, radiusH?, radiusV?)
					case MapV2GrammarLexer.POSITION:
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
					case MapV2GrammarLexer.CANT_ELEMENT:
						node.Arguments.Add("cant", Visit(context.cant));
						break;
					// Gauge(gauge)
					case MapV2GrammarLexer.GAUGE:
						node.Arguments.Add("gauge", Visit(context.gauge));
						break;
				}

				return node;
			}
		}

		/// <summary>
		/// ストラクチャの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitStructure([NotNull] MapV2GrammarParser.StructureContext context)
		{
			// Load(filePath)
			if (context.func.Type == MapV2GrammarLexer.LOAD)
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
				case MapV2GrammarLexer.PUT:
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
				case MapV2GrammarLexer.PUT0:
					node.Arguments.Add("trackkey", Visit(context.trackKey));
					node.Arguments.Add("tilt", Visit(context.tilt));
					node.Arguments.Add("span", Visit(context.span));
					break;
				// PutBetween(trackkey1, trackkey2, flag?)
				case MapV2GrammarLexer.PUTBETWEEN:
					node.Arguments.Add("trackkey1", Visit(context.trackKey1));
					node.Arguments.Add("trackkey2", Visit(context.trackKey2));

					if (context.flag != null)
					{
						node.Arguments.Add("flag", Visit(context.flag));
					}
					break;
			}

			return node;
		}

		/// <summary>
		/// 連続ストラクチャの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitRepeater([NotNull] MapV2GrammarParser.RepeaterContext context)
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
				case MapV2GrammarLexer.BEGIN:
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
				case MapV2GrammarLexer.BEGIN0:
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
				case MapV2GrammarLexer.END:
					break;
			}

			return node;
		}

		/// <summary>
		/// 背景の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitBackground([NotNull] MapV2GrammarParser.BackgroundContext context)
		{
			// 構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "background",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(structurekey)
			if (context.func.Type == MapV2GrammarLexer.CHANGE)
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
		public override MapGrammarAstNodes VisitStation([NotNull] MapV2GrammarParser.StationContext context)
		{
			// Load(filePath)
			if (context.func.Type == MapV2GrammarLexer.LOAD)
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
			if (context.func.Type == MapV2GrammarLexer.PUT)
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
		public override MapGrammarAstNodes VisitSection([NotNull] MapV2GrammarParser.SectionContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "section",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Begin(signalN+)
				// BeginNew(signalN+)
				case MapV2GrammarLexer.BEGIN:
				case MapV2GrammarLexer.BEGIN_NEW:
					node.Arguments.Add("signal0", Visit(context.nullableExpr()));

					for (int i = 0; i < context.exprArgs().Length; i++)
					{
						node.Arguments.Add("signal" + (i + 1), Visit(context.exprArgs()[i]));
					}
					break;
				// SetSpeedLimit(vN+)
				case MapV2GrammarLexer.SET_SPEEDLIMIT:
					node.Arguments.Add("v0", Visit(context.nullableExpr()));

					for (int i = 0; i < context.exprArgs().Length; i++)
					{
						node.Arguments.Add("v" + (i + 1), Visit(context.exprArgs()[i]));
					}
					break;
			}

			return node;
		}

		/// <summary>
		/// 信号機の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitSignal([NotNull] MapV2GrammarParser.SignalContext context)
		{
			string funcName = context.func.Text.ToLowerInvariant();

			// Load(filePath)
			if (context.func.Type == MapV2GrammarLexer.LOAD)
			{
				return new LoadListNode(context.Start, context.Stop)
				{
					MapElementName = "signal",
					Path = Visit(context.path)
				};
			}

			// Signal.SpeedLimit(vN+)
			if (context.func.Type == MapV2GrammarLexer.SPEEDLIMIT)
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
			if (context.func.Type == MapV2GrammarLexer.PUT)
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
		public override MapGrammarAstNodes VisitBeacon([NotNull] MapV2GrammarParser.BeaconContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "beacon",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Put(type, section, senddata)
			if (context.func.Type == MapV2GrammarLexer.PUT)
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
		public override MapGrammarAstNodes VisitSpeedLimit([NotNull] MapV2GrammarParser.SpeedLimitContext context)
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
				case MapV2GrammarLexer.BEGIN:
					node.Arguments.Add("v", Visit(context.v));
					break;
				// End()
				case MapV2GrammarLexer.END:
					break;
			}

			return node;
		}

		/// <summary>
		/// 先行列車の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitPreTrain([NotNull] MapV2GrammarParser.PreTrainContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "pretrain",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Pass(time)
			if (context.func.Type == MapV2GrammarLexer.PASS)
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
		public override MapGrammarAstNodes VisitLight([NotNull] MapV2GrammarParser.LightContext context)
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
				case MapV2GrammarLexer.AMBIENT:
					node.Arguments.Add("red", Visit(context.red));
					node.Arguments.Add("green", Visit(context.green));
					node.Arguments.Add("blue", Visit(context.blue));
					break;
				// Diffuse(red, green, blue)
				case MapV2GrammarLexer.DIFFUSE:
					node.Arguments.Add("red", Visit(context.red));
					node.Arguments.Add("green", Visit(context.green));
					node.Arguments.Add("blue", Visit(context.blue));
					break;
				// Direction(pitch, yaw)
				case MapV2GrammarLexer.DIRECTION:
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
		public override MapGrammarAstNodes VisitFog([NotNull] MapV2GrammarParser.FogContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "fog",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Interpolate(density, red?, green?, blue?)
				// Set(density, red, green, blue)
				case MapV2GrammarLexer.INTERPOLATE:
				case MapV2GrammarLexer.SET:
					if (context.densityE != null)
					{
						node.Arguments.Add("density", Visit(context.densityE));
					}
					else if (context.density != null)
					{
						node.Arguments.Add("density", Visit(context.density));
						node.Arguments.Add("red", Visit(context.red));
						node.Arguments.Add("green", Visit(context.green));
						node.Arguments.Add("blue", Visit(context.blue));
					}
					break;
			}

			return node;
		}

		/// <summary>
		/// 風景描画距離の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitDrawDistance([NotNull] MapV2GrammarParser.DrawDistanceContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "drawdistance",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(value)
			if (context.func.Type == MapV2GrammarLexer.CHANGE)
			{
				node.Arguments.Add("value", Visit(context.value));
			}

			return node;
		}

		/// <summary>
		/// 運転台の明るさの巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitCabIlluminance([NotNull] MapV2GrammarParser.CabIlluminanceContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "cabilluminance",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			switch (context.func.Type)
			{
				// Interpolate(value?)
				// Set(value)
				case MapV2GrammarLexer.INTERPOLATE:
				case MapV2GrammarLexer.SET:
					if (context.valueE != null)
					{
						node.Arguments.Add("value", Visit(context.valueE));
					}
					break;
			}

			return node;
		}

		/// <summary>
		/// 軌道変位の巡回
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns>構文ASTノード</returns>
		public override MapGrammarAstNodes VisitIrregularity([NotNull] MapV2GrammarParser.IrregularityContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "irregularity",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(x, y, r, lx, ly, lr)
			if (context.func.Type == MapV2GrammarLexer.CHANGE)
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
		public override MapGrammarAstNodes VisitAdhesion([NotNull] MapV2GrammarParser.AdhesionContext context)
		{
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "adhesion",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(a, b?, c?)
			if (context.func.Type == MapV2GrammarLexer.CHANGE)
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
		public override MapGrammarAstNodes VisitSound([NotNull] MapV2GrammarParser.SoundContext context)
		{
			// Load(filePath)
			if (context.func.Type == MapV2GrammarLexer.LOAD)
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
		public override MapGrammarAstNodes VisitSound3d([NotNull] MapV2GrammarParser.Sound3dContext context)
		{
			// Load(filePath)
			if (context.func.Type == MapV2GrammarLexer.LOAD)
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
			if (context.func.Type == MapV2GrammarLexer.PUT)
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
		public override MapGrammarAstNodes VisitRollingNoise([NotNull] MapV2GrammarParser.RollingNoiseContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "rollingnoise",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(index)
			if (context.func.Type == MapV2GrammarLexer.CHANGE)
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
		public override MapGrammarAstNodes VisitFlangeNoise([NotNull] MapV2GrammarParser.FlangeNoiseContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "flangenoise",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Change(index)
			if (context.func.Type == MapV2GrammarLexer.CHANGE)
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
		public override MapGrammarAstNodes VisitJointNoise([NotNull] MapV2GrammarParser.JointNoiseContext context)
		{
			// 全て構文タイプ1
			var node = new Syntax1Node(context.Start, context.Stop)
			{
				MapElementName = "jointnoise",
				FunctionName = context.func.Text.ToLowerInvariant()
			};

			// 引数の登録
			// Play(index)
			if (context.func.Type == MapV2GrammarLexer.PLAY)
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
		public override MapGrammarAstNodes VisitTrain([NotNull] MapV2GrammarParser.TrainContext context)
		{
			// Add(trainkey, filePath, trackkey, direction)
			if (context.func.Type == MapV2GrammarLexer.ADD)
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
				node.Arguments.Add("trackkey", Visit(context.trackKey));
				node.Arguments.Add("direction", Visit(context.direction));

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
					// Load(filePath, trackkey, direction)
					case MapV2GrammarLexer.LOAD:
						node.Arguments.Add("filepath", Visit(context.path));
						node.Arguments.Add("trackkey", Visit(context.trackKey));
						node.Arguments.Add("direction", Visit(context.direction));
						break;
					// Enable(time)
					case MapV2GrammarLexer.ENABLE:
						node.Arguments.Add("time", Visit(context.time));
						break;
					// Stop(decelerate, stopTime, accelerate, speed)
					case MapV2GrammarLexer.STOP:
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
		public override MapGrammarAstNodes VisitLegacy([NotNull] MapV2GrammarParser.LegacyContext context)
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
				case MapV2GrammarLexer.FOG:
					node.Arguments.Add("start", Visit(context.startArg));
					node.Arguments.Add("end", Visit(context.endArg));
					node.Arguments.Add("red", Visit(context.red));
					node.Arguments.Add("green", Visit(context.green));
					node.Arguments.Add("blue", Visit(context.blue));
					break;
				// Curve(radius, cant)
				case MapV2GrammarLexer.CURVE:
					node.Arguments.Add("radius", Visit(context.radius));
					node.Arguments.Add("cant", Visit(context.cant));
					break;
				// Pitch(rate)
				case MapV2GrammarLexer.PITCH:
					node.Arguments.Add("rate", Visit(context.rate));
					break;
				// Turn(slope)
				case MapV2GrammarLexer.TURN:
					node.Arguments.Add("slope", Visit(context.slope));
					break;
			}

			return node;
		}

		#endregion

		#region 数式と変数Visitors

		/// <summary>
		/// 変数宣言Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitVarAssign([NotNull] MapV2GrammarParser.VarAssignContext context)
		{
			return new VarAssignNode(context.Start, context.Stop)
			{
				VarName = context.v.varName,
				Value = Visit(context.expr())
			};
		}

		/// <summary>
		/// 数式の連続引数Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitExprArgs([NotNull] MapV2GrammarParser.ExprArgsContext context)
		{
			return Visit(context.nullableExpr());
		}

		/// <summary>
		/// null許容数式Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitNullableExpr([NotNull] MapV2GrammarParser.NullableExprContext context)
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
		/// 括弧数式Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitParensExpr([NotNull] MapV2GrammarParser.ParensExprContext context)
		{
			return base.Visit(context.expr());
		}

		/// <summary>
		/// ユーナリ演算Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitUnaryExpr([NotNull] MapV2GrammarParser.UnaryExprContext context)
		{
			switch (context.op.Type)
			{
				case MapV2GrammarLexer.PLUS:
					return Visit(context.expr());
				case MapV2GrammarLexer.MINUS:
					return new UnaryNode(context.Start, context.Stop)
					{
						InnerNode = Visit(context.expr())
					};
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// 演算Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitInfixExpr([NotNull] MapV2GrammarParser.InfixExprContext context)
		{
			InfixExpressionNode node;

			switch (context.op.Type)
			{
				case MapV2GrammarLexer.PLUS:
					node = new AdditionNode(context.Start, context.Stop);
					break;
				case MapV2GrammarLexer.MINUS:
					node = new SubtractionNode(context.Start, context.Stop);
					break;
				case MapV2GrammarLexer.MULT:
					node = new MultiplicationNode(context.Start, context.Stop);
					break;
				case MapV2GrammarLexer.DIV:
					node = new DivisionNode(context.Start, context.Stop);
					break;
				case MapV2GrammarLexer.MOD:
					node = new ModuloNode(context.Start, context.Stop);
					break;
				default:
					throw new NotSupportedException();
			}

			node.Left = Visit(context.left);
			node.Right = Visit(context.right);

			return node;
		}

		/// <summary>
		/// 数学関数AbsVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitAbsExpr([NotNull] MapV2GrammarParser.AbsExprContext context)
		{
			return new AbsNode(context.Start, context.Stop)
			{
				Value = Visit(context.value)
			};
		}

		/// <summary>
		/// 数学関数Atan2Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitAtan2Expr([NotNull] MapV2GrammarParser.Atan2ExprContext context)
		{
			return new Atan2Node(context.Start, context.Stop)
			{
				Y = Visit(context.y),
				X = Visit(context.x)
			};
		}

		/// <summary>
		/// 数学関数CeilVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitCeilExpr([NotNull] MapV2GrammarParser.CeilExprContext context)
		{
			return new CeilNode(context.Start, context.Stop)
			{
				Value = Visit(context.value)
			};
		}

		/// <summary>
		/// 数学関数CosVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitCosExpr([NotNull] MapV2GrammarParser.CosExprContext context)
		{
			return new CosNode(context.Start, context.Stop)
			{
				Value = Visit(context.value)
			};
		}

		/// <summary>
		/// 数学関数ExpVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitExpExpr([NotNull] MapV2GrammarParser.ExpExprContext context)
		{
			return new ExpNode(context.Start, context.Stop)
			{
				Value = Visit(context.value)
			};
		}

		/// <summary>
		/// 数学関数FloorVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitFloorExpr([NotNull] MapV2GrammarParser.FloorExprContext context)
		{
			return new FloorNode(context.Start, context.Stop)
			{
				Value = Visit(context.value)
			};
		}

		/// <summary>
		/// 数学関数LogVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitLogExpr([NotNull] MapV2GrammarParser.LogExprContext context)
		{
			return new LogNode(context.Start, context.Stop)
			{
				Value = Visit(context.value)
			};
		}

		/// <summary>
		/// 数学関数PowVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitPowExpr([NotNull] MapV2GrammarParser.PowExprContext context)
		{
			return new Atan2Node(context.Start, context.Stop)
			{
				X = Visit(context.x),
				Y = Visit(context.y)
			};
		}

		/// <summary>
		/// 数学関数RandVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitRandExpr([NotNull] MapV2GrammarParser.RandExprContext context)
		{
			var node = new RandNode(context.Start, context.Stop);

			if (context.value != null)
			{
				node.Value = Visit(context.value);
			}

			return node;
		}

		/// <summary>
		/// 数学関数SinVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitSinExpr([NotNull] MapV2GrammarParser.SinExprContext context)
		{
			return new SinNode(context.Start, context.Stop)
			{
				Value = Visit(context.value)
			};
		}

		/// <summary>
		/// 数学関数SqrtVisitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitSqrtExpr([NotNull] MapV2GrammarParser.SqrtExprContext context)
		{
			return new SqrtNode(context.Start, context.Stop)
			{
				Value = Visit(context.value)
			};
		}

		/// <summary>
		/// 変数Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitVarExpr([NotNull] MapV2GrammarParser.VarExprContext context)
		{
			return new VarNode(context.Start, context.Stop)
			{
				Key = context.v.varName
			};
		}

		/// <summary>
		/// 数字項Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitNumberExpr([NotNull] MapV2GrammarParser.NumberExprContext context)
		{
			return new NumberNode(context.Start, context.Stop) { Value = context.num };
		}

		/// <summary>
		/// 文字列Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitStringExpr([NotNull] MapV2GrammarParser.StringExprContext context)
		{
			return new StringNode(context.Start, context.Stop) { Value = context.str };
		}

		/// <summary>
		/// 文字列Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitString([NotNull] MapV2GrammarParser.StringContext context)
		{
			return new StringNode(context.Start, context.Stop) { Value = context };
		}

		/// <summary>
		/// 距離変数項Visitor
		/// </summary>
		/// <param name="context">構文解析の文脈データ</param>
		/// <returns></returns>
		public override MapGrammarAstNodes VisitDistanceExpr([NotNull] MapV2GrammarParser.DistanceExprContext context)
		{
			return new DistanceVariableNode(context.Start, context.Stop);
		}

		#endregion
	}
}
