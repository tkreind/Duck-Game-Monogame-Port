// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckGameTestArea
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class DuckGameTestArea : DeathmatchLevel
  {
    private Editor _editor;
    protected int _seed;
    protected RandomLevelData _center;
    protected LevGenType _genType;
    private string _levelValue;
    public static Editor currentEditor;
    private MenuBoolean _capture = new MenuBoolean();
    private MenuBoolean _quit = new MenuBoolean();
    private MenuBoolean _restart = new MenuBoolean();
    private MenuBoolean _startTestMode = new MenuBoolean();
    private UIComponent _pauseGroup;
    private UIMenu _pauseMenu;
    private UIMenu _confirmMenu;
    private UIMenu _testMode;
    public int numPlayers = 4;
    private bool _paused;
    private int wait;

    public DuckGameTestArea(
      Editor editor,
      string level,
      int seed = 0,
      RandomLevelData center = null,
      LevGenType genType = LevGenType.Any)
      : base(level)
    {
      this._editor = editor;
      DeathmatchLevel._started = true;
      this._followCam.lerpMult = 1.1f;
      this._seed = seed;
      this._center = center;
      this._genType = genType;
      this._levelValue = level;
      DuckGameTestArea.currentEditor = editor;
    }

    public override void Initialize()
    {
      this._pauseGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._pauseMenu = new UIMenu("@LWING@PAUSE@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@DPAD@MOVE  @SELECT@SELECT");
      this._confirmMenu = new UIMenu("REALLY QUIT?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      this._testMode = new UIMenu("TEST MODE", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      UIDivider uiDivider = new UIDivider(true, 0.8f);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("RESTART", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._restart), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("RESUME", (UIMenuAction) new UIMenuActionCloseMenu(this._pauseGroup), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("OPTIONS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._pauseMenu, (UIComponent) Options.optionsMenu), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("TEST MODE", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._pauseMenu, (UIComponent) this._testMode), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIText("", Color.White), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("QUIT", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._quit), UIAlign.Left), true);
      uiDivider.rightSection.Add((UIComponent) new UIImage("pauseIcons", UIAlign.Right), true);
      this._pauseMenu.Add((UIComponent) uiDivider, true);
      this._pauseMenu.Close();
      this._pauseGroup.Add((UIComponent) this._pauseMenu, false);
      this._pauseGroup.Add((UIComponent) Options.optionsMenu, false);
      Options.openOnClose = this._pauseMenu;
      this._confirmMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._confirmMenu, (UIComponent) this._pauseMenu)), true);
      this._confirmMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._quit)), true);
      this._confirmMenu.Close();
      this._pauseGroup.Add((UIComponent) this._confirmMenu, false);
      this._testMode.Add((UIComponent) new UIMenuItemNumber("PLAYERS", field: new FieldBinding((object) this, "numPlayers", 2f, 4f, 1f)), true);
      this._testMode.Add((UIComponent) new UIMenuItem("START", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._startTestMode)), true);
      this._testMode.Add((UIComponent) new UIText("", Color.White), true);
      this._testMode.Add((UIComponent) new UIMenuItem("CANCEL", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._testMode, (UIComponent) this._pauseMenu), backButton: true), true);
      this._testMode.Close();
      this._pauseGroup.Add((UIComponent) this._testMode, false);
      this._pauseGroup.Close();
      Level.Add((Thing) this._pauseGroup);
      if (this._level == "RANDOM")
      {
        LevelGenerator.MakeLevel(this._center, this._center.left && this._center.right, this._seed, this._genType, Editor._procTilesWide, Editor._procTilesHigh, Editor._procXPos, Editor._procYPos).LoadParts(0.0f, 0.0f, (Level) this, this._seed);
        List<SpawnPoint> source1 = new List<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in this.things[typeof (SpawnPoint)])
          source1.Add(spawnPoint);
        List<SpawnPoint> chosenSpawns = new List<SpawnPoint>();
        for (int index = 0; index < 4; ++index)
        {
          if (chosenSpawns.Count == 0)
          {
            chosenSpawns.Add(source1.ElementAt<SpawnPoint>(Rando.Int(source1.Count - 1)));
          }
          else
          {
            IOrderedEnumerable<SpawnPoint> source2 = source1.OrderByDescending<SpawnPoint, int>((Func<SpawnPoint, int>) (x =>
            {
              int num = 9999999;
              foreach (Transform transform in chosenSpawns)
                num = (int) Math.Min((transform.position - x.position).length, (float) num);
              return num;
            }));
            chosenSpawns.Add(source2.First<SpawnPoint>());
          }
        }
        foreach (SpawnPoint spawnPoint in source1)
        {
          if (!chosenSpawns.Contains(spawnPoint))
            Level.Remove((Thing) spawnPoint);
        }
        PyramidBackground pyramidBackground = new PyramidBackground(0.0f, 0.0f);
        pyramidBackground.visible = false;
        Level.Add((Thing) pyramidBackground);
      }
      else
      {
        this._level = this._level.Replace(Directory.GetCurrentDirectory() + "\\", "");
        LevelData levelData = DuckFile.LoadLevel(this._level);
        if (levelData != null)
        {
          foreach (BinaryClassChunk node in levelData.objects.objects)
          {
            Thing t = Thing.LoadThing(node);
            if (t != null)
            {
              if (!t.visibleInGame)
                t.visible = false;
              this.AddThing(t);
            }
          }
        }
      }
      this.things.RefreshState();
      foreach (Duck spawnPlayer in new Deathmatch((Level) this).SpawnPlayers())
      {
        Level.Add((Thing) spawnPlayer);
        this.followCam.Add((Thing) spawnPlayer);
      }
    }

    public void PauseLogic()
    {
      if (Input.Pressed("START"))
      {
        this._pauseGroup.Open();
        this._pauseMenu.Open();
        MonoMain.pauseMenu = this._pauseGroup;
        if (this._paused)
          return;
        SFX.Play("pause", 0.6f);
        this._paused = true;
      }
      else
      {
        if (!this._paused || MonoMain.pauseMenu != null)
          return;
        this._paused = false;
        SFX.Play("resume", 0.6f);
        DeathmatchLevel._started = false;
      }
    }

    public override void Update()
    {
      if (this._startTestMode.value)
      {
        foreach (Profile profile in Profiles.active)
          profile.team = (Team) null;
        if (this.numPlayers > 3)
        {
          Profiles.DefaultPlayer4.team = Teams.Player4;
          if (Profiles.DefaultPlayer4.inputProfile == null)
            Profiles.DefaultPlayer4.inputProfile = InputProfile.DefaultPlayer4;
        }
        if (this.numPlayers > 2)
        {
          Profiles.DefaultPlayer3.team = Teams.Player3;
          if (Profiles.DefaultPlayer3.inputProfile == null)
            Profiles.DefaultPlayer3.inputProfile = InputProfile.DefaultPlayer3;
        }
        if (this.numPlayers > 1)
        {
          Profiles.DefaultPlayer2.team = Teams.Player2;
          if (Profiles.DefaultPlayer2.inputProfile == null)
            Profiles.DefaultPlayer2.inputProfile = InputProfile.DefaultPlayer2;
        }
        if (this.numPlayers > 0)
        {
          Profiles.DefaultPlayer1.team = Teams.Player1;
          if (Profiles.DefaultPlayer1.inputProfile == null)
            Profiles.DefaultPlayer1.inputProfile = InputProfile.DefaultPlayer1;
        }
        EditorTestLevel editorTestLevel = (EditorTestLevel) null;
        if (this.things[typeof (EditorTestLevel)].Count<Thing>() > 0)
          editorTestLevel = this.things[typeof (EditorTestLevel)].First<Thing>() as EditorTestLevel;
        Level.current = (Level) new GameLevel(this._levelValue, editorTestMode: true);
        if (editorTestLevel == null)
          return;
        Level.current.AddThing((Thing) editorTestLevel);
      }
      else if (this._restart.value)
      {
        this.transitionSpeedMultiplier = 2f;
        EditorTestLevel editorTestLevel = (EditorTestLevel) null;
        if (this.things[typeof (EditorTestLevel)].Count<Thing>() > 0)
          editorTestLevel = this.things[typeof (EditorTestLevel)].First<Thing>() as EditorTestLevel;
        Level.current = (Level) new DuckGameTestArea(this._editor, this._levelValue, this._seed, this._center, this._genType);
        Level.current.transitionSpeedMultiplier = 2f;
        if (editorTestLevel == null)
          return;
        Level.current.AddThing((Thing) editorTestLevel);
      }
      else
      {
        if (this._level == "RANDOM")
        {
          if (this.wait < 4)
            ++this.wait;
          if (this.wait == 4)
          {
            ++this.wait;
            foreach (AutoBlock autoBlock in this.things[typeof (AutoBlock)])
              autoBlock.PlaceBlock();
            foreach (AutoPlatform autoPlatform in this.things[typeof (AutoPlatform)])
            {
              autoPlatform.PlaceBlock();
              autoPlatform.UpdateNubbers();
            }
            foreach (BlockGroup blockGroup in this.things[typeof (BlockGroup)])
            {
              foreach (Block block in blockGroup.blocks)
              {
                if (block is AutoBlock)
                  (block as AutoBlock).PlaceBlock();
              }
            }
          }
        }
        this.PauseLogic();
        if (this._quit.value)
          Level.current = (Level) this._editor;
        base.Update();
      }
    }
  }
}
