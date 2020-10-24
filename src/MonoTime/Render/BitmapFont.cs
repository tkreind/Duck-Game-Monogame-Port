// Decompiled with JetBrains decompiler
// Type: DuckGame.BitmapFont
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Linq;

namespace DuckGame
{
  public class BitmapFont : Transform
  {
    private const int kTilesPerRow = 16;
    private SpriteMap _texture;
    private static bool _mapInitialized = false;
    public static char[] _characters = new char[95]
    {
      ' ',
      '!',
      '"',
      '#',
      '$',
      '%',
      '&',
      '\'',
      '(',
      ')',
      '*',
      '+',
      ',',
      '-',
      '.',
      '/',
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      ':',
      ';',
      '>',
      '=',
      '<',
      '?',
      '@',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z',
      '[',
      '\\',
      ']',
      '^',
      '_',
      '`',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'g',
      'h',
      'i',
      'j',
      'k',
      'l',
      'm',
      'n',
      'o',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z',
      '{',
      '|',
      '}',
      '~'
    };
    private static int[] _characterMap = new int[(int) byte.MaxValue];
    private int _tileSize = 8;
    private InputProfile _inputProfile;
    private Sprite _titleWing;
    private int _maxWidth;
    private int _letterIndex;
    public int characterYOffset;
    public Vec2 spriteScale = new Vec2(1f, 1f);

    public float height => (float) this._texture.height * this.scale.y;

    public InputProfile inputProfile
    {
      get => this._inputProfile;
      set => this._inputProfile = value;
    }

    public int maxWidth
    {
      get => this._maxWidth;
      set => this._maxWidth = value;
    }

    public BitmapFont(string image, int size, int ysize = -1)
    {
      if (ysize < 0)
        ysize = size;
      this._texture = new SpriteMap(image, size, ysize);
      this._tileSize = size;
      if (!BitmapFont._mapInitialized)
      {
        for (int index1 = 0; index1 < (int) byte.MaxValue; ++index1)
        {
          char ch = (char) index1;
          BitmapFont._characterMap[index1] = 91;
          for (int index2 = 0; index2 < BitmapFont._characters.Length; ++index2)
          {
            if ((int) BitmapFont._characters[index2] == (int) ch)
            {
              BitmapFont._characterMap[index1] = index2;
              break;
            }
          }
        }
        BitmapFont._mapInitialized = true;
      }
      this._titleWing = new Sprite("arcade/titleWing");
    }

    public Sprite ParseSprite(string text, InputProfile input)
    {
      ++this._letterIndex;
      string str = "";
      for (; this._letterIndex != text.Length && text[this._letterIndex] != ' ' && text[this._letterIndex] != '@'; ++this._letterIndex)
        str += (string) (object) text[this._letterIndex];
      Sprite sprite = (Sprite) null;
      if (input != null)
      {
        sprite = input.GetTriggerImage(str);
        if (sprite == null && Triggers.IsTrigger(str))
          return new Sprite();
      }
      if (sprite == null)
        sprite = Input.GetTriggerSprite(str);
      return sprite;
    }

    public Color ParseColor(string text)
    {
      ++this._letterIndex;
      string color = "";
      for (; this._letterIndex != text.Length && text[this._letterIndex] != ' ' && text[this._letterIndex] != '|'; ++this._letterIndex)
        color += (string) (object) text[this._letterIndex];
      return Colors.ParseColor(color);
    }

    public InputProfile GetInputProfile(InputProfile input)
    {
      if (input == null)
        input = this._inputProfile != null ? this._inputProfile : InputProfile.FirstProfileWithDevice;
      return input;
    }

    public float GetWidth(string text, bool thinButtons = false, InputProfile input = null)
    {
      input = this.GetInputProfile(input);
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (this._letterIndex = 0; this._letterIndex < text.Length; ++this._letterIndex)
      {
        bool flag = false;
        if (text[this._letterIndex] == '@')
        {
          int letterIndex = this._letterIndex;
          Sprite sprite = this.ParseSprite(text, input);
          if (sprite != null)
          {
            if (sprite.texture != null)
              num1 += thinButtons ? 6f : (float) ((double) sprite.width * (double) sprite.scale.x + 1.0);
            flag = true;
          }
          else
            this._letterIndex = letterIndex;
        }
        else if (text[this._letterIndex] == '|')
        {
          int letterIndex = this._letterIndex;
          if (this.ParseColor(text) != Colors.Transparent)
            flag = true;
          else
            this._letterIndex = letterIndex;
        }
        else if (text[this._letterIndex] == '\n')
        {
          if ((double) num1 > (double) num2)
            num2 = num1;
          num1 = 0.0f;
        }
        if (!flag)
          num1 += (float) this._tileSize * this.scale.x;
      }
      if ((double) num1 > (double) num2)
        num2 = num1;
      return num2;
    }

