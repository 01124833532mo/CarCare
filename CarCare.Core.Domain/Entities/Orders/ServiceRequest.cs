﻿using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Entities.Common;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders.ServicesDetails;
using CarCare.Core.Domain.Entities.ServiceTypes;

namespace CarCare.Core.Domain.Entities.Orders
{
    public class ServiceRequest : BaseAuditableEntity<int>, IBaseUserId
    {
        public required string UserId { get; set; }
        public virtual required ApplicationUser User { get; set; }

        public string TechId { get; set; }
        public virtual required ApplicationUser Technical { get; set; }

        public int ServiceTypeId { get; set; }
        public virtual required ServiceType ServiceType { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public BusnissStatus BusnissStatus { get; set; } = BusnissStatus.Pending;

        public bool IsAutomatic { get; set; } = false;

        public TireSize? TireSize { get; set; }


        public TypeOfFuel? TypeOfFuel { get; set; }


        public BettaryType? BettaryType { get; set; }


        public TypeOfOil? TypeOfOil { get; set; }

        public TypeOfWinch? TypeOfWinch { get; set; }

        public int ServiceQuantity { get; set; } = 1;

        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
        public double Distance { get; set; }

        public decimal ServicePrice { get; set; }
        public int BasePrice { get; set; } = 5;
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public string? JopId { get; set; }


    }
}
