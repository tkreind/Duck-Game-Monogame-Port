// Decompiled with JetBrains decompiler
// Type: DuckGame.SynchronizedContentManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Threading;

namespace DuckGame
{
  public class SynchronizedContentManager : ContentManager
  {
    private object syncRoot = new object();

    public SynchronizedContentManager(IServiceProvider serviceProvider)
      : base(serviceProvider)
    {
    }

    public override T Load<T>(string assetName)
    {
      lock (this.syncRoot)
      {
        if (Thread.CurrentThread != MonoMain.mainThread)
        {
          while (!MonoMain.contentLoadLockLoop)
            ;
        }
        MonoMain.contentLoadingLock = true;
        try
        {
          T obj = base.Load<T>(assetName);
          MonoMain.contentLoadingLock = false;
          return obj;
        }
        catch (Exception ex)
        {
          MonoMain.contentLoadingLock = false;
          throw ex;
        }
      }
    }

    public Texture2D FromStream(Stream stream)
    {
      lock (this.syncRoot)
      {
        do
          ;
        while (!MonoMain.contentLoadLockLoop);
        MonoMain.contentLoadingLock = true;
        try
        {
          Texture2D texture2D = Texture2D.FromStream(((IGraphicsDeviceService) this.ServiceProvider.GetService(typeof (IGraphicsDeviceService))).GraphicsDevice, stream);
          MonoMain.contentLoadingLock = false;
          return texture2D;
        }
        catch (Exception ex)
        {
          MonoMain.contentLoadingLock = false;
          throw ex;
        }
      }
    }
  }
}
