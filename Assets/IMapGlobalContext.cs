using System;
using UnityEngine;

public interface IMapGlobalContext
{

	int curGridY
	{
		get;
	}

	int curGridX
	{
		get;
	}
	

	int lod
	{
		get;
	}

	IMapCameraScroller cameraScroller {
		get;
	}


	RectInt viewRect
	{
		get;
	}
	

}
