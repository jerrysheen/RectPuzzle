using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapAsset : MonoBehaviour
{
    public static MapAsset instance = null; 
    
    public MeshResourceSetPack meshResSetPack = null;
    public CityCameraAsset cityCamAsset = null;
    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
