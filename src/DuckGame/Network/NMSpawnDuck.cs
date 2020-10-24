// Decompiled with JetBrains decompiler
// Type: DuckGame.NMSpawnDuck
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMSpawnDuck : NMEvent
  {
    public byte index;

    public NMSpawnDuck(byte idx) => this.index = idx;

    public NMSpawnDuck()
    {
    }

    public override void Activate()
    {
      Profile profile = DuckNetwork.profiles[(int) this.index];
      if (profile.localPlayer)
        profile.duck.connection = DuckNetwork.localConnection;
      else
        profile.duck.connection = profile.connection;
      profile.duck.visible = true;
      Vec3 color = profile.persona.color;
      Level.Add((Thing) new SpawnLine(profile.duck.x, profile.duck.y, 0, 0.0f, new Color((int) color.x, (int) color.y, (int) color.z), 32f));
      Level.Add((Thing) new SpawnLine(profile.duck.x, profile.duck.y, 0, -4f, new Color((int) color.x, (int) color.y, (int) color.z), 4f));
      Level.Add((Thing) new SpawnLine(profile.duck.x, profile.duck.y, 0, 4f, new Color((int) color.x, (int) color.y, (int) color.z), 4f));
      SFX.Play("pullPin", 0.7f);
    }
  }
}
