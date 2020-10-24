// Decompiled with JetBrains decompiler
// Type: DuckGame.GameContext
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class GameContext
  {
    public LayerCore layerCore;
    private LayerCore _oldLayerCore;
    public LevelCore levelCore;
    private LevelCore _oldLevelCore;

    public GameContext()
    {
      this.layerCore = new LayerCore();
      this.layerCore.InitializeLayers();
      this.levelCore = new LevelCore();
    }

    public void ApplyStates()
    {
      this._oldLayerCore = Layer.core;
      Layer.core = this.layerCore;
      this._oldLevelCore = Level.core;
      Level.core = this.levelCore;
    }

    public void RevertStates()
    {
      Layer.core = this._oldLayerCore;
      Level.core = this._oldLevelCore;
    }

    public void Update()
    {
      this.ApplyStates();
      Level.UpdateLevelChange();
      Level.UpdateCurrentLevel();
      this.RevertStates();
    }

    public void Draw(RenderTarget2D target = null, Camera c = null, Vec2 offset = default (Vec2))
    {
      this.ApplyStates();
      c.position += offset;
      if (c != null)
        Level.current.camera = c;
      RenderTarget2D t = (RenderTarget2D) null;
      if (target != null)
      {
        t = Graphics.GetRenderTarget();
        Graphics.SetRenderTarget(target);
      }
      Level.DrawCurrentLevel();
      if (target != null)
        Graphics.SetRenderTarget(t);
      this.RevertStates();
      c.position -= offset;
    }
  }
}
