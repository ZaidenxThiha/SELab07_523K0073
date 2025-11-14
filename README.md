# SELab07_523K0073

Full implementation of the SE Lab 7 DB‑First exercises described in `SELab7_2025_ASP.Net-Core_DB_First.pdf`. The repository contains:

## Solution map

- `SchoolDB.sql` – creates the SQL Server schema (Departments, Courses, Students, Instructors, Enrollments) and inserts the sample rows from the lab handout.
- `SchoolAppCore.sln`
  - `SchoolAppCore.Data` – shared EF Core 8 DbContext (`SchoolContext`), entity classes that mirror the `SchoolDB` schema, and a small seeding helper (`EnsureSeedDataAsync`).
  - `SchoolAppCoreRazor` – ASP.NET Core 8 Razor Pages app with full CRUD for every entity plus navigation that matches Exercise 1.
  - `SchoolAppCoreMVC` – ASP.NET Core 8 MVC app with controllers and views for every entity as required by Exercise 2.
- `Reports/EnrollmentsReport.rdlc` – starter RDLC file for Exercise 4 showing a tabular Enrollment report (Course, Student, Grade).

## Database setup (Exercise 1, Step 1)

1. Open SQL Server Management Studio (or Azure Data Studio) and run `SchoolDB.sql`.
2. Connection strings now point to the LocalDB pipe `Server=np:\\.\pipe\LOCALDB#20734081\tsql\query`. If your LocalDB uses a different pipe, update the `SchoolDBConnectionString` entry in:
   - `SchoolAppCoreRazor/appsettings.json`
   - `SchoolAppCoreMVC/appsettings.json`
3. First run of either web app calls `EnsureSeedDataAsync`, so the schema is created automatically if it does not yet exist and the starter rows are inserted.

## Running the web apps

Use separate terminals if you want both apps running at once.

```bash
# Razor Pages (Exercise 1)
dotnet run --project SchoolAppCoreRazor

# MVC (Exercise 2)
dotnet run --project SchoolAppCoreMVC
```

Both projects display guidance on their home pages and expose menu entries for Departments, Courses, Students, Instructors, and Enrollments. Course and Enrollment forms include drop‑downs for related data to match the original instructions. CRUD validation + antiforgery is configured through the generated scaffolding.

## Viewing the RDLC report

- Launch the Razor Pages app and use the navigation link **Enrollments Report (PDF)**.
- The app generates a PDF on the fly by feeding `Reports/EnrollmentsReport.rdlc` with live data through `AspNetCore.Reporting`.
- On macOS/Linux, install the native dependencies needed by `System.Drawing` (e.g., `brew install mono-libgdiplus`) before rendering the report; on Windows no extra setup is required.

## EF Core DB‑First notes

- The models and context are stored in `SchoolAppCore.Data` so Razor Pages and MVC can stay in sync with the same schema.
- If the schema changes, update `SchoolDB.sql`, adjust the entity classes (or run `dotnet ef dbcontext scaffold` against the live database), and both apps will pick up the change after rebuilding.
- `EnsureSeedDataAsync` is optional when you connect to an existing populated database; remove the call in `Program.cs` if you prefer migrations only.

## RDLC starter (Exercise 4)

`Reports/EnrollmentsReport.rdlc` is a lightweight definition that expects a dataset named `EnrollmentDataSet` with fields `CourseTitle`, `StudentFullName`, and `Grade`.

1. Add a WebForm/Page or Razor component that hosts a ReportViewer control.
2. Supply a `DataTable` (or `IEnumerable`) that projects the EF entities to the three expected columns.
3. Point the viewer to `Reports/EnrollmentsReport.rdlc` at runtime.

This layout fulfils the “make a form and report” portion without locking you into a specific UI technology.

## Mapping to GitHub (Exercise 3)

The repo is already initialized, but to push it elsewhere:

```bash
git remote add origin <your_repo_url>
git add .
git commit -m "Initial commit for SE Lab 7"
git push -u origin main
```

## Testing

```bash
dotnet build SchoolAppCore.sln
```

The solution currently builds cleanly with .NET SDK 9.0.200 targeting .NET 8.
