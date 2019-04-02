using Antlr4.Runtime;
using Bve5Parser.MapGrammar.V2.ANTLR_SyntaxDefinitions;

namespace Bve5Parser.MapGrammar.V2
{
	/// <summary>
	/// MapGrammar構文解析器のエラー処理を行うクラス。
	/// </summary>
	internal class MapV2GrammarErrorStrategy : MapGrammarErrorStrategy
	{
		/// <summary>
		/// エラーの復帰処理を行います。
		/// 次のステートメントの終わり、もしくは構文の終わり(EOF)まで字句を読み飛ばします。
		/// </summary>
		/// <param name="recognizer"></param>
		/// <param name="e"></param>
		public override void Recover(Parser recognizer, RecognitionException e)
		{
			var type = recognizer.InputStream.La(1);

			while (type != MapV2GrammarLexer.Eof && type != MapV2GrammarLexer.STATE_END)
			{
				recognizer.Consume();
				type = recognizer.InputStream.La(1);
			}
		}
	}
}
