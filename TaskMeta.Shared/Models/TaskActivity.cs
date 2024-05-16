﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMeta.Shared.Models;

public partial class TaskActivity
{
    [Key]
    public int Id { get; set; }

    public int Sequence { get; set; }

    public int TaskWeekId { get; set; }

    public bool Complete { get; set; }

    public int TaskDefinitionId { get; set; }

    [Required]
    [StringLength(50)]
    public string Description { get; set; }

    [Column(TypeName = "money")]
    public decimal Value { get; set; } = 0;
    public DateOnly TaskDate { get; set; }

    [ForeignKey("TaskWeekId")]
    [InverseProperty("TaskActivities")]
    public virtual TaskWeek TaskWeek { get; set; }
}