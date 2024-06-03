﻿using System.ComponentModel.DataAnnotations;

namespace AddressBook.Core.Entities
{
    public class Job : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
