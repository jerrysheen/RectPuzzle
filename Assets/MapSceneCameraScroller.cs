using System;
//using Spine;
using UnityEngine;

public class MapSceneCameraScroller:IMapCameraScroller
{
	protected enum EScrollMoveType
	{
		None,
		ZChange,
		PosChange,
		AllChange
	}
	/// <summary>
	/// 不同时变化;
	/// </summary>
	protected struct ScrollMoveContext
	{
		public Vector3 startPos;
		public Vector3 endPos;

		public float startZ;
		public float endZ;
		
		public float currentTime;
		public float totalTime;

		public EScrollMoveType moveType;

		public void Clear()
		{
			moveType = EScrollMoveType.None;
			currentTime = totalTime = 0f;
		}
	}
	protected Action<bool, Vector3> _onMapMoveComplete;
	protected Action<bool, Vector3> _onMapScaleComplete;
	
	protected bool _scrollingZ = false;

	protected bool _onPinch = false;
	
	protected float _pinchStartTwoFigerDis;
	protected float _pinchStartScale;

	protected ScrollMoveContext _scrollMoveContext;
	protected bool _continueScroll = false;

	protected bool _onSwipe = false;

	protected Camera _mainCamera;
	public Camera mainCamera
    {
        get
        {
			return _mainCamera;
		}
	}
	protected Transform _cameraTrans;
	
	protected int _minLod = 0;
	protected int _maxLod = 0;
	
//	private const float PINCH_MIN_VALUE = 2;
//
//	protected const float PINCH_MAX_VALUE = 42;

	//protected float _currentPinchValue = PINCH_MIN_VALUE;

	protected float _maxScale = MapCalcConst.DEFAULT_MAX_SCALE;
	public float maxScale
	{
		get
		{
			return _maxScale;
		}
		set
		{
			_maxScale = value > MapCalcConst.DEFAULT_MAX_SCALE ? MapCalcConst.DEFAULT_MAX_SCALE : value;
			if (_currentScale > _maxScale)
			{
				MoveCameraScaleTo(_maxScale);
			}
		}
	}

	protected float _minScale = MapCalcConst.DEFAULT_MIN_SCALE;
	// private float _minScale;
	public float minScale
	{
		get
		{
			return _minScale;
		}
		set
		{
			_minScale = value < MapCalcConst.DEFAULT_MIN_SCALE ? MapCalcConst.DEFAULT_MIN_SCALE : value;
			if (_currentScale != 0 && _currentScale < _minScale)
			{
				MoveCameraScaleTo(_minScale);
			}
		}
	}

	// 常量内城相机便宜。-- 开放世界以后需要统一;

	protected float _currentScale = 0;
	public float currentScale
	{
		get
		{
			return _currentScale;
		}

		set
		{
			if (value < _minScale)
			{
				value = _minScale;
			}
			else if (value > _maxScale)
			{
				value = _maxScale;
			}

			InternalSetCurrentScale(value);
		}
	}

	public virtual void SetCameraLPos(Vector3 lpos)
	{
		if (_cameraTrans == null) return;
		_cameraTrans.localPosition = lpos;
	}
	
	public virtual Vector3 GetCameraLPos()
	{
		if (_cameraTrans == null) return Vector3.zero;
		return _cameraTrans.localPosition;
	}

	protected MapCameraInfo endLevelInfo;
	protected float ratio;

	protected virtual void InternalSetCurrentScale(float value)
	{
		int curLevel = 0;
		
		int infoLen = _cameraInfos.Length;
		if (infoLen < 2) return;

		if (value >= _maxScale)
		{
			curLevel = _maxLod - 1;
		}
		else
		{
			for (int i = _minLod + 1; i <= _maxLod; ++i)
			{
				if (value < _cameraInfos[i].y)
				{
					curLevel = i - 1;
					break;
				}
			}
		}

		currentLevel = curLevel;
		InternalSetCameraZValue(_currentScale, value);
		endLevelInfo = _cameraInfos[curLevel + 1];

		ratio = (_currentScale - _currentCameraInfo.y) / (endLevelInfo.y - _currentCameraInfo.y);
		InternalSetCameraFOV(Mathf.Lerp(_currentCameraInfo.fov, endLevelInfo.fov, ratio));
		InternalSetCameraFarClip(Mathf.Lerp(_currentCameraInfo.farClip, endLevelInfo.farClip, ratio));
		InternalSetCameraNearClip(Mathf.Lerp(_currentCameraInfo.nearClip, endLevelInfo.nearClip, ratio));

		// 缩放也影响跟随;
		//UITrackerManager.GetInstance().UpdateTrackersPosImmediately();
	}