    public void DrawOutline(string text, Vec2 pos, Color c, Color outline, Depth deep = default (Depth))
    {
      this.Draw(text, pos + new Vec2(-1f * this.scale.x, 0.0f), outline, deep + 2, colorSymbols: true);
      this.Draw(text, pos + new Vec2(1f * this.scale.x, 0.0f), outline, deep + 2, colorSymbols: true);
      this.Draw(text, pos + new Vec2(0.0f, -1f * this.scale.y), outline, deep + 2, colorSymbols: true);
      this.Draw(text, pos + new Vec2(0.0f, 1f * this.scale.y), outline, deep + 2, colorSymbols: true);
      this.Draw(text, pos + new Vec2(-1f * this.scale.x, -1f * this.scale.y), outline, deep + 2, colorSymbols: true);
      this.Draw(text, pos + new Vec2(1f * this.scale.x, -1f * this.scale.y), outline, deep + 2, colorSymbols: true);
      this.Draw(text, pos + new Vec2(-1f * this.scale.x, 1f * this.scale.y), outline, deep + 2, colorSymbols: true);
      this.Draw(text, pos + new Vec2(1f * this.scale.x, 1f * this.scale.y), outline, deep + 2, colorSymbols: true);
      this.Draw(text, pos, c, deep + 5);
    }

    public void Draw(
      string text,
      Vec2 pos,
      Color c,
      Depth deep = default (Depth),
      InputProfile input = null,
      bool colorSymbols = false)
    {
      this.Draw(text, pos.x, pos.y, c, deep, input, colorSymbols);
    }

    public void Draw(
      string text,
      float xpos,
      float ypos,
      Color c,
      Depth deep = default (Depth),
      InputProfile input = null,
      bool colorSymbols = false)
    {
      if (input == null)
        input = MonoMain.started ? (Profiles.active.Count <= 0 || Network.isActive ? (this._inputProfile != null ? this._inputProfile : Input.lastActiveProfile) : Profiles.active[0].inputProfile) : InputProfile.DefaultPlayer1;
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (this._letterIndex = 0; this._letterIndex < text.Length; ++this._letterIndex)
      {
        bool flag = false;
        if (text[this._letterIndex] == '@')
        {
          int letterIndex = this._letterIndex;
          Sprite sprite1 = this.ParseSprite(text, input);
          if (sprite1 != null)
          {
            if (sprite1.texture != null)
            {
              float alpha = sprite1.alpha;
              sprite1.alpha = this.alpha * c.ToVector4().w;
              if (sprite1 != null)
              {
                Vec2 scale = sprite1.scale;
                Sprite sprite2 = sprite1;
                sprite2.scale = sprite2.scale * this.spriteScale;
                float num3 = (float) ((int) ((double) this._texture.height * (double) this.spriteScale.y / 2.0) - (int) ((double) sprite1.height * (double) this.spriteScale.y / 2.0));
                if (colorSymbols)
                  sprite1.color = c;
                Graphics.Draw(sprite1, xpos + num2, ypos + num1 + num3, deep);
                num2 += (float) ((double) sprite1.width * (double) sprite1.scale.x + 1.0);
                sprite1.scale = scale;
                sprite1.color = Color.White;
              }
              sprite1.alpha = alpha;
            }
            flag = true;
          }
          else
            this._letterIndex = letterIndex;
        }
        else if (text[this._letterIndex] == '|')
        {
          int letterIndex = this._letterIndex;
          Color color = this.ParseColor(text);
          if (color != Colors.Transparent)
          {
            float w = c.ToVector4().w;
            c = color;
            c *= w;
            flag = true;
          }
          else
            this._letterIndex = letterIndex;
        }
        if (!flag)
        {
          if (this.maxWidth > 0)
          {
            string source = "";
            for (int letterIndex = this._letterIndex; letterIndex < text.Count<char>() && text[letterIndex] != ' ' && (text[letterIndex] != '|' && text[letterIndex] != '@'); ++letterIndex)
              source += (string) (object) text[letterIndex];
            if ((double) num2 + (double) source.Count<char>() * ((double) this._tileSize * (double) this.scale.x) > (double) this.maxWidth)
            {
              num1 += (float) this._texture.height * this.scale.y;
              num2 = 0.0f;
            }
          }
          if (text[this._letterIndex] == '\n')
          {
            num1 += (float) this._texture.height * this.scale.y;
            num2 = 0.0f;
          }
          else
          {
            byte num3 = (byte) Maths.Clamp((int) text[this._letterIndex], 0, 254);
            this._texture.frame = BitmapFont._characterMap[(int) num3];
            this._texture.scale = this.scale;
            this._texture.color = c;
            this._texture.alpha = this.alpha;
            Graphics.ResetDepthBias();
            Graphics.Draw((Sprite) this._texture, xpos + num2, ypos + num1 + (float) this.characterYOffset, deep);
            num2 += (float) this._tileSize * this.scale.x;
          }
        }
      }
    }
  }
}
