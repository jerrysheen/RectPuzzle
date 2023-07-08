using System;
using UnityEngine;

public class DungeonCameraScroller : MapSceneCameraScroller
{
	protected override bool CheckOutOfBound(ref Vector3 localPos)
	{
		if (mDraggableCamera == null)
		{
			return false;
		}

		float worldPosY = localPos.z + _currentScale;
		
		bool xOverflow = localPos.x < _boundXMin || localPos.x > _boundXMax;
		bool yOverflow = worldPosY < _boundYMin || worldPosY > _boundYMax;
		if (xOverflow || yOverflow)
		{
			if (xOverflow)
			{
				localPos.x = Mathf.Clamp(localPos.x, _boundXMin, _boundXMax);
			}
			if (yOverflow)
			{
				localPos.z = Mathf.Clamp(worldPosY, _boundYMin, _boundYMax);
				localPos.z -= _currentScale;
			}
			return true;
		}
		return false;
	}
	
	protected override void GlobalGridToPos(int globalGridX, int globalGridY, out float x, out float y)
	{
		//MapUtils.GlobalGridPosToDungeonPos(globalGridX, globalGridY, out x, out y);
		x = 0; y = 0;
	}
}
