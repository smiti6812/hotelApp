using System;
using System.Collections.Generic;

namespace accomondationApp.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? RoomNumber { get; set; }

    public int? RoomCapacityId { get; set; }

    public int? RoomStatusId { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual RoomCapacity? RoomCapacity { get; set; }

    public virtual RoomStatus? RoomStatus { get; set; }
}
