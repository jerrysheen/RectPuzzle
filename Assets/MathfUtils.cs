using System;
using System.Collections.Generic;
using UnityEngine;

public static class MathExt
{
    public static float ToFixed(this float value) {
        return Mathf.Floor(Mathf.Abs(value * 1000)) * 0.001f * value.ToDirection();
    }
    
    public static int ToDirection(this float value) {
        return value >= 0 ? 1 : -1;
    }
    
    public static Vector3 ToFixed(this Vector3 value) {
        value.x = value.x.ToFixed();
        value.y = value.y.ToFixed();
        value.z = value.z.ToFixed();
        return value;
    }
    
    public static Vector2 ToFixed(this Vector2 value) {
        value.x = value.x.ToFixed();
        value.y = value.y.ToFixed();
        return value;
    }

    public static int GetFixedHashCode(this float value)
    {
        return (Mathf.RoundToInt(value * 100) * 0.01f).GetHashCode();
    }
}

public class MathfUtils
{
    public const float MIN_DIS = 0.01f;
    
    public const float EPSILON = 0.000001f;
    public const float FLOAT_THRESHHOLD = 0.0001f;
    

    /**
     * 两点之间快速距离计算
     * 
     * @param p1x
     * @param p1y
     * @param p2x
     * @param p2y
     * @return
     */
    public static float fastApproxDistance(float p1x, float p1y, float p2x, float p2y)
    {
        p1x = (p1x > p2x) ? (p1x - p2x) : (p2x - p1x);
        p1y = (p1y > p2y) ? (p1y - p2y) : (p2y - p1y);
        return (p1x > p1y) ? (p1x + 0.42f * p1y) : (p1y + 0.42f * p1x);
    }

    /**
     * 两点之间快速距离计算
     * 
     * @param p1x
     * @param p1y
     * @param p2x
     * @param p2y
     * @return
     */
    public static float fastApproxDeltaDistance(float deltax, float deltay)
    {
        deltax = (deltax > 0) ? deltax : -deltax;
        deltay = (deltay > 0) ? deltay : -deltay;
        return (deltax > deltay) ? (deltax + 0.42f * deltay) : (deltay + 0.42f * deltax);
    }

    /**
     * 两点之间快速距离计算
     * 
     * @param p1x
     * @param p1y
     * @param p1z
     * @param p2x
     * @param p2y
     * @param p2z
     * @return
     */
    public static float fastApproxDistance(float p1x, float p1y, float p1z, float p2x, float p2y, float p2z)
    {
        p1x = (p1x > p2x) ? (p1x - p2x) : (p2x - p1x);
        p1y = (p1y > p2y) ? (p1y - p2y) : (p2y - p1y);
        p1z = (p1z > p2z) ? (p1z - p2z) : (p2z - p1z);

        p1x = (p1x > p1y) ? (p1x + 0.42f * p1y) : (p1y + 0.42f * p1x);
        return (p1x > p1z) ? (p1x + 0.42f * p1z) : (p1z + 0.42f * p1x);
    }

    /**
     * 两点之间快速距离计算
     * 
     * @param p1x
     * @param p1y
     * @param p2x
     * @param p2y
     * @return
     */
    public static float fastApproxDeltaDistance(float deltax, float deltay, float deltaz)
    {
        deltax = (deltax > 0) ? deltax : -deltax;
        deltay = (deltay > 0) ? deltay : -deltay;
        deltaz = (deltaz > 0) ? deltaz : -deltaz;
        deltax = (deltax > deltay) ? (deltax + 0.42f * deltay) : (deltay + 0.42f * deltax);
        return (deltax > deltaz) ? (deltax + 0.42f * deltaz) : (deltaz + 0.42f * deltax);
    }

    public static float RegularAngle(float angle)
    {
        if (angle >= 360)
        {
            while (angle >= 360)
            {
                angle -= 360;
            }
        }
        else if (angle < 0)
        {
            while (angle < 0)
            {
                angle += 360;
            }
        }
        return angle;
    }

    public static bool Approximately(float f1, float f2)
    {
        return (Math.Abs(f1 - f2) < EPSILON);
    }
    
    public static bool Approximately(double f1, double f2)
    {
        return (Math.Abs(f1 - f2) < FLOAT_THRESHHOLD);
    }

    public static float GetDistanceEx(Vector3 pos1, Vector3 pos2)
    {
        float absX = Mathf.Abs(pos1.x - pos2.x);
        float absZ = Mathf.Abs(pos1.z - pos2.z);
        return (absX > absZ) ? absX : absZ;
    }

    public static float GetDistance(Vector3 pos1, Vector3 pos2)
    {
        pos1.y = pos2.y;
        return Vector3.Distance(pos1, pos2);
    }
    
    public static float GetXZSqrtDistance(Vector3 pos1, Vector3 pos2)
    {
        float x = pos1.x - pos2.x;
        float z = pos1.z - pos2.z;
        return x * x + z * z;
    }
    
