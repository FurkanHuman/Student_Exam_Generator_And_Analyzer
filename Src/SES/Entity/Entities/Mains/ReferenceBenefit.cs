using Entity.Base;

namespace Entity.Entities.Mains;

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
    public int SchoolYear { get; set; }
    public int EndSchcoolYear { get; set; }

    public int SchoolId { get; set; }
    public int ExamId { get; set; }
    public int LearningAreaId { get; set; }

    public virtual IList<QuizQuestion> QuizQuestions { get; set; }
    public virtual IList<LearningArea> LearningAreas { get; set; }
    public virtual School School { get; set; }
    public virtual Exam Exam { get; set; }
}

public class LearningArea : Entity<int>
{
    public required string Name { get; set; }

    public int SubLearningAreaId { get; set; }

    public IList<SubLearningArea> SubLearningAreas { get; set; }
}
public class SubLearningArea : Entity<int>
{
    public required string Name { get; set; }

    public int BenefitId { get; set; }

    public virtual IList<Benefit> Benefits { get; set; }
}

public class Benefit : Entity<int>
{
    public int SubLearningAreaId { get; set; }

    public string ReferenceBenefitNumber { get; set; }
    public string ReferenceBenefitComments { get; set; }

    public virtual SubLearningArea SubLearningArea { get; set; }

}