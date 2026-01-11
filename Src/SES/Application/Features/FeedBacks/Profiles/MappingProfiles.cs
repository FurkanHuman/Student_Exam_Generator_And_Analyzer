using Application.Features.FeedBacks.Commands.Create;
using Application.Features.FeedBacks.Commands.Delete;
using Application.Features.FeedBacks.Commands.Update;
using Application.Features.FeedBacks.Queries.GetById;
using Application.Features.FeedBacks.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.FeedBacks.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateFeedBackCommand, FeedBack>();
        CreateMap<FeedBack, CreatedFeedBackResponse>();

        CreateMap<UpdateFeedBackCommand, FeedBack>();
        CreateMap<FeedBack, UpdatedFeedBackResponse>();

        CreateMap<DeleteFeedBackCommand, FeedBack>();
        CreateMap<FeedBack, DeletedFeedBackResponse>();

        CreateMap<FeedBack, GetByIdFeedBackResponse>();

        CreateMap<FeedBack, GetListFeedBackListItemDto>();
        CreateMap<IPaginate<FeedBack>, GetListResponse<GetListFeedBackListItemDto>>();
    }
}