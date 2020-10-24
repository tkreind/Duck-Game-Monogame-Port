// Decompiled with JetBrains decompiler
// Type: DuckGame.InvisiGoody
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("special|goodies")]
  public class InvisiGoody : Goody
  {
    public EditorProperty<bool> valid;
    public EditorProperty<bool> sound;
    public EditorProperty<int> size;

    public override void EditorPropertyChanged(object property)
    {
      this.UpdateHeight();
      this.sequence.isValid = this.valid.value;
      if (this.sound.value)
        this.collectSound = "goody";
      else
        this.collectSound = "";
    }

    public void UpdateHeight()
    {
      float num = (float) this.size.value;
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(num * 16f);
      this.collisionOffset = new Vec2((float) (-((double) num * 16.0) / 2.0));
      this.scale = new Vec2(num);
    }

    public InvisiGoody(float xpos, float ypos)
      : base(xpos, ypos, new Sprite("swirl"))
    {
      this._visibleInGame = false;
      this.sequence.isValid = false;
      this.size = new EditorProperty<int>(1, (Thing) this, 1f, 16f, 1f);
      this.valid = new EditorProperty<bool>(false, (Thing) this);
      this.sound = new EditorProperty<bool>(false, (Thing) this);
    }
  }
}
