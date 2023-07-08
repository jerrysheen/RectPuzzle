using System;
using System.Collections.Generic;
using UnityEngine;

public class MapUtils
{
	public const float GRID_SIZE = 1f * MapPlaceConfig.MAP_FIX_SCALE;
    
	public static int GlobalGridPosToGlobalIndex(int globalGridX, int globalGridY)
    {
	    //LogUtility.Log("xhj" + MapPlaceConfig.mapGlobalGridHeight + " : " + MapPlaceConfig.mapGlobalGridWidth) ;
	    if (globalGridY >= 0 && globalGridY < MapPlaceConfig.mapGlobalGridHeight && globalGridX >= 0 && globalGridX < MapPlaceConfig.mapGlobalGridWidth)
	    {
		    return globalGridY * MapPlaceConfig.mapGlobalGridWidth + globalGridX;
	    }
	    return -1;
    }

	public static void GlobalPosToWorldPos(float globalPosX, float globalPosY, out float worldPosX, out float worldPosZ)
	{
		worldPosX = globalPosX - MapPlaceConfig.mapBlockGridWidth * MapPlaceConfig.SERVER_ROW_COUNT / 2;
		worldPosZ = globalPosY - MapPlaceConfig.mapBlockGridWidth * MapPlaceConfig.SERVER_ROW_COUNT / 2;
	}
	

	public static void GlobalGridPosToWorldPos(int globalGridX, int globalGridY, out float x, out float y)
    {
		x = globalGridX * GRID_SIZE - MapPlaceConfig.halfMapGlobalGridWidth;
		y = globalGridY * GRID_SIZE - MapPlaceConfig.halfMapGlobalGridHeight;
    }
	
	public static void GlobalGridPosToDungeonPos(int globalGridX, int globalGridY, out float x, out float y)
	{
		x = globalGridX * GRID_SIZE;
		y = globalGridY * GRID_SIZE;
	}
	
	
	public static void WorldPosToGlobalGridPos(float x, float z, out int globalGridX, out int globalGridZ)
	{
		x = x + MapPlaceConfig.halfMapGlobalGridWidth;
		z = z + MapPlaceConfig.halfMapGlobalGridHeight;
		globalGridX = (int)(x / GRID_SIZE);
		globalGridZ = (int)(z / GRID_SIZE);
	}
	
	public static void CalculateGridByWorldCamera(Camera s_mainCamera, out int globalGridX, out int globalGridZ)
	{
		Vector3 leftTopPos = s_mainCamera.ScreenToWorldPoint(new Vector3(0, s_mainCamera.pixelHeight, 30));
		Vector3 rightTopPos = s_mainCamera.ScreenToWorldPoint(new Vector3(s_mainCamera.pixelWidth, s_mainCamera.pixelHeight, 30));
		Vector3 leftBottomPos = s_mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 30));

		Vector3 leftTopDir = leftTopPos - s_mainCamera.transform.position;
		Vector3 rightTopDir = rightTopPos - s_mainCamera.transform.position;
		Vector3 leftBottomDir = leftBottomPos - s_mainCamera.transform.position;

		leftTopPos = MapCalcConst.GetIntersectWithLineAndPlane(s_mainCamera.transform.position, leftTopDir, new Vector3(0, 1, 0),
			new Vector3(0, 0, 0));
		rightTopPos = MapCalcConst.GetIntersectWithLineAndPlane(s_mainCamera.transform.position, rightTopDir, new Vector3(0, 1, 0),
			new Vector3(0, 0, 0));
		leftBottomPos = MapCalcConst.GetIntersectWithLineAndPlane(s_mainCamera.transform.position, leftBottomDir, new Vector3(0, 1, 0),
			new Vector3(0, 0, 0));
		WorldPosToGlobalGridPos((leftTopPos.x + rightTopPos.x) * 0.5f, (leftTopPos.z + leftBottomPos.z) * 0.5f, out globalGridX, out globalGridZ);
	}
	
	public static void CalculateGridByDungeonCamera(Camera s_mainCamera, out int globalGridX, out int globalGridZ)
	{
		Vector3 leftTopPos = s_mainCamera.ScreenToWorldPoint(new Vector3(0, s_mainCamera.pixelHeight, 30));
		Vector3 rightTopPos = s_mainCamera.ScreenToWorldPoint(new Vector3(s_mainCamera.pixelWidth, s_mainCamera.pixelHeight, 30));
		Vector3 leftBottomPos = s_mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 30));

		Vector3 leftTopDir = leftTopPos - s_mainCamera.transform.position;
		Vector3 rightTopDir = rightTopPos - s_mainCamera.transform.position;
		Vector3 leftBottomDir = leftBottomPos - s_mainCamera.transform.position;

		leftTopPos = MapCalcConst.GetIntersectWithLineAndPlane(s_mainCamera.transform.position, leftTopDir, new Vector3(0, 1, 0),
			new Vector3(0, 0, 0));
		rightTopPos = MapCalcConst.GetIntersectWithLineAndPlane(s_mainCamera.transform.position, rightTopDir, new Vector3(0, 1, 0),
			new Vector3(0, 0, 0));
		leftBottomPos = MapCalcConst.GetIntersectWithLineAndPlane(s_mainCamera.transform.position, leftBottomDir, new Vector3(0, 1, 0),
			new Vector3(0, 0, 0));
		
		globalGridX = (int)((leftTopPos.x + rightTopPos.x) * 0.5f / GRID_SIZE);
		globalGridZ = (int)((leftTopPos.z + leftBottomPos.z) * 0.5f / GRID_SIZE);
	}

	public static Vector3 GetCameraPosLookAt(Vector3 localCameraPos, float cameraZ)
    {
	    return new Vector3(localCameraPos.x, 0f,localCameraPos.z - cameraZ);;
    }
	
	public static Vector3 GetCameraWorldPos(Vector3 localCameraPos, float cameraZ)
	{
		return new Vector3(localCameraPos.x, 0f,localCameraPos.z + cameraZ);;
	}

	public static bool GetGlobalGridPosFromScreenPos(Vector3 screenPos, out Vector2Int gridPos)
    {
	    if (null == Camera.main)
	    {
		    gridPos = Vector2Int.zero;
		    return false; 
	    }
        Vector3 worldPos = CameraUtil.GetWorldPos(Camera.main, screenPos, false);
        int gridX, gridZ;
        WorldPosToGlobalGridPos(worldPos.x, worldPos.z, out gridX, out gridZ);
        gridPos = new Vector2Int(gridX, gridZ);
        return true;
    }
	
	public static bool IsInsectBetween(ref RectInt a, ref RectInt b)
	{
		return a.xMin < b.xMax && a.xMax > b.xMin && a.yMin < b.yMax && a.yMax > b.yMin;
	}

	public static bool IsInsectBetween(int xMin, int yMin, int xMax, int yMax, RectInt b)
	{
		return xMin < b.xMax && xMax > b.xMin && yMin < b.yMax && yMax > b.yMin;
	}
	

}
