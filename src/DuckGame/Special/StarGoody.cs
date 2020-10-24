// Decompiled with JetBrains decompiler
// Type: DuckGame.StarGoody
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("special|goodies")]
  [BaggedProperty("isOnlineCapable", false)]
  public class StarGoody : Goody
  {
    public EditorProperty<bool> valid;

    public override void EditorPropertyChanged(object property) => this.sequence.isValid = this.valid.value;

    public StarGoody(float xpos, float ypos)
      : base(xpos, ypos, new Sprite("challenge/star"))
      => this.valid = new EditorProperty<bool>(true, (Thing) this);
  }
}
