using System;
using UnityEngine;

public class HomeSceneCameraScroller:MapSceneCameraScroller
{
	protected float minFov = 0;
	protected float maxFov = 0;
	//家园场景相机最小fov时可移动范围
	private float _boundXMinLow = float.MinValue;
	private float _boundXMaxLow = float.MaxValue;
	private float _boundYMinLow = float.MinValue;
	private float _boundYMaxLow = float.MaxValue;
	
	protected MapDraggableTarget mDraggableTarget;
	protected Transform mTargetTrans;
	
	public override void InitCameraPos(Camera mainCamera, Vector3 defaultCenter, MapCameraInfo[] cameraInfos, int defaultLevel, float defaultScale = 0)
	{
		ClearData();
		_mainCamera = mainCamera;
		_cameraInfos = cameraInfos;
		_cameras = _mainCamera.GetComponentsInChildren<Camera>();
		mDraggableTarget = GameObject.Find("CamTarget").GetComponent<MapDraggableTarget>();
		mDraggableTarget.Init();
		mDraggableTarget.ConstrainCameraPos = CheckOutOfBound;
		mDraggableTarget.UpdateCameraPos = UpdateCameraPos;
		mTargetTrans = mDraggableTarget.transform;
		// 相机添加了一个中间节点，用于震屏;
		_cameraTrans = _mainCamera.transform.parent;
		_container = _cameraTrans.parent;
		SetTargetLPos(defaultCenter);
		
		_minLod = 0;
		_maxLod = cameraInfos.Length - 1;

		_minScale = _cameraInfos[0].y;
		MapCameraInfo maxCameraInfo = _cameraInfos[cameraInfos.Length - 1];
		_maxScale = maxCameraInfo.y;
		CameraManager.s_maxFov = maxCameraInfo.fov;
		
		//if (defaultScale > 0)
		//{
		//	currentLevel = CalLodByScale(defaultScale);
		//	currentScale = defaultScale;
		//}
		//else if(defaultLevel>=0)
		//{
		//	currentLevel = defaultLevel;
		//	currentScale = _currentCameraInfo.y;
		//}
		//else
		//{
		//	currentLevel = CityConst.instance.DefaultLevel;
		//	currentScale = _currentCameraInfo.y;
		//}
	}

	public void SetTargetLPos(Vector3 lpos)
	{
		if (mTargetTrans == null) return;
		mTargetTrans.localPosition = lpos;
	}
	
	public override void OnPinchOut(Gesture gesture)
	{
		_onPinch = true;
		_continueScroll = false;
		
		if (MapDraggableCamera.LOCK_PINCH)
			return;
		
//		if (currentScale > _minScale)
//		{
//			float newScale = _pinchStartScale / (gesture.twoFingerDistance / _pinchStartTwoFigerDis);
//			if (scaleInterceptor == null || !scaleInterceptor(_currentScale, newScale))
//			{
//				currentScale = currentScale + (newScale - currentScale) * CityConst.instance.PinchFactor;
//			}
//		}

		float delta = gesture.deltaPinch * Mathf.Clamp(gesture.deltaTime, 0f, 0.0333333f) * Mathf.Sqrt(2)*CityConst.instance.PinchFactor;
		float curPinchVal =  currentScale;
		curPinchVal -= delta;
		if (curPinchVal <= _minScale)
		{
			currentScale = _minScale;
		}
		else
		{
			currentScale = curPinchVal;
		}
		
	}

	public override void OnPinchIn(Gesture gesture)
	{
		_onPinch = true;
		_continueScroll = false;

		if (MapDraggableCamera.LOCK_PINCH)
			return;
		
		float delta = gesture.deltaPinch * Mathf.Clamp(gesture.deltaTime, 0f, 0.0333333f) * Mathf.Sqrt(2)*CityConst.instance.PinchFactor;
		float curPinchVal =  currentScale;
		curPinchVal += delta;
		if (curPinchVal >= _maxScale)
		{
			currentScale = _maxScale;
		}
		else
		{
			currentScale = curPinchVal;
		}
		


//		if (currentScale < _maxScale)
//		{
//			float newScale = _pinchStartScale / (gesture.twoFingerDistance / _pinchStartTwoFigerDis);
//			if (scaleInterceptor == null || !scaleInterceptor(_currentScale, newScale))
//			{
//				//currentScale = newScale * CityConst.instance.PinchFactor ;
//				currentScale = currentScale + (newScale - currentScale) * CityConst.instance.PinchFactor;
//			}
//		}
	}
	
	public override bool OnSwipe(Gesture gesture)
	{
		if (_onPinch)
			return false;
		
		mDraggableTarget?.Drag(gesture);	// 改用新的摄像机拖动算法
		return true;
	}

	private Vector2 _lastOffset = new Vector2();
	public override void OnSwipeStart(Gesture gesture)
	{
		if (_onPinch)
		{
			return;
		}
		
		mDraggableTarget?.Drag(gesture);		// 改用新的摄像机拖动算法
		
		_lastOffset.Set(0, 0);
		_onSwipe = true;
		_continueScroll = false;
		_scrollMoveContext.Clear ();
		DoDisturbLastAutoMoveCallback();
		DoDisturbLastAutoScaleCallback();

	}