	protected int _currentLevel = -1;
	
	public int currentLevel
	{
		get
		{
			return _currentLevel;
		}

		set
		{
			if (_currentLevel != value)
			{
				_currentLevel = value;
				if (_currentLevel < _cameraInfos.Length)
				{
					_currentCameraInfo = _cameraInfos[_currentLevel];
				}
				else
				{
					//Debugger.LogError("Set level error " + _currentLevel);
				}
				
			}
		}
	}

	public float fov
	{
		get
		{
			if (mainCamera == null)
			{
				return 0;
			}
			return mainCamera.fieldOfView;
		}
	}

	public Func<float, float, bool> scaleInterceptor;

	protected Transform _container;

	protected MapCameraInfo[] _cameraInfos;
    public MapCameraInfo[] cameraInfos
    {
        get
        {
            return _cameraInfos;
        }
        set
        {
            _cameraInfos = value;
        }
    }

    //private float[] _pinchSpeedScaleArr;

	protected MapCameraInfo _currentCameraInfo;

//	private float _zRange;

	protected Camera[] _cameras;
	protected MapDraggableCamera mDraggableCamera;

	protected bool _worldmapBound = true;
	protected float _boundXMin = float.MinValue;

	protected float _boundXMax = float.MaxValue;

	protected float _boundYMin = float.MinValue;

	protected float _boundYMax = float.MaxValue;

	private void SetWorldBoundFlag( bool worldmapBound)
	{
		_worldmapBound = worldmapBound;
	}

	public void SetSwipeBound(float xMin, float yMin, float xMax, float yMax, bool worldmapBound = true)
	{
		_boundXMin = xMin;
		_boundXMax = xMax;
		_boundYMin = yMin;
		_boundYMax = yMax;
		SetWorldBoundFlag(worldmapBound);
	}

	public void ResetSwipeBound()
	{
		_boundXMin = float.MinValue;
		_boundXMax = float.MaxValue;
		_boundYMin = float.MinValue;
		_boundYMax = float.MaxValue;
		SetWorldBoundFlag(true);
	}

	public virtual void InitCameraPos(Camera mainCamera, Vector3 defaultCenter, MapCameraInfo[] cameraInfos, int defaultLevel, float defaultScale = 0)
	{
		try
		{
			ClearData();
			_mainCamera = mainCamera;
//			if (_mainCamera != null)
//			{
//				Debug.LogError("InitCameraPos : " + _mainCamera.transform.name);
//			}
			_cameraInfos = cameraInfos;
			_cameras = _mainCamera.GetComponentsInChildren<Camera>();
			if (_cameras == null)
			{
				Debug.LogError("InitCameraPos _cameras is null ");
			}
			mDraggableCamera = _mainCamera.GetComponent<MapDraggableCamera>();
			if (mDraggableCamera == null)
			{
				Debug.LogError("InitCameraPos mDraggableCamera is null ");
			}
			mDraggableCamera.ConstrainCameraPos = CheckOutOfBound;
			// 相机添加了一个中间节点，用于震屏;
			_cameraTrans = _mainCamera.transform.parent;
			_container = _cameraTrans.parent;
			SetCameraLPos(defaultCenter);

			_minLod = 0;
			_maxLod = cameraInfos.Length - 1;
		
			_minScale = _cameraInfos[0].y;
			MapCameraInfo maxCameraInfo = _cameraInfos[cameraInfos.Length - 1];
			_maxScale = maxCameraInfo.y;
			CameraManager.s_maxFov = maxCameraInfo.fov;
		
			if (defaultScale > 0)
			{
				currentLevel = CalLodByScale(defaultScale);
				currentScale = defaultScale;
			}
			else
			{
				currentLevel = defaultLevel;
				currentScale = _currentCameraInfo.y;
			}
		}
		catch (Exception e)
		{
			Debug.LogError("InitCameraPos:  " + e);
			throw;
		}

	}
	
	public void SetMinMaxLod(int minLod, int maxLod)
	{
		if(_cameraInfos == null)
			return;

		_minLod = minLod;
		_maxLod = maxLod;
		
		if (minLod >= 0 && minLod <= _cameraInfos.Length - 1)
		{
			_minScale = _cameraInfos[minLod].y;
			if (currentScale < _minScale)
				currentScale = _minScale;
		}

		if (maxLod >= 0 && maxLod >= minLod && maxLod <= _cameraInfos.Length - 1)
		{
			_maxScale = _cameraInfos[maxLod].y;
			if (currentScale > _maxScale)
				currentScale = _maxScale;
		}
	}

