// Decompiled with JetBrains decompiler
// Type: DuckGame.Unlockable
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class Unlockable
  {
    private bool _locked = true;
    private string _description;
    private string _name;
    private string _achievement;
    private string _id;
    protected bool _showScreen;
    private Func<bool> _condition;

    public bool locked => this._locked;

    public string description => this._description;

    public string name => this._name;

    public string achievement => this._achievement;

    public string id => this._id;

    public bool showScreen => this._showScreen;

    public Unlockable(
      string identifier,
      Func<bool> condition,
      string nam,
      string desc,
      string achieve = "")
    {
      this._condition = condition;
      this._description = desc;
      this._name = nam;
      this._achievement = achieve;
      this._id = identifier;
    }

    public bool CheckCondition() => this._condition();

    public virtual void Initialize()
    {
    }

    public virtual void DoUnlock()
    {
      this.Unlock();
      this._locked = false;
      if (this._achievement == null || !(this._achievement != ""))
        return;
      Steam.SetAchievement(this._achievement);
    }

    protected virtual void Unlock()
    {
    }

    public virtual void DoLock()
    {
      this.Lock();
      this._locked = true;
    }

    protected virtual void Lock()
    {
    }

    public virtual void Draw(float xpos, float ypos, Depth depth)
    {
    }
  }
}
