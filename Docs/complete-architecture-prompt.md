# SES Projesi - Mimari Tasarım Dokümanı

## Doküman Hakkında

### Bu Doküman Nedir?

Bu doküman, SES (Sınav/Exam Sistemi) projesinin .NET 10'a geçiş sürecinde uygulanacak mimari kararların, tasarım prensiplerinin ve yapısal dönüşümlerin kayıt altına alındığı bir **mimari niyet ve tasarım belgesidir**.

Doküman şunları içerir:

- Mevcut durum analizi ve hedef mimari
- Tasarım prensipleri ve kısıtlar
- Katman sorumlulukları ve bağımlılık kuralları
- Teknoloji seçimleri ve gerekçeleri
- Bilinçli olarak alınan mimari kararlar
- Yapılmayacaklar ve gerekçeleri

### Bu Doküman Ne Değildir?

- ❌ Kod üretim şablonu değildir
- ❌ Otomatik proje oluşturma talimatı değildir
- ❌ Copy-paste edilebilir implementation guide değildir
- ❌ AI'ya yetki devreden bir prompt değildir

Bu doküman, ekip tarafından tartışılmış, analiz edilmiş ve kararlaştırılmış mimari tercihlerin yazılı kaydıdır. Implementation aşamasında rehber olarak kullanılması amaçlanmıştır.

---

## Proje Bağlamı

### Mevcut Durum

- **Framework:** .NET 8
- **Mimari:** Katmanlı mimari (Application, Domain, Infrastructure, Persistence, BlazorWebUI)
- **Temel Sorunlar:**
  - Modüller arası sıkı bağlılık (tight coupling)
  - MediatR bağımlılığı
  - Cross-cutting concerns yönetimi karmaşık
  - Audit/trace mekanizması eksik
  - Vertical slice organizasyonu yok
  - Domain entity'lere doğrudan bağımlılık

### Hedef Durum

