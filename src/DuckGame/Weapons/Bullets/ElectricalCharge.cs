// Decompiled with JetBrains decompiler
// Type: DuckGame.ElectricalCharge
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class ElectricalCharge : Thing
  {
    private List<Vec2> _prevPositions = new List<Vec2>();
    private Vec2 _travelVec;

    public ElectricalCharge(float xpos, float ypos, int off, Thing own)
      : base(xpos, ypos)
    {
      this.offDir = (sbyte) off;
      this._travelVec = new Vec2((float) this.offDir * Rando.Float(6f, 10f), Rando.Float(-10f, 10f));
      this.owner = own;
    }

    public override void Update()
    {
      if (this._prevPositions.Count == 0)
        this._prevPositions.Insert(0, this.position);
      Vec2 position = this.position;
      ElectricalCharge electricalCharge = this;
      electricalCharge.position = electricalCharge.position + this._travelVec;
      this._travelVec = new Vec2((float) this.offDir * Rando.Float(6f, 10f), Rando.Float(-10f, 10f));
      this._prevPositions.Insert(0, this.position);
      this.alpha -= 0.1f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      foreach (IAmADuck amAduck in Level.CheckLineAll<IAmADuck>(position, this.position))
      {
        if (amAduck is MaterialThing materialThing && amAduck != this.owner.owner)
          materialThing.Zap(this.owner);
      }
      base.Update();
    }

    public override void Draw()
    {
      Vec2 p2 = Vec2.Zero;
      bool flag = false;
      float num = 1f;
      foreach (Vec2 prevPosition in this._prevPositions)
      {
        if (!flag)
        {
          flag = true;
          p2 = prevPosition;
        }
        else
        {
          Graphics.DrawLine(prevPosition, p2, Colors.DGYellow * num, depth: (new Depth(0.9f)));
          num -= 0.25f;
        }
        if ((double) num <= 0.0)
          break;
      }
      base.Draw();
    }
  }
}
