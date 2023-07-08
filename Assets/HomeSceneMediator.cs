using System;
using System.Collections;
using System.Collections.Generic;
//using LuaInterface;
using UnityEngine;

public class HomeSceneMediator : SceneMediatorBase
{
    private string _curCityId = null;

    public string CurCityId
    {
        get
        {
            return _curCityId;
        }
    }

    private HomeSceneView _homeScene = null;
    public HomeSceneView HomeScene => _homeScene;

    private Transform _cameraTrans = null;
    //private new HomeSceneCameraScroller _cameraScroller;
    public override IMapCameraScroller cameraScroller
    {
        get
        {
            return _cameraScroller;
        }
    }

    public override MediaType curMediaType
    {
        get { return MediaType.Home; }
    }

    public void OnInitHomeStart()
    {
        if (_homeScene == null)
        {
            EasyTouch.instance.enableTwist = false;
            EasyTouch.instance.longTapTime = MapCalcConst.EASY_TOUCH_LONG_TAP_TIME;
            _homeScene = new HomeSceneView();
            if (MapAsset.instance != null)
            {
                CityConst.instance.UpdateCameraInfo(MapAsset.instance.cityCamAsset);
            }
        }
    }

    public void RefreshCameraParam(float worldPosX, float worldPosZ, int defaultLevel, float defaultScale = 0)
    {

        Vector3 defaultViewCenter = new Vector3(worldPosX, 0, worldPosZ);
        _cameraScroller.InitCameraPos(CameraManager.GetMainCamera(), defaultViewCenter, CityConst.instance.LOD_CAM_INFO_LIST_HOME, defaultLevel, defaultScale);
        _cameraScroller.scaleInterceptor = OnCameraScaleIntercept;
        if (MapAsset.instance != null)
        {
            UpdateCameraBound(MapAsset.instance.cityCamAsset);
        }
    }

    private HomeSceneCameraScroller GetCameraScroller()
    {
        if (_cameraScroller == null)
        {
            _cameraScroller = new HomeSceneCameraScroller();
        }

        return _cameraScroller as HomeSceneCameraScroller;
    }

    private bool OnCameraScaleIntercept(float curScale, float newScale)
    {
        return false;
    }

    private static Vector3 _cacheCameraCenter; // 离开时缓存当前镜头位置，下线和迁城清理
    private static Vector3 _cacheCameraScale; // 缓存镜头大小

    /// <summary>
    /// 缓存相机当前位置
    /// </summary>
    /// <param name="clear"></param>
    public void SaveCameraPos(bool clear = false)
    {
        if (clear)
        {
            _cacheCameraCenter.Set(0, 0, 0);
            return;
        }
        _cacheCameraCenter = _cameraScroller.GetCameraCenterWorldPos();
        _cacheCameraScale.x = _cameraScroller.currentScale - 0.01f;
        _cacheCameraScale.y = Mathf.Max(_cameraScroller.currentLevel, 1);
    }


    public void EnterHomeScene(string cityId, float worldPosX, float worldPosZ, int defaultLevel)
    {
        WorldMapManager.GetInstance().mCurActiveScene = this;
        OnInitHomeStart();
        InitCamera();
        float scale = 0;
        if (!_cacheCameraCenter.Equals(Vector3.zero))
        {

            scale = _cacheCameraScale.x;
            defaultLevel = (int)_cacheCameraScale.y;
        }
        AddEasyTouchListener();
        RefreshCameraParam(worldPosX, worldPosZ, defaultLevel, scale);
        _homeScene?.EnterHomeScene(cityId);
        _curCityId = cityId;
    }

    //监听主城交互
    public void EnterMainCityInteract()
    {
        WorldMapManager.instance.mMainCityMedia.OnEnterMainCityInteractMode(_homeScene, _cameraScroller);
    }

    public void LeaveHomeScene()
    {
        GetCameraScroller().ClearCamera();
        SaveCameraPos();
        RemoveEasyTouchListener();
        _homeScene?.LeaveHomeScene();
        WorldMapManager.instance.mMainCityMedia.OnLeaveMainCityInteractMode(_homeScene);
        _curCityId = null;
    }

    public void ActiveMainCityMode()
    {
        WorldMapManager.instance.mMainCityMedia.ActiveMainCityMode();
    }

    public void DisMainCityMode()
    {
        WorldMapManager.instance.mMainCityMedia.DisMainCityMode();
    }

    public void UpdateOneBuildingEraser(int xLeftBottom, int yLeftBottom, int xRightTop, int yRightTop, bool bVisible)
    {
        _homeScene?.UpdateOneBuildingEraser(xLeftBottom, yLeftBottom, xRightTop, yRightTop, bVisible);
    }

    private void InitCamera()
    {
        Camera mainCamera = CameraManager.GetMainCamera();
        // 相机添加了一个中间节点，用于震屏;
        _cameraTrans = mainCamera.transform.parent;
        _cameraScroller = GetCameraScroller();
    }

