
using System.CodeDom;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 客户端model.data:|1bit阻1通0|7bit类型|
/// 
/// 服务器map.bytes:|0|0000000|
/// |1bit阻1通0|7bit区域|
/// </summary>
public static class MapPlaceCategory
{
	// 0x00 ~ 0x7f ：非阻挡;
	// 0x80 ~ 0xff ：阻挡;
	
	public const byte NONE = 0x0;
	public const byte PASS_MOUNTAIN = 0x1;//关卡山脉;
	public const byte PASS = 0x2;	//关卡;
	public const byte PASS_CHANCEL = 0x3;// 圣坛;
	public const byte PASS_TEMPLE = 0x4;// 神庙;
	public const byte RIVER = 0x5;// 河流;

	public const byte FOREST = 0x6;	// 森林;
	public const byte MOUNTAIN = 0x7;	// 山;

	public const byte RES_AREA = 0xf; // 资源带【编辑器用，没啥意义】;

	// 用于检测阻挡的掩码;
	public const byte BLOCK_MASK = 0x80;				// 阻挡mask值;
	
}

public class MapPlaceConfig
{
	// public const int MAP_BLOCK_WIDTH = 3;
	/// <summary>
	/// 世界地图缩放倍数;
	/// </summary>
	public const int MAP_FIX_SCALE = 5;

	public static int mapBlockGridCnt = 1200;

	public static int mapBlockGridWidth = 1200 * MAP_FIX_SCALE;
	public static int mapBlockGridHeight = 1200 * MAP_FIX_SCALE;

	public static int originGlobalMapWidth = 1200;
	public static int originGlobalMapHeight = originGlobalMapWidth;
	
	public static int mapGlobalGridWidth = originGlobalMapWidth * MAP_FIX_SCALE;
	public static int mapGlobalGridHeight = originGlobalMapHeight * MAP_FIX_SCALE;
	

	public static int halfMapGlobalGridWidth = mapGlobalGridWidth/2;
	public static int halfMapGlobalGridHeight = mapGlobalGridHeight/2;
	
	public const int AOI_CELL_WIDTH = 10;
	public const int SERVER_ROW_COUNT = 1;
	/// <summary>
	/// 格子尺寸【10】
	/// </summary>
    public static int SHOW_TILE_SIZE;

	public static void SetMapPlaceConfig(int width,int height)
	{
		mapBlockGridCnt = width;
		mapBlockGridWidth = width * MAP_FIX_SCALE;
		mapBlockGridHeight = height * MAP_FIX_SCALE;
		originGlobalMapWidth = width;
		originGlobalMapHeight = height;
		mapGlobalGridWidth = width * MAP_FIX_SCALE;
		mapGlobalGridHeight = height * MAP_FIX_SCALE;
		halfMapGlobalGridWidth = mapGlobalGridWidth/2;
		halfMapGlobalGridHeight = mapGlobalGridHeight/2;
	}
}
