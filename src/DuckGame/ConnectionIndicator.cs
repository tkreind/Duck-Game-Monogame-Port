// Decompiled with JetBrains decompiler
// Type: DuckGame.ConnectionIndicator
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class ConnectionIndicator
  {
    private static Dictionary<NetworkConnection, ConnectionIndicatorElement> _connections = new Dictionary<NetworkConnection, ConnectionIndicatorElement>();
    private static SpriteMap _lagIcons;
    private static Sprite _rainbowGradient;

    public static void Update()
    {
      foreach (KeyValuePair<NetworkConnection, ConnectionIndicatorElement> connection in ConnectionIndicator._connections)
      {
        if (connection.Value.duck != null && (connection.Value.duck.removeFromLevel || connection.Value.duck.level != Level.current))
          connection.Value.duck = (Duck) null;
        connection.Value.position = connection.Value.duck == null ? new Vec2(-1000f, -1000f) : connection.Value.duck.cameraPosition;
        foreach (KeyValuePair<ConnectionIndicatorType, ConnectionIndicatorDetail> detail in connection.Value.details)
        {
          detail.Value.buildup -= Maths.IncFrameTimer() * 0.25f;
          if ((double) detail.Value.buildup > (double) detail.Value.maxBuildup)
            detail.Value.buildup = detail.Value.maxBuildup;
          if ((double) detail.Value.buildup < 0.0)
          {
            detail.Value.buildup = 0.0f;
            detail.Value.popOut += 0.02f;
            if ((double) detail.Value.popOut > 1.0)
              detail.Value.popOut = 1f;
            detail.Value.grow -= 0.1f;
            if ((double) detail.Value.grow < 0.0)
              detail.Value.grow = 0.0f;
          }
          else
          {
            detail.Value.popOut = 0.0f;
            detail.Value.grow = Lerp.FloatSmooth(detail.Value.grow, 1f, 0.2f);
            if ((double) detail.Value.grow > 1.0)
              detail.Value.grow = 1f;
          }
        }
      }
      foreach (Duck duck in Level.current.things[typeof (Duck)])
      {
        NetworkConnection key = (NetworkConnection) null;
        if (duck.profile != null && duck.profile.connection != null)
          key = duck.profile.connection;
        if (Network.isActive && key != null)
        {
          ConnectionIndicatorElement indicatorElement = (ConnectionIndicatorElement) null;
          if (!ConnectionIndicator._connections.TryGetValue(key, out indicatorElement))
          {
            indicatorElement = new ConnectionIndicatorElement();
            ConnectionIndicator._connections[key] = indicatorElement;
          }
          indicatorElement.duck = duck;
          if ((double) key.manager.ping > 0.25)
            indicatorElement.GetDetail(ConnectionIndicatorType.Lag).buildup += Maths.IncFrameTimer();
          if ((double) key.manager.ping > 0.899999976158142 || key.status != ConnectionStatus.Connected)
            indicatorElement.GetDetail(ConnectionIndicatorType.Failure).buildup += Maths.IncFrameTimer();
          if (key.manager.lossThisFrame)
          {
            indicatorElement.GetDetail(ConnectionIndicatorType.Loss).buildup += 0.2f;
            key.manager.lossThisFrame = false;
          }
          if ((double) key.manager.jitter > 0.800000011920929)
            indicatorElement.GetDetail(ConnectionIndicatorType.Loss).buildup += 0.25f;
          ConnectionIndicatorDetail detail1 = indicatorElement.GetDetail(ConnectionIndicatorType.AFK);
          if (!duck.afk)
            detail1.buildup = 0.0f;
          else
            detail1.buildup += Maths.IncFrameTimer();
          ConnectionIndicatorDetail detail2 = indicatorElement.GetDetail(ConnectionIndicatorType.Chatting);
          if (duck.chatting)
            detail2.buildup += 0.25f;
          else
            detail2.buildup = 0.0f;
        }
      }
      if (ConnectionIndicator._lagIcons != null)
        return;
      ConnectionIndicator._lagIcons = new SpriteMap("lagturtle", 16, 16);
      ConnectionIndicator._lagIcons.CenterOrigin();
      ConnectionIndicator._rainbowGradient = new Sprite("rainbowGradient");
    }

    public static void Draw()
    {
      foreach (KeyValuePair<NetworkConnection, ConnectionIndicatorElement> connection in ConnectionIndicator._connections)
      {
        if (connection.Value.duck != null && !connection.Value.duck.dead)
        {
          int num1 = connection.Value.NumActiveDetails();
          if (num1 > 0)
          {
            float num2 = 27f;
            float num3 = (float) (num1 - 1) * num2;
            Vec2 vec2_1 = new Vec2(-1000f, -1000f);
            vec2_1 = connection.Value.position + new Vec2(0.0f, 6f);
            float num4 = (float) num1 / (float) connection.Value.details.Count;
            int num5 = 0;
            float num6 = -20f;
            bool flag = false;
            Vec2 vec2_2 = Vec2.Zero;
            foreach (KeyValuePair<ConnectionIndicatorType, ConnectionIndicatorDetail> detail in connection.Value.details)
            {
              if (detail.Value.visible)
              {
                float deg = (float) (-(double) num3 / 2.0 + (double) num5 * (double) num2);
                float x = vec2_1.x - (float) Math.Sin((double) Maths.DegToRad(deg)) * num6;
                float y = vec2_1.y + (float) Math.Cos((double) Maths.DegToRad(deg)) * num6;
                ConnectionIndicator._lagIcons.depth = new Depth(0.9f);
                ConnectionIndicator._lagIcons.frame = detail.Value.iconFrame;
                ConnectionIndicator._lagIcons.scale = new Vec2((float) (1.0 + (1.0 - (double) detail.Value.grow) * 0.300000011920929));
                Graphics.Draw((Sprite) ConnectionIndicator._lagIcons, x, y);
                if (flag)
                {
                  Vec2 vec2_3 = new Vec2(x, y);
                  Vec2 normalized = (vec2_3 - vec2_2).normalized;
                  Graphics.DrawTexturedLine(ConnectionIndicator._rainbowGradient.texture, vec2_2 - normalized, vec2_3 + normalized, Color.White * num4, (float) (0.600000023841858 + 0.600000023841858 * (double) num4), new Depth(0.85f));
                }
                flag = true;
                vec2_2 = new Vec2(x, y);
                ++num5;
              }
            }
          }
        }
      }
    }
  }
}
