
using UnityEngine;

namespace ELEX.Common.Utility.Internal
{
	internal class RootUtility
	{
		protected static Transform s_PoolRootTran;

		protected static Transform m_ManagerRootTran;

		public static Transform poolRootTran
		{
			get
			{
				if ( s_PoolRootTran == null )
				{
					GameObject gObj = new GameObject("Pool_Root");
					GameObject.DontDestroyOnLoad(gObj);

					s_PoolRootTran = gObj.transform;
                }

				return s_PoolRootTran;
			}
		}
		public static Transform managerRoot
		{
			get
			{
				if (m_ManagerRootTran == null)
				{
					GameObject gObj = new GameObject("ManagerRoot");
					GameObject.DontDestroyOnLoad(gObj);
					m_ManagerRootTran = gObj.transform;
				}

				return m_ManagerRootTran;
			}
		}
	}
}