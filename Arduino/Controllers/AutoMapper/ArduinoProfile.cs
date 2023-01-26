using Arduino.Controllers.Requests;
using Arduino.DomainModel.ProjectAggregate;
using AutoMapper;

namespace Arduino.Controllers.AutoMapper;

public class ArduinoProfile : Profile
{
    public ArduinoProfile()
    {
        CreateMap<Project, ProjectDTO>();
    }
}