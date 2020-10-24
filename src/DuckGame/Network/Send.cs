// Decompiled with JetBrains decompiler
// Type: DuckGame.Send
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Send
  {
    public static void Message(NetMessage msg)
    {
      msg.priority = NetMessagePriority.ReliableOrdered;
      Network.activeNetwork.QueueMessage(msg);
    }

    public static void Message(NetMessage msg, NetworkConnection who)
    {
      if (who == DuckNetwork.localConnection)
        return;
      msg.priority = NetMessagePriority.ReliableOrdered;
      Network.activeNetwork.QueueMessage(msg, who);
    }

    public static void Message(NetMessage msg, NetMessagePriority priority, NetworkConnection who = null)
    {
      if (who == DuckNetwork.localConnection)
        return;
      Network.activeNetwork.QueueMessage(msg, priority, who);
    }

    public static void MessageToAllBut(
      NetMessage msg,
      NetMessagePriority priority,
      NetworkConnection who)
    {
      if (who == DuckNetwork.localConnection)
        return;
      Network.activeNetwork.QueueMessageForAllBut(msg, priority, who);
    }
  }
}
