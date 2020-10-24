// Decompiled with JetBrains decompiler
// Type: DuckGame.NetworkConsole
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WinFormsGraphicsDevice;

namespace DuckGame
{
  public class NetworkConsole : Form
  {
    private Dictionary<string, RichTextBox> _logBoxes = new Dictionary<string, RichTextBox>();
    private Dictionary<string, Chart> _logCharts = new Dictionary<string, Chart>();
    private Dictionary<string, Dictionary<string, List<Vec2>>> _waitingPoints = new Dictionary<string, Dictionary<string, List<Vec2>>>();
    /// <summary>Required designer variable.</summary>
    private IContainer components;
    private TabControl Tabs;
    private TabPage LogTab;
    private TabControl LogTabs;
    private TabPage tabPage1;
    private TabPage LogTabsAll;
    private TableLayoutPanel AllLogsPanel;
    private TabPage chartsTab;
    private TableLayoutPanel chartPanel;
    private NetGraphControl netGraphControl1;
    private NetGraphControl netGraphControl2;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel toolStripStatusLabel1;
    private NetGraphControl netGraphControl3;

    public NetworkConsole() => this.InitializeComponent();

    public void UpdateGraph(int index, NetGraph target)
    {
      switch (index)
      {
        case 0:
          this.netGraphControl1.UpdateGraph(target);
          break;
        case 1:
          this.netGraphControl2.UpdateGraph(target);
          break;
        case 2:
          this.netGraphControl3.UpdateGraph(target);
          break;
      }
    }

    public void DoIt()
    {
    }

    public void ChartValue(string chart, string valueName, double x, double y, System.Drawing.Color? color = null)
    {
    }

    public void AddChart(string name)
    {
    }

    public void AddLog(string name)
    {
      this.LogTabs.TabPages.Add(name, name);
      RichTextBox richTextBox = new RichTextBox();
      richTextBox.Dock = DockStyle.Fill;
      richTextBox.ReadOnly = true;
      richTextBox.BackColor = System.Drawing.Color.Black;
      this._logBoxes[name] = richTextBox;
      this.LogTabs.TabPages[name].Controls.Add((Control) richTextBox);
      this.AllLogsPanel.Controls.Clear();
      if (this._logBoxes.Count > 0)
      {
        this.AllLogsPanel.Controls.Add((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(0).Value);
        this.AllLogsPanel.SetColumn((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(0).Value, 0);
        this.AllLogsPanel.SetRow((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(0).Value, 0);
      }
      if (this._logBoxes.Count > 1)
      {
        this.AllLogsPanel.Controls.Add((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(1).Value);
        this.AllLogsPanel.SetColumn((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(1).Value, 1);
        this.AllLogsPanel.SetRow((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(1).Value, 0);
      }
      if (this._logBoxes.Count > 2)
      {
        this.AllLogsPanel.Controls.Add((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(2).Value);
        this.AllLogsPanel.SetColumn((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(2).Value, 0);
        this.AllLogsPanel.SetRow((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(2).Value, 1);
      }
      if (this._logBoxes.Count <= 3)
        return;
      this.AllLogsPanel.Controls.Add((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(3).Value);
      this.AllLogsPanel.SetColumn((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(3).Value, 1);
      this.AllLogsPanel.SetRow((Control) this._logBoxes.ElementAt<KeyValuePair<string, RichTextBox>>(3).Value, 1);
    }

    public void Log(string key, string text, System.Drawing.Color? color = null)
    {
      if (!this._logBoxes.ContainsKey(key))
        this.AddLog(key);
      NetworkConsole.output(this._logBoxes[key], text, color);
    }

    public static void output(RichTextBox box, string str, System.Drawing.Color? color = null)
    {
      if (str == null || box == null)
        return;
      if (box.InvokeRequired)
      {
        box.BeginInvoke((Delegate) new NetworkConsole.OutputDelegate(NetworkConsole.output), (object) box, (object) str, (object) color);
      }
      else
      {
        int textLength = box.TextLength;
        box.SelectionStart = box.TextLength;
        box.AppendText(str + "\n");
        box.SelectionStart = textLength;
        box.SelectionLength = box.TextLength - textLength;
        box.SelectionColor = color.HasValue ? color.Value : System.Drawing.Color.Black;
        box.SelectionStart = box.TextLength;
        box.SelectionLength = 0;
        box.SelectionColor = box.ForeColor;
      }
    }

    /// <summary>Clean up any resources being used.</summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (NetworkConsole));
      this.Tabs = new TabControl();
      this.tabPage1 = new TabPage();
      this.LogTab = new TabPage();
      this.LogTabs = new TabControl();
      this.LogTabsAll = new TabPage();
      this.AllLogsPanel = new TableLayoutPanel();
      this.chartsTab = new TabPage();
      this.chartPanel = new TableLayoutPanel();
      this.statusStrip1 = new StatusStrip();
      this.toolStripStatusLabel1 = new ToolStripStatusLabel();
      this.netGraphControl1 = new NetGraphControl();
      this.netGraphControl2 = new NetGraphControl();
      this.netGraphControl3 = new NetGraphControl();
      this.Tabs.SuspendLayout();
      this.LogTab.SuspendLayout();
      this.LogTabs.SuspendLayout();
      this.LogTabsAll.SuspendLayout();
      this.chartsTab.SuspendLayout();
      this.chartPanel.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      this.Tabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Tabs.Controls.Add((Control) this.tabPage1);
      this.Tabs.Controls.Add((Control) this.LogTab);
      this.Tabs.Controls.Add((Control) this.chartsTab);
      this.Tabs.Location = new Point(0, 0);
      this.Tabs.Name = "Tabs";
      this.Tabs.SelectedIndex = 0;
      this.Tabs.Size = new Size(783, 503);
      this.Tabs.TabIndex = 0;
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(775, 477);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "General";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.LogTab.Controls.Add((Control) this.LogTabs);
      this.LogTab.Location = new Point(4, 22);
      this.LogTab.Name = "LogTab";
      this.LogTab.Padding = new Padding(3);
      this.LogTab.Size = new Size(788, 483);
      this.LogTab.TabIndex = 1;
      this.LogTab.Text = "Logs";
      this.LogTab.UseVisualStyleBackColor = true;
      this.LogTabs.Controls.Add((Control) this.LogTabsAll);
      this.LogTabs.Dock = DockStyle.Fill;
      this.LogTabs.Location = new Point(3, 3);
      this.LogTabs.Name = "LogTabs";
      this.LogTabs.SelectedIndex = 0;
      this.LogTabs.Size = new Size(782, 477);
      this.LogTabs.TabIndex = 0;
      this.LogTabsAll.Controls.Add((Control) this.AllLogsPanel);
      this.LogTabsAll.Location = new Point(4, 22);
      this.LogTabsAll.Name = "LogTabsAll";
      this.LogTabsAll.Padding = new Padding(3);
      this.LogTabsAll.Size = new Size(774, 451);
      this.LogTabsAll.TabIndex = 0;
      this.LogTabsAll.Text = "All";
      this.LogTabsAll.UseVisualStyleBackColor = true;
      this.AllLogsPanel.ColumnCount = 2;
      this.AllLogsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.AllLogsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.AllLogsPanel.Dock = DockStyle.Fill;
      this.AllLogsPanel.Location = new Point(3, 3);
      this.AllLogsPanel.Name = "AllLogsPanel";
      this.AllLogsPanel.RowCount = 2;
      this.AllLogsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.AllLogsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.AllLogsPanel.Size = new Size(768, 445);
      this.AllLogsPanel.TabIndex = 0;
      this.chartsTab.Controls.Add((Control) this.chartPanel);
      this.chartsTab.Controls.Add((Control) this.statusStrip1);
      this.chartsTab.Location = new Point(4, 22);
      this.chartsTab.Name = "chartsTab";
      this.chartsTab.Padding = new Padding(3);
      this.chartsTab.Size = new Size(788, 483);
      this.chartsTab.TabIndex = 2;
      this.chartsTab.Text = "Charts";
      this.chartsTab.UseVisualStyleBackColor = true;
      this.chartPanel.ColumnCount = 2;
      this.chartPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.chartPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.chartPanel.Controls.Add((Control) this.netGraphControl1, 0, 0);
      this.chartPanel.Controls.Add((Control) this.netGraphControl2, 1, 0);
      this.chartPanel.Controls.Add((Control) this.netGraphControl3, 0, 1);
      this.chartPanel.Dock = DockStyle.Fill;
      this.chartPanel.Location = new Point(3, 3);
      this.chartPanel.Name = "chartPanel";
      this.chartPanel.RowCount = 2;
      this.chartPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.chartPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.chartPanel.Size = new Size(782, 455);
      this.chartPanel.TabIndex = 3;
      this.statusStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.toolStripStatusLabel1
      });
      this.statusStrip1.Location = new Point(3, 458);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new Size(782, 22);
      this.statusStrip1.TabIndex = 1;
      this.statusStrip1.Text = "statusStrip1";
      this.toolStripStatusLabel1.Image = (Image) componentResourceManager.GetObject("toolStripStatusLabel1.Image");
      this.toolStripStatusLabel1.ImageScaling = ToolStripItemImageScaling.None;
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new Size(259, 17);
      this.netGraphControl1.Dock = DockStyle.Fill;
      this.netGraphControl1.Location = new Point(3, 3);
      this.netGraphControl1.Name = "netGraphControl1";
      this.netGraphControl1.Size = new Size(385, 221);
      this.netGraphControl1.TabIndex = 0;
      this.netGraphControl1.Text = "netGraphControl1";
      this.netGraphControl2.Dock = DockStyle.Fill;
      this.netGraphControl2.Location = new Point(394, 3);
      this.netGraphControl2.Name = "netGraphControl2";
      this.netGraphControl2.Size = new Size(385, 221);
      this.netGraphControl2.TabIndex = 1;
      this.netGraphControl2.Text = "netGraphControl2";
      this.netGraphControl3.Dock = DockStyle.Fill;
      this.netGraphControl3.Location = new Point(3, 230);
      this.netGraphControl3.Name = "netGraphControl3";
      this.netGraphControl3.Size = new Size(385, 222);
      this.netGraphControl3.TabIndex = 2;
      this.netGraphControl3.Text = "netGraphControl3";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(783, 503);
      this.Controls.Add((Control) this.Tabs);
      this.Name = nameof (NetworkConsole);
      this.Text = nameof (NetworkConsole);
      this.Tabs.ResumeLayout(false);
      this.LogTab.ResumeLayout(false);
      this.LogTabs.ResumeLayout(false);
      this.LogTabsAll.ResumeLayout(false);
      this.chartsTab.ResumeLayout(false);
      this.chartsTab.PerformLayout();
      this.chartPanel.ResumeLayout(false);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
    }

    private delegate void OutputDelegate(RichTextBox box, string str, System.Drawing.Color? color = null);
  }
}
