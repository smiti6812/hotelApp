﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace accomondationApp.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int? RommId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? CustomerId { get; set; }

    public int? PaymentStatusId { get; set; }

    public DateTime ReservationDate { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual PaymentStatus PaymentStatus { get; set; }

    public virtual Room Romm { get; set; }
}