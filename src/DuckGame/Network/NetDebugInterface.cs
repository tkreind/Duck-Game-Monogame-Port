// Decompiled with JetBrains decompiler
// Type: DuckGame.NetDebugInterface
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class NetDebugInterface
  {
    public bool visible;
    private List<NetDebugInterface.NetDebugButton> buttons = new List<NetDebugInterface.NetDebugButton>()
    {
      new NetDebugInterface.NetDebugButton("Quality", 20, 100, 5, NetDebugInterface.NetDebugButtonType.Quality),
      new NetDebugInterface.NetDebugButton("Latency", 0, 1000, 50, NetDebugInterface.NetDebugButtonType.Milliseconds),
      new NetDebugInterface.NetDebugButton("Jitter", 0, 400, 20, NetDebugInterface.NetDebugButtonType.Milliseconds),
      new NetDebugInterface.NetDebugButton("Loss", 0, 100, 5, NetDebugInterface.NetDebugButtonType.Percent),
      new NetDebugInterface.NetDebugButton("Traffic", 0, 500, 25, NetDebugInterface.NetDebugButtonType.BytesPerTick),
      new NetDebugInterface.NetDebugButton("Failure", 0, 100, 5, NetDebugInterface.NetDebugButtonType.Likelyhood),
      new NetDebugInterface.NetDebugButton("Strip", 0, 5, 0, NetDebugInterface.NetDebugButtonType.ButtonStrip)
    };
    public int index;
    public Vec2 position = Vec2.Zero;
    private bool _drawGraph;
    private bool _pin;
    private bool _mute;

    public float x
    {
      get => this.position.x;
      set => this.position.x = value;
    }

    public float y
    {
      get => this.position.y;
      set => this.position.y = value;
    }

    public bool pin => this._pin;

    public bool mute => this._mute;

    public void Update(Network core)
    {
      float y = 0.0f;
      foreach (NetDebugInterface.NetDebugButton button in this.buttons)
      {
        button.position = this.position + new Vec2(0.0f, y);
        button.Update();
        button.Hover(Mouse.positionConsole);
        if (button.name == "Quality")
        {
          float num1 = (float) (button.level * button.increment) / (float) button.maxLevel;
          if ((double) core.core.quality != (double) num1)
          {
            float num2 = 1f - num1;
            core.core.quality = num1;
            this.buttons.FirstOrDefault<NetDebugInterface.NetDebugButton>((Func<NetDebugInterface.NetDebugButton, bool>) (x => x.name == "Latency")).level = (int) ((double) num2 * 10.0);
            this.buttons.FirstOrDefault<NetDebugInterface.NetDebugButton>((Func<NetDebugInterface.NetDebugButton, bool>) (x => x.name == "Jitter")).level = (int) ((double) num2 * 10.0);
            this.buttons.FirstOrDefault<NetDebugInterface.NetDebugButton>((Func<NetDebugInterface.NetDebugButton, bool>) (x => x.name == "Loss")).level = (int) ((double) num2 * 10.0);
            this.buttons.FirstOrDefault<NetDebugInterface.NetDebugButton>((Func<NetDebugInterface.NetDebugButton, bool>) (x => x.name == "Failure")).level = (int) ((double) num2 * 10.0);
            core.core.spikes = num2;
          }
        }
        else if (button.name == "Latency")
          core.core.minimumLatency = (float) (button.level * button.increment) / (float) button.maxLevel;
        if (button.name == "Jitter")
          core.core.jitter = (float) ((double) (button.level * button.increment) / (double) button.maxLevel * 0.200000002980232);
        if (button.name == "Loss")
          core.core.dropPercent = (float) (button.level * button.increment) / (float) button.maxLevel;
        if (button.name == "Failure")
          core.core.failurePercent = (float) (button.level * button.increment) / (float) button.maxLevel;
        if (button.name == "Traffic")
          core.core.traffic = (float) (button.level * button.increment) / (float) button.maxLevel;
        if (button.name == "Strip")
        {
          if (button.level == 0)
            Network.Disconnect();
          if (button.level == 2)
            this._drawGraph = !this._drawGraph;
          if (button.level == 3)
          {
            this._pin = !this._pin;
            button.pin = this._pin;
          }
          if (button.level == 4)
          {
            this._mute = !this._mute;
            button.mute = this.mute;
          }
          button.level = -1;
        }
        y += 9f;
      }
    }

    public void Draw(Network core)
    {
      if (!DevConsole.enableNetworkDebugging || !NetworkDebugger.enabled)
        return;
      this.visible = false;
      if (!this._pin && ((double) Mouse.xConsole < 0.0 || (double) Mouse.yConsole < 0.0 || ((double) Mouse.xConsole > (double) Layer.Console.width || (double) Mouse.yConsole > (double) Layer.Console.height)))
        return;
      this.visible = true;
      foreach (NetDebugInterface.NetDebugButton button in this.buttons)
        button.Draw(core);
      Vec2 vec2 = new Vec2(Layer.Console.width, Layer.Console.height);
      if (NetworkDebugger.enabled)
        vec2 = new Vec2(vec2.x / 2f, vec2.y / 2f);
      if (this._drawGraph)
      {
        core.core.netGraph.DrawChart(new Vec2(this.x + 4f, (float) ((double) this.y + (double) vec2.y - 108.0)));
        Graphics.DrawRect(new Vec2(this.x, (float) ((double) this.y + (double) vec2.y - 120.0)), new Vec2(this.x + (vec2.x - 20f), (float) ((double) this.y + (double) vec2.y - 20.0)), Color.Black * 0.8f, (Depth) 0.1f);
      }
      else
      {
        float num1 = vec2.y - 22f;
        int index1 = DevConsole.core.lines.Count - 1;
        float num2 = 0.0f;
        int num3 = 10;
        if ((double) Mouse.y * 2.0 > (double) this.y + (double) num1 - 40.0)
          num3 = 60;
        bool flag = (double) DevConsole.core.lines.Count / 2.0 % 1.0 == 0.0;
        for (int index2 = 0; index2 < num3 && index1 >= 0; ++index2)
        {
          DCLine dcLine = DevConsole.core.lines.ElementAt<DCLine>(index1);
          if (!NetworkDebugger.enabled || dcLine.threadIndex == core.networkIndex)
          {
            float num4 = 1f;
            if (index2 > 7)
              num4 = (float) (1.0 - (double) (index2 - 7) / (double) (num3 - 7));
            float num5 = 1f;
            if (flag)
              num5 = 0.7f;
            flag = !flag;
            Color color = dcLine.color;
            color.r = (byte) ((double) color.r * (double) num5);
            color.g = (byte) ((double) color.g * (double) num5);
            color.b = (byte) ((double) color.b * (double) num5);
            Graphics.DrawString(dcLine.SectionString() + dcLine.line, new Vec2(this.x + 10f, (float) ((double) this.y + (double) num1 - (double) num2 - 10.0)), color * 0.8f * num4, (Depth) 0.9f);
            num2 += 9f;
          }
          --index1;
        }
        Graphics.DrawRect(new Vec2(this.x, this.y + vec2.y - (float) (num3 * 9 + 28)), new Vec2(this.x + (vec2.x - 20f), (float) ((double) this.y + (double) vec2.y - 20.0)), Color.Black * 0.8f, (Depth) 0.1f);
      }
    }

    public enum NetDebugButtonType
    {
      Milliseconds,
      Percent,
      Likelyhood,
      ButtonStrip,
      BytesPerTick,
      Quality,
    }

    public class NetDebugButton
    {
      public NetDebugInterface.NetDebugButtonType type;
      public string name;
      public Vec2 position;
      public int level;
      public int hoverLevel;
      public int maxLevel;
      public int increment;
      public bool pin;
      public bool mute;
      public Vec2 barTL;
      public Vec2 barBR;

      public NetDebugButton(
        string nam,
        int lev,
        int maxLev,
        int inc,
        NetDebugInterface.NetDebugButtonType tp)
      {
        this.name = nam;
        this.level = lev;
        this.maxLevel = maxLev;
        this.increment = inc;
        this.type = tp;
      }

      public string GetValueString()
      {
        int num1 = this.level * this.increment;
        if (this.hoverLevel != -1)
          num1 = this.hoverLevel * this.increment;
        if (this.type == NetDebugInterface.NetDebugButtonType.Milliseconds)
          return num1.ToString() + "ms";
        if (this.type == NetDebugInterface.NetDebugButtonType.Percent)
          return num1.ToString() + "%";
        if (this.type == NetDebugInterface.NetDebugButtonType.Likelyhood)
        {
          float num2 = (float) num1 / (float) this.maxLevel;
          if ((double) num2 < 0.00999999977648258)
            return "Never";
          if ((double) num2 < 0.300000011920929)
            return "Rarely";
          if ((double) num2 < 0.600000023841858)
            return "Sometimes";
          return (double) num2 < 0.949999988079071 ? "Often" : "Always";
        }
        if (this.type == NetDebugInterface.NetDebugButtonType.BytesPerTick)
          return num1.ToString() + " bytes per tick";
        if (this.type == NetDebugInterface.NetDebugButtonType.Quality)
        {
          num1 = 100 - num1;
          if (num1 <= 0)
            return "perfect";
          if (num1 <= 10)
            return "great";
          if (num1 <= 20)
            return "good";
          if (num1 <= 50)
            return "poor";
          if (num1 <= 65)
            return "bad";
          if (num1 <= 80)
            return "awful";
          if (num1 <= 90)
            return "terrible";
          if (num1 <= 100)
            return "hand delivered packets";
        }
        else if (this.type == NetDebugInterface.NetDebugButtonType.ButtonStrip)
        {
          if (this.level == 0)
            return "Disconnect";
          if (this.level == 1)
            return "Kill";
          if (this.level == 2)
            return "Graph";
          if (this.level == 3)
            return this.pin ? "UNPIN" : "PIN";
          if (this.level == 4)
            return this.mute ? "UNMUTE" : "MUTE";
        }
        return num1.ToString();
      }

      public void Hover(Vec2 m)
      {
        this.hoverLevel = -1;
        if (this.type == NetDebugInterface.NetDebugButtonType.ButtonStrip)
        {
          this.level = -1;
          if ((double) m.x <= (double) this.barTL.x || (double) m.y <= (double) this.barTL.y || ((double) m.x >= (double) this.barBR.x || (double) m.y >= (double) this.barBR.y))
            return;
          this.hoverLevel = (int) Math.Round((double) this.maxLevel * (((double) m.x - (double) this.barTL.x) / ((double) this.barBR.x - (double) this.barTL.x)));
          if (Mouse.left != InputState.Pressed)
            return;
          this.level = this.hoverLevel;
        }
        else
        {
          if ((double) m.x <= (double) this.barTL.x || (double) m.y <= (double) this.barTL.y || ((double) m.x >= (double) this.barBR.x || (double) m.y >= (double) this.barBR.y))
            return;
          this.hoverLevel = (int) Math.Round((double) (this.maxLevel / this.increment) * (((double) m.x - (double) this.barTL.x) / ((double) this.barBR.x - (double) this.barTL.x)));
          if (Mouse.left != InputState.Pressed)
            return;
          this.level = this.hoverLevel;
        }
      }

      public void Draw(Network n)
      {
        if (!DevConsole.enableNetworkDebugging || !NetworkDebugger.enabled)
          return;
        if (this.type == NetDebugInterface.NetDebugButtonType.ButtonStrip)
        {
          Vec2 position = this.position;
          this.barTL = position;
          int maxLevel = this.maxLevel;
          for (int index = 0; index < maxLevel; ++index)
          {
            Color col = Color.Gray;
            if (this.hoverLevel == index)
              col = new Color(170, 170, 170);
            float x = 100f;
            Graphics.DrawRect(position, position + new Vec2(x, 8f), col, (Depth) 0.8f);
            this.level = index;
            string valueString = this.GetValueString();
            Graphics.DrawString(valueString, position + new Vec2(x / 2f, 0.0f) - new Vec2(Graphics.GetStringWidth(valueString) / 2f, 0.0f), Color.White, (Depth) 0.9f);
            position.x += x + 2f;
          }
          this.level = -1;
          this.barBR = new Vec2(position.x, position.y + 8f);
          position.x += 2f;
          Graphics.DrawRect(this.position, new Vec2(this.position.x + 480f, position.y + 9f), Color.Black * 0.8f, (Depth) 0.1f);
        }
        else
        {
          Graphics.DrawString(this.name + ": ", this.position, Color.White, (Depth) 1f);
          int num = this.maxLevel / this.increment;
          Vec2 vec2 = this.position + new Vec2(80f, 0.0f);
          this.barTL = vec2;
          for (int index = 0; index < num; ++index)
          {
            Color col = Color.Gray;
            bool flag = false;
            if (this.hoverLevel >= index && this.hoverLevel != -1)
            {
              col = Color.White;
              flag = true;
            }
            else if (this.level >= index)
            {
              col = new Color(200, 200, 200);
              flag = true;
            }
            if (flag && this.name == "Quality")
            {
              float r = (float) (1.0 - (double) index / (double) num);
              Color color = new Color(r, 1f - r, 0.0f);
              col.r = (byte) ((double) col.r * (double) r);
              col.g = (byte) ((double) col.g * (1.0 - (double) r));
              col.b = (byte) 0;
            }
            Graphics.DrawRect(vec2, vec2 + new Vec2(4f, 8f), col, (Depth) 0.9f);
            vec2.x += 5f;
          }
          this.barBR = new Vec2(vec2.x, vec2.y + 8f);
          vec2.x += 2f;
          string text = "(" + this.GetValueString() + ")";
          if (this.name == "Latency" && n.core.connections.Count > 0)
            text = text + " |GREEN|(" + ((int) ((double) n.core.averagePing * 1000.0)).ToString() + "ms actual)" + " |YELLOW|(" + ((int) ((double) n.core.averagePingPeak * 1000.0)).ToString() + "ms peak)";
          else if (this.name == "Jitter" && n.core.connections.Count > 0)
            text = text + " |GREEN|(" + ((int) ((double) n.core.averageJitter * 1000.0)).ToString() + "ms actual)" + " |YELLOW|(" + ((int) ((double) n.core.averageJitterPeak * 1000.0)).ToString() + "ms peak)";
          else if (this.name == "Loss" && n.core.connections.Count > 0)
            text = text + " |RED|(" + n.core.averagePacketLoss.ToString() + " lost)" + " |RED|(" + n.core.averagePacketLossPercent.ToString() + "% avg)";
          Graphics.DrawString(text, vec2, Color.White, (Depth) 1f);
          vec2.x += Graphics.GetStringWidth(text);
          Graphics.DrawRect(this.position, new Vec2(this.position.x + 480f, vec2.y + 9f), Color.Black * 0.8f, (Depth) 0.1f);
        }
      }

      public void Update()
      {
      }
    }
  }
}
