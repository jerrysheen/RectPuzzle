using System;
using UnityEngine;
using System.Collections;

public interface IMapCameraScroller
{
	float maxScale
	{
		get;
		set;
	}

	float minScale
	{
		get;
		set;
	}

	float currentScale
	{
		get;
	}

	int currentLevel
	{
		get;
	}

	float fov { get; }

	void ResetAutoMoveCallback(bool notify);

	void CancelMove();

	void MoveCameraToGrid(Vector2Int targetGrid, float stepFactor = .4f, Action<bool, Vector3> callback = null);

	void MoveCameraToPosition (Vector3 worldPos, float stepFactor = .4f, Action<bool, Vector3> callback = null);

	void MoveCameraScaleTo (float value, float stepFactor = .4f, Action<bool, Vector3> callback = null);

	void MoveCameraQuickToGrid(Vector2Int targetGrid);

	void MoveCameraQuickToPosition (Vector3 worldPos);

	void MoveCameraToPositionWithScale(Vector3 worldPos, float scale, float stepFactor = .4f, Action<bool, Vector3> callback = null);

	void ShrinkCameraScale ();

	void ExpandCameraScale();

	void SetFovRange(float minFov, float maxFov);
	
	void ClearCamera();
	
	Vector3 GetCameraCenterWorldPos();
}

