using System;
using System.Collections.Generic;
using System.Configuration;
//using ELEX.Config;
//using LuaInterface;
using UnityEngine;

public class MainCityMediator:SceneMediatorBase
{
    public const string NAME = "MainCityMediator";
    private HomeSceneView _mainCityView;

    // 这个是用来规避EasyTouch的一个问题
    // EasyTouch在两个手指操作了以后，如果先松开一个手指，再松开另一个手指的时候，就会触发没有松开手指的点击事件，显然我们不应该处理该次点击
    private Touch2FingerCancelStatus _touch2FingerCancelStatus = Touch2FingerCancelStatus.None;
    // tap的时候，如果是先停止了移动，则不会触发此次tap
    private bool _justStopCameraMoveWhenSimpleTap = false;
    
    private enum PlaceBuildingMode
    {
        Invalid,
        Build,
        Move,
        Rotate,
    }

    private bool _addEvent = false;
    
    public void OnEnterMainCityInteractMode(HomeSceneView view,MapSceneCameraScroller cameraScroller)
    {
        _cameraScroller = cameraScroller;
        ActiveMainCityMode();
        _mainCityView = view;
    }

    public void ActiveMainCityMode()
    {
        AddEasyTouchEvent();
        AddCommonEvent();
    }

    public void OnLeaveMainCityInteractMode(object obj)
    {
        _cameraScroller = null;
        DisMainCityMode();
    }

    public void DisMainCityMode()
    {
        RemoveEasyTouchEvent();
        RemoveCommonEvent();
    }

    public void OnRemove()
    {
        LeaveBuildingDragMode();
        RemoveEasyTouchEvent();
        RemoveCommonEvent();
    }
    
    #region PlayerControllInput

    private void AddCommonEvent()
    {
        EasyTouch.On_PinchOut += EasyTouch_On_PinchOut;
        EasyTouch.On_PinchIn += EasyTouch_On_PinchIn;
        EasyTouch.On_PinchEnd += EasyTouch_On_PinchEnd;
    }
    
    private void RemoveCommonEvent()
    {
        EasyTouch.On_PinchOut -= EasyTouch_On_PinchOut;
        EasyTouch.On_PinchIn -= EasyTouch_On_PinchIn;
        EasyTouch.On_PinchEnd -= EasyTouch_On_PinchEnd;
    }

    private void AddEasyTouchEvent()
    {
        Debug.Log("Add Event");
        if ( ! _addEvent )
        {
            _addEvent = true;
            EasyTouch.On_SimpleTap += EasyTouch_On_SimpleTap;
            //EasyTouch.On_LongTap += EasyTouch_On_SimpleTap;
            EasyTouch.On_SwipeStart += EasyTouch_On_SwipeStart;
            EasyTouch.On_Swipe += EasyTouch_On_Swipe;
            EasyTouch.On_SwipeEnd += EasyTouch_On_SwipeEnd;
            EasyTouch.On_DragStart += EasyTouch_On_DragStart;
            EasyTouch.On_LongTapStart += EasyTouch_On_LongTapStart;
            EasyTouch.On_LongTapEnd += EasyTouch_On_LongTapEnd;
            EasyTouch.On_LongTap += EasyTouch_On_LongTap;
            EasyTouch.On_DoubleTap += EasyTouch_On_DoubleTap;
            EasyTouch.On_TouchStart += EasyTouch_On_TouchStart;
            EasyTouch.On_TouchUp += EasyTouch_On_TouchUp;
        }
    }

    private void RemoveEasyTouchEvent()
    {
        if ( _addEvent )
        {
            _addEvent = false;
            EasyTouch.On_SimpleTap -= EasyTouch_On_SimpleTap;
            //EasyTouch.On_LongTap -= EasyTouch_On_SimpleTap;
            EasyTouch.On_SwipeStart -= EasyTouch_On_SwipeStart;
            EasyTouch.On_Swipe -= EasyTouch_On_Swipe;
            EasyTouch.On_SwipeEnd -= EasyTouch_On_SwipeEnd;
            EasyTouch.On_DragStart -= EasyTouch_On_DragStart;
            EasyTouch.On_LongTapStart -= EasyTouch_On_LongTapStart;
            EasyTouch.On_LongTapEnd -= EasyTouch_On_LongTapEnd;
            EasyTouch.On_LongTap -= EasyTouch_On_LongTap;
            EasyTouch.On_DoubleTap -= EasyTouch_On_DoubleTap;
            EasyTouch.On_TouchStart -= EasyTouch_On_TouchStart;
            EasyTouch.On_TouchUp -= EasyTouch_On_TouchUp;
        }
    }
    private bool GetInputContext(Vector2 screenInput,out Vector2Int globalGridPos,out Vector3 worldPos,out int globalIndex)
    {
        //todo ::
        globalIndex = 0;
        globalGridPos = Vector2Int.zero;
        worldPos = Vector3.zero;
        globalIndex = 0;
        return false;


        //if (MapUtils.GetGlobalGridPosFromScreenPos(screenInput, out globalGridPos))
        //{
        //    globalIndex = MapUtils.GlobalGridPosToGlobalIndex(globalGridPos.x,globalGridPos.y);
        //    worldPos = CameraUtil.GetWorldPos(_cameraScroller.mainCamera, screenInput,false);
        //    return true;
        //}
        //worldPos = Vector3.zero;
        //globalIndex = 0;
        //return false;
    }

    private Vector3 _worldPos;
    private int _globalIndex;
    private Vector2Int _globalGridPos;
    
    private void _TryTouch(Gesture gesture, EasyTouch.EventName type)
    {
        //try
        //{
        //    if (!GetInputContext(gesture.position, out _globalGridPos, out _worldPos, out _globalIndex))
        //    {
        //        return;
        //    }
            int gridX, gridY;
            _mainCityView.WorldPosToCityGridPos(_worldPos, out gridX, out gridY);
            //Debug.Log("Test grid X : Test grid Y" + gridX +  gridY);
            //CityViewHelper.OnTouchHomeScene(_worldPos.x, _worldPos.z, gridX, gridY, (int)type, gesture.position.x, gesture.position.y);
        //}
        //catch (Exception e)
        //{
        //    Debug.LogException(e);
        //}
    }

    private void EasyTouch_On_SimpleTap(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_SimpleTap");

        if (gesture.isOverGui) return;
        if (_touch2FingerCancelStatus == Touch2FingerCancelStatus.SkipNextSimpleTap)
        {
            _touch2FingerCancelStatus = Touch2FingerCancelStatus.None;
            return;
        }
        _TryTouch(gesture, EasyTouch.EventName.On_SimpleTap);
    }
    
    private void EasyTouch_On_SwipeStart(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_SwipeStart");

        _TryTouch(gesture, EasyTouch.EventName.On_SwipeStart);
    }
    
    private void EasyTouch_On_Swipe(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_Swipe");
        _TryTouch(gesture, EasyTouch.EventName.On_Swipe);
        
        //todo :: 解除限定。
        //if (MapDraggableCamera.LOCK_DRAG) // 行军状态下是锁定地图拖动的
        {
            EdgeMovement(gesture); // 不过如果滑动到地图边缘还是会触发自动移动地图
        }
    }

    void EasyTouch_On_SwipeEnd(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_SwipeEnd");

        _TryTouch(gesture, EasyTouch.EventName.On_SwipeEnd);
    }
    
    void EasyTouch_On_DoubleTap(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_DoubleTap");

        if (gesture.isOverGui) return;
        _TryTouch(gesture, EasyTouch.EventName.On_DoubleTap);
    }

    void EasyTouch_On_PinchOut(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_PinchOut");

        //DebugUtil.Log("EasyTouch_On_PinchOut " + gesture);
        _cameraScroller?.OnPinchOut(gesture);
    }

    void EasyTouch_On_PinchIn(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_PinchIn");

        //DebugUtil.Log("EasyTouch_On_PinchIn " + gesture);
        _cameraScroller?.OnPinchIn(gesture);
    }
    
    private void EasyTouch_On_PinchEnd(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_PinchEnd");

        //DebugUtil.Log("EasyTouch_On_PinchEnd " + gesture);
        _cameraScroller?.OnPinchEnd(gesture);
    }
    
    private void EasyTouch_On_LongTapStart(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_LongTapStart");

        _TryTouch(gesture, EasyTouch.EventName.On_LongTapStart);
    }
    
    private void EasyTouch_On_LongTap(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_LongTap");

        _TryTouch(gesture, EasyTouch.EventName.On_LongTap);
    }
    
    private void EasyTouch_On_LongTapEnd(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_LongTapEnd");

        _TryTouch(gesture, EasyTouch.EventName.On_LongTapEnd);
    }

    private Vector2Int _gestureOffset = Vector2Int.zero;
    private void EasyTouch_On_DragStart(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_DragStart");

        if (gesture.touchCount > 1) //  多点触控下 不走Drag逻辑
        {
            return;
        }
        
        _TryTouch(gesture, EasyTouch.EventName.On_LongTapEnd);

        // Debug.Log("EasyTouch_On_DragStart");
        //        if (_interactingBuilding == null || !_interactingBuilding.isFakeBuilding)
        //        {
        //            _TrySelectTileGridPos(gesture, TileTouchType.DragStart);
        //        }
        //
        //        if (_interactingBuilding == null)
        //        {
        //            return;
        //        }
        //
        //        if ( ! ClickOnCity(_globalGridPos)  )
        //        {
        //            if ( _placeContext.mode != PlaceBuildingMode.Build )
        //            {
        //                ResetOperation();
        //            }
        //            return;
        //        }
        //
        //        
        //        Vector2Int InputPos = new Vector2Int(gridX,gridY);
        //        bool inputInInteractiveBuilding = CityUtils.ClickedOnBuilding(_interactingBuilding.cityGridPos,_interactingBuilding.width,InputPos);
        //        if ( ! _interactingBuilding.movable || ! inputInInteractiveBuilding )
        //        {
        ////            if (_placeContext.mode != PlaceBuildingMode.Build)
        ////            {
        ////                ResetOperation();
        ////            }            
        //            return;
        //        }
        //
        //       
        //        if (string.IsNullOrEmpty(_interactingBuilding.UID) /*|| _placeContext.lockMap.ContainsKey(_interactingBuilding.buildId)*/)
        //        {
        //            if (string.IsNullOrEmpty(_interactingBuilding.UID) && _placeContext.mode != PlaceBuildingMode.Build)
        //            {
        //                //DebugUtil.LogError("string.IsNullOrEmpty(_interactingBuilding.buildId)");
        //            }
        //            //  SendNotification(NotificationConst.SHOW_NOTICE,"上一次移动操作还未完成");
        ////对于新建建筑不做处理,只对建造中的或者升级中的处理
        //            if (_placeContext.mode != PlaceBuildingMode.Build)
        //            {
        //                LeaveBuildingDragMode();
        //                return;
        //            }
        //        }
        //        //检测逻辑上是否可移动
        //        bool canMove = CityViewHelper.IsCityNodeCanMove(_interactingBuilding.UID,true);
        //        if (!canMove)
        //        {
        //            EasyTouch.instance.eventInterceptor += DragEventInterceptorNone;
        //            return;
        //        }
        //        
        //        _gestureOffset = InputPos - _interactingBuilding.cityGridPos;
        //        EnterBuildingDragMode();
    }

    private void EasyTouch_On_TouchStart(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_TouchStart");

        if (_cameraScroller == null)
        {
            return;
        }

//        _followTroopId = -1;
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
        _TryTouch(gesture, EasyTouch.EventName.On_TouchStart);
    }
    
    private void EasyTouch_On_TouchUp(Gesture gesture)
    {
        Debug.Log("EasyTouch_On_TouchUp");

        //        _cameraScroller?.OnTouchUp(gesture);
        _TryTouch(gesture, EasyTouch.EventName.On_TouchUp);
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
    
    private bool _dragInterceptorAdded = false;

    public void EnterBuildingDragMode()
    {
        if ( ! _dragInterceptorAdded )
        {
            _dragInterceptorAdded = true;
            EasyTouch.instance.eventInterceptor += DragEventInterceptor;
        }
    }

    public void LeaveBuildingDragMode()
    {
        if ( _dragInterceptorAdded )
        {
            _dragInterceptorAdded = false;
            EasyTouch.instance.eventInterceptor -= DragEventInterceptor;
        }
    }

    Vector2Int _inputPos  = Vector2Int.down;
    Vector2Int _buildingNewPos = Vector2Int.down;
    Vector2Int _lastMovepos = Vector2Int.down;

    private bool DragEventInterceptorNone(EasyTouch.EventName eventName, Gesture gesture)
    {
         if ( eventName == EasyTouch.EventName.On_Drag || eventName == EasyTouch.EventName.On_Swipe )
         {
             return false;
         }
         else if ( eventName == EasyTouch.EventName.On_DragEnd || eventName == EasyTouch.EventName.On_SwipeEnd )
         {
             EasyTouch.instance.eventInterceptor -= DragEventInterceptorNone;
         }
         return false;
    }

    private bool DragEventInterceptor(EasyTouch.EventName eventName,Gesture gesture)
    {
        //if (!GetInputContext(gesture.position, out _globalGridPos, out _worldPos, out _globalIndex))
        //{
        //    return false;
        //}
        //int gridX,gridY;
        //_mainCityView.WorldPosToCityGridPos(_worldPos,out gridX,out gridY);
        //CityViewHelper.OnHomeDragInterceptor(_worldPos.x, _worldPos.z, gridX, gridY, (int)eventName);
        //EdgeMovement(gesture);
        return false;
    }
    
    #endregion
    
    

    #region newBuilding 
    

    public void ConfirmMove()
    {
    }
    
    // 这里只做重置操作
    public void ConfirmMerge(bool isMerging = false)
    {
//        if (_interactionMode)
//        {
//            if (isMerging) // 只隐藏选择框
//            {
//                if (_mainCityView != null && _interactingBuilding != null)
//                {
//                    _mainCityView.TryDeselectBuilding(_interactingBuilding);
//                }
//            }
//            else
//            {
//                CancelSelect();
//                if (_mainCityView != null)
//                {
//                    _mainCityView.ChangeGridVisibility(false);
//                }
//                _placeContext.mode = PlaceBuildingMode.Invalid;
//            }
//        }
    }

    public void CancelMove()
    {
//        CancelBuildingMove(_interactingBuilding);
//        CancelSelect();
//        _placeContext.mode = PlaceBuildingMode.Invalid;
    }

    #endregion

    public bool GetGridXY(Vector2 screenPos, ref int gridX, ref int gridY)
    {
        return false;
        //Vector2Int GridPos;
        //if (_cameraScroller.mainCamera != null && MapUtils.GetGlobalGridPosFromScreenPos(screenPos, out GridPos))
        //{
        //    Vector3 worldPos = CameraUtil.GetWorldPos(_cameraScroller.mainCamera, screenPos,false);
        //    _mainCityView.WorldPosToCityGridPos(worldPos,out gridX,out gridY);
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }
}
