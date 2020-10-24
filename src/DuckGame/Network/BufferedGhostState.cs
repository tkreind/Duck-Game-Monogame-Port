// Decompiled with JetBrains decompiler
// Type: DuckGame.BufferedGhostState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class BufferedGhostState
  {
    public BufferedGhostState prev;
    public List<BufferedGhostProperty> properties = new List<BufferedGhostProperty>(30);
    public long mask;
    public NetIndex16 tick = (NetIndex16) 0;
    public byte interpolationTime;
    public byte interpolationProgress;
    public bool applied;
    public int delayTime;
    public int life = 15;
    protected int _refs;
    public List<ushort> inputStates = new List<ushort>();
    public static BufferedGhostState lastAppliedState;
    public static BufferedGhostState lastPreviousState;

    public BufferedGhostState()
    {
      for (int index = 0; index < NetworkConnection.packetsEvery; ++index)
        this.inputStates.Add((ushort) 0);
    }

    public void ReInitialize() => this.properties.Clear();

    public void Reset(bool clearProperties = true)
    {
      if (!clearProperties)
        return;
      this.properties.Clear();
    }

    public Thing owner => this.properties.Count <= 0 ? (Thing) null : this.properties[0].binding.owner as Thing;

    public void Apply(BufferedGhostState previous, float lerp = 1f)
    {
      BufferedGhostState.lastAppliedState = this;
      BufferedGhostState.lastPreviousState = previous;
      for (int index = 0; index < this.properties.Count; ++index)
      {
        BufferedGhostProperty property = this.properties[index];
        if (property.valid)
          property.binding.value = property.value;
        else if (previous != null)
          property.binding.value = property.value = previous.properties[index].value;
        else
          property.value = property.binding.value;
      }
    }

    public bool Trickle(BufferedGhostState downFrom)
    {
      bool flag = false;
      for (int index = 0; index < this.properties.Count; ++index)
      {
        BufferedGhostProperty property = this.properties[index];
        if (!property.isNetValue && downFrom.properties[index].valid)
        {
          property.value = downFrom.properties[index].value;
          property.valid = true;
          flag = true;
        }
      }
      return flag;
    }
  }
}
