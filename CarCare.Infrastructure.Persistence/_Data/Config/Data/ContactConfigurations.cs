﻿using CarCare.Core.Domain.Entities.Contacts;
using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Infrastructure.Persistence._Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Infrastructure.Persistence._Data.Config.Data
{
	internal class ContactConfigurations : BaseAuditableEntityConfigurations<Contact, int>
	{
		public override void Configure(EntityTypeBuilder<Contact> builder)
		{
			base.Configure(builder);

			//builder.HasKey(c => c.Id);
			//builder.Property(c => c.Id).UseIdentityColumn();

			builder.Property(c => c.Message)
				   .HasColumnType("nvarchar")
				   .HasMaxLength(200);

			builder.Property(user => user.MessageFor)
				.HasConversion
				(
				(UStatus) => UStatus.ToString(),
				(UStatus) => (Types)Enum.Parse(typeof(Types), UStatus)
				);

		}
	}
}
