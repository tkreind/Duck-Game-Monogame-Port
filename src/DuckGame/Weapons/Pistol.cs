// Decompiled with JetBrains decompiler
// Type: DuckGame.Pistol
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("guns")]
  public class Pistol : Gun
  {
    private SpriteMap _sprite;

    public Pistol(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 9;
      this._ammoType = (AmmoType) new AT9mm();
      this._type = "gun";
      this._sprite = new SpriteMap("pistol", 18, 10);
      this._sprite.AddAnimation("idle", 1f, true, new int[1]);
      this._sprite.AddAnimation("fire", 0.8f, false, 1, 2, 2, 3, 3);
      this._sprite.AddAnimation("empty", 1f, true, 2);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(10f, 3f);
      this.collisionOffset = new Vec2(-8f, -3f);
      this.collisionSize = new Vec2(16f, 9f);
      this._barrelOffsetTL = new Vec2(18f, 2f);
      this._fireSound = "pistolFire";
      this._kickForce = 3f;
      this._holdOffset = new Vec2(-1f, 0.0f);
      this.loseAccuracy = 0.1f;
      this.maxAccuracyLost = 0.6f;
      this._bio = "Old faithful, the 9MM pistol.";
      this._editorName = nameof (Pistol);
      this.physicsMaterial = PhysicsMaterial.Metal;
    }

    public override void Update()
    {
      if (this._sprite.currentAnimation == "fire" && this._sprite.finished)
        this._sprite.SetAnimation("idle");
      base.Update();
    }

    public override void OnPressAction()
    {
      if (this.ammo > 0)
      {
        this._sprite.SetAnimation("fire");
        for (int index = 0; index < 3; ++index)
        {
          Vec2 vec2 = this.Offset(new Vec2(-9f, 0.0f));
          Vec2 hitAngle = this.barrelVector.Rotate(Rando.Float(1f), Vec2.Zero);
          Level.Add((Thing) Spark.New(vec2.x, vec2.y, hitAngle, 0.1f));
        }
      }
      else
        this._sprite.SetAnimation("empty");
      this.Fire();
    }
  }
}
