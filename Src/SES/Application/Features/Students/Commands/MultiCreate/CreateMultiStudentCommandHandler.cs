using Application.Features.Students.Rules;
using Application.Services.PdfReaderService;
using Application.Services.Repositories;
using Application.Services.StudentClasses;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Students.Commands.MultiCreate;

public class CreateMultiStudentCommandHandler : IRequestHandler<CreateMultiStudentCommand, ICollection<CreatedMultiStudentResponse>>
{
    private readonly IMapper _mapper;
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentClassService _studentClassService;
    private readonly IPdfReaderService _pdfReaderService;
    private readonly StudentBusinessRules _studentBusinessRules;

    public CreateMultiStudentCommandHandler(IMapper mapper, IStudentRepository studentRepository, IStudentClassService studentClassService, IPdfReaderService pdfReaderService, StudentBusinessRules studentBusinessRules)
    {
        _mapper = mapper;
        _studentRepository = studentRepository;
        _studentClassService = studentClassService;
        _pdfReaderService = pdfReaderService;
        _studentBusinessRules = studentBusinessRules;
    }

    public async Task<ICollection<CreatedMultiStudentResponse>> Handle(CreateMultiStudentCommand request, CancellationToken cancellationToken)
    {
        ICollection<Services.PdfReaderService.Dtos.ClassWithStudentsDto> extractStudentAndClasess = await _pdfReaderService.GetAllClassesAndStudents(request.PdfFile);
        ICollection<Student> createStudents = [];

        foreach (Services.PdfReaderService.Dtos.ClassWithStudentsDto classWithStudents in extractStudentAndClasess)
        {
            
            StudentClass autocreatedStudentClass = await _studentClassService.AddAsync(new()
            {
                Name = "Auto Added",
                ClassAge = classWithStudents.Age,
                ClassBranch = classWithStudents.Section,
                SchoolId = request.SchoolId,
                SemesterId = request.SemesterId,
                RefTeacherId = request.RefTeacherId
            });

            foreach (string[] sArray in classWithStudents.Students)
            {
                createStudents.Add(new()
                {
                    SchoolNumber = sArray[0],
                    Name = sArray[1],
                    SurName = sArray[2],
                    Gender = sArray[3][0],
                    SchoolId = autocreatedStudentClass.SchoolId,
                    StudentClassId = autocreatedStudentClass.Id,
                    Description = "Auto Added"
                });
            }
        }

        ICollection<Student> addedStudents = await _studentRepository.AddRangeAsync(createStudents, cancellationToken);

        return _mapper.Map<ICollection<CreatedMultiStudentResponse>>(addedStudents);
    }
}

