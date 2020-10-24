// Decompiled with JetBrains decompiler
// Type: DuckGame.SpikesRight
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("stuff|spikes")]
  public class SpikesRight : Spikes
  {
    private SpriteMap _sprite;

    public SpikesRight(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("spikes", 16, 19);
      this._sprite.speed = 0.1f;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 14f);
      this.collisionOffset = new Vec2(-2f, -8f);
      this.collisionSize = new Vec2(6f, 16f);
      this._editorName = "Spikes Right";
      this.thickness = 100f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.angle = 1.570796f;
      this.up = false;
      this.editorOffset = new Vec2(-6f, 0.0f);
      this.hugWalls = WallHug.Left;
      this._killImpact = ImpactedFrom.Right;
    }

    public override void Update() => base.Update();

    public override void Draw() => base.Draw();
  }
}
