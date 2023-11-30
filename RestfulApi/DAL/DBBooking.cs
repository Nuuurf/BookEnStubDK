﻿using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using RestfulApi.Models;

namespace RestfulApi.DAL;

public class DBBooking : IDBBooking
{
    /// <summary>
    ///     Inserts a new booking with its values into the database.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="booking"> the booking object that needs to be persisted</param>
    /// <param name="bookingOrderID"></param>
    /// <param name="transaction"></param>
    /// <returns>A boolean indicading where the action was successful or not</returns>
    public async Task<int> CreateBooking(IDbConnection conn,
        Booking booking,
        int bookingOrderID,
        IDbTransaction transaction = null!)
    {
        string query = @"
                INSERT INTO Booking (TimeStart, TimeEnd, StubId, Notes, BookingOrderID)
                VALUES (@StartTime, @EndTime, @StubId, @Note, @OrderId);
                SELECT SCOPE_IDENTITY();";

        return await conn.ExecuteScalarAsync<int>(query, new { StartTime = booking.TimeStart, EndTime = booking.TimeEnd, StubId = booking.StubId, Note = booking.Notes, OrderId = bookingOrderID }, transaction:transaction);

    }

    public async Task<List<Booking>> GetBookingsInTimeslot(IDbConnection conn, BookingRequestFilter req, IDbTransaction transaction = null!)
    {
        StringBuilder script = new StringBuilder(
            "SELECT b.Id, b.TimeStart, b.TimeEnd, b.Notes, b.StubId, b.BookingOrderID, bo.Id as OrderID" +
            ", c.name as FirstName, c.phone, c.email " +
            "FROM Booking b " +
            "INNER JOIN BookingOrder bo ON b.BookingOrderID = bo.Id " +
            "INNER JOIN Customer c ON bo.customer_id_FK = c.Id " +
            "WHERE b.TimeStart < @TimeEnd AND b.TimeEnd > @TimeStart");


        // Dynamically build the query based on provided filters
        if (req.StubId.HasValue)
            script.Append(" AND b.StubId = @StubId");
        if (req.OrderId.HasValue)
            script.Append(" AND b.BookingOrderID = @OrderId");
        if (!string.IsNullOrWhiteSpace(req.CustomerEmail))
            script.Append(" AND c.email = @CustomerEmail");
        if (!string.IsNullOrWhiteSpace(req.CustomerPhone))
            script.Append(" AND c.phone = @CustomerPhone");

        // Adjust the sorting mechanism
        switch (req.SortOption)
        {
            case BookingSortOption.Date:
                script.Append(" ORDER BY b.TimeStart");
                break;
            case BookingSortOption.OrderId:
                script.Append(" ORDER BY b.BookingOrderID");
                break;
            case BookingSortOption.CustomerId:
                script.Append(" ORDER BY bo.customer_id_FK");
                break;
        }

        var parameters = new
        {
            TimeStart = req.Start,
            TimeEnd = req.End,
            StubId = req.StubId,
            OrderId = req.OrderId,
            CustomerEmail = req.CustomerEmail,
            CustomerPhone = req.CustomerPhone
        };

        List<Booking> bookings;

        try
        {
            var query = script.ToString();
            bookings = (await conn.QueryAsync<Booking, Customer, Booking>(
                query,
                (booking, customer) =>
                {
                    booking.Customer = customer; // Map the customer to the booking
                    return booking;
                },
                parameters, transaction, splitOn: "FirstName")).ToList();
        }
        catch
        {
            throw;

        }

        return bookings;
    }


    public async Task<int> CreateNewBookingOrder(IDbConnection conn, IDbTransaction trans = null!)
    {
        const string insertBookingOrderQuery = "INSERT INTO BookingOrder DEFAULT VALUES; SELECT SCOPE_IDENTITY();";
        int bookingOrderId = -1;

        try
        {
            // Create a new BookingOrder
            CommandDefinition commandDefinition = new CommandDefinition(insertBookingOrderQuery, transaction: trans);
            bookingOrderId = await conn.ExecuteScalarAsync<int>(commandDefinition);
        }
        catch
        {
            // Return fail value if failed to create BookingOrder
            return -1;
        }

        return bookingOrderId;
    }

    public async Task<List<int>> GetBookedStubsForHour(IDbConnection conn, DateTime hour, IDbTransaction transaction = null!)
    {
        string query = @"
                SELECT StubId FROM Booking
                WHERE TimeStart >= @HourStart AND TimeStart < @HourEnd";

        DateTime hourStart = new DateTime(hour.Year, hour.Month, hour.Day, hour.Hour, 0, 0);
        DateTime hourEnd = hourStart.AddHours(1);
        var result = await conn.QueryAsync<int>(query, new { HourStart = hourStart, HourEnd = hourEnd }, transaction : transaction);
        return result.ToList();
    }
    public async Task<List<int>> GetAllStubs(IDbConnection conn, IDbTransaction transaction = null!)
    {
        string query = "SELECT Id FROM Stub";
        var stubs = await conn.QueryAsync<int>(query, transaction : transaction);

        return stubs.ToList();
    }
}