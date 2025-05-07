# Student Exam System & Analyzer (S.E.S.)

> **Short Name:** S.E.S.
> **Status:** Currently under active development—new features coming soon!

## 🔧 Infrastructure Reference

* **Semantic Commit Messages:** Follow [Semantic Commit Messages](https://github.com/FurkanHuman/Student_Exam_Generator_And_Analyzer/blob/Main/Docs/Semantic%20Commit%20Messages.md) for commit history consistency.

## Support the Project

If you find this project helpful or want to support its development, consider buying the author a coffee! Your support ensures the continued development and improvement of the project.

[Support the Project on Buy Me a Coffee](https://buymeacoffee.com/furkanhuman)

---

## 📋 Overview

Student Exam System & Analyzer (S.E.S.) is an innovative open-source platform designed to revolutionize the way educators manage exams. By leveraging modern technologies, S.E.S. simplifies the creation, administration, and analysis of exams, saving valuable time for teachers and providing actionable insights into student performance. The platform ensures data privacy and security by offering deployment options tailored to institutional needs, including on-premises and private cloud setups.

With S.E.S., educators can generate diverse question types, automate grading, and access detailed analytics to identify learning gaps and improve teaching strategies. The system also promotes academic integrity by creating unique exam versions for each student, reducing the likelihood of cheating.

---

## 🔍 Key Features

* **Automated Question Generation:** Input syllabus topics or reference materials; S.E.S. generates multiple-choice, true/false, and short-answer questions.
* **Automated Grading & Analytics:** Auto-grade submissions and visualize results (score distribution, averages, topic performance).
* **Exam Management:** Central question bank, exam scheduling, and unique exam permutations per student to discourage academic dishonesty.
* **Customizable Deployment:** Supports on-premises servers, private clouds, or Dockerized setups.

## 🛠️ Tech Stack

* **Backend:** C# / .NET 8 (ASP.NET Core MVC)
* **ORM:** Entity Framework Core
* **Database:** PostgreSQL
* **Containers:** Docker (optional)

## 🚀 Quick Start

1. **Clone Repository**

   ```bash
   git clone https://github.com/FurkanHuman/Student_Exam_Generator_And_Analyzer.git
   cd Student_Exam_Generator_And_Analyzer
   ```

2. **Configure**
   Edit `appsettings.json` to set your database connection.

3. **Build & Run**

   ```bash
   dotnet build
   dotnet run --urls "http://localhost:5000"
   ```

4. **Access**
   Open `http://localhost:5000` in your web browser. Use default teacher credentials as documented in the Docs folder.

---

## 🎯 Benefits for Educators

* **Time Efficiency:** Automates exam creation and grading workflows.
* **Insightful Data:** Comprehensive analytics identify learning gaps.
* **Enhanced Integrity:** Unique exam versions per student reduce cheating.
* **Organized Workflow:** Centralized question bank and exam scheduling.

---

## 📚 References

* Infrastructure reference: [Kodlama.io nArchitecture](https://github.com/kodlamaio-projects/nArchitecture.git)
