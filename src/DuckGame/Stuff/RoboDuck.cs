// Decompiled with JetBrains decompiler
// Type: DuckGame.RoboDuck
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [BaggedProperty("canSpawn", false)]
  [EditorGroup("survival")]
  public class RoboDuck : PhysicsObject
  {
    private SpriteMap _sprite;
    public static float _waitDif;
    private float wait = 1f;

    public RoboDuck(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("survival/robot", 32, 32);
      this._sprite.AddAnimation("idle", 0.4f, true, 0, 1, 2, 3, 4, 5, 6, 7);
      this._sprite.AddAnimation("walk", 0.2f, true, 8, 9, 10, 11, 12, 13, 14, 15);
      this._sprite.SetAnimation("walk");
      this.graphic = (Sprite) this._sprite;
      this._collisionSize = new Vec2(8f, 22f);
      this._collisionOffset = new Vec2(-4f, -7f);
      this.center = new Vec2(16f, 16f);
      this.wait = 0.1f + RoboDuck._waitDif;
      RoboDuck._waitDif += 0.1f;
      this._visibleInGame = false;
    }

    public override void Update()
    {
      this.wait -= 0.004f;
      if ((double) this.wait >= 0.0)
        return;
      this.wait = 1f;
      Duck duck = new Duck(this.x, this.y, Profiles.DefaultPlayer1)
      {
        ai = new DuckAI()
      };
      duck.mindControl = (InputProfile) duck.ai;
      duck.derpMindControl = false;
      (Level.current.camera as FollowCam).Add((Thing) duck);
      Level.Add((Thing) duck);
    }
  }
}
