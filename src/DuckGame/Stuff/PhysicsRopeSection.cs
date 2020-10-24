// Decompiled with JetBrains decompiler
// Type: DuckGame.PhysicsRopeSection
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class PhysicsRopeSection : PhysicsObject
  {
    public Vec2 tempPos;
    public Vec2 calcPos;
    public Vec2 velocity;
    public Vec2 accel;
    public PhysicsRope rope;

    public PhysicsRopeSection(float xpos, float ypos, PhysicsRope r)
      : base(xpos, ypos)
    {
      this.tempPos = this.position;
      this.collisionSize = new Vec2(4f, 4f);
      this.collisionOffset = new Vec2(-2f, -2f);
      this.weight = 0.1f;
      this.updatePhysics = false;
      this.rope = r;
    }
  }
}
