using System;
using System.Collections.Generic;

namespace accomondationApp.Models;

public partial class RoomCapacity
{
    /// <summary>
    /// it shows the number of beds
    /// </summary>
    public int RoomCapacityId { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