- **Framework:** .NET 10 LTS (2025-2028)
- **Dil:** C# 14 (yeni dil özellikleri roadmap'te)
- **Mimari Tipi:** Monolith-Hybrid (Vertical Slice + Plugin Architecture)
- **Temel Hedefler:**
  - Modül izolasyonu (interface'lere bağımlılık)
  - Domain-centric vertical slices
  - Plugin-based infrastructure
  - Audit/trace için event history
  - Bakım maliyetinin azaltılması

---

## Mimari Prensipler ve Kısıtlar

### 1. Tam İzolasyon Prensibi

**Niyet:** Her katman sadece bir alt katmana referans verir. Modüller birbirinden izole çalışır.

**Bağımlılık Hiyerarşisi:**

```
Host → Application.Services (sadece interface'ler)
Application.Services → ∅ (hiçbir şey, pure interfaces)
Application.Contracts → Application.Services
Application.Modules → Application.Services + Application.Contracts
Infrastructure Plugins → Application.Services
Persistence Modules → Application.Services + Domain
Domain → ∅ (pure domain)
```

**Kritik Kısıt:** Module'ler Infrastructure, Persistence veya diğer module'lere doğrudan referans veremez.

### 2. Vertical Slice Organizasyonu

**Niyet:** Her domain konsepti kendi izole modülünde organize edilir.

**Yapı:**

- `Application.Exam/` → Exam domain'i
- `Application.Analysis/` → Analysis domain'i
- `Application.Question/` → Question domain'i

**Kısıt:** Module'ler birbirini göremez. Cross-module communication sadece `Application.Contracts` üzerinden Query (okuma) operasyonları ile sağlanır.

### 3. Plugin Architecture

**Niyet:** Infrastructure implementasyonları (Logger, Cache, AI provider) runtime'da değiştirilebilir plugin'ler olarak yapılandırılır.

**Mekanizma:**

- Infrastructure implementasyonları `[ServiceImplementation("key")]` attribute ile işaretlenir
- Host, bu implementasyonları compile-time veya runtime'da keşfeder
- `appsettings.json` üzerinden hangi implementasyonun kullanılacağı belirlenir (Keyed DI)

**Örnek:**

```json
{
  "ServiceProviders": {
    "Logger": "serilog",
    "Cache": "redis",
    "AI": "openai"
  }
}
```

### 4. Event History (Audit/Trace)

**Niyet:** Her entity'nin oluşturulma, güncellenme ve silinme zamanları audit/trace amacıyla kaydedilir.

**⚠️ ÖNEMLİ NETLEŞTIRME:**

- Bu **Event Sourcing değildir**
- Bu **CQRS pattern'i değildir**
- Bu sadece **audit trail / debug / compliance** amaçlıdır
- Entity'nin mevcut durumu (current state) yine entity tablosundadır
- Event tablosu sadece metadata (kim, ne zaman, ne yaptı) tutar

**Yapı:**

- Her entity için 1:1 ilişkili ayrı event tablosu
- `ExamEvent` → `Exam` (1:1)
- `QuestionEvent` → `Question` (1:1)

**Event Tablosu İçeriği:**

- `CreatedDate`, `UpdatedDate`, `DeletedDate`
- `IsDeleted` (soft delete flag)
- `CreatedBy`, `ModifiedBy`
- İleride genişletilebilir: versioning, change log

### 5. Cross-Cutting Concerns (CCC)

**Niyet:** Caching, logging, validation gibi cross-cutting concern'ler interface-based olarak uygulanır.

**Kısıt:** Attribute-based CCC tercih edilmez (attribute hell riski). Bunun yerine interface'ler kullanılır:

- `ICached` → Service cache'lenebilir olarak işaretlenir
- `ILogged` → Service loglanır
- `IValidated` → Service input validation uygular
- `IResilient` → Service retry/timeout uygular (sadece dış API'ler için)

**Mekanizma:** Source Generator, bu interface'leri implement eden service'ler için wrapper kod üretir (compile-time).

### 6. Cross-Module Communication

**Niyet:** Module'ler arası iletişim kontrollü ve tek yönlü olmalıdır.

**`Application.Contracts` Katmanı:**

- Cross-module communication için merkezi interface tanımları
- **Sadece Query (okuma) operasyonları** tanımlanır
- Command (yazma) operasyonları tanımlanmaz

**Örnek:**

```csharp
// ✅ İzin verilen (Query)
public interface IExamQueryService
{
    Task<ExamDto> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}

// ❌ İzin verilmeyen (Command)
public interface IExamCommandService
{
    Task CreateAsync(...);  // Başka module'den çağrılamaz
}
```

**Cross-module yazma için:** Domain Events kullanılır (in-memory event bus).

### 7. Configuration Management

**Niyet:** Runtime configuration değişiklikleri desteklenir (hot-reload).

**Kısıt:** `IConfiguration` doğrudan inject edilmez. Bunun yerine Options Pattern kullanılır:

- `IOptions<T>` → Static configuration
- `IOptionsSnapshot<T>` → Per-request configuration
- `IOptionsMonitor<T>` → Hot-reload configuration (tercih edilen)

**Kullanım Yerleri:**

- Infrastructure plugins → `IOptionsMonitor<T>` (hot-reload)
- Module services → `IOptionsSnapshot<T>` (per-request)
- Domain → Options kullanılmaz (configuration concern değil)

## 8. Persistence Organization

**Niyet:** Her module kendi database context'ine sahip olur, ancak cross-module transaction ihtiyacında merkezi bir context kullanılabilir.

### 8.1. Temel Yapı

- `Persistence.Exam/` → `ExamDbContext` (schema: exam)
- `Persistence.Analysis/` → `AnalysisDbContext` (schema: analysis)
- `Persistence.Question/` → `QuestionDbContext` (schema: question)
- `Persistence.Aggregator/` → Hem design-time migration hem de runtime cross-module transaction için

Tek database kullanımı sayesinde ACID transaction garantisi korunur. Saga veya Outbox pattern'lerine ihtiyaç yoktur.

### 8.2. Database Concurrency & Parallelism Management (Güncelleme)

Bu bölüm, EF Core'un thread-safe olmamasından kaynaklanan **"A second operation was started on this context"** hatalarını mimari seviyede önlemek amacıyla eklenmiştir.

Paralel okuma gereksinimi olan senaryolarda (örneğin aynı request içinde dersler ve dönemlerin eş zamanlı yüklenmesi), standart scoped `DbContext` yerine **`IDbContextFactory<TContext>`** kullanımı zorunludur.

Factory yaklaşımı, her paralel task için izole ve kısa ömürlü bir context örneği üreterek çakışmaları engeller.

**Unit of Work Bütünlüğü:**

- Yazma (Command) işlemleri scoped `DbContext` üzerinden yürütülmeye devam eder.
- Factory yalnızca **ReadOnly** ve **Parallel** sorgular için kullanılır.

**C# 14 & .NET 10 Optimizasyonu:**

- Factory ile üretilen context'ler C# 14 `using` scope iyileştirmeleriyle yönetilir.
- .NET 10 DbContext Pooling aktif olmak zorundadır.

**Zorunlu Kurallar:**

- Paralel sorgularda kullanılan tüm context'ler `AsNoTracking()` ile işaretlenmelidir.
- Servis katmanında bu ayrımı netleştirmek için `IReadOnlyExamService` benzeri interface'ler üzerinden factory kullanımı teşvik edilir.

### 9. Dependency Management

**Niyet:** Tüm NuGet paket versiyonları merkezi olarak yönetilir.

**Mekanizma:** `Directory.Packages.props` (solution root)

- Merkezi version tanımları
- Tüm projeler bu version'ları kullanır
- Version conflict önlenir

### 10. Localization (i18n)

**Niyet:** Çok dilli destek, vertical slice izolasyonu korunarak sağlanır.

**Yapı:**

- Her module kendi `Resources/Locales/` klasörüne sahip
- YAML formatında resource dosyaları (okunabilir, version control friendly)
- Type-safe accessor'lar (Source Generator ile üretilir)

**Örnek:**

```yaml
# exam.tr.yaml
exam:
  validation:
    title_required: "Sınav başlığı zorunludur"
```

```csharp
// Type-safe kullanım
throw new ValidationException(_examLocalizer.TitleRequired);
```

---

## Bilinçli Olarak Yapılmayanlar (Monolith-Hybrid Kararları)

### Mikroservis Pattern'leri

**Yapılmayan:** Outbox Pattern, Saga Pattern, Distributed Tracing (OpenTelemetry), Service Mesh, API Gateway

**Gerekçe:**

- Proje monolith-hybrid mimarisinde
- Tek database, tek deployment
- ACID transaction yeterli
- Operational complexity artışı haklı çıkmaz
- İleride mikroservise geçişte kolayca eklenebilir (mimari hazır)

### Heavy Background Job Infrastructure

**Yapılmayan:** Hangfire, Quartz gibi dağıtık job scheduler'lar

**Gerekçe:**

- Monolith için `IHostedService` yeterli
- Basit periyodik task'ler (örn: embedding generation, cleanup)
- Dağıtık koordinasyon ihtiyacı yok

### Circuit Breaker (Internal Calls)

**Yapılmayan:** Internal service-to-service çağrıları için circuit breaker

**Gerekçe:**

- Internal çağrılar in-process, hızlı
- Circuit breaker sadece dış API'ler için (OpenAI, Cloudinary vb.)
- Basit retry + timeout yeterli

### Translation Management System

**Yapılmayan:** Ayrı translation service, crowdsourced translation platform

**Gerekçe:**

- Basit YAML dosyaları yeterli
- Version control üzerinden yönetilebilir
- Monolith için overkill

### MediatR

**Yapılmayan:** MediatR kullanımına devam edilmeyecek

**Gerekçe:**

- Runtime reflection overhead
- Magic strings, runtime hatalar
- Vertical Slice architecture ile uyumlu değil
- Minimal API + direct service calls daha basit ve performanslı

---

## Katman Yapısı ve Sorumluluklar

### Domain Layer

**Sorumluluk:** Pure domain logic, business rules, entity tanımları

**Özellikler:**

- Hiçbir framework'e bağımlı değil
- Hiçbir katmana referans vermez
- Value objects (EF Core 10 Complex Types ile)

**Örnek:**

```
Domain/
├── Exam/Exam.cs
├── Analysis/Analysis.cs
├── Question/Question.cs (+ Vector Embedding için float[] property)
├── ValueObjects/Address.cs, Money.cs
└── Common/Entity.cs, ValueObject.cs
```

### Application.Services (Pure Interfaces)

**Sorumluluk:** Tüm servis interface tanımları, attribute'ler, behavior interface'leri

**Özellikler:**

- Sadece interface ve attribute tanımları
- Implementation yok
- Hiçbir katmana referans yok

**İçerik:**

```
Application.Services/
├── Abstractions/
│   ├── Persistence/IRepository.cs, IUnitOfWork.cs
│   ├── Caching/ICacheService.cs
│   ├── Logging/ILogger.cs
│   ├── Events/IDomainEvent.cs, IEventBus.cs
│   ├── AI/IAIService.cs
│   └── Health/IHealthCheck.cs
├── Behaviors/
│   ├── ICached.cs
│   ├── ILogged.cs
│   ├── IValidated.cs
│   └── IResilient.cs
└── Attributes/
    ├── ServiceImplementationAttribute.cs
    ├── GenerateEventAttribute.cs
    ├── AutoInjectAttribute.cs
    └── InvalidatesCacheAttribute.cs
```

### Application.Contracts (Cross-Module)

**Sorumluluk:** Module'ler arası Query interface'leri ve DTO'lar

**Özellikler:**

- Sadece Query (okuma) operasyonları
- Command (yazma) operasyonları yok
- Referans: Application.Services

**İçerik:**

```
Application.Contracts/
├── Exam/IExamQueryService.cs, ExamDto.cs
├── Analysis/IAnalysisQueryService.cs, AnalysisDto.cs
├── Question/IQuestionQueryService.cs, QuestionDto.cs
└── Shared/PagedResult.cs, ResponseDto.cs
```

### Application.Shared (Ortak Implementasyonlar)

**Sorumluluk:** Module'ler arasında paylaşılan base class'lar, utility'ler, ortak localization

**Özellikler:**

- Validator base class'ları
- Ortak exception'lar
- Ortak localization (common, validation)
- Email/notification template'leri

**İçerik:**

```
Application.Shared/
├── Validators/EntityValidatorBase.cs
├── Exceptions/NotFoundException.cs, LocalizedException.cs
├── Resources/
│   ├── Locales/common.tr.yaml, validation.tr.yaml
│   └── Templates/email-exam-created.tr.html
└── Extensions/DomainExtensions.cs (C# 14 Extension Members)
```

### Application.AI (AI-First Layer)

**Sorumluluk:** AI-powered özellikler (exam generation, question recommendation, vector embedding)

**Özellikler:**

- Microsoft.Extensions.AI kullanımı (.NET 10)
- Agent pattern
- Prompt template yönetimi

**İçerik:**

```
Application.AI/
├── Agents/
│   ├── ExamGenerationAgent.cs
│   └── QuestionRecommendationAgent.cs
├── Prompts/exam-generation.prompt
└── Embeddings/VectorEmbeddingService.cs
```

### Application Modules (Vertical Slices)

**Sorumluluk:** Domain-specific business logic, commands, queries, validation

**Özellikler:**

- İzole modül
- Kendi localization dosyaları
- Sadece Application.Services + Application.Contracts + Application.Shared'e referans

**Örnek (Application.Exam):**

```
Application.Exam/
├── Services/ExamService.cs (implements IExamQueryService, ICached, ILogged)
├── Commands/CreateExamCommand.cs
├── Queries/GetExamByIdQuery.cs
├── DTOs/CreateExamRequest.cs
├── Validators/CreateExamValidator.cs
├── EventHandlers/ExamCreatedEventHandler.cs
├── Extensions/ExamExtensions.cs (C# 14 Extension Members)
└── Resources/
    ├── Locales/exam.tr.yaml, exam.en.yaml
    └── ExamLocalizer.cs (type-safe accessor)
```

### Application.Endpoints (Minimal APIs)

**Sorumluluk:** HTTP endpoint tanımları

**Özellikler:**

- ASP.NET Core 10 Minimal APIs
- Built-in validation (.AddValidation())
- Server-Sent Events (SSE) desteği
- OpenAPI 3.1 + YAML

**İçerik:**

```
Application.Endpoints/
├── ExamEndpoints.cs
├── AnalysisEndpoints.cs
└── AIEndpoints.cs
```

### Infrastructure Plugins

**Sorumluluk:** Dış servislere adapters (Logger, Cache, AI, Storage)

**Özellikler:**

- `[ServiceImplementation("key")]` attribute
- `IHealthCheck` implementation
- Dış API'ler için `IResilient` implementation (retry + timeout)
- `IOptionsMonitor<T>` ile hot-reload configuration

**Örnek (Infrastructure.AI.OpenAI):**

```csharp
[ServiceImplementation("openai")]
public class OpenAIService : IAIService, IResilient, IHealthCheck
{
    public int MaxRetryAttempts => 3;
    public int TimeoutSeconds => 30;
    
    // Polly ResiliencePipeline (retry + timeout)
    // IHealthCheck: API connectivity test
}
```

### Persistence Modules

**Sorumluluk:** Database access, entity configuration, migrations

**Özellikler:**

- Her module kendi DbContext'i
- EF Core 10: Vector types, Complex types, Named query filters
- Event tabloları (Source Generator ile üretilir)

**Yapı:**

```
Persistence.Exam/
├── ExamRepository.cs (implements IRepository<Exam, Guid>)
├── ExamDbContext.cs (schema: exam, named filters)
├── Configuration/ExamConfiguration.cs (Complex types)
└── Migrations/Exam_Initial.cs

Persistence.Aggregator/
├── AggregatedDbContext.cs (tüm module'leri toplar)
└── Migrations/ (tek migration point, cross-module transaction)
```

**Aggregator Kullanımı:**

- Design-time: Migration üretimi (`dotnet ef migrations add`)
- Runtime: Cross-module transaction (monolith avantajı, Saga gerekmez)

### Source Generators

**Sorumluluk:** Compile-time kod üretimi (boilerplate elimination)

**Generator'lar:**

1. **Event Entity Generator:** `[GenerateEvent]` → Event entity'leri üretir (Pure Domain ihlali. elle oluşturulurlacak.)
2. **Service Registration Generator:** `[ServiceImplementation]` → Compile-time service registry
3. **CCC Wrapper Generator:** `ICached`, `ILogged` → Wrapper kod üretir
4. **DI Generator:** `[AutoInject]` + C# 14 partial constructor → Constructor injection
5. **Cache Invalidation Generator:** `[InvalidatesCache]` → Invalidation kodu üretir
6. **Localizer Generator:** `[Localizable]` → Type-safe localization accessor

**Önemli:** Generator'lar compile-time çalışır, runtime overhead yoktur.

### Host (Composition Root)

**Sorumluluk:** Dependency injection setup, configuration, middleware pipeline

**Özellikler:**

- Sadece Application.Services'e referans verir
- Plugin'leri compile-time registry'den bulur (assembly scanning yerine)
- Keyed DI + IOptionsMonitor ile provider seçimi
- Health check endpoint (/health)
- Environment variables ile secret management

**Kritik Nokta:** Host, module ve infrastructure implementation'larını görmez. Compile-time registry aracılığıyla bağımlılıkları çözer.

---

## Teknoloji Kararları

### .NET 10 + C# 14 (Roadmap)

**Statü:** Gelecek vizyonu / roadmap notu

**Hedef Özellikler:**

- **C# 14:**
  - Field keyword (lazy initialization)
  - Extension members (domain logic extension)
  - Partial constructors (DI generation)
  - Null-conditional assignment
  - Single-file scripts
- **.NET 10:**
  - Built-in AI (Microsoft.Extensions.AI)
  - Vector search (EF Core 10)
  - NativeAOT improvements
  - Post-quantum cryptography (ML-DSA, ML-KEM)
- **ASP.NET Core 10:**
  - Built-in validation
  - OpenAPI 3.1 + YAML
  - Server-Sent Events
- **EF Core 10:**
  - Vector types
  - Complex types (value objects)
  - Named query filters
  - Native JSON type

**Not:** Bu özellikler .NET 10 release'i ile birlikte değerlendirilecek. Mevcut mimari bu özellikleri destekleyecek şekilde tasarlanmıştır ancak zorunlu değildir.

### Resilience (Polly)

**Karar:** Sadece dış API çağrıları için basit resilience

**Uygulama:**

- Retry (3 attempt, exponential backoff)
- Timeout (30 saniye)
- Internal çağrılar için uygulanmaz (in-process, hızlı)

**Yapılmayan:** Circuit breaker, bulkhead (monolith için overkill)

### Secret Management

**Karar:** Environment variables (basit, yeterli)

**Uygulama:**

- Development: .NET User Secrets
- Production: Environment variables (Docker, Kubernetes secrets)
- Opsiyonel: Azure Key Vault, AWS Secrets Manager (enterprise gereksinimi varsa)

**appsettings.json:** API key'ler boş bırakılır (security)

### Health Check

**Karar:** Basit interface-based health check

**Uygulama:**

- `IHealthCheck` interface
- Her plugin implement eder (Redis ping, OpenAI connectivity)
- `/health` endpoint (503 on failure)

**Yapılmayan:** Advanced health check orchestration (liveness/readiness probe yeterli)

### Background Jobs

**Karar:** `IHostedService` (basit, yeterli)

**Kullanım:**

- Periyodik task'ler (örn: vector embedding generation, event cleanup)
- 5-10 dakikalık interval'ler

**Yapılmayan:** Hangfire, Quartz (dağıtık job scheduling gereksiz)

### Localization

**Karar:** YAML resource files + type-safe accessor

**Uygulama:**

- Vertical slice isolation (her module kendi locale'leri)
- Source Generator ile type-safe accessor
- Runtime dil değişimi (query string, cookie, header)

**Yapılmayan:** Translation Management System (overkill)

---

## Bilinen Riskler ve Mitigasyon

### Risk 1: Source Generator Complexity

**Risk:** Generator'lar karmaşık, debug zor olabilir

**Mitigasyon:**

- Incremental generator kullanımı
- Kapsamlı unit test coverage
- Clear error messages
- Generated kod output'unu review sürecine dahil etme

### Risk 2: Compile-time Service Registry Performance

**Risk:** Çok fazla plugin eklenirse compile-time artabilir

**Mitigasyon:**

- Incremental generation
- Lazy evaluation
- Monitör et, gerekirse optimize et

### Risk 3: Cross-Module Transaction Complexity

**Risk:** AggregatedDbContext runtime kullanımı karışıklık yaratabilir

**Mitigasyon:**

- Kullanım senaryolarını dokümante et
- Code review'da dikkat et
- Transaction scope'u açıkça belirt

### Risk 4: Hot-reload Configuration Issues

**Risk:** IOptionsMonitor.OnChange event'i doğru handle edilmezse memory leak

**Mitigasyon:**

- Dispose pattern'i doğru uygula
- Event subscription/unsubscription lifecycle'ı kontrol et

### Risk 5: Localization Maintenance

**Risk:** YAML dosyaları büyüdükçe yönetim zorlaşabilir

**Mitigasyon:**

- Hierarchical key structure (exam:validation:title_required)
- Namespace isolation (her module kendi)
- Translation completeness testi (CI/CD)

---

## Migration Stratejisi (Özet)

### Phase 1: Altyapı (3 hafta)

- Event tracking infrastructure
- Application katmanı reorganizasyonu
- Vertical slice ilk örnek (Application.Exam)
- Localization altyapısı

### Phase 2: Source Generators (3-4 hafta)

- Event entity generator
- CCC wrapper generator
- DI generator
- Service registry generator
- Cache invalidation generator
- Localizer generator

### Phase 3: Infrastructure & Persistence (2-3 hafta)

- Infrastructure plugin'leri
- Persistence vertical slices
- Aggregator setup
- Health check implementation

### Phase 4: AI & Advanced Features (2-3 hafta)

- AI layer
- Vector search
- EF Core 10 advanced features

### Phase 5: Endpoints & Integration (2-3 hafta)

- Minimal API endpoints
- Host setup (compile-time registry)
- Full integration test
- Performance profiling

### Phase 6: Testing & Optimization (1-2 hafta)

- Unit, integration, e2e tests
- Performance regression tests
- Documentation

**Toplam:** 15-17 hafta

---

## Doküman Revizyonları

- **v1.0** - 2024-01-09: İlk versiyon
- **v1.1** - 2024-01-09: Mimari netleştirme, yapılmayanlar bölümü eklendi

---

**Not:** Bu doküman, SES projesinin mimari evriminin bir snapshot'udur. Implementation sırasında karşılaşılan gerçek dünya kısıtlarına göre güncellenecektir. Her güncelleme, revizyon geçmişine eklenmelidir.
