// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");


//ServiceProvider serviceProvider = new ServiceCollection()
//    .AddAppServiceRegistration()
//    .BuildServiceProvider();

//QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;
//QuestPDF.Settings.License = LicenseType.Community;

////Document.Create(document =>
////{
////    document.Page(page =>
////    {
////        page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));
////        page.Margin(0.5f, Unit.Centimetre);
////        page.Size(pageSize: PageSizes.A4);
////        page.Header().Height(3, Unit.Centimetre).Row(headerRow =>
////        {
////            headerRow.RelativeItem(2).Padding(0.2f, Unit.Centimetre).Column(nametag =>
////            {
////                nametag.Item().DefaultTextStyle(ts => ts.FontSize(9));

////                nametag.Item().AlignCenter().Text(Placeholders.Name());
////                nametag.Item().AlignCenter().Text(Placeholders.Name());
////                nametag.Item().AlignCenter().Text($"{Placeholders.Decimal()}");

////            });

////            headerRow.RelativeItem(5).Padding(0.2f, Unit.Centimetre).Column(generalInfo =>
////            {
////                generalInfo.Item().DefaultTextStyle(ts => ts.FontSize(13));

////                generalInfo.Item().AlignCenter().Text("2023 - 2024");
////                generalInfo.Item().AlignCenter().Text(Placeholders.Label());
////                generalInfo.Item().AlignCenter().Text(Placeholders.Label());
////                generalInfo.Item().AlignCenter().Text(Placeholders.PhoneNumber()).FontSize(12);
////            });

////            headerRow.RelativeItem(1.5f).Padding(0.2f, Unit.Centimetre).Column(examSummary =>
////            {
////                examSummary.Item().AlignCenter().Text("FFF").FontSize(10);
////                examSummary.Item().AlignCenter().Text("Puan").Underline().FontSize(14); // Note: ingilizce sonra gelecek
////                examSummary.Item().AlignCenter().Text("").FontSize(25); // note : boş çünkü otomatik analizci yapılmadı.
////            });
////        });

////        page.Content().PaddingTop(0.4f, Unit.Centimetre).PaddingBottom(0.4f, Unit.Centimetre)
////        .Column(questionContentColumn =>
////        {
////            questionContentColumn.Spacing(0.4f, Unit.Centimetre);

////            for (int i = 0; i < 10; i++)

////            questionContentColumn.Item().Row(questionContentColumnRow =>
////            {

////                if (i % 4 == 0)
////                {
////                    questionContentColumnRow.RelativeItem(14).Height(2,Unit.Centimetre).AlignMiddle().AlignLeft().Text($"{i + 1}) " + Placeholders.Question());
////                }
////                else
////                {

////                    if (i % 2 == 0)
////                    {
////                        questionContentColumnRow.RelativeItem(1.8f).AlignMiddle().AlignCenter().Image("C:\\Users\\furka\\Desktop\\kelebek.png").WithCompressionQuality(ImageCompressionQuality.Best);
////                    }
////                    else
////                        questionContentColumnRow.RelativeItem(1.8f).AlignMiddle().AlignCenter().Image("C:\\Users\\furka\\Desktop\\tasarim.jpg").FitArea().WithCompressionQuality(ImageCompressionQuality.Best);

////                    questionContentColumnRow.RelativeItem(0.2f);
////                    questionContentColumnRow.RelativeItem(14).AlignMiddle().AlignLeft().Text($"{i + 1}) " + Placeholders.Question());
////                }
////            });

////        });

////        page.Footer().Height(1, Unit.Centimetre).Row(footerNotes =>
////        {
////            footerNotes.RelativeItem(16).Text("TEST TEST TEST ").FontSize(9);
////            footerNotes.RelativeItem(1).AlignMiddle().AlignCenter().Text("FFF").FontSize(10);
////        });
////    });
////}).ShowInPreviewer();
//IAnalsysPage analsysPage = serviceProvider.GetRequiredService<IAnalsysPage>();

