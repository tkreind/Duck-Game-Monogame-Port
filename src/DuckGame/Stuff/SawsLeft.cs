// Decompiled with JetBrains decompiler
// Type: DuckGame.SawsLeft
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|spikes")]
  public class SawsLeft : Saws
  {
    private SpriteMap _sprite;
    public new bool up = true;

    public SawsLeft(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("movingSpikes", 16, 21);
      this._sprite.speed = 0.3f;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 14f);
      this.collisionOffset = new Vec2(-2f, -6f);
      this.collisionSize = new Vec2(4f, 12f);
      this.depth = (Depth) 0.3f;
      this._editorName = "Saws Left";
      this.thickness = 3f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.angle = -1.570796f;
      this.editorOffset = new Vec2(6f, 0.0f);
      this.hugWalls = WallHug.Right;
      this._editorImageCenter = true;
      this.impactThreshold = 0.01f;
    }

    public override void Touch(MaterialThing with)
    {
      if (with.destroyed)
        return;
      with.Destroy((DestroyType) new DTImpale((Thing) this));
      with.hSpeed = -3f;
    }

    public override void Update() => base.Update();

    public override void Draw() => base.Draw();
  }
}
