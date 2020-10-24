// Decompiled with JetBrains decompiler
// Type: DuckGame.VirtualShotgun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|shotguns")]
  [BaggedProperty("isSuperWeapon", true)]
  public class VirtualShotgun : Shotgun
  {
    public StateBinding _roomIndexBinding = new StateBinding(nameof (roomIndex), 2);
    private byte _roomIndex;

    public byte roomIndex
    {
      get => this._roomIndex;
      set
      {
        this._roomIndex = value;
        if (!Network.isClient || !(Level.current is TeamSelect2) || this._roomIndex >= (byte) 4)
          return;
        (Level.current as TeamSelect2).GetBox(this._roomIndex).gun = this;
      }
    }

    public VirtualShotgun(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 99;
      this.graphic = new Sprite("virtualShotgun");
      this._loaderSprite = new SpriteMap("virtualShotgunLoader", 8, 8);
      this._loaderSprite.center = new Vec2(4f, 4f);
    }

    public override void Update()
    {
      this.ammo = 99;
      base.Update();
    }
  }
}
