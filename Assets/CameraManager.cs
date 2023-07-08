/*--------------------------------------------------------*/
// 相机管理;
// 3D相机UI相机等相机的参数和状态已经常用方法;
/*--------------------------------------------------------*/

using System;
using ELEX.Common.Utility;
using UnityEngine;
//using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviourSingle<CameraManager>
{
    private static Camera s_UICamera = null;
    private static Camera s_MainCamera = null;
    public static Transform s_transCache = null;
    public static bool s_IsCameraChange = true;
    private static Vector3 s_CameraPos = Vector3.zero;
    private static Vector3 s_CameraForward = Vector3.zero;
    private static float s_fov = 0;
    
    internal static float s_fovDis = 0f;
    private static float s_fovFactor = 0f;
    internal static float s_mapHeight = 0f;
    /// <summary>
    /// 【IMPORTANT:仅用于世界地图】
    ///  世界地图UI缩放相关系数;
    ///  算法：去除相机height的影响，fov会影响大小，但从fov(min)->fov(max)，影响越来越小;
    /// </summary>
    public static float s_mapHeadFactor = 0f;
    internal static float s_maxFov;
    private static float s_cacheMapHeight = 0f;
    
    private bool doURPCamera = false;
    private Camera recordCamera = null;

    protected override void OnInit()
    {
    }

//    public override void OnUpdate(float deltaTime)
//    {
//        UpdateURPCamera();
//        UpdateCameraPos();
//    }

    protected override void OnUpdate(float deltaTime)
    {
        UpdateURPCamera();
    }

    private void LateUpdate()
    {
        UpdateCameraPos();
    }

    public void OnChangeScene()
    {
        s_MainCamera = null; // 不同场景相机不同，清理缓存
    }

    public static Camera GetMainCamera()
    {
        if (s_MainCamera == null)
        {
            s_MainCamera = Camera.main;
            if (s_MainCamera != null)
                s_transCache = s_MainCamera.transform;
        }
        return s_MainCamera;
    }

    public static Camera GetUICamera()
    {
        //if (s_UICamera == null) s_UICamera = BridgeDefine.GetUICamera?.Invoke();
        return s_UICamera;
    }

    // 该点是否在相机后面;
    internal static bool IfBehindOfCamera(Vector3 pos)
    {
        Vector3 dir = (pos - s_CameraPos).normalized;
        float dot = Vector3.Dot(dir, s_CameraForward);
        return (dot < 0);
    }
    
    internal static bool IsInView(Vector3 worldPos)
    {
        Camera mainCamera = GetMainCamera();
        if (mainCamera == null)
            return false;
        
        Vector2 viewPos = GetMainCamera().WorldToViewportPoint(worldPos);
        Vector3 dir = (worldPos - s_transCache.position).normalized;
        float dot = Vector3.Dot(s_transCache.forward, dir);     //判断物体是否在相机前面
        // 判断是否在屏幕内
        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            return true;
        else
            return false;
    }

    private void UpdateCameraPos()
    {
        Camera camera = GetMainCamera();
        if (camera == null) return;

        s_IsCameraChange = false;
        Vector3 curPos = camera.transform.position;
        if (!MathfUtils.IsEqualEx(curPos, s_CameraPos))
        {
            s_CameraPos = curPos;
            s_CameraForward = camera.transform.forward;
            s_IsCameraChange = true;
            //AudioListenerBinder.SetPosition(curPos);
        }

        if (!MathfUtils.Approximately(camera.fieldOfView, s_fov))
        {
            s_fov = camera.fieldOfView;
            s_IsCameraChange = true;
            UpdateFovDis(camera);
        }

        s_mapHeight = camera.transform.position.y;
        if (!MathfUtils.Approximately(s_mapHeight, s_cacheMapHeight))
        {
            s_IsCameraChange = true;
            s_cacheMapHeight = s_mapHeight;
        }
        
        if (s_IsCameraChange)
        {
            s_mapHeadFactor = s_mapHeight / s_fovFactor;
        }
    }
    
    private void UpdateFovDis(Camera mainCamera)
    {
        s_fovDis = 2.0f * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        s_fovFactor = 3f * s_maxFov * s_fov - s_fov * s_fov;
    }
    
    internal void BeforeLoadSceneDoURP()
    {
        //Camera mainCam = GetMainCamera();
        //if (mainCam != null)
        //{
        //    UniversalAdditionalCameraData mainData = mainCam.GetUniversalAdditionalCameraData();
        //    mainData.renderType = CameraRenderType.Overlay;
        //}

        //Camera uiCam = GetUICamera();
        //if (uiCam != null)
        //{
        //    UniversalAdditionalCameraData uiData = uiCam.GetUniversalAdditionalCameraData();
        //    uiData.renderType = CameraRenderType.Base;
        //}
        
        //recordCamera = mainCam;
    }

    internal void AfterLoadSceneDoURP()
    {
        Camera mainCam = GetMainCamera();
        if (mainCam != null && mainCam != recordCamera)
        {
            DoURPCamera();
        }
        else
        {
            doURPCamera = true;       
        }
    }

    private void UpdateURPCamera()
    {
        if(!doURPCamera) return;
        
        Camera mainCam = GetMainCamera();
        if (mainCam == null || recordCamera == mainCam)
        {
            return;
        }

        DoURPCamera();
    }
    
    private void DoURPCamera()
    {
        //Camera uiCam = GetUICamera();
        //if (uiCam != null)
        //{
        //    UniversalAdditionalCameraData uiData = uiCam.GetUniversalAdditionalCameraData();
        //    uiData.renderType = CameraRenderType.Overlay;
        //}
        
        //Camera mainCam = GetMainCamera();
        //UniversalAdditionalCameraData mainData = mainCam.GetUniversalAdditionalCameraData();
        //mainData.renderType = CameraRenderType.Base;
        //if (mainData.cameraStack != null && TimelineCamera.cam == null && !mainData.cameraStack.Contains(uiCam))
        //{
        //    mainData.cameraStack.Add(uiCam);
        //}
        
        //doURPCamera = false;
        //recordCamera = null;
    }
}