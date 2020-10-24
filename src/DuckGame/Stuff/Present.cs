// Decompiled with JetBrains decompiler
// Type: DuckGame.Present
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class Present : Holdable, IPlatform
  {
    private SpriteMap _sprite;
    private System.Type _contains;

    public Present(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("presents", 16, 16);
      this._sprite.frame = Rando.Int(0, 7);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-7f, -4f);
      this.collisionSize = new Vec2(14f, 11f);
      this.depth = (Depth) -0.5f;
      this.thickness = 0.0f;
      this.weight = 3f;
      this.collideSounds.Add("presentLand");
    }

    public override void Initialize()
    {
      List<System.Type> physicsObjects = ItemBox.GetPhysicsObjects(Editor.Placeables);
      physicsObjects.RemoveAll((Predicate<System.Type>) (t => t == typeof (Present) || t == typeof (LavaBarrel) || t == typeof (Grapple)));
      this._contains = physicsObjects[Rando.Int(physicsObjects.Count - 1)];
    }

    public override void OnPressAction()
    {
      if (this.owner == null)
        return;
      Thing owner = this.owner;
      Duck duck = this.duck;
      if (duck != null)
      {
        ++duck.profile.stats.presentsOpened;
        this.duck.ThrowItem();
      }
      Level.Remove((Thing) this);
      Level.Add((Thing) new OpenPresent(this.x, this.y, this._sprite.frame));
      for (int index = 0; index < 4; ++index)
        Level.Add((Thing) SmallSmoke.New(this.x + Rando.Float(-2f, 2f), this.y + Rando.Float(-2f, 2f)));
      SFX.Play("harp", 0.8f);
      if (this._contains == (System.Type) null)
        this.Initialize();
      if (!(Editor.CreateThing(this._contains) is Holdable thing))
        return;
      if (Rando.Int(500) == 1 && thing is Gun && (thing as Gun).CanSpawnInfinite())
      {
        (thing as Gun).infiniteAmmoVal = true;
        (thing as Gun).infinite.value = true;
      }
      thing.x = owner.x;
      thing.y = owner.y;
      Level.Add((Thing) thing);
      if (duck == null)
        return;
      duck.GiveHoldable(thing);
      duck.resetAction = true;
    }
  }
}
