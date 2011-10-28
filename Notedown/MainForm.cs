﻿using System;
using System.Text;
using System.IO;
using Eto;
using Eto.Forms;
using Eto.Drawing;

namespace Notedown
{
    public class MainForm : Form
    {
        public NoteView Notes { get; private set; }

        public MainForm()
        {
            this.Text = "Notedown";
            this.Icon = new Icon(null, "Icon.ico");
            this.Size = new Size(1280, 800);
        }
        
        public override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            Preferences.Load();
            Notes = NoteView.CreateFromDirectory(Preferences.Folder);
            
            GenerateActions();
            GenerateContent();
        }
        
        private void GenerateActions()
        {
            // generate action args
            GenerateActionArgs args = new GenerateActionArgs();
            Application.Instance.GetSystemActions(args);
            args.Actions.Add(new Actions.About());
            args.Actions.Add(new Actions.Quit());
            args.Actions.Add(new Actions.Close());
            args.Actions.Add(new Actions.New());
            args.Actions.Add(new Actions.Save());
            args.Actions.Add(new Actions.Delete());
            args.Actions.Add(new Actions.Preferences());
            args.Actions.Add(new Actions.Rename());
            
            // generate menu and toolbar
            GenerateMenu(args);
            GenerateToolBar(args);
        }
        
        private void GenerateContent()
        {
            var splitter = new Splitter();
            splitter.Panel1 = Notes.ListBox;
            splitter.Panel2 = Notes.TextArea;
            splitter.Position = 200;
            
            this.AddDockedControl(splitter);
            this.Shown += delegate
            {
                splitter.Panel1.Focus();
            };
        }
        
        private void GenerateMenu(GenerateActionArgs args)
        {
            var main = args.Menu.FindAddSubMenu("Notedown", 0);
            var file = args.Menu.FindAddSubMenu("&File", 100);
            var window = args.Menu.FindAddSubMenu("&Window", 900);
            var help = args.Menu.FindAddSubMenu("Help", 1000);
            
            int i = 0;
            if (Generator.ID == "mac")
            {
                main.Actions.Add(Actions.About.ActionID, i++);
                main.Actions.AddSeparator(i++);
                main.Actions.Add(Actions.Preferences.ActionID, i++);
                main.Actions.AddSeparator(i++);
                main.Actions.Add("mac_hide", i++);
                main.Actions.Add("mac_hideothers", i++);
                main.Actions.Add("mac_showall", i++);
                main.Actions.AddSeparator(i++);
                main.Actions.Add(Actions.Quit.ActionID, i++);
                
                file.Actions.Add(Actions.Save.ActionID, i++);
                file.Actions.Add(Actions.New.ActionID, i++);
                file.Actions.Add(Actions.Delete.ActionID, i++);
                file.Actions.Add(Actions.Rename.ActionID, i++);
                
                window.Actions.Add("mac_performMiniaturize");
                window.Actions.Add("mac_performZoom");
            }
            else
            {
                file.Actions.Add(Actions.Quit.ActionID);
                
                help.Actions.Add(Actions.About.ActionID);
            }
            
            this.Menu = args.Menu.GenerateMenuBar();
        }
        
        private void GenerateToolBar(GenerateActionArgs args)
        {
            args.ToolBar.Add(Actions.New.ActionID);
            args.ToolBar.Add(Actions.Delete.ActionID);
            args.ToolBar.Add(Actions.Rename.ActionID);
            args.ToolBar.AddFlexibleSpace();
            args.ToolBar.Add(Actions.Preferences.ActionID);
            //args.ToolBar.Add(Actions.About.ActionID);
            
            this.ToolBar = args.ToolBar.GenerateToolBar();
            this.ToolBar.Style = "ToolBar";
        }
    }
}
