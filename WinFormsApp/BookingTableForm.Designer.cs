﻿namespace WinFormsApp
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            panel1 = new Panel();
            pnl_Tabel = new Panel();
            dataGridView1 = new DataGridView();
            pnl_Buttons = new Panel();
            btn_Details = new Button();
            btn_Delete = new Button();
            btn_Add = new Button();
            btn_Edit = new Button();
            pnl_Filter = new Panel();
            btn_clearFilter = new Button();
            txt_email = new TextBox();
            label3 = new Label();
            txt_Phone = new TextBox();
            label4 = new Label();
            txt_OrderID = new TextBox();
            label2 = new Label();
            txt_StubID = new TextBox();
            label1 = new Label();
            btn_refresh = new Button();
            lbl_EndDate = new Label();
            lbl_StartDate = new Label();
            dtp_EndDate = new DateTimePicker();
            dtp_StartDate = new DateTimePicker();
            chkBox_Today = new CheckBox();
            toolTip1 = new ToolTip(components);
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
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
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
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            dataGridView1.RowEnter += dataGridView1_RowEnter;
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
            btn_Details.Location = new Point(39, 47);
            btn_Details.Margin = new Padding(3, 4, 3, 4);
            btn_Details.Name = "btn_Details";
            btn_Details.Size = new Size(86, 31);
            btn_Details.TabIndex = 9;
            btn_Details.Text = "Details";
            toolTip1.SetToolTip(btn_Details, "NOT IMPLEMENTED");
            btn_Details.UseVisualStyleBackColor = true;
            btn_Details.Click += btn_Details_Click;
            // 
            // btn_Delete
            // 
            btn_Delete.Enabled = false;
            btn_Delete.Location = new Point(39, 124);
            btn_Delete.Margin = new Padding(3, 4, 3, 4);
            btn_Delete.Name = "btn_Delete";
            btn_Delete.Size = new Size(86, 31);
            btn_Delete.TabIndex = 8;
            btn_Delete.Text = "Delete";
            toolTip1.SetToolTip(btn_Delete, "Deletes the selected booking");
            btn_Delete.UseVisualStyleBackColor = true;
            btn_Delete.Click += btn_Delete_Click;
            // 
            // btn_Add
            // 
            btn_Add.AccessibleDescription = "";
            btn_Add.AccessibleName = "";
            btn_Add.Location = new Point(39, 8);
            btn_Add.Margin = new Padding(3, 4, 3, 4);
            btn_Add.Name = "btn_Add";
            btn_Add.Size = new Size(86, 31);
            btn_Add.TabIndex = 7;
            btn_Add.Text = "Add";
            toolTip1.SetToolTip(btn_Add, "Click here to add a booking");
            btn_Add.UseVisualStyleBackColor = true;
            btn_Add.Click += btn_Add_Click;
            // 
            // btn_Edit
            // 
            btn_Edit.Enabled = false;
            btn_Edit.Location = new Point(39, 85);
            btn_Edit.Margin = new Padding(3, 4, 3, 4);
            btn_Edit.Name = "btn_Edit";
            btn_Edit.Size = new Size(86, 31);
            btn_Edit.TabIndex = 6;
            btn_Edit.Text = "Edit";
            toolTip1.SetToolTip(btn_Edit, "NOT IMPLEMENTED");
            btn_Edit.UseVisualStyleBackColor = true;
            btn_Edit.Click += btn_Edit_Click;
            // 
            // pnl_Filter
            // 
            pnl_Filter.Controls.Add(btn_clearFilter);
            pnl_Filter.Controls.Add(txt_email);
            pnl_Filter.Controls.Add(label3);
            pnl_Filter.Controls.Add(txt_Phone);
            pnl_Filter.Controls.Add(label4);
            pnl_Filter.Controls.Add(txt_OrderID);
            pnl_Filter.Controls.Add(label2);
            pnl_Filter.Controls.Add(txt_StubID);
            pnl_Filter.Controls.Add(label1);
            pnl_Filter.Controls.Add(btn_refresh);
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
            // btn_clearFilter
            // 
            btn_clearFilter.Location = new Point(667, 91);
            btn_clearFilter.Margin = new Padding(3, 4, 3, 4);
            btn_clearFilter.Name = "btn_clearFilter";
            btn_clearFilter.Size = new Size(86, 29);
            btn_clearFilter.TabIndex = 19;
            btn_clearFilter.Text = "Clear Filter";
            toolTip1.SetToolTip(btn_clearFilter, "Click to clear the filters, will update table");
            btn_clearFilter.UseVisualStyleBackColor = true;
            btn_clearFilter.Click += btn_clearFilter_Click;
            // 
            // txt_email
            // 
            txt_email.Location = new Point(455, 89);
            txt_email.Margin = new Padding(3, 4, 3, 4);
            txt_email.Name = "txt_email";
            txt_email.Size = new Size(114, 27);
            txt_email.TabIndex = 18;
            toolTip1.SetToolTip(txt_email, "Sets filter to only include a certain customer by email");
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(390, 96);
            label3.Name = "label3";
            label3.Size = new Size(46, 20);
            label3.TabIndex = 17;
            label3.Text = "Email";
            toolTip1.SetToolTip(label3, "Sets filter to only include a certain customer by email");
            // 
            // txt_Phone
            // 
            txt_Phone.Location = new Point(455, 51);
            txt_Phone.Margin = new Padding(3, 4, 3, 4);
            txt_Phone.Name = "txt_Phone";
            txt_Phone.Size = new Size(114, 27);
            txt_Phone.TabIndex = 16;
            toolTip1.SetToolTip(txt_Phone, "Sets filter to only include a certain customer by phonenumber");
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(390, 57);
            label4.Name = "label4";
            label4.Size = new Size(50, 20);
            label4.TabIndex = 15;
            label4.Text = "Phone";
            toolTip1.SetToolTip(label4, "Sets filter to only include a certain customer by phonenumber");
            // 
            // txt_OrderID
            // 
            txt_OrderID.Location = new Point(266, 89);
            txt_OrderID.Margin = new Padding(3, 4, 3, 4);
            txt_OrderID.Name = "txt_OrderID";
            txt_OrderID.Size = new Size(114, 27);
            txt_OrderID.TabIndex = 14;
            toolTip1.SetToolTip(txt_OrderID, "Sets filter to only include a certain order by ID (must be a number)");
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(201, 96);
            label2.Name = "label2";
            label2.Size = new Size(66, 20);
            label2.TabIndex = 13;
            label2.Text = "Order ID";
            toolTip1.SetToolTip(label2, "Sets filter to only include a certain order by ID (must be a number)");
            // 
            // txt_StubID
            // 
            txt_StubID.Location = new Point(266, 51);
            txt_StubID.Margin = new Padding(3, 4, 3, 4);
            txt_StubID.Name = "txt_StubID";
            txt_StubID.Size = new Size(114, 27);
            txt_StubID.TabIndex = 12;
            toolTip1.SetToolTip(txt_StubID, "Sets filter to only include a certain stub by ID (must be a number)");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(201, 57);
            label1.Name = "label1";
            label1.Size = new Size(58, 20);
            label1.TabIndex = 11;
            label1.Text = "Stub ID";
            toolTip1.SetToolTip(label1, "Sets filter to only include a certain stub by ID (must be a number)");
            // 
            // btn_refresh
            // 
            btn_refresh.Location = new Point(575, 91);
            btn_refresh.Margin = new Padding(3, 4, 3, 4);
            btn_refresh.Name = "btn_refresh";
            btn_refresh.Size = new Size(86, 29);
            btn_refresh.TabIndex = 10;
            btn_refresh.Text = "Refresh";
            toolTip1.SetToolTip(btn_refresh, "Refreshes the booking-table");
            btn_refresh.UseVisualStyleBackColor = true;
            btn_refresh.Click += btn_refresh_Click;
            // 
            // lbl_EndDate
            // 
            lbl_EndDate.AutoSize = true;
            lbl_EndDate.Location = new Point(2, 95);
            lbl_EndDate.Name = "lbl_EndDate";
            lbl_EndDate.Size = new Size(68, 20);
            lbl_EndDate.TabIndex = 5;
            lbl_EndDate.Text = "End date";
            toolTip1.SetToolTip(lbl_EndDate, "Sets the upper range of the date filter");
            // 
            // lbl_StartDate
            // 
            lbl_StartDate.AutoSize = true;
            lbl_StartDate.Location = new Point(2, 57);
            lbl_StartDate.Name = "lbl_StartDate";
            lbl_StartDate.Size = new Size(74, 20);
            lbl_StartDate.TabIndex = 4;
            lbl_StartDate.Text = "Start date";
            toolTip1.SetToolTip(lbl_StartDate, "Sets the lower range of the date filter");
            // 
            // dtp_EndDate
            // 
            dtp_EndDate.Enabled = false;
            dtp_EndDate.Format = DateTimePickerFormat.Short;
            dtp_EndDate.Location = new Point(82, 91);
            dtp_EndDate.Margin = new Padding(3, 4, 3, 4);
            dtp_EndDate.Name = "dtp_EndDate";
            dtp_EndDate.Size = new Size(111, 27);
            dtp_EndDate.TabIndex = 3;
            toolTip1.SetToolTip(dtp_EndDate, "Sets the upper range of the date filter");
            dtp_EndDate.Value = new DateTime(2023, 11, 30, 0, 0, 0, 0);
            // 
            // dtp_StartDate
            // 
            dtp_StartDate.AccessibleDescription = "Test";
            dtp_StartDate.AccessibleRole = AccessibleRole.ToolTip;
            dtp_StartDate.Enabled = false;
            dtp_StartDate.Format = DateTimePickerFormat.Short;
            dtp_StartDate.Location = new Point(82, 52);
            dtp_StartDate.Margin = new Padding(3, 4, 3, 4);
            dtp_StartDate.Name = "dtp_StartDate";
            dtp_StartDate.Size = new Size(111, 27);
            dtp_StartDate.TabIndex = 2;
            toolTip1.SetToolTip(dtp_StartDate, "Sets the lower range of the date filter");
            dtp_StartDate.ValueChanged += dtp_StartDate_ValueChanged;
            // 
            // chkBox_Today
            // 
            chkBox_Today.AutoSize = true;
            chkBox_Today.Checked = true;
            chkBox_Today.CheckState = CheckState.Checked;
            chkBox_Today.Location = new Point(82, 20);
            chkBox_Today.Margin = new Padding(3, 4, 3, 4);
            chkBox_Today.Name = "chkBox_Today";
            chkBox_Today.Size = new Size(71, 24);
            chkBox_Today.TabIndex = 1;
            chkBox_Today.Text = "Today";
            toolTip1.SetToolTip(chkBox_Today, "Checked: Get bookings from today");
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
        private Button btn_refresh;
        private TextBox txt_StubID;
        private Label label1;
        private Label label2;
        private TextBox txt_email;
        private Label label3;
        private TextBox txt_Phone;
        private Label label4;
        private TextBox txt_OrderID;
        private Button btn_clearFilter;
        private ToolTip toolTip1;
    }
}