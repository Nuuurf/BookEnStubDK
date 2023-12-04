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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
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
            panel1.Name = "panel1";
            panel1.Size = new Size(796, 450);
            panel1.TabIndex = 0;
            // 
            // pnl_Tabel
            // 
            pnl_Tabel.Controls.Add(dataGridView1);
            pnl_Tabel.Dock = DockStyle.Fill;
            pnl_Tabel.Location = new Point(0, 100);
            pnl_Tabel.Name = "pnl_Tabel";
            pnl_Tabel.Size = new Size(659, 350);
            pnl_Tabel.TabIndex = 2;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(659, 350);
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
            pnl_Buttons.Location = new Point(659, 100);
            pnl_Buttons.Name = "pnl_Buttons";
            pnl_Buttons.Size = new Size(137, 350);
            pnl_Buttons.TabIndex = 1;
            // 
            // btn_Details
            // 
            btn_Details.Enabled = false;
            btn_Details.Location = new Point(34, 35);
            btn_Details.Name = "btn_Details";
            btn_Details.Size = new Size(75, 23);
            btn_Details.TabIndex = 9;
            btn_Details.Text = "Details";
            toolTip1.SetToolTip(btn_Details, "NOT IMPLEMENTED");
            btn_Details.UseVisualStyleBackColor = true;
            btn_Details.Click += btn_Details_Click;
            // 
            // btn_Delete
            // 
            btn_Delete.Enabled = false;
            btn_Delete.Location = new Point(34, 93);
            btn_Delete.Name = "btn_Delete";
            btn_Delete.Size = new Size(75, 23);
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
            btn_Add.Location = new Point(34, 6);
            btn_Add.Name = "btn_Add";
            btn_Add.Size = new Size(75, 23);
            btn_Add.TabIndex = 7;
            btn_Add.Text = "Add";
            toolTip1.SetToolTip(btn_Add, "Click here to add a booking");
            btn_Add.UseVisualStyleBackColor = true;
            btn_Add.Click += btn_Add_Click;
            // 
            // btn_Edit
            // 
            btn_Edit.Enabled = false;
            btn_Edit.Location = new Point(34, 64);
            btn_Edit.Name = "btn_Edit";
            btn_Edit.Size = new Size(75, 23);
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
            pnl_Filter.Name = "pnl_Filter";
            pnl_Filter.Size = new Size(796, 100);
            pnl_Filter.TabIndex = 0;
            // 
            // btn_clearFilter
            // 
            btn_clearFilter.Location = new Point(584, 68);
            btn_clearFilter.Name = "btn_clearFilter";
            btn_clearFilter.Size = new Size(75, 22);
            btn_clearFilter.TabIndex = 19;
            btn_clearFilter.Text = "Clear Filter";
            toolTip1.SetToolTip(btn_clearFilter, "Click to clear the filters, will update table");
            btn_clearFilter.UseVisualStyleBackColor = true;
            btn_clearFilter.Click += btn_clearFilter_Click;
            // 
            // txt_email
            // 
            txt_email.Location = new Point(398, 67);
            txt_email.Name = "txt_email";
            txt_email.Size = new Size(100, 23);
            txt_email.TabIndex = 18;
            toolTip1.SetToolTip(txt_email, "Sets filter to only include a certain customer by email");
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(341, 72);
            label3.Name = "label3";
            label3.Size = new Size(36, 15);
            label3.TabIndex = 17;
            label3.Text = "Email";
            // 
            // txt_Phone
            // 
            txt_Phone.Location = new Point(398, 38);
            txt_Phone.Name = "txt_Phone";
            txt_Phone.Size = new Size(100, 23);
            txt_Phone.TabIndex = 16;
            toolTip1.SetToolTip(txt_Phone, "Sets filter to only include a certain customer by phonenumber");
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(341, 43);
            label4.Name = "label4";
            label4.Size = new Size(41, 15);
            label4.TabIndex = 15;
            label4.Text = "Phone";
            // 
            // txt_OrderID
            // 
            txt_OrderID.Location = new Point(233, 67);
            txt_OrderID.Name = "txt_OrderID";
            txt_OrderID.Size = new Size(100, 23);
            txt_OrderID.TabIndex = 14;
            toolTip1.SetToolTip(txt_OrderID, "Sets filter to only include a certain order by ID (must be a number)");
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(176, 72);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 13;
            label2.Text = "Order ID";
            // 
            // txt_StubID
            // 
            txt_StubID.Location = new Point(233, 38);
            txt_StubID.Name = "txt_StubID";
            txt_StubID.Size = new Size(100, 23);
            txt_StubID.TabIndex = 12;
            toolTip1.SetToolTip(txt_StubID, "Sets filter to only include a certain stub by ID (must be a number)");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(176, 43);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 11;
            label1.Text = "Stub ID";
            // 
            // btn_refresh
            // 
            btn_refresh.Location = new Point(72, 8);
            btn_refresh.Name = "btn_refresh";
            btn_refresh.Size = new Size(75, 23);
            btn_refresh.TabIndex = 10;
            btn_refresh.Text = "Refresh";
            toolTip1.SetToolTip(btn_refresh, "Refreshes the booking-table");
            btn_refresh.UseVisualStyleBackColor = true;
            btn_refresh.Click += btn_refresh_Click;
            // 
            // lbl_EndDate
            // 
            lbl_EndDate.AutoSize = true;
            lbl_EndDate.Location = new Point(2, 71);
            lbl_EndDate.Name = "lbl_EndDate";
            lbl_EndDate.Size = new Size(53, 15);
            lbl_EndDate.TabIndex = 5;
            lbl_EndDate.Text = "End date";
            // 
            // lbl_StartDate
            // 
            lbl_StartDate.AutoSize = true;
            lbl_StartDate.Location = new Point(2, 43);
            lbl_StartDate.Name = "lbl_StartDate";
            lbl_StartDate.Size = new Size(57, 15);
            lbl_StartDate.TabIndex = 4;
            lbl_StartDate.Text = "Start date";
            // 
            // dtp_EndDate
            // 
            dtp_EndDate.Enabled = false;
            dtp_EndDate.Format = DateTimePickerFormat.Short;
            dtp_EndDate.Location = new Point(72, 68);
            dtp_EndDate.Name = "dtp_EndDate";
            dtp_EndDate.Size = new Size(98, 23);
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
            dtp_StartDate.Location = new Point(72, 39);
            dtp_StartDate.Name = "dtp_StartDate";
            dtp_StartDate.Size = new Size(98, 23);
            dtp_StartDate.TabIndex = 2;
            toolTip1.SetToolTip(dtp_StartDate, "Sets the lower range of the date filter");
            dtp_StartDate.ValueChanged += dtp_StartDate_ValueChanged;
            // 
            // chkBox_Today
            // 
            chkBox_Today.AutoSize = true;
            chkBox_Today.Checked = true;
            chkBox_Today.CheckState = CheckState.Checked;
            chkBox_Today.Location = new Point(3, 12);
            chkBox_Today.Name = "chkBox_Today";
            chkBox_Today.Size = new Size(57, 19);
            chkBox_Today.TabIndex = 1;
            chkBox_Today.Text = "Today";
            toolTip1.SetToolTip(chkBox_Today, "Checked: Get bookings from today");
            chkBox_Today.UseVisualStyleBackColor = true;
            chkBox_Today.CheckedChanged += chkBox_Today_CheckedChanged;
            // 
            // BookingTableForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(796, 450);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
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