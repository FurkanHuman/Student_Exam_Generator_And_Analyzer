using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class ReferenceBenefit : Entity<int>
{
    /*
     * şehirler lere göre değişir
     * kaazanımlara göre soru hazırlanır.
     * many to many sorular ve kazanımlar.
     * tema kavramı var. özel obje
     * SABİT seçeneğeni koy.
     * karıştırma kodu  analizci tarafından öğretmene sınav kağıdındaki gibi girdi sırsaını 
     * sınavı kaydet jsonm dformatyında exam list olarak kayıt et.ve onu analayzer e gönder.
     * yıl kavramı 
     * sınavlar ve öğretmenler many to many
     * ders adı
     * yıl bitmeden değişmez.
     * konu ve numara ekle
     * öğrenci güncelleme her şey. okul numarası 
     */

    public string ReferenceBenefitName { get; set; }
    public int LessonId { get; set; }
    public int SchoolId { get; set; }
    public int SemesterId { get; set; }

    public Lesson Lesson { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<QuizQuestion> QuizQuestions { get; set; }
    public IList<LearningArea> LearningAreas { get; set; }
}