	public void ClearData()
	{
		_currentLevel = -1;
		_currentScale = 0;
	}

	public int CalLodByScale(float scale)
	{
		if (cameraInfos == null)
		{
			return 0;
		}

		int ret = -1;
		for (int i = 1; i < cameraInfos.Length; i++)
		{
			if (scale < cameraInfos[i].y)
			{
				ret = i-1;
				break;
			}
		}
		return ret<0?0:ret;
	}

	/// <summary>
	/// 只修改Y，但要确保看得中心位置不变，所以需要额外调整z坐标;
	/// </summary>
	/// <param name="oldY"></param>
	/// <param name="newY"></param>
	protected virtual void InternalSetCameraZValue(float oldY, float newY)
	{
		Vector3 cameraLocalPos = _cameraTrans.localPosition;
		//cameraLocalPos.y += oldZ;
		Vector3 containerPos = _container.localPosition;
		containerPos.y = newY;
		_container.localPosition = containerPos;
		cameraLocalPos.z += (oldY-newY);
		SetCameraLPos(cameraLocalPos);
		_currentScale = newY;
		// float currentRatio = (_currentScale - MapCalcConst.DEFAULT_MIN_SCALE) / (MapCalcConst.DEFAULT_MAX_SCALE - MapCalcConst.DEFAULT_MIN_SCALE);
		// _currentPinchValue = PINCH_MIN_VALUE + (PINCH_MAX_VALUE - PINCH_MIN_VALUE) * Mathf.Pow(currentRatio, 0.25f);
	}

	protected virtual void InternalSetCameraFOV(float fov)
	{
		_mainCamera.fieldOfView = fov;
	}

	private void InternalSetCameraFarClip(float farClip)
	{
		foreach(var cam in _cameras)
		{
			cam.farClipPlane = farClip;
		}
	}
	
	private void InternalSetCameraNearClip(float nearClip)
	{
		foreach(var cam in _cameras)
		{
			cam.nearClipPlane = nearClip;
		}
	}

	public void Reset()
	{
		_onPinch = false;
		_onSwipe = false;
	}

	public virtual void OnPinchOut(Gesture gesture)
	{
		_onPinch = true;
		_continueScroll = false;
		
		if (MapDraggableCamera.LOCK_PINCH)
			return;
		
		//float delta = gesture.deltaPinch * Mathf.Clamp(gesture.deltaTime, 0f, 0.0333333f) * Mathf.Sqrt(2) * .5f;
		
		if (currentScale > _minScale)
		// float pinchVal = PINCH_MIN_VALUE - OVER_Z;
		//if (_currentPinchValue > pinchVal)
		{
//			float curPinchVal =  _currentPinchValue;
//			curPinchVal -= delta;
//			if (curPinchVal <= pinchVal)
//			{
//				curPinchVal = pinchVal;
//			}

			float newScale = _pinchStartScale / (gesture.twoFingerDistance / _pinchStartTwoFigerDis);
			if (scaleInterceptor == null || !scaleInterceptor(_currentScale, newScale))
			{
				//_currentPinchValue = curPinchVal;
				currentScale = newScale;
			}
		}
	}

	public virtual void OnPinchIn(Gesture gesture)
	{
		_onPinch = true;
		_continueScroll = false;

		if (MapDraggableCamera.LOCK_PINCH)
			return;
		
		//float delta = gesture.deltaPinch * Mathf.Clamp(gesture.deltaTime, 0f, 0.0333333f) * Mathf.Sqrt(2) * .5f;

		// float pinchVal = PINCH_MAX_VALUE + OVER_Z;

		if (currentScale < _maxScale)
		// if (_currentPinchValue < pinchVal)
		{
//			float curPinchVal =  _currentPinchValue;
//			curPinchVal += delta;
//			if (curPinchVal >= pinchVal)
//			{
//				curPinchVal = pinchVal;
//			}
			
			float newScale = _pinchStartScale / (gesture.twoFingerDistance / _pinchStartTwoFigerDis);
			if (scaleInterceptor == null || !scaleInterceptor(_currentScale, newScale))
			{
				//_currentPinchValue = curPinchVal;
				currentScale = newScale;
			}
		}
	}

