using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLodParam : ScriptableObject
{
    public int id;  // 场景id
    public string comment; // 注释 仅在编辑器当中使用
    public int[] lodLayerRange; // lod应用的层级范围
    public float[] y; // 高度
    public float[] fov; // 相机fov
    public int[] nearClip;  // 相机近处裁剪
    public int[] farClip;  // 相机远处裁剪

    private string[] lods;
    public void SetValue(int sceneId,string label, int minLod,int maxLod)
    {
        id = sceneId;
        comment = label;
        int len = maxLod - minLod  + 1;
        lodLayerRange = new int[2];
        lodLayerRange[0] = minLod;
        lodLayerRange[1] = maxLod;
        y = new float[len];
        fov = new float[len];
        nearClip = new int[len];
        farClip = new int[len];
    }

    public string[] GetLodRangeByStringList()
    {
        if (lods != null)
        {
            return lods;
        }
        if (lodLayerRange != null && lodLayerRange.Length > 0)
        {
            int len = lodLayerRange[1] - lodLayerRange[0] + 1;
            lods = new string[len];
            for (int i = 0; i < len; i++)
            {
                lods[i] = (lodLayerRange[0] + i).ToString();
            }
            return lods;
        }
        return null;
    }
}