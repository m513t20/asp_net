using System;
using PersonalAccount.Domain.Models.Dto;
using PersonalAccount.Web.Interfaces;

namespace PersonalAccount.Web.Models;

public class WorkScheduleViewModel : WorkScheduleDto, IReportSettings
{
    public Guid BranchId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    DateTime IReportSettings.Start { get => throw new NotImplementedException(); set => Start = value; }
    DateTime IReportSettings.Stop { get => throw new NotImplementedException(); set => Stop = value; }
}
