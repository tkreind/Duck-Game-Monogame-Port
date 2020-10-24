// Decompiled with JetBrains decompiler
// Type: DuckGame.Anchor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Anchor
  {
    private Thing _thing;
    public Vec2 offset = Vec2.Zero;

    public Thing thing => this._thing;

    public Vec2 position => this._thing.anchorPosition + this.offset;

    public Anchor(Thing to) => this._thing = to;

    public static implicit operator Anchor(Thing val) => new Anchor(val);

    public static implicit operator Thing(Anchor val) => val._thing;

    public static bool operator ==(Anchor c1, Thing c2)
    {
      if ((object) c1 != null)
        return c1._thing == c2;
      return c2 == null;
    }

    public static bool operator !=(Anchor c1, Thing c2)
    {
      if ((object) c1 != null)
        return c1._thing != c2;
      return c2 != null;
    }

    public bool Equals(Thing p) => p == this._thing;

    public override bool Equals(object obj) => obj is Thing ? this.Equals(obj as Thing) : base.Equals(obj);

    public override int GetHashCode() => this._thing.GetHashCode();
  }
}
