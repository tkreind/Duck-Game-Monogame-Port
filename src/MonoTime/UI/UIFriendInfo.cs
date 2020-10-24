// Decompiled with JetBrains decompiler
// Type: DuckGame.UIFriendInfo
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class UIFriendInfo : UIMenuItem
  {
    private UIMenu _rootMenu;
    private Sprite _avatar;

    public UIFriendInfo(User friend, UIMenu rootMenu)
      : base(" " + friend.name)
    {
      byte[] avatarSmall = friend.avatarSmall;
      if (avatarSmall != null)
      {
        Texture2D texture2D = new Texture2D(DuckGame.Graphics.device, 32, 32);
        texture2D.SetData<byte>(avatarSmall);
        this._avatar = new Sprite((Tex2D) texture2D);
        this._avatar.CenterOrigin();
      }
      this._rootMenu = rootMenu;
      this._collisionSize.y = 14f;
      this._textElement.SetFont(new BitmapFont("smallBiosFont", 7, 6));
      this._textElement.text = "  " + friend.name + "\n  |LIME|WANTS TO PLAY";
    }

    public override void Activate(string trigger)
    {
    }

    public override void Update() => base.Update();

    public override void Draw()
    {
      DuckGame.Graphics.DrawRect(this.leftSection.topLeft, this.rightSection.bottomRight, Colors.BlueGray, this.depth - 1);
      if (this._avatar != null)
      {
        this._avatar.depth = this.depth + 2;
        this._avatar.scale = new Vec2(0.25f);
        DuckGame.Graphics.Draw(this._avatar, (float) ((double) this.leftSection.left + (double) this._avatar.width * (double) this._avatar.scale.x / 2.0 + 6.0), this.y + 3f);
      }
      base.Draw();
    }
  }
}
