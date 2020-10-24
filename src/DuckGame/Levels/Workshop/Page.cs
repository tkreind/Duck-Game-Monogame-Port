// Decompiled with JetBrains decompiler
// Type: DuckGame.Page
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Page : Level
  {
    protected CategoryState _state;
    public static float camOffset;

    public virtual void DeactivateAll()
    {
    }

    public virtual void ActivateAll()
    {
    }

    public virtual void TransitionOutComplete()
    {
    }

    public override void Update()
    {
      Layer.HUD.camera.x = Page.camOffset;
      if (this._state == CategoryState.OpenPage)
      {
        this.DeactivateAll();
        Page.camOffset = Lerp.FloatSmooth(Page.camOffset, 360f, 0.1f);
        if ((double) Page.camOffset <= 330.0)
          return;
        this.TransitionOutComplete();
      }
      else
      {
        if (this._state != CategoryState.Idle)
          return;
        Page.camOffset = Lerp.FloatSmooth(Page.camOffset, -40f, 0.1f);
        if ((double) Page.camOffset < 0.0)
          Page.camOffset = 0.0f;
        if ((double) Page.camOffset == 0.0)
          this.ActivateAll();
        else
          this.DeactivateAll();
      }
    }
  }
}
