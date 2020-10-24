// Decompiled with JetBrains decompiler
// Type: DuckGame.Persona
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public static class Persona
  {
    private static List<DuckPersona> _personas = new List<DuckPersona>()
    {
      new DuckPersona(new Vec3((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)),
      new DuckPersona(new Vec3(125f, 125f, 125f)),
      new DuckPersona(new Vec3(247f, 224f, 90f)),
      new DuckPersona(new Vec3(205f, 107f, 29f))
    };

    public static DuckPersona Duck1 => Persona._personas[0];

    public static DuckPersona Duck2 => Persona._personas[1];

    public static DuckPersona Duck3 => Persona._personas[2];

    public static DuckPersona Duck4 => Persona._personas[3];

    public static IEnumerable<DuckPersona> all => (IEnumerable<DuckPersona>) Persona._personas;

    public static void Update()
    {
      foreach (DuckPersona persona in Persona._personas)
        persona.Update();
    }

    public static void Initialize()
    {
    }

    public static int Number(DuckPersona p) => Persona._personas.IndexOf(p);
  }
}
