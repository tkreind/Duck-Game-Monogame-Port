// Decompiled with JetBrains decompiler
// Type: DuckGame.ItemBoxOneTime
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("spawns")]
  public class ItemBoxOneTime : ItemBox
  {
    public ItemBoxOneTime(float xpos, float ypos)
      : base(xpos, ypos)
    {
    }

    public override void UpdateCharging() => this.charging = 500;

    public override void Draw()
    {
      this._sprite.frame += 4;
      base.Draw();
      this._sprite.frame -= 4;
    }
  }
}
