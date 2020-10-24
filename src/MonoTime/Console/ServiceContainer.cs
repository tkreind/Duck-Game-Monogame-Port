// Decompiled with JetBrains decompiler
// Type: WinFormsGraphicsDevice.ServiceContainer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace WinFormsGraphicsDevice
{
  /// <summary>
  /// Container class implements the IServiceProvider interface. This is used
  /// to pass shared services between different components, for instance the
  /// ContentManager uses it to locate the IGraphicsDeviceService implementation.
  /// </summary>
  public class ServiceContainer : IServiceProvider
  {
    private Dictionary<Type, object> services = new Dictionary<Type, object>();

    /// <summary>Adds a new service to the collection.</summary>
    public void AddService<T>(T service) => this.services.Add(typeof (T), (object) service);

    /// <summary>Looks up the specified service.</summary>
    public object GetService(Type serviceType)
    {
      object obj;
      this.services.TryGetValue(serviceType, out obj);
      return obj;
    }
  }
}
