// Decompiled with JetBrains decompiler
// Type: DuckGame.ContextRadio
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ContextRadio : ContextMenu
  {
    private SpriteMap _radioButton;
    private FieldBinding _field;
    private object _index = (object) 0;
    private bool _selected;

    public ContextRadio(
      string text,
      bool selected,
      object index,
      IContextListener owner,
      FieldBinding field = null)
      : base(owner)
    {
      this.itemSize.x = 150f;
      this.itemSize.y = 16f;
      this._text = text;
      this._selected = field == null ? selected : field.value == index;
      this._field = field;
      this._index = index;
      this.depth = new Depth(0.8f);
      this._radioButton = new SpriteMap("Editor/radioButton", 16, 16);
    }

    public override void Selected()
    {
      if (this.greyOut)
        return;
      SFX.Play("highClick", 0.3f);
      if (Level.current is Editor)
      {
        if (this._field == null)
          return;
        this._field.value = this._index;
      }
      else
      {
        if (this._owner == null)
          return;
        this._owner.Selected((ContextMenu) this);
      }
    }

    public override void Update()
    {
      base.Update();
      if (this._field == null)
        return;
      if (this._field.value != null)
        this._selected = this._field.value.Equals(this._index);
      else
        this._selected = false;
    }

    public override void Draw()
    {
      if (this._hover && !this.greyOut)
        Graphics.DrawRect(this.position, this.position + this.itemSize, new Color(70, 70, 70), new Depth(0.83f));
      Color color = Color.White;
      if (this.greyOut)
        color = Color.White * 0.3f;
      Graphics.DrawString(this._text, this.position + new Vec2(4f, 5f), color, new Depth(0.85f));
      this._radioButton.depth = new Depth(0.9f);
      this._radioButton.x = (float) ((double) this.x + (double) this.itemSize.x - 16.0);
      this._radioButton.y = this.y;
      this._radioButton.frame = this._selected ? 1 : 0;
      this._radioButton.color = color;
      this._radioButton.Draw();
    }
  }
}
