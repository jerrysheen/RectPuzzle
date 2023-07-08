
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapCameraInfo
{
	public float y;
	public float z;
	//[MinValue(0.1f)]
	public float fov;
	//[MinValue(0.1f)]
	public float strength = 12;
	//[MinValue(0.1f)]
	public float nearClip = 3;
	public float farClip = 300;
}
public class MapCalcConst
{
	public const int DEFAULT_VISIBLE_WIDTH = 24;
	public const int DEFAULT_VISIBLE_HEIGHT = 12;
	public static float MAP_CAMERA_MOVE_SPPED = 0.02f;
	public const int LOD_NUM = 9;

	public const float DEFAULT_MIN_SCALE = -2f;

	public const float DEFAULT_MAX_SCALE = 300f;//MIN_Z + VIEW_DISTANCE_ADDITION * LOD_NUM;

//	public static float CITY_Z = 28f;
//	public static float HOME_MIN_FOV = 13.5f;
//	public static float HOME_MAX_FOV = 30;

	private static MapCameraInfo[] world_lod_cam_info_list = null;
	private static Dictionary<int, MapCameraInfo[]> worldLodList = new Dictionary<int, MapCameraInfo[]>();
	public static float world_default_height = 70;
	public static float world_cache_size_height = 70;
	public static float EASY_TOUCH_LONG_TAP_TIME = 0.5f;
	/// <summary>
	/// 元素是10个，但LOD分九层 0~1、1~2、2~3等等;
	/// </summary>
	public static MapCameraInfo[] LOD_CAM_INFO_LIST(CameraLodParam list)
	{
		int id = list.id; 
		if (worldLodList == null || !worldLodList.ContainsKey(id))
		{
			if (list == null)
				return null;
			int count = list.lodLayerRange[1] - list.lodLayerRange[0] + 1;
			world_lod_cam_info_list = new MapCameraInfo[count];
			for (int i = 0; i < count; i++)
			{
				MapCameraInfo camInfo = new MapCameraInfo();
				world_lod_cam_info_list[i] = camInfo;

				camInfo.y = (float) list.y[i];
				camInfo.fov = (float) list.fov[i];
				camInfo.nearClip = list.nearClip[i];
				camInfo.farClip = list.farClip[i];
			}
			worldLodList.Add(id, world_lod_cam_info_list);
		}
		else
			world_lod_cam_info_list = worldLodList[id];
		
		world_default_height = world_lod_cam_info_list[2].y;
		world_cache_size_height = world_lod_cam_info_list[1].y;
		
		return world_lod_cam_info_list;
	}

//	public static MapCameraInfo[] LOD_CAM_INFO_LIST(int id = 1001)
//	{
//		if (world_lod_cam_info_list == null)
//		{
//			ConfWorldLodParamList list = ConfHelper.GetConfWorldLodParamList();
//			if (list == null)
//				return null;
//
//			int idx = 0;
//			int count = list.list.Count;
//			world_lod_cam_info_list = new MapCameraInfo[count];
//			foreach (ConfWorldLodParam lodParam in list.list)
//			{
//				MapCameraInfo camInfo = new MapCameraInfo();
//				world_lod_cam_info_list[idx++] = camInfo;
//				
//				camInfo.y = (float)lodParam.y;
//				camInfo.fov = (float)lodParam.fov;
//				camInfo.nearClip = lodParam.nearClip;
//				camInfo.farClip = lodParam.farClip;
//			}
//
//			world_default_height = world_lod_cam_info_list[2].y;
//			world_cache_size_height = world_lod_cam_info_list[1].y;
//		}
//
//		return world_lod_cam_info_list;
//	}
	
	private static List<Vector2> s_cachedViewSizeList = new List<Vector2>();
	private static Camera s_mainCamera = null;

	public static void LoadConf()
	{
//		MapCalcConst.CITY_Z = float.Parse(ConfHelper.GetConfDataConstant("city_world_z").value);
//		MapCalcConst.HOME_MIN_FOV = float.Parse(ConfHelper.GetConfDataConstant("h_min_fov").value);
//		MapCalcConst.HOME_MAX_FOV = float.Parse(ConfHelper.GetConfDataConstant("h_max_fov").value);
	}
	
