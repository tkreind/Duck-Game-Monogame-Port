// Decompiled with JetBrains decompiler
// Type: DuckGame.FireManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class FireManager
  {
    private static int _curFireID;
    private static int _curUpdateID;
    private static int _curSmokeID;

    public static int GetFireID()
    {
      ++FireManager._curFireID;
      if (FireManager._curFireID > 20)
        FireManager._curFireID = 0;
      return FireManager._curFireID;
    }

    public static int GetSmokeID()
    {
      ++FireManager._curSmokeID;
      if (FireManager._curSmokeID > 20)
        FireManager._curSmokeID = 0;
      return FireManager._curSmokeID;
    }

    public static void Update()
    {
      foreach (SmallFire smallFire in Level.current.things[typeof (SmallFire)])
      {
        if (smallFire.fireID == FireManager._curUpdateID && (double) smallFire.alpha > 0.5)
        {
          Thing thing = (Thing) null;
          if (smallFire.stick != null && smallFire.stick is DartGun)
            thing = smallFire.stick.owner;
          smallFire.doFloat = false;
          foreach (MaterialThing materialThing in Level.CheckCircleAll<MaterialThing>(smallFire.position + new Vec2(0.0f, -4f), 6f))
          {
            if (materialThing.isServerForObject && materialThing != thing)
            {
              if (materialThing is FluidPuddle)
              {
                smallFire.doFloat = true;
                FluidPuddle fluidPuddle = materialThing as FluidPuddle;
                if ((double) fluidPuddle.data.flammable <= 0.5 && (double) fluidPuddle.data.heat < 0.5)
                {
                  Level.Remove((Thing) smallFire);
                  break;
                }
              }
              if (materialThing is Duck duck && ((double) duck.slideBuildup > 0.0 && duck.sliding || duck.holdObject is Sword && (duck.holdObject as Sword)._slamStance))
              {
                SmallSmoke smallSmoke = SmallSmoke.New(smallFire.x + Rando.Float(-1f, 1f), smallFire.y + Rando.Float(-1f, 1f));
                smallSmoke.vSpeed -= Rando.Float(0.1f, 0.2f);
                Level.Add((Thing) smallSmoke);
                Level.Remove((Thing) smallFire);
              }
              else if ((double) Rando.Float(1000f) < (double) materialThing.flammable * 1000.0 && (smallFire.whoWait == null || duck != smallFire.whoWait))
                materialThing.Burn(smallFire.position + new Vec2(0.0f, 4f), (Thing) smallFire);
              materialThing.DoHeatUp(0.05f, smallFire.position);
            }
          }
        }
      }
      foreach (FluidPuddle fluidPuddle in Level.current.things[typeof (FluidPuddle)])
      {
        if ((double) fluidPuddle.data.flammable <= 0.5)
          fluidPuddle.onFire = false;
        else if (fluidPuddle.onFire && (double) fluidPuddle.fireID == (double) FireManager._curUpdateID && (double) fluidPuddle.alpha > 0.5)
        {
          foreach (MaterialThing materialThing in Level.CheckRectAll<MaterialThing>(fluidPuddle.topLeft + new Vec2(0.0f, -4f), fluidPuddle.topRight + new Vec2(0.0f, 2f)))
          {
            if (materialThing != fluidPuddle)
            {
              if ((!(materialThing is Duck duck) || (double) duck.slideBuildup <= 0.0) && (double) Rando.Float(1000f) < (double) materialThing.flammable * 1000.0)
                materialThing.Burn(fluidPuddle.position + new Vec2(0.0f, 4f), (Thing) fluidPuddle);
              materialThing.DoHeatUp(0.05f, fluidPuddle.position);
            }
          }
        }
        else if (!fluidPuddle.onFire)
        {
          Rectangle rectangle = fluidPuddle.rectangle;
          foreach (Spark spark in Level.current.things[typeof (Spark)])
          {
            if ((double) spark.x > (double) rectangle.x && (double) spark.x < (double) rectangle.x + (double) rectangle.width && ((double) spark.y > (double) rectangle.y && (double) spark.y < (double) rectangle.y + (double) rectangle.height))
            {
              fluidPuddle.Burn(fluidPuddle.position, (Thing) spark);
              break;
            }
          }
        }
      }
      foreach (ExtinguisherSmoke extinguisherSmoke in Level.current.things[typeof (ExtinguisherSmoke)])
      {
        if (extinguisherSmoke.smokeID == FireManager._curUpdateID)
        {
          foreach (SmallFire smallFire in Level.CheckCircleAll<SmallFire>(extinguisherSmoke.position + new Vec2(0.0f, -8f), 12f))
          {
            if ((double) extinguisherSmoke.scale.x > 1.0)
              smallFire.SuckLife(10f);
          }
          foreach (MaterialThing materialThing in Level.CheckCircleAll<MaterialThing>(extinguisherSmoke.position + new Vec2(0.0f, -8f), 4f))
          {
            if ((double) extinguisherSmoke.scale.x > 1.0)
              materialThing.spreadExtinguisherSmoke = 1f;
            if (materialThing.onFire && (double) Rando.Float(1000f) > (double) materialThing.flammable * 650.0)
              materialThing.Extinquish();
          }
        }
      }
      ++FireManager._curUpdateID;
      if (FireManager._curUpdateID <= 20)
        return;
      FireManager._curUpdateID = 0;
    }
  }
}
