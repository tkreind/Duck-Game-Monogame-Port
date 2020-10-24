// Decompiled with JetBrains decompiler
// Type: DuckGame.SplitScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DuckGame
{
  public class SplitScreen
  {
    private static List<FollowCam> _cameras = new List<FollowCam>();

    public static void Draw()
    {
      Camera camera = Level.current.camera;
      foreach (Profile profile in Profiles.active)
      {
        Viewport viewport = DuckGame.Graphics.viewport;
        if (profile.duck != null)
        {
          if (profile.persona == Persona.Duck1)
          {
            DuckGame.Graphics.viewport = new Viewport(0, 0, DuckGame.Graphics.viewport.Width / 2 - 2, DuckGame.Graphics.viewport.Height / 2 - 2);
            if (SplitScreen._cameras.Count < 1)
              SplitScreen._cameras.Add(new FollowCam());
            SplitScreen._cameras[0].minSize = 160f;
            SplitScreen._cameras[0].Clear();
            if (profile.duck.ragdoll != null)
              SplitScreen._cameras[0].Add((Thing) profile.duck.ragdoll);
            else if (profile.duck._trapped != null)
              SplitScreen._cameras[0].Add((Thing) profile.duck._trapped);
            else if (profile.duck._cooked != null)
              SplitScreen._cameras[0].Add((Thing) profile.duck._cooked);
            else
              SplitScreen._cameras[0].Add((Thing) profile.duck);
            Level.current.camera = (Camera) SplitScreen._cameras[0];
            SplitScreen._cameras[0].lerpSpeed = 0.11f;
            SplitScreen._cameras[0].Update();
            Layer.DrawLayers();
          }
          else if (profile.persona == Persona.Duck2)
          {
            DuckGame.Graphics.viewport = new Viewport(DuckGame.Graphics.viewport.Width / 2 + 2, 0, DuckGame.Graphics.viewport.Width / 2 - 2, DuckGame.Graphics.viewport.Height / 2 - 2);
            if (SplitScreen._cameras.Count < 2)
              SplitScreen._cameras.Add(new FollowCam());
            SplitScreen._cameras[1].minSize = 160f;
            SplitScreen._cameras[1].Clear();
            if (profile.duck.ragdoll != null)
              SplitScreen._cameras[1].Add((Thing) profile.duck.ragdoll);
            else if (profile.duck._trapped != null)
              SplitScreen._cameras[1].Add((Thing) profile.duck._trapped);
            else if (profile.duck._cooked != null)
              SplitScreen._cameras[1].Add((Thing) profile.duck._cooked);
            else
              SplitScreen._cameras[1].Add((Thing) profile.duck);
            Level.current.camera = (Camera) SplitScreen._cameras[1];
            SplitScreen._cameras[1].lerpSpeed = 0.11f;
            SplitScreen._cameras[1].Update();
            Layer.DrawLayers();
          }
          else if (profile.persona == Persona.Duck3)
          {
            DuckGame.Graphics.viewport = new Viewport(0, DuckGame.Graphics.viewport.Height / 2 + 2, DuckGame.Graphics.viewport.Width / 2 - 2, DuckGame.Graphics.viewport.Height / 2 - 2);
            if (SplitScreen._cameras.Count < 3)
              SplitScreen._cameras.Add(new FollowCam());
            SplitScreen._cameras[2].minSize = 160f;
            SplitScreen._cameras[2].Clear();
            if (profile.duck.ragdoll != null)
              SplitScreen._cameras[2].Add((Thing) profile.duck.ragdoll);
            else if (profile.duck._trapped != null)
              SplitScreen._cameras[2].Add((Thing) profile.duck._trapped);
            else if (profile.duck._cooked != null)
              SplitScreen._cameras[2].Add((Thing) profile.duck._cooked);
            else
              SplitScreen._cameras[2].Add((Thing) profile.duck);
            Level.current.camera = (Camera) SplitScreen._cameras[2];
            SplitScreen._cameras[2].lerpSpeed = 0.11f;
            SplitScreen._cameras[2].Update();
            Layer.DrawLayers();
          }
          else if (profile.persona == Persona.Duck4)
          {
            DuckGame.Graphics.viewport = new Viewport(DuckGame.Graphics.viewport.Width / 2 + 2, DuckGame.Graphics.viewport.Height / 2 + 2, DuckGame.Graphics.viewport.Width / 2 - 2, DuckGame.Graphics.viewport.Height / 2 - 2);
            if (SplitScreen._cameras.Count < 4)
              SplitScreen._cameras.Add(new FollowCam());
            SplitScreen._cameras[3].minSize = 160f;
            SplitScreen._cameras[3].Clear();
            if (profile.duck.ragdoll != null)
              SplitScreen._cameras[3].Add((Thing) profile.duck.ragdoll);
            else if (profile.duck._trapped != null)
              SplitScreen._cameras[3].Add((Thing) profile.duck._trapped);
            else if (profile.duck._cooked != null)
              SplitScreen._cameras[3].Add((Thing) profile.duck._cooked);
            else
              SplitScreen._cameras[3].Add((Thing) profile.duck);
            Level.current.camera = (Camera) SplitScreen._cameras[3];
            SplitScreen._cameras[3].Update();
            SplitScreen._cameras[3].lerpSpeed = 0.11f;
            Layer.DrawLayers();
          }
          DuckGame.Graphics.viewport = viewport;
        }
      }
      Level.current.camera = camera;
    }
  }
}
