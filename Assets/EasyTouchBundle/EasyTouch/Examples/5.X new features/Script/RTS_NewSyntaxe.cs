using UnityEngine;
using System.Collections;
using HedgehogTeam.EasyTouch;

public class RTS_NewSyntaxe : MonoBehaviour {


	private GameObject camHolder;
	void Start () {
        camHolder = this.transform.Find("CameraHolder").gameObject;
		if (!camHolder) 
		{
			Debug.LogError("Can't get holder");
		}
    }
	
	void Update () {
	
		Gesture current = EasyTouch.current;

		//// Cube
		//if (current.type == EasyTouch.EvtType.On_SimpleTap && current.pickedObject !=null && current.pickedObject.name=="Cube"){
		//	ResteColor();
		//	cube = current.pickedObject;
		//	cube.GetComponent<Renderer>().material.color = Color.red;
		//	//transform.Translate(Vector2.up, Space.World);
		//}

		// Swipe
		if (current.type == EasyTouch.EvtType.On_Swipe && current.touchCount == 1){
			transform.Translate(-camHolder.transform.right * current.deltaPosition.x * 0.01f);
			transform.Translate(-camHolder.transform.forward * current.deltaPosition.y * 0.01f);
		}

		//// Pinch
		if (current.type == EasyTouch.EvtType.On_Pinch)
		{
			Camera.main.transform.position += current.deltaPinch * 3.0f * Time.deltaTime * Camera.main.transform.forward;
		}

		// Twist
		if (current.type == EasyTouch.EvtType.On_Twist ){
			transform.Rotate( Vector3.up * current.twistAngle);
		}
	}

}
