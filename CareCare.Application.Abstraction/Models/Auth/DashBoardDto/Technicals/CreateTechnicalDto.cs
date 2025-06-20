﻿using CarCare.Core.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals
{
    public class CreateTechnicalDto
    {


        [Required]
        public required string Name { get; set; }



        [Required]
        public required string PhoneNumber { get; set; }

        [Required]
        //[RegularExpression("(?=^.{6,10}$)(?=.\\d)(?=.[a-z])(?=.[A-Z])(?=.[!@#%^&amp;()_+}{&quot;:;'?/&gt;.&lt;,])(?!.\\s).*$",
        //					ErrorMessage = "Password must have 1 UpperCase,1 LowerCase,1 number , 1 non alphanumberic and at least 6 characters ")]
        public required string Password { get; set; }

        public required string Email { get; set; }
        public required string NationalId { get; set; }

        //to do Service

        public int? ServiceId { get; set; }


        public Types Type = Types.Technical;

    }
}
