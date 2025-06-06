﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace web_api.Models;

public partial class Transaction
{
    /// <summary>
    /// Transaction unique identification
    /// </summary>
    public long TransactionId { get; set; }

    /// <summary>
    /// User unique identification with reference to User table
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Account unique identification with reference to Account table
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// Category unique identification with reference to Category table
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// Transaction money amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Transaction comment
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Transaction DateTime
    /// </summary>
    public DateTime CreatedDateTime { get; set; }

    public virtual Account Account { get; set; }

    public virtual Category Category { get; set; }

    public virtual User User { get; set; }
}