using AutoMapper;
using ITIDATask.DAL.Entities;

namespace ITIDATask.Utitlites
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SubmitTimeModel, Timesheet>();
            CreateMap<Timesheet, SubmitTimeModel>();

            CreateMap<Timesheet, TimeSheetModel>();
            CreateMap<TimeSheetModel, Timesheet>();
            CreateMap<Timesheet, UpdateSubmitedTimetModel>();
            CreateMap<UpdateSubmitedTimetModel, Timesheet>();



        }


    }
}
