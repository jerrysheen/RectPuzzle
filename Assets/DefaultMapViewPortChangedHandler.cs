using System.Collections.Generic;
//using LuaInterface;
using UnityEngine;

public class DefaultMapViewPortChangedHandler
{
    //public IMapDisplayDataProvider[] _dataProviderArr = null;
    //private List<MapDisplayData> _lastDisplayData = new List<MapDisplayData>(400);
    //private List<MapDisplayData> _swapDisplayData = new List<MapDisplayData>(400);

    //private HashSet<long> _lastDataIndexSet = new HashSet<long>();
    //private HashSet<long> _swapDataIndexSet = new HashSet<long>();
    //private Dictionary<int, IMapObjectRenderManager> _renderMgrMap = new Dictionary<int, IMapObjectRenderManager>(10);
    //private List<IMapObjectRenderManager> _renderMgrList = new List<IMapObjectRenderManager>(10);

    //private float _lastViewDis = 0;
    //private int _lastLod = -1;
    //private int _lastXMin = -1, _lastXMax = -1, _lastYMin = -1, _lastYMax = -1;
    ////private MapProxy _mapProxy;

    //public DefaultMapViewPortChangedHandler(IMapDisplayDataProvider[] dataProviders)
    //{
    //    _dataProviderArr = dataProviders;
    //}

    //public void RegisterMapObjectRenderManager(int type, IMapObjectRenderManager mgr)
    //{
    //    if (!_renderMgrMap.ContainsKey(type))
    //    {
    //        _renderMgrMap.Add(type, mgr);
    //        if (!_renderMgrList.Contains(mgr))
    //        {
    //            _renderMgrList.Add(mgr);
    //        }
    //    }
    //}
    
    //public void OnViewportChange(RectInt newViewport, float viewDistance, int lod, Vector3 cameraPos)
    //{
    //    int xMin = Mathf.Max(0, newViewport.xMin);
    //    int yMin = Mathf.Max(0, newViewport.yMin);
    //    int xMax = Mathf.Min(MapPlaceConfig.originGlobalMapWidth, newViewport.xMax);
    //    int yMax = Mathf.Min(MapPlaceConfig.originGlobalMapWidth, newViewport.yMax);
    //    OnViewportChange(xMin, yMin, xMax, yMax, viewDistance, lod, cameraPos);
    //}

    //private void OnViewportChange(int xMin, int yMin, int xMax, int yMax, float viewDistance, int lod, Vector3 cameraPos)
    //{
    //    float viewDistanceEx = CameraManager.s_fovDis * viewDistance;
        
    //    bool lodChanged = lod != _lastLod;
    //    bool viewDistanceChanged = viewDistanceEx != _lastViewDis;
    //    // bool viewportChanged = xMin != _lastXMin || yMin != _lastYMin || xMax != _lastXMax || yMax != _lastYMax;

    //    _lastXMin = xMin;
    //    _lastXMax = xMax;
    //    _lastYMin = yMin;
    //    _lastYMax = yMax;

    //    _swapDisplayData.Clear();
    //    _swapDataIndexSet.Clear();
        
    //    IMapDisplayDataProvider dataProvider = _dataProviderArr[lod];
    //    if (dataProvider != null)
    //    {
    //        dataProvider.QueryMapDisplayData(xMin * MapPlaceConfig.MAP_FIX_SCALE, 
    //            yMin * MapPlaceConfig.MAP_FIX_SCALE, 
    //            xMax * MapPlaceConfig.MAP_FIX_SCALE, 
    //            yMax * MapPlaceConfig.MAP_FIX_SCALE, OnQueryNewViewportData);
    //    }

    //    foreach(var mgr in _renderMgrList)
    //    {
    //        mgr.StartViewportCheck();
    //    }
        
