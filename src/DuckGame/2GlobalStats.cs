// Decompiled with JetBrains decompiler
// Type: DuckGame.StatBinding
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class StatBinding
  {
    private string _name;
    private object _value;

    public object value
    {
      get => this._value;
      set => this._value = value;
    }

    public int valueInt
    {
      get => this._value is float ? (int) (float) this._value : (int) this._value;
      set
      {
        if (!(this._value is int) || value > (int) this._value)
          this._value = (object) value;
        Steam.SetStat(this._name, this.valueInt);
      }
    }

    public float valueFloat
    {
      get => this._value is int ? (float) (int) this._value : (float) this._value;
      set
      {
        if (!(this._value is float) || (double) value > (double) (float) this._value)
          this._value = (object) value;
        Steam.SetStat(this._name, this.valueFloat);
      }
    }

    public void BindName(string name)
    {
      this._name = name;
      this._value = (object) 0.0f;
      if (!Steam.IsInitialized())
        return;
      this._value = (object) Steam.GetStat(this._name);
    }

    public bool isFloat => this._value is float;

    public static implicit operator float(StatBinding val) => val.valueFloat;

    public static implicit operator int(StatBinding val) => val.valueInt;

    public static StatBinding operator +(StatBinding c1, int c2)
    {
      c1.valueInt += c2;
      return c1;
    }

    public static StatBinding operator -(StatBinding c1, int c2)
    {
      c1.valueInt -= c2;
      return c1;
    }

    public static StatBinding operator +(StatBinding c1, float c2)
    {
      c1.valueFloat += c2;
      return c1;
    }

    public static StatBinding operator -(StatBinding c1, float c2)
    {
      c1.valueFloat -= c2;
      return c1;
    }

    public static bool operator <(StatBinding c1, float c2) => (double) c1.valueFloat < (double) c2;

    public static bool operator >(StatBinding c1, float c2) => (double) c1.valueFloat > (double) c2;

    public static bool operator <(StatBinding c1, int c2) => c1.valueInt < c2;

    public static bool operator >(StatBinding c1, int c2) => c1.valueInt > c2;
  }
}
