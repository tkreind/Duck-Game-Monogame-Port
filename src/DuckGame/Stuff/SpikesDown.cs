// Decompiled with JetBrains decompiler
// Type: DuckGame.SpikesDown
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("stuff|spikes")]
  public class SpikesDown : Spikes
  {
    private SpriteMap _sprite;

    public SpikesDown(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("spikes", 16, 19);
      this._sprite.speed = 0.1f;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 14f);
      this.collisionOffset = new Vec2(-8f, -2f);
      this.collisionSize = new Vec2(16f, 6f);
      this._editorName = "Spikes Down";
      this.thickness = 100f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.angle = 3.141593f;
      this.up = false;
      this.editorOffset = new Vec2(0.0f, -6f);
      this.hugWalls = WallHug.Ceiling;
      this._killImpact = ImpactedFrom.Bottom;
    }

    public override void Update() => base.Update();

    public override void Draw() => base.Draw();
  }
}
