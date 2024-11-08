using System;
using System.Collections.Generic;

namespace accomondationApp.Models;

public partial class RoomStatus
{
    public int RoomStatusId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
