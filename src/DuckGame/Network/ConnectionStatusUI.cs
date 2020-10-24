// Decompiled with JetBrains decompiler
// Type: DuckGame.ConnectionStatusUI
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Linq;

namespace DuckGame
{
  public class ConnectionStatusUI
  {
    private static ConnectionStatusUICore _core = new ConnectionStatusUICore();
    private static BitmapFont _smallBios;
    private static Sprite _bar;

    public static ConnectionStatusUICore core
    {
      get => ConnectionStatusUI._core;
      set => ConnectionStatusUI._core = value;
    }

    public static void Initialize()
    {
      ConnectionStatusUI._smallBios = new BitmapFont("smallBiosFont", 7, 6);
      ConnectionStatusUI._bar = new Sprite("statusBar");
    }

    public static void Show()
    {
      ConnectionStatusUI._core.bars.Clear();
      foreach (Profile profile in Profiles.active)
        ConnectionStatusUI._core.bars.Add(new ConnectionStatusBar()
        {
          profile = profile
        });
      ConnectionStatusUI._core.open = true;
    }

    public static void Hide() => ConnectionStatusUI._core.open = false;

    public static void Update()
    {
      if (ConnectionStatusUI._core.open)
      {
        ConnectionStatusBar connectionStatusBar = (ConnectionStatusBar) null;
        foreach (ConnectionStatusBar bar in ConnectionStatusUI._core.bars)
        {
          if (connectionStatusBar == null || (double) connectionStatusBar.position > 0.300000011920929)
            bar.position = Lerp.FloatSmooth(bar.position, 1f, 0.16f, 1.1f);
          connectionStatusBar = bar;
        }
      }
      else
      {
        ConnectionStatusBar connectionStatusBar = (ConnectionStatusBar) null;
        foreach (ConnectionStatusBar bar in ConnectionStatusUI._core.bars)
        {
          if (connectionStatusBar == null || (double) connectionStatusBar.position < 0.699999988079071)
            bar.position = Lerp.FloatSmooth(bar.position, 0.0f, 0.08f, 1.1f);
          connectionStatusBar = bar;
        }
      }
    }

    public static void Draw()
    {
      Vec2 vec2_1 = new Vec2(30f, (float) ((double) Layer.HUD.height / 2.0 - (double) ConnectionStatusUI._core.bars.Count * 14.0 / 2.0));
      int num1 = 0;
      foreach (ConnectionStatusBar bar in ConnectionStatusUI._core.bars)
      {
        if (bar.profile.connection != null && bar.profile.connection.status != ConnectionStatus.Disconnected)
        {
          if ((double) bar.position > 0.00999999977648258)
          {
            Vec2 vec2_2 = new Vec2(vec2_1.x, vec2_1.y + (float) (num1 * 14));
            vec2_2.x -= Layer.HUD.width * (1f - bar.position);
            ConnectionStatusUI._bar.depth = (Depth) 0.8f;
            Graphics.Draw(ConnectionStatusUI._bar, vec2_2.x, vec2_2.y);
            ConnectionStatusUI._smallBios.depth = (Depth) 0.9f;
            int num2 = 0;
            bool flag = false;
            int transferProgress;
            if (bar.profile.connection == DuckNetwork.localConnection)
            {
              transferProgress = DuckNetwork.core.levelTransferProgress;
              num2 = DuckNetwork.core.levelTransferSize;
              flag = true;
            }
            else
            {
              transferProgress = bar.profile.connection.dataTransferProgress;
              num2 = bar.profile.connection.dataTransferSize;
            }
            if (transferProgress != num2)
            {
              ConnectionStatusUI._smallBios.scale = new Vec2(0.5f, 0.5f);
              if (flag)
                ConnectionStatusUI._smallBios.Draw("@ONLINENEUTRAL@|DGYELLOW|DOWNLOADING   " + transferProgress.ToString() + "\\" + num2.ToString() + "B", new Vec2(vec2_2.x + 3f, vec2_2.y + 3f), Color.White, (Depth) 0.9f);
              else
                ConnectionStatusUI._smallBios.Draw("@ONLINENEUTRAL@|DGYELLOW|SENDING CUSTOM " + transferProgress.ToString() + "\\" + num2.ToString() + "B", new Vec2(vec2_2.x + 3f, vec2_2.y + 3f), Color.White, (Depth) 0.9f);
              float num3 = (float) transferProgress / (float) num2;
              int num4 = 3;
              int num5 = 11;
              int num6 = 7;
              int num7 = 90;
              Graphics.DrawRect(vec2_2 + new Vec2((float) num5, (float) num6), vec2_2 + new Vec2((float) (num5 + num7), (float) (num6 + num4)), Color.White, (Depth) 0.9f, false, 0.5f);
              Graphics.DrawRect(vec2_2 + new Vec2((float) num5, (float) num6), vec2_2 + new Vec2((float) num5 + (float) num7 * num3, (float) (num6 + num4)), Colors.DGGreen, (Depth) 0.87f);
              Graphics.DrawRect(vec2_2 + new Vec2((float) num5, (float) num6), vec2_2 + new Vec2((float) (num5 + num7), (float) (num6 + num4)), Colors.DGRed, (Depth) 0.84f);
            }
            else if ((int) bar.profile.connection.loadingStatus != (int) DuckNetwork.levelIndex)
              ConnectionStatusUI._smallBios.Draw("@ONLINENEUTRAL@|DGYELLOW|SENDING...", new Vec2(vec2_2.x + 3f, vec2_2.y + 3f), Color.White, (Depth) 0.9f);
            else
              ConnectionStatusUI._smallBios.Draw("@ONLINEGOOD@|DGGREEN|READY!", new Vec2(vec2_2.x + 3f, vec2_2.y + 3f), Color.White, (Depth) 0.9f);
            ConnectionStatusUI._smallBios.scale = new Vec2(1f, 1f);
            string str1 = bar.profile.name;
            if (str1.Length > 11)
              str1 = str1.Substring(0, 11) + ".";
            string str2 = "|WHITE|";
            if (bar.profile.networkIndex == (byte) 1)
              str2 = "|LIGHTGRAY|";
            else if (bar.profile.networkIndex == (byte) 2)
              str2 = "|DGYELLOW|";
            else if (bar.profile.networkIndex == (byte) 3)
              str2 = "|MENUORANGE|";
            string text1 = str2 + str1;
            ConnectionStatusUI._smallBios.Draw(text1, new Vec2((float) ((double) vec2_2.x + (double) ConnectionStatusUI._bar.width - 3.0 - (double) ConnectionStatusUI._smallBios.GetWidth(text1) - 60.0), vec2_2.y + 3f), Color.White, (Depth) 0.9f);
            int num8 = (int) Math.Round((double) bar.profile.connection.manager.ping * 1000.0);
            string source = num8.ToString() + "|WHITE|MS";
            source.Count<char>();
            string text2 = num8 >= 150 ? (num8 >= 250 ? (bar.profile.connection.status != ConnectionStatus.Connected ? "|DGRED|" + source + "@SIGNALDEAD@" : "|DGRED|" + source + "@SIGNALBAD@") : "|DGYELLOW|" + source + "@SIGNALNORMAL@") : "|DGGREEN|" + source + "@SIGNALGOOD@";
            ConnectionStatusUI._smallBios.Draw(text2, new Vec2((float) ((double) vec2_2.x + (double) ConnectionStatusUI._bar.width - 3.0) - ConnectionStatusUI._smallBios.GetWidth(text2), vec2_2.y + 3f), Color.White, (Depth) 0.9f);
          }
          ++num1;
        }
      }
    }
  }
}
