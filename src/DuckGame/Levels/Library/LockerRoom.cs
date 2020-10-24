﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.LockerRoom
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class LockerRoom : Level
  {
    private Sprite _background;
    private Sprite _boardHighlight;
    private Sprite _trophiesHighlight;
    private SinWave _pulse = (SinWave) 0.1f;
    private LockerSelection _selection;
    private LockerScreen _screen = LockerScreen.Stats;
    private LockerScreen _desiredScreen = LockerScreen.Stats;
    private float _statScroll;
    private List<LockerStat> _stats = new List<LockerStat>();
    private float _fade = 1f;
    private Profile _profile;
    private UIComponent _confirmGroup;
    private UIMenu _confirmMenu;
    private MenuBoolean _clearStats = new MenuBoolean();

    public LockerRoom(Profile p)
    {
      this._centeredView = true;
      this._profile = p;
    }

    public override void Initialize()
    {
      this._background = new Sprite("gym");
      this._boardHighlight = new Sprite("boardHighlight");
      this._boardHighlight.CenterOrigin();
      this._trophiesHighlight = new Sprite("trophiesHighlight");
      this._trophiesHighlight.CenterOrigin();
      HUD.AddCornerControl(HUDCorner.BottomLeft, "MOVE@DPAD@");
      HUD.AddCornerMessage(HUDCorner.TopLeft, this._profile.name);
      HUD.AddCornerControl(HUDCorner.TopRight, "@QUACK@EXIT");
      HUD.AddCornerControl(HUDCorner.BottomRight, "@GRAB@RESET");
      this.backgroundColor = Color.Black;
      this._confirmGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._confirmMenu = new UIMenu("CLEAR STATS?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      this._confirmMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionCloseMenu(this._confirmGroup)), true);
      this._confirmMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._confirmGroup, this._clearStats)), true);
      this._confirmMenu.Close();
      this._confirmGroup.Add((UIComponent) this._confirmMenu, false);
      this._confirmGroup.Close();
      Level.Add((Thing) this._confirmGroup);
      Profile profile = this._profile;
      this._stats.Add(new LockerStat("KILLS: " + Change.ToString((object) profile.stats.kills), Color.GreenYellow));
      this._stats.Add(new LockerStat("DEATHS: " + Change.ToString((object) profile.stats.timesKilled), Color.Red));
      this._stats.Add(new LockerStat("SUICIDES: " + Change.ToString((object) profile.stats.suicides), Color.Red));
      this._stats.Add(new LockerStat("", Color.Red));
      this._stats.Add(new LockerStat("ROUNDS WON: " + Change.ToString((object) profile.stats.matchesWon), Color.GreenYellow));
      this._stats.Add(new LockerStat("ROUNDS LOST: " + Change.ToString((object) (profile.stats.timesSpawned - profile.stats.matchesWon)), Color.Red));
      this._stats.Add(new LockerStat("GAMES WON: " + Change.ToString((object) profile.stats.trophiesWon), Color.GreenYellow));
      this._stats.Add(new LockerStat("GAMES LOST: " + Change.ToString((object) (profile.stats.gamesPlayed - profile.stats.trophiesWon)), Color.Red));
      this._stats.Add(new LockerStat("", Color.Red));
      this._stats.Add(new LockerStat("FANS: " + Change.ToString((object) profile.stats.GetFans()), Color.Lime));
      int fans = profile.stats.GetFans();
      int num1 = 0;
      if (fans > 0)
        num1 = (int) Math.Round((double) profile.stats.loyalFans / (double) profile.stats.GetFans() * 100.0);
      this._stats.Add(new LockerStat("FAN LOYALTY: " + Change.ToString((object) num1) + "%", Color.Lime));
      this._stats.Add(new LockerStat("", Color.Red));
      float num2 = 0.0f;
      if (profile.stats.bulletsFired > 0)
        num2 = (float) profile.stats.bulletsThatHit / (float) profile.stats.bulletsFired;
      this._stats.Add(new LockerStat("ACCURACY: " + Change.ToString((object) (int) Math.Round((double) num2 * 100.0)) + "%", (double) num2 > 0.600000023841858 ? Color.Green : Color.Red));
      this._stats.Add(new LockerStat("", Color.Red));
      this._stats.Add(new LockerStat("TIMES QUACKED: " + Change.ToString((object) profile.stats.quacks), Color.Orange));
      this._stats.Add(new LockerStat("MINES STEPPED ON: " + Change.ToString((object) profile.stats.minesSteppedOn), Color.Orange));
      this._stats.Add(new LockerStat("PRESENTS OPENED: " + Change.ToString((object) profile.stats.presentsOpened), Color.Orange));
      this._stats.Add(new LockerStat("", Color.Red));
      this._stats.Add(new LockerStat("SPIRITUALITY", Color.White));
      this._stats.Add(new LockerStat("FUNERALS: " + Change.ToString((object) profile.stats.funeralsPerformed), Color.Orange));
      this._stats.Add(new LockerStat("CONVERSIONS: " + Change.ToString((object) profile.stats.conversions), Color.Orange));
      this._stats.Add(new LockerStat("", Color.Red));
      this._stats.Add(new LockerStat("TIME SPENT", Color.White));
      this._stats.Add(new LockerStat("IN NET: " + Change.ToString((object) (int) profile.stats.timeInNet) + " SECONDS", Color.Orange));
      this._stats.Add(new LockerStat("ON FIRE: " + Change.ToString((object) (int) profile.stats.timeOnFire) + " SECONDS", Color.Orange));
      this._stats.Add(new LockerStat("BRAINWASHED: " + Change.ToString((object) (int) profile.stats.timeUnderMindControl) + " SECONDS", Color.Orange));
      this._stats.Add(new LockerStat("MOUTH OPEN: " + Change.ToString((object) (int) profile.stats.timeWithMouthOpen) + " SECONDS", Color.Orange));
      base.Initialize();
    }

    public override void Update()
    {
      int num = (int) this._selection;
      if (this._desiredScreen != this._screen)
      {
        this._fade = Lerp.Float(this._fade, 0.0f, 0.06f);
        if ((double) this._fade <= 0.0)
        {
          this._screen = this._desiredScreen;
          if (this._screen == LockerScreen.Stats)
          {
            this._statScroll = 0.0f;
            HUD.AddCornerControl(HUDCorner.BottomLeft, "MOVE@DPAD@");
            HUD.AddCornerMessage(HUDCorner.TopLeft, this._profile.name);
            HUD.AddCornerControl(HUDCorner.TopRight, "@QUACK@BACK");
          }
          else if (this._screen == LockerScreen.Trophies)
          {
            this._statScroll = 0.0f;
            HUD.AddCornerControl(HUDCorner.BottomLeft, "MOVE@DPAD@");
            HUD.AddCornerMessage(HUDCorner.TopLeft, "TROPHIES");
            HUD.AddCornerControl(HUDCorner.TopRight, "@QUACK@BACK");
          }
          else if (this._screen == LockerScreen.Locker)
          {
            HUD.AddCornerControl(HUDCorner.BottomLeft, "MOVE@DPAD@");
            HUD.AddCornerMessage(HUDCorner.TopLeft, "LOCKER ROOM");
            HUD.AddCornerControl(HUDCorner.TopRight, "@QUACK@EXIT");
          }
          else if (this._screen == LockerScreen.Exit)
          {
            Graphics.fade = 0.0f;
            Level.current = (Level) new DoorRoom(this._profile);
          }
        }
      }
      else
      {
        this._fade = Lerp.Float(this._fade, 1f, 0.06f);
        if (this._screen == LockerScreen.Locker)
        {
          if (InputProfile.active.Pressed("LEFT"))
          {
            --num;
            if (num < 0)
              num = 1;
          }
          if (InputProfile.active.Pressed("RIGHT"))
          {
            ++num;
            if (num >= 2)
              num = 0;
          }
          this._selection = (LockerSelection) num;
          if (InputProfile.active.Pressed("SELECT"))
          {
            if (this._selection == LockerSelection.Stats)
            {
              this._desiredScreen = LockerScreen.Stats;
              HUD.CloseAllCorners();
            }
            if (this._selection == LockerSelection.Trophies)
            {
              this._desiredScreen = LockerScreen.Trophies;
              HUD.CloseAllCorners();
            }
          }
          if (InputProfile.active.Pressed("QUACK"))
          {
            this._desiredScreen = LockerScreen.Exit;
            HUD.CloseAllCorners();
          }
        }
        else if (this._screen == LockerScreen.Stats)
        {
          if (InputProfile.active.Down("UP"))
          {
            this._statScroll -= 0.02f;
            if ((double) this._statScroll < 0.0)
              this._statScroll = 0.0f;
          }
          if (InputProfile.active.Down("DOWN"))
          {
            this._statScroll += 0.02f;
            if ((double) this._statScroll > 1.0)
              this._statScroll = 1f;
          }
          if (InputProfile.active.Pressed("QUACK"))
          {
            this._desiredScreen = LockerScreen.Exit;
            HUD.CloseAllCorners();
          }
          if (this._clearStats.value)
          {
            this._clearStats.value = false;
            this._profile.stats = new ProfileStats();
            Profiles.Save(this._profile);
            Level.current = (Level) new LockerRoom(this._profile);
          }
          if (InputProfile.active.Pressed("GRAB"))
          {
            MonoMain.pauseMenu = this._confirmGroup;
            this._confirmGroup.Open();
            this._confirmMenu.Open();
          }
        }
        else if (this._screen == LockerScreen.Trophies && InputProfile.active.Pressed("QUACK"))
        {
          this._desiredScreen = LockerScreen.Locker;
          HUD.CloseAllCorners();
        }
      }
      base.Update();
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer == Layer.Background)
      {
        if (this._screen == LockerScreen.Locker)
        {
          this._background.scale = new Vec2(1f, 1f);
          this._background.depth = new Depth(0.4f);
          this._background.alpha = this._fade;
          Graphics.Draw(this._background, 0.0f, 0.0f);
          string text = this._profile.name;
          Vec2 vec2 = new Vec2(115f, 46f);
          Graphics.DrawString(text, vec2 + new Vec2((float) (-(double) Graphics.GetStringWidth(text) / 2.0), 0.0f), Color.Gray * this._fade, new Depth(0.5f));
          if (this._selection == LockerSelection.Stats)
          {
            this._boardHighlight.depth = new Depth(0.5f);
            this._boardHighlight.alpha = (float) (0.5 + (double) this._pulse.normalized * 0.5) * this._fade;
            this._boardHighlight.xscale = this._boardHighlight.yscale = (float) (1.0 + (double) this._pulse.normalized * 0.100000001490116);
            Graphics.Draw(this._boardHighlight, (float) (75 + this._boardHighlight.w / 2), (float) (60 + this._boardHighlight.h / 2));
            text = "STATISTICS";
          }
          else if (this._selection == LockerSelection.Trophies)
          {
            this._trophiesHighlight.depth = new Depth(0.5f);
            this._trophiesHighlight.alpha = (float) (0.5 + (double) this._pulse.normalized * 0.5) * this._fade;
            this._trophiesHighlight.xscale = this._trophiesHighlight.yscale = (float) (1.0 + (double) this._pulse.normalized * 0.100000001490116);
            Graphics.Draw(this._trophiesHighlight, (float) (161 + this._trophiesHighlight.w / 2), (float) (53 + this._trophiesHighlight.h / 2));
            text = "TROPHIES";
          }
          vec2 = new Vec2(160f, 140f);
          Graphics.DrawString(text, vec2 + new Vec2((float) (-(double) Graphics.GetStringWidth(text) / 2.0), 0.0f), new Color(14, 20, 27) * this._fade, new Depth(0.5f));
        }
        else if (this._screen == LockerScreen.Stats)
        {
          int num = 0;
          foreach (LockerStat stat in this._stats)
          {
            Vec2 vec2 = new Vec2(160f, (float) (18 + num * 10) - this._statScroll * (float) (this._stats.Count * 10 - 150));
            string name = stat.name;
            Graphics.DrawString(name, vec2 + new Vec2((float) (-(double) Graphics.GetStringWidth(name) / 2.0), 0.0f), stat.color * this._fade, new Depth(0.5f));
            ++num;
          }
        }
        else if (this._screen == LockerScreen.Trophies)
        {
          Vec2 vec2 = new Vec2(160f, 84f);
          string text = "NOPE";
          Graphics.DrawString(text, vec2 + new Vec2((float) (-(double) Graphics.GetStringWidth(text) / 2.0), 0.0f), Color.White * this._fade, new Depth(0.5f));
        }
      }
      base.PostDrawLayer(layer);
    }
  }
}
