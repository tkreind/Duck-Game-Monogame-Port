// Decompiled with JetBrains decompiler
// Type: DuckGame.TextLine
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class TextLine
  {
    public List<TextSegment> segments = new List<TextSegment>();
    public Color lineColor = Color.White;

    public string text
    {
      get
      {
        string str = "";
        foreach (TextSegment segment in this.segments)
          str += segment.text;
        return str;
      }
    }

    public void Add(char letter)
    {
      if (this.segments.Count == 0)
        this.segments.Add(new TextSegment()
        {
          color = this.lineColor
        });
      this.segments[0].text += (string) (object) letter;
    }

    public void Add(string val)
    {
      if (this.segments.Count == 0)
        this.segments.Add(new TextSegment()
        {
          color = this.lineColor
        });
      this.segments[0].text += val;
    }

    public void SwitchColor(Color c)
    {
      this.lineColor = c;
      if (this.segments.Count > 0 && this.segments[this.segments.Count - 1].text.Length == 0)
        this.segments[this.segments.Count - 1].color = c;
      else
        this.segments.Insert(0, new TextSegment()
        {
          color = c
        });
    }

    public int Length()
    {
      int num1 = 0;
      foreach (TextSegment segment in this.segments)
      {
        int num2 = 0;
        for (int index = 0; index < segment.text.Length; ++index)
        {
          if (segment.text[index] == '@')
          {
            ++index;
            while (segment.text[index] != '@')
              ++index;
          }
          else
            ++num2;
        }
        num1 += num2;
      }
      return num1;
    }
  }
}
