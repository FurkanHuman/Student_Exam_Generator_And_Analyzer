using Application.Services.Repositories;
using Application.Services.StudentClasses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Students.Commands.GhostCreate;

public class CreateGhostStudentCommand : IRequest<CreatedGhostStudentResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required int SchoolId { get; set; }
    public required int SemesterId { get; set; }
    public required int RefTeacherId { get; set; }
    public required int ClassAge { get; set; }
    public required char ClassBranch { get; set; }

    public string? SchoolNumbers { get; set; }
    public int FemaleCount { get; set; }
    public int MaleCount { get; set; }
    public bool GenerateRandomNumbers { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetStudents"];

    public class CreateGhostStudentCommandHandler : IRequestHandler<CreateGhostStudentCommand, CreatedGhostStudentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentClassService _studentClassService;

        public CreateGhostStudentCommandHandler(
            IMapper mapper,
            IStudentRepository studentRepository,
            IStudentClassService studentClassService)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _studentClassService = studentClassService;
        }

        public async Task<CreatedGhostStudentResponse> Handle(CreateGhostStudentCommand request, CancellationToken cancellationToken)
        {
            // todo: create ghost student class but already exist check is not needed. after control logic will be added to service layer if needed.
            StudentClass ghostStudentClass = await _studentClassService.AddAsync(new()
            {
                Name = "Hayalet Öðrenci Sýnýfý",
                ClassAge = request.ClassAge,
                ClassBranch = request.ClassBranch,
                SchoolId = request.SchoolId,
                SemesterId = request.SemesterId,
                RefTeacherId = request.RefTeacherId
            });

            List<Student> ghostStudents = [];
            List<NumberMapping> numberMappings = [];

            List<string> schoolNumbers = [];
            if (!string.IsNullOrWhiteSpace(request.SchoolNumbers) && !request.GenerateRandomNumbers)
            {
                schoolNumbers = [.. request.SchoolNumbers
                    .Split(',')
                    .Select(n => n.Trim())
                    .Where(n => !string.IsNullOrWhiteSpace(n))];
            }

            int currentNumberIndex = 0;

            for (int i = 0; i < request.FemaleCount; i++)
            {
                string schoolNumber = GetSchoolNumber(
                    schoolNumbers,
                    ref currentNumberIndex,
                    request.GenerateRandomNumbers,
                    "K");

                string maskedNumber = MaskSchoolNumber(schoolNumber);

                ghostStudents.Add(new()
                {
                    SchoolNumber = maskedNumber,
                    Name = $"Hayalet_K_{i + 1}",
                    SurName = $"Öðrenci",
                    Gender = 'K',
                    SchoolId = ghostStudentClass.SchoolId,
                    StudentClassId = ghostStudentClass.Id,
                    Description = "Hayalet Öðrenci - Gizlilik Amaçlý",
                    IsGhostStudent = true

                });

                numberMappings.Add(new NumberMapping
                {
                    OriginalNumber = schoolNumber,
                    MaskedNumber = maskedNumber,
                    Gender = "Kýz",
                    StudentName = $"Hayalet_K_{i + 1} Öðrenci"
                });
            }

            for (int i = 0; i < request.MaleCount; i++)
            {
                string schoolNumber = GetSchoolNumber(
                    schoolNumbers,
                    ref currentNumberIndex,
                    request.GenerateRandomNumbers,
                    "E");

                string maskedNumber = MaskSchoolNumber(schoolNumber);

                ghostStudents.Add(new()
                {
                    SchoolNumber = maskedNumber,
                    Name = $"Hayalet_E_{i + 1}",
                    SurName = $"Öðrenci",
                    Gender = 'E',
                    SchoolId = ghostStudentClass.SchoolId,
                    StudentClassId = ghostStudentClass.Id,
                    Description = "Hayalet Öðrenci - Gizlilik Amaçlý",
                    IsGhostStudent = true
                });

                numberMappings.Add(new NumberMapping
                {
                    OriginalNumber = schoolNumber,
                    MaskedNumber = maskedNumber,
                    Gender = "Erkek",
                    StudentName = $"Hayalet_E_{i + 1} Öðrenci"
                });
            }

            await _studentRepository.AddRangeAsync(ghostStudents, cancellationToken);

            return new CreatedGhostStudentResponse
            {
                ClassId = ghostStudentClass.Id,
                ClassName = $"{ghostStudentClass.ClassAge}/{ghostStudentClass.ClassBranch}",
                TotalStudents = ghostStudents.Count,
                FemaleCount = request.FemaleCount,
                MaleCount = request.MaleCount,
                NumberMappings = numberMappings,
                Students = _mapper.Map<List<GhostStudentDto>>(ghostStudents)
            };
        }

        private static string GetSchoolNumber(
            List<string> providedNumbers,
            ref int currentIndex,
            bool generateRandom,
            string genderPrefix)
        {
            if (generateRandom || providedNumbers.Count == 0)
                return $"{genderPrefix}{Random.Shared.Next(10000000, 99999999)}";


            if (currentIndex < providedNumbers.Count)
                return providedNumbers[currentIndex++];

            return $"{genderPrefix}{Random.Shared.Next(10000000, 99999999)}";
        }

        private static string MaskSchoolNumber(string schoolNumber)
        {
            if (string.IsNullOrWhiteSpace(schoolNumber))
                return "MASKED_000000";

            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(schoolNumber));
            string hash = Convert.ToBase64String(hashBytes);

            string masked = new([.. hash.Where(char.IsLetterOrDigit).Take(10)]);

            return $"MASK_{masked}";
        }
    }
}