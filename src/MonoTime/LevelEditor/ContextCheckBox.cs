// Decompiled with JetBrains decompiler
// Type: DuckGame.ContextCheckBox
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections;

namespace DuckGame
{
  public class ContextCheckBox : ContextMenu
  {
    private SpriteMap _checkBox;
    private FieldBinding _field;
    public bool isChecked;
    public string path = "";
    public System.Type _myType;

    public ContextCheckBox(
      string text,
      IContextListener owner,
      FieldBinding field,
      System.Type myType,
      string valTooltip)
      : base(owner)
    {
      this.itemSize.x = 150f;
      this.itemSize.y = 16f;
      this._text = text;
      this._field = field;
      this._checkBox = new SpriteMap("Editor/checkBox", 16, 16);
      this.depth = new Depth(0.8f);
      this._myType = myType;
      if (field == null)
        this._field = new FieldBinding((object) this, nameof (isChecked));
      this.tooltip = valTooltip;
    }

    public ContextCheckBox(string text, IContextListener owner, FieldBinding field = null, System.Type myType = null)
      : base(owner)
    {
      this.itemSize.x = 150f;
      this.itemSize.y = 16f;
      this._text = text;
      this._field = field;
      this._checkBox = new SpriteMap("Editor/checkBox", 16, 16);
      this.depth = new Depth(0.8f);
      this._myType = myType;
      if (field != null)
        return;
      this._field = new FieldBinding((object) this, nameof (isChecked));
    }

    public override void Selected()
    {
      SFX.Play("highClick", 0.3f);
      if (Level.current is Editor)
      {
        if (this._field == null)
          return;
        if (this._field.value is IList)
        {
          IList list = this._field.value as IList;
          if (list.Contains((object) this._myType))
            list.Remove((object) this._myType);
          else
            list.Add((object) this._myType);
        }
        else
          this._field.value = (object) !(bool) this._field.value;
      }
      else
      {
        if (this._owner == null)
          return;
        this._owner.Selected((ContextMenu) this);
      }
    }

    public override void Update() => base.Update();

    public override void Draw()
    {
      if (this._hover)
        Graphics.DrawRect(this.position, this.position + this.itemSize, new Color(70, 70, 70), new Depth(0.82f));
      Graphics.DrawString(this._text, this.position + new Vec2(2f, 5f), Color.White, new Depth(0.85f));
      bool flag = !(this._field.value is IList) ? (bool) this._field.value : (this._field.value as IList).Contains((object) this._myType);
      this._checkBox.depth = new Depth(0.9f);
      this._checkBox.x = (float) ((double) this.x + (double) this.itemSize.x - 16.0);
      this._checkBox.y = this.y;
      this._checkBox.frame = flag ? 1 : 0;
      this._checkBox.Draw();
    }
  }
}
