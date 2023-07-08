using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class CityCameraAsset :ScriptableObject
{
    //基础地表水平高度 地表低于此不会降低相机基本平面高度  高于此会提高相机基本平面高度 
    public float baseHeight = 0;
    public float pinchFactor = 1;
    //相机竖向移动节点
    public List<MapCameraInfo> points = new List<MapCameraInfo>();
    //相机移动范围中心位置 世界坐标
    public float centerX = 0;
    public float centerY = 0;
    
    //相机拉到最低移动范围
    public float lowLeft = 0;
    public float lowRight = 0;
    public float lowTop = 0;
    public float lowBottom = 0;
    //相机拉到最高移动范围
    public float highLeft = 0;
    public float highRight = 0;
    public float highTop = 0;
    public float highBottom = 0;
    public int defaultLevel = 0;
    //相机惯性配置
    public float momentumAmount = 35;
}
