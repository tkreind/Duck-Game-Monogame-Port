// Decompiled with JetBrains decompiler
// Type: DuckGame.Spikes
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("stuff|spikes")]
  public class Spikes : MaterialThing, IDontMove
  {
    private SpriteMap _sprite;
    public bool up = true;
    protected ImpactedFrom _killImpact;

    public Spikes(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("spikes", 16, 19);
      this._sprite.speed = 0.1f;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 14f);
      this.collisionOffset = new Vec2(-8f, -5f);
      this.collisionSize = new Vec2(16f, 7f);
      this.depth = (Depth) 0.36f;
      this._editorName = "Spikes Up";
      this.thickness = 100f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.editorOffset = new Vec2(0.0f, 6f);
      this.hugWalls = WallHug.Floor;
      this._editorImageCenter = true;
      this._killImpact = ImpactedFrom.Top;
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      switch (with)
      {
        case TV _:
          break;
        case Hat _:
          break;
        default:
          Duck duck = with as Duck;
          if (this._killImpact == ImpactedFrom.Top && duck != null && (duck.holdObject is Sword && (duck.holdObject as Sword)._slamStance))
            break;
          float num = 1f;
          if (from != this._killImpact)
            break;
          if (from == ImpactedFrom.Left && (double) with.hSpeed > (double) num)
            with.Destroy((DestroyType) new DTImpale((Thing) this));
          if (from == ImpactedFrom.Right && (double) with.hSpeed < -(double) num)
            with.Destroy((DestroyType) new DTImpale((Thing) this));
          if (from == ImpactedFrom.Top && (double) with.vSpeed > (double) num && (duck == null || !duck.HasEquipment(typeof (Boots))))
          {
            bool flag = true;
            if (with is PhysicsObject)
            {
              PhysicsObject physicsObject = with as PhysicsObject;
              Vec2 bottomRight = with.bottomRight;
              Vec2 bottomLeft = with.bottomLeft;
              Vec2 p1_1 = new Vec2(with.x, with.bottom);
              Vec2 p2 = p1_1;
              Vec2 vec2 = physicsObject.lastPosition - physicsObject.position;
              Vec2 p1_2 = bottomRight + vec2;
              Vec2 p1_3 = bottomLeft + vec2;
              p1_1 += vec2;
              flag = false;
              if (Collision.LineIntersect(p1_1, p2, this.topLeft, this.topRight) || Collision.LineIntersect(p1_3, with.bottomLeft, this.topLeft, this.topRight) || Collision.LineIntersect(p1_2, with.bottomRight, this.topLeft, this.topRight))
                flag = true;
            }
            if (flag)
              with.Destroy((DestroyType) new DTImpale((Thing) this));
          }
          if (from != ImpactedFrom.Bottom || (double) with.vSpeed >= -(double) num || duck != null && duck.HasEquipment(typeof (Helmet)))
            break;
          with.Destroy((DestroyType) new DTImpale((Thing) this));
          break;
      }
    }

    public override void Update() => base.Update();

    public override void Draw() => base.Draw();
  }
}
