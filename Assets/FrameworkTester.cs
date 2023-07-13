using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameworkTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //WorldMapManager.instance.mCurActiveScene.curMediaType = MediaType.Home;
        //public void EnterHomeScene(string cityId, float worldPosX, float worldPosZ, int defaultLevel)
        WorldMapManager.instance.InitMgr();
        WorldMapManager.instance.mHomeMedia?.EnterHomeScene("test", 0.0f, 0.0f, 1);
        WorldMapManager.instance.mHomeMedia?.EnterMainCityInteract();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(CameraManager.GetMainCamera().name);
    }
}
