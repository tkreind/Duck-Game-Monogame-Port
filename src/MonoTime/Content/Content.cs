// Decompiled with JetBrains decompiler
// Type: DuckGame.Content
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace DuckGame
{
  public class Content
  {
    private static MultiMap<string, LevelData> _levels = new MultiMap<string, LevelData>();
    private static MultiMap<string, LevelData> _levelPreloadList = new MultiMap<string, LevelData>();
    private static Dictionary<string, MTEffect> _effects = new Dictionary<string, MTEffect>();
    private static Dictionary<Effect, MTEffect> _effectMap = new Dictionary<Effect, MTEffect>();
    private static List<MTEffect> _effectList = new List<MTEffect>();
    private static short _currentEffectIndex = 0;
    private static Dictionary<string, Tex2D> _textures = new Dictionary<string, Tex2D>();
    private static Dictionary<object, Tex2D> _texture2DMap = new Dictionary<object, Tex2D>();
    private static List<Tex2D> _textureList = new List<Tex2D>();
    private static short _currentTextureIndex = 0;
    public static Tex2D invalidTexture;
    private static volatile bool readyToRenderPreview = false;
    private static volatile bool previewRendering = false;
    public static volatile bool renderingPreview = false;
    public static XMLLevel previewLevel;
    private static Camera _previewCamera;
    public static volatile bool cancelPreview = false;
    public static int customPreviewWidth = 0;
    public static int customPreviewHeight = 0;
    public static Vec2 customPreviewCenter = Vec2.Zero;
    private static LayerCore _previewLayerCore = (LayerCore) null;
    private static RenderTarget2D _previewTarget;
    private static string _previewPath = (string) null;
    private static MTSpriteBatch _previewBatch;
    private static bool _previewBackground = false;
    private static Thread _previewThread;
    private static RenderTarget2D _currentPreviewTarget;
    private static Dictionary<System.Type, string> _extensionList = new Dictionary<System.Type, string>()
    {
      {
        typeof (Tex2D),
        "*.png"
      },
      {
        typeof (Texture2D),
        "*.png"
      },
      {
        typeof (SoundEffect),
        "*.wav"
      },
      {
        typeof (Song),
        "*.ogg"
      },
      {
        typeof (Level),
        "*.lev"
      },
      {
        typeof (Effect),
        "*.xnb"
      }
    };
    private static List<string> _texturesToProcess = new List<string>();
    private static ContentManager _base;
    private static string _path = "";
    private static object _loadLock = new object();

    public static LevelData GetLevel(string guid, LevelLocation location = LevelLocation.Any)
    {
      List<LevelData> list;
      if (DuckGame.Content._levels.TryGetValue(guid, out list))
      {
        foreach (LevelData levelData in list)
        {
          if (levelData.GetLocation() == location || location == LevelLocation.Any)
            return levelData;
        }
      }
      return (LevelData) null;
    }

    public static List<LevelData> GetAllLevels(string guid)
    {
      List<LevelData> list;
      return DuckGame.Content._levels.TryGetValue(guid, out list) ? list : new List<LevelData>();
    }

    public static void MapLevel(string lev, LevelData dat, LevelLocation location)
    {
      List<LevelData> list;
      if (DuckGame.Content._levels.TryGetValue(lev, out list))
      {
        LevelData levelData1 = (LevelData) null;
        foreach (LevelData levelData2 in list)
        {
          if (levelData2.GetLocation() == location)
          {
            levelData1 = levelData2;
            break;
          }
        }
        if (levelData1 != null)
          list.Remove(levelData1);
      }
      dat.SetLocation(location);
      DuckGame.Content._levels.Add(lev, dat);
    }

    public static List<MTEffect> effectList => DuckGame.Content._effectList;

    public static Dictionary<string, Tex2D> textures => DuckGame.Content._textures;

    public static List<Tex2D> textureList => DuckGame.Content._textureList;

    private static void PreviewThread()
    {
      Level activeLevel = Level.activeLevel;
      Level currentLevel = Level.core.currentLevel;
      LayerCore core = Layer.core;
      try
      {
        DuckGame.Content.renderingPreview = true;
        if (!DuckGame.Content._previewBackground)
          Thing.skipLayerAdding = true;
        XMLLevel xmlLevel = new XMLLevel(DuckGame.Content._previewPath);
        if (DuckGame.Content.cancelPreview)
          return;
        DuckGame.Content.previewLevel = xmlLevel;
        DuckGame.Content.previewLevel.ignoreVisibility = true;
        Level.skipInitialize = !DuckGame.Content._previewBackground;
        if (!DuckGame.Content._previewBackground)
          DuckGame.Content.previewLevel.isPreview = true;
        DuckGame.Content._previewLayerCore = (LayerCore) null;
        if (DuckGame.Content._previewBackground)
        {
          Layer.core = DuckGame.Content._previewLayerCore = new LayerCore();
          Layer.core.InitializeLayers();
          Level.core.currentLevel = (Level) DuckGame.Content.previewLevel;
          Level.activeLevel = (Level) DuckGame.Content.previewLevel;
        }
        DuckGame.Content.previewLevel.Initialize();
        if (DuckGame.Content._previewBackground)
        {
          Level.activeLevel = activeLevel;
          Level.core.currentLevel = currentLevel;
        }
        if (DuckGame.Content.cancelPreview)
          return;
        Thing.skipLayerAdding = false;
        Level.skipInitialize = false;
        DuckGame.Content.previewLevel.CalculateBounds();
        DuckGame.Content._previewCamera = DuckGame.Content.customPreviewWidth == 0 ? new Camera(0.0f, 0.0f, 1280f, 1280f * DuckGame.Graphics.aspect) : new Camera(0.0f, 0.0f, (float) DuckGame.Content.customPreviewWidth, (float) DuckGame.Content.customPreviewHeight);
        Vec2 vec2 = (DuckGame.Content.previewLevel.topLeft + DuckGame.Content.previewLevel.bottomRight) / 2f;
        if (DuckGame.Content.cancelPreview)
          return;
        DuckGame.Content._previewCamera.width /= 2f;
        DuckGame.Content._previewCamera.height /= 2f;
        DuckGame.Content._previewCamera.center = !(DuckGame.Content.customPreviewCenter != Vec2.Zero) ? vec2 : DuckGame.Content.customPreviewCenter;
        DuckGame.Content.readyToRenderPreview = true;
        if (DuckGame.Content._previewThread != null)
        {
          while (DuckGame.Content.readyToRenderPreview)
          {
            if (DuckGame.Content.cancelPreview)
              return;
          }
        }
        DuckGame.Content.previewRendering = false;
        DuckGame.Content.renderingPreview = false;
      }
      catch (Exception ex)
      {
        Program.LogLine(ex.ToString());
        DuckGame.Content.renderingPreview = false;
        Thing.skipLayerAdding = false;
        Level.skipInitialize = false;
      }
      if (!DuckGame.Content._previewBackground)
        return;
      Level.activeLevel = activeLevel;
      Level.core.currentLevel = currentLevel;
      Layer.core = core;
    }

    private static void DoPreviewRender()
    {
      MTSpriteBatch screen = DuckGame.Graphics.screen;
      DuckGame.Graphics.screen = DuckGame.Content._previewBatch;
      Viewport viewport = DuckGame.Graphics.viewport;
      DuckGame.Graphics.SetRenderTarget(DuckGame.Content._currentPreviewTarget);
      DuckGame.Graphics.viewport = new Viewport(0, 0, DuckGame.Content._currentPreviewTarget.width, DuckGame.Content._currentPreviewTarget.height);
      string str1 = Custom.data[CustomType.Block][0];
      if (Custom.previewData[CustomType.Block][0] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Block][0].GetTileData(), 0, CustomType.Block);
      string str2 = Custom.data[CustomType.Block][1];
      if (Custom.previewData[CustomType.Block][1] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Block][1].GetTileData(), 1, CustomType.Block);
      string str3 = Custom.data[CustomType.Block][2];
      if (Custom.previewData[CustomType.Block][2] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Block][2].GetTileData(), 2, CustomType.Block);
      string str4 = Custom.data[CustomType.Background][0];
      if (Custom.previewData[CustomType.Background][0] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Background][0].GetTileData(), 0, CustomType.Background);
      string str5 = Custom.data[CustomType.Background][1];
      if (Custom.previewData[CustomType.Background][1] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Background][1].GetTileData(), 1, CustomType.Background);
      string str6 = Custom.data[CustomType.Background][2];
      if (Custom.previewData[CustomType.Background][2] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Background][2].GetTileData(), 2, CustomType.Background);
      string str7 = Custom.data[CustomType.Platform][0];
      if (Custom.previewData[CustomType.Platform][0] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Platform][0].GetTileData(), 0, CustomType.Platform);
      string str8 = Custom.data[CustomType.Platform][1];
      if (Custom.previewData[CustomType.Platform][1] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Platform][1].GetTileData(), 1, CustomType.Platform);
      string str9 = Custom.data[CustomType.Platform][2];
      if (Custom.previewData[CustomType.Platform][2] != null)
        Custom.ApplyCustomData(Custom.previewData[CustomType.Platform][2].GetTileData(), 2, CustomType.Platform);
      if (DuckGame.Content._previewBackground)
      {
        Level activeLevel = Level.activeLevel;
        Level currentLevel = Level.core.currentLevel;
        LayerCore core = Layer.core;
        if (DuckGame.Content._previewLayerCore != null)
          Layer.core = DuckGame.Content._previewLayerCore;
        Level.activeLevel = (Level) DuckGame.Content.previewLevel;
        Level.core.currentLevel = (Level) DuckGame.Content.previewLevel;
        DuckGame.Graphics.defaultRenderTarget = DuckGame.Content._currentPreviewTarget;
        Layer.HUD.visible = false;
        DuckGame.Content.previewLevel.camera = DuckGame.Content._previewCamera;
        DuckGame.Content.previewLevel.simulatePhysics = false;
        DuckGame.Content.previewLevel.DoUpdate();
        DuckGame.Content.previewLevel.DoUpdate();
        DuckGame.Content.previewLevel.DoDraw();
        Layer.HUD.visible = true;
        DuckGame.Graphics.defaultRenderTarget = (RenderTarget2D) null;
        Level.activeLevel = activeLevel;
        Level.core.currentLevel = currentLevel;
        Layer.core = core;
      }
      else
      {
        DuckGame.Graphics.Clear(Color.Black);
        DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, DuckGame.Content._previewCamera.getMatrix());
        foreach (Thing thing in DuckGame.Content.previewLevel.things)
        {
          if (thing.layer == Layer.Game || thing.layer == Layer.Blocks || thing.layer == null)
            thing.Draw();
          DuckGame.Graphics.material = (Material) null;
        }
        DuckGame.Graphics.screen.End();
      }
      DuckGame.Graphics.screen = screen;
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      Custom.data[CustomType.Block][0] = str1;
      Custom.data[CustomType.Block][1] = str2;
      Custom.data[CustomType.Block][2] = str3;
      Custom.data[CustomType.Background][0] = str4;
      Custom.data[CustomType.Background][1] = str5;
      Custom.data[CustomType.Background][2] = str6;
      Custom.data[CustomType.Platform][0] = str7;
      Custom.data[CustomType.Platform][1] = str8;
      Custom.data[CustomType.Platform][2] = str9;
    }

    public static Thread previewThread => DuckGame.Content._previewThread;

    public static RenderTarget2D GeneratePreview(
      string levelPath,
      RenderTarget2D target = null,
      bool background = false)
    {
      DuckGame.Content._previewBackground = background;
      DuckGame.Content.readyToRenderPreview = false;
      if (DuckGame.Content._previewThread != null && DuckGame.Content._previewThread.IsAlive)
      {
        DuckGame.Content.cancelPreview = true;
        int num = 250;
        while (DuckGame.Content._previewThread.IsAlive)
        {
          Tasker.RunTasks();
          Thread.Sleep(2);
          --num;
        }
        DuckGame.Content.readyToRenderPreview = false;
      }
      DuckGame.Content._previewThread = (Thread) null;
      DuckGame.Content.cancelPreview = false;
      Thing.skipLayerAdding = false;
      Level.skipInitialize = false;
      if (DuckGame.Content._previewBatch == null)
        DuckGame.Content._previewBatch = new MTSpriteBatch(DuckGame.Graphics.device);
      DuckGame.Content._previewPath = levelPath;
      if (DuckGame.Content._previewTarget == null)
        DuckGame.Content._previewTarget = new RenderTarget2D(MonoMain.screenWidth, MonoMain.screenHeight);
      DuckGame.Content._currentPreviewTarget = target == null ? DuckGame.Content._previewTarget : target;
      if (DuckGame.Content._previewBackground)
      {
        DuckGame.Content.renderingPreview = true;
        DuckGame.Content.readyToRenderPreview = true;
        DuckGame.Content.PreviewThread();
        DuckGame.Content.DoPreviewRender();
        DuckGame.Content.renderingPreview = false;
        DuckGame.Content.readyToRenderPreview = false;
      }
      else
      {
        DuckGame.Content._previewThread = new Thread(new ThreadStart(DuckGame.Content.PreviewThread));
        DuckGame.Content._previewThread.CurrentCulture = CultureInfo.InvariantCulture;
        DuckGame.Content._previewThread.Priority = ThreadPriority.BelowNormal;
        DuckGame.Content._previewThread.IsBackground = true;
        DuckGame.Content._previewThread.Start();
      }
      return DuckGame.Content._currentPreviewTarget;
    }

    public static void SetTextureAtIndex(short index, Tex2D tex)
    {
      while ((int) index >= DuckGame.Content._textureList.Count)
      {
        DuckGame.Content._textureList.Add((Tex2D) null);
        ++DuckGame.Content._currentTextureIndex;
      }
      DuckGame.Content._textureList[(int) index] = tex;
      DuckGame.Content._texture2DMap[tex.nativeObject] = tex;
      DuckGame.Content._textures[tex.textureName] = tex;
      tex.SetTextureIndex(index);
    }

    public static Tex2D AssignTextureIndex(Tex2D tex)
    {
      Tex2D tex2D = (Tex2D) null;
      DuckGame.Content._texture2DMap.TryGetValue((object) tex, out tex2D);
      if (tex2D == null)
      {
        tex.SetTextureIndex(DuckGame.Content._currentTextureIndex);
        ++DuckGame.Content._currentTextureIndex;
        DuckGame.Content._textureList.Add(tex);
        DuckGame.Content._texture2DMap[(object) tex] = tex;
      }
      return tex2D;
    }

    public static Tex2D GetTex2D(object tex)
    {
      Tex2D tex2D = (Tex2D) null;
      DuckGame.Content._texture2DMap.TryGetValue(tex, out tex2D);
      if (tex2D == null)
      {
        tex2D = new Tex2D((Texture2D) tex, "", DuckGame.Content._currentTextureIndex);
        ++DuckGame.Content._currentTextureIndex;
        DuckGame.Content._textureList.Add(tex2D);
        DuckGame.Content._texture2DMap[tex] = tex2D;
      }
      return tex2D;
    }

    public static void SetEffectAtIndex(short index, MTEffect e)
    {
      while ((int) index > DuckGame.Content._effectList.Count)
      {
        DuckGame.Content._effectList.Add((MTEffect) null);
        ++DuckGame.Content._currentEffectIndex;
      }
      DuckGame.Content._effectList[(int) index] = e;
      DuckGame.Content._effectMap[e.effect] = e;
      DuckGame.Content._effects[e.effectName] = e;
      e.SetEffectIndex(index);
    }

    public static MTEffect GetMTEffect(Effect e)
    {
      MTEffect mtEffect = (MTEffect) null;
      DuckGame.Content._effectMap.TryGetValue(e, out mtEffect);
      if (mtEffect == null)
      {
        mtEffect = new MTEffect(e, "", DuckGame.Content._currentEffectIndex);
        ++DuckGame.Content._currentEffectIndex;
        DuckGame.Content._effectList.Add(mtEffect);
        DuckGame.Content._effectMap[e] = mtEffect;
      }
      return mtEffect;
    }

    public static Tex2D GetTex2DFromIndex(short index) => DuckGame.Content._textureList[(int) index];

    public static MTEffect GetMTEffectFromIndex(short index) => index < (short) 0 ? (MTEffect) null : DuckGame.Content._effectList[(int) index];

    public static List<string> GetFiles<T>(string path)
    {
      List<string> files = new List<string>();
      string ext = (string) null;
      if (DuckGame.Content._extensionList.TryGetValue(typeof (T), out ext))
        DuckGame.Content.GetFilesInternal<T>(path, files, ext);
      return files;
    }

    public static List<string> GetFilesInternal<T>(string path, List<string> files, string ext)
    {
      foreach (string file in DuckFile.GetFiles(path, ext))
        files.Add(file);
      foreach (string directory in DuckGame.Content.GetDirectories(path))
        DuckGame.Content.GetFilesInternal<T>(directory, files, ext);
      return files;
    }

    private static void SearchDirLevels(string dir, LevelLocation location)
    {
      foreach (string path in location == LevelLocation.Content ? DuckGame.Content.GetFiles(dir) : DuckFile.GetFiles(dir))
        DuckGame.Content.ProcessLevel(path, location);
      foreach (string dir1 in location == LevelLocation.Content ? DuckGame.Content.GetDirectories(dir) : DuckFile.GetDirectories(dir))
        DuckGame.Content.SearchDirLevels(dir1, location);
    }

    public static void ReloadLevels(string s) => DuckGame.Content.SearchDirLevels("Content/levels/" + s, LevelLocation.Content);

    private static void SearchDirTextures(string dir, bool reverse = false)
    {
      if (reverse)
      {
        foreach (string path in ((IEnumerable<string>) DuckGame.Content.GetFiles(dir)).Reverse<string>())
          DuckGame.Content.ProcessTexture(path);
        foreach (string dir1 in ((IEnumerable<string>) DuckGame.Content.GetDirectories(dir)).Reverse<string>())
        {
          if (!dir1.EndsWith("Audio") && !dir1.EndsWith("Shaders"))
            DuckGame.Content.SearchDirTextures(dir1, reverse);
        }
      }
      else
      {
        foreach (string file in DuckGame.Content.GetFiles(dir))
          DuckGame.Content.ProcessTexture(file);
        foreach (string directory in DuckGame.Content.GetDirectories(dir))
        {
          if (!directory.EndsWith("Audio") && !directory.EndsWith("Shaders"))
            DuckGame.Content.SearchDirTextures(directory);
        }
      }
    }

    private static void SearchDirEffects(string dir)
    {
      foreach (string file in DuckGame.Content.GetFiles(dir))
        DuckGame.Content.ProcessEffect(file);
      foreach (string directory in DuckGame.Content.GetDirectories(dir))
        DuckGame.Content.SearchDirEffects(directory);
    }

    public static string GetLevelID(string path, LevelLocation loc = LevelLocation.Content)
    {
      if (!path.EndsWith(".lev"))
        path += ".lev";
      foreach (KeyValuePair<string, List<LevelData>> level in (MultiMap<string, LevelData, List<LevelData>>) DuckGame.Content._levels)
      {
        foreach (LevelData levelData in level.Value)
        {
          if ((levelData.GetLocation() == loc || loc == LevelLocation.Any) && levelData.GetPath().EndsWith("/" + path))
            return levelData.metaData.guid;
        }
      }
      LevelData levelData1 = DuckFile.LoadLevel(DuckGame.Content.path + "/levels/" + path + ".lev");
      return levelData1 != null ? levelData1.metaData.guid : "";
    }

    public static List<string> GetLevels(string dir, LevelLocation location)
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, List<LevelData>> level in (MultiMap<string, LevelData, List<LevelData>>) DuckGame.Content._levels)
      {
        foreach (LevelData levelData in level.Value)
        {
          if ((levelData.GetLocation() == location || location == LevelLocation.Any) && levelData.GetPath().IndexOf(dir + "/") >= 0)
            stringList.Add(level.Key);
        }
      }
      return stringList;
    }

    private static void ProcessLevel(string path, LevelLocation location)
    {
      try
      {
        Main.SpecialCode = "Loading Level " + path != null ? path : "null";
        if (!path.EndsWith(".lev"))
          return;
        path = path.Replace('\\', '/');
        LevelData dat = location != LevelLocation.Content ? DuckFile.LoadLevel(path) : DuckFile.LoadLevel(DuckFile.ReadEntireStream(TitleContainer.OpenStream(path)));
        if (dat != null)
        {
          dat.SetPath(path);
          path = path.Substring(0, path.Length - 4);
          path.Substring(path.IndexOf("/levels/") + 8);
          if (dat.metaData.guid != null)
            DuckGame.Content.MapLevel(dat.metaData.guid, dat, location);
        }
        ++MonoMain.loadyBits;
      }
      catch (Exception ex)
      {
        DuckGame.Content.LogLevelFailure(ex.ToString());
      }
    }

    private static void LogLevelFailure(string s)
    {
      try
      {
        Program.LogLine("Level Load Failure (Did not cause crash)\n================================================\n " + s + "\n================================================\n");
      }
      catch (Exception ex)
      {
      }
    }

    private static void ProcessTexture(string path)
    {
      if (MonoMain.lockLoading)
      {
        MonoMain.loadingLocked = true;
        while (MonoMain.lockLoading)
          Thread.Sleep(10);
      }
      MonoMain.loadingLocked = false;
      if (!path.EndsWith(".xnb"))
        return;
      path = path.Replace('\\', '/');
      if (path.StartsWith("Content/"))
        path = path.Substring(8);
      path = path.Substring(0, path.Length - 4);
      DuckGame.Content.Load<Tex2D>(path);
      ++MonoMain.lazyLoadyBits;
    }

    private static void ProcessEffect(string path)
    {
      if (!path.EndsWith(".xnb"))
        return;
      path = path.Replace('\\', '/');
      if (path.StartsWith("Content/"))
        path = path.Substring(8);
      path = path.Substring(0, path.Length - 4);
      DuckGame.Content.Load<MTEffect>(path);
      ++MonoMain.lazyLoadyBits;
    }

    public static string path => DuckGame.Content._path;

    public static void InitializeBase(ContentManager manager)
    {
      DuckGame.Content._base = manager;
      DuckGame.Content.invalidTexture = DuckGame.Content.Load<Tex2D>("notexture");
      DuckGame.Content._path = Directory.GetCurrentDirectory() + "/Content/";
    }

    public static void InitializeLevels()
    {
      MonoMain.loadMessage = "Loading Levels";
      DuckGame.Content.SearchDirLevels("Content/levels", LevelLocation.Content);
      DuckGame.Content.SearchDirLevels(DuckFile.levelDirectory, LevelLocation.Custom);
      if (!Steam.IsInitialized())
        return;
      bool done = false;
      WorkshopQueryUser queryUser = Steam.CreateQueryUser(Steam.user.id, WorkshopList.Subscribed, WorkshopType.UsableInGame, WorkshopSortOrder.TitleAsc);
      queryUser.requiredTags.Add("Map");
      queryUser.onlyQueryIDs = true;
      queryUser.QueryFinished += (WorkshopQueryFinished) (sender => done = true);
      queryUser.ResultFetched += (WorkshopQueryResultFetched) ((sender, result) =>
      {
        WorkshopItem publishedFile = result.details.publishedFile;
        if ((publishedFile.stateFlags & WorkshopItemState.Installed) == WorkshopItemState.None)
          return;
        DuckGame.Content.SearchDirLevels(publishedFile.path, LevelLocation.Workshop);
      });
      queryUser.Request();
      while (!done)
      {
        Steam.Update();
        Thread.Sleep(13);
      }
    }

    public static void Initialize(bool reverse)
    {
      MonoMain.loadMessage = "Loading Textures";
      DuckGame.Content.SearchDirTextures("Content/", reverse);
    }

    public static void Initialize() => DuckGame.Content.Initialize(false);

    public static void InitializeEffects()
    {
      MonoMain.loadMessage = "Loading Effects";
      DuckGame.Content.SearchDirEffects("Content/Shaders");
    }

    public static string[] GetFiles(string path, string filter = "*.*")
    {
      path = path.Replace('\\', '/');
      path = path.Trim('/');
      string str1 = (Directory.GetCurrentDirectory() + "/").Replace('\\', '/');
      List<string> stringList = new List<string>();
      foreach (string path1 in DuckFile.GetFilesNoCloud(path, filter))
      {
        if (!Path.GetFileName(path1).Contains("._"))
        {
          string str2 = path1.Replace('\\', '/');
          int startIndex = str2.IndexOf(str1);
          if (startIndex != -1)
            str2 = str2.Remove(startIndex, str1.Length);
          stringList.Add(str2);
        }
      }
      return stringList.ToArray();
    }

    public static string[] GetDirectories(string path, string filter = "*.*")
    {
      path = path.Replace('\\', '/');
      path = path.Trim('/');
      List<string> stringList = new List<string>();
      foreach (string path1 in DuckFile.GetDirectoriesNoCloud(path))
      {
        if (!Path.GetFileName(path1).Contains("._"))
          stringList.Add(path1);
      }
      return stringList.ToArray();
    }

    public static void Update()
    {
    }

    public static void UpdateRender()
    {
      if (!DuckGame.Content.renderingPreview || !DuckGame.Content.readyToRenderPreview)
        return;
      DuckGame.Content.DoPreviewRender();
      DuckGame.Content.readyToRenderPreview = false;
      do
        ;
      while (DuckGame.Content.previewRendering);
    }

    public static T Load<T>(string name)
    {
      if (typeof (T) == typeof (Tex2D))
      {
        Tex2D tex2D = (Tex2D) null;
        lock (DuckGame.Content._textures)
          DuckGame.Content._textures.TryGetValue(name, out tex2D);
        if (tex2D == null)
        {
          Texture2D tex = (Texture2D) null;
          bool flag = false;
          if (MonoMain.moddingEnabled && ModLoader.modsEnabled && (name.Length > 1 && name[1] == ':'))
            flag = true;
          if (!flag)
          {
            lock (DuckGame.Content._loadLock)
            {
              try
              {
                tex = DuckGame.Content._base.Load<Texture2D>(name);
              }
              catch
              {
                flag = MonoMain.moddingEnabled && ModLoader.modsEnabled;
              }
            }
          }
          if (flag)
          {
            foreach (Mod accessibleMod in (IEnumerable<Mod>) ModLoader.accessibleMods)
            {
              if (accessibleMod.configuration != null && accessibleMod.configuration.content != null)
                tex = accessibleMod.configuration.content.Load<Texture2D>(name);
              if (tex != null)
                break;
            }
          }
          else if (tex == null)
          {
            try
            {
              tex = ContentPack.LoadTexture2D(name);
            }
            catch (Exception ex)
            {
            }
          }
          if (tex == null)
          {
            tex = (Texture2D) DuckGame.Content.invalidTexture;
            Main.SpecialCode = "Couldn't load texture " + name;
          }
          lock (DuckGame.Content._loadLock)
          {
            tex2D = new Tex2D(tex, name, DuckGame.Content._currentTextureIndex);
            ++DuckGame.Content._currentTextureIndex;
            DuckGame.Content._textureList.Add(tex2D);
            DuckGame.Content._textures[name] = tex2D;
            DuckGame.Content._texture2DMap[(object) tex] = tex2D;
          }
        }
        return (T) tex2D;
      }
      if (typeof (T) == typeof (MTEffect))
      {
        MTEffect mtEffect = (MTEffect) null;
        lock (DuckGame.Content._effects)
          DuckGame.Content._effects.TryGetValue(name, out mtEffect);
        if (mtEffect == null)
        {
          Effect effect = (Effect) null;
          lock (DuckGame.Content._loadLock)
            effect = DuckGame.Content._base.Load<Effect>(name);
          lock (DuckGame.Content._loadLock)
          {
            mtEffect = new MTEffect(effect, name, DuckGame.Content._currentEffectIndex);
            ++DuckGame.Content._currentEffectIndex;
            DuckGame.Content._effectList.Add(mtEffect);
            DuckGame.Content._effects[name] = mtEffect;
            DuckGame.Content._effectMap[effect] = mtEffect;
          }
        }
        return (T) mtEffect;
      }
      if (typeof (T) == typeof (SoundEffect))
      {
        SoundEffect soundEffect = (SoundEffect) null;
        lock (DuckGame.Content._loadLock)
        {
          try
          {
            soundEffect = DuckGame.Content._base.Load<SoundEffect>(name);
          }
          catch
          {
          }
        }
        if (MonoMain.moddingEnabled && ModLoader.modsEnabled && soundEffect == null)
        {
          foreach (Mod accessibleMod in (IEnumerable<Mod>) ModLoader.accessibleMods)
          {
            if (accessibleMod.configuration != null && accessibleMod.configuration.content != null)
              soundEffect = accessibleMod.configuration.content.Load<SoundEffect>(name);
            if (soundEffect != null)
              break;
          }
        }
        return (T) soundEffect;
      }
      if (!(typeof (T) == typeof (Song)))
        return DuckGame.Content._base.Load<T>(name);
      if (MonoMain.moddingEnabled && ModLoader.modsEnabled)
      {
        foreach (Mod accessibleMod in (IEnumerable<Mod>) ModLoader.accessibleMods)
        {
          if (accessibleMod.configuration != null && accessibleMod.configuration.content != null)
          {
            Song song = accessibleMod.configuration.content.Load<Song>(name);
            if (song != null)
              return (T) song;
          }
        }
      }
      return default (T);
    }
  }
}
