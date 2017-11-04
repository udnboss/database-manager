using System;

namespace EngineManager
{
	/// <summary>
	/// Description of DBViewManager.
	/// </summary>
	public static class DBViewManager
	{
		public static void Refresh(DBView v)
		{
			//reset
			DBObjectManager.BeforeRefresh(v);
			
			//self
			
			//columns
			DBObjectManager.RefreshColumns(v);
			
			//properties
			DBObjectManager.RefreshProperties(v);
			
			//intact			
			DBObjectManager.AfterRefresh(v);
		}
	}
}
