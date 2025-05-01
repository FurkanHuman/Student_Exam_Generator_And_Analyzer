using Application.Features.ReferenceBenefits.Commands.Create;
using Application.Features.ReferenceBenefits.Commands.Delete;
using Application.Features.ReferenceBenefits.Commands.Update;
using Application.Features.ReferenceBenefits.Queries.GetById;
using Application.Features.ReferenceBenefits.Queries.GetList;
using Application.Features.ReferenceBenefits.Queries.GetListByIdReferenceBenefitBenefit;
using Application.Services.PdfFactory.PdfReaderService.Dtos;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.ReferenceBenefits.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, CreatedReferenceBenefitResponse>();

        CreateMap<ReferenceBenefit, ReferenceBenefitDto>()
            .ForMember(destinationMember: rbd => rbd.RBName, memberOptions: opt => opt.MapFrom(rb => rb.ReferenceBenefitName))
            .ForMember(destinationMember: rbd => rbd.LessonId, memberOptions: opt => opt.MapFrom(rb => rb.LessonId))
            .ForMember(destinationMember: rbd => rbd.SchoolId, memberOptions: opt => opt.MapFrom(rb => rb.SchoolId))
            .ForMember(destinationMember: rbd => rbd.SemesterId, memberOptions: opt => opt.MapFrom(rb => rb.SemesterId));

        CreateMap<ReferenceBenefitDto, ReferenceBenefit>()
            .ForMember(destinationMember: rb => rb.ReferenceBenefitName, memberOptions: opt => opt.MapFrom(rbd => rbd.RBName));

        CreateMap<UpdateReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, UpdatedReferenceBenefitResponse>();

        CreateMap<DeleteReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, DeletedReferenceBenefitResponse>();

        CreateMap<ReferenceBenefit, GetByIdReferenceBenefitResponse>();

        CreateMap<ReferenceBenefit, List<GetListByIdReferenceBenefitBenefitDto>>()
            .ConvertUsing(rb => rb.LearningAreas
                .SelectMany(la => la.SubLearningAreas)
                .SelectMany(sla => sla.Benefits)
                .Select(b => new GetListByIdReferenceBenefitBenefitDto
                {
                    Id = b.Id,
                    BenefitCode = b.BenefitCode,
                    Description = b.Description
                }).ToList());

        CreateMap<ReferenceBenefit, GetListReferenceBenefitListItemDto>()
            .ForMember(destinationMember: lid => lid.Name, memberOptions: opt => opt.MapFrom(rb => rb.ReferenceBenefitName))
            .ForMember(destinationMember: lid => lid.SemesterName, memberOptions: opt => opt.MapFrom(rb => rb.Semester.Name))
            .ForMember(destinationMember: lid => lid.BeginSemesterDate, memberOptions: opt => opt.MapFrom(rb => rb.Semester.BeginSemesterDate))
            .ForMember(destinationMember: lid => lid.EndSemesterDate, memberOptions: opt => opt.MapFrom(rb => rb.Semester.EndSemesterDate))
            .ForMember(destinationMember: lid => lid.SchoolName, memberOptions: opt => opt.MapFrom(rb => rb.School.Name))
            .ForMember(destinationMember: lid => lid.LessonName, memberOptions: opt => opt.MapFrom(rb => rb.Lesson.LessonName));

        CreateMap<IPaginate<ReferenceBenefit>, GetListResponse<GetListReferenceBenefitListItemDto>>();
    }
}