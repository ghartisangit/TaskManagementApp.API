using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.DTOs.Auth;
using TaskManagementApp.Application.DTOs.Project;
using TaskManagementApp.Application.DTOs.Task;
using TaskManagementApp.Domain.Entities;
namespace TaskManagementApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Auth
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, LoginUserDto>().ReverseMap();

            // Project
            CreateMap<Project, ProjectCreateDto>().ReverseMap();
            CreateMap<Project, ProjectUpdateDto>().ReverseMap();
            CreateMap<Project, ProjectDetailDto>().ReverseMap();
            CreateMap<Project, ProjectProgressDto>().ReverseMap();

           

            CreateMap<Project, ProjectDetailDto>()
               .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager.FullName))
               .ForMember(dest => dest.AssignedDevelopers, opt => opt.MapFrom(
                   src => src.ProjectDevelopers
                       .Select(pd => pd.Developer.FullName)
                       .Distinct()
                       .ToList()
               ))
               .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));

            // Task
            CreateMap<TaskItem, TaskCreateDto>().ReverseMap();
            CreateMap<TaskItem, TaskUpdateDto>().ReverseMap();
            CreateMap<TaskItem, TaskDetailDto>().ReverseMap();
            CreateMap<TaskItem, TaskStatusUpdateDto>().ReverseMap();

            CreateMap<TaskItem, TaskDetailDto>()
           .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo.FullName))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new TaskStatussDto
           {
               Id = (int)src.Status,
               Name = src.Status.ToString()
           }));
            CreateMap<TaskRequest, TaskRequestCreateDto>().ReverseMap();

            CreateMap<TaskRequest, TaskRequestResponseDto>()
             .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.RequestedBy.FullName))
             .ForMember(dest => dest.ProjectTitle, opt => opt.MapFrom(src => src.Project.Title))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new RequestStatusDto
             {
                 Id = (int)src.Status,
                 Name = src.Status.ToString()
             }));
        }
    }
}
