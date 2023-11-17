using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMVVM.Models
{
    public class RoomId
    {
        public int FloorNumber { get; }
        public int RoomNumber { get; }

        public RoomId(int floorNumber, int roomNumber)
        {
            FloorNumber = floorNumber;
            RoomNumber = roomNumber;
        }

        public override string ToString()
        {
            return $"{FloorNumber} {RoomNumber}";
        }

        public override bool Equals(object? obj)
        {
            return obj is RoomId roomId &&
                FloorNumber == roomId.FloorNumber &&
                RoomNumber == roomId.RoomNumber;
        }

        public static bool operator ==(RoomId room1, RoomId room2)
        {
            if (room1 is null && room2 is null)
            {
                return true;
            }

            return room1 is not null && room2.Equals(room2);
        }

        public static bool operator !=(RoomId room1, RoomId room2)
        {
            return !(room1 == room2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FloorNumber, RoomNumber);
        }
    }
}
