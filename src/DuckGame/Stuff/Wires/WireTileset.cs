// Decompiled with JetBrains decompiler
// Type: DuckGame.WireTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("stuff|wires")]
  [BaggedProperty("isOnlineCapable", true)]
  public class WireTileset : AutoTile
  {
    private List<WireConnection> _connections = new List<WireConnection>();
    private List<WireTileset.WireSignal> _signals = new List<WireTileset.WireSignal>();
    private List<WireTileset.WireSignal> _addSignals = new List<WireTileset.WireSignal>();
    private List<WireTileset.WireSignal> _removeSignals = new List<WireTileset.WireSignal>();
    private WireConnection _centerWire;
    private Sprite _signalSprite;
    public bool dullSignalLeft;
    public bool dullSignalRight;
    public bool dullSignalUp;
    public bool dullSignalDown;

    public override int frame
    {
      get => base.frame;
      set
      {
        base.frame = value;
        if (base.frame == value)
          return;
        this.UpdateConnections();
      }
    }

    public WireConnection centerWire => this._centerWire;

    public WireTileset(float x, float y)
      : base(x, y, "wireTileset")
    {
      this._editorName = "Wire";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 8f;
      this.verticalWidthThick = 8f;
      this.horizontalHeight = 8f;
      this.layer = Layer.Foreground;
      this.depth = new Depth(-0.8f);
      this._signalSprite = new Sprite("wireBulge");
      this._signalSprite.CenterOrigin();
    }

    public void Emit(WireTileset.WireSignal signal = null, float overshoot = 0.0f, int type = 0)
    {
      if (this._centerWire == null)
        return;
      if (signal == null)
      {
        if (this._centerWire.up != null)
          this._signals.Add(new WireTileset.WireSignal()
          {
            position = this._centerWire.position,
            prevPosition = this._centerWire.position,
            travel = this._centerWire.up,
            from = this._centerWire,
            currentWire = this,
            signalType = type
          });
        if (this._centerWire.down != null)
          this._signals.Add(new WireTileset.WireSignal()
          {
            position = this._centerWire.position,
            prevPosition = this._centerWire.position,
            travel = this._centerWire.down,
            from = this._centerWire,
            currentWire = this,
            signalType = type
          });
        if (this._centerWire.left != null)
          this._signals.Add(new WireTileset.WireSignal()
          {
            position = this._centerWire.position,
            prevPosition = this._centerWire.position,
            travel = this._centerWire.left,
            from = this._centerWire,
            currentWire = this,
            signalType = type
          });
        if (this._centerWire.right == null)
          return;
        this._signals.Add(new WireTileset.WireSignal()
        {
          position = this._centerWire.position,
          prevPosition = this._centerWire.position,
          travel = this._centerWire.right,
          from = this._centerWire,
          currentWire = this,
          signalType = type
        });
      }
      else
      {
        WireConnection travel = signal.travel;
        this._removeSignals.Add(signal);
        signal.finished = true;
        if (travel == this._centerWire)
          Level.CheckCircle<IWirePeripheral>(this._centerWire.position, 3f)?.Pulse(signal.signalType, this);
        if (travel.up != null && travel.up != signal.from && !this.dullSignalUp)
        {
          WireTileset.WireSignal signal1 = new WireTileset.WireSignal()
          {
            position = travel.position,
            travel = travel.up,
            from = travel,
            currentWire = this,
            life = signal.life,
            prevPosition = signal.prevPosition,
            signalType = signal.signalType
          };
          this.TravelSignal(signal1, overshoot, false);
          if (!signal1.finished)
            this._addSignals.Add(signal1);
        }
        if (travel.down != null && travel.down != signal.from && !this.dullSignalDown)
        {
          WireTileset.WireSignal signal1 = new WireTileset.WireSignal()
          {
            position = travel.position,
            travel = travel.down,
            from = travel,
            currentWire = this,
            life = signal.life,
            prevPosition = signal.prevPosition,
            signalType = signal.signalType
          };
          this.TravelSignal(signal1, overshoot, false);
          if (!signal1.finished)
            this._addSignals.Add(signal1);
        }
        if (travel.left != null && travel.left != signal.from && !this.dullSignalLeft)
        {
          WireTileset.WireSignal signal1 = new WireTileset.WireSignal()
          {
            position = travel.position,
            travel = travel.left,
            from = travel,
            currentWire = this,
            life = signal.life,
            prevPosition = signal.prevPosition,
            signalType = signal.signalType
          };
          this.TravelSignal(signal1, overshoot, false);
          if (!signal1.finished)
            this._addSignals.Add(signal1);
        }
        if (travel.right != null && travel.right != signal.from && !this.dullSignalRight)
        {
          WireTileset.WireSignal signal1 = new WireTileset.WireSignal()
          {
            position = travel.position,
            travel = travel.right,
            from = travel,
            currentWire = this,
            life = signal.life,
            prevPosition = signal.prevPosition,
            signalType = signal.signalType
          };
          this.TravelSignal(signal1, overshoot, false);
          if (!signal1.finished)
            this._addSignals.Add(signal1);
        }
        Vec2 position = signal.travel.position;
        if (travel.wireLeft && !this.dullSignalLeft && (this.leftTile is WireTileset leftTile && leftTile != signal.currentWire))
        {
          signal.travel = leftTile.GetConnection(position);
          leftTile.Emit(signal, overshoot, signal.signalType);
        }
        if (travel.wireRight && !this.dullSignalRight && (this.rightTile is WireTileset rightTile && rightTile != signal.currentWire))
        {
          signal.travel = rightTile.GetConnection(position);
          rightTile.Emit(signal, overshoot, signal.signalType);
        }
        if (travel.wireUp && !this.dullSignalUp && (this.upTile is WireTileset upTile && upTile != signal.currentWire))
        {
          signal.travel = upTile.GetConnection(position);
          upTile.Emit(signal, overshoot, signal.signalType);
        }
        if (travel.wireDown && !this.dullSignalDown && (this.downTile is WireTileset downTile && downTile != signal.currentWire))
        {
          signal.travel = downTile.GetConnection(position);
          downTile.Emit(signal, overshoot, signal.signalType);
        }
        this.dullSignalDown = false;
        this.dullSignalUp = false;
        this.dullSignalLeft = false;
        this.dullSignalRight = false;
      }
    }

    public void TravelSignal(WireTileset.WireSignal signal, float travelSpeed, bool updatePrev = true)
    {
      if (updatePrev)
        signal.prevPosition = signal.position;
      float overshoot;
      if ((double) signal.travel.position.x < (double) signal.position.x)
      {
        signal.position.x -= travelSpeed;
        overshoot = signal.travel.position.x - signal.position.x;
      }
      else if ((double) signal.travel.position.x > (double) signal.position.x)
      {
        signal.position.x += travelSpeed;
        overshoot = signal.position.x - signal.travel.position.x;
      }
      else if ((double) signal.travel.position.y > (double) signal.position.y)
      {
        signal.position.y += travelSpeed;
        overshoot = signal.position.y - signal.travel.position.y;
      }
      else if ((double) signal.travel.position.y < (double) signal.position.y)
      {
        signal.position.y -= travelSpeed;
        overshoot = signal.travel.position.y - signal.position.y;
      }
      else
        overshoot = 0.0f;
      signal.life -= (float) ((double) travelSpeed / 16.0 * 0.00999999977648258);
      if ((double) overshoot >= 0.0 && (double) signal.life > 0.0)
        this.Emit(signal, overshoot, signal.signalType);
      if ((double) signal.life > 0.0)
        return;
      this._removeSignals.Add(signal);
    }

    public WireConnection GetConnection(Vec2 pos)
    {
      float num = 9999f;
      WireConnection wireConnection = this._centerWire;
      foreach (WireConnection connection in this._connections)
      {
        float lengthSq = (connection.position - pos).lengthSq;
        if ((double) lengthSq < (double) num)
        {
          num = lengthSq;
          wireConnection = connection;
        }
      }
      return wireConnection;
    }

    public override void Update()
    {
      if (this._centerWire == null)
        this.UpdateConnections();
      float travelSpeed = 16f;
      foreach (WireTileset.WireSignal signal in this._signals)
        this.TravelSignal(signal, travelSpeed);
      foreach (WireTileset.WireSignal removeSignal in this._removeSignals)
        this._signals.Remove(removeSignal);
      foreach (WireTileset.WireSignal addSignal in this._addSignals)
        this._signals.Add(addSignal);
      this._removeSignals.Clear();
      this._addSignals.Clear();
      base.Update();
    }

    public override void Draw()
    {
      foreach (WireTileset.WireSignal signal in this._signals)
      {
        this._signalSprite.depth = new Depth(-0.6f);
        this._signalSprite.alpha = signal.life;
        this._signalSprite.xscale = this._signalSprite.yscale = 1f;
        Graphics.Draw(this._signalSprite, signal.position.x, signal.position.y);
        Vec2 prevPosition = signal.prevPosition;
        Vec2 vec2 = signal.position - signal.prevPosition;
        float length = vec2.length;
        vec2.Normalize();
        float num = 0.3f;
        for (int index = 0; index < 3; ++index)
        {
          Sprite signalSprite = this._signalSprite;
          signalSprite.depth = signalSprite.depth - 1;
          prevPosition += vec2 * (length / 4f);
          this._signalSprite.alpha = num * signal.life;
          num += 0.2f;
          Graphics.Draw(this._signalSprite, prevPosition.x, prevPosition.y);
        }
      }
      base.Draw();
    }

    private void UpdateConnections()
    {
      this.upTile = Level.CheckPoint<AutoTile>(this.x, this.y - 16f, (Thing) this);
      this.downTile = Level.CheckPoint<AutoTile>(this.x, this.y + 16f, (Thing) this);
      this.leftTile = Level.CheckPoint<AutoTile>(this.x - 16f, this.y, (Thing) this);
      this.rightTile = Level.CheckPoint<AutoTile>(this.x + 16f, this.y, (Thing) this);
      this._connections.Clear();
      if (this._sprite.frame == 32 || this._sprite.frame == 41)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection = new WireConnection()
        {
          position = this.position + new Vec2(8f, -4f),
          left = this._centerWire,
          wireRight = true
        };
        this._centerWire.right = wireConnection;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection);
      }
      else if (this._sprite.frame == 37 || this._sprite.frame == 43)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection = new WireConnection()
        {
          position = this.position + new Vec2(-8f, -4f),
          right = this._centerWire,
          wireLeft = true
        };
        this._centerWire.left = wireConnection;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection);
      }
      else if (this._sprite.frame == 33 || this._sprite.frame == 35 || (this._sprite.frame == 36 || this._sprite.frame == 59))
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(8f, -4f),
          left = this._centerWire,
          wireRight = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(-8f, -4f),
          right = this._centerWire,
          wireLeft = true
        };
        this._centerWire.right = wireConnection1;
        this._centerWire.left = wireConnection2;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
      }
      else if (this._sprite.frame == 34)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(8f, -4f),
          left = this._centerWire,
          wireRight = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(-8f, -4f),
          right = this._centerWire,
          wireLeft = true
        };
        WireConnection wireConnection3 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 8f),
          up = this._centerWire,
          wireDown = true
        };
        this._centerWire.right = wireConnection1;
        this._centerWire.left = wireConnection2;
        this._centerWire.down = wireConnection3;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
        this._connections.Add(wireConnection3);
      }
      else if (this._sprite.frame == 42)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(8f, -4f),
          left = this._centerWire,
          wireRight = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(-8f, -4f),
          right = this._centerWire,
          wireLeft = true
        };
        WireConnection wireConnection3 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 8f),
          up = this._centerWire,
          wireDown = true
        };
        WireConnection wireConnection4 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -8f),
          down = this._centerWire,
          wireUp = true
        };
        this._centerWire.right = wireConnection1;
        this._centerWire.left = wireConnection2;
        this._centerWire.down = wireConnection3;
        this._centerWire.up = wireConnection4;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
        this._connections.Add(wireConnection3);
        this._connections.Add(wireConnection4);
      }
      else if (this._sprite.frame == 44)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 0.0f)
        };
        WireConnection wireConnection = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -8f),
          down = this._centerWire,
          wireUp = true
        };
        this._centerWire.up = wireConnection;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection);
      }
      else if (this._sprite.frame == 45)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(-8f, -4f),
          right = this._centerWire,
          wireLeft = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 8f),
          up = this._centerWire,
          wireDown = true
        };
        WireConnection wireConnection3 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -8f),
          down = this._centerWire,
          wireUp = true
        };
        this._centerWire.left = wireConnection1;
        this._centerWire.down = wireConnection2;
        this._centerWire.up = wireConnection3;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
        this._connections.Add(wireConnection3);
      }
      else if (this._sprite.frame == 49)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 0.0f)
        };
        WireConnection wireConnection = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 8f),
          up = this._centerWire,
          wireDown = true
        };
        this._centerWire.down = wireConnection;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection);
      }
      else if (this._sprite.frame == 50)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 0.0f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 8f),
          up = this._centerWire,
          wireDown = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -8f),
          down = this._centerWire,
          wireUp = true
        };
        this._centerWire.down = wireConnection1;
        this._centerWire.up = wireConnection2;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
      }
      else if (this._sprite.frame == 51)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(8f, -4f),
          left = this._centerWire,
          wireRight = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 8f),
          up = this._centerWire,
          wireDown = true
        };
        this._centerWire.right = wireConnection1;
        this._centerWire.down = wireConnection2;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
      }
      else if (this._sprite.frame == 52)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(-8f, -4f),
          right = this._centerWire,
          wireLeft = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 8f),
          up = this._centerWire,
          wireDown = true
        };
        this._centerWire.left = wireConnection1;
        this._centerWire.down = wireConnection2;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
      }
      else if (this._sprite.frame == 53)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(8f, -4f),
          left = this._centerWire,
          wireRight = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, 8f),
          up = this._centerWire,
          wireDown = true
        };
        WireConnection wireConnection3 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -8f),
          down = this._centerWire,
          wireUp = true
        };
        this._centerWire.right = wireConnection1;
        this._centerWire.down = wireConnection2;
        this._centerWire.up = wireConnection3;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
        this._connections.Add(wireConnection3);
      }
      else if (this._sprite.frame == 57)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(8f, -4f),
          left = this._centerWire,
          wireRight = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(-8f, -4f),
          right = this._centerWire,
          wireLeft = true
        };
        WireConnection wireConnection3 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -8f),
          down = this._centerWire,
          wireUp = true
        };
        this._centerWire.right = wireConnection1;
        this._centerWire.left = wireConnection2;
        this._centerWire.up = wireConnection3;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
        this._connections.Add(wireConnection3);
      }
      else if (this._sprite.frame == 58)
      {
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(8f, -4f),
          left = this._centerWire,
          wireRight = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -8f),
          down = this._centerWire,
          wireUp = true
        };
        this._centerWire.right = wireConnection1;
        this._centerWire.up = wireConnection2;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
      }
      else
      {
        if (this._sprite.frame != 60)
          return;
        this._centerWire = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -4f)
        };
        WireConnection wireConnection1 = new WireConnection()
        {
          position = this.position + new Vec2(-8f, -4f),
          right = this._centerWire,
          wireLeft = true
        };
        WireConnection wireConnection2 = new WireConnection()
        {
          position = this.position + new Vec2(0.0f, -8f),
          down = this._centerWire,
          wireUp = true
        };
        this._centerWire.left = wireConnection1;
        this._centerWire.up = wireConnection2;
        this._connections.Add(this._centerWire);
        this._connections.Add(wireConnection1);
        this._connections.Add(wireConnection2);
      }
    }

    public class WireSignal
    {
      public Vec2 position;
      public WireConnection travel;
      public WireConnection from;
      public WireTileset currentWire;
      public bool finished;
      public int signalType;
      public float life = 1f;
      public Vec2 prevPosition;
    }
  }
}
