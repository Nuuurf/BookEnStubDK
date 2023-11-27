namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_bookings_Click(object sender, EventArgs e)
        {
            BookingTableForm bookingView = new BookingTableForm();
            bookingView.TopLevel = false;
            pnl_MainView.Controls.Clear();
            pnl_MainView.Controls.Add(bookingView);
            bookingView.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}