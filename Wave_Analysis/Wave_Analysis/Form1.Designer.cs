﻿namespace WAVE_Analysis
 {
     partial class Form1
     {
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
             if (disposing && (components != null))
             {
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
            this.components = new System.ComponentModel.Container();
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.plotView2 = new OxyPlot.WindowsForms.PlotView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Play = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // plotView1
            // 
            this.plotView1.Location = new System.Drawing.Point(13, 64);
            this.plotView1.Margin = new System.Windows.Forms.Padding(4);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(503, 250);
            this.plotView1.TabIndex = 0;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // plotView2
            // 
            this.plotView2.Location = new System.Drawing.Point(13, 322);
            this.plotView2.Margin = new System.Windows.Forms.Padding(4);
            this.plotView2.Name = "plotView2";
            this.plotView2.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView2.Size = new System.Drawing.Size(503, 250);
            this.plotView2.TabIndex = 0;
            this.plotView2.Text = "plotView2";
            this.plotView2.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView2.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView2.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // timer1
            // 
            this.timer1.Interval = 67;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Play
            // 
            this.Play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Play.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Play.Location = new System.Drawing.Point(6, 12);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(75, 23);
            this.Play.TabIndex = 1;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // Stop
            // 
            this.Stop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Stop.Location = new System.Drawing.Point(87, 13);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 23);
            this.Stop.TabIndex = 2;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 585);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.plotView2);
            this.Controls.Add(this.plotView1);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "ANC Manager";
            this.ResumeLayout(false);

         }

         private void timer1_Tick(object sender, System.EventArgs e)
         {
             //throw new System.NotImplementedException();
         }

         #endregion

         //private OxyPlot.WindowsForms.PlotView plot1;
         private OxyPlot.WindowsForms.PlotView plotView1;
         private OxyPlot.WindowsForms.PlotView plotView2;
         private System.Windows.Forms.Timer timer1;
         private System.Windows.Forms.Button Play;
         private System.Windows.Forms.Button Stop;
     }
 }
