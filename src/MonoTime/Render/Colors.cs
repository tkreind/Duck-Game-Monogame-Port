// Decompiled with JetBrains decompiler
// Type: DuckGame.Colors
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Colors
  {
    public static Color MenuOption = new Color(235, 137, 51);
    public static Color Bronze = new Color(216, 105, 65);
    public static Color Silver = new Color(191, 191, 191);
    public static Color Gold = new Color(247, 224, 90);
    public static Color Platinum = new Color(178, 220, 239);
    public static Color Developer = new Color(222, 32, 45);
    public static Color DGYellow = new Color(247, 224, 90);
    public static Color DGOrange = new Color(235, 136, 49);
    public static Color DGPink = new Color(235, 190, 242);
    public static Color DGEgg = new Color(245, 235, 211);
    public static Color DGBlue = new Color(49, 162, 242);
    public static Color DGGreen = new Color(163, 206, 39);
    public static Color DGRed = new Color(192, 32, 45);
    public static Color BlueGray = new Color(47, 73, 79);
    public static Color DGPurple = new Color(179, 54, 242);
    public static Color Transparent = new Color(0, 0, 0, 0);
    public static Color SuperDarkBlueGray = new Color(8, 12, 16);

    public static Color ParseColor(string color)
    {
      Color color1 = Color.Transparent;
      if (color == "RED")
        color1 = Color.Red;
      else if (color == "WHITE")
        color1 = Color.White;
      else if (color == "BLACK")
        color1 = Color.Black;
      else if (color == "DARKNESS")
        color1 = new Color(10, 10, 10);
      else if (color == "BLUE")
        color1 = Color.Blue;
      else if (color == "DGBLUE")
        color1 = Colors.DGBlue;
      else if (color == "DGRED")
        color1 = Colors.DGRed;
      else if (color == "DGGREEN")
        color1 = Colors.DGGreen;
      else if (color == "DGYELLOW")
        color1 = Colors.DGYellow;
      else if (color == "DGORANGE")
        color1 = new Color(235, 136, 49);
      else if (color == "ORANGE")
        color1 = new Color(235, 137, 51);
      else if (color == "MENUORANGE")
        color1 = Colors.MenuOption;
      else if (color == "YELLOW")
        color1 = new Color(247, 224, 90);
      else if (color == "GREEN")
        color1 = Color.LimeGreen;
      else if (color == "LIME")
        color1 = Color.LimeGreen;
      else if (color == "GRAY")
        color1 = new Color(70, 70, 70);
      else if (color == "LIGHTGRAY")
        color1 = new Color(96, 119, 124);
      else if (color == "CREDITSGRAY")
        color1 = new Color(137, 159, 164);
      else if (color == "BLUEGRAY")
        color1 = Colors.BlueGray;
      else if (color == "PINK")
        color1 = new Color(246, 88, 191);
      else if (color == "PURPLE" || color == "DGPURPLE")
        color1 = new Color(115, 48, 242);
      return color1;
    }
  }
}
