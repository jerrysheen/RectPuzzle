using System;
using System.Collections;
using System.Collections.Generic;
//using LuaInterface;
using UnityEngine;

public class DungeonSceneMediator : MapSceneMediatorBase
{
   // public override MediaType curMediaType
   // {
   //     get { return MediaType.Dungeon; }
   // }

   // protected override void InitCamera()
   // {
   //     Camera mainCamera = CameraManager.GetMainCamera();
   //     _cameraTrans = mainCamera.transform.parent;
   //     _cameraScroller = new DungeonCameraScroller();
   //     _viewDistance = -1;
   // }

   // protected override void CalculateGridByCamera(out int gridX, out int gridY)
   // {
   //     MapUtils.CalculateGridByDungeonCamera(_cameraScroller.mainCamera, out gridX, out gridY);
   // }

   // public void SetSceneSize(float mapWidth, float mapHeight)
   // {
   //     _cameraScroller?.SetSwipeBound(0, 0,
   //         MapUtils.GRID_SIZE * mapWidth, MapUtils.GRID_SIZE * mapHeight);
   // }

   // protected override void ShowWorldMap()
   //{
   //    if (_inited)
   //    {
   //        return;
   //    }
   //    _inited = true;
   //    if (_itemContainer == null)
   //    {
   //        _itemContainer = MapRoot.AddMapObject("ItemContainer").transform;
   //    }
       
   //    _viewportChangeHandler = new DefaultMapViewPortChangedHandler(new IMapDisplayDataProvider[MapCalcConst.LOD_NUM]{
   //        new MapLevel0DisplayDataProvider(),
   //        new MapLevel1DisplayDataProvider(),
   //        new MapLevel2DisplayDataProvider(),
   //        new MapLevel3DisplayDataProvider(),
   //        new MapLevel4DisplayDataProvider(),
   //        new MapLevel5DisplayDataProvider(),
   //        new MapLevel6DisplayDataProvider(),
   //        new MapLevel7DisplayDataProvider(),
   //        new MapLevel8DisplayDataProvider()
   //    });
   // }
}
