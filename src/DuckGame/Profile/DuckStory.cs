// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckStory
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DuckStory
  {
    public string text = "";
    public NewsSection section;

    public event DuckStory.OnStoryBeginDelegate OnStoryBegin;

    public void DoCallback()
    {
      if (this.OnStoryBegin == null)
        return;
      this.OnStoryBegin(this);
    }

    public delegate void OnStoryBeginDelegate(DuckStory story);
  }
}
