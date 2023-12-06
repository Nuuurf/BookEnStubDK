namespace WinFormsApp
{
    partial class AddBookingForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtBox_FullName = new TextBox();
            txtBox_Email = new TextBox();
            txtBox_PhoneNumber = new TextBox();
            txtBox_Notes = new TextBox();
            btn_OK = new Button();
            btn_Cancel = new Button();
            dtp_BookingDate = new DateTimePicker();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            cmb_AvailableTimeSlot = new ComboBox();
            btn_Add = new Button();
            btn_Remove = new Button();
            lbo_SelectedTime = new ListBox();
            toolTip1 = new ToolTip(components);
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 237);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 0;
            label1.Text = "Full name";
            toolTip1.SetToolTip(label1, "Enter customers name");
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 274);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 1;
            label2.Text = "Email";
            toolTip1.SetToolTip(label2, "Enter customers email");
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(30, 193);
            label3.Name = "label3";
            label3.Size = new Size(86, 15);
            label3.TabIndex = 2;
            label3.Text = "Phone number";
            toolTip1.SetToolTip(label3, "Enter customers phonenumber, if it exists rest of the customer data will be fetched from the database");
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(30, 313);
            label4.Name = "label4";
            label4.Size = new Size(33, 15);
            label4.TabIndex = 3;
            label4.Text = "Note";
            toolTip1.SetToolTip(label4, "Enter customers special requests, eg: Birthday");
            // 
            // txtBox_FullName
            // 
            txtBox_FullName.Location = new Point(131, 232);
            txtBox_FullName.Margin = new Padding(3, 2, 3, 2);
            txtBox_FullName.Name = "txtBox_FullName";
            txtBox_FullName.PlaceholderText = "Required";
            txtBox_FullName.Size = new Size(321, 23);
            txtBox_FullName.TabIndex = 7;
            txtBox_FullName.TextChanged += UpdateButtonOk;
            // 
            // txtBox_Email
            // 
            txtBox_Email.Location = new Point(131, 268);
            txtBox_Email.Margin = new Padding(3, 2, 3, 2);
            txtBox_Email.Name = "txtBox_Email";
            txtBox_Email.PlaceholderText = "Required";
            txtBox_Email.Size = new Size(321, 23);
            txtBox_Email.TabIndex = 8;
            txtBox_Email.TextChanged += UpdateButtonOk;
            // 
            // txtBox_PhoneNumber
            // 
            txtBox_PhoneNumber.Location = new Point(131, 190);
            txtBox_PhoneNumber.Margin = new Padding(3, 2, 3, 2);
            txtBox_PhoneNumber.Name = "txtBox_PhoneNumber";
            txtBox_PhoneNumber.PlaceholderText = "Required";
            txtBox_PhoneNumber.Size = new Size(321, 23);
            txtBox_PhoneNumber.TabIndex = 6;
            txtBox_PhoneNumber.TextChanged += UpdateButtonOk;
            txtBox_PhoneNumber.Leave += txtBox_PhoneNumber_Leave;
            // 
            // txtBox_Notes
            // 
            txtBox_Notes.Location = new Point(131, 306);
            txtBox_Notes.Margin = new Padding(3, 2, 3, 2);
            txtBox_Notes.Name = "txtBox_Notes";
            txtBox_Notes.PlaceholderText = "Optional";
            txtBox_Notes.Size = new Size(321, 23);
            txtBox_Notes.TabIndex = 9;
            // 
            // btn_OK
            // 
            btn_OK.Enabled = false;
            btn_OK.Location = new Point(294, 340);
            btn_OK.Margin = new Padding(3, 2, 3, 2);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(70, 22);
            btn_OK.TabIndex = 10;
            btn_OK.Text = "OK";
            toolTip1.SetToolTip(btn_OK, "Confirms the current booking, watch for errors");
            btn_OK.UseVisualStyleBackColor = true;
            btn_OK.Click += btn_OK_Click;
            // 
            // btn_Cancel
            // 
            btn_Cancel.Location = new Point(370, 340);
            btn_Cancel.Margin = new Padding(3, 2, 3, 2);
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.Size = new Size(82, 22);
            btn_Cancel.TabIndex = 11;
            btn_Cancel.Text = "Cancel";
            toolTip1.SetToolTip(btn_Cancel, "Cancels the current booking and closes the window");
            btn_Cancel.UseVisualStyleBackColor = true;
            btn_Cancel.Click += btn_Cancel_Click;
            // 
            // dtp_BookingDate
            // 
            dtp_BookingDate.Format = DateTimePickerFormat.Short;
            dtp_BookingDate.Location = new Point(30, 40);
            dtp_BookingDate.Margin = new Padding(3, 2, 3, 2);
            dtp_BookingDate.Name = "dtp_BookingDate";
            dtp_BookingDate.Size = new Size(150, 23);
            dtp_BookingDate.TabIndex = 1;
            toolTip1.SetToolTip(dtp_BookingDate, "Sets the date of the booking");
            dtp_BookingDate.ValueChanged += dtp_BookingDate_ValueChanged;
            // 
            // cmb_AvailableTimeSlot
            // 
            cmb_AvailableTimeSlot.FormattingEnabled = true;
            cmb_AvailableTimeSlot.Location = new Point(30, 76);
            cmb_AvailableTimeSlot.Margin = new Padding(3, 2, 3, 2);
            cmb_AvailableTimeSlot.Name = "cmb_AvailableTimeSlot";
            cmb_AvailableTimeSlot.Size = new Size(150, 23);
            cmb_AvailableTimeSlot.TabIndex = 2;
            toolTip1.SetToolTip(cmb_AvailableTimeSlot, "Pick a time to add to the booking");
            cmb_AvailableTimeSlot.SelectedIndexChanged += cmb_AvailableTimeSlot_SelectedIndexChanged;
            cmb_AvailableTimeSlot.SelectedValueChanged += cmb_AvailableTimeSlot_SelectedValueChanged;
            // 
            // btn_Add
            // 
            btn_Add.Enabled = false;
            btn_Add.Location = new Point(30, 112);
            btn_Add.Margin = new Padding(3, 2, 3, 2);
            btn_Add.Name = "btn_Add";
            btn_Add.Size = new Size(69, 22);
            btn_Add.TabIndex = 3;
            btn_Add.Text = "Add";
            toolTip1.SetToolTip(btn_Add, "Adds the chosen date and time to current booking");
            btn_Add.UseVisualStyleBackColor = true;
            btn_Add.Click += btn_Add_Click;
            // 
            // btn_Remove
            // 
            btn_Remove.Enabled = false;
            btn_Remove.Location = new Point(105, 112);
            btn_Remove.Margin = new Padding(3, 2, 3, 2);
            btn_Remove.Name = "btn_Remove";
            btn_Remove.Size = new Size(75, 22);
            btn_Remove.TabIndex = 4;
            btn_Remove.Text = "Remove";
            toolTip1.SetToolTip(btn_Remove, "Removes the chosen date and time from current booking");
            btn_Remove.UseVisualStyleBackColor = true;
            btn_Remove.Click += btn_Remove_Click;
            // 
            // lbo_SelectedTime
            // 
            lbo_SelectedTime.Enabled = false;
            lbo_SelectedTime.FormattingEnabled = true;
            lbo_SelectedTime.ItemHeight = 15;
            lbo_SelectedTime.Items.AddRange(new object[] { "No bookings selected...." });
            lbo_SelectedTime.Location = new Point(186, 40);
            lbo_SelectedTime.Margin = new Padding(3, 2, 3, 2);
            lbo_SelectedTime.Name = "lbo_SelectedTime";
            lbo_SelectedTime.Size = new Size(266, 94);
            lbo_SelectedTime.TabIndex = 5;
            toolTip1.SetToolTip(lbo_SelectedTime, "This contains the current date and times for the current booking");
            lbo_SelectedTime.SelectedIndexChanged += lbo_SelectedTime_SelectedIndexChanged;
            lbo_SelectedTime.EnabledChanged += UpdateButtonOk;
            // 
            // AddBookingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(498, 385);
            Controls.Add(lbo_SelectedTime);
            Controls.Add(btn_Remove);
            Controls.Add(btn_Add);
            Controls.Add(cmb_AvailableTimeSlot);
            Controls.Add(dtp_BookingDate);
            Controls.Add(btn_Cancel);
            Controls.Add(btn_OK);
            Controls.Add(txtBox_Notes);
            Controls.Add(txtBox_PhoneNumber);
            Controls.Add(txtBox_Email);
            Controls.Add(txtBox_FullName);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "AddBookingForm";
            Text = "Add booking";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtBox_FullName;
        private TextBox txtBox_Email;
        private TextBox txtBox_PhoneNumber;
        private TextBox txtBox_Notes;
        private Button btn_OK;
        private Button btn_Cancel;
        private DateTimePicker dtp_BookingDate;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ComboBox cmb_AvailableTimeSlot;
        private Button btn_Add;
        private Button btn_Remove;
        private ListBox lbo_SelectedTime;
        private ToolTip toolTip1;
    }
}