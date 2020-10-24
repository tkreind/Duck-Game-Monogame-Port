// Decompiled with JetBrains decompiler
// Type: DuckGame.StateFlagBinding
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class StateFlagBinding : StateBinding
  {
    private string[] _fields;
    private List<AccessorInfo> _accessors = new List<AccessorInfo>();
    private ushort _value;

    public override string ToString() => this.GetDebugString((object) null);

    public override string GetDebugString(object with)
    {
      string str = "";
      int index = 0;
      byte num = 1;
      foreach (string field in this._fields)
      {
        if (with == null)
          str = str + field + ": " + Convert.ToString((bool) this._accessors[index].getAccessor(this._thing) ? 1 : 0) + " | ";
        else
          str = str + field + ": " + Convert.ToString(((long) (ushort) with & 1L << this._bits - (int) num) != 0L ? 1 : 0) + " | ";
        ++index;
        ++num;
      }
      return str;
    }

    public override System.Type type => typeof (ushort);

    public override object value
    {
      get
      {
        this._value = (ushort) 0;
        bool flag = true;
        foreach (AccessorInfo accessor in this._accessors)
        {
          if (!flag)
            this._value <<= 1;
          this._value |= (bool) accessor.getAccessor(this._thing) ? (ushort) 1 : (ushort) 0;
          flag = false;
        }
        return (object) this._value;
      }
      set
      {
        this._value = (ushort) value;
        byte num = 1;
        foreach (AccessorInfo accessor in this._accessors)
        {
          accessor.setAccessor(this._thing, (object) (((long) this._value & 1L << this._bits - (int) num) != 0L));
          ++num;
        }
      }
    }

    public StateFlagBinding(params string[] fields)
      : base("multiple")
    {
      this._fields = fields;
      this._priority = GhostPriority.Normal;
    }

    public StateFlagBinding(GhostPriority p, params string[] fields)
      : base("multiple")
    {
      this._fields = fields;
      this._priority = p;
    }

    public override void Connect(Thing t)
    {
      this._bits = 0;
      this._thing = (object) t;
      System.Type type = t.GetType();
      this._accessors.Clear();
      foreach (string field in this._fields)
      {
        this._accessors.Add(Editor.GetAccessorInfo(type, field));
        ++this._bits;
      }
    }
  }
}
