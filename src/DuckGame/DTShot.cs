// Decompiled with JetBrains decompiler
// Type: DuckGame.DTShot
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DTShot : DestroyType
  {
    private Bullet _bullet;

    public Bullet bullet => this._bullet;

    public Thing bulletOwner => this._bullet == null ? (Thing) null : this._bullet.owner;

    public Thing bulletFiredFrom => this._bullet == null ? (Thing) null : this._bullet.firedFrom;

    public DTShot(Bullet b)
      : base((Thing) b)
      => this._bullet = b;
  }
}
