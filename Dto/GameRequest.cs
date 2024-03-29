﻿using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class GameRequest
    {
        [Required(ErrorMessage = "Please enter room name")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string GameName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter room number")]
        [MaxLength(200, ErrorMessage = "Do not enter more than 200 characters")]
        public string Description { get; set; } = null!;
    }
}
