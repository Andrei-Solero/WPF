using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMVVM.Models
{
    public class Reservation
    {
        public RoomId RoomId { get; }
        public string Username { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan Length => EndTime.Subtract(StartTime); 

        public Reservation(RoomId roomId, string username, DateTime startTime, DateTime startEnd)
        {
            RoomId = roomId;
            Username = username; 
            StartTime = startTime;
            EndTime = startEnd;
        }

        public  bool Conflicts(Reservation reservation)
        {
            if (!reservation.RoomId.Equals(RoomId))
            {
                return false;
            }

            return reservation.StartTime < reservation.EndTime && reservation.EndTime > StartTime;
        }
    }
}
