using System;
//using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///从 UIDraggableCamera 修改的用于场景中摄像机移动的类. allowing you to drag a secondary camera while keeping it constrained to a certain area.
/// </summary>
public class MapDraggableTarget : MonoBehaviour
{
	public Transform baseNode = null; 
	/// <summary>
	/// Scale value applied to the drag delta. Set X or Y to 0 to disallow dragging in that direction.
	/// </summary>
	public Vector2 scale = Vector2.one;

	/// <summary>
	/// Effect the scroll wheel will have on the momentum.
	/// </summary>

	public float scrollWheelFactor = 0.1f;

	/// <summary>
	/// Effect to apply when dragging.
	/// </summary>

	public DragEffect dragEffect = DragEffect.MomentumAndSpring;

	/// <summary>
	/// Whether the drag operation will be started smoothly, or if if it will be precise (but will have a noticeable "jump").
	/// </summary>

	public bool smoothDragStart = true;

	/// <summary>
	/// How much momentum gets applied when the press is released after dragging.
	/// </summary>

	public float momentumAmount = 35f;
	//滑动momentum惯性的截止值
	public float momentumThreshold = 1;

	public static bool LOCK_DRAG = false; // 全局参数，锁定拖动操作;
	Camera mCam;
	Transform mTrans;
	bool mPressed = false;
	Vector2 mMomentum = Vector2.zero;
	Vector2 mLastFrameDrag = Vector2.zero;
	bool mDragStarted = false;
	
	private Vector2 mPrevGesturePos;	// 拖动时,上一帧的触摸点,用来计算摄像机的平移距离

	private const int MaxDistanceCount = 5;
	private Vector2[] mDragDistance = new Vector2[MaxDistanceCount];	// 保存最后12帧的拖动位移
	private int mDragFillIndex = 0;		// 保存mDragDistance要存储到的位置
	private float strength = 1;
	/// <summary>
	/// Current momentum, exposed just in case it's needed.
	/// </summary>

	public Vector2 currentMomentum { get { return mMomentum; } set { mMomentum = value; } }

	/// <summary>
	/// Cache the root.
	/// </summary>
	private int layerMask = 0;
	public void Init()
	{
		if (WorldMapManager.instance.mCurActiveScene.curMediaType == MediaType.Home)
		{
			mCam = ((HomeSceneCameraScroller)WorldMapManager.instance.mHomeMedia.cameraScroller).mainCamera;
		}
		else if(WorldMapManager.instance.mCurActiveScene.curMediaType == MediaType.Dungeon)
		{
			mCam = ((DungeonCameraScroller)WorldMapManager.instance.mDungeonMedia.cameraScroller).mainCamera;
		}
		
		mCam.depthTextureMode |= DepthTextureMode.Depth;
		mTrans = transform;
		layerMask = LayerMask.GetMask("Terrain");
	}

	public delegate bool CheckOutOfBound(ref Vector3 localPos);
	public delegate void UpdateCamPosDelegate(Vector3 localPos);
	
	/// <summary>
	/// 限制摄像机位置的检测函数
	/// </summary>
	public CheckOutOfBound ConstrainCameraPos { get; set; }
	public UpdateCamPosDelegate UpdateCameraPos { get; set; }
	
	/// <summary>
	/// Calculate the offset needed to be constrained within the panel's bounds.
	/// </summary>

	Vector3 CalculateConstrainOffset ()
	{
		if (ConstrainCameraPos != null)
		{
			var curPos = mTrans.localPosition;
			var newCameraPos = curPos;
			ConstrainCameraPos(ref newCameraPos);
			return curPos - newCameraPos;
		}
		return Vector3.zero;
	}

	/// <summary>
	/// Constrain the current camera's position to be within the viewable area's bounds.
	/// </summary>

