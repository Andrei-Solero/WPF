using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFMVVM.Exceptions;
using WPFMVVM.Models;

namespace WPFMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Hotel hotel = new Hotel("Sweet home alabama");

            //try
            //{
            //    hotel.MakeReservation(new Reservation(
            //            new RoomId(1, 3),
            //            "Andrei",
            //            new DateTime(2000, 1, 1),
            //            new DateTime(2000, 1, 2)));

            //    hotel.MakeReservation(new Reservation(
            //        new RoomId(1, 3),
            //        "Kaeceline",
            //        new DateTime(2000, 1, 1),
            //        new DateTime(2000, 1, 2)));

                
            //}
            //catch (ReservationConflictException ex)
            //{

            //    throw;
            //}

            //IEnumerable<Reservation> reservations = hotel.GetReservationsFor("Andrei");

            base.OnStartup(e);
        }
    }
}
