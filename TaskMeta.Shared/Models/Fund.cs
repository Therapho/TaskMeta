﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskMeta.Shared.Interfaces;

namespace TaskMeta.Shared.Models;

public partial class Fund : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(200)]
    public string Description { get; set; }

    public DateOnly? TargetDate { get; set; }

    public int? Allocation { get; set; }

    [Column(TypeName = "money")]
    public decimal? TargetBalance { get; set; }

    [Column(TypeName = "money")]
    public decimal Balance { get; set; }

    public bool Locked { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; }

    [InverseProperty("SourceFund")]
    public virtual ICollection<TransactionLog> TransactionLogSourceFunds { get; set; } = new List<TransactionLog>();

    [InverseProperty("TargetFund")]
    public virtual ICollection<TransactionLog> TransactionLogTargetFunds { get; set; } = new List<TransactionLog>();

    [NotMapped]
    public DateTime? TargetDateTime
    {
        get
        {
            if(TargetDate == null) return null;
            return TargetDate!.Value.ToDateTime(TimeOnly.FromDateTime(DateTime.MinValue));
        }
        set
        {
            TargetDate = DateOnly.FromDateTime(value.Value);
        }
    }
}
