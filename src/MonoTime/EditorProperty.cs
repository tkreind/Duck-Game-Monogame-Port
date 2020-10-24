// Decompiled with JetBrains decompiler
// Type: DuckGame.EditorProperty`1
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class EditorProperty<T>
  {
    private T _value;
    public string _tooltip;
    private float _min;
    private float _max = 1f;
    private float _increment = 0.1f;
    private string _minSpecial;
    private bool _isTime;
    private bool _isLevel;
    private Thing _notify;
    private string _section = "";

    public T value
    {
      get => this._value;
      set
      {
        this._value = value;
        if (this._notify == null)
          return;
        this._notify.EditorPropertyChanged((object) this);
      }
    }

    public EditorPropertyInfo info => new EditorPropertyInfo()
    {
      value = (object) this._value,
      min = this._min,
      max = this._max,
      increment = this._increment,
      minSpecial = this._minSpecial,
      isTime = this._isTime,
      isLevel = this._isLevel,
      tooltip = this._tooltip
    };

    public string section => this._section;

    public EditorProperty(
      T val,
      Thing notify = null,
      float min = 0.0f,
      float max = 1f,
      float increment = 0.1f,
      string minSpecial = null,
      bool isTime = false,
      bool isLevel = false)
    {
      this._value = val;
      this._min = min;
      this._max = max;
      this._increment = increment;
      this._minSpecial = minSpecial;
      this._isTime = isTime;
      this._notify = notify;
      this._isLevel = isLevel;
    }

    public EditorProperty(
      T val,
      string varSection,
      Thing notify = null,
      float min = 0.0f,
      float max = 1f,
      float increment = 0.1f,
      string minSpecial = null,
      bool isTime = false,
      bool isLevel = false)
    {
      this._value = val;
      this._min = min;
      this._max = max;
      this._increment = increment;
      this._minSpecial = minSpecial;
      this._isTime = isTime;
      this._notify = notify;
      this._section = varSection;
      this._isLevel = isLevel;
    }

    public static implicit operator EditorProperty<T>(T val) => new EditorProperty<T>(val);

    public static implicit operator T(EditorProperty<T> val) => val._value;
  }
}
