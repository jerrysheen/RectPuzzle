
using System.Collections.Generic;
using UnityEngine;

// !!!!IMPORTANT:本类只在Editor继承MonoBehaviour类只为方便自定义Inspector窗口，方便调试测试Manager。
// 严禁使用MonoBehaviour的Awake、OnUpdate等方法，在真机无效。

#if UNITY_EDITOR
public abstract class MgrBase : MonoBehaviour
#else
public abstract class MgrBase 
#endif
{
	protected static List<MgrBase> s_MgrList = new List<MgrBase>();
	protected static List<MgrBase> s_UpdateMgrList = new List<MgrBase>();
	protected static List<MgrBase> s_LateUpdateMgrList = new List<MgrBase>();
	protected static List<MgrBase> s_FocusMgrList = new List<MgrBase>();

	public static Transform s_Parent = null;

	/// <summary>
	/// 重连成功后触发;
	/// </summary>
	internal static void ReConnectAll()
	{
		for (int i = 0; i < s_MgrList.Count; i++)
		{
            MgrBase mgrBase = s_MgrList[i];
            if (mgrBase == null)
			{
				continue;
			}

            mgrBase.OnReConnect();
		}
	}

	/// <summary>
	/// 数据清除;
	/// </summary>
	public static void ClearAll()
	{
		foreach (MgrBase mgrBase in s_MgrList)
		{
			mgrBase.Clear();
		}
	}

	public static void UpdateAll(float deltaTime)
	{
		foreach (MgrBase mgrBase in s_UpdateMgrList)
		{
			// Debugger.LogWarningFormat("mgrBase {0} ~~~~~~~", mgrBase.GetType().Name);
			mgrBase.OnUpdate(deltaTime);
		}
	}

	/// <summary>
	/// 延迟更新;
	/// </summary>
	/// <param name="deltaTime"></param>
	public static void LateUpdate(float deltaTime)
	{
		//s_LateUpdateMgrList
		foreach (MgrBase mgrBase in s_LateUpdateMgrList)
		{
			mgrBase.OnLateUpdate(deltaTime);
		}
	}

	/// <summary>
	/// 返回登陆;
	/// </summary>
	public static void ReturnLoginAll()
    {
	    foreach (MgrBase mgrBase in s_MgrList)
        {
	        mgrBase.ReturnLogin();
        }
    }
	
	/// <summary>
	/// 焦点事件;
	/// </summary>
	public static void Focus(bool hasFocus)
	{
		foreach (MgrBase mgrBase in s_FocusMgrList)
		{
			mgrBase.OnFocus(hasFocus);
		}
	}
	
	#if UNITY_EDITOR
	internal void InitForEditor() { OnInit(); }
	#endif

	protected virtual void OnInit() { }

	public virtual void OnUpdate(float deltaTime) { }
	
	public virtual void OnLateUpdate(float deltaTime) { }

	public virtual void OnReConnect() { }

	public virtual void Clear() { }

    // 返回登陆大清;
    internal virtual void ReturnLogin() { }
    
    // 焦点事件;
    internal virtual void OnFocus(bool hasFocus) { }
}

#if UNITY_EDITOR
public abstract class Singleton<T> : MgrBase where T : MgrBase
#else
public abstract class Singleton<T>: MgrBase where T : Singleton<T>, new()
#endif
{
	static T _Instance = null;

	public static T GetInstance()
	{
		if (null == _Instance)
		{
#if UNITY_EDITOR
			GameObject gObj = new GameObject(typeof(T).Name);
			DontDestroyOnLoad(gObj);
			Transform tran = gObj.transform;
			tran.localPosition = Vector3.zero;
			tran.localEulerAngles = Vector3.zero;
			tran.localScale = Vector3.one;
			if (s_Parent != null)
			{
				tran.SetParent(s_Parent, false);
			}
			else
			{
				GameObject parentObj = new GameObject("MgrBaseRoot");
				s_Parent = parentObj.transform;
				tran.SetParent(s_Parent, false);
			}

			_Instance = gObj.AddComponent<T>();
#else
            _Instance = new T();
#endif

			s_MgrList.Add(_Instance);
			
			// Debugger.LogErrorFormat("Add Update mgrBase {0} ~~~~~~~", _Instance.GetType().Name);
			s_UpdateMgrList.Add(_Instance);
			
#if UNITY_EDITOR
			_Instance.InitForEditor();;
#else
			_Instance.OnInit();
#endif
        }
		return _Instance;
	}

	public static T instance
	{
		get
		{
			return GetInstance();
		}
	}
	
	public static bool CheckInstance()
	{
		return _Instance != null;
	}
	
	/// <summary>
	/// 关闭Update;
	/// </summary>
	internal bool closeUpdate
	{
		set
		{
			if (value)
			{
				// Debugger.LogErrorFormat("Close Update mgrBase {0} ~~~~~~~", this.GetType().Name);
				s_UpdateMgrList.Remove(this);
			}
		}
	}

	/// <summary>
	/// 启动LateUpdate;
	/// </summary>
	public bool lateUpdateEnable
	{
		set
		{
			if (value) s_LateUpdateMgrList.Add(this);
		}
	}
	
	/// <summary>
	/// 启动焦点事件;
	/// </summary>
	internal bool focusEnable
	{
		set
		{
			if (value) s_FocusMgrList.Add(this);
		}
	}
}