
//using LuaInterface;
//using UnityEngine;

//public static class CityViewHelper
//{
//    public static HomeSceneView TryGetCityView(string cityId = "")
//    {
//        return WorldMapManager.instance.mHomeMedia?.HomeScene;
//    }

//    // 点击Home场景
//    public static void OnTouchHomeScene(float x, float y, int gX, int gY, int touchEvent, float gestureX, float gestureY)
//    {
//        LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("MapUtil4CS.OnTouchHomeScene");
//        if (luaFunc != null)
//        {
//            luaFunc.Invoke<float, float, int, int, int, float, float, bool>(x, y, gX, gY, touchEvent, gestureX, gestureY);
//        }
//    }
//    // Home拖动拦截事件
//    public static void OnHomeDragInterceptor(float x, float y, int gX, int gY, int touchEvent)
//    {
//        LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("MapUtil4CS.OnHomeDragInterceptor");
//        if (luaFunc != null)
//        {
//            luaFunc.Invoke<float, float, int, int, int, bool>(x, y, gX, gY, touchEvent);
//        }
//    }
    
//    // 设置选择CityNode的数据
//    public static void SetSelectHomeNode(string uId, bool isBuild)
//    {
//        LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("MapUtil4CS.SelectHomeNode");
//        if (luaFunc != null && luaFunc.IsAlive)
//        {
//            luaFunc.Call(uId, isBuild);
//        }
//    }
    
//    public static void SetEntityMenuActive(string uId, bool isActive)
//    {
//        LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("MapUtil4CS.SetEntityMenuActive");
//        luaFunc?.Call(uId, isActive);
//    }
//    //获取家园场景
//    public static HomeSceneView GetHomeScene()
//    {
//        if (WorldMapManager.instance.mHomeMedia == null)
//        {
//            return null;
//        }
//        return WorldMapManager.instance.mHomeMedia.HomeScene;
//    }
    
//    public static string GetSelfCityId()
//    {
//        string selfCityId = "";
//        LuaFunction luaFunc = LuaInterfacePool.GetLuaFunction("CommonInterface4CS.GetSelfCityId");
//        if (luaFunc != null)
//        {
//            selfCityId = luaFunc.Invoke<string>();
//        }
//        return selfCityId;
//    }
//}
