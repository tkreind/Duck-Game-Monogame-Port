// Decompiled with JetBrains decompiler
// Type: DuckGame.PortalDrawTransformer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class PortalDrawTransformer : Thing
  {
    private Portal _portal;
    private Thing _thing;

    public new Portal portal => this._portal;

    public Thing thing => this._thing;

    public PortalDrawTransformer(Thing t, Portal p)
      : base()
    {
      this._portal = p;
      this._thing = t;
    }

    public override void Draw()
    {
      Vec2 position = this._thing.position;
      foreach (PortalDoor door in this._portal.GetDoors())
      {
        if (Graphics.currentLayer == door.layer)
        {
          if (door.isLeft && (double) this._thing.x > (double) door.center.x + 32.0)
            this._thing.position = this._thing.position + (door.center - this._portal.GetOtherDoor(door).center);
          else if (!door.isLeft && (double) this._thing.x < (double) door.center.x - 32.0)
            this._thing.position = this._thing.position + (this._portal.GetOtherDoor(door).center - door.center);
          this._thing.DoDraw();
          this._thing.position = position;
        }
      }
    }
  }
}
