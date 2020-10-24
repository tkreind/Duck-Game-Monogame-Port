// Decompiled with JetBrains decompiler
// Type: DuckGame.XMLLevel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace DuckGame
{
  public class XMLLevel : Level
  {
    public bool collectItems;
    public List<Thing> levelItems = new List<Thing>();
    public Texture2D preview;
    private bool _customLevel;
    private bool _customLoad;
    private uint _checksum;
    private string _loadString = "";
    private LevelData _data;
    private byte[] _compressedData;
    private MemoryStream _compressedDataReceived;
    private Stream _stream;
    public bool ignoreVisibility;
    public volatile bool cancelLoading;
    public bool onlineEnabled;

    public bool customLevel => this._customLevel;

    public uint checksum => this._checksum;

    public LevelData data
    {
      get => this._data;
      set => this._data = value;
    }

    public byte[] compressedData => this._compressedData;

    public MemoryStream compressedDataReceived => this._compressedDataReceived;

    public XMLLevel(LevelData level) => this._data = level;

    public XMLLevel(string level)
    {
      if (level.EndsWith(".custom"))
      {
        this.isCustomLevel = true;
        this._customLevel = true;
        level = level.Substring(0, level.Length - 7);
        if (Network.isActive)
        {
          LevelData level1 = Content.GetLevel(level);
          this._checksum = level1.GetChecksum();
          this._data = level1;
          this._customLoad = true;
          if (Network.isServer)
          {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter((Stream) new GZipStream((Stream) memoryStream, CompressionMode.Compress));
            binaryWriter.Write(level);
            BitBuffer data = level1.GetData();
            binaryWriter.Write(data.lengthInBytes);
            binaryWriter.Write(data.buffer, 0, data.lengthInBytes);
            binaryWriter.Close();
            this._compressedData = memoryStream.ToArray();
          }
        }
      }
      if (level == "WORKSHOP")
      {
        this._customLevel = true;
        this.isCustomLevel = true;
        level = level.Substring(0, level.Length - 7);
        LevelData nextLevel = RandomLevelDownloader.GetNextLevel();
        this._checksum = nextLevel.GetChecksum();
        this._data = nextLevel;
        this._customLoad = true;
        if (Network.isServer && Network.isActive)
        {
          MemoryStream memoryStream = new MemoryStream();
          BinaryWriter binaryWriter = new BinaryWriter((Stream) new GZipStream((Stream) memoryStream, CompressionMode.Compress));
          binaryWriter.Write(nextLevel.metaData.guid.ToString());
          BitBuffer data = nextLevel.GetData();
          binaryWriter.Write(data.lengthInBytes);
          binaryWriter.Write(data.buffer, 0, data.lengthInBytes);
          binaryWriter.Close();
          this._compressedData = memoryStream.ToArray();
        }
      }
      this._level = level;
    }

    public void ApplyLevelData(ReceivedLevelInfo info)
    {
      this.waitingOnNewData = false;
      this._data = info.data;
      this._level = info.name;
      this._customLevel = true;
      this.isCustomLevel = true;
      string str1 = DuckFile.onlineLevelDirectory;
      if (NetworkDebugger.networkDrawingIndex != 0)
        str1 = str1.Insert(str1.Length - 1, NetworkDebugger.networkDrawingIndex.ToString());
      string str2 = this._level;
      if (str2.EndsWith(".custom"))
        str2 = str2.Substring(0, this.level.Length - 7);
      DuckFile.SaveChunk((BinaryClassChunk) this._data, str1 + str2 + ".lev");
    }

    public string ProcessLevelPath(string path)
    {
      bool flag = false;
      if (path.EndsWith(".online"))
      {
        this.isCustomLevel = true;
        string str1 = path.Substring(0, path.Length - 7);
        string str2 = DuckFile.onlineLevelDirectory;
        if (NetworkDebugger.networkDrawingIndex != 0)
          str2 = str2.Insert(str2.Length - 1, NetworkDebugger.networkDrawingIndex.ToString());
        string path1 = str2 + str1 + ".lev";
        if (File.Exists(path1))
          return path1;
        flag = true;
      }
      if (flag || path.EndsWith(".custom"))
      {
        this.isCustomLevel = true;
        string str1 = path.Substring(0, path.Length - 7);
        string str2 = DuckFile.levelDirectory;
        if (NetworkDebugger.networkDrawingIndex != 0)
          str2 = str2.Insert(str2.Length - 1, NetworkDebugger.networkDrawingIndex.ToString());
        string path1 = str2 + str1 + ".lev";
        return File.Exists(path1) ? path1 : (string) null;
      }
      this._data = Content.GetLevel(path);
      if (this._data != null)
        return path;
      string path2 = DuckFile.levelDirectory + path + ".lev";
      if (File.Exists(path2))
        return path2;
      string path3 = Editor.initialDirectory + "/" + path + ".lev";
      return File.Exists(path3) ? path3 : (string) null;
    }

    private LevelData LoadLevelDoc()
    {
      if (this._data != null)
        return this._data;
      if (this._level == "WORKSHOP")
        return RandomLevelDownloader.GetNextLevel();
      LevelData levelData;
      if (!this._level.Contains("_tempPlayLevel"))
      {
        this._loadString = this._level;
        levelData = Content.GetLevel(this._level);
        if (levelData == null)
        {
          bool flag = false;
          if (this._level.Contains(":/") || this._level.Contains(":\\"))
            flag = true;
          if (flag)
          {
            this._loadString = this._level;
            if (!this._loadString.EndsWith(".lev"))
              this._loadString += ".lev";
          }
          else
            this._loadString = DuckFile.levelDirectory + this._level + ".lev";
          levelData = DuckFile.LoadLevel(this._loadString);
          if (levelData == null && !flag)
          {
            this._loadString = Editor.initialDirectory + "/" + this._level + ".lev";
            levelData = DuckFile.LoadLevel(this._loadString);
          }
          if (this is GameLevel)
            this._customLoad = true;
        }
      }
      else
      {
        if (this is GameLevel && this._level.ToLowerInvariant().Contains(DuckFile.levelDirectory.ToLowerInvariant()))
          this._customLoad = true;
        this._level = this._level.Replace(Directory.GetCurrentDirectory() + "\\", "");
        this._stream = TitleContainer.OpenStream(this._level);
        levelData = DuckFile.LoadLevel(DuckFile.ReadEntireStream(this._stream));
      }
      return levelData;
    }

    public override void Initialize()
    {
      AutoBlock._kBlockIndex = (ushort) 0;
      if (this.level == "RANDOM" || this.cancelLoading)
        return;
      if (this._data == null)
        this._data = this.LoadLevelDoc();
      if (this.cancelLoading || this._data == null)
        return;
      this._id = this._data.metaData.guid;
      if ((this.level == "WORKSHOP" || this._customLoad || this._customLevel) && !this.bareInitialize)
        Global.PlayCustomLevel(this._id);
      if (!Content.renderingPreview)
        Custom.ClearCustomData();
      if (!Content.renderingPreview)
      {
        Custom.previewData[CustomType.Block][0] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Block][1] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Block][2] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Background][0] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Background][1] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Background][2] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Platform][0] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Platform][1] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Platform][2] = (CustomTileDataChunk) null;
        Custom.previewData[CustomType.Parallax][0] = (CustomTileDataChunk) null;
        if (this._data.customData != null)
        {
          if (this._data.customData.customTileset01Data != null)
          {
            Custom.previewData[CustomType.Block][0] = this._data.customData.customTileset01Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Block][0].GetTileData(), 0, CustomType.Block);
          }
          if (this._data.customData.customTileset02Data != null)
          {
            Custom.previewData[CustomType.Block][1] = this._data.customData.customTileset02Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Block][1].GetTileData(), 1, CustomType.Block);
          }
          if (this._data.customData.customTileset03Data != null)
          {
            Custom.previewData[CustomType.Block][2] = this._data.customData.customTileset03Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Block][2].GetTileData(), 2, CustomType.Block);
          }
          if (this._data.customData.customBackground01Data != null)
          {
            Custom.previewData[CustomType.Background][0] = this._data.customData.customBackground01Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Background][0].GetTileData(), 0, CustomType.Background);
          }
          if (this._data.customData.customBackground02Data != null)
          {
            Custom.previewData[CustomType.Background][1] = this._data.customData.customBackground02Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Background][1].GetTileData(), 1, CustomType.Background);
          }
          if (this._data.customData.customBackground03Data != null)
          {
            Custom.previewData[CustomType.Background][2] = this._data.customData.customBackground03Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Background][2].GetTileData(), 2, CustomType.Background);
          }
          if (this._data.customData.customPlatform01Data != null)
          {
            Custom.previewData[CustomType.Platform][0] = this._data.customData.customPlatform01Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Platform][0].GetTileData(), 0, CustomType.Platform);
          }
          if (this._data.customData.customPlatform02Data != null)
          {
            Custom.previewData[CustomType.Platform][1] = this._data.customData.customPlatform02Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Platform][1].GetTileData(), 1, CustomType.Platform);
          }
          if (this._data.customData.customPlatform03Data != null)
          {
            Custom.previewData[CustomType.Platform][2] = this._data.customData.customPlatform03Data;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Platform][2].GetTileData(), 2, CustomType.Platform);
          }
          if (this._data.customData.customParallaxData != null)
          {
            Custom.previewData[CustomType.Parallax][0] = this._data.customData.customParallaxData;
            if (!Content.renderingPreview)
              Custom.ApplyCustomData(Custom.previewData[CustomType.Parallax][0].GetTileData(), 0, CustomType.Parallax);
          }
        }
      }
      if (this.cancelLoading)
        return;
      if (!this.bareInitialize)
        this.preview = Editor.LoadPreview(this._data.previewData.preview);
      this.onlineEnabled = this._data.metaData.online;
      bool flag = true;
      foreach (BinaryClassChunk node in this._data.objects.objects)
      {
        if (this.cancelLoading)
          return;
        Thing t = Thing.LoadThing(node);
        if (t != null)
        {
          if (!ContentProperties.GetBag(t.GetType()).GetOrDefault<bool>("isOnlineCapable", true) || t.serverOnly && !Network.isServer)
          {
            flag = false;
            if (Network.isActive)
              continue;
          }
          if (!this.bareInitialize || t is ArcadeMachine)
          {
            if (!t.visibleInGame && !this.ignoreVisibility)
              t.visible = false;
            if (Network.isActive && t.isStateObject && (!this.bareInitialize && !this.isPreview))
            {
              GhostManager.context.MakeGhost(t, initLevel: true);
              t.ghostType = Editor.IDToType[t.GetType()];
              if (DuckNetwork.hostDuckIndex >= 0 && DuckNetwork.hostDuckIndex < 4 && DuckNetwork.profiles[DuckNetwork.hostDuckIndex].connection != null)
                t.connection = DuckNetwork.profiles[DuckNetwork.hostDuckIndex].connection;
            }
            this.AddThing(t);
          }
        }
      }
      if (flag)
        this.onlineEnabled = true;
      this._things.RefreshState();
      if (this._stream == null)
        return;
      this._stream.Close();
    }
  }
}
