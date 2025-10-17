# Student Exam System & Analyzer (S.E.S.)

> **Short Name:** S.E.S.
> **Status:** 🚧 Actively being developed — New features on the way!

---

## 📌 What is S.E.S.?

S.E.S. (Student Exam System & Analyzer) is a powerful, open-source platform built to streamline the exam process for educators. It makes exam preparation, grading, and result analysis faster and easier — with built-in tools that help reduce cheating and improve learning outcomes.

Whether you’re an individual teacher, a school, or an institution, S.E.S. can adapt to your needs — and can even be deployed on-premises or in private clouds.

---

## 🌟 Key Features

* **Auto Question Generation** — Enter topics, get multiple-choice, true/false, and short-answer questions.
* **Smart Grading & Analytics** — Automatically grade and visualize student performance.
* **Unique Exams Per Student** — Reduces cheating by generating different versions of each test.
* **Exam Management Dashboard** — Centralized control for question banks and scheduling.
* **Flexible Deployment** — Works on local servers, private clouds, or via Docker.

---

## 🧰 Tech Stack

* **Backend:** C# / .NET 8 (ASP.NET Core MVC)
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core
* **Containerization:** Docker (optional)

---

## 💻 Demo

A small live demo of S.E.S. is available on the following subdomains:

* [ses.furkanhuman.app](https://ses.furkanhuman.app)
* [oss.furkanhuman.app](https://oss.furkanhuman.app)

This demo allows you to explore the system with sample data. Please note that it is for demonstration purposes only.

---

## 🚀 How to Get Started

1. **Clone the Repository**

   ```bash
   git clone https://github.com/FurkanHuman/Student_Exam_Generator_And_Analyzer.git
   cd Student_Exam_Generator_And_Analyzer
   ```

2. **Configure the Database**

   Update `appsettings.json` with your database connection info.

3. **Build and Run the App**

   For Linux-based systems

   ```bash
      # Docker mode
      ./Build.sh docker SES_App_Name amd64 ./cert.pfx Cert_Password
      # Run
      ./Run.sh docker SES_App_Name amd64

      # Standalone mode  
      ./Build.sh standalone SES_App_Name Release      
      # Run
      ./Run.sh standalone SES_App_Name
   ```

   For Windows systems

   ```powershell
      # Docker mode
      .\Build.ps1 docker SES_App_Name amd64 .\cert.pfx Cert_Password      
      # Run
      .\Run.ps1 docker SES_App_Name amd64

      # Standalone mode
      .\Build.ps1 standalone SES_App_Name Release      
      # Run
      .\Run.ps1 standalone SES_App_Name
   ```

## 🏗 Architecture Overview

The S.E.S. solution follows a **multi-layered architecture**:

* **BlazorWebUI** – UI / main entry point  
* **BlazorWebUI.Client** – Client-side logic  
* **Application** – Application services and logic
* **Domain** – Database Entities  
* **Infrastructure** – Data access & external services  
* **Persistence** – Database / storage  

---

## 🚀 Running the Application

1. Open the solution in your IDE.  
2. Set **BlazorWebUI** as the **Startup Project**.  
3. Restore dependencies:

   ```bash
   dotnet restore
   ```

   Build the solution:

   ```bash
   dotnet build
   ```

   Run the application:

   ```bash
   dotnet run --project Src/SES/BlazorWebUI/BlazorWebUI.csproj
   ```

4. Open your browser and navigate to `https://localhost:5001` (or the port specified in your configuration).

<!-- ## 🧪 Running Tests

Set Tests project as startup in IDE, or run

   ```bash
   dotnet test
   ``` -->

---

## 🎓 Why Educators Love It

* Save time with automatic generation and grading
* Identify learning gaps with data-driven insights
* Reduce cheating with unique exams for each student
* Manage everything from one dashboard

---

## 🔒 Privacy & Security Notice

For legal reasons, certain links, adapters, or external service providers are **not included** in this repository.  
Users are responsible for configuring and managing any required integrations themselves.

---

## 🔄 Contribution Guidelines

* If you are interested in Contributing to this Project checkout the [Contributing Guidelines](https://github.com/FurkanHuman/Student_Exam_Generator_And_Analyzer/blob/Main/CONTRIBUTING.md)

---

# 📝 Feature Requests

If you’d like to see a new feature added, please open a feature request using our  
[Feature Request Template](https://github.com/FurkanHuman/Student_Exam_Generator_And_Analyzer/blob/Main/.github/ISSUE_TEMPLATE/feature_request.md).

We appreciate your feedback and contributions!

---

## 🌐 Community & Support

Join the S.E.S. community or stay updated through:

* [Discord Community](https://discord.gg/efRbrdrZft)  
* [Whatsapp Community](https://chat.whatsapp.com/ITLsdZrvWBoAcmNZJvtZMr?mode=wwt)
* [Telegram Community](https://t.me/+hSkOKOKD4thhYzA0)  

Feel free to ask questions, share feedback, or contribute to the project.

---

## 💖 Support the Project

Like what you see? Help keep the project growing by buying the author a coffee:

[👉 Buy Me a Coffee](https://buymeacoffee.com/furkanhuman)

---

## 📚 References

* Infrastructure inspired by: [Kodlama.io nArchitecture](https://github.com/kodlamaio-projects/nArchitecture.git)