	public static void CalcCurrentViewRectSize(float currentScale, int lod, out int width, out int height)
	{
		s_mainCamera = CameraManager.GetMainCamera();
		int viewRectIndex = (int)currentScale - 1;
		if (viewRectIndex < 0)
		{
			viewRectIndex = 0;
		}

		while (s_cachedViewSizeList.Count < viewRectIndex + 1)
		{
			s_cachedViewSizeList.Add(Vector2.zero);
		}

		Vector2 pos = s_cachedViewSizeList[viewRectIndex];
		if (pos.Equals(Vector2.zero))
		{
			if (s_mainCamera == null)
			{
				width = 2;
				height = 2;
				return;
			}

			Vector3 leftTopPos = s_mainCamera.ScreenToWorldPoint(new Vector3(0, s_mainCamera.pixelHeight, 30));
			Vector3 rightTopPos = s_mainCamera.ScreenToWorldPoint(new Vector3(s_mainCamera.pixelWidth, s_mainCamera.pixelHeight, 30));
			Vector3 leftBottomPos = s_mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 30));
			Vector3 rightBottomPos = s_mainCamera.ScreenToWorldPoint(new Vector3(s_mainCamera.pixelWidth, 0, 30));

			Vector3 leftTopDir = leftTopPos - s_mainCamera.transform.position;
			Vector3 rightTopDir = rightTopPos - s_mainCamera.transform.position;
			Vector3 leftBottomDir = leftBottomPos - s_mainCamera.transform.position;
			Vector3 rightBottomDir = rightBottomPos - s_mainCamera.transform.position;

            leftTopPos = GetIntersectWithLineAndPlane(s_mainCamera.transform.position, leftTopDir, new Vector3(0, 1, 0),
                new Vector3(0, 0, 0));
            rightTopPos = GetIntersectWithLineAndPlane(s_mainCamera.transform.position, rightTopDir, new Vector3(0, 1, 0),
                new Vector3(0, 0, 0));
            leftBottomPos = GetIntersectWithLineAndPlane(s_mainCamera.transform.position, leftBottomDir, new Vector3(0, 1, 0),
                new Vector3(0, 0, 0));
            rightBottomPos = GetIntersectWithLineAndPlane(s_mainCamera.transform.position, rightBottomDir, new Vector3(0, 1, 0),
                new Vector3(0, 0, 0));

            pos = new Vector2(Math.Abs((leftTopPos.x - rightTopPos.x)/MapPlaceConfig.MAP_FIX_SCALE)+5, 
					Math.Abs((leftTopPos.z - rightBottomPos.z)/MapPlaceConfig.MAP_FIX_SCALE)+5);
            s_cachedViewSizeList[viewRectIndex] = pos;
		}

		//by[hxq]调整地图拖拽数据加载最小范围，解决放大主城下看其他玩家主城不平滑问题
		width = Math.Max(7, (int)Math.Ceiling(pos.x));
		height = Math.Max(4, (int)Math.Ceiling(pos.y));
	}

	/**
	 * 计算当前场景相机中心点在沙盘上的世界坐标
	 */
	public static Vector3 CalcCameraLookAtPos(Camera cam)
	{
		s_mainCamera = cam;
		Vector3 centerPos = s_mainCamera.ScreenToWorldPoint(new Vector3(s_mainCamera.pixelWidth*0.5f, s_mainCamera.pixelHeight*0.5f, 30));
		Vector3 centerDir = centerPos - s_mainCamera.transform.position;
		//求得向量与平面交点
		centerPos = GetIntersectWithLineAndPlane(s_mainCamera.transform.position, centerDir, new Vector3(0, 1, 0),
			new Vector3(0, 0, 0));
		return centerPos;
	}

	public static Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
	{
		float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);
		//print(d);
		return d * direct.normalized + point;
	}
}
