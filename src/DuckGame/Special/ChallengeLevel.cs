﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ChallengeLevel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class ChallengeLevel : XMLLevel, IHaveAVirtualTransition
  {
    protected FollowCam _followCam;
    private BitmapFont _font;
    private LevelData _levelData;
    public bool _validityTest;
    private List<Duck> _pendingSpawns;
    public static int targetsShot = 0;
    public static int goodiesGot = 0;
    public static bool running = false;
    public static bool allTargetsShot = false;
    private static ChallengeTimer _timer;
    public static bool random = false;
    private MenuBoolean _capture = new MenuBoolean();
    private MenuBoolean _quit = new MenuBoolean();
    private MenuBoolean _restart = new MenuBoolean();
    private UIComponent _pauseGroup;
    private UIComponent _trophyGroup;
    private UIMenu _pauseMenu;
    private UIMenu _confirmMenu;
    private UIMenu _trophyMenu;
    private UIMenu _captureMenu;
    private bool _firstStart = true;
    private float _finishWait = 0.75f;
    private bool _finished;
    private bool _playedEndMusic;
    private float _restartMessageWait = 1f;
    private bool _win;
    private bool _developer;
    private ChallengeMode _challenge;
    private bool _doRestart;
    private float _waitForRestart = 1f;
    private float _waitFade;
    protected float _waitSpawn = 2f;
    private float _showResultsWait = 1f;
    private float _waitAfterSpawn = 1f;
    private int _waitAfterSpawnDings;
    private bool _didFade;
    private bool _started;
    private float _fontFade = 1f;
    private bool _paused;
    private bool _restarting;
    private static Duck _duck;
    private bool _showedEndMenu;
    private float _showEndTextWait = 1f;
    private bool _fading;
    private RenderTarget2D _captureTarget;

    public FollowCam followCam => this._followCam;

    public ChallengeLevel(string name)
      : base(name)
    {
      this._followCam = new FollowCam();
      this._followCam.lerpMult = 1f;
      this._followCam.startCentered = false;
      this.camera = (Camera) this._followCam;
      this.simulatePhysics = false;
    }

    public ChallengeLevel(LevelData data, bool validityTest)
      : base(data)
    {
      this._followCam = new FollowCam();
      this._followCam.lerpMult = 1f;
      this._followCam.startCentered = false;
      this.camera = (Camera) this._followCam;
      this.simulatePhysics = false;
      this._levelData = data;
      this._validityTest = validityTest;
    }

    public static ChallengeTimer timer => ChallengeLevel._timer;

    public override void Initialize()
    {
      ChallengeLevel.targetsShot = 0;
      ChallengeLevel.goodiesGot = 0;
      ChallengeLevel.allTargetsShot = true;
      ChallengeLevel.running = false;
      ChallengeLevel._timer = new ChallengeTimer();
      base.Initialize();
      this._font = new BitmapFont("biosFont", 8);
      foreach (Team team in Teams.all)
        team.prevScoreboardScore = team.score = 0;
      bool flag = true;
      foreach (Profile prof in Profiles.active)
      {
        if (flag)
        {
          flag = false;
        }
        else
        {
          if (prof.team != null)
            prof.team.Leave(prof);
          prof.inputProfile = (InputProfile) null;
        }
      }
      this._pendingSpawns = new Deathmatch((Level) this).SpawnPlayers();
      this._pendingSpawns = this._pendingSpawns.OrderBy<Duck, float>((Func<Duck, float>) (sp => sp.x)).ToList<Duck>();
      foreach (Thing pendingSpawn in this._pendingSpawns)
        this.followCam.Add(pendingSpawn);
      this._pauseGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._pauseMenu = new UIMenu("@LWING@PAUSE@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@DPAD@MOVE  @SELECT@SELECT");
      this._confirmMenu = new UIMenu("REALLY QUIT?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      this._captureMenu = (UIMenu) new UICaptureBox(this._pauseMenu, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f);
      this._captureMenu.Close();
      this._pauseGroup.Add((UIComponent) this._captureMenu, false);
      UIDivider uiDivider = new UIDivider(true, 0.8f);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("RESTART!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._restart), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("RESUME", (UIMenuAction) new UIMenuActionCloseMenu(this._pauseGroup), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("OPTIONS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._pauseMenu, (UIComponent) Options.optionsMenu), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("QUIT", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._pauseMenu, (UIComponent) this._confirmMenu), UIAlign.Left), true);
      if (this.things[typeof (EditorTestLevel)].Count<Thing>() > 0)
      {
        uiDivider.leftSection.Add((UIComponent) new UIText("", Color.White), true);
        uiDivider.leftSection.Add((UIComponent) new UIMenuItem("CAPTURE ICON", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._pauseMenu, (UIComponent) this._captureMenu), UIAlign.Left), true);
      }
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
      this._pauseGroup.Close();
      Level.Add((Thing) this._pauseGroup);
      Music.volume = 1f;
      this.followCam.Adjust();
    }

    public void ChallengeEnded(ChallengeMode challenge)
    {
      Music.Stop();
      this._developer = false;
      if (challenge.wonTrophies.Count > 0)
      {
        SFX.Play("scoreDing");
        this._win = true;
        if (challenge.wonTrophies[0].type == TrophyType.Developer)
          this._developer = true;
      }
      else
      {
        SFX.Play("recordStop");
        this._win = false;
      }
      this._finished = true;
      this._challenge = challenge;
    }

    public void RestartChallenge() => this._doRestart = true;

    public static Duck duck => ChallengeLevel._duck;

    public override void Update()
    {
      ChallengeLevel._timer.Update();
      if (this._fading)
      {
        DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, 0.0f, 0.05f);
        if ((double) DuckGame.Graphics.fade >= 0.00999999977648258)
          return;
        if (this._validityTest)
        {
          ArcadeTestDialogue.success = this._challenge.wonTrophies.Count > 0 && this._challenge.wonTrophies[0].type == TrophyType.Developer;
          Level.current = (Level) ArcadeTestDialogue.currentEditor;
          DuckGame.Graphics.fade = 1f;
        }
        else
        {
          if (this.things[typeof (EditorTestLevel)].Count<Thing>() > 0)
          {
            Level.current = (Level) (this.things[typeof (EditorTestLevel)].First<Thing>() as EditorTestLevel).editor;
            Music.Stop();
          }
          else
            Level.current = Arcade.currentArcade == null ? (Level) ArcadeLevel.currentArcade : (Level) Arcade.currentArcade;
          this._fading = false;
        }
      }
      else
      {
        if ((double) this._restartMessageWait > 0.0)
          this._restartMessageWait -= 0.008f;
        else
          HUD.CloseCorner(HUDCorner.TopLeft);
        if (this._doRestart)
        {
          ChallengeLevel.running = false;
          this._waitForRestart -= 0.04f;
          if ((double) this._waitForRestart <= 0.0)
            this._restarting = true;
        }
        this._waitFade -= 0.04f;
        if (!this._didFade && (double) this._waitFade <= 0.0 && (double) DuckGame.Graphics.fade < 1.0)
          DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, 1f, 0.04f);
        else if (this._restarting)
        {
          ChallengeLevel.running = false;
          this.transitionSpeedMultiplier = 2f;
          EditorTestLevel editorTestLevel = (EditorTestLevel) null;
          if (this.things[typeof (EditorTestLevel)].Count<Thing>() > 0)
            editorTestLevel = this.things[typeof (EditorTestLevel)].First<Thing>() as EditorTestLevel;
          Level.current = !(this._level != "") ? (Level) new ChallengeLevel(this._levelData, this._validityTest) : (Level) new ChallengeLevel(this._level);
          Level.current.transitionSpeedMultiplier = 2f;
          ((ChallengeLevel) Level.current)._waitSpawn = 0.0f;
          if (editorTestLevel == null)
            return;
          Level.current.AddThing((Thing) editorTestLevel);
        }
        else
        {
          if ((double) this._waitFade > 0.0)
            return;
          this._didFade = true;
          if (this._finished)
          {
            ChallengeLevel.running = false;
            this.PauseLogic();
            if ((double) this._finishWait > 0.0)
            {
              this._finishWait -= 0.01f;
            }
            else
            {
              if (!this._playedEndMusic)
              {
                this._playedEndMusic = true;
                Level.current.simulatePhysics = false;
                ArcadeFrame arcadeFrame = (ArcadeFrame) null;
                if (this._win)
                {
                  if (ArcadeLevel.currentArcade != null)
                  {
                    arcadeFrame = ArcadeLevel.currentArcade.GetFrame();
                    if (arcadeFrame != null)
                    {
                      Vec2 renderTargetSize = arcadeFrame.GetRenderTargetSize();
                      float renderTargetZoom = arcadeFrame.GetRenderTargetZoom();
                      if (this._captureTarget == null)
                        this._captureTarget = new RenderTarget2D((int) ((double) renderTargetSize.x * 6.0), (int) ((double) renderTargetSize.y * 6.0));
                      int num = DuckGame.Graphics.width / 320;
                      Camera camera = new Camera(0.0f, 0.0f, (float) this._captureTarget.width * renderTargetZoom, (float) this._captureTarget.height * renderTargetZoom);
                      if (ChallengeLevel._duck != null)
                      {
                        Layer.HUD.visible = false;
                        MonoMain.RenderGame(MonoMain.screenCapture);
                        Layer.HUD.visible = true;
                        Matrix result;
                        Matrix.CreateOrthographicOffCenter(0.0f, (float) MonoMain.screenCapture.width, (float) MonoMain.screenCapture.height, 0.0f, 0.0f, -1f, out result);
                        result.M41 += -0.5f * result.M11;
                        result.M42 += -0.5f * result.M22;
                        Matrix matrix = Level.current.camera.getMatrix();
                        Vec3 vec3 = (Vec3) DuckGame.Graphics.viewport.Project((Vector3) new Vec3(ChallengeLevel._duck.cameraPosition.x, ChallengeLevel._duck.cameraPosition.y, 0.0f), (Microsoft.Xna.Framework.Matrix) result, (Microsoft.Xna.Framework.Matrix) matrix, (Microsoft.Xna.Framework.Matrix) Matrix.Identity);
                        DuckGame.Graphics.SetRenderTarget(this._captureTarget);
                        camera.center = new Vec2(vec3.x, vec3.y);
                        if ((double) camera.bottom > (double) MonoMain.screenCapture.height)
                          camera.centerY = (float) MonoMain.screenCapture.height - camera.height / 2f;
                        if ((double) camera.top < 0.0)
                          camera.centerY = camera.height / 2f;
                        if ((double) camera.right > (double) MonoMain.screenCapture.width)
                          camera.centerX = (float) MonoMain.screenCapture.width - camera.width / 2f;
                        if ((double) camera.left < 0.0)
                          camera.centerX = camera.width / 2f;
                        DuckGame.Graphics.Clear(Color.Black);
                        DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, (MTEffect) null, camera.getMatrix());
                        DuckGame.Graphics.Draw((Tex2D) MonoMain.screenCapture, 0.0f, 0.0f);
                        DuckGame.Graphics.screen.End();
                        DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
                      }
                    }
                  }
                  if (this._challenge.wonTrophies.Count > 0 && this._challenge.wonTrophies[0].type == TrophyType.Developer)
                    SFX.Play("developerWin");
                  else
                    SFX.Play("challengeWin");
                  this._showEndTextWait = 1f;
                }
                else
                {
                  SFX.Play("challengeLose");
                  this._showEndTextWait = 1f;
                }
                if (this._challenge.wonTrophies.Count > 0)
                {
                  this._trophyGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
                  this._trophyMenu = new UIMenu("@LWING@" + this._challenge.challenge.name + "@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 210f, conString: "@DPAD@MOVE  @SELECT@SELECT");
                  UIDivider uiDivider1 = new UIDivider(false, 0.0f, 6f);
                  UIDivider uiDivider2 = new UIDivider(true, 0.0f);
                  SpriteMap spriteMap = new SpriteMap("challengeTrophy", 70, 65);
                  if (this._challenge.wonTrophies.Count > 0)
                  {
                    if (this._challenge.wonTrophies[0].type == TrophyType.Bronze)
                    {
                      spriteMap.frame = 1;
                      uiDivider2.leftSection.Add((UIComponent) new UIText("BRONZE", Colors.Bronze, UIAlign.Top), true);
                    }
                    else if (this._challenge.wonTrophies[0].type == TrophyType.Silver)
                    {
                      spriteMap.frame = 2;
                      uiDivider2.leftSection.Add((UIComponent) new UIText("SILVER", Colors.Silver, UIAlign.Top), true);
                    }
                    else if (this._challenge.wonTrophies[0].type == TrophyType.Gold)
                    {
                      spriteMap.frame = 3;
                      uiDivider2.leftSection.Add((UIComponent) new UIText("GOLD", Colors.Gold, UIAlign.Top), true);
                    }
                    else if (this._challenge.wonTrophies[0].type == TrophyType.Platinum)
                    {
                      spriteMap.frame = 4;
                      uiDivider2.leftSection.Add((UIComponent) new UIText("PLATINUM", Colors.Platinum, UIAlign.Top), true);
                    }
                    else if (this._challenge.wonTrophies[0].type == TrophyType.Developer)
                    {
                      spriteMap.frame = 5;
                      uiDivider2.leftSection.Add((UIComponent) new UIText("UR THE BEST", Colors.Developer, UIAlign.Top), true);
                    }
                  }
                  uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                  bool flag = false;
                  ChallengeSaveData saveData = Challenges.GetSaveData(this.id, ChallengeLevel._duck.profile);
                  int bestTime = saveData.bestTime;
                  if (saveData.bestTime == 0 || (int) (ChallengeLevel.timer.elapsed.TotalSeconds * 1000.0) < saveData.bestTime)
                    saveData.bestTime = (int) (ChallengeLevel.timer.elapsed.TotalSeconds * 1000.0);
                  if (this._challenge.wonTrophies[0].type > saveData.trophy)
                  {
                    saveData.trophy = this._challenge.wonTrophies[0].type;
                    if (saveData.trophy > TrophyType.Silver)
                      flag = true;
                  }
                  int targets1 = saveData.targets;
                  if (ChallengeLevel.targetsShot > saveData.targets)
                    saveData.targets = ChallengeLevel.targetsShot;
                  int targets2 = saveData.targets;
                  if (ChallengeLevel.goodiesGot > saveData.goodies)
                    saveData.goodies = ChallengeLevel.goodiesGot;
                  if (this._challenge.challenge.hasTimeRequirements)
                  {
                    uiDivider2.leftSection.Add((UIComponent) new UIText("TIME", Color.White, UIAlign.Left), true);
                    uiDivider2.leftSection.Add((UIComponent) new UIText(MonoMain.TimeString(ChallengeLevel.timer.elapsed, small: true), Color.Lime, UIAlign.Right), true);
                    uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                    if (targets1 != 0)
                    {
                      if ((double) bestTime < ChallengeLevel.timer.elapsed.TotalSeconds * 1000.0)
                      {
                        TimeSpan span = TimeSpan.FromMilliseconds(ChallengeLevel.timer.elapsed.TotalSeconds * 1000.0 - (double) bestTime);
                        uiDivider2.leftSection.Add((UIComponent) new UIText("DIFFERENCE", Color.White, UIAlign.Left), true);
                        uiDivider2.leftSection.Add((UIComponent) new UIText("+" + MonoMain.TimeString(span, small: true), Color.Red, UIAlign.Right), true);
                      }
                      else
                      {
                        TimeSpan span = TimeSpan.FromMilliseconds((double) bestTime - ChallengeLevel.timer.elapsed.TotalSeconds * 1000.0);
                        uiDivider2.leftSection.Add((UIComponent) new UIText("DIFFERENCE", Color.White, UIAlign.Left), true);
                        uiDivider2.leftSection.Add((UIComponent) new UIText("-" + MonoMain.TimeString(span, small: true), Color.Lime, UIAlign.Right), true);
                        flag = true;
                      }
                    }
                    else
                    {
                      uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                      uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                    }
                  }
                  if (saveData.trophy < TrophyType.Gold)
                    flag = false;
                  if (this._challenge.challenge.countTargets)
                  {
                    if (this._challenge.challenge.prefix != "" && this._challenge.challenge.prefix != null)
                      uiDivider2.leftSection.Add((UIComponent) new UIText(this._challenge.challenge.prefix, Color.White, UIAlign.Left), true);
                    else
                      uiDivider2.leftSection.Add((UIComponent) new UIText("TARGETS", Color.White, UIAlign.Left), true);
                    string textVal = Convert.ToString(ChallengeLevel.targetsShot);
                    Color c = Color.Lime;
                    if (targets1 != 0)
                    {
                      if (targets1 < ChallengeLevel.targetsShot)
                      {
                        int num = ChallengeLevel.targetsShot - targets1;
                        c = Color.Lime;
                        textVal = textVal + " (+" + Convert.ToString(num) + ")";
                        flag = true;
                      }
                      else if (targets1 > ChallengeLevel.targetsShot)
                      {
                        int num = targets1 - ChallengeLevel.targetsShot;
                        c = Color.Red;
                        textVal = textVal + " (-" + Convert.ToString(num) + ")";
                      }
                      else
                        c = Color.White;
                    }
                    uiDivider2.leftSection.Add((UIComponent) new UIText(textVal, c, UIAlign.Right), true);
                    uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                  }
                  if (this._challenge.challenge.countGoodies)
                  {
                    if (this._challenge.challenge.prefix != "" && this._challenge.challenge.prefix != null)
                      uiDivider2.leftSection.Add((UIComponent) new UIText(this._challenge.challenge.prefix, Color.White, UIAlign.Left), true);
                    else
                      uiDivider2.leftSection.Add((UIComponent) new UIText("NUMBER", Color.White, UIAlign.Left), true);
                    string textVal = Convert.ToString(ChallengeLevel.goodiesGot);
                    Color c = Color.Lime;
                    if (targets2 != 0)
                    {
                      if (targets2 < ChallengeLevel.goodiesGot)
                      {
                        int num = ChallengeLevel.goodiesGot - targets2;
                        c = Color.Lime;
                        textVal = textVal + " (+" + Convert.ToString(num) + ")";
                        flag = true;
                      }
                      else if (targets2 > ChallengeLevel.goodiesGot)
                      {
                        int num = targets2 - ChallengeLevel.goodiesGot;
                        c = Color.Red;
                        textVal = textVal + " (-" + Convert.ToString(num) + ")";
                      }
                      else
                        c = Color.White;
                    }
                    uiDivider2.leftSection.Add((UIComponent) new UIText(textVal, c, UIAlign.Right), true);
                    uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                  }
                  uiDivider2.rightSection.Add((UIComponent) new UIImage((Sprite) spriteMap, UIAlign.Right), true);
                  uiDivider1.leftSection.Add((UIComponent) uiDivider2, true);
                  uiDivider1.rightSection.vertical = false;
                  uiDivider1.rightSection.borderSize.y = 2f;
                  if (this._validityTest)
                  {
                    if (this._challenge.wonTrophies.Count > 0 && this._challenge.wonTrophies[0].type == TrophyType.Developer)
                      uiDivider1.rightSection.Add((UIComponent) new UIMenuItem("CONTINUE   ", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._trophyGroup, this._quit), UIAlign.Left), true);
                    else
                      uiDivider1.rightSection.Add((UIComponent) new UIMenuItem("RETRY", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._trophyGroup, this._restart), UIAlign.Left), true);
                  }
                  else
                  {
                    uiDivider1.rightSection.Add((UIComponent) new UIMenuItem("CONTINUE   ", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._trophyGroup, this._quit), UIAlign.Left), true);
                    uiDivider1.rightSection.Add((UIComponent) new UIMenuItem("RETRY", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._trophyGroup, this._restart), UIAlign.Left), true);
                  }
                  this._trophyMenu.Add((UIComponent) uiDivider1, true);
                  this._trophyMenu.Close();
                  this._trophyGroup.Add((UIComponent) this._trophyMenu, false);
                  this._trophyGroup.Close();
                  Level.Add((Thing) this._trophyGroup);
                  if (arcadeFrame != null && flag && saveData != null)
                  {
                    saveData.frameID = arcadeFrame._identifier;
                    saveData.frameImage = Editor.TextureToString((Texture2D) (Tex2D) this._captureTarget);
                    arcadeFrame.saveData = saveData;
                  }
                  Challenges.Save(this.id);
                  Profiles.Save(ChallengeLevel._duck.profile);
                }
                else
                {
                  this._trophyGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
                  this._trophyMenu = new UIMenu("@LWING@" + this._challenge.challenge.name + "@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 210f, conString: "@DPAD@MOVE  @SELECT@SELECT");
                  UIDivider uiDivider1 = new UIDivider(false, 0.0f, 6f);
                  UIDivider uiDivider2 = new UIDivider(true, 0.0f);
                  uiDivider2.leftSection.Add((UIComponent) new UIText("FAILED", Color.Red, UIAlign.Top), true);
                  uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                  uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                  uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.Lime, UIAlign.Right), true);
                  uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                  uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                  uiDivider2.leftSection.Add((UIComponent) new UIText("               ", Color.White, UIAlign.Left), true);
                  uiDivider2.rightSection.Add((UIComponent) new UIImage((Sprite) new SpriteMap("challengeTrophy", 70, 65)
                  {
                    frame = 0
                  }, UIAlign.Right), true);
                  uiDivider1.leftSection.Add((UIComponent) uiDivider2, true);
                  uiDivider1.rightSection.vertical = false;
                  uiDivider1.rightSection.borderSize.y = 2f;
                  uiDivider1.rightSection.Add((UIComponent) new UIMenuItem("CONTINUE   ", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._trophyGroup, this._quit), UIAlign.Left), true);
                  uiDivider1.rightSection.Add((UIComponent) new UIMenuItem("RETRY", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._trophyGroup, this._restart), UIAlign.Left), true);
                  this._trophyMenu.Add((UIComponent) uiDivider1, true);
                  this._trophyMenu.Close();
                  this._trophyGroup.Add((UIComponent) this._trophyMenu, false);
                  this._trophyGroup.Add((UIComponent) Options.optionsMenu, false);
                  Options.openOnClose = this._trophyMenu;
                  this._trophyGroup.Close();
                  Level.Add((Thing) this._trophyGroup);
                }
              }
              if ((double) this._showEndTextWait > 0.0)
              {
                this._showEndTextWait -= 0.01f;
              }
              else
              {
                this._fontFade = 1f;
                if ((double) this._showResultsWait > 0.0)
                  this._showResultsWait -= 0.01f;
                else if (!this._showedEndMenu)
                {
                  this._trophyGroup.Open();
                  this._trophyMenu.Open();
                  MonoMain.pauseMenu = this._trophyGroup;
                  SFX.Play("pause", 0.6f, -0.2f);
                  this._showedEndMenu = true;
                }
                if (this._restart.value)
                {
                  this._restarting = true;
                  SFX.Play("resume", 0.6f);
                }
                else
                {
                  if (!this._quit.value)
                    return;
                  this._fading = true;
                  SFX.Play("resume", 0.6f);
                }
              }
            }
          }
          else
          {
            this._waitSpawn -= 0.06f;
            if ((double) this._waitSpawn > 0.0)
              return;
            if (this._pendingSpawns != null && this._pendingSpawns.Count > 0)
            {
              this._waitSpawn = 0.5f;
              Duck pendingSpawn = this._pendingSpawns[0];
              this.AddThing((Thing) pendingSpawn);
              this._pendingSpawns.RemoveAt(0);
              Vec3 color = pendingSpawn.profile.persona.color;
              Level.Add((Thing) new SpawnLine(pendingSpawn.x, pendingSpawn.y, 0, 0.0f, new Color((int) color.x, (int) color.z, (int) color.z), 32f));
              Level.Add((Thing) new SpawnLine(pendingSpawn.x, pendingSpawn.y, 0, -4f, new Color((int) color.x, (int) color.y, (int) color.z), 4f));
              Level.Add((Thing) new SpawnLine(pendingSpawn.x, pendingSpawn.y, 0, 4f, new Color((int) color.x, (int) color.y, (int) color.z), 4f));
              SFX.Play("pullPin", 0.7f);
              ChallengeLevel._duck = pendingSpawn;
              this._challenge = this.things[typeof (ChallengeMode)].First<Thing>() as ChallengeMode;
              ChallengeLevel.random = this._challenge.random.value;
              this._challenge.duck = pendingSpawn;
              ChallengeLevel._timer.maxTime = TimeSpan.FromSeconds((double) this._challenge.challenge.trophies[0].timeRequirement);
              HUD.AddCornerTimer(HUDCorner.BottomRight, "", (Timer) ChallengeLevel._timer);
              if (this._challenge.challenge.countTargets)
              {
                int targets = this._challenge.challenge.trophies[0].targets;
                HUD.AddCornerCounter(HUDCorner.BottomLeft, "@RETICULE@", new FieldBinding((object) this, "targetsShot"), targets > 0 ? targets : 0);
              }
              if (this._challenge.challenge.countGoodies)
              {
                MultiMap<System.Type, ISequenceItem> multiMap = new MultiMap<System.Type, ISequenceItem>();
                foreach (ISequenceItem element in Level.current.things[typeof (ISequenceItem)])
                {
                  System.Type type = element.GetType();
                  SequenceItem sequence = (element as Thing).sequence;
                  if (sequence.isValid && sequence.type == SequenceItemType.Goody)
                    multiMap.Add(type, element);
                }
                System.Type key = (System.Type) null;
                int num = 0;
                foreach (KeyValuePair<System.Type, List<ISequenceItem>> keyValuePair in (MultiMap<System.Type, ISequenceItem, List<ISequenceItem>>) multiMap)
                {
                  if (keyValuePair.Value.Count > num)
                  {
                    key = keyValuePair.Key;
                    num = keyValuePair.Value.Count;
                  }
                }
                if (key != (System.Type) null)
                {
                  ISequenceItem sequenceItem = multiMap[key][0];
                  string text = "@STARGOODY@";
                  switch (sequenceItem)
                  {
                    case LapGoody _:
                    case InvisiGoody _:
                      text = "@LAPGOODY@";
                      break;
                    case SuitcaseGoody _:
                      text = "@SUITCASEGOODY@";
                      break;
                    case Window _:
                    case YellowBarrel _:
                    case Door _:
                      text = "@RETICULE@";
                      break;
                  }
                  int goodies = this._challenge.challenge.trophies[0].goodies;
                  HUD.AddCornerCounter(HUDCorner.BottomLeft, text, new FieldBinding((object) this, "goodiesGot"), goodies > 0 ? goodies : 0);
                }
              }
              if (this._firstStart)
              {
                if (ChallengeLevel.random)
                {
                  IEnumerable<Thing> thing = this.things[typeof (ISequenceItem)];
                  if (thing.Count<Thing>() > 0)
                    thing.ElementAt<Thing>(Rando.Int(thing.Count<Thing>() - 1)).sequence.BeginRandomSequence();
                }
                else
                {
                  foreach (TargetDuck targetDuck in this.things[typeof (TargetDuck)])
                  {
                    if (targetDuck.sequence.order == 0)
                      targetDuck.sequence.Activate();
                  }
                }
                this._firstStart = false;
              }
              if (!Music.stopped)
                return;
              if ((string) this._challenge.music == "")
                Music.Load("Challenging");
              else if ((string) this._challenge.music == "donutmystery")
                Music.Load("spacemystery");
              else
                Music.Load(Music.FindSong((string) this._challenge.music));
            }
            else if (!this._started)
            {
              this._waitAfterSpawn -= 0.06f;
              if ((double) this._waitAfterSpawn > 0.0)
                return;
              ++this._waitAfterSpawnDings;
              if (this._waitAfterSpawnDings > 2)
              {
                this._started = true;
                this.simulatePhysics = true;
                ChallengeLevel.running = true;
                SFX.Play("ding");
                ChallengeLevel._timer.Start();
                if (Music.stopped)
                  Music.PlayLoaded();
              }
              else
                SFX.Play("preStartDing");
              this._waitSpawn = 1.1f;
            }
            else
            {
              this._fontFade -= 0.1f;
              if ((double) this._fontFade < 0.0)
                this._fontFade = 0.0f;
              this.PauseLogic();
            }
          }
        }
      }
    }

    public void PauseLogic()
    {
      if (Input.Pressed("START"))
      {
        this._pauseGroup.Open();
        this._pauseMenu.Open();
        MonoMain.pauseMenu = this._pauseGroup;
        if (!this._paused)
        {
          SFX.Play("pause", 0.6f);
          ChallengeLevel._timer.Stop();
          this._paused = true;
        }
        this.simulatePhysics = false;
      }
      else
      {
        if (!this._paused || MonoMain.pauseMenu != null)
          return;
        this._paused = false;
        SFX.Play("resume", 0.6f);
        this._waitAfterSpawn = 1f;
        this._waitAfterSpawnDings = 0;
        this._started = false;
        this._fontFade = 1f;
        if (this._restart.value)
        {
          this._restarting = true;
        }
        else
        {
          if (!this._quit.value)
            return;
          this._fading = true;
        }
      }
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer == Layer.HUD && (!this._started || this._finished) && (this._waitAfterSpawnDings > 0 && (double) this._fontFade > 0.00999999977648258))
      {
        this._font.scale = new Vec2(2f, 2f);
        this._font.alpha = this._fontFade;
        string text = "GET";
        if (this._finished)
          text = !this._win ? "LOSE!" : (!this._developer ? "WIN!" : "WOAH!");
        else if (this._waitAfterSpawnDings == 2)
          text = "READY";
        else if (this._waitAfterSpawnDings == 3)
          text = "";
        float width = this._font.GetWidth(text);
        float num = 1f;
        this._font.Draw(text, (float) ((double) Layer.HUD.camera.width / 2.0 - (double) width / 2.0) - num, (float) ((double) Layer.HUD.camera.height / 2.0 - (double) this._font.height / 2.0) - num, Color.Black, (Depth) 0.9f);
        this._font.Draw(text, (float) ((double) Layer.HUD.camera.width / 2.0 - (double) width / 2.0) - num, (float) ((double) Layer.HUD.camera.height / 2.0 - (double) this._font.height / 2.0) + num, Color.Black, (Depth) 0.9f);
        this._font.Draw(text, (float) ((double) Layer.HUD.camera.width / 2.0 - (double) width / 2.0) + num, (float) ((double) Layer.HUD.camera.height / 2.0 - (double) this._font.height / 2.0) - num, Color.Black, (Depth) 0.9f);
        this._font.Draw(text, (float) ((double) Layer.HUD.camera.width / 2.0 - (double) width / 2.0) + num, (float) ((double) Layer.HUD.camera.height / 2.0 - (double) this._font.height / 2.0) + num, Color.Black, (Depth) 0.9f);
        this._font.Draw(text, (float) ((double) Layer.HUD.camera.width / 2.0 - (double) width / 2.0), (float) ((double) Layer.HUD.camera.height / 2.0 - (double) this._font.height / 2.0), Color.White, (Depth) 1f);
      }
      base.PostDrawLayer(layer);
    }
  }
}
