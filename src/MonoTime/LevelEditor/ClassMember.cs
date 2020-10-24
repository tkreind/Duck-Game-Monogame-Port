// Decompiled with JetBrains decompiler
// Type: DuckGame.ClassMember
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Reflection;

namespace DuckGame
{
  public class ClassMember
  {
    private FieldInfo _fieldInfo;
    private PropertyInfo _propertyInfo;
    private System.Type _declaringType;
    private bool _isPrivate;
    private string _name;
    private AccessorInfo _accessor;

    public FieldInfo field => this._fieldInfo;

    public PropertyInfo property => this._propertyInfo;

    public System.Type declaringType => this._declaringType;

    public bool isPrivate => this._isPrivate;

    public string name => this._name;

    public System.Type type => this._fieldInfo != (FieldInfo) null ? this._fieldInfo.FieldType : this._propertyInfo.PropertyType;

    public object GetValue(object instance)
    {
      if (this._accessor == null)
        this._accessor = Editor.GetAccessorInfo(this._declaringType, this._name, this._fieldInfo, this._propertyInfo);
      return this._accessor.getAccessor(instance);
    }

    public void SetValue(object instance, object value)
    {
      if (this._accessor == null)
        this._accessor = Editor.GetAccessorInfo(this._declaringType, this._name, this._fieldInfo, this._propertyInfo);
      this._accessor.setAccessor(instance, value);
    }

    public ClassMember(string n, System.Type declaringTp, FieldInfo field)
    {
      this._fieldInfo = field;
      this._name = n;
      this._declaringType = declaringTp;
      this._isPrivate = field.IsPrivate;
    }

    public ClassMember(string n, System.Type declaringTp, PropertyInfo property)
    {
      this._propertyInfo = property;
      this._name = n;
      this._declaringType = declaringTp;
    }
  }
}
