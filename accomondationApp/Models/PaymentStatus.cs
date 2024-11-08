﻿using System;
using System.Collections.Generic;

namespace accomondationApp.Models;

public partial class PaymentStatus
{
    public int PaymentStatusId { get; set; }

    public string? PaymentStatus1 { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
