// Decompiled with JetBrains decompiler
// Type: DuckGame.ConnectionIndicatorElement
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class ConnectionIndicatorElement
  {
    public float indicatorWaitTime;
    public Duck duck;
    public Vec2 position;
    public Dictionary<ConnectionIndicatorType, ConnectionIndicatorDetail> details = new Dictionary<ConnectionIndicatorType, ConnectionIndicatorDetail>()
    {
      {
        ConnectionIndicatorType.Lag,
        new ConnectionIndicatorDetail()
        {
          type = ConnectionIndicatorType.Lag,
          buildupThreshold = 0.0f,
          iconFrame = 0
        }
      },
      {
        ConnectionIndicatorType.Loss,
        new ConnectionIndicatorDetail()
        {
          type = ConnectionIndicatorType.Loss,
          buildupThreshold = 1f,
          maxBuildup = 2f,
          iconFrame = 1
        }
      },
      {
        ConnectionIndicatorType.AFK,
        new ConnectionIndicatorDetail()
        {
          type = ConnectionIndicatorType.AFK,
          buildupThreshold = 0.0f,
          maxBuildup = 1f,
          iconFrame = 3
        }
      },
      {
        ConnectionIndicatorType.Chatting,
        new ConnectionIndicatorDetail()
        {
          type = ConnectionIndicatorType.Chatting,
          buildupThreshold = 0.0f,
          iconFrame = 4
        }
      },
      {
        ConnectionIndicatorType.Failure,
        new ConnectionIndicatorDetail()
        {
          type = ConnectionIndicatorType.Failure,
          buildupThreshold = 0.0f,
          iconFrame = 2
        }
      }
    };

    public int NumActiveDetails()
    {
      int num = 0;
      foreach (KeyValuePair<ConnectionIndicatorType, ConnectionIndicatorDetail> detail in this.details)
      {
        if (detail.Value.visible)
          ++num;
      }
      return num;
    }

    public ConnectionIndicatorDetail GetDetail(ConnectionIndicatorType t)
    {
      if (this.details.ContainsKey(t))
        return this.details[t];
      ConnectionIndicatorDetail connectionIndicatorDetail = new ConnectionIndicatorDetail();
      this.details[t] = connectionIndicatorDetail;
      return connectionIndicatorDetail;
    }
  }
}