    public void UpdateCameraParamByAsset(CityCameraAsset asset)
    {
        UpdateCameraBound(asset);
        CityConst.instance.UpdateCameraInfo(asset);
    }

    private void UpdateCameraBound(CityCameraAsset asset)
    {
        if (asset == null)
        {
            return;
        }
        var camScroller = GetCameraScroller();
        camScroller.SetSwipeBound(asset.centerX - asset.highLeft, asset.centerY - asset.highBottom, asset.centerX + asset.highRight,
            asset.centerY + asset.highTop, false);
        camScroller.SetLowCameraSwipeBound(asset.centerX - asset.lowLeft, asset.centerY - asset.lowBottom,
            asset.centerX + asset.lowRight, asset.centerY + asset.lowTop);
        camScroller.SetCameraMomentumAmount(asset.momentumAmount);
    }

    public override void Update()
    {
        if (_curCityId == null)
        {
            return;
        }

        if (_cameraTrans == null)
        {
            return;
        }
        InvalidateCameraPos(_cameraTrans.localPosition);
    }

    private void InvalidateCameraPos(Vector3 cameraPosWhenStart)
    {
        Vector3 worldPos = _cameraTrans.position;
        _cameraScroller.CalculateCameraPos(out worldPos);
        FollowEntity();
    }



    #region EasyTouchHandlers
    // 这个是用来规避EasyTouch的一个问题
    // EasyTouch在两个手指操作了以后，如果先松开一个手指，再松开另一个手指的时候，就会触发没有松开手指的点击事件，显然我们不应该处理该次点击
    private Touch2FingerCancelStatus _touch2FingerCancelStatus = Touch2FingerCancelStatus.None;
    // tap的时候，如果是先停止了移动，则不会触发此次tap
    private bool _justStopCameraMoveWhenSimpleTap = false;
    private bool _hasAddEasyTouchListener = false;

    private void AddEasyTouchListener()
    {
        if (!_hasAddEasyTouchListener)
        {
            _hasAddEasyTouchListener = true;
            EasyTouch.On_Swipe += EasyTouch_On_Swipe;
            EasyTouch.On_PinchOut += EasyTouch_On_PinchOut;
            EasyTouch.On_PinchIn += EasyTouch_On_PinchIn;
            EasyTouch.On_TouchStart2Fingers += EasyTouch_On_TouchStart2Fingers;
            EasyTouch.On_SimpleTap += EasyTouch_On_SimpleTap;
            EasyTouch.On_SwipeStart += EasyTouch_On_SwipeStart;
            EasyTouch.On_PinchEnd += EasyTouch_On_PinchEnd;
            EasyTouch.On_TouchStart += EasyTouch_On_TouchStart;
            EasyTouch.On_SwipeEnd += EasyTouch_On_SwipeEnd;
            EasyTouch.On_DragEnd += EasyTouch_On_DragEnd;
            EasyTouch.On_Cancel2Fingers += EasyTouch_On_Cancel2Fingers;
            EasyTouch.On_TouchUp += EasyTouch_On_TouchUp;

            //EasyTouch.instance.nGUILayers = LayerUtils.UILayer;
        }
    }
    private void RemoveEasyTouchListener()
    {
        if (_hasAddEasyTouchListener)
        {
            _hasAddEasyTouchListener = false;
            EasyTouch.On_DragEnd -= EasyTouch_On_DragEnd;
            EasyTouch.On_Swipe -= EasyTouch_On_Swipe;
            EasyTouch.On_PinchOut -= EasyTouch_On_PinchOut;
            EasyTouch.On_PinchIn -= EasyTouch_On_PinchIn;
            EasyTouch.On_TouchStart2Fingers -= EasyTouch_On_TouchStart2Fingers;
            EasyTouch.On_SimpleTap -= EasyTouch_On_SimpleTap;
            EasyTouch.On_SwipeStart -= EasyTouch_On_SwipeStart;
            EasyTouch.On_PinchEnd -= EasyTouch_On_PinchEnd;
            EasyTouch.On_TouchStart -= EasyTouch_On_TouchStart;
            EasyTouch.On_SwipeEnd -= EasyTouch_On_SwipeEnd;
            EasyTouch.On_Cancel2Fingers -= EasyTouch_On_Cancel2Fingers;
            EasyTouch.On_TouchUp -= EasyTouch_On_TouchUp;
        }
    }

    void EasyTouch_On_SimpleTap(Gesture gesture)
    {
        if (_touch2FingerCancelStatus == Touch2FingerCancelStatus.SkipNextSimpleTap)
        {
            _touch2FingerCancelStatus = Touch2FingerCancelStatus.None;
            return;
        }
    }


    void EasyTouch_On_PinchOut(Gesture gesture)
    {
        //DebugUtil.Log("EasyTouch_On_PinchOut " + gesture);
        _cameraScroller?.OnPinchOut(gesture);
    }

    void EasyTouch_On_PinchIn(Gesture gesture)
    {
        //DebugUtil.Log("EasyTouch_On_PinchIn " + gesture);
        _cameraScroller?.OnPinchIn(gesture);
    }

    void EasyTouch_On_TouchStart2Fingers(Gesture gesture)
    {
        //DebugUtil.Log("EasyTouch_On_TouchStart2Fingers " + gesture);
        _cameraScroller?.OnTouchStart2Fingers(gesture);
    }


    void EasyTouch_On_PinchEnd(Gesture gesture)
    {
        //DebugUtil.Log("EasyTouch_On_PinchEnd " + gesture);
        _cameraScroller?.OnPinchEnd(gesture);
    }

    void EasyTouch_On_Swipe(Gesture gesture)
    {
        //DebugUtil.Log("EasyTouch_On_Swipe " + gesture);
        _cameraScroller?.OnSwipe(gesture);
    }

    void EasyTouch_On_SwipeStart(Gesture gesture)
    {
        //DebugUtil.Log("EasyTouch_On_SwipeStart " + gesture);
        _cameraScroller?.OnSwipeStart(gesture);
    }

    void EasyTouch_On_TouchStart(Gesture gesture)
    {
        if (_cameraScroller == null)
        {
            return;
        }
        //DebugUtil.Log("EasyTouch_On_TouchStart " + gesture);
        _justStopCameraMoveWhenSimpleTap = false;
        _cameraScroller.OnTouchStart(gesture, ref _justStopCameraMoveWhenSimpleTap);

        if (_touch2FingerCancelStatus == Touch2FingerCancelStatus.JustReleaseOneFinger)
        {
            _touch2FingerCancelStatus = Touch2FingerCancelStatus.SkipNextSimpleTap;
        }
        else
        {
            _touch2FingerCancelStatus = Touch2FingerCancelStatus.None;
        }
    }

    void EasyTouch_On_TouchUp(Gesture gesture)
    {
        _cameraScroller?.OnTouchUp(gesture);
    }

    public void EasyTouch_On_SwipeEnd(Gesture gesture)
    {
        //DebugUtil.Log("EasyTouch_On_SwipeEnd " + gesture);
        _cameraScroller?.OnSwipeEnd(gesture);
    }

    void EasyTouch_On_DragEnd(Gesture gesture)
    {
        //Debug.Log("EasyTouch_On_DragEnd " + gesture);
        EasyTouch_On_SwipeEnd(gesture);
    }

    void EasyTouch_On_Cancel2Fingers(Gesture gesture)
    {
        //DebugUtil.Log("EasyTouch_On_Cancel2Fingers " + gesture);
        EasyTouch.SetEnabled(true); //重置一下touch
        if (_cameraScroller != null)
        {
            _cameraScroller.Reset();
        }

        if (gesture.touchCount > 0)
        {
            // 解决点击
            _touch2FingerCancelStatus = Touch2FingerCancelStatus.JustReleaseOneFinger;
        }
    }

    #endregion

    public void DoCameraFocusOnCity(int x, int y, Action<bool, Vector3> callback = null)
    {
        //Vector3 buildingPos = CityUtils.CityGridPosToWorldPos(x, y, _homeScene.gridPivotTrans);
        //_cameraScroller.MoveCameraToPosition(buildingPos, 0.4f, callback);
    }

    public void DoCameraFocusOnCity(Vector3 buildingPos, Action<bool, Vector3> callback = null, float duration = 0.4f)
    {
        _cameraScroller.MoveCameraToPosition(buildingPos, duration, callback);
    }

    public Vector2 CameraCenterToCityGridPos()
    {
        //Vector2 ret = Vector2.one * -1;
        //if (cameraScroller == null || _homeScene == null)
        //{
        //    return ret;
        //}

        //var scroller = GetCameraScroller();
        //Vector3 worldPos = scroller.GetCameraCenterWorldPos();
        //CityUtils.WorldPostoCityGridPos(worldPos, _homeScene.GetGridPivotRoot(), out int x, out int y);
        //ret.x = x;
        //ret.y = y;
        //return ret;

        return Vector3.zero;
    }

    public Vector3 CameraCenterWorldPos()
    {
        Vector2 ret = Vector2.zero;
        if (cameraScroller == null || _homeScene == null)
        {
            return ret;
        }
        var scroller = GetCameraScroller();
        return scroller.GetCameraCenterWorldPos();
    }

    public Vector3 CameraCenterLocalPos()
    {
        //Vector3 ret = Vector3.zero;
        //if (cameraScroller == null || _homeScene == null)
        //{
        //    return ret;
        //}
        //Vector3 worldPos = CameraCenterWorldPos();
        //CityUtils.WorldPosToCityNodeLocalPos(worldPos, _homeScene.GetGridPivotRoot(), ref ret);
        //return ret;

        return Vector3.zero;
    }


    public void OnRemove()
    {
        //LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnHomeSceneRemove");
        //if (luaFunc != null)
        //{
        //    luaFunc.Call();
        //}
    }
}
