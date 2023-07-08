using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModelData
{
    public Vector3 pos { get; set; }
    public Vector3 scale;
    public Quaternion rot;
    public string prefabPath;
    //MapPlaceCategory
    public byte modelType;
    public Vector2Int extent;
}

public class WarpModelData
{
    public ModelData data;
    public float posX;
    public float posZ;
}

/// <summary>
/// 资源带数据;
/// </summary>
public class ResAreaData
{
    public Vector3 origin;
    public float width;
    public float height;
    public byte area;
}

public class MapDataProvider : IMapDataProvider
{

    private Dictionary<Vector2Int, List<ushort>> m_MapGird = new Dictionary<Vector2Int, List<ushort>>();
    private List<ModelData> m_ModelInfoList = new List<ModelData>();
    private Dictionary<Vector2Int, List<ushort>> m_TilesInfo = new Dictionary<Vector2Int, List<ushort>>();
    private byte[][] m_gridInfo;
    private byte[] buf;
    private int bufLength = 4;
    private List<ResAreaData> m_ResAreaList = new List<ResAreaData>();
    
    public void InitializeWith(Stream fs, Stream gridInfo)
    {
        if (fs != null)
        {
            InitModelInfo(fs);
        }
        if (gridInfo != null)
        {
            InitGridInfo(gridInfo);
        }
    }

    public void InitGridInfo(Stream gridInfo)
    {
        // map.bytes
        byte[] tmp = new byte[4];

        gridInfo.Read(tmp, 0, tmp.Length);
        int width = BitConverter.ToInt32(tmp, 0);

        gridInfo.Read(tmp, 0, tmp.Length);
        int height = BitConverter.ToInt32(tmp, 0);

        MapPlaceConfig.SetMapPlaceConfig(width,height);
        
        
        
        m_gridInfo = new byte[height][];

        for (int i = 0; i < height; i++)
        {
            m_gridInfo[i] = new byte[width];
            gridInfo.Read(m_gridInfo[i], 0, width);
        }
        m_ResAreaList.Clear();
    }

    public void InitModelInfo(Stream fs)
    {
        byte[] modelCountBuf = new byte[4];
        fs.Read(modelCountBuf, 0, modelCountBuf.Length);
        int nTotalModels = BitConverter.ToInt32(modelCountBuf, 0);
     
        buf = new byte[bufLength];
        byte[] strBuf = null;
        ModelData mi = null;
        m_ModelInfoList.Clear();
        for (int i = 0; i < nTotalModels; i++)
        {
            mi = new ModelData();
            mi.pos = LoadVec3(fs) * MapPlaceConfig.MAP_FIX_SCALE;
            mi.scale = LoadVec3(fs);
            mi.rot = LoadQuat(fs);

            int strCount = fs.ReadByte();            
            if (strBuf == null || strBuf.Length != strCount)
                strBuf = new byte[strCount];

            fs.Read(strBuf, 0, strBuf.Length);
            mi.prefabPath = System.Text.Encoding.Default.GetString(strBuf);
            mi.modelType = (byte)fs.ReadByte();

            mi.extent = LoadVecInt2(fs);

            m_ModelInfoList.Add(mi);
        }

        if (m_TilesInfo.Count > 0)
        {
            m_TilesInfo.Clear();
        }
        
        byte[] tempBuff = new byte[2];
        fs.Read(tempBuff, 0, tempBuff.Length);
        MapPlaceConfig.SHOW_TILE_SIZE = (int)(BitConverter.ToUInt16(tempBuff, 0) * MapPlaceConfig.MAP_FIX_SCALE);

        fs.Read(tempBuff, 0, tempBuff.Length);
        int nTiles = BitConverter.ToUInt16(tempBuff, 0);
        
        Vector2Int coord = new Vector2Int();
        int nModelIndCount;
        ushort modelInd;
        List<ushort> modelIndList = null;
        for (int iTile = 0; iTile < nTiles; iTile++)
        {           
            fs.Read(tempBuff, 0, tempBuff.Length);
            coord.x = BitConverter.ToUInt16(tempBuff, 0);
            fs.Read(tempBuff, 0, tempBuff.Length);
            coord.y = BitConverter.ToUInt16(tempBuff, 0);

            modelIndList = new List<ushort>();

            fs.Read(tempBuff, 0, tempBuff.Length);
            nModelIndCount = BitConverter.ToUInt16(tempBuff, 0);
            for (int iModel = 0; iModel < nModelIndCount; iModel++)
            {
                fs.Read(tempBuff, 0, tempBuff.Length);
                modelInd = BitConverter.ToUInt16(tempBuff, 0);
                modelIndList.Add(modelInd);
            }

            if (m_TilesInfo.ContainsKey(coord))
            {
                new Exception();
            }

            m_TilesInfo.Add(coord, modelIndList);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="serverX">服务器X位置</param>
    /// <param name="serverY">服务器Y位置</param>
    /// <returns></returns>
    public bool CheckBlock(int serverX, int serverY)
    {
        if (m_gridInfo == null || serverX < 0 || serverY < 0||serverX>=MapPlaceConfig.mapBlockGridCnt||serverY>=MapPlaceConfig.mapBlockGridCnt)
            return true;

        int indexX = serverX % MapPlaceConfig.mapBlockGridCnt;
        int indexY = serverY % MapPlaceConfig.mapBlockGridCnt;
  
        byte val = m_gridInfo[indexY][indexX];
        if ((val & MapPlaceCategory.BLOCK_MASK) != 0)
            return true;
        
        return false;
    }

//    public void GetMapGridTypesAt(int globalGridX, int globalGridY, int lod, ref List<ushort> typesList)
//    {
//        int gridWidth = globalGridX % MapPlaceConfig.mapBlockGridWidth;
//        int gridHeight = globalGridY % MapPlaceConfig.mapBlockGridWidth;
//
//        Vector2Int coord = new Vector2Int(gridWidth, gridHeight);
//        if (m_MapGird.ContainsKey(coord))
//        {
//            List<ushort> gridList = m_MapGird[coord];
//            typesList = gridList;
//        }
//    }

    public List<ModelData> GetModelList(List<ushort> typesList)
    {
        List<ModelData> modelList = new List<ModelData>();
        foreach (ushort i in typesList)
        {
            modelList.Add(m_ModelInfoList[i]);
        }
        return modelList;
    }

    //public BlockMeta GetBlockInfoAt(int globalGridX, int globalGridY, int lod, out int hostGlobalIndex)
    //{
    //    hostGlobalIndex = -1;
    //    return new BlockMeta();
    //}

//    public int GetResourceHostIndex(int globalGridX, int globalGridY, int lod)
//    {
//        return -1;
//    }

    public List<ushort> GetModelIndListInTile(int tileX, int tileY)
    {
        Vector2Int coord = new Vector2Int(tileX, tileY);
        if (m_TilesInfo.ContainsKey(coord))
        {
            return m_TilesInfo[coord];
        }
        else
        {
            return null;
        }
    }
    private Vector3 v3 = new Vector3();
    private Vector3 LoadVec3(Stream fs)
    {
        fs.Read(buf, 0, bufLength);
        v3.x = BitConverter.ToSingle(buf, 0);

        fs.Read(buf, 0, bufLength);
        v3.y = BitConverter.ToSingle(buf, 0);

        fs.Read(buf, 0, bufLength);
        v3.z = BitConverter.ToSingle(buf, 0);

        return v3;
    }

    private Quaternion q4 = new Quaternion();
    private Quaternion LoadQuat(Stream fs)
    {
        fs.Read(buf, 0, bufLength);
        q4.x = BitConverter.ToSingle(buf, 0);

        fs.Read(buf, 0, bufLength);
        q4.y = BitConverter.ToSingle(buf, 0);

        fs.Read(buf, 0, bufLength);
        q4.z = BitConverter.ToSingle(buf, 0);

        fs.Read(buf, 0, bufLength);
        q4.w = BitConverter.ToSingle(buf, 0);

        return q4;
    }

    private Vector3 LoadVec2(Stream fs)
    {
        fs.Read(buf, 0, bufLength);
        v3.x = BitConverter.ToSingle(buf, 0);

        fs.Read(buf, 0, bufLength);
        v3.y = BitConverter.ToSingle(buf, 0);

        v3.z = 0;
        return v3;
    }

    private Vector2Int v2 = new Vector2Int();
    private Vector2Int LoadVecInt2(Stream fs)
    {
        fs.Read(buf, 0, bufLength);
        v2.x = BitConverter.ToInt32(buf, 0);

        fs.Read(buf, 0, bufLength);
        v2.y = BitConverter.ToInt32(buf, 0);

        return v2;
    }

}