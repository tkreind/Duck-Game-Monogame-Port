// Decompiled with JetBrains decompiler
// Type: DuckGame.InputCode
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class InputCode
  {
    private Dictionary<InputProfile, InputCode.InputCodeProfileStatus> status = new Dictionary<InputProfile, InputCode.InputCodeProfileStatus>();
    public List<string> triggers = new List<string>();
    public float breakSpeed = 0.04f;

    public bool Update(InputProfile p)
    {
      InputCode.InputCodeProfileStatus codeProfileStatus = (InputCode.InputCodeProfileStatus) null;
      if (!this.status.TryGetValue(p, out codeProfileStatus))
      {
        codeProfileStatus = new InputCode.InputCodeProfileStatus();
        this.status[p] = codeProfileStatus;
      }
      if (codeProfileStatus.lastUpdateFrame == Graphics.frame)
        return codeProfileStatus.lastResult;
      codeProfileStatus.lastUpdateFrame = Graphics.frame;
      codeProfileStatus.breakTimer -= this.breakSpeed;
      if ((double) codeProfileStatus.breakTimer <= 0.0)
        codeProfileStatus.Break();
      string trigger = this.triggers[codeProfileStatus.currentIndex];
      int num = 1 << Network.synchronizedTriggers.Count - Network.synchronizedTriggers.IndexOf(trigger);
      if ((int) p.state == num)
      {
        if (!codeProfileStatus.release)
        {
          if (codeProfileStatus.currentIndex == this.triggers.Count - 1)
          {
            codeProfileStatus.Break();
            codeProfileStatus.lastResult = true;
            return true;
          }
          codeProfileStatus.release = true;
          if (codeProfileStatus.currentIndex == 0)
            codeProfileStatus.breakTimer = 1f;
        }
      }
      else if (p.state == (ushort) 0)
      {
        if (codeProfileStatus.release)
          codeProfileStatus.Progress();
      }
      else
        codeProfileStatus.Break();
      codeProfileStatus.lastResult = false;
      return false;
    }

    private class InputCodeProfileStatus
    {
      public long lastUpdateFrame;
      public bool lastResult;
      public int currentIndex;
      public bool release;
      public float breakTimer = 1f;

      public void Break()
      {
        this.currentIndex = 0;
        this.release = false;
        this.breakTimer = 1f;
      }

      public void Progress()
      {
        ++this.currentIndex;
        this.release = false;
        this.breakTimer = 1f;
      }
    }
  }
}
