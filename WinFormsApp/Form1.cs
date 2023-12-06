namespace WinFormsApp
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_bookings_Click(object sender, EventArgs e)
        {
            ShowBookings();
        }

        private void ShowBookings()
        {
            this.Text = "Bookings";
            BookingTableForm bookingView = new BookingTableForm();
            bookingView.TopLevel = false;
            pnl_MainView.Controls.Clear();
            pnl_MainView.Controls.Add(bookingView);
            bookingView.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowBookings();
        }
    }
}