using System;
using System.Collections.Generic;
using System.IO;
//using ELEX.Common.ClientEvent;
using ELEX.Common.Utility;
//using ELEX.Resource;
using UnityEngine;

public enum MediaType
{
    World  = 1,
    Home   = 2,
    // Battle = 3,
    MainCity = 4,
    Dungeon = 5,
}
public class WorldMapManager : MonoBehaviourSingle<WorldMapManager>
{
    public SceneMediatorBase mCurActiveScene = null;

    public IMapDataProvider mMapData = null;
    public MapSceneMediator mMapMedia = null;
    public HomeSceneMediator mHomeMedia = null;
    public MainCityMediator mMainCityMedia = null;
    public DungeonSceneMediator mDungeonMedia = null;

    public Dictionary<MediaType, IMapGlobalContext> mMediaDic = new Dictionary<MediaType, IMapGlobalContext>();
    private const string mWorldSceneName = "worldmapscene";
    private const string mHomeSceneName = "homescene";
    private bool mIsInit = false;
    protected override void OnInit()
    {
    }

    protected void OnDestroy()
    {
        //        mMapMedia?.OnRemove();
        //        mHomeMedia?.OnRemove();
    }

    public void InitMapData(string blockName, string modelName = "")
    {
        //string gridFilePath = HotfixManager.GetBinaryFilePath(blockName);
        //FileStream gridFs = File.OpenRead(gridFilePath);

        //string localMapPrefixPath;
        //FileStream modelFs = null;
        //if (modelName != "")
        //{
        //    localMapPrefixPath = HotfixManager.GetBinaryFilePath(modelName);
        //    modelFs = File.OpenRead(localMapPrefixPath);
        //}
        //MapDataProvider dataProvider = new MapDataProvider();
        //dataProvider.InitializeWith(modelFs, gridFs);
        //gridFs.Close();
        //if (modelFs != null)
        //{
        //    modelFs.Close();
        //}
        //this.mMapData = dataProvider;
    }



    public void InitMgr()
    {
        //if (!mIsInit)
        //{
        //    CityConst.instance.InitData();
        //    MapCalcConst.LoadConf();
        //    mMediaDic.Clear();
        //    mMapMedia?.OnRemove();
        //    mMainCityMedia?.OnRemove();

        //    mMapMedia = new MapSceneMediator();
        //    mMediaDic.Add(MediaType.World, mMapMedia);
        //    mHomeMedia = new HomeSceneMediator();
        //    mMediaDic.Add(MediaType.Home, mHomeMedia);
        //    mMainCityMedia = new MainCityMediator();
        //    mMediaDic.Add(MediaType.MainCity, mMainCityMedia);
        //    mDungeonMedia = new DungeonSceneMediator();
        //    mMediaDic.Add(MediaType.Dungeon, mDungeonMedia);
        //    mIsInit = true;
        //}
    }

    private void Update()
    {
        if (mCurActiveScene != null)
        {
            mCurActiveScene.Update();
        }

        //        if (mWorldSceneName.Equals(SceneResMgr.instance.curActiveScene, StringComparison.Ordinal))
        //        {
        //            mMapMedia?.Update();
        //        }
        //        else if(mHomeSceneName.Equals(SceneResMgr.instance.curActiveScene, StringComparison.Ordinal))
        //        {
        //            mHomeMedia?.Update();
        //        }
        //
        //        // 里面已经有判断;
        //        if (mDungeonMedia != null)
        //        {
        //            mDungeonMedia.Update();
        //        }
    }

    public IMapCameraScroller GetCameraByMediaType(MediaType type)
    {
        if (this.mMediaDic.TryGetValue(type, out IMapGlobalContext context))
        {
            return context?.cameraScroller;
        }
        return null;
    }
}