//IList<StudentQuizAnswer> quizForAnswers = new List<StudentQuizAnswer>();

//// analsysPage.AnalysisPageGenerateAsync(quizForAnswers).ShowInPreviewer();

//IQuizPool quizPool = serviceProvider.GetRequiredService<IQuizPool>();

////Console.Write("Lütfen sınav adını girin: ");
////string? quizName = Console.ReadLine();


////Console.Write("Lütfen okul adını girin: ");
////string? schoolName = Console.ReadLine();

////Console.Write("Lütfen SEED girin: ");
////int seed = int.Parse(Console.ReadLine());

//string? quizName = "BİLİŞİM 6.SINIFLAR 1.DÖNEM 2.YAZILI";
//string? schoolName = "HACIİLBEY MENSUCAT SANTRAL ORTAOKULU";
//IList<Student> students = serviceProvider.GetRequiredService<IAddStudent>().AddStudentsLoadFromJsonFile("C:\\Users\\furka\\Desktop\\students.json").Where(s => s.ClassAge == 6).ToList();


//quizPool.QuestionPoolAddQuiz(quizName);
//IList<QuizQuestion> quizPoolLoad = quizPool.GetQuestionPoolForName(quizName);

//List<QuizQuestion> quizQuestions = new();

//IExamPage page = serviceProvider.GetRequiredService<IExamPage>();
//School school = new() { Id = 1, Name = schoolName };
//List<School> schools = [school];
//Teacher teacher = new() { Id = 1, Name = "Emre", SurName = "Dumancı", Schools = schools, Students = students, SchoolId = 1 };

//Random r = new(0);
//for (int i = 0; i < 5; i++)
//    Console.WriteLine(r.Next(0xFFFF).ToString("X3"));

//IList<Exam> exams = new List<Exam>();

//int examId = 0;
//int examCode = r.Next(0xABC);
//foreach (Student student in students)
//{
//    Random r2 = new(examCode);
//    examId++;

//    Exam exam = new()
//    {
//        Id = examId,
//        ExamSemesterYear = "2023 - 2024",
//        LessonName = "BİLİŞİM DERSİ",
//        Semester = "1. Dönem",
//        SemesterSesion = "2. Sınav",
//        ExamCode = examCode.ToString("X3"),
//        FooterNote = "Başarılar",

//        TeacherId = teacher.Id,
//        StudentId = student.Id,
//        SchoolId = school.Id,

//        Teacher = teacher,
//        Student = student,
//        School = school,
//        QuizQuestions = quizPoolLoad.OrderBy(_ => r2.Next(quizPoolLoad.Count)).Take(14).ToList()
//    };

//    exams.Add(exam);
//}

//IList<IDocument> documents = new List<IDocument>();
//Console.Clear();
//foreach (Exam exam in exams)
//{
//    string s = JsonSerializer.Serialize(exam);
//    documents.Add(page.FrontPageGenerate(exam));
//    documents.Add(page.BackPageGenerate(exam));

//    Console.WriteLine(s);
//}









////Exam exam = new();

////IList<Student> sts = serviceProvider.GetRequiredService<IAddStudent>().AddStudentsLoadFromJsonFile("C:\\Users\\furka\\Desktop\\students.json");

////exam.School = schoolService.AddSchool(ref exam);

////Console.WriteLine(exam.School.Name);

////exam.ExamCode = "fff".ToUpper();
////exam.ExamSemesterYear = "Demo Year";
////exam.QuizQuestions = quizPool;

////    page.BackPageGenerate(exam)
////};
//string sumaryExam = JsonSerializer.Serialize(exams);

//File.WriteAllText(Environment.CurrentDirectory + "\\Exam2.json", sumaryExam);
//Console.WriteLine(sumaryExam);

//Document.Merge(documents).ShowInPreviewer();
//Console.Clear();