	public override void OnTouchStart(Gesture gesture, ref bool isStopCameraMove)
	{
		mDraggableTarget?.Press(true);		// 改用新的摄像机拖动算法
		
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

	public override void OnTouchUp(Gesture gesture)
	{
		mDraggableTarget?.Press(false);		// 改用新的摄像机拖动算法
	}
	
	//镜头低比高可移动范围大
	public void SetLowCameraSwipeBound(float xMin, float yMin, float xMax, float yMax)
	{
		_boundXMinLow = xMin;
		_boundXMaxLow = xMax;
		_boundYMinLow = yMin;
		_boundYMaxLow = yMax;
	}
	
	public void SetCameraMomentumAmount(float value)
	{
		if (mDraggableTarget != null)
		{
			mDraggableTarget.momentumAmount = value;
		}
	}
	
	private float boundXMin, boundYMin, boundXMax, boundYMax;
	protected override bool CheckOutOfBound(ref Vector3 localPos)
	{
		float curScale = Mathf.Clamp(currentScale, _minScale, _maxScale);
		float ratio = (curScale-_minScale) / (_maxScale - _minScale);

		boundXMin = Mathf.Lerp(_boundXMinLow,_boundXMin, ratio);
		boundYMin = Mathf.Lerp(_boundYMinLow,_boundYMin, ratio);
		boundXMax = Mathf.Lerp(_boundXMaxLow,_boundXMax, ratio);
		boundYMax = Mathf.Lerp(_boundYMaxLow,_boundYMax, ratio);
		
		bool xOverflow = localPos.x < boundXMin || localPos.x > boundXMax;
		bool yOverflow = localPos.z < boundYMin || localPos.z > boundYMax;
		if (xOverflow || yOverflow)
		{
			if (xOverflow)
			{
				localPos.x = Mathf.Clamp(localPos.x, boundXMin, boundXMax);
			}

			if (yOverflow)
			{
				localPos.z = Mathf.Clamp(localPos.z, boundYMin, boundYMax);
			}

			return true;
		}

		return false;
	}

	protected void UpdateCameraPos(Vector3 targetPos)
	{
		float z = GetCurZOffset();
		base.SetCameraLPos(new Vector3(targetPos.x, 0, targetPos.z - z));
		_cameraTrans.LookAt(mTargetTrans);
	}
	//这里重写  修改相机中心目标点位置 反算相机位置
	public override void SetCameraLPos(Vector3 lpos)
	{
		SetTargetLPos(lpos);
		UpdateCameraPos(lpos);
	}
	
	public override Vector3 GetCameraLPos()
	{
		if (mTargetTrans == null) return Vector3.zero;
		return mTargetTrans.localPosition;
	}

	public override void SetFovRange(float minFov, float maxFov)
	{
		this.minFov = minFov;
		this.maxFov = maxFov;
	}


	protected override void InternalSetCameraFOV(float fov)
	{
		base.InternalSetCameraFOV(fov);
		// 缩放也影响跟随;
		//UITrackerManager.GetInstance().UpdateTrackersPosImmediately();
	}
	
	protected override void InternalSetCameraZValue(float oldY, float newY)
	{
		Vector3 cameraLocalPos = _cameraTrans.localPosition;
		Vector3 containerPos = _container.localPosition;
		containerPos.y = newY;
		_container.localPosition = containerPos;
		var localPosition = mTargetTrans.localPosition;
		cameraLocalPos.x = localPosition.x;
		_currentScale = newY;
		float z = GetCurZOffset();
		cameraLocalPos.z = localPosition.z - z;
		base.SetCameraLPos(cameraLocalPos);
		_cameraTrans.LookAt(mTargetTrans);
	}
	
	public override Vector3 GetCameraCenterWorldPos()
	{
		if (mTargetTrans == null)
		{
			return Vector3.zero;
		}
		return mTargetTrans.position;
	}

	private float GetCurZOffset()
	{
		MapCameraInfo endLevelInfo = _cameraInfos[currentLevel + 1];
		float ratio = (_currentScale - _currentCameraInfo.y) / (endLevelInfo.y - _currentCameraInfo.y);
		return Mathf.Lerp(_currentCameraInfo.z, endLevelInfo.z, ratio);
	}

	public void SetCameraFov(float newFov)
	{
		newFov = Mathf.Clamp(newFov, minFov, maxFov);
		this.InternalSetCameraFOV(newFov);
	}
	
	
	public override void MoveCameraToPosition(Vector3 worldPos, float duration = .2f, Action<bool, Vector3> callback = null)
	{
		_scrollMoveContext.Clear ();
		
		worldPos.y = 0;
		_scrollMoveContext.startPos = mTargetTrans.localPosition;
		_scrollMoveContext.endPos = mTargetTrans.parent.worldToLocalMatrix.MultiplyPoint3x4(worldPos);
		_scrollMoveContext.moveType = EScrollMoveType.PosChange;
		_scrollMoveContext.totalTime = duration;
		_continueScroll = true;
		
		_onMapMoveComplete = callback;
	}

	
	public override void MoveCameraQuickToPosition(Vector3 worldPos)
	{
		_continueScroll = false;
		_scrollMoveContext.Clear ();
		SetCameraLPos(mTargetTrans.parent.worldToLocalMatrix.MultiplyPoint3x4(worldPos));
		DoDisturbLastAutoMoveCallback();
	}
	
	protected override void InternalSetCurrentScale(float value)
	{
		base.InternalSetCurrentScale(value);

		InternalSetCameraStrength(Mathf.Lerp(_currentCameraInfo.strength, endLevelInfo.strength, ratio));
	}
	
	public void InternalSetCameraStrength(float strength)
	{
		if (mDraggableTarget != null) mDraggableTarget.SetStrength(strength);
	}
}
