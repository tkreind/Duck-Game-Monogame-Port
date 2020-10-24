// Decompiled with JetBrains decompiler
// Type: DuckGame.Custom
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace DuckGame
{
  public class Custom
  {
    private static Dictionary<CustomType, Dictionary<string, CustomTileData>> _customTilesetData = new Dictionary<CustomType, Dictionary<string, CustomTileData>>()
    {
      {
        CustomType.Block,
        new Dictionary<string, CustomTileData>()
      },
      {
        CustomType.Platform,
        new Dictionary<string, CustomTileData>()
      },
      {
        CustomType.Background,
        new Dictionary<string, CustomTileData>()
      },
      {
        CustomType.Parallax,
        new Dictionary<string, CustomTileData>()
      }
    };
    public static Dictionary<CustomType, string[]> data = new Dictionary<CustomType, string[]>()
    {
      {
        CustomType.Block,
        new string[3]{ "", "", "" }
      },
      {
        CustomType.Platform,
        new string[3]{ "", "", "" }
      },
      {
        CustomType.Background,
        new string[3]{ "", "", "" }
      },
      {
        CustomType.Parallax,
        new string[1]{ "" }
      }
    };
    public static Dictionary<CustomType, CustomTileDataChunk[]> previewData = new Dictionary<CustomType, CustomTileDataChunk[]>()
    {
      {
        CustomType.Block,
        new CustomTileDataChunk[3]
      },
      {
        CustomType.Platform,
        new CustomTileDataChunk[3]
      },
      {
        CustomType.Background,
        new CustomTileDataChunk[3]
      },
      {
        CustomType.Parallax,
        new CustomTileDataChunk[1]
      }
    };

    public static void ClearCustomData()
    {
      foreach (KeyValuePair<CustomType, string[]> keyValuePair in Custom.data)
      {
        for (int index = 0; index < keyValuePair.Value.Length; ++index)
        {
          keyValuePair.Value[index] = "";
          Custom.previewData[keyValuePair.Key][index] = (CustomTileDataChunk) null;
        }
      }
    }

    public static string ApplyCustomData(CustomTileData tData, int index, CustomType type)
    {
      string key = tData.path + "@" + (object) tData.checksum + ".png";
      if (tData.texture != null)
      {
        string str = DuckFile.GetCustomDownloadDirectory(type) + key;
        if (!File.Exists(str))
        {
          DuckFile.CreatePath(str);
          FileStream fileStream = File.Create(str);
          tData.texture.SaveAsPng((Stream) fileStream, tData.texture.Width, tData.texture.Height);
          fileStream.Close();
        }
      }
      else if (tData.path == null)
        return "";
      Custom._customTilesetData[type][key] = tData;
      Custom.data[type][index] = key;
      return key;
    }

    public static CustomTileData LoadTileData(string path, CustomType type)
    {
      CustomTileData customTileData = new CustomTileData();
      if (path == "" || path == null)
        return customTileData;
      Texture2D texture2D = ContentPack.LoadTexture2D(DuckFile.GetCustomDirectory(type) + path + ".png");
      if (texture2D != null)
      {
        try
        {
          Color[] data = new Color[texture2D.Width * texture2D.Height];
          texture2D.GetData<Color>(data);
          for (int index1 = 0; index1 < 5; ++index1)
          {
            int num1 = 112;
            int num2 = 64 + index1 * 16;
            if (index1 == 1)
            {
              int num3 = num2;
              num2 = num1;
              num1 = num3;
            }
            if (index1 == 3)
            {
              num1 = 96;
              num2 = 112;
            }
            else if (index1 == 4)
            {
              num1 = 112;
              num2 = 112;
            }
            int num4 = -1;
            int num5 = 0;
            for (int index2 = 0; index2 < 16; ++index2)
            {
              bool flag = false;
              for (int index3 = 0; index3 < 16; ++index3)
              {
                int index4 = index1 != 1 ? num1 + index3 + (num2 + index2) * texture2D.Width : num2 + index2 + (num1 + index3) * texture2D.Width;
                if (index1 == 3 || index1 == 4 ? data[index4].a != (byte) 0 : data[index4].r == (byte) 0 && data[index4].g == byte.MaxValue && data[index4].b == (byte) 0 && data[index4].a == byte.MaxValue)
                {
                  if (num4 == -1)
                    num4 = index3;
                }
                else if (num4 != -1)
                {
                  num5 = index3 - num4;
                  flag = true;
                  break;
                }
              }
              if (flag)
                break;
            }
            switch (index1)
            {
              case 0:
                customTileData.verticalWidth = num5;
                break;
              case 1:
                customTileData.horizontalHeight = num5;
                break;
              case 2:
                customTileData.verticalWidthThick = num5;
                break;
              case 3:
                customTileData.leftNubber = num5 != 0;
                break;
              case 4:
                customTileData.rightNubber = num5 != 0;
                break;
            }
          }
        }
        catch (Exception ex)
        {
          texture2D = (Texture2D) null;
        }
        customTileData.texture = texture2D;
        customTileData.path = path;
        Custom._customTilesetData[type][path] = customTileData;
      }
      return customTileData;
    }

    public static CustomTileData GetData(int index, CustomType type)
    {
      CustomTileData customTileData = (CustomTileData) null;
      if (!Custom._customTilesetData[type].TryGetValue(Custom.data[type][index], out customTileData))
        customTileData = Custom.LoadTileData(Custom.data[type][index], type);
      return customTileData;
    }
  }
}
