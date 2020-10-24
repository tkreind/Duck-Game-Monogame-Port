// Decompiled with JetBrains decompiler
// Type: DuckGame.StateBinding
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>
  /// A state binding allows a Thing to communicate the state of a field over the network during multiplayer.
  /// These are generally private members of your Thing.
  /// </summary>
  public class StateBinding
  {
    public int bitIndex;
    protected GhostPriority _priority;
    public bool valid = true;
    protected bool _lerp;
    private bool _initialized = true;
    private string _fieldName;
    protected object _thing;
    public object _previousValue;
    protected int _bits = -1;
    private bool _trueOnly;
    private bool _isRotation;
    private bool _isVelocity;
    public bool skipLerp;
    protected AccessorInfo _accessor = new AccessorInfo();

    public GhostPriority priority => this._priority;

    public override string ToString() => this.GetDebugString((object) null);

    public virtual string GetDebugString(object with)
    {
      if (with != null)
        return this.name + " = " + Convert.ToString(with);
      return this.value == null ? this.name + " = null" : this.name + " = " + Convert.ToString(this.value);
    }

    public bool lerp => this._lerp;

    public bool initialized
    {
      get => this._initialized;
      set => this._initialized = value;
    }

    public string name => this._fieldName;

    public object owner => this._thing;

    public virtual object value
    {
      get => this._accessor.getAccessor == null ? (object) null : this._accessor.getAccessor(this._thing);
      set
      {
        if (this._accessor.setAccessor == null)
          return;
        this._accessor.setAccessor(this._thing, value);
      }
    }

    public virtual System.Type type => this._accessor.type;

    public static bool Compare(object o1, object o2)
    {
      switch (o1)
      {
        case float num:
          return (double) Math.Abs(num - (float) o2) < 1.0 / 1000.0;
        case Vec2 vec2_2:
          Vec2 vec2_1 = vec2_2 - (Vec2) o2;
          return (double) Math.Abs(vec2_1.x) < 0.00499999988824129 && (double) Math.Abs(vec2_1.y) < 0.00499999988824129;
        case BitBuffer _:
          return false;
        default:
          return object.Equals(o1, o2);
      }
    }

    public bool dirty => !StateBinding.Compare(this._previousValue, this.value);

    public void Clean()
    {
      if (this._trueOnly)
        this._previousValue = this.value = (object) false;
      else
        this._previousValue = this.value;
    }

    public virtual int bits => this._bits;

    public bool trueOnly => this._trueOnly;

    public bool isRotation => this._isRotation;

    public bool isVelocity => this._isVelocity;

    public StateBinding(string field, int bits = -1, bool rot = false, bool vel = false)
    {
      this._fieldName = field;
      this._previousValue = (object) null;
      this._bits = bits;
      this._isRotation = rot;
      this._isVelocity = vel;
    }

    public StateBinding(bool doLerp, string field, int bits = -1, bool rot = false, bool vel = false)
    {
      this._fieldName = field;
      this._previousValue = (object) null;
      this._bits = bits;
      this._isRotation = rot;
      this._isVelocity = vel;
      this._lerp = doLerp;
      if (!this._lerp)
        return;
      this._priority = GhostPriority.Normal;
    }

    public StateBinding(
      GhostPriority p,
      string field,
      int bits = -1,
      bool rot = false,
      bool vel = false,
      bool doLerp = false)
    {
      this._fieldName = field;
      this._previousValue = (object) null;
      this._bits = bits;
      this._isRotation = rot;
      this._isVelocity = vel;
      this._priority = p;
      this._lerp = doLerp;
    }

    public StateBinding(string field, int bits, bool rot)
    {
      this._fieldName = field;
      this._previousValue = (object) null;
      this._bits = bits;
      this._isRotation = rot;
      this._isVelocity = false;
    }

    public StateBinding(string boolfield, bool trueOnly)
    {
      this._fieldName = boolfield;
      this._previousValue = (object) null;
      this._bits = -1;
      this._trueOnly = true;
    }

    public virtual object GetNetValue() => this.value;

    public virtual object ReadNetValue(object val) => val;

    public bool connected => this._accessor != null;

    public virtual void Connect(Thing t)
    {
      this._thing = (object) t;
      this._accessor = Editor.GetAccessorInfo(t.GetType(), this._fieldName);
      if (this._accessor == null)
        throw new Exception("Could not find accessor for binding.");
    }
  }
}
