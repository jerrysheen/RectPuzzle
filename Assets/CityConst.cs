using System.Collections;
using System.Collections.Generic;
//using ELEX.Config;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class CityConst: Singleton<CityConst>{
    public float CityTileRealWidth ;    // 主城每个格子的世界单位宽度(米) map
    public float CityTileRealHeight ;    // 主城每个格子的世界单位高度(米) 
    public int CityWidth ;    // 主城宽度(格子数量) 
    public int CityHeight ;    // 主城高度(格子数量) 
    public int CityFogEdgeWidth ;    // 主城迷雾图宽度(格子数量) 
    public int CityFogEdgeHeight ;    // 主城迷雾图高度(格子数量) 
    public float HomeCamNearClip ;    // 内城相机nearClip 
    public float HomeCamFarClip ;    // 内城相机farClip 
    public static readonly Vector3 buildingSelectOffest = new Vector3(0,0,0.001f);
    public bool WarFogOpen = false;    // 是否开启战争迷雾
    public float PinchFactor  = 1;    // 内城相机缩放系数 
    public float BaseHeight  = 0;    // 内城相机基础高度 
    public int DefaultLevel  = 0;    // 内城相机基础高度 
    public MapCameraInfo[] LOD_CAM_INFO_LIST_HOME = null;
    
    public void InitData()
    {
        //CityFogEdgeWidth = int.Parse(ConfHelper.GetConfDataConstant("city_fog_width").value);
        //CityFogEdgeHeight = int.Parse(ConfHelper.GetConfDataConstant("city_fog_height").value);
        //CityWidth = int.Parse(ConfHelper.GetConfDataConstant("city_width").value);
        //CityHeight = int.Parse(ConfHelper.GetConfDataConstant("city_height").value);
        //HomeCamNearClip = float.Parse(ConfHelper.GetConfDataConstant("h_near_clip").value);
        //HomeCamFarClip = float.Parse(ConfHelper.GetConfDataConstant("h_far_clip").value);
        //CityTileRealWidth =float.Parse(ConfHelper.GetConfDataConstant("city_tile_width").value); 
        //CityTileRealHeight = float.Parse(ConfHelper.GetConfDataConstant("city_tile_width").value);
    }
    
    public int FogGridPosX = 0;
    public int FogGridPosY = 0;
    public int FogMapW = 0;
    public int FogMapH = 0;
    public void SetFogParam(int fogX,int fogY,int width,int height)
    {
        FogGridPosX = fogX;
        FogGridPosY = fogY;
        FogMapW = width;
        FogMapH = height;
    }
    
    public void SetFogSwitch(bool isOpen)
    {
        WarFogOpen = isOpen;
    }

    public void UpdateCameraInfo(CityCameraAsset asset)
    {
        if (asset == null)
        {
            return;
        }
        LOD_CAM_INFO_LIST_HOME = asset.points.ToArray();
        PinchFactor = asset.pinchFactor;
        BaseHeight = asset.baseHeight;
        DefaultLevel = asset.defaultLevel;
    }

}

