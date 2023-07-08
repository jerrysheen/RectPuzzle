using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum Touch2FingerCancelStatus
{
    None,
    JustReleaseOneFinger,
    SkipNextSimpleTap,
}

/// <summary>
/// 沙盘模式得地图操作逻辑;
/// </summary>
public class MapSceneMediatorBase : SceneMediatorBase
{
    //protected bool _inited = false;
    
    //private int _curGridY;
    //private int _curGridX;
    //private int _curVisibleCenterY;
    //private int _curVisibleCenterX;
    //private int _lod = 0;
    //protected float _viewDistance = -1;
    //private RectInt _visibleRect;
    
    //protected Transform _cameraTrans = null;
    //private Vector3 _oldCameraPos;
    
    //private int _followTroopId = -1;
    //private int _followState = -1;
    
    //private Action<float, int> _viewDistanceChangeDelegate;
    //protected Transform _itemContainer;
    //private bool _inWorld;
    
    //private int _minLod = 0;
    //private int _maxLod = 0;

    //// 这个是用来规避EasyTouch的一个问题
    //// EasyTouch在两个手指操作了以后，如果先松开一个手指，再松开另一个手指的时候，就会触发没有松开手指的点击事件，显然我们不应该处理该次点击
    //private Touch2FingerCancelStatus _touch2FingerCancelStatus = Touch2FingerCancelStatus.None;

    //// tap的时候，如果是先停止了移动，则不会触发此次tap
    //private bool _justStopCameraMoveWhenSimpleTap = false;
    
    //private bool _hasAddEasyTouchListener = false;
    
    //public int FollowTroopId
    //{
    //    set
    //    {
    //        if (_followTroopId != value)
    //        {
    //            _followTroopId = value;
    //        }
    //        _followState = _inWorld ? 1 : -1;
    //    }
    //}
    
    //public Transform itemContainer
    //{
    //    //get
    //    //{
    //    //    if (_itemContainer == null)
    //    //    {
    //    //        _itemContainer = MapRoot.AddMapObject("ItemContainer").transform;
    //    //    }
    //    //    return _itemContainer;
    //    //}
    //}
    
    //protected DefaultMapViewPortChangedHandler _viewportChangeHandler;
    
    //private void InitRoot()
    //{
    //    //if (MapRoot.go == null)
    //    //{
    //    //    GameObject mapRoot = new GameObject("MapRoot");
    //    //    mapRoot.AddComponent<MapRoot>();
    //    //    MapRoot.go = mapRoot;
    //    //}
    //}
    
    //protected virtual void ShowWorldMap()
    //{
    //    //if (_inited)
    //    //{
    //    //    return;
    //    //}
    //    //_inited = true;
    //    //if (_itemContainer == null)
    //    //{
    //    //    _itemContainer = MapRoot.AddMapObject("ItemContainer").transform;
    //    //}
        
    //    //_viewportChangeHandler = new DefaultMapViewPortChangedHandler(new IMapDisplayDataProvider[MapCalcConst.LOD_NUM]{
    //    //    new MapLevel0DisplayDataProvider(),
    //    //    new MapLevel1DisplayDataProvider(),
    //    //    new MapLevel2DisplayDataProvider(),
    //    //    new MapLevel3DisplayDataProvider(),
    //    //    new MapLevel4DisplayDataProvider(),
    //    //    new MapLevel5DisplayDataProvider(),
    //    //    new MapLevel6DisplayDataProvider(),
    //    //    new MapLevel7DisplayDataProvider(),
    //    //    new MapLevel8DisplayDataProvider()
    //    //});

    //    //_viewportChangeHandler.RegisterMapObjectRenderManager(MapObjectDisplayType.MOUNTAIN, new DefaultMapMountainRenderManager(this));
    //}
    
    //#region EasyTouchHandlers
    
    //private Vector3 _worldPos;
    //private int _globalIndex;
    //private Vector2Int _globalGridPos;
    
    //private bool GetInputContext(Vector2 screenInput,out Vector2Int globalGridPos,out Vector3 worldPos,out int globalIndex)
    //{
    //    if (_cameraScroller.mainCamera && MapUtils.GetGlobalGridPosFromScreenPos(screenInput, out globalGridPos))
    //    {
    //        globalIndex = MapUtils.GlobalGridPosToGlobalIndex(globalGridPos.x,globalGridPos.y);
    //        worldPos = CameraUtil.GetWorldPos(_cameraScroller.mainCamera, screenInput);
    //        return true;
    //    }
    //    worldPos = Vector3.zero;
    //    globalIndex = 0;
    //    globalGridPos = Vector2Int.zero;
    //    return false;
    //}
    
    //// 通知lua层点击事件
    //private void _TryTouch(Gesture gesture, EasyTouch.EventName type)
    //{
    //    if (!GetInputContext(gesture.position, out _globalGridPos, out _worldPos, out _globalIndex))
    //    {
    //        return;
    //    }

    //    //屏蔽多手指操作对于lua侧的影响，如果后续有需求，可以把touchCount作为参数传入
    //    if (gesture.touchCount>1)
    //    {
    //        return;
    //    }
    //    LuaFunction func = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnTouchMap");
    //    if (func != null)
    //    {
    //        func.Call(_worldPos.x, _worldPos.z,  _globalGridPos.x, _globalGridPos.y, (int)type, gesture.position.x, gesture.position.y);
    //    }
    //}
    
    //private void EasyTouch_On_DoubleTap(Gesture gesture)
    //{
    //    if (gesture.isOverGui) return;
    //    _TryTouch(gesture, EasyTouch.EventName.On_DoubleTap);
    //}
    
    //private void EasyTouch_On_SimpleTap(Gesture gesture)
    //{
    //    if (gesture.isOverGui) return;
    //    if (_touch2FingerCancelStatus == Touch2FingerCancelStatus.SkipNextSimpleTap)
    //    {
    //        _touch2FingerCancelStatus = Touch2FingerCancelStatus.None;
    //        return;
    //    }
    //    _TryTouch(gesture, EasyTouch.EventName.On_SimpleTap);
    //}
    
    //private void EasyTouch_On_PinchOut(Gesture gesture)
    //{
    //    //DebugUtil.Log("EasyTouch_On_PinchOut " + gesture);
    //    _cameraScroller?.OnPinchOut(gesture);
    //}

    //private void EasyTouch_On_PinchIn(Gesture gesture)
    //{
    //    //DebugUtil.Log("EasyTouch_On_PinchIn " + gesture);
    //    _cameraScroller?.OnPinchIn(gesture);
    //}

    //private void EasyTouch_On_TouchStart2Fingers(Gesture gesture)
    //{
    //    //DebugUtil.Log("EasyTouch_On_TouchStart2Fingers " + gesture);
    //    _cameraScroller?.OnTouchStart2Fingers(gesture);
    //}
    
    //private void EasyTouch_On_PinchEnd(Gesture gesture)
    //{
    //    //DebugUtil.Log("EasyTouch_On_PinchEnd " + gesture);
    //    _cameraScroller?.OnPinchEnd(gesture);
    //}
    
    //private void EasyTouch_On_Swipe(Gesture gesture)
    //{
    //    //Debug.LogError("EasyTouch_On_Swipe " + gesture);
    //    _cameraScroller?.OnSwipe(gesture);
    //    _TryTouch(gesture, EasyTouch.EventName.On_Swipe);
    //    if (MapDraggableCamera.LOCK_DRAG) // 行军状态下是锁定地图拖动的
    //    {
    //        EdgeMovement(gesture); // 不过如果滑动到地图边缘还是会触发自动移动地图
    //    }
    //}

    //private void EasyTouch_On_SwipeStart(Gesture gesture)
    //{
    //    //DebugUtil.Log("EasyTouch_On_SwipeStart " + gesture);

    //    _followTroopId = -1;
    //    _cameraScroller?.OnSwipeStart(gesture);
    //    _TryTouch(gesture, EasyTouch.EventName.On_SwipeStart);
    //}

    //private void EasyTouch_On_TouchStart(Gesture gesture)
    //{
    //    if (_cameraScroller == null || gesture.touchCount > 1) // 多点触摸直接取消这个消息，防止bug
    //    {
    //        return;
    //    }

    //    _followTroopId = -1;
    //    //DebugUtil.Log("EasyTouch_On_TouchStart " + gesture);
    //    _justStopCameraMoveWhenSimpleTap = false;
    //    _cameraScroller.OnTouchStart(gesture, ref _justStopCameraMoveWhenSimpleTap);

    //    if (_touch2FingerCancelStatus == Touch2FingerCancelStatus.JustReleaseOneFinger)
    //    {
    //        _touch2FingerCancelStatus = Touch2FingerCancelStatus.SkipNextSimpleTap;
    //    }
    //    else
    //    {
    //        _touch2FingerCancelStatus = Touch2FingerCancelStatus.None;
    //    }
    //    _TryTouch(gesture, EasyTouch.EventName.On_TouchStart);
    //}

    //private void EasyTouch_On_TouchUp(Gesture gesture)
    //{
    //    _cameraScroller?.OnTouchUp(gesture);
    //    _TryTouch(gesture, EasyTouch.EventName.On_TouchUp);
    //}
    
    //void EasyTouch_On_DragEnd(Gesture gesture)
    //{
    //    //Debug.Log("EasyTouch_On_DragEnd " + gesture);
    //    EasyTouch_On_SwipeEnd(gesture);
    //    // _troopManager.On_DragEnd(gesture);
    //    _TryTouch(gesture, EasyTouch.EventName.On_DragEnd);
    //}

    //void EasyTouch_On_DragStart(Gesture gesture)
    //{
    //    //EasyTouch_On_SwipeStart(gesture);
    //    //_troopManager.On_DragStart(gesture);
    //    _TryTouch(gesture, EasyTouch.EventName.On_DragStart);
    //}

    //void EasyTouch_On_Drag(Gesture gesture)
    //{
    //    //_troopManager.On_Drag(gesture);
    //    _TryTouch(gesture, EasyTouch.EventName.On_Drag);
    //}
    
    //void EasyTouch_On_Cancel2Fingers(Gesture gesture)
    //{
    //    //DebugUtil.Log("EasyTouch_On_Cancel2Fingers " + gesture);
    //    EasyTouch.SetEnabled(true); //重置一下touch
    //    if (_cameraScroller != null)
    //    {
    //        _cameraScroller.Reset();
    //    }

    //    if (gesture.touchCount > 0)
    //    {
    //        // 解决点击
    //        _touch2FingerCancelStatus = Touch2FingerCancelStatus.JustReleaseOneFinger;
    //    }
    //}

    //void EasyTouch_On_LongTapStart(Gesture gesture)
    //{
    //    _TryTouch(gesture, EasyTouch.EventName.On_LongTapStart);
    //}

    //void EasyTouch_On_LongTap(Gesture gesture)
    //{
    //    _TryTouch(gesture, EasyTouch.EventName.On_LongTap);
    //}

    //void EasyTouch_On_LongTapEnd(Gesture gesture)
    //{
    //    //_selectingMapObjFrame.circling.gameObject.SetActive(false);
    //    _TryTouch(gesture, EasyTouch.EventName.On_LongTapEnd);
    //}
    
    //void EasyTouch_On_SwipeEnd(Gesture gesture)
    //{
    //    //Debug.LogError("EasyTouch_On_SwipeEnd " + gesture);
    //    _cameraScroller?.OnSwipeEnd(gesture);
    //    _TryTouch(gesture, EasyTouch.EventName.On_SwipeEnd);
    //}

    //private void AddEasyTouchListener()
    //{
    //    if (!_hasAddEasyTouchListener)
    //    {
    //        _hasAddEasyTouchListener = true;
    //        EasyTouch.On_DoubleTap += EasyTouch_On_DoubleTap;
    //        EasyTouch.On_Drag += EasyTouch_On_Drag;
    //        EasyTouch.On_Swipe += EasyTouch_On_Swipe;
    //        EasyTouch.On_PinchOut += EasyTouch_On_PinchOut;
    //        EasyTouch.On_PinchIn += EasyTouch_On_PinchIn;
    //        EasyTouch.On_TouchStart2Fingers += EasyTouch_On_TouchStart2Fingers;
    //        EasyTouch.On_SimpleTap += EasyTouch_On_SimpleTap;
    //        EasyTouch.On_SwipeStart += EasyTouch_On_SwipeStart;
    //        EasyTouch.On_PinchEnd += EasyTouch_On_PinchEnd;
    //        EasyTouch.On_TouchStart += EasyTouch_On_TouchStart;
    //        EasyTouch.On_SwipeEnd += EasyTouch_On_SwipeEnd;
    //        EasyTouch.On_DragStart += EasyTouch_On_DragStart;
    //        EasyTouch.On_DragEnd += EasyTouch_On_DragEnd;
    //        EasyTouch.On_Cancel2Fingers += EasyTouch_On_Cancel2Fingers;
    //        EasyTouch.On_TouchUp += EasyTouch_On_TouchUp;
    //        EasyTouch.On_LongTapStart += EasyTouch_On_LongTapStart;
    //        EasyTouch.On_LongTap += EasyTouch_On_LongTap;
    //        EasyTouch.On_LongTapEnd += EasyTouch_On_LongTapEnd;
            
    //        EasyTouch.instance.nGUILayers = LayerUtils.UILayer;
    //    }
    //}
    //private void RemoveEasyTouchListener()
    //{
    //    if (_hasAddEasyTouchListener)
    //    {
    //        _hasAddEasyTouchListener = false;
    //        EasyTouch.On_DoubleTap -= EasyTouch_On_DoubleTap;
    //        EasyTouch.On_DragStart -= EasyTouch_On_DragStart;
    //        EasyTouch.On_DragEnd -= EasyTouch_On_DragEnd;
    //        EasyTouch.On_Drag -= EasyTouch_On_Drag;
    //        EasyTouch.On_Swipe -= EasyTouch_On_Swipe;
    //        EasyTouch.On_PinchOut -= EasyTouch_On_PinchOut;
    //        EasyTouch.On_PinchIn -= EasyTouch_On_PinchIn;
    //        EasyTouch.On_TouchStart2Fingers -= EasyTouch_On_TouchStart2Fingers;
    //        EasyTouch.On_SimpleTap -= EasyTouch_On_SimpleTap;
    //        EasyTouch.On_SwipeStart -= EasyTouch_On_SwipeStart;
    //        EasyTouch.On_PinchEnd -= EasyTouch_On_PinchEnd;
    //        EasyTouch.On_TouchStart -= EasyTouch_On_TouchStart;
    //        EasyTouch.On_SwipeEnd -= EasyTouch_On_SwipeEnd;
    //        EasyTouch.On_Cancel2Fingers -= EasyTouch_On_Cancel2Fingers;
    //        EasyTouch.On_TouchUp -= EasyTouch_On_TouchUp;
    //        EasyTouch.On_LongTapStart -= EasyTouch_On_LongTapStart;
    //        EasyTouch.On_LongTap -= EasyTouch_On_LongTap;
    //        EasyTouch.On_LongTapEnd -= EasyTouch_On_LongTapEnd;
    //    }
    //}
    //#endregion

    //protected virtual void InitCamera()
    //{
    //    Camera mainCamera = CameraManager.GetMainCamera();
    //    _cameraTrans = mainCamera.transform.parent;
    //    _cameraScroller = new MapSceneCameraScroller();
    //    //_cameraScroller = GetCameraScroller();
    //    _viewDistance = -1;
    //    _oldCameraPos = Vector3.zero;
    //}
    
    //protected virtual void OnInitMapStart() {}
    
    //protected void SetVisibleRect(RectInt value)
    //{
    //    _visibleRect = value;
    //}

    //private bool OnCameraScaleIntercept(float curScale, float newScale)
    //{
    //    return false;
    //}
    
    
    //private void RefreshCameraParam(float globalPosX, float globalPosY, int lodLevel, float defaultScale,int lodId)
    //{
    //    AssetRequest request = AssetsManager.LoadAssetSync("lod/" + lodId,ResourceType.ScriptObject, result =>
    //    {
    //        if (result == null || result.isError())
    //        {
    //            Debug.LogError("RefreshCameraParam load lod asset is error!!!");
    //        }
    //    });
    //    CameraLodParam list = request.asset as CameraLodParam;
    //    request.Release();
    //    // 特别注意 这里的最大和最小lod的层级代表lod在数组的下标值 不是实际的lod
    //    _minLod = Math.Max(0, list.lodLayerRange[0] - 1);
    //    _maxLod = Math.Min(MapCalcConst.LOD_NUM - 1, list.lodLayerRange[1] - 2);
    //    Vector3 defaultViewCenter = new Vector3(globalPosX,0,globalPosY);
    //    _cameraScroller.InitCameraPos(CameraManager.GetMainCamera(), defaultViewCenter, MapCalcConst.LOD_CAM_INFO_LIST(list), lodLevel, defaultScale);
    //    _cameraScroller.scaleInterceptor = OnCameraScaleIntercept;
    //    _cameraScroller.SetMinMaxLod(_minLod, _maxLod);
        
    //    CalculateGridByCamera(out _curGridX, out _curGridY);
        
    //    _lod = Mathf.Clamp(_cameraScroller.currentLevel, _minLod, _maxLod);
    //}
    
    //protected virtual void CalculateGridByCamera(out int gridX, out int gridY)
    //{
    //    MapUtils.CalculateGridByWorldCamera(_cameraScroller.mainCamera, out gridX, out gridY);
    //}
    
    //protected virtual void InvalidateMapChange(int gridX, int gridY, int lod, float viewDistance) 
    //{ }

    //protected void CheckScreenPosUpdate(int newGridX, int newGridY, float newViewDistance, int newLod,
    //    bool forceUpdate = false)
    //{
    //    bool posChanged = newGridY != _curGridY || newGridX != _curGridX;
    //    bool viewDistanceChanged = (int)(newViewDistance * 10000) != (int)(_viewDistance * 10000);
    //    if (posChanged)
    //    {
    //        _curGridX = newGridX;
    //        _curGridY = newGridY;
    //    }

    //    if (viewDistanceChanged || forceUpdate)
    //    {
    //        _viewDistance = newViewDistance;
    //    }
        
    //    _lod = newLod;

    //    bool bothViewDistanceAndPosChanged = posChanged || viewDistanceChanged;

    //    bool viewRectChanged = false;

    //    if (bothViewDistanceAndPosChanged)
    //    {
    //        int newWidth, newHeight;
    //        MapCalcConst.CalcCurrentViewRectSize(_cameraScroller.currentScale, newLod, out newWidth, out newHeight);
    //        RectInt newViewRect = new RectInt(_curGridX - newWidth / 2, _curGridY - newHeight / 2, newWidth, newHeight);
    //        viewRectChanged = !newViewRect.Equals(_visibleRect);
    //        if (viewRectChanged)
    //        {
    //            SetVisibleRect(newViewRect);
    //        }
    //    }

    //    // 精度?
    //    int cacheSizeX = _viewDistance < MapCalcConst.world_cache_size_height ? 1 : 3;
    //    int cacheSizeY = _viewDistance < MapCalcConst.world_cache_size_height ? 1 : 3;
    //    bool needRefreshSceneDisplay = forceUpdate ||
    //                                   (Mathf.Abs(newGridY - _curVisibleCenterY) > cacheSizeY ||
    //                                    Mathf.Abs(newGridX - _curVisibleCenterX) > cacheSizeX) ||
    //                                   viewRectChanged;
        
    //    /// 精度问题，需进一步更新LookAtPos;
    //    Vector3 curCameraPos = _cameraTrans.position;
    //    if (!needRefreshSceneDisplay && _cameraTrans != null &&
    //        !MathGUtils.Vector3Equal(_oldCameraPos, curCameraPos))
    //    {
    //        _oldCameraPos = curCameraPos;
    //        LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.RefreshLookAtPos");
    //        if (luaFunc != null)
    //        {
    //            float lookAtX = curCameraPos.x;
    //            float lookAtZ = curCameraPos.z + curCameraPos.y;
    //            luaFunc.Call(lookAtX, lookAtZ);
    //        }
    //    }
        
    //    if (bothViewDistanceAndPosChanged)
    //    {
    //        InvalidateMapChange(newGridX, newGridY, _lod, _viewDistance);
    //    }

    //    if (needRefreshSceneDisplay)
    //    {
    //        _curVisibleCenterY = newGridY;
    //        _curVisibleCenterX = newGridX;
    //        _oldCameraPos = curCameraPos;

    //        if (_viewportChangeHandler != null && _cameraTrans != null)
    //        {
    //            _viewportChangeHandler.OnViewportChange(_visibleRect, _viewDistance, _lod, curCameraPos);
    //        }
    //    }
    //    else if (viewDistanceChanged && _viewportChangeHandler != null)
    //    {
    //        _viewportChangeHandler.OnViewDistanceChange(_viewDistance, _lod);
    //    }

    //    if ((viewDistanceChanged) && _viewDistanceChangeDelegate != null)
    //    {
    //        _viewDistanceChangeDelegate(_viewDistance, _lod);
    //    }

    //    if (viewDistanceChanged)
    //    {
    //        CameraManager.s_mapHeight =  _viewDistance;
    //        UITrackerManager.GetInstance().UpdateTrackersPosImmediately();
    //    }
    //}
    
    //protected void FollowTroop()
    //{
    //    // 军队跟随
    //    if (_followTroopId > 0)
    //    {
    //        EntityBase entityBase = EntityManager.GetInstance().GetEntity(_followTroopId);
    //        if (entityBase != null)
    //        {
    //            if (_followState < 0) // 开启跟随
    //            {
    //                _cameraScroller.MoveCameraToPositionWithScale(entityBase.position, MapCalcConst.world_default_height, 0.2f, (bool value, Vector3 pos) => { _followState = 1; });
    //                _followState = 0; // 暂时停止跟随
    //            }
    //            else if (_followState > 0) // 只是跟随位置
    //            {
    //                _cameraScroller.MoveCameraToPosition(entityBase.position);
    //            }
    //        }
    //    }
    //}
    
    //private void InvalidateCameraPos()
    //{
    //    if (_cameraScroller == null) { return; }

    //    FollowTroop();
        
    //    // 修正相机位置
    //    Vector3 worldPos = _cameraTrans.position;
    //    _cameraScroller.CalculateCameraPos(out worldPos);

    //    int newGridRow, newGridCol;
    //    CalculateGridByCamera(out newGridRow, out newGridCol);
        
    //    float currentScale = _cameraScroller.currentScale;
    //    int newLod = Mathf.Clamp(_cameraScroller.currentLevel, _minLod, _maxLod);
    //    CheckScreenPosUpdate(newGridRow, newGridCol, currentScale, newLod);
    //}
    
    //protected override float GetEdgeMoveFactor()
    //{
    //    return MapPlaceConfig.MAP_FIX_SCALE * 0.1f * Mathf.Max(0.5f, Mathf.Log(_cameraScroller.currentScale, 2));
    //}

    //protected override float GetCameraMoveSpeed()
    //{
    //    return 0.8f;
    //}
    
    ///// <summary>
    ///// 进入资源相关初始化;
    ///// </summary>
    //private void DoEnterSceneLogic()
    //{
    //    InitRoot();
    //    OnInitMapStart();
    //    ShowWorldMap();
    //    //EnterMapEnd();
    //}

    ///// <summary>
    ///// 坐标初始化;
    ///// </summary>
    ///// <param name="globalPosX"></param>
    ///// <param name="globalPosY"></param>
    ///// <param name="lod"></param>
    ///// <param name="defaultScale">Y</param>
    //public void OnEnterWorldMap(float globalPosX, float globalPosY, int lod, float defaultScale, int lodId = 1001)
    //{
    //    WorldMapManager.GetInstance().mCurActiveScene = this;
        
    //    DoEnterSceneLogic();

    //    InitCamera();
        
    //    _visibleRect.x = _visibleRect.y = _visibleRect.width = _visibleRect.height = 0;
    //    RefreshCameraParam(globalPosX, globalPosY, lod, defaultScale, lodId);
    //    AddEasyTouchListener();
    //    CheckScreenPosUpdate(_curGridX, _curGridY, defaultScale, _lod);
    //    InvalidateMapChange(_curGridX, _curGridY, _lod, _viewDistance);
    //    _inWorld = true;
    //    LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnEnterWorldFinish");
    //    luaFunc?.Call();
    //} 
    
    //public virtual void OnRemove()
    //{
    //    _viewDistanceChangeDelegate = null;
    //    RemoveEasyTouchListener();
    //}

    //public bool GetGridXY(Vector2 screenPos, ref int gridX, ref int gridY)
    //{
    //    Vector2Int GridPos;
    //    if (_cameraScroller.mainCamera != null && MapUtils.GetGlobalGridPosFromScreenPos(screenPos, out GridPos))
    //    {
    //        Vector3 worldPos = CameraUtil.GetWorldPos(_cameraScroller.mainCamera, screenPos);
    //        gridX = (int)worldPos.x;
    //        gridY = (int)worldPos.z;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    
    //public override void Update()
    //{
    //    if (!_inWorld)
    //    {
    //        return;
    //    }
    //    if (_cameraScroller == null || _cameraTrans == null)
    //    {
    //        return;
    //    }
    //    InvalidateCameraPos();
    //}
    
    //public void OnMoveCameraToGrid(int x,int y, Action<bool, Vector3> callBack = null)
    //{
    //    Vector2Int gridPos = new Vector2Int(x,y);
    //    _cameraScroller.MoveCameraToGrid(gridPos, 0.4f, callBack);
    //}
    
    //public virtual void OnLeaveWorldMap()
    //{
    //    if(!_inWorld) return;
        
    //    RemoveEasyTouchListener();
    //    _inWorld = false;
    //    _followTroopId = -1;
    //    LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnLeaveWorldFinish");
    //    luaFunc?.Call();
    //    cameraScroller.ClearCamera();
    //}
}
