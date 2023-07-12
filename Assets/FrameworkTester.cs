using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameworkTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //WorldMapManager.instance.mCurActiveScene.curMediaType = MediaType.Home;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CameraManager.GetMainCamera().name);
    }
}
