using UnityEngine;
//using DG.Tweening;

public class CameraUtil : MonoBehaviour 
{ 
	public enum DragMode
	{
		None,
		Drag,
		Scroll
	}
	public static Plane GROUND_PLANE = new Plane(Vector3.up, Vector3.zero);
	private static Plane XYGROUND_PLANE = new Plane(Vector3.forward, Vector3.zero);
	public static int camLayer = LayerMask.GetMask("Terrain");
    public static Vector3 GetWorldPos(Camera camera, Vector2 screenPos, float distance)
    {
        Ray ray = camera.ScreenPointToRay( screenPos );
        return ray.GetPoint(distance);
    }

    public static Vector3 GetWorldPos(Camera camera, Vector2 screenPos, bool useXYPlane = false)
    {
	    Ray ray = camera.ScreenPointToRay( screenPos );
		
	    RaycastHit hit;
	    if (Physics.Raycast(ray, out hit, Mathf.Infinity,camLayer ))
	    {
		    return hit.point;
	    }
	    //没检测到地表选择0水平面
	    Plane plane = useXYPlane ? XYGROUND_PLANE : GROUND_PLANE;
	    float t;
	    plane.Raycast (ray, out t);
	    return ray.GetPoint( t );
    }


    public static Vector3 GetWorldPos(Vector2 screenPos, bool useXYPlane = false)
    {
	    Camera mainCamera = CameraManager.GetMainCamera();
	    if (mainCamera == null)
		    return Vector3.zero;
	    
	    return GetWorldPos(mainCamera, screenPos, false);
    }
	
	public static Vector3 GetScreenPos(Camera camera, Camera uiCamera, Vector3 worldPos)
	{
		Vector3 pos = camera.WorldToViewportPoint(worldPos);
		pos.z = 0;
		return uiCamera.ViewportToWorldPoint(pos);
	} 

    public static Vector3 GetScreenPosEx(Camera camera, Camera uiCamera, Vector3 worldPos, out bool isReverse)
    {
        Vector3 pos = camera.WorldToViewportPoint(worldPos);
//        //DebugUtil.Log(pos);
        if(pos.z <= 0)
        {
            isReverse = true;
        }
        else
        {
            isReverse = false;
        }
        pos.z = 0;
        return uiCamera.ViewportToWorldPoint(pos);
    }

	public static void ShakeScreen(Camera camera, float time, float amountX = 1f, float amountY = 1f, float amountZ = 1f, float delay = 0f)
	{
		//		paramTbl["amount"] = new Vector3(amountX, amountY, amountZ);
		//		paramTbl["islocal"] = true;
		//		paramTbl["time"] = time;
		//		paramTbl["space"] = Space.Self;
		//		iTween.ShakePosition(Camera.main.gameObject, paramTbl);
		if (camera != null)
		{
			//camera.DOShakePosition(time, new Vector3(amountX, amountY, amountZ)).SetDelay(delay);
		}
	}
}
