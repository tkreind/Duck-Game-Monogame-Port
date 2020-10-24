// Decompiled with JetBrains decompiler
// Type: DuckGame.DeviceChangeNotifier
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Threading;
using System.Windows.Forms;

namespace DuckGame
{
  public class DeviceChangeNotifier : Form
  {
    private static DeviceChangeNotifier mInstance;
    private static Thread thread;

    public static void Start()
    {
      try
      {
        DeviceChangeNotifier.thread = new Thread(new ThreadStart(DeviceChangeNotifier.runForm));
        DeviceChangeNotifier.thread.SetApartmentState(ApartmentState.STA);
        DeviceChangeNotifier.thread.IsBackground = true;
        DeviceChangeNotifier.thread.Priority = ThreadPriority.BelowNormal;
        DeviceChangeNotifier.thread.Start();
      }
      catch
      {
      }
    }

    public static void Stop()
    {
      try
      {
        try
        {
          if (DeviceChangeNotifier.mInstance == null)
            throw new InvalidOperationException("Notifier not started");
          DeviceChangeNotifier.mInstance.Invoke((Delegate) new MethodInvoker(DeviceChangeNotifier.mInstance.endForm));
        }
        catch (Exception ex)
        {
        }
        if (DeviceChangeNotifier.thread == null)
          return;
        DeviceChangeNotifier.thread.Abort();
      }
      catch
      {
      }
    }

    private static void runForm()
    {
      try
      {
        Application.Run((Form) new DeviceChangeNotifier());
      }
      catch
      {
      }
    }

    private void endForm()
    {
      try
      {
        this.Close();
      }
      catch
      {
      }
    }

    protected override void SetVisibleCore(bool value)
    {
      try
      {
        if (DeviceChangeNotifier.mInstance == null)
          this.CreateHandle();
        DeviceChangeNotifier.mInstance = this;
        value = false;
        base.SetVisibleCore(value);
      }
      catch
      {
      }
    }

    protected override void WndProc(ref Message m)
    {
      try
      {
        if (m.Msg == 537)
          Input.devicesChanged = true;
        base.WndProc(ref m);
      }
      catch
      {
      }
    }
  }
}
