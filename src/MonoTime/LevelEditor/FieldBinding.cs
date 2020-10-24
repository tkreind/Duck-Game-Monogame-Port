// Decompiled with JetBrains decompiler
// Type: DuckGame.FieldBinding
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Reflection;

namespace DuckGame
{
  public class FieldBinding
  {
    private object _thing;
    private FieldInfo _field;
    private PropertyInfo _property;
    private float _min;
    private float _max;
    private float _inc;

    public object thing => this._thing;

    public object value
    {
      get => !(this._field != (FieldInfo) null) ? this._property.GetValue(this._thing, (object[]) null) : this._field.GetValue(this._thing);
      set
      {
        if (this._field != (FieldInfo) null)
          this._field.SetValue(this._thing, value);
        else
          this._property.SetValue(this._thing, value, (object[]) null);
      }
    }

    public float min => this._min;

    public float max => this._max;

    public float inc => this._inc;

    public FieldBinding(object thing, string field, float min = 0.0f, float max = 1f, float increment = 0.1f)
    {
      this._thing = thing;
      this._field = thing.GetType().GetField(field);
      if (this._field == (FieldInfo) null)
        this._property = thing.GetType().GetProperty(field);
      this._min = min;
      this._max = max;
      this._inc = increment;
    }

    public FieldBinding(System.Type thing, string field, float min = 0.0f, float max = 1f, float increment = 0.1f)
    {
      this._thing = (object) thing;
      this._field = thing.GetField(field);
      if (this._field == (FieldInfo) null)
        this._property = thing.GetProperty(field);
      this._min = min;
      this._max = max;
      this._inc = increment;
    }
  }
}
