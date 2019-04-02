using System.Collections.Generic;

namespace Bve5Parser.MapGrammar.V2
{
	/// <summary>
	/// 変数管理クラス
	/// </summary>
	public class VariableStore
	{
		public Dictionary<string, object> Vars { get; private set; }

		/// <summary>
		/// 変数を追加、もしくは上書きします。
		/// </summary>
		/// <param name="key">変数名</param>
		/// <param name="val">変数の値</param>
		public void SetVar(string key, object val)
		{
			if (Vars.ContainsKey(key))
			{
				Vars[key] = val;
			}
			else
			{
				Vars.Add(key, val);
			}
		}

		/// <summary>
		/// 変数を取得します。
		/// </summary>
		/// <param name="key">変数名</param>
		/// <returns>変数の値</returns>
		public object GetVar(string key)
		{
			// TODO: 本家仕様を確認する。変数がない場合は0だっけ？
			return Vars.ContainsKey(key) ? Vars[key] : 0;
		}

		/// <summary>
		/// 変数をすべてクリアします。
		/// </summary>
		public void ClearVar()
		{
			Vars = new Dictionary<string, object>();
		}
	}
}