    public static bool IsXZEqual(Vector3 pos1, Vector3 pos2)
    {
//         float xOffset = pos1.x - pos2.x;
//         float zOffset = pos1.z - pos2.z;
        float squareDistance = GetDistanceEx(pos1, pos2);
        if (squareDistance < FLOAT_THRESHHOLD)
        {
            return true;
        }
        return false;
    }

    public static bool IsEqualEx(Vector3 pos1, Vector3 pos2)
    {
        if(Mathf.Abs(pos1.x - pos2.x) > FLOAT_THRESHHOLD 
            || Mathf.Abs(pos1.y - pos2.y) > FLOAT_THRESHHOLD 
            || Mathf.Abs(pos1.z - pos2.z) > FLOAT_THRESHHOLD)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// [11,11,11] => Vector3;
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector3 ConvertToVector3(string value)
    {
        Vector3 result = Vector3.zero;
        if (string.IsNullOrEmpty(value))
        {
            return result;
        }

        string[] valueList = value.Split(new[] { '[', ',', ']' });
        if (valueList.Length < 4)
        {
            return result;
        }
        float.TryParse(valueList[1], out result.x);
        float.TryParse(valueList[2], out result.y);
        float.TryParse(valueList[3], out result.z);
        return result;
    }

    public static bool IsApproximatelyEqualTo(double initialValue, double value)
    {
        return IsApproximatelyEqualTo(initialValue, value, 0.00001);
    }

    public static bool IsApproximatelyEqualTo(double initialValue, double value, double maximumDifferenceAllowed)
    {
        // Handle comparisons of floating point values that may not be exactly the same
        return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
    }
    
    public static bool ContainsPoint(List<Vector2> polyPoints, Vector2 p)
    {
        var j = polyPoints.Count - 1;
        var inside = false;
        for (int i = 0; i < polyPoints.Count; j = i++)
        {
            var pi = polyPoints[i];
            var pj = polyPoints[j];
            if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                inside = !inside;
        }

        return inside;
    }
    
    
    /// <summary>
    /// 支持凹多边形
    /// </summary>
    /// <param name="polyPoints"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
    {
        int len = polyPoints != null ? polyPoints.Length : 0;
        if (len == 0)
            return false;
        
        int i, j, c = 0;
        for (i = 0, j = len - 1; i < len; j = i++)
        {
            Vector2 iPos = polyPoints[i];
            Vector2 jPos = polyPoints[j];
            if (((iPos.y > p.y) != (jPos.y > p.y)) &&
                (p.x < (jPos.x - iPos.x) * (p.y - iPos.y) / (jPos.y - iPos.y) + iPos.x))
            {
                c = 1 + c;
            }
        }
        if (c % 2 == 0)
        {
            return false;
        }
        return true;
    }
    
    public static float Cross(Vector2 a, Vector2 b)
    {
        return (a.x * b.y) - (b.x * a.y);
    }

    public static bool PointAtLine(Vector2 point, Vector2 p1, Vector2 p2)
    {
        return point.x >= Math.Min(p1.x, p2.x) && point.x <= Math.Max(p1.x, p2.x)
            && point.y >= Math.Min(p1.y, p2.y) && point.y <= Math.Max(p1.y, p2.y);
    }

    /// <summary>
    /// 求两个线段的交点 https://blog.csdn.net/luoyikun/article/details/115485938
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <param name="IntrPos"></param>
    /// <returns>0不想交，1相交，2or3平行相交</returns>
    public static int SegmentsInterPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 d, ref Vector2 IntrPos, bool checkParallel = false)
    {

        //v1×v2=x1y2-y1x2 
        //以线段ab为准，是否c，d在同一侧
        Vector2 ab = b - a;
        Vector2 ac = c - a;
        float abXac = Cross(ab,ac);

        Vector2 ad = d - a;
        float abXad = Cross(ab, ad);

        if (abXac * abXad >= 0)
        {
            // 平行相交的情况 todo 只有一个cross为零则是T相交的情况暂时没处理
            if (checkParallel && abXac == 0 && abXad == 0){
                if (PointAtLine(c, a, b)){
                    IntrPos = c;
                    return 2; 
                }
                if (PointAtLine(d, a, b)){
                    IntrPos = d;
                    return 2; 
                }
            }
            return 0;
        }

        //以线段cd为准，是否ab在同一侧
        Vector2 cd = d - c;
        Vector2 ca = a - c;
        Vector2 cb = b - c;

        float cdXca = Cross(cd, ca);
        float cdXcb = Cross(cd, cb);
        if (cdXca * cdXcb >= 0)
        {
            // 平行相交的情况
            if (checkParallel && cdXca == 0 && cdXcb == 0){
                if (PointAtLine(a, c, d)){
                    IntrPos = a;
                    return 3; 
                }
                if (PointAtLine(b, c, d)){
                    IntrPos = b;
                    return 3; 
                }
            }
            return 0;
        }
        //计算交点坐标  
        float t = (Cross(a -c, d -c) / Cross (d-c,b-a));
        float dx = t * (b.x - a.x);
        float dy = t * (b.y - a.y);

        IntrPos = new Vector2(a.x + dx, a.y + dy); // .ToFixed();
        return 1;
    }
}
