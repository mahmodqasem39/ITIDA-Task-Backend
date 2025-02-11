using AutoMapper;
using ITIDATask.DAL.Entities;

namespace ITIDATask.Utitlites
{

    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            //CreateMap<DateTime, DateOnly>().ConvertUsing<DateTimeToDateOnlyConverter>();

            CreateMap<SubmitTimeModel, Timesheet>();
            CreateMap<Timesheet, SubmitTimeModel>();

            CreateMap<Timesheet, TimeSheetModel>()
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.RegisterDate))); 
            CreateMap<TimeSheetModel, Timesheet>()
                   .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate));

            CreateMap<Timesheet, UpdateSubmitedTimetModel>();
            CreateMap<UpdateSubmitedTimetModel, Timesheet>();

        }
    }

    public class DateTimeToDateOnlyConverter : ITypeConverter<DateTime, DateOnly>
    {
        public DateOnly Convert(DateTime source, DateOnly destination, ResolutionContext context)
        {
            return DateOnly.FromDateTime(source);
        }
    }
}
