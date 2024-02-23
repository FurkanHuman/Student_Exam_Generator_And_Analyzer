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
    public string ReferenceBenefitSeason { get; set; }
    public int SchoolYear { get; set; }
    public int EndSchcoolYear { get; set; }

    public int SchoolId { get; set; }
    public int ExamId { get; set; }
    public int LearningAreaId { get; set; }

    public virtual IList<QuizQuestion> QuizQuestions { get; set; }
    public virtual IList<LearningArea> LearningAreas { get; set; }
    public virtual School School { get; set; }
}
