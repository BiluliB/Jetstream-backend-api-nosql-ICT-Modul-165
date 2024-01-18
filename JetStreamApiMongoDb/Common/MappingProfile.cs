using AutoMapper;
using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.DTOs.Responses;
using JetStreamApiMongoDb.Models;


namespace JetStreamApiMongoDb.Common
{
    public class MappingProfile : Profile
    {
      public MappingProfile()
        {
            CreateMap<OrderSubmission, OrderSubmissionDTO>();
            CreateMap<OrderSubmissionDTO, OrderSubmission>();
            
            CreateMap<OrderSubmission, OrderSubmissionCreateDTO>();
            CreateMap<OrderSubmissionCreateDTO, OrderSubmission>();

            CreateMap<OrderSubmission, OrderSubmissionUpdateDTO>();
            CreateMap<OrderSubmissionUpdateDTO, OrderSubmission>();

            CreateMap<User, UserCreateDTO>();
            CreateMap<UserCreateDTO, User>();

            CreateMap<User, UserLoginDTO>();
            CreateMap<UserLoginDTO, User>();

            CreateMap<Priority, PriorityDTO>();
            CreateMap<Service, ServiceDTO>();
            CreateMap<Status, StatusDTO>();
        }
    }
}
