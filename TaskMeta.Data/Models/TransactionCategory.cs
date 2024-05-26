﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskMeta.Data.Models;

public partial class TransactionCategory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Name { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();
}