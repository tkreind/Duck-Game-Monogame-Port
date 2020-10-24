// Decompiled with JetBrains decompiler
// Type: DuckGame.NCPacketBreakdown
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class NCPacketBreakdown
  {
    private static IEnumerable<NCPacketDataType> _dataTypes;
    private Dictionary<NCPacketDataType, int> _bitsPerType = new Dictionary<NCPacketDataType, int>();

    public static IEnumerable<NCPacketDataType> dataTypes
    {
      get
      {
        if (NCPacketBreakdown._dataTypes == null)
          NCPacketBreakdown._dataTypes = Enum.GetValues(typeof (NCPacketDataType)).Cast<NCPacketDataType>();
        return NCPacketBreakdown._dataTypes;
      }
    }

    public NCPacketBreakdown()
    {
      if (NCPacketBreakdown._dataTypes == null)
        NCPacketBreakdown._dataTypes = Enum.GetValues(typeof (NCPacketDataType)).Cast<NCPacketDataType>();
      foreach (NCPacketDataType dataType in NCPacketBreakdown._dataTypes)
        this._bitsPerType[dataType] = 0;
    }

    public void Add(NCPacketDataType type, int bits)
    {
      Dictionary<NCPacketDataType, int> bitsPerType;
      NCPacketDataType key;
      (bitsPerType = this._bitsPerType)[key = type] = bitsPerType[key] + bits;
    }

    public int Get(NCPacketDataType type) => this._bitsPerType[type];

    public static Color GetTypeColor(NCPacketDataType type)
    {
      switch (type)
      {
        case NCPacketDataType.InputStream:
          return Color.Pink;
        case NCPacketDataType.Ghost:
          return Color.Red;
        case NCPacketDataType.Ack:
          return Color.Lime;
        case NCPacketDataType.Event:
          return Color.Blue;
        case NCPacketDataType.ExtraData:
          return Color.White;
        default:
          return Color.Yellow;
      }
    }
  }
}
