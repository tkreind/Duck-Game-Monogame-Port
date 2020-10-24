// Decompiled with JetBrains decompiler
// Type: DuckGame.VideoEditor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class VideoEditor
  {
    public static void Draw() => Graphics.DrawLine(new Vec2(32f, Layer.HUD.camera.height - 16f), new Vec2(Layer.HUD.camera.width - 32f, Layer.HUD.camera.height - 16f), Color.White, depth: (new Depth(1f)));
  }
}
