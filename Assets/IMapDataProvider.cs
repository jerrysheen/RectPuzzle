using System.Collections.Generic;

public interface IMapDataProvider
{
    // byte GetMapGridTypeAt(int globalGridX, int globalGridY, int lod);

    // void GetMapGridTypesAt(int globalGridX, int globalGridY, int lod, ref List<ushort> typesList);

    //BlockMeta GetBlockInfoAt(int globalGridX, int globalGridY, int lod, out int hostGlobalIndex);

    // int GetResourceHostIndex(int globalGridX, int globalGridY, int lod);

    List<ModelData  > GetModelList(List<ushort> typesList);

    List<ushort> GetModelIndListInTile(int tileX, int tileY);

    bool CheckBlock(int serverX, int serverY);
}