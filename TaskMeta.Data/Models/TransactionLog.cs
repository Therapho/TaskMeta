﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskMeta.Data.Models;

[Table("TransactionLog")]
public partial class TransactionLog
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(50)]
    public string Description { get; set; }

    public int CategoryId { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(450)]
    public string TargetUserId { get; set; }

    [Required]
    [StringLength(450)]
    public string CallingUserId { get; set; }

    public int? SourceFundId { get; set; }

    public int? TargetFundId { get; set; }

    [Column(TypeName = "money")]
    public decimal? PreviousAmount { get; set; }

    [ForeignKey("SourceFundId")]
    [InverseProperty("TransactionLogSourceFunds")]
    public virtual Fund SourceFund { get; set; }

    [ForeignKey("TargetFundId")]
    [InverseProperty("TransactionLogTargetFunds")]
    public virtual Fund TargetFund { get; set; }
}