// Decompiled with JetBrains decompiler
// Type: DuckGame.KeyImage
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class KeyImage : Sprite
  {
    private FancyBitmapFont _font;
    private Sprite _keySprite;
    private string _keyString;

    public KeyImage(char key)
    {
      this._font = new FancyBitmapFont("smallFont");
      this._keySprite = new Sprite("buttons/keyboard/key");
      this._keyString = string.Concat((object) key);
      this._texture = this._keySprite.texture;
    }

    public override void Draw()
    {
      this._keySprite.position = this.position;
      this._keySprite.alpha = this.alpha;
      this._keySprite.color = this.color;
      this._keySprite.depth = this.depth;
      this._keySprite.scale = this.scale;
      this._keySprite.Draw();
      this._font.scale = this.scale;
      this._font.Draw(this._keyString, this.position + new Vec2((float) ((double) this._keySprite.width * (double) this._keySprite.scale.x / 2.0 - (double) this._font.GetWidth(this._keyString) / 2.0 - 1.0), 2f * this._keySprite.scale.y), new Color(20, 32, 34), this.depth + 2);
    }
  }
}
