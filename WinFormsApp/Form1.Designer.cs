﻿namespace WinFormsApp
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            btn_bookings = new Button();
            pnl_Main = new Panel();
            pnl_MainView = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            pnl_Main.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.BookEnStub_Logo;
            pictureBox1.Location = new Point(3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(121, 95);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(btn_bookings);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(126, 561);
            panel1.TabIndex = 2;
            // 
            // btn_bookings
            // 
            btn_bookings.Location = new Point(3, 103);
            btn_bookings.Name = "btn_bookings";
            btn_bookings.Size = new Size(115, 23);
            btn_bookings.TabIndex = 0;
            btn_bookings.Text = "Bookings";
            btn_bookings.UseVisualStyleBackColor = true;
            btn_bookings.Click += btn_bookings_Click;
            // 
            // pnl_Main
            // 
            pnl_Main.AutoSize = true;
            pnl_Main.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnl_Main.Controls.Add(pnl_MainView);
            pnl_Main.Controls.Add(panel1);
            pnl_Main.Dock = DockStyle.Fill;
            pnl_Main.Location = new Point(0, 0);
            pnl_Main.Name = "pnl_Main";
            pnl_Main.Size = new Size(924, 561);
            pnl_Main.TabIndex = 3;
            // 
            // pnl_MainView
            // 
            pnl_MainView.Dock = DockStyle.Fill;
            pnl_MainView.Location = new Point(126, 0);
            pnl_MainView.Name = "pnl_MainView";
            pnl_MainView.Size = new Size(798, 561);
            pnl_MainView.TabIndex = 4;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(924, 561);
            Controls.Add(pnl_Main);
            MinimumSize = new Size(940, 600);
            Name = "MainWindow";
            Text = "Book en stub";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            pnl_Main.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Panel panel1;
        private Button btn_bookings;
        private Panel pnl_Main;
        private Panel pnl_MainView;
    }
}