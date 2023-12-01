namespace WinFormsApp.Models
{
    public class Customer
    {
        //public int Id { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public override string ToString()
        {
            if(FullName == null)
            {
                return "N/A";
            }
            return FullName;
        }
    }
}
