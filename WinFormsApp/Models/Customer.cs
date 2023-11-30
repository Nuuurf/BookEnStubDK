﻿namespace WinFormsApp.Models
{
    public class Customer
    {
        //public int Id { get; set; }
        public string FirstName { get; set; }
        //public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public override string? ToString()
        {
            return FirstName;
        }
    }
}