	public void OnPinchEnd(Gesture gesture)
	{
//		_scrollingZ = true;
//		_onPinch = true;
		_onPinch = false;
	}

	public void OnTouchStart2Fingers(Gesture gesture)
	{
		_pinchStartTwoFigerDis = gesture.twoFingerDistance;
		_pinchStartScale = currentScale;
	}

	public virtual bool OnSwipe(Gesture gesture)
	{
		if (_onPinch)
			return false;
		
		mDraggableCamera?.Drag(gesture);	// 改用新的摄像机拖动算法
		return true;
	}

	private Vector2 _lastOffset = new Vector2();
	public virtual void OnSwipeStart(Gesture gesture)
	{
		if (_onPinch)
		{
			return;
		}
		
		mDraggableCamera?.Drag(gesture);		// 改用新的摄像机拖动算法
		
		_lastOffset.Set(0, 0);
		_onSwipe = true;
		_continueScroll = false;
		_scrollMoveContext.Clear ();
		DoDisturbLastAutoMoveCallback();
		DoDisturbLastAutoScaleCallback();

	}

	public virtual void OnTouchStart(Gesture gesture, ref bool isStopCameraMove)
	{
		mDraggableCamera?.Press(true);		// 改用新的摄像机拖动算法
		
		_onSwipe = false;
		if (_onPinch)
		{
			return;
		}
		
		isStopCameraMove = _continueScroll;
		_continueScroll = false;
		_scrollMoveContext.Clear ();
		DoDisturbLastAutoMoveCallback();
		DoDisturbLastAutoScaleCallback();
	}

	public virtual void OnTouchUp(Gesture gesture)
	{
		mDraggableCamera?.Press(false);		// 改用新的摄像机拖动算法
	}

	public void OnSwipeEnd(Gesture gesture)
	{
		if (_onPinch || !_onSwipe)
		{
			return;
		}
		_onSwipe = false;
	}

	public void ResetAutoMoveCallback(bool notify)
	{
		if (notify)
		{
			DoDisturbLastAutoMoveCallback();
			DoDisturbLastAutoScaleCallback();
		}
		else
		{
			_onMapMoveComplete = null;
			_onMapScaleComplete = null;	
		}
	}

	protected void DoDisturbLastAutoMoveCallback(bool ret = false)
	{
		if (_onMapMoveComplete != null)
		{
			Action<bool, Vector3> tmpCallback = _onMapMoveComplete;
			_onMapMoveComplete = null;
			tmpCallback(ret, _cameraTrans.localPosition);
		}
	}

	protected void DoDisturbLastAutoScaleCallback(bool ret = false){
		if (_onMapScaleComplete != null)
		{
			Action<bool, Vector3> tmpCallback = _onMapScaleComplete;
			_onMapScaleComplete = null;
			tmpCallback(ret, _cameraTrans.localPosition);
		}
	}

	private void OnAutoScrollFinish()
	{
		//LuaInterface.LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnAutoScrollFinish");
		//if (luaFunc != null)
		//{
		//	luaFunc.Call();
		//}
	}

	public void CancelMove()
	{
		_continueScroll = false;
		_scrollMoveContext.Clear ();

		DoDisturbLastAutoMoveCallback();
	}
	
	/// <summary>
	/// 获取相机当前中心点所在的世界坐标
	/// </summary>
	/// <returns></returns>
	public virtual Vector3 GetCameraCenterWorldPos()
	{
		//if (_cameraTrans == null)
		//{
		//	return Vector3.zero;
		//}
		//return MapUtils.GetCameraWorldPos(_cameraTrans.localPosition, _currentScale);
		return Vector3.zero;
	}
	
	public void MoveCameraToGrid(Vector2Int targetGrid, float duration = .4f, Action<bool, Vector3> callback = null)
	{
		Vector3 worldPos = new Vector3();
		GlobalGridToPos(targetGrid.x, targetGrid.y, out worldPos.x, out worldPos.z);
		MoveCameraToPosition(worldPos, duration, callback);
	}

	public void MoveCameraQuickToGrid(Vector2Int targetGrid)
	{
		Vector3 worldPos = new Vector3();
		GlobalGridToPos(targetGrid.x, targetGrid.y, out worldPos.x, out worldPos.z);
		MoveCameraQuickToPosition(worldPos);
	}

	/// <summary>
	/// 同时移动 z、postion
	/// </summary>
	public void MoveCameraToPosWithScaleEx(Vector3 worldPos, float scale, float duration = .4f, Action<bool, Vector3> callback = null)
	{
		if(_cameraTrans == null)
			return;
		
		_scrollMoveContext.Clear ();

		scale = Mathf.Clamp(scale, _minScale, _maxScale);
		_scrollMoveContext.startZ = currentScale;
		_scrollMoveContext.endZ = scale;
		
		worldPos.y = 0;
		_scrollMoveContext.startPos = GetCameraCenterWorldPos();
		_scrollMoveContext.endPos = worldPos;

		_scrollMoveContext.moveType = EScrollMoveType.AllChange;
		_scrollMoveContext.totalTime = duration;
		_continueScroll = true;
		
		_onMapScaleComplete = callback;
	}

	/// <summary>
	/// 先移动position、再移动scale
	/// </summary>
	/// <param name="worldPos"></param>
	/// <param name="scale"></param>
	/// <param name="duration"></param>
	/// <param name="callback"></param>
	public void MoveCameraToPositionWithScale(Vector3 worldPos, float scale, float duration = .4f, Action<bool, Vector3> callback = null)
	{
		MoveCameraToPosition(worldPos, duration, (bool b, Vector3 v) => {MoveCameraScaleTo(scale, duration, callback); });
	}
	
	public virtual void MoveCameraToPosition(Vector3 worldPos, float duration = .2f, Action<bool, Vector3> callback = null)
	{
		//_scrollMoveContext.Clear ();
		
		//worldPos.y = 0;
		//Vector3 expectedPos = MapUtils.GetCameraPosLookAt(worldPos, _currentScale);

		//_scrollMoveContext.startPos = _cameraTrans.localPosition;
		//_scrollMoveContext.endPos = expectedPos;
		//_scrollMoveContext.moveType = EScrollMoveType.PosChange;
		//_scrollMoveContext.totalTime = duration;
		//_continueScroll = true;
		
		//_onMapMoveComplete = callback;
	}

	public virtual void MoveCameraScaleTo(float value, float duration = .2f, Action<bool, Vector3> callback = null)
	{
		if(_cameraTrans == null)
			return;
		
		_scrollMoveContext.Clear ();

		value = Mathf.Clamp(value, _minScale, _maxScale);
		_scrollMoveContext.startZ = currentScale;
		_scrollMoveContext.endZ = value;
		_scrollMoveContext.moveType = EScrollMoveType.ZChange;
		_scrollMoveContext.totalTime = duration;
		_continueScroll = true;
		
		_onMapScaleComplete = callback;
		//DoDisturbLastAutoScaleCallback();
	}

	public virtual void MoveCameraQuickToPosition(Vector3 worldPos)
	{
		//_continueScroll = false;
		//_scrollMoveContext.Clear ();
		//Vector3 pos = MapUtils.GetCameraPosLookAt(worldPos, _currentScale);
		//SetCameraLPos(pos);
		//DoDisturbLastAutoMoveCallback();
	}
	
	public void SetCameraPosAndScale(float x,float z,float scale)
	{
		var m = WorldMapManager.instance.mHomeMedia;
		if (m != null)
		{
			if (m.cameraScroller != null)
			{
				var scroller = (HomeSceneCameraScroller) m.cameraScroller;
				scroller.currentLevel = scroller.CalLodByScale(scale);
				scroller.currentScale = scale;
			}
		}
		MoveCameraQuickToPosition(new Vector3(x, 0, z));
	}

	public void ShrinkCameraScale()
	{
		if (currentScale > maxScale)
		{
			currentScale = maxScale;
			if (_scrollingZ)
			{
				_scrollingZ = false;
				_onPinch = false;
			}
			_continueScroll = false;
			_scrollMoveContext.Clear ();
		}
	}

	public void ExpandCameraScale()
	{
		if (currentScale < maxScale)
		{
			currentScale = maxScale;
			if (_scrollingZ)
			{
				_scrollingZ = false;
				_onPinch = false;
			}
			_continueScroll = false;
			_scrollMoveContext.Clear ();
		}
	}

	public virtual void SetFovRange(float minFov, float maxFov)
	{
	}

	internal float InOutExpoEase(float time, float duration)
	{
		if (time == duration)
		{
			return 1f;
		}
		return (-((float) Math.Pow(2.0, (double) ((-10f * time) / duration))) + 1f);
	}

	public void CalculateCameraPos(out Vector3 localPos)
	{
		localPos = GetCameraLPos();
		
		bool posMoveComplete = false;
		bool scaleMoveComplete = false;
		
		if (_continueScroll)
		{
			_onPinch = false;
			if (_scrollMoveContext.currentTime < _scrollMoveContext.totalTime)
			{
				_scrollMoveContext.currentTime += Time.smoothDeltaTime;//Mathf.Clamp(Time.smoothDeltaTime, 0, 0.03333f);
				float t = InOutExpoEase (_scrollMoveContext.currentTime, _scrollMoveContext.totalTime);
				//当已经很接近目标位置时 直接取1 到达目标位置
				if (Mathf.Abs(t - 1)<0.01f)
				{
					t = 1;
				}
				switch (_scrollMoveContext.moveType)
				{
					case EScrollMoveType.ZChange:
					{
						posMoveComplete = true;

						float _z = Mathf.Lerp(_scrollMoveContext.startZ, _scrollMoveContext.endZ, t);
						InternalSetCurrentScale(_z);
						break;
					}
					case EScrollMoveType.PosChange:
					{
						scaleMoveComplete = true;
					
						Vector3 curPos = Vector3.Lerp(_scrollMoveContext.startPos, _scrollMoveContext.endPos, t);
						localPos.x = curPos.x;
						localPos.z = curPos.z;
						SetCameraLPos(localPos);
						break;
					}
					case EScrollMoveType.AllChange:
					{
						float _z = Mathf.Lerp(_scrollMoveContext.startZ, _scrollMoveContext.endZ, t);
						InternalSetCurrentScale(_z);
						Vector3 pos = Vector3.Lerp(_scrollMoveContext.startPos, _scrollMoveContext.endPos, t);
						pos = MapUtils.GetCameraPosLookAt(pos, _currentScale);
						SetCameraLPos(pos);
						break;
					}
				}
			}
			else
			{
				posMoveComplete = true;
				scaleMoveComplete = true;
				_continueScroll = false;
			}
//			DebugUtil.Log("continueScroll " + _continueScroll + " currentTime " + _scrollMoveContext.currentTime + " totalTime " + _scrollMoveContext.totalTime);
		}
		//上面可能已经对相机坐标做了修改 这里刷新下缓存
		localPos = GetCameraLPos();
		if (CheckOutOfBound(ref localPos))
		{
			SetCameraLPos(localPos);
		}

		if(scaleMoveComplete) {
			DoDisturbLastAutoScaleCallback(true);
		}
		if(posMoveComplete) {
			DoDisturbLastAutoMoveCallback(true);
		}
		
		if (!_continueScroll && posMoveComplete && scaleMoveComplete)
		{
			OnAutoScrollFinish();
		}
	}
	
	protected virtual bool CheckOutOfBound(ref Vector3 localPos)
	{
		if (mDraggableCamera == null)
		{
			return false;
		}
		float boundXMin, boundYMin, boundXMax, boundYMax;
		if (_worldmapBound)
		{
			GlobalGridToPos(0, 0, out boundXMin, out boundYMin);
			GlobalGridToPos(MapPlaceConfig.originGlobalMapWidth, MapPlaceConfig.originGlobalMapHeight, out boundXMax, out boundYMax);

			boundXMin = Mathf.Max(_boundXMin, boundXMin);
			boundYMin = Mathf.Max(_boundYMin, boundYMin);
			boundXMax = Mathf.Min(_boundXMax, boundXMax);
			boundYMax = Mathf.Min(_boundYMax, boundYMax);
		}
		else
		{
			boundXMin = _boundXMin;
			boundYMin = _boundYMin;
			boundXMax = _boundXMax;
			boundYMax = _boundYMax;
		}

		float worldPosY = localPos.z + _currentScale;

		bool xOverflow = localPos.x < boundXMin || localPos.x > boundXMax;
		bool yOverflow = worldPosY < boundYMin || worldPosY > boundYMax;
		if (xOverflow || yOverflow)
		{
			if (xOverflow)
			{
				localPos.x = Mathf.Clamp(localPos.x, boundXMin, boundXMax);
			}
			if (yOverflow)
			{
				localPos.z = Mathf.Clamp(worldPosY, boundYMin, boundYMax);
				localPos.z -= _currentScale;
			}

			return true;
		}
		return false;
	}

	public void ClearCamera()
	{
		mDraggableCamera = null;
	}
	
	protected virtual void GlobalGridToPos(int globalGridX, int globalGridY, out float x, out float y)
	{
		MapUtils.GlobalGridPosToWorldPos(globalGridX, globalGridY, out x, out y);
	}
}
