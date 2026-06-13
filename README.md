# Personal Blog Web Application
### By using is you can publish articles as an *admin*, and view as a *guest*.
**Arctile** is a complex of title, date and text. 
## What about realisation?
This a *backend*-based application with light *frontend* (frontend provides only fetches with backend).
Backend - **ASP.NET** Core with **Postgres** or **MongoDB** (you can choose one updating a "*CurrentDatabase*" in *appsettings.json* with "*PostgreSQL*" or "*MongoDB*")
Frontend - HTML + JS + SCSS application

Idea: https://roadmap.sh/projects/personal-blog

## CI
The project uses GitHub Actions for basic CI verification.

Current pipeline checks:
- `dotnet restore` for the solution
- `dotnet build --no-restore` for the solution

Why this step matters:
- it confirms the repository is buildable in a clean environment;
- it creates a stable base before adding SAST, DAST and dependency scanning;
- it makes security pipeline failures easier to interpret, because build problems are separated from scanner problems.
