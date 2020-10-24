// Decompiled with JetBrains decompiler
// Type: DuckGame.ItemBoxRandom
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("spawns")]
  public class ItemBoxRandom : ItemBox
  {
    public ItemBoxRandom(float xpos, float ypos)
      : base(xpos, ypos)
    {
    }

    public override void Update() => base.Update();

    public override void Draw()
    {
      this._sprite.frame += 2;
      base.Draw();
      this._sprite.frame -= 2;
    }

    public override PhysicsObject GetSpawnItem()
    {
      List<System.Type> physicsObjects = ItemBox.GetPhysicsObjects(Editor.Placeables);
      physicsObjects.RemoveAll((Predicate<System.Type>) (t => t == typeof (LavaBarrel) || t == typeof (Grapple)));
      this.contains = physicsObjects[NetRand.Int(physicsObjects.Count - 1)];
      PhysicsObject thing = Editor.CreateThing(this.contains) as PhysicsObject;
      if (Rando.Int(1000) == 1 && thing is Gun && (thing as Gun).CanSpawnInfinite())
      {
        (thing as Gun).infiniteAmmoVal = true;
        (thing as Gun).infinite.value = true;
      }
      return thing;
    }

    public override void DrawHoverInfo()
    {
    }
  }
}
