// Decompiled with JetBrains decompiler
// Type: DuckGame.Saws
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|spikes")]
  public class Saws : MaterialThing, IDontMove
  {
    private SpriteMap _sprite;
    public bool up = true;

    public Saws(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("movingSpikes", 16, 21);
      this._sprite.speed = 0.3f;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 14f);
      this.collisionOffset = new Vec2(-6f, -2f);
      this.collisionSize = new Vec2(12f, 4f);
      this.depth = new Depth(0.3f);
      this._editorName = "Saws Up";
      this.thickness = 3f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.editorOffset = new Vec2(0.0f, 6f);
      this.hugWalls = WallHug.Floor;
      this._editorImageCenter = true;
      this.impactThreshold = 0.01f;
    }

    public override void Touch(MaterialThing with)
    {
      if (with is Duck duck && duck.holdObject is Sword && (duck.holdObject as Sword)._slamStance || with.destroyed)
        return;
      with.Destroy((DestroyType) new DTImpale((Thing) this));
      with.vSpeed = -3f;
    }

    public override void Update() => base.Update();

    public override void Draw() => base.Draw();
  }
}
