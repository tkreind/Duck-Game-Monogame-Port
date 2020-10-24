// Decompiled with JetBrains decompiler
// Type: DuckGame.Keyboard
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Keyboard : InputDevice
  {
    private static KeyboardState _keyState;
    private static KeyboardState _keyStatePrev;
    private static bool _keyboardPress = false;
    private static int _lastKeyCount = 0;
    private static int _flipper = 0;
    public static string keyString = "";
    private Dictionary<int, string> _triggerNames;
    private static Dictionary<int, Sprite> _triggerImages;
    private static bool _repeat = false;
    private List<Keys> _repeatList = new List<Keys>();
    private List<Keyboard.RepeatKey> _repeatingKeys = new List<Keyboard.RepeatKey>();
    private static bool ignoreCore = false;

    public Keyboard(string name, int index)
      : base(index)
    {
      this._name = "keyboard";
      this._productName = name;
      this._productGUID = "";
    }

    public override Dictionary<int, string> GetTriggerNames()
    {
      if (this._triggerNames == null)
      {
        this._triggerNames = new Dictionary<int, string>();
        foreach (Keys key in Enum.GetValues(typeof (Keys)).Cast<Keys>())
        {
          char ch = Keyboard.KeyToChar(key);
          if (ch == ' ')
          {
            switch (key)
            {
              case Keys.Back:
                this._triggerNames[(int) key] = "BACK";
                continue;
              case Keys.Tab:
                this._triggerNames[(int) key] = "TAB";
                continue;
              case Keys.Enter:
                this._triggerNames[(int) key] = "ENTER";
                continue;
              case Keys.Escape:
                this._triggerNames[(int) key] = "ESC";
                continue;
              case Keys.Space:
                this._triggerNames[(int) key] = "SPACE";
                continue;
              case Keys.PageUp:
                this._triggerNames[(int) key] = "PGUP";
                continue;
              case Keys.PageDown:
                this._triggerNames[(int) key] = "PGDN";
                continue;
              case Keys.End:
                this._triggerNames[(int) key] = "END";
                continue;
              case Keys.Home:
                this._triggerNames[(int) key] = "HOME";
                continue;
              case Keys.Left:
                this._triggerNames[(int) key] = "LEFT";
                continue;
              case Keys.Up:
                this._triggerNames[(int) key] = "UP";
                continue;
              case Keys.Right:
                this._triggerNames[(int) key] = "RIGHT";
                continue;
              case Keys.Down:
                this._triggerNames[(int) key] = "DOWN";
                continue;
              case Keys.Insert:
                this._triggerNames[(int) key] = "INSRT";
                continue;
              case Keys.F1:
                this._triggerNames[(int) key] = "F1";
                continue;
              case Keys.F2:
                this._triggerNames[(int) key] = "F2";
                continue;
              case Keys.F3:
                this._triggerNames[(int) key] = "F3";
                continue;
              case Keys.F4:
                this._triggerNames[(int) key] = "F4";
                continue;
              case Keys.F5:
                this._triggerNames[(int) key] = "F5";
                continue;
              case Keys.F6:
                this._triggerNames[(int) key] = "F6";
                continue;
              case Keys.F7:
                this._triggerNames[(int) key] = "F7";
                continue;
              case Keys.F8:
                this._triggerNames[(int) key] = "F8";
                continue;
              case Keys.F9:
                this._triggerNames[(int) key] = "F9";
                continue;
              case Keys.F10:
                this._triggerNames[(int) key] = "F10";
                continue;
              case Keys.F11:
                this._triggerNames[(int) key] = "F11";
                continue;
              case Keys.F12:
                this._triggerNames[(int) key] = "F12";
                continue;
              case Keys.LeftShift:
                this._triggerNames[(int) key] = "LSHFT";
                continue;
              case Keys.RightShift:
                this._triggerNames[(int) key] = "RSHFT";
                continue;
              case Keys.LeftControl:
                this._triggerNames[(int) key] = "LCTRL";
                continue;
              case Keys.RightControl:
                this._triggerNames[(int) key] = "RCTRL";
                continue;
              case Keys.LeftAlt:
                this._triggerNames[(int) key] = "LALT";
                continue;
              case Keys.RightAlt:
                this._triggerNames[(int) key] = "RALT";
                continue;
              default:
                continue;
            }
          }
          else
            this._triggerNames[(int) key] = string.Concat((object) ch);
        }
      }
      return this._triggerNames;
    }

    public static void InitTriggerImages()
    {
      if (Keyboard._triggerImages != null)
        return;
      Keyboard._triggerImages = new Dictionary<int, Sprite>();
      Keyboard._triggerImages[9999] = new Sprite("buttons/keyboard/arrows");
      foreach (Keys key1 in Enum.GetValues(typeof (Keys)).Cast<Keys>())
      {
        char key2 = Keyboard.KeyToChar(key1);
        if (key2 == ' ')
        {
          switch (key1)
          {
            case Keys.Back:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/back");
              continue;
            case Keys.Tab:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/tab");
              continue;
            case Keys.Enter:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/enter");
              continue;
            case Keys.Escape:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/escape");
              continue;
            case Keys.Space:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/space");
              continue;
            case Keys.PageUp:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/pgup");
              continue;
            case Keys.PageDown:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/pgdown");
              continue;
            case Keys.End:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/end");
              continue;
            case Keys.Home:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/home");
              continue;
            case Keys.Left:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/leftKey");
              continue;
            case Keys.Up:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/upKey");
              continue;
            case Keys.Right:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/rightKey");
              continue;
            case Keys.Down:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/downKey");
              continue;
            case Keys.Insert:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/insert");
              continue;
            case Keys.F1:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f1");
              continue;
            case Keys.F2:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f2");
              continue;
            case Keys.F3:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f3");
              continue;
            case Keys.F4:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f4");
              continue;
            case Keys.F5:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f5");
              continue;
            case Keys.F6:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f6");
              continue;
            case Keys.F7:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f7");
              continue;
            case Keys.F8:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f8");
              continue;
            case Keys.F9:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f9");
              continue;
            case Keys.F10:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f10");
              continue;
            case Keys.F11:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f11");
              continue;
            case Keys.F12:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/f12");
              continue;
            case Keys.LeftShift:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/shift");
              continue;
            case Keys.RightShift:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/shift");
              continue;
            case Keys.LeftControl:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/control");
              continue;
            case Keys.RightControl:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/control");
              continue;
            case Keys.LeftAlt:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/alt");
              continue;
            case Keys.RightAlt:
              Keyboard._triggerImages[(int) key1] = new Sprite("buttons/keyboard/alt");
              continue;
            default:
              continue;
          }
        }
        else
          Keyboard._triggerImages[(int) key1] = (Sprite) new KeyImage(key2);
      }
    }

    public override Sprite GetMapImage(int map)
    {
      Sprite sprite = (Sprite) null;
      Keyboard._triggerImages.TryGetValue(map, out sprite);
      return sprite;
    }

    public static char KeyToChar(Keys key, bool caps = true, bool shift = false)
    {
      if (caps)
      {
        switch (key)
        {
          case Keys.D0:
            return '0';
          case Keys.D1:
            return '1';
          case Keys.D2:
            return '2';
          case Keys.D3:
            return '3';
          case Keys.D4:
            return '4';
          case Keys.D5:
            return '5';
          case Keys.D6:
            return '6';
          case Keys.D7:
            return '7';
          case Keys.D8:
            return '8';
          case Keys.D9:
            return '9';
          case Keys.A:
            return 'A';
          case Keys.B:
            return 'B';
          case Keys.C:
            return 'C';
          case Keys.D:
            return 'D';
          case Keys.E:
            return 'E';
          case Keys.F:
            return 'F';
          case Keys.G:
            return 'G';
          case Keys.H:
            return 'H';
          case Keys.I:
            return 'I';
          case Keys.J:
            return 'J';
          case Keys.K:
            return 'K';
          case Keys.L:
            return 'L';
          case Keys.M:
            return 'M';
          case Keys.N:
            return 'N';
          case Keys.O:
            return 'O';
          case Keys.P:
            return 'P';
          case Keys.Q:
            return 'Q';
          case Keys.R:
            return 'R';
          case Keys.S:
            return 'S';
          case Keys.T:
            return 'T';
          case Keys.U:
            return 'U';
          case Keys.V:
            return 'V';
          case Keys.W:
            return 'W';
          case Keys.X:
            return 'X';
          case Keys.Y:
            return 'Y';
          case Keys.Z:
            return 'Z';
          case Keys.NumPad0:
            return '0';
          case Keys.NumPad1:
            return '1';
          case Keys.NumPad2:
            return '2';
          case Keys.NumPad3:
            return '3';
          case Keys.NumPad4:
            return '4';
          case Keys.NumPad5:
            return '5';
          case Keys.NumPad6:
            return '6';
          case Keys.NumPad7:
            return '7';
          case Keys.NumPad8:
            return '8';
          case Keys.NumPad9:
            return '9';
          case Keys.OemSemicolon:
            return ';';
          case Keys.OemPlus:
            return '=';
          case Keys.OemComma:
            return ',';
          case Keys.OemMinus:
            return '-';
          case Keys.OemPeriod:
            return '.';
          case Keys.OemQuestion:
            return '/';
          case Keys.OemTilde:
            return '~';
          case Keys.OemOpenBrackets:
            return '[';
          case Keys.OemPipe:
            return '\\';
          case Keys.OemCloseBrackets:
            return ']';
          case Keys.OemQuotes:
            return '\'';
          case Keys.OemBackslash:
            return '\\';
        }
      }
      else if (shift)
      {
        switch (key)
        {
          case Keys.D0:
            return ')';
          case Keys.D1:
            return '!';
          case Keys.D2:
            return '@';
          case Keys.D3:
            return '#';
          case Keys.D4:
            return '$';
          case Keys.D5:
            return '%';
          case Keys.D6:
            return '^';
          case Keys.D7:
            return '&';
          case Keys.D8:
            return '*';
          case Keys.D9:
            return '(';
          case Keys.A:
            return 'A';
          case Keys.B:
            return 'B';
          case Keys.C:
            return 'C';
          case Keys.D:
            return 'D';
          case Keys.E:
            return 'E';
          case Keys.F:
            return 'F';
          case Keys.G:
            return 'G';
          case Keys.H:
            return 'H';
          case Keys.I:
            return 'I';
          case Keys.J:
            return 'J';
          case Keys.K:
            return 'K';
          case Keys.L:
            return 'L';
          case Keys.M:
            return 'M';
          case Keys.N:
            return 'N';
          case Keys.O:
            return 'O';
          case Keys.P:
            return 'P';
          case Keys.Q:
            return 'Q';
          case Keys.R:
            return 'R';
          case Keys.S:
            return 'S';
          case Keys.T:
            return 'T';
          case Keys.U:
            return 'U';
          case Keys.V:
            return 'V';
          case Keys.W:
            return 'W';
          case Keys.X:
            return 'X';
          case Keys.Y:
            return 'Y';
          case Keys.Z:
            return 'Z';
          case Keys.NumPad0:
            return '0';
          case Keys.NumPad1:
            return '1';
          case Keys.NumPad2:
            return '2';
          case Keys.NumPad3:
            return '3';
          case Keys.NumPad4:
            return '4';
          case Keys.NumPad5:
            return '5';
          case Keys.NumPad6:
            return '6';
          case Keys.NumPad7:
            return '7';
          case Keys.NumPad8:
            return '8';
          case Keys.NumPad9:
            return '9';
          case Keys.OemSemicolon:
            return ':';
          case Keys.OemPlus:
            return '+';
          case Keys.OemComma:
            return '<';
          case Keys.OemMinus:
            return '_';
          case Keys.OemPeriod:
            return '>';
          case Keys.OemQuestion:
            return '?';
          case Keys.OemTilde:
            return '~';
          case Keys.OemOpenBrackets:
            return '{';
          case Keys.OemPipe:
            return '|';
          case Keys.OemCloseBrackets:
            return '}';
          case Keys.OemQuotes:
            return '"';
          case Keys.OemBackslash:
            return '|';
        }
      }
      else
      {
        switch (key)
        {
          case Keys.D0:
            return '0';
          case Keys.D1:
            return '1';
          case Keys.D2:
            return '2';
          case Keys.D3:
            return '3';
          case Keys.D4:
            return '4';
          case Keys.D5:
            return '5';
          case Keys.D6:
            return '6';
          case Keys.D7:
            return '7';
          case Keys.D8:
            return '8';
          case Keys.D9:
            return '9';
          case Keys.A:
            return 'a';
          case Keys.B:
            return 'b';
          case Keys.C:
            return 'c';
          case Keys.D:
            return 'd';
          case Keys.E:
            return 'e';
          case Keys.F:
            return 'f';
          case Keys.G:
            return 'g';
          case Keys.H:
            return 'h';
          case Keys.I:
            return 'i';
          case Keys.J:
            return 'j';
          case Keys.K:
            return 'k';
          case Keys.L:
            return 'l';
          case Keys.M:
            return 'm';
          case Keys.N:
            return 'n';
          case Keys.O:
            return 'o';
          case Keys.P:
            return 'p';
          case Keys.Q:
            return 'q';
          case Keys.R:
            return 'r';
          case Keys.S:
            return 's';
          case Keys.T:
            return 't';
          case Keys.U:
            return 'u';
          case Keys.V:
            return 'v';
          case Keys.W:
            return 'w';
          case Keys.X:
            return 'x';
          case Keys.Y:
            return 'y';
          case Keys.Z:
            return 'z';
          case Keys.NumPad0:
            return '0';
          case Keys.NumPad1:
            return '1';
          case Keys.NumPad2:
            return '2';
          case Keys.NumPad3:
            return '3';
          case Keys.NumPad4:
            return '4';
          case Keys.NumPad5:
            return '5';
          case Keys.NumPad6:
            return '6';
          case Keys.NumPad7:
            return '7';
          case Keys.NumPad8:
            return '8';
          case Keys.NumPad9:
            return '9';
          case Keys.OemSemicolon:
            return ';';
          case Keys.OemPlus:
            return '=';
          case Keys.OemComma:
            return ',';
          case Keys.OemMinus:
            return '-';
          case Keys.OemPeriod:
            return '.';
          case Keys.OemQuestion:
            return '/';
          case Keys.OemTilde:
            return '~';
          case Keys.OemOpenBrackets:
            return '[';
          case Keys.OemPipe:
            return '\\';
          case Keys.OemCloseBrackets:
            return ']';
          case Keys.OemQuotes:
            return '\'';
          case Keys.OemBackslash:
            return '\\';
        }
      }
      return ' ';
    }

    public static bool repeat
    {
      get => Keyboard._repeat;
      set => Keyboard._repeat = value;
    }

    public override void Update()
    {
      if (!Graphics.inFocus)
        return;
      if (Keyboard._flipper == 0)
      {
        Keyboard._keyStatePrev = Keyboard._keyState;
        Keyboard._keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        Keyboard._keyboardPress = false;
        int num = ((IEnumerable<Microsoft.Xna.Framework.Input.Keys>) Keyboard._keyState.GetPressedKeys()).Count<Microsoft.Xna.Framework.Input.Keys>();
        if (num != Keyboard._lastKeyCount && num != 0)
          Keyboard._keyboardPress = true;
        Keyboard._lastKeyCount = num;
        this.updateKeyboardString();
        Keyboard._flipper = 1;
      }
      else
        --Keyboard._flipper;
      this._repeatList.Clear();
      Keyboard.ignoreCore = true;
      if (Keyboard._repeat)
      {
        using (IEnumerator<Keys> enumerator = Enum.GetValues(typeof (Keys)).Cast<Keys>().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Keys k = enumerator.Current;
            if (this.MapPressed((int) k, false) && this._repeatingKeys.FirstOrDefault<Keyboard.RepeatKey>((Func<Keyboard.RepeatKey, bool>) (x => x.key == k)) == null)
              this._repeatingKeys.Add(new Keyboard.RepeatKey()
              {
                key = k,
                repeatTime = 2f
              });
          }
        }
        List<Keyboard.RepeatKey> repeatKeyList = new List<Keyboard.RepeatKey>();
        foreach (Keyboard.RepeatKey repeatingKey in this._repeatingKeys)
        {
          repeatingKey.repeatTime -= 0.1f;
          bool flag = this.MapDown((int) repeatingKey.key, false);
          if (flag && (double) repeatingKey.repeatTime < 0.0)
            this._repeatList.Add(repeatingKey.key);
          if ((double) repeatingKey.repeatTime <= 0.0 && flag)
            repeatingKey.repeatTime = 0.25f;
          if (!flag)
            repeatKeyList.Add(repeatingKey);
        }
        foreach (Keyboard.RepeatKey repeatKey in repeatKeyList)
          this._repeatingKeys.Remove(repeatKey);
      }
      Keyboard.ignoreCore = false;
    }

    private void updateKeyboardString()
    {
      Keyboard.ignoreCore = true;
      bool shift = Keyboard.Down(Keys.LeftShift) || Keyboard.Down(Keys.RightShift);
      bool capsLock = Console.CapsLock;
      foreach (Microsoft.Xna.Framework.Input.Keys pressedKey in Keyboard._keyState.GetPressedKeys())
      {
        if (this.MapPressed((int) pressedKey, false))
        {
          switch (pressedKey)
          {
            case Microsoft.Xna.Framework.Input.Keys.Back:
              if (Keyboard.keyString.Length > 0)
              {
                Keyboard.keyString = Keyboard.keyString.Remove(Keyboard.keyString.Length - 1, 1);
                continue;
              }
              continue;
            case Microsoft.Xna.Framework.Input.Keys.Space:
              Keyboard.keyString = Keyboard.keyString.Insert(Keyboard.keyString.Length, " ");
              continue;
            default:
              char ch = Keyboard.KeyToChar((Keys) pressedKey, capsLock, shift);
              if (ch != ' ')
              {
                Keyboard.keyString += (string) (object) ch;
                continue;
              }
              continue;
          }
        }
      }
      Keyboard.ignoreCore = false;
    }

    public override bool MapPressed(int mapping, bool any = false)
    {
      if (!Keyboard.ignoreCore && (DevConsole.core.open || DuckNetwork.enteringText || Editor.enteringText))
        return false;
      Keys key = (Keys) mapping;
      return Keyboard.Pressed(key, any) || this._repeatList.Contains(key);
    }

    public static bool Pressed(Keys key, bool any = false) => !DuckGame.Input.ignoreInput && (any && Keyboard._keyboardPress || Keyboard._keyState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys) key) && !Keyboard._keyStatePrev.IsKeyDown((Microsoft.Xna.Framework.Input.Keys) key));

    public override bool MapReleased(int mapping) => (Keyboard.ignoreCore || !DevConsole.core.open && !DuckNetwork.enteringText && !Editor.enteringText) && Keyboard.Released((Keys) mapping);

    public static bool Released(Keys key) => !DuckGame.Input.ignoreInput && !Keyboard._keyState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys) key) && Keyboard._keyStatePrev.IsKeyDown((Microsoft.Xna.Framework.Input.Keys) key);

    public override bool MapDown(int mapping, bool any = false) => (Keyboard.ignoreCore || !DevConsole.core.open && !DuckNetwork.enteringText && !Editor.enteringText) && Keyboard.Down((Keys) mapping);

    public static bool Down(Keys key) => !DuckGame.Input.ignoreInput && Keyboard._keyState.IsKeyDown((Microsoft.Xna.Framework.Input.Keys) key);

    public class RepeatKey
    {
      public Keys key;
      public float repeatTime;
    }
  }
}