    //    int len = _lastDisplayData.Count;
    //    for (int i = 0; i < len; ++i)
    //    {
    //        if (!_swapDataIndexSet.Contains(_lastDisplayData[i].signature))
    //        {
    //            if (_renderMgrMap.ContainsKey(_lastDisplayData[i].type))
    //            {
    //                _renderMgrMap[_lastDisplayData[i].type].OnObjectSwapOut(_lastDisplayData[i]);
    //            }
    //            else
    //            {
    //                OnObjectOutToLua(_lastDisplayData[i].type, _lastDisplayData[i]);
    //            }
               
    //        }
    //    }

    //    if (lodChanged || viewDistanceChanged)
    //    {
    //        if (lodChanged)
    //        {
    //            _lastLod = lod;
    //        }
    //        if (viewDistanceChanged)
    //        {
    //            _lastViewDis = viewDistanceEx;
    //        }
    //        foreach(var mgr in _renderMgrList)
    //        {
    //            mgr.OnViewDistanceChange(_lastViewDis, _lastLod);
    //        }
    //    }

    //    len = _swapDisplayData.Count;
    //    for (int i = 0; i < len; ++i)
    //    {
    //        if (!_lastDataIndexSet.Contains(_swapDisplayData[i].signature))
    //        {
    //            if (_renderMgrMap.ContainsKey(_swapDisplayData[i].type))
    //            {
    //                _renderMgrMap[_swapDisplayData[i].type].OnObjectSwapIn(_swapDisplayData[i]);
    //            }
    //            else
    //            {
    //                OnObjectInToLua(_swapDisplayData[i].type, _swapDisplayData[i]);
    //            }
    //        }
    //    }

    //    foreach(var mgr in _renderMgrList)
    //    {
    //        mgr.EndViewportCheck();
    //    }
        

    //    List<MapDisplayData> tmpList = _swapDisplayData;
    //    _swapDisplayData = _lastDisplayData;
    //    _lastDisplayData = tmpList;

    //    HashSet<long> tmpSet = _swapDataIndexSet;
    //    _swapDataIndexSet = _lastDataIndexSet;
    //    _lastDataIndexSet = tmpSet;
        
    //    LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnViewPortChange");
    //    if (luaFunc != null)
    //    {
    //        float lookAtX = cameraPos.x;
    //        float lookAtZ = cameraPos.z + cameraPos.y;
    //        luaFunc.Call(xMin, yMin, xMax, yMax, viewDistance, viewDistanceEx, lod, lookAtX, lookAtZ);
    //    }
    //}

    //private void OnQueryNewViewportData(ref MapDisplayData data)
    //{
    //    _swapDisplayData.Add(data);
    //    _swapDataIndexSet.Add(data.signature);
    //}

    //public void OnViewDistanceChange(float viewDistance, int lod)
    //{
    //    //DebugUtil.Log("[Test]OnViewDistanceChanged");

    //    _lastLod = lod;
    //    _lastViewDis = viewDistance;
    //    foreach(var mgr in _renderMgrList)
    //    {
    //        mgr.OnViewDistanceChange(_lastViewDis, _lastLod);
    //    }
    //}
    //public void ClearCacheData()
    //{
    //    if (_renderMgrList != null)
    //    {
    //        foreach(var mgr in _renderMgrList)
    //        {
    //            mgr.Dispose();
    //        }
    //    }
    //    _lastDisplayData.Clear();
    //    _swapDisplayData.Clear();
    //    _lastDataIndexSet.Clear();
    //    _swapDataIndexSet.Clear();
    //    _lastViewDis = 0;
    //    _lastLod = -1;
    //    _lastXMin = -1; 
    //    _lastXMax = -1;
    //    _lastYMin = -1; 
    //    _lastYMax = -1;
    //}

    //private void OnObjectInToLua(byte t,MapDisplayData data)
    //{
    //    LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnObjectIn");
    //    if (luaFunc != null)
    //    {
    //        luaFunc.Call(t, data);
    //    }
    //}
    
    //private void OnObjectOutToLua(byte t,MapDisplayData data)
    //{
    //    LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnObjectOut");
    //    if (luaFunc != null)
    //    {
    //        luaFunc.Call(t, data);
    //    }
    //}
}
