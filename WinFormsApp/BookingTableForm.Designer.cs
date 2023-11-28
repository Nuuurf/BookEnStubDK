namespace WinFormsApp
{
    partial class BookingTableForm
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
            panel1 = new Panel();
            pnl_Tabel = new Panel();
            dataGridView1 = new DataGridView();
            Stub_ID = new DataGridViewTextBoxColumn();
            Booking_Name = new DataGridViewTextBoxColumn();
            Start_Time = new DataGridViewTextBoxColumn();
            End_Time = new DataGridViewTextBoxColumn();
            Booking_Notes = new DataGridViewTextBoxColumn();
            pnl_Buttons = new Panel();
            btn_Details = new Button();
            btn_Delete = new Button();
            btn_Add = new Button();
            btn_Edit = new Button();
            pnl_Filter = new Panel();
            lbl_EndDate = new Label();
            lbl_StartDate = new Label();
            dtp_EndDate = new DateTimePicker();
            dtp_StartDate = new DateTimePicker();
            chkBox_Today = new CheckBox();
            panel1.SuspendLayout();
            pnl_Tabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            pnl_Buttons.SuspendLayout();
            pnl_Filter.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(pnl_Tabel);
            panel1.Controls.Add(pnl_Buttons);
            panel1.Controls.Add(pnl_Filter);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(910, 600);
            panel1.TabIndex = 0;
            // 
            // pnl_Tabel
            // 
            pnl_Tabel.Controls.Add(dataGridView1);
            pnl_Tabel.Dock = DockStyle.Fill;
            pnl_Tabel.Location = new Point(0, 133);
            pnl_Tabel.Margin = new Padding(3, 4, 3, 4);
            pnl_Tabel.Name = "pnl_Tabel";
            pnl_Tabel.Size = new Size(753, 467);
            pnl_Tabel.TabIndex = 2;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Stub_ID, Booking_Name, Start_Time, End_Time, Booking_Notes });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 4, 3, 4);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(753, 467);
            dataGridView1.TabIndex = 0;
            // 
            // Stub_ID
            // 
            Stub_ID.HeaderText = "Stub ID";
            Stub_ID.MinimumWidth = 6;
            Stub_ID.Name = "Stub_ID";
            Stub_ID.ReadOnly = true;
            Stub_ID.Width = 125;
            // 
            // Booking_Name
            // 
            Booking_Name.HeaderText = "Name";
            Booking_Name.MinimumWidth = 6;
            Booking_Name.Name = "Booking_Name";
            Booking_Name.ReadOnly = true;
            Booking_Name.Width = 125;
            // 
            // Start_Time
            // 
            Start_Time.HeaderText = "Start time";
            Start_Time.MinimumWidth = 6;
            Start_Time.Name = "Start_Time";
            Start_Time.ReadOnly = true;
            Start_Time.Width = 125;
            // 
            // End_Time
            // 
            End_Time.HeaderText = "End time";
            End_Time.MinimumWidth = 6;
            End_Time.Name = "End_Time";
            End_Time.ReadOnly = true;
            End_Time.Width = 125;
            // 
            // Booking_Notes
            // 
            Booking_Notes.HeaderText = "Notes";
            Booking_Notes.MinimumWidth = 6;
            Booking_Notes.Name = "Booking_Notes";
            Booking_Notes.ReadOnly = true;
            Booking_Notes.Width = 125;
            // 
            // pnl_Buttons
            // 
            pnl_Buttons.Controls.Add(btn_Details);
            pnl_Buttons.Controls.Add(btn_Delete);
            pnl_Buttons.Controls.Add(btn_Add);
            pnl_Buttons.Controls.Add(btn_Edit);
            pnl_Buttons.Dock = DockStyle.Right;
            pnl_Buttons.Location = new Point(753, 133);
            pnl_Buttons.Margin = new Padding(3, 4, 3, 4);
            pnl_Buttons.Name = "pnl_Buttons";
            pnl_Buttons.Size = new Size(157, 467);
            pnl_Buttons.TabIndex = 1;
            // 
            // btn_Details
            // 
            btn_Details.Enabled = false;
            btn_Details.Location = new Point(39, 161);
            btn_Details.Margin = new Padding(3, 4, 3, 4);
            btn_Details.Name = "btn_Details";
            btn_Details.Size = new Size(86, 31);
            btn_Details.TabIndex = 9;
            btn_Details.Text = "Details";
            btn_Details.UseVisualStyleBackColor = true;
            // 
            // btn_Delete
            // 
            btn_Delete.Enabled = false;
            btn_Delete.Location = new Point(39, 239);
            btn_Delete.Margin = new Padding(3, 4, 3, 4);
            btn_Delete.Name = "btn_Delete";
            btn_Delete.Size = new Size(86, 31);
            btn_Delete.TabIndex = 8;
            btn_Delete.Text = "Delete";
            btn_Delete.UseVisualStyleBackColor = true;
            // 
            // btn_Add
            // 
            btn_Add.Location = new Point(39, 123);
            btn_Add.Margin = new Padding(3, 4, 3, 4);
            btn_Add.Name = "btn_Add";
            btn_Add.Size = new Size(86, 31);
            btn_Add.TabIndex = 7;
            btn_Add.Text = "Add";
            btn_Add.UseVisualStyleBackColor = true;
            btn_Add.Click += btn_Add_Click;
            // 
            // btn_Edit
            // 
            btn_Edit.Enabled = false;
            btn_Edit.Location = new Point(39, 200);
            btn_Edit.Margin = new Padding(3, 4, 3, 4);
            btn_Edit.Name = "btn_Edit";
            btn_Edit.Size = new Size(86, 31);
            btn_Edit.TabIndex = 6;
            btn_Edit.Text = "Edit";
            btn_Edit.UseVisualStyleBackColor = true;
            // 
            // pnl_Filter
            // 
            pnl_Filter.Controls.Add(lbl_EndDate);
            pnl_Filter.Controls.Add(lbl_StartDate);
            pnl_Filter.Controls.Add(dtp_EndDate);
            pnl_Filter.Controls.Add(dtp_StartDate);
            pnl_Filter.Controls.Add(chkBox_Today);
            pnl_Filter.Dock = DockStyle.Top;
            pnl_Filter.Location = new Point(0, 0);
            pnl_Filter.Margin = new Padding(3, 4, 3, 4);
            pnl_Filter.Name = "pnl_Filter";
            pnl_Filter.Size = new Size(910, 133);
            pnl_Filter.TabIndex = 0;
            // 
            // lbl_EndDate
            // 
            lbl_EndDate.AutoSize = true;
            lbl_EndDate.Location = new Point(2, 95);
            lbl_EndDate.Name = "lbl_EndDate";
            lbl_EndDate.Size = new Size(68, 20);
            lbl_EndDate.TabIndex = 5;
            lbl_EndDate.Text = "End date";
            // 
            // lbl_StartDate
            // 
            lbl_StartDate.AutoSize = true;
            lbl_StartDate.Location = new Point(2, 57);
            lbl_StartDate.Name = "lbl_StartDate";
            lbl_StartDate.Size = new Size(74, 20);
            lbl_StartDate.TabIndex = 4;
            lbl_StartDate.Text = "Start date";
            // 
            // dtp_EndDate
            // 
            dtp_EndDate.Enabled = false;
            dtp_EndDate.Location = new Point(82, 90);
            dtp_EndDate.Margin = new Padding(3, 4, 3, 4);
            dtp_EndDate.Name = "dtp_EndDate";
            dtp_EndDate.Size = new Size(228, 27);
            dtp_EndDate.TabIndex = 3;
            // 
            // dtp_StartDate
            // 
            dtp_StartDate.Enabled = false;
            dtp_StartDate.Location = new Point(82, 52);
            dtp_StartDate.Margin = new Padding(3, 4, 3, 4);
            dtp_StartDate.Name = "dtp_StartDate";
            dtp_StartDate.Size = new Size(228, 27);
            dtp_StartDate.TabIndex = 2;
            // 
            // chkBox_Today
            // 
            chkBox_Today.AutoSize = true;
            chkBox_Today.Checked = true;
            chkBox_Today.CheckState = CheckState.Checked;
            chkBox_Today.Location = new Point(3, 16);
            chkBox_Today.Margin = new Padding(3, 4, 3, 4);
            chkBox_Today.Name = "chkBox_Today";
            chkBox_Today.Size = new Size(71, 24);
            chkBox_Today.TabIndex = 1;
            chkBox_Today.Text = "Today";
            chkBox_Today.UseVisualStyleBackColor = true;
            chkBox_Today.CheckedChanged += chkBox_Today_CheckedChanged;
            // 
            // BookingTableForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(910, 600);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "BookingTableForm";
            Text = "BookingTableForm";
            panel1.ResumeLayout(false);
            pnl_Tabel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            pnl_Buttons.ResumeLayout(false);
            pnl_Filter.ResumeLayout(false);
            pnl_Filter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel pnl_Tabel;
        private Panel pnl_Buttons;
        private Panel pnl_Filter;
        private DataGridView dataGridView1;
        private Label lbl_EndDate;
        private Label lbl_StartDate;
        private DateTimePicker dtp_EndDate;
        private DateTimePicker dtp_StartDate;
        private CheckBox chkBox_Today;
        private Button btn_Delete;
        private Button btn_Add;
        private Button btn_Edit;
        private Button btn_Details;
        private DataGridViewTextBoxColumn Stub_ID;
        private DataGridViewTextBoxColumn Booking_Name;
        private DataGridViewTextBoxColumn Start_Time;
        private DataGridViewTextBoxColumn End_Time;
        private DataGridViewTextBoxColumn Booking_Notes;
    }
}