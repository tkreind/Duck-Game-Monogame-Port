// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckSkeleton
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DuckSkeleton
  {
    private DuckBone _upperTorso = new DuckBone();
    private DuckBone _head = new DuckBone();
    private DuckBone _lowerTorso = new DuckBone();

    public DuckBone upperTorso => this._upperTorso;

    public DuckBone head => this._head;

    public DuckBone lowerTorso => this._lowerTorso;

    public void Draw()
    {
      Graphics.DrawRect(this._upperTorso.position + new Vec2(-1f, -1f), this._upperTorso.position + new Vec2(1f, 1f), Color.LimeGreen * 0.9f, new Depth(0.8f));
      Graphics.DrawLine(this._upperTorso.position, this._upperTorso.position + Maths.AngleToVec(this._upperTorso.orientation) * 4f, Color.Yellow, depth: (new Depth(0.9f)));
      Graphics.DrawRect(this._lowerTorso.position + new Vec2(-1f, -1f), this._lowerTorso.position + new Vec2(1f, 1f), Color.LimeGreen * 0.9f, new Depth(0.8f));
      Graphics.DrawLine(this._lowerTorso.position, this._lowerTorso.position + Maths.AngleToVec(this._lowerTorso.orientation) * 4f, Color.Yellow, depth: (new Depth(0.9f)));
      Graphics.DrawRect(this._head.position + new Vec2(-1f, -1f), this._head.position + new Vec2(1f, 1f), Color.LimeGreen * 0.9f, new Depth(0.8f));
      Graphics.DrawLine(this._head.position, this._head.position + Maths.AngleToVec(this._head.orientation) * 4f, Color.Yellow, depth: (new Depth(0.9f)));
    }
  }
}
