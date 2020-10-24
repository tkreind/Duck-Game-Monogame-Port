﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Feather
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Feather : Thing
  {
    private static int kMaxObjects = 64;
    private static Feather[] _objects = new Feather[Feather.kMaxObjects];
    private static int _lastActiveObject = 0;
    private SpriteMap _sprite;
    private bool _rested;

    public static Feather New(float xpos, float ypos, DuckPersona who)
    {
      if (who == null)
        who = Persona.Duck1;
      Feather feather;
      if (Feather._objects[Feather._lastActiveObject] == null)
      {
        feather = new Feather();
        Feather._objects[Feather._lastActiveObject] = feather;
      }
      else
        feather = Feather._objects[Feather._lastActiveObject];
      Level.Remove((Thing) feather);
      Feather._lastActiveObject = (Feather._lastActiveObject + 1) % Feather.kMaxObjects;
      feather.Init(xpos, ypos, who);
      feather.ResetProperties();
      feather._sprite.globalIndex = (int) Thing.GetGlobalIndex();
      feather.globalIndex = Thing.GetGlobalIndex();
      return feather;
    }

    private Feather()
      : base()
    {
      this._sprite = new SpriteMap("feather", 12, 4);
      this._sprite.speed = 0.3f;
      this._sprite.AddAnimation("feather", 1f, true, 0, 1, 2, 3);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(6f, 1f);
    }

    private void Init(float xpos, float ypos, DuckPersona who)
    {
      this.position.x = xpos;
      this.position.y = ypos;
      this.alpha = 1f;
      this.hSpeed = Rando.Float(6f) - 3f;
      this.vSpeed = (float) ((double) Rando.Float(2f) - 1.0 - 1.0);
      this._sprite = who.featherSprite.CloneMap();
      this._sprite.SetAnimation("feather");
      this._sprite.frame = Rando.Int(3);
      if (Rando.Double() > 0.5)
        this._sprite.flipH = true;
      else
        this._sprite.flipH = false;
      this.graphic = (Sprite) this._sprite;
      this._rested = false;
    }

    public override void Update()
    {
      if (this._rested)
        return;
      if ((double) this.hSpeed > 0.0)
        this.hSpeed -= 0.1f;
      if ((double) this.hSpeed < 0.0)
        this.hSpeed += 0.1f;
      if ((double) this.hSpeed < 0.1 && (double) this.hSpeed > -0.100000001490116)
        this.hSpeed = 0.0f;
      if ((double) this.vSpeed < 1.0)
        this.vSpeed += 0.06f;
      if ((double) this.vSpeed < 0.0)
      {
        this._sprite.speed = 0.0f;
        if ((Thing) Level.CheckPoint<Block>(this.x, this.y - 7f) != null)
          this.vSpeed = 0.0f;
      }
      else if (Level.CheckPoint<IPlatform>(this.x, this.y + 3f) is Thing thing)
      {
        this.vSpeed = 0.0f;
        this._sprite.speed = 0.0f;
        if (thing is Block)
          this._rested = true;
      }
      else
        this._sprite.speed = 0.3f;
      this.x += this.hSpeed;
      this.y += this.vSpeed;
    }
  }
}
