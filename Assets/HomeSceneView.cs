//using CityFog.View;
//using LuaInterface;
using UnityEngine;
//using NUnit.Framework.Constraints;

public struct CityRootContext
{
    public Transform gridPivotTrans;
    public GameObject fog;
}

public class HomeSceneView
{
    #region privateField

    private const string CITY_TEMPLATE_ID = "home";
    #endregion

    #region Property

    public Transform gridPivotTrans
    {
        get { return GetGridPivotRoot(); }
    }

    private CityRootContext _cityContext;

    public CityRootContext cityContext
    {
        get
        {
            return _cityContext;
        }
    }


    private bool _isSelf;

    public bool isSelf
    {
        get
        {
            return _isSelf;
        }
    }

    private string _cityId = "0";
    public string CityId
    {
        get
        {
            return _cityId;
        }
    }

    //private CityFogController _cityFogController= null;
    //private CityObjectEraser _cityObjectEraser = null;

    #endregion

    #region Initialization
    private HomeSceneMediator homeMediator = null;
    public HomeSceneView()
    {
        homeMediator = WorldMapManager.instance.mHomeMedia;
    }

    public void EnterHomeScene(string cityId)
    {
        //_cityId = cityId;
        //_isSelf = cityId == CityViewHelper.GetSelfCityId();
        InitCityView();
    }

    public void LeaveHomeScene()
    {
        Dispose();
    }

    private const string GRID_PIVOT_NAME = "CityGridPivot";
    private const string FOG = "fog";

    private void InitCityView()
    {
        //if (CityConst.instance.WarFogOpen)
        //{
        //    SingleResLoadMgr.instance.LoadModel(FOG, (go) =>
        //    {
        //        if (go == null)
        //        {
        //            return;
        //        }
        //        go.SetActive(true);
        //        go.transform.parent = gridPivotTrans;
        //        _cityContext.fog = go;
        //        InitFogInfo(_cityContext.fog);
        //        LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.OnFogLoaded");
        //        if (luaFunc != null)
        //        {
        //            luaFunc.Call();
        //        }
        //    }, false, true);
        //}

        //InitBuildingEraser();

        //Physics.SyncTransforms();
    }

    public Transform GetGridPivotRoot()
    {
        //if (_cityContext.gridPivotTrans == null)
        //{
        //    GameObject go = GameObject.Find(GRID_PIVOT_NAME);
        //    if (go != null)
        //    {
        //        _cityContext.gridPivotTrans = go.transform;
        //    }
        //}
        //return _cityContext.gridPivotTrans;

        return null;
    }

    #endregion

    #region publicFunc

    public void WorldPosToCityGridPos(Vector3 worldPos, out int x, out int y)
    {
        // CityUtils.WorldPostoCityGridPos(worldPos, GetGridPivotRoot(), out x, out y);

        x = 0; y = 0;
    }
    public void WorldPosToCityNodeLocalPos(Vector3 worldPos, ref Vector3 localPos)
    {
       // CityUtils.WorldPosToCityNodeLocalPos(worldPos, GetGridPivotRoot(), ref localPos);


    }

    public void Dispose()
    {
        //_cityContext.fog = null;
        //_cityContext.gridPivotTrans = null;
        //if (this._cityFogController != null)
        //{
        //    this._cityFogController.Release();
        //    this._cityFogController = null;
        //}
        //SingleResLoadMgr.instance.RecycleModel(FOG);
    }

    #endregion

    #region unlock


    private void InitFogInfo(GameObject fog)
    {
        //if (homeMediator == null)
        //{
        //    return;
        //}

        //if (!CityConst.instance.WarFogOpen)
        //{
        //    HideFog();
        //    return;
        //}
        //if (_cityContext.fog != null)
        //{
        //    _cityContext.fog.transform.localPosition = new Vector3(CityConst.instance.FogGridPosX * CityConst.instance.CityTileRealWidth, 13,
        //        CityConst.instance.FogGridPosY * CityConst.instance.CityTileRealHeight);
        //    _cityContext.fog.transform.localScale = new Vector3(-CityConst.instance.FogMapW * CityConst.instance.CityTileRealWidth,
        //                                                        -CityConst.instance.FogMapH * CityConst.instance.CityTileRealHeight);
        //    _cityContext.fog.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        //}

        //_cityFogController = _cityContext.fog.GetComponent<CityFogController>();
        //if (_cityFogController != null)
        //{
        //    _cityFogController.OnInit();
        //}
    }

    public void HideFog()
    {
        //if (_cityContext.fog != null)
        //{
        //    _cityContext.fog.SetActive(false);
        //}
    }

    public void ShowFog()
    {
        if (_cityContext.fog != null)
        {
            _cityContext.fog.SetActive(true);
        }
    }

    /// <summary>
    /// 设置解锁迷雾序号
    /// </summary>
    /// <param name="idx"></param>
    public void SetUnlockFogIdx(int idx, int index_value = 1)
    {
        //_cityFogController?.UpdateUnlockData(idx, index_value);
    }


    /// <summary>
    ///迷雾选中效果设置  0清空选中
    /// </summary>
    /// <param name="idx"></param>
    public void UpdateFogHighlightArea(int index)
    {
        //_cityFogController?.UpdateCurrentHighlightArea(index);
    }

    public int GetFogAreaByXY(int gridX, int gridY)
    {
        //if (_cityFogController == null)
        //{
        //    return -1;
        //}
        //return -1;
        // return _fogView.GetFogAreaByXY(gridX,gridY);

        return -1;
    }

    public void InitBuildingEraser()
    {
        //GameObject go = GameObject.Find("CityObjectEraser");
        //if (go == null)
        //{
        //    return;
        //}
        //_cityObjectEraser = go.GetComponent<CityObjectEraser>();
        //if (_cityObjectEraser == null)
        //{
        //    return;
        //}

        //_cityObjectEraser.InitWithHomeSceneView(this);
    }

    public void UpdateAllBuildingsEraser()
    {
        //if (_cityObjectEraser)
        //{
        //    _cityObjectEraser.UpdateWithBuildings();
        //}
    }

    public void UpdateOneBuildingEraser(int xLeftBottom, int yLeftBottom, int xRightTop, int yRightTop, bool bVisible)
    {
        //if (_cityObjectEraser)
        //{
        //    _cityObjectEraser.UpdateGameObjectState(xLeftBottom, yLeftBottom, xRightTop, yRightTop, bVisible);
        //}
    }


    #endregion
}
