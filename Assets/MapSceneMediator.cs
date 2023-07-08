//using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using ELEX.Common;
//using LuaInterface;
using UnityEngine;
using Object = UnityEngine.Object;


public class MapSceneMediator: MapSceneMediatorBase
{
//    public override MediaType curMediaType
//    {
//        get { return MediaType.World; }
//    }

//    public override void OnRemove()
//    {
//        base.OnRemove();
        
//        if (_terrainBaseMat != null)
//        {
//            Object.Destroy(_terrainBaseMat);
//            _terrainBaseMat = null;
//        }
//        if (_terrainColorBaseMat != null)
//        {
//            Object.Destroy(_terrainColorBaseMat);
//            _terrainColorBaseMat = null;
//        }
//    }

//    private void ClearCache()
//    {
//        if(activeTerrain.go != null) GameObject.Destroy(activeTerrain.go);
//        if(activeTerrain.sea != null) GameObject.Destroy(activeTerrain.sea);
        
//        _viewportChangeHandler?.ClearCacheData();
//    }

//    protected override void OnInitMapStart()
//    {
//        EasyTouch.instance.enableTwist = false;
//        EasyTouch.instance.longTapTime = MapCalcConst.EASY_TOUCH_LONG_TAP_TIME;


//        //if (MapAsset.instance != null)
//        //{
//        //    OnAssetLoadFinish(MapAsset.instance.meshResSetPack);

//        //    _lastTerrainMatId = 0;

//        //    CreateTerrain();
//        //}
//        //else
//        //{
//        //    Debugger.LogError("MapAsset is null");
//        //}
//    }

//    private void CreateTerrain()
//    {
////        if (_terrainAssetPack == null ||
////            _terrainAssetPack.gameObjects == null || _terrainAssetPack.gameObjects.Length<=0)
////        {
////            return;
////        }
////        float boundXMin, boundYMin, boundXMax, boundYMax;
////        float edge = 0;//terrain边缘冗余  防止相机看到界外
////        MapUtils.GlobalGridPosToWorldPos(0, 0, out boundXMin, out boundYMin);
////        MapUtils.GlobalGridPosToWorldPos(MapPlaceConfig.originGlobalMapWidth, MapPlaceConfig.originGlobalMapHeight, out boundXMax, out boundYMax);
        
//////        GameObject go = new GameObject("Terrain");
//////       
//////        go.transform.localScale = new Vector3(boundXMax - boundXMin + edge,1,boundYMax - boundYMin + edge);
//////       
//////        go.AddComponent<MeshFilter>();
//////        go.GetComponent<MeshFilter>().sharedMesh = _terrainAssetPack.meshes[0];
//////        go.AddComponent<MeshRenderer>();
//////        go.GetComponent<MeshRenderer>().sharedMaterials = _terrainTexMats;
////        GameObject go = GameObject.Instantiate(_terrainAssetPack.gameObjects[1]);
////        go.transform.position = new Vector3(0, -0.2f, 0);
////        go.transform.eulerAngles = _terrainAssetPack.terrainRot; 
////        go.layer = Layers.TERRAIN;
////        GameObject sea = GameObject.Instantiate(_terrainAssetPack.gameObjects[0]);
////        sea.transform.position = new Vector3(0, -0.1f, 0);
////        sea.transform.eulerAngles = _terrainAssetPack.seaRot; 
////        activeTerrain = new TerrainInfo
////        {
////            filter = go.GetComponent<MeshFilter>(),
////            renderer = go.GetComponent<MeshRenderer>(),
////            go = go,
////            sea = sea,
////            trans = go.transform
////        };
//    }
//    public override void OnLeaveWorldMap()
//    {
//        base.OnLeaveWorldMap();
//        ClearCache();
//    }

//    private void OnAssetLoadFinish(MeshResourceSetPack asset)
//    {
//        if (asset == null)
//        {
//            return;
//        }
//        _terrainAssetPack = asset;
        
//        _terrainBaseMat = new Material(_terrainAssetPack.materials[0]);
//        _terrainColorBaseMat = new Material(_terrainAssetPack.materials[1]);

//        _terrainTexMats = new Material[] { _terrainBaseMat };
//        _terrainColMats = new Material[] { _terrainColorBaseMat };
//    }

//    #region TerrainUpdate
    
//    private int _lastTerrainMatId = 0;
//    private MeshResourceSetPack _terrainAssetPack;


//    private class TerrainInfo
//    {
//        public Transform trans;
//        public GameObject go;
//        public GameObject sea;

//        public MeshRenderer renderer;
//        public MeshFilter filter;
//    }
    
    
//    private Material _terrainBaseMat;
//    private Material _terrainColorBaseMat;
    
//    private Material[] _terrainTexMats;
//    private Material[] _terrainColMats;
//    private TerrainInfo activeTerrain = null;
    
//    protected override void InvalidateMapChange(int gridX, int gridY, int lod, float viewDistance)
//    {
//        int newTerrainMatId = 0;
//        if (lod > 6)
//        {
//            newTerrainMatId = 1;
//        }

//        Material[] curMaterials = null;
//        switch (newTerrainMatId)
//        {
//            case 1: curMaterials = _terrainColMats;
//                break;
//            default: curMaterials = _terrainTexMats;
//                break;
//        }
//        bool matChanged = newTerrainMatId != _lastTerrainMatId;
//        if (matChanged)
//        {
//            _lastTerrainMatId = newTerrainMatId;
//            activeTerrain.renderer.sharedMaterials = curMaterials;
//        }
//    }
//    #endregion
}

public class Layers
{
    public const int DEFAULT = 0;

    public const int IGNORE_RAYCAST = 2;

    public const int TERRAIN = 8;

    public const int NGUI = 9;

    public const int FORCEGROUND = 10;

    public const int GUIDE_TOP_UI = 11;

    public const int UI3DMODEL = 12;

    public const int CITY_WALL = 13;

    public const int OCCLUDER = 15;
}
