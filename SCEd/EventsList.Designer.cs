using System;
using System.Collections.Generic;
using System.Reflection;

namespace SCEd
{
    partial class EventsList
    {
        private static readonly String EVENT_TYPE = "StoneCircle.EVENT";

        protected override void OnShown(System.EventArgs e)
        {
            base.OnShown(e);
            List<Type> availableEvents = loadEvents();

            foreach (Type t in availableEvents) {
                listBox1.Items.Add(t.Name + " : " + t.BaseType.Name);
            }
        }

        /// <summary>
        /// Load a list of the types that inherit from Event.
        /// Move to a data model class.
        /// </summary>
        private List<Type> loadEvents()
        {
            Type[] assemblyTypes;

            Assembly assembly = Assembly.LoadFrom("StoneCircle.exe");
            assemblyTypes = assembly.GetExportedTypes();

            List<Type> events = new List<Type>();


            foreach (Type t in assemblyTypes) {
                Type baseType = t.BaseType;
                while (baseType != null) {
                    if (baseType.ToString() == EVENT_TYPE) {
                        if (!t.IsAbstract) {
                            events.Add(t);
                        }
                        break;
                    }

                    baseType = baseType.BaseType;
                }
            }

            return events;
            
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(259, 238);
            this.listBox1.TabIndex = 0;
            // 
            // EventsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.listBox1);
            this.Name = "EventsList";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
    }
}

