// Decompiled with JetBrains decompiler
// Type: DuckGame.FancyBitmapFont
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class FancyBitmapFont : Transform
  {
    private const int kTilesPerRow = 16;
    private static Dictionary<string, List<Rectangle>> widthMap = new Dictionary<string, List<Rectangle>>();
    private Sprite _texture;
    private static bool _mapInitialized = false;
    private static char[] _characters = new char[96]
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
      '~',
      '`'
    };
    private static int[] _characterMap = new int[(int) byte.MaxValue];
    private InputProfile _inputProfile;
    private int _maxWidth;
    private List<Rectangle> _widths;
    private int _charHeight;
    private int _firstYPixel;
    private int _letterIndex;
    private bool _drawingOutline;
    public int _highlightStart = -1;
    public int _highlightEnd = -1;

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

    public int maxRows { get; set; }

    public int characterHeight => this._charHeight;

    public FancyBitmapFont(string image)
    {
      this._texture = new Sprite(image);
      if (!FancyBitmapFont.widthMap.TryGetValue(image, out this._widths))
      {
        this._widths = new List<Rectangle>();
        Color[] data = this._texture.texture.GetData();
        bool flag = false;
        int num1 = -1;
        for (int index1 = 1; index1 < this._texture.height; index1 += this._charHeight + 1)
        {
          for (int index2 = 0; index2 < this._texture.width; ++index2)
          {
            if (data[index2 + index1 * this._texture.width].r == (byte) 0 && data[index2 + index1 * this._texture.width].g == (byte) 0 && (data[index2 + index1 * this._texture.width].b == (byte) 0 && data[index2 + index1 * this._texture.width].a == (byte) 0))
            {
              if (num1 == -1)
                num1 = index2;
            }
            else if (num1 != -1)
            {
              if (this._charHeight == 0)
              {
                this._firstYPixel = index1;
                int num2 = index2 - 1;
                for (int index3 = index1 + 1; index3 < this._texture.height; ++index3)
                {
                  if (data[num2 + index3 * this._texture.width].r != (byte) 0 || data[num2 + index3 * this._texture.width].g != (byte) 0 || (data[num2 + index3 * this._texture.width].b != (byte) 0 || data[num2 + index3 * this._texture.width].a != (byte) 0))
                  {
                    this._charHeight = index3 - index1;
                    break;
                  }
                }
                index2 = num2 + 1;
              }
              this._widths.Add(new Rectangle((float) num1, (float) index1, (float) (index2 - num1), (float) this._charHeight));
              num1 = -1;
            }
          }
          if (flag)
            break;
        }
      }
      FancyBitmapFont.widthMap[image] = this._widths;
      if (this._widths.Count > 0)
        this._charHeight = (int) this._widths[0].height;
      if (FancyBitmapFont._mapInitialized)
        return;
      for (int index1 = 0; index1 < (int) byte.MaxValue; ++index1)
      {
        char ch = (char) index1;
        FancyBitmapFont._characterMap[index1] = 91;
        for (int index2 = 0; index2 < FancyBitmapFont._characters.Length; ++index2)
        {
          if ((int) FancyBitmapFont._characters[index2] == (int) ch)
          {
            FancyBitmapFont._characterMap[index1] = index2;
            break;
          }
        }
      }
      FancyBitmapFont._mapInitialized = true;
    }

    public Sprite ParseSprite(string text, InputProfile input)
    {
      ++this._letterIndex;
      string trigger = "";
      for (; this._letterIndex != text.Length && text[this._letterIndex] != ' ' && text[this._letterIndex] != '@'; ++this._letterIndex)
        trigger += (string) (object) text[this._letterIndex];
      Sprite sprite = (Sprite) null;
      if (input != null)
        sprite = input.GetTriggerImage(trigger);
      if (sprite == null)
        sprite = Input.GetTriggerSprite(trigger);
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

    public float GetWidth(string text, bool thinButtons = false)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (this._letterIndex = 0; this._letterIndex < text.Length; ++this._letterIndex)
      {
        bool flag = false;
        if (text[this._letterIndex] == '@')
        {
          int letterIndex = this._letterIndex;
          Sprite sprite = this.ParseSprite(text, (InputProfile) null);
          if (sprite != null)
          {
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
        {
          byte num3 = (byte) Maths.Clamp((int) text[this._letterIndex], 0, 254);
          Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num3]];
          num1 += (width.width - 1f) * this.scale.x;
        }
      }
      if ((double) num1 > (double) num2)
        num2 = num1;
      return num2;
    }

    public int GetCharacterIndex(
      string text,
      float xPosition,
      float yPosition,
      int maxRows = 2147483647,
      bool thinButtons = false)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      int num3 = 0;
      float num4 = 0.0f;
      for (this._letterIndex = 0; this._letterIndex < text.Length; ++this._letterIndex)
      {
        if ((double) num1 >= (double) xPosition && (double) yPosition < (double) num2 + (double) this._charHeight * (double) this.scale.y || num3 >= maxRows)
          return this._letterIndex - 1;
        bool flag1 = false;
        if (text[this._letterIndex] == '@')
        {
          int letterIndex = this._letterIndex;
          Sprite sprite = this.ParseSprite(text, (InputProfile) null);
          if (sprite != null)
          {
            num1 += thinButtons ? 6f : (float) ((double) sprite.width * (double) sprite.scale.x + 1.0);
            flag1 = true;
          }
          else
            this._letterIndex = letterIndex;
        }
        else if (text[this._letterIndex] == '|')
        {
          int letterIndex = this._letterIndex;
          if (this.ParseColor(text) != Colors.Transparent)
            flag1 = true;
          else
            this._letterIndex = letterIndex;
        }
        else if (text[this._letterIndex] == '\n')
        {
          if ((double) num1 > (double) num4)
            num4 = num1;
          num1 = 0.0f;
          ++num3;
          num2 += (float) this._charHeight * this.scale.y;
          flag1 = true;
          if (num3 >= maxRows)
            return this._letterIndex;
        }
        if (!flag1)
        {
          bool flag2 = false;
          if (this.maxWidth > 0)
          {
            if (text[this._letterIndex] == ' ' || text[this._letterIndex] == '|' || text[this._letterIndex] == '@')
            {
              int index = this._letterIndex + 1;
              float num5 = 0.0f;
              for (; index < text.Count<char>() && text[index] != ' ' && (text[index] != '|' && text[index] != '@'); ++index)
              {
                byte num6 = (byte) Maths.Clamp((int) text[index], 0, 254);
                Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num6]];
                num5 += (width.width - 1f) * this.scale.x;
              }
              if ((double) num1 + (double) num5 > (double) this.maxWidth)
              {
                ++num3;
                num2 += (float) this._charHeight * this.scale.y;
                num1 = 0.0f;
                flag2 = true;
                if (num3 >= maxRows)
                  return this._letterIndex;
              }
            }
            else
            {
              byte num5 = (byte) Maths.Clamp((int) text[this._letterIndex], 0, 254);
              Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num5]];
              if ((double) num1 + (double) width.width * (double) this.scale.x > (double) this.maxWidth)
              {
                ++num3;
                num2 += (float) this._charHeight * this.scale.y;
                num1 = 0.0f;
                if (num3 >= maxRows)
                  return this._letterIndex;
              }
            }
          }
          if (!flag2)
          {
            byte num5 = (byte) Maths.Clamp((int) text[this._letterIndex], 0, 254);
            Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num5]];
            num1 += (width.width - 1f) * this.scale.x;
          }
        }
        if ((double) num2 > (double) yPosition)
          return this._letterIndex;
      }
      return this._letterIndex;
    }

    public Vec2 GetCharacterPosition(string text, int index, bool thinButtons = false)
    {
      float x = 0.0f;
      float y = 0.0f;
      float num1 = 0.0f;
      for (this._letterIndex = 0; this._letterIndex < text.Length; ++this._letterIndex)
      {
        if (this._letterIndex >= index)
          return new Vec2(x, y);
        bool flag1 = false;
        if (text[this._letterIndex] == '@')
        {
          int letterIndex = this._letterIndex;
          Sprite sprite = this.ParseSprite(text, (InputProfile) null);
          if (sprite != null)
          {
            x += thinButtons ? 6f : (float) ((double) sprite.width * (double) sprite.scale.x + 1.0);
            flag1 = true;
          }
          else
            this._letterIndex = letterIndex;
        }
        else if (text[this._letterIndex] == '|')
        {
          int letterIndex = this._letterIndex;
          if (this.ParseColor(text) != Colors.Transparent)
            flag1 = true;
          else
            this._letterIndex = letterIndex;
        }
        else if (text[this._letterIndex] == '\n')
        {
          if ((double) x > (double) num1)
            num1 = x;
          x = 0.0f;
          y += (float) this._charHeight * this.scale.y;
          flag1 = true;
        }
        if (!flag1)
        {
          bool flag2 = false;
          if (this.maxWidth > 0)
          {
            if (text[this._letterIndex] == ' ' || text[this._letterIndex] == '|' || text[this._letterIndex] == '@')
            {
              int index1 = this._letterIndex + 1;
              float num2 = 0.0f;
              for (; index1 < text.Count<char>() && text[index1] != ' ' && (text[index1] != '|' && text[index1] != '@'); ++index1)
              {
                byte num3 = (byte) Maths.Clamp((int) text[index1], 0, 254);
                Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num3]];
                num2 += (width.width - 1f) * this.scale.x;
              }
              if ((double) x + (double) num2 > (double) this.maxWidth)
              {
                y += (float) this._charHeight * this.scale.y;
                x = 0.0f;
                flag2 = true;
              }
            }
            else
            {
              byte num2 = (byte) Maths.Clamp((int) text[this._letterIndex], 0, 254);
              Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num2]];
              if ((double) x + (double) width.width * (double) this.scale.x > (double) this.maxWidth)
              {
                y += (float) this._charHeight * this.scale.y;
                x = 0.0f;
              }
            }
          }
          if (!flag2)
          {
            byte num2 = (byte) Maths.Clamp((int) text[this._letterIndex], 0, 254);
            Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num2]];
            x += (width.width - 1f) * this.scale.x;
          }
        }
      }
      return new Vec2(x, y);
    }

    public void DrawOutline(
      string text,
      Vec2 pos,
      Color c,
      Color outline,
      Depth deep = default (Depth),
      float outlineThickness = 1f)
    {
      this._drawingOutline = true;
      this.Draw(text, pos + new Vec2(-outlineThickness, 0.0f), outline, deep + 2, true);
      this.Draw(text, pos + new Vec2(outlineThickness, 0.0f), outline, deep + 2, true);
      this.Draw(text, pos + new Vec2(0.0f, -outlineThickness), outline, deep + 2, true);
      this.Draw(text, pos + new Vec2(0.0f, outlineThickness), outline, deep + 2, true);
      this.Draw(text, pos + new Vec2(-outlineThickness, -outlineThickness), outline, deep + 2, true);
      this.Draw(text, pos + new Vec2(outlineThickness, -outlineThickness), outline, deep + 2, true);
      this.Draw(text, pos + new Vec2(-outlineThickness, outlineThickness), outline, deep + 2, true);
      this.Draw(text, pos + new Vec2(outlineThickness, outlineThickness), outline, deep + 2, true);
      this._drawingOutline = false;
      this.Draw(text, pos, c, deep + 5);
    }

    public void Draw(string text, Vec2 pos, Color c, Depth deep = default (Depth), bool colorSymbols = false) => this.Draw(text, pos.x, pos.y, c, deep, colorSymbols);

    public void Draw(
      string text,
      float xpos,
      float ypos,
      Color c,
      Depth deep = default (Depth),
      bool colorSymbols = false)
    {
      if (string.IsNullOrWhiteSpace(text))
        return;
      Color color1 = new Color((int) byte.MaxValue - (int) c.r, (int) byte.MaxValue - (int) c.g, (int) byte.MaxValue - (int) c.b);
      float num1 = 0.0f;
      float num2 = 0.0f;
      int num3 = 0;
      for (this._letterIndex = 0; this._letterIndex < text.Length; ++this._letterIndex)
      {
        bool flag1 = false;
        if (text[this._letterIndex] == '@')
        {
          int letterIndex = this._letterIndex;
          Sprite sprite = this.ParseSprite(text, (InputProfile) null);
          if (sprite != null)
          {
            float alpha = sprite.alpha;
            sprite.alpha = this.alpha * c.ToVector4().w;
            if (sprite != null)
            {
              float num4 = (float) (this.characterHeight / 2 - sprite.height / 2);
              if (colorSymbols)
                sprite.color = c;
              Graphics.Draw(sprite, xpos + num2, ypos + num1 + num4, deep);
              num2 += (float) ((double) sprite.width * (double) sprite.scale.x + 1.0);
              sprite.color = Color.White;
            }
            sprite.alpha = alpha;
            flag1 = true;
          }
          else
            this._letterIndex = letterIndex;
        }
        else if (text[this._letterIndex] == '|')
        {
          int letterIndex = this._letterIndex;
          Color color2 = this.ParseColor(text);
          if (color2 != Colors.Transparent)
          {
            if (!this._drawingOutline)
            {
              float w = c.ToVector4().w;
              c = color2;
              c *= w;
            }
            flag1 = true;
          }
          else
            this._letterIndex = letterIndex;
        }
        if (!flag1)
        {
          bool flag2 = false;
          if (this.maxWidth > 0)
          {
            if (text[this._letterIndex] == ' ' || text[this._letterIndex] == '|' || text[this._letterIndex] == '@')
            {
              int index = this._letterIndex + 1;
              char ch = ' ';
              float width1 = this._widths[FancyBitmapFont._characterMap[(int) (byte) ch]].width;
              for (; index < text.Count<char>() && text[index] != ' ' && (text[index] != '|' && text[index] != '@'); ++index)
              {
                byte num4 = (byte) Maths.Clamp((int) text[index], 0, 254);
                Rectangle width2 = this._widths[FancyBitmapFont._characterMap[(int) num4]];
                width1 += (width2.width - 1f) * this.scale.x;
              }
              if ((double) num2 + (double) width1 > (double) this.maxWidth)
              {
                num1 += (float) this._charHeight * this.scale.y;
                num2 = 0.0f;
                ++num3;
                flag2 = true;
              }
            }
            else
            {
              byte num4 = (byte) Maths.Clamp((int) text[this._letterIndex], 0, 254);
              Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num4]];
              if ((double) num2 + (double) width.width * (double) this.scale.x > (double) this.maxWidth)
              {
                num1 += (float) this._charHeight * this.scale.y;
                num2 = 0.0f;
                ++num3;
              }
            }
          }
          if (this.maxRows != 0 && num3 >= this.maxRows)
            break;
          if (!flag2)
          {
            if (text[this._letterIndex] == '\n')
            {
              num1 += (float) this._charHeight * this.scale.y;
              num2 = 0.0f;
              ++num3;
            }
            else
            {
              byte num4 = (byte) Maths.Clamp((int) text[this._letterIndex], 0, 254);
              Rectangle width = this._widths[FancyBitmapFont._characterMap[(int) num4]];
              Graphics.ResetDepthBias();
              this._texture.scale = this.scale;
              if (this._highlightStart != -1 && this._highlightStart != this._highlightEnd && (this._highlightStart < this._highlightEnd && this._letterIndex >= this._highlightStart && this._letterIndex < this._highlightEnd || this._letterIndex < this._highlightStart && this._letterIndex >= this._highlightEnd))
              {
                Graphics.DrawRect(new Vec2(xpos + num2, ypos + num1), new Vec2(xpos + num2, ypos + num1) + new Vec2(width.width * this.scale.x, (float) this._charHeight * this.scale.y), c, deep - 5);
                this._texture.color = color1;
              }
              else
                this._texture.color = c;
              this._texture.alpha = this.alpha;
              Graphics.Draw(this._texture, xpos + num2, ypos + num1, width, deep);
              num2 += (width.width - 1f) * this.scale.x;
            }
          }
        }
      }
    }
  }
}