	public bool ConstrainToBounds (bool immediate)
	{
		if (mTrans != null)
		{
			Vector3 offset = CalculateConstrainOffset();

			if (offset.sqrMagnitude > 0f)
			{
				if (immediate)
				{
					mTrans.localPosition -= offset;
				}
				else
				{
					SpringPosition sp = SpringPosition.Begin(gameObject, mTrans.localPosition - offset, 13f);
					sp.ignoreTimeScale = true;
					sp.worldSpace = false;
				}
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Calculate the bounds of all widgets under this game object.
	/// </summary>

	public void Press (bool isPressed)
	{
		mDragStarted = false;
		
		mPressed = isPressed;
		
		if(isPressed)
		{
			// Remove all momentum on press
			mMomentum = Vector2.zero;

			// 清空最后几帧记录的距离
			for (var index = 0; index < mDragDistance.Length; ++index)
				mDragDistance[index] = Vector2.zero;

			// Disable the spring movement
			SpringPosition sp = GetComponent<SpringPosition>();
			if (sp != null) sp.enabled = false;
		}
		else if (dragEffect == DragEffect.MomentumAndSpring)
		{
			ConstrainToBounds(false);

			if (mLastFrameDrag.magnitude < 0.001f)
			{
				mMomentum = Vector2.zero;
			}
			else
			{
				int total = 0;
				Vector2 all = new Vector2();
				for (var index = 0; index < mDragDistance.Length; ++index)
				{
					if (Mathf.Abs(mDragDistance[index].x - 0)>0.001f|| Mathf.Abs(mDragDistance[index].y - 0)>0.001f)
					{           
						all = mDragDistance[index] + all;
						total++;
					}
				}

				if (total > 0)
				{	
					all = all * (1.0f / total);
					
					// Adjust the momentum
					mMomentum = Vector2.Lerp(mMomentum, mMomentum + all * (0.01f * momentumAmount), 0.67f);
					mMomentum.x = Mathf.Abs(mMomentum.x) * Mathf.Sign(mLastFrameDrag.x);
					mMomentum.y = Mathf.Abs(mMomentum.y) * Mathf.Sign(mLastFrameDrag.y);
					
				}
			}
		}
	}

	/// <summary>
	/// Drag event receiver.
	/// </summary>

	public void Drag (Gesture gesture)
	{
		if (LOCK_DRAG) return;
		// Prevents the initial jump when the drag threshold gets passed
		if (smoothDragStart && !mDragStarted)
		{
			mPrevGesturePos = gesture.position;	// 记录一下本帧的触摸点
			mDragStarted = true;
			return;
		}
		
		// 计算出屏幕触摸移动对应的摄像机移动距离. 注: 因为摄像机是透视的,所以需要用XY平面的相交点来计算摄像机的移动量
		UpdateTargetPos(ref mPrevGesturePos, ref gesture.position);
		mPrevGesturePos = gesture.position;

		// 保存最后5帧的数据
		mDragDistance[mDragFillIndex] = gesture.deltaPosition;
		mLastFrameDrag = mDragDistance[mDragFillIndex];
		mDragFillIndex++;
		mDragFillIndex = mDragFillIndex % MaxDistanceCount;
		
	}

	public void CancelDrag()
	{
		mDragStarted = false;
	}

	/// <summary>
	/// 由传入的上一帧的屏幕触摸位置与当前帧的屏幕触摸位置来计算摄像机的平移距离,并更新摄像机位置
	/// </summary>
	/// <param name="prevGesturePos">上一帧的屏幕触摸位置</param>
	/// <param name="curGesturePos">当前帧的屏幕触摸位置</param>
	private void UpdateTargetPos(ref Vector2 prevGesturePos, ref Vector2 curGesturePos)
	{
		if (mCam == null)
		{
			return;
		}
		// 计算出屏幕触摸移动对应的摄像机移动距离. 注: 因为摄像机是透视的,所以需要用XY平面的相交点来计算摄像机的移动量
		Vector3 prevSwipePos = CameraUtil.GetWorldPos(mCam, prevGesturePos);
		Vector3 currentSwipePos = CameraUtil.GetWorldPos(mCam, curGesturePos);
		prevSwipePos = baseNode.worldToLocalMatrix.MultiplyPoint3x4(prevSwipePos);
		currentSwipePos = baseNode.worldToLocalMatrix.MultiplyPoint3x4(currentSwipePos);
		
		var newPos = mTrans.localPosition + new Vector3(-(currentSwipePos.x - prevSwipePos.x), 0, -(currentSwipePos.z - prevSwipePos.z));
		ConstrainCameraPos(ref newPos);
		mTrans.localPosition = newPos;
		UpdateCameraPos(newPos);

		//UITrackerManager.GetInstance().UpdateTrackersPosImmediately();
	}

    /// <summary>
    /// Apply the dragging momentum.
    /// </summary>
    private SpringPosition sp;
    void Update ()
	{
        //float delta = RealTime.deltaTime;
        float delta = Time.deltaTime;
        bool highSpeed = false;

		do
		{
			if (mPressed)
			{
				// Disable the spring movement
				if (sp == null)
					sp = GetComponent<SpringPosition>();

				if (sp != null && sp.enabled) sp.enabled = false;
			}
			else
			{
				if (mMomentum.sqrMagnitude > momentumThreshold && !IsMoveStop())
				{
					highSpeed = true;

					// Apply the momentum
					var offset = NGUIMath.SpringDampen(ref mMomentum, strength, delta);
					var curGesturePos = mPrevGesturePos + offset;

					UpdateTargetPos(ref mPrevGesturePos, ref curGesturePos);
					mPrevGesturePos = curGesturePos;

					if (!ConstrainToBounds(dragEffect == DragEffect.None))
					{
						if (sp == null)
							sp = GetComponent<SpringPosition>();

						if (sp != null) sp.enabled = false;
					}

					break;
				}
			}
			
			// Dampen the momentum
			NGUIMath.SpringDampen(ref mMomentum, strength, delta);
		} while (false);

#if !NO_LIMIT_FRAME && !UNITY_EDITOR
		// 在拖动时,我们希望有更高的帧速率,以便获取更流畅的体验
		if (highSpeed || mDragStarted)
		{
			if (Application.targetFrameRate != 60)
				Application.targetFrameRate = 60;
		}
		else{
			if(Application.targetFrameRate != 30)
				Application.targetFrameRate = 30;
		}
#endif

		bool IsMoveStop()
		{
			float fDistance = 0f;
			foreach (var moveDelta in mDragDistance)
			{
				fDistance += moveDelta.magnitude;
			}

			return fDistance <= scrollWheelFactor;
		}
		AdjustCamHorizontalPlane();
	}
    
    private Vector3 sourcePos = new Vector3(0,200,0);
    private Vector3 tempTargetPos = new Vector3(0,0,0);
    
    private Vector3 prePos = new Vector3(0,0,0);
    private float tolerate = 0.001f;
    public void AdjustCamHorizontalPlane(bool force = false)
    {
	    if (baseNode == null)
	    {
		    return;
	    }
	    //位置移动检测地表高度
	    if (Mathf.Abs(prePos.x - transform.position.x) < tolerate &&
	        Mathf.Abs(prePos.z - transform.position.z) < tolerate && force!=true)
	    {
		    return;
	    }

	    prePos = transform.position;
	    Ray ray = new Ray(transform.position + sourcePos, Vector3.down);
	    RaycastHit hit;
	    if (Physics.Raycast(ray, out hit, 400, layerMask))
	    {
		    var tempH = hit.point.y;
		    if (tempH < CityConst.instance.BaseHeight)
		    {
			    tempH = CityConst.instance.BaseHeight;
		    }

		    if (Mathf.Abs(tempH - transform.position.y) > tolerate)
		    {
			    tempTargetPos.y = tempH;
			    baseNode.position = tempTargetPos;
		    }
	    }
    }

    public void SetStrength(float value)
    {
	    strength = value;
    }
}
