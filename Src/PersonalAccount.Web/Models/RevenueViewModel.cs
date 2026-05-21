using System;
using PersonalAccount.Domain.Models.Dto;
using PersonalAccount.Web.Interfaces;

namespace PersonalAccount.Web.Models;

public class RevenueViewModel : RevenueDto, IReportSettings
{
    public Guid BranchId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime Start { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime Stop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
