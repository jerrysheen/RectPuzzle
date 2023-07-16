using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SceneMediatorBase :IMapGlobalContext {
    
    public float FollowDuration = 0.2f;
    protected int _followEntityId = -1;
    public int FollowEntityId
    {
        set
        {
            if (_followEntityId != value)
            {
                _followEntityId = value;
            }
        }
    }
    
    protected MapSceneCameraScroller _cameraScroller;

    /// <summary>
    /// 滑动到地图边缘自动移动地图
    /// </summary>
    /// <param name="gesture"></param>

    protected Vector3 centerScreen = new Vector3(0.5f, 0.5f);
    protected void EdgeMovement(Gesture gesture)
    {

        Vector3 screenPos = CameraManager.GetMainCamera().ScreenToViewportPoint(gesture.position);
        Debug.Log("screenPos : " + screenPos);

        if (screenPos.x < 0.1f || screenPos.x > 0.9f || screenPos.y < 0.1f || screenPos.y > 0.9f)
        {
            Vector3 direction = screenPos;
            direction.x -= 0.5f;
            direction.y -= 0.5f;
            direction = direction.normalized * MapCalcConst.MAP_CAMERA_MOVE_SPPED * GetEdgeMoveFactor();
            Vector3 nextScreenPos = CameraManager.GetMainCamera().ViewportToScreenPoint(centerScreen + direction);
            Vector3 nextWorldPos = CameraUtil.GetWorldPos(CameraManager.GetMainCamera(), nextScreenPos, false);
            // UnityEngine.Debug.LogError(nextWorldPos);
            if (cameraScroller != null)
            {
                Debug.Log("CameraScroller is not null");

                cameraScroller.MoveCameraToPosition(nextWorldPos, this.GetCameraMoveSpeed());
            }
            else 
            {
                Debug.Log("CameraScroller is null");
            }
        }
    }
    
    protected virtual float GetEdgeMoveFactor()
    {
        return 1f;
    }

    protected virtual float GetCameraMoveSpeed()
    {
        return 0.4f;
    }

    //获取当前相机视野中心所对的世界坐标位置
    public Vector3 GetCameraCenterPos()
    {
        if (_cameraScroller == null)
        {
            return Vector3.zero;
        }
        return MapCalcConst.CalcCameraLookAtPos(_cameraScroller.mainCamera);
    }

    protected void FollowEntity()
    {
        //// 军队跟随
        //if (_followEntityId <= 0)
        //{
        //    return;   
        //}
        //EntityBase entityBase = EntityManager.GetInstance().GetEntity(_followEntityId);
        //if (entityBase == null || cameraScroller == null)
        //{
        //    _followEntityId = -1;
        //    return;
        //}
        //var pos = entityBase.position;
        //if (FollowDuration > 0) {
        //    cameraScroller.MoveCameraToPosition(pos, FollowDuration);
        //}else {
        //    cameraScroller.MoveCameraQuickToPosition(pos);
        //}
    }

    public int curGridY { get; }
    public int curGridX { get; }
    public int lod { get; }
    public virtual IMapCameraScroller cameraScroller => _cameraScroller;
    public RectInt viewRect { get; }
    
    public virtual void Update() {}
    public virtual MediaType curMediaType { get; }
}
