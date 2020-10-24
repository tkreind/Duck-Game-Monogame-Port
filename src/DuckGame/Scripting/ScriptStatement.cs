// Decompiled with JetBrains decompiler
// Type: DuckGame.ScriptStatement
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class ScriptStatement
  {
    public object data;
    public object leftObject;
    public object rightObject;
    public ScriptOperator op;
    public string functionName;
    public string error;
    public string remainingStatementString;

    public static ScriptStatement Parse(
      string statement,
      object left = null,
      object right = null,
      ScriptOperator operat = ScriptOperator.None,
      string func = null,
      bool isChild = false)
    {
      ScriptStatement scriptStatement1 = new ScriptStatement();
      scriptStatement1.leftObject = left;
      scriptStatement1.rightObject = right;
      scriptStatement1.op = operat;
      scriptStatement1.functionName = func;
      int num = 1;
      if (!isChild)
        ++num;
      string str = "";
      ScriptOperator operat1 = ScriptOperator.None;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      while (statement.Length > 0)
      {
        char c = statement[0];
        statement = statement.Remove(0, 1);
        if (!flag3 && char.IsNumber(c))
        {
          if (str.Length > 0 && !flag1)
          {
            if (flag4 && str == "-")
            {
              flag4 = false;
            }
            else
            {
              scriptStatement1.error = "Found unexpected number.";
              break;
            }
          }
          flag1 = true;
        }
        else if (c == '.' && !flag3)
        {
          if (str.Length > 0 && !flag1)
          {
            scriptStatement1.error = "Found unexpected Period.";
            break;
          }
          flag2 = true;
        }
        else if (c == ' ' || c == '(' || c == ')')
        {
          flag5 = true;
          if (flag3 && (str == "and" || str == "or"))
          {
            flag3 = false;
            flag4 = true;
          }
          if (scriptStatement1.data == null)
          {
            if (flag3)
              scriptStatement1.data = (object) str;
            else if (flag2)
              scriptStatement1.data = (object) Change.ToSingle((object) str);
            else if (flag1)
              scriptStatement1.data = (object) Convert.ToInt32(str);
          }
          if (flag4)
          {
            if (str == "+")
              operat1 = ScriptOperator.Plus;
            else if (str == "-")
              operat1 = ScriptOperator.Minus;
            else if (str == "*")
              operat1 = ScriptOperator.Multiply;
            else if (str == "/")
              operat1 = ScriptOperator.Divide;
            else if (str == "==")
              operat1 = ScriptOperator.IsEqual;
            else if (str == "!=")
              operat1 = ScriptOperator.IsNotEqual;
            else if (str == ">")
              operat1 = ScriptOperator.GreaterThan;
            else if (str == "<")
              operat1 = ScriptOperator.LessThan;
            else if (str == "&&")
              operat1 = ScriptOperator.And;
            else if (str == "and")
              operat1 = ScriptOperator.And;
            else if (str == "or")
              operat1 = ScriptOperator.Or;
            if (scriptStatement1.op == ScriptOperator.None)
              scriptStatement1.op = operat1;
          }
          switch (c)
          {
            case '(':
              ScriptStatement scriptStatement2 = ScriptStatement.Parse(statement, func: (flag3 ? str : (string) null), isChild: true);
              statement = scriptStatement2.remainingStatementString;
              if (scriptStatement2.error != null)
              {
                scriptStatement1.error = scriptStatement2.error;
                goto label_76;
              }
              else
              {
                scriptStatement1.data = (object) scriptStatement2;
                break;
              }
            case ')':
              --num;
              break;
          }
          if (scriptStatement1.data != null)
          {
            object obj = scriptStatement1.data;
            scriptStatement1.data = (object) null;
            if (operat1 > ScriptOperator.COMPARATORS)
            {
              ScriptStatement scriptStatement3 = ScriptStatement.Parse(statement, obj, isChild: true);
              obj = (object) scriptStatement3;
              statement = scriptStatement3.remainingStatementString;
            }
            if (scriptStatement1.leftObject == null)
            {
              scriptStatement1.leftObject = obj;
            }
            else
            {
              if (scriptStatement1.rightObject != null)
                return ScriptStatement.Parse(statement, (object) scriptStatement1, obj, operat1, isChild: true);
              scriptStatement1.rightObject = obj;
            }
          }
          str = "";
          flag3 = false;
          flag2 = false;
          flag1 = false;
          flag4 = false;
          if (num <= 0)
            break;
        }
        else if (!flag3 && c == '+' || (c == '-' || c == '*') || (c == '/' || c == '=' || (c == '<' || c == '!')) || (c == '>' || c == '&'))
        {
          if (str.Length > 0 && !flag4)
          {
            scriptStatement1.error = "Found unexpected operator.";
            break;
          }
          flag4 = true;
        }
        else if (c == '"')
        {
          flag5 = true;
          flag3 = true;
        }
        else
        {
          if (str.Length > 0 && !flag3)
          {
            scriptStatement1.error = "Found unexpected letter.";
            break;
          }
          flag3 = true;
        }
        if (!flag5)
          str += (string) (object) c;
        flag5 = false;
      }
label_76:
      scriptStatement1.remainingStatementString = statement;
      return scriptStatement1;
    }

    public object result
    {
      get
      {
        object obj1 = (object) null;
        object obj2 = !(this.leftObject is ScriptStatement leftObject) ? this.leftObject : leftObject.result;
        object obj3 = !(this.rightObject is ScriptStatement rightObject) ? this.rightObject : (obj2 is bool || this.op <= ScriptOperator.IsNotEqual ? rightObject.result : (object) null);
        if (obj2 != null)
        {
          if (obj3 != null)
          {
            switch (obj2)
            {
              case float _:
              case int _ when obj3 is float || obj3 is int:
                if (obj2 is float || obj3 is float)
                {
                  float single1 = Change.ToSingle(obj2);
                  float single2 = Change.ToSingle(obj3);
                  if (this.op == ScriptOperator.Plus)
                  {
                    obj1 = (object) (float) ((double) single1 + (double) single2);
                    break;
                  }
                  if (this.op == ScriptOperator.Minus)
                  {
                    obj1 = (object) (float) ((double) single1 - (double) single2);
                    break;
                  }
                  if (this.op == ScriptOperator.Multiply)
                  {
                    obj1 = (object) (float) ((double) single1 * (double) single2);
                    break;
                  }
                  if (this.op == ScriptOperator.Divide)
                  {
                    obj1 = (object) (float) ((double) single2 != 0.0 ? (double) single1 / (double) single2 : 0.0);
                    break;
                  }
                  if (this.op == ScriptOperator.IsEqual)
                  {
                    obj1 = (object) ((double) single1 == (double) single2);
                    break;
                  }
                  if (this.op == ScriptOperator.IsNotEqual)
                  {
                    obj1 = (object) ((double) single1 != (double) single2);
                    break;
                  }
                  if (this.op == ScriptOperator.GreaterThan)
                  {
                    obj1 = (object) ((double) single1 > (double) single2);
                    break;
                  }
                  if (this.op == ScriptOperator.LessThan)
                  {
                    obj1 = (object) ((double) single1 < (double) single2);
                    break;
                  }
                  break;
                }
                int num1 = (int) obj2;
                int num2 = (int) obj3;
                if (this.op == ScriptOperator.Plus)
                {
                  obj1 = (object) (num1 + num2);
                  break;
                }
                if (this.op == ScriptOperator.Minus)
                {
                  obj1 = (object) (num1 - num2);
                  break;
                }
                if (this.op == ScriptOperator.Multiply)
                {
                  obj1 = (object) (num1 * num2);
                  break;
                }
                if (this.op == ScriptOperator.Divide)
                {
                  obj1 = (object) (num2 != 0 ? num1 / num2 : 0);
                  break;
                }
                if (this.op == ScriptOperator.IsEqual)
                {
                  obj1 = (object) (num1 == num2);
                  break;
                }
                if (this.op == ScriptOperator.IsNotEqual)
                {
                  obj1 = (object) (num1 != num2);
                  break;
                }
                if (this.op == ScriptOperator.GreaterThan)
                {
                  obj1 = (object) (num1 > num2);
                  break;
                }
                if (this.op == ScriptOperator.LessThan)
                {
                  obj1 = (object) (num1 < num2);
                  break;
                }
                break;
              case string _ when obj3 is string:
                string str1 = (string) obj2;
                string str2 = (string) obj3;
                if (this.op == ScriptOperator.Plus)
                {
                  obj1 = (object) (str1 + str2);
                  break;
                }
                if (this.op == ScriptOperator.IsEqual)
                {
                  obj1 = (object) (str1 == str2);
                  break;
                }
                if (this.op == ScriptOperator.IsNotEqual)
                {
                  obj1 = (object) (str1 != str2);
                  break;
                }
                break;
              case bool _ when obj3 is bool flag2:
                bool flag1 = (bool) obj2;
                if (this.op == ScriptOperator.IsEqual)
                {
                  obj1 = (object) (flag1 == flag2);
                  break;
                }
                if (this.op == ScriptOperator.IsNotEqual)
                {
                  obj1 = (object) (flag1 != flag2);
                  break;
                }
                if (this.op == ScriptOperator.And)
                {
                  obj1 = (object) (bool) (!flag1 ? false : (flag2 ? true : false));
                  break;
                }
                if (this.op == ScriptOperator.Or)
                {
                  obj1 = (object) (bool) (flag1 ? true : (flag2 ? true : false));
                  break;
                }
                break;
            }
          }
          else
            obj1 = obj2;
        }
        else if (obj3 != null)
          obj1 = obj3;
        if (this.functionName != null)
        {
          object obj4;
          switch (obj1)
          {
            case string _:
              obj4 = Script.CallMethod(this.functionName, (object) (obj1 as string));
              break;
            case int num3:
              obj4 = Script.CallMethod(this.functionName, (object) num3);
              break;
            case float num3:
              obj4 = Script.CallMethod(this.functionName, (object) num3);
              break;
            default:
              obj4 = Script.CallMethod(this.functionName, (object) null);
              break;
          }
          obj1 = !(obj4 is ScriptObject scriptObject) ? obj4 : scriptObject.objectProperty.GetValue(scriptObject.obj, (object[]) null);
          // TODO fix this decompiler issue
          //if (obj1 is float && scriptObject != null)
          //  obj1 = (object) (float) ((double) (float) obj1 * (scriptObject.negative ? -1.0 : 1.0));
          //if (obj1 is int && scriptObject != null)
          //  obj1 = (object) ((int) obj1 * (scriptObject.negative ? -1 : 1));
        }
        // TODO
        //if (obj3 == null || rightObject == null || rightObject.op <= ScriptOperator.IsNotEqual)
        //  return obj1;
        return new ScriptStatement()
        {
            // TODO
          //leftObject = obj1,
          //rightObject = rightObject.rightObject,
          //op = rightObject.op
        }.result;
      }
    }
  }
}
