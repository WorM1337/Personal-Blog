# Personal Blog Web Application

ASP.NET Core blog application with a lightweight HTML/JS/SCSS frontend.
The backend can work with PostgreSQL or MongoDB depending on `CurrentDatabase`.

Idea: https://roadmap.sh/projects/personal-blog

## Stack

- Backend: ASP.NET Core / .NET 9
- Databases: PostgreSQL or MongoDB
- Frontend: HTML + JavaScript + SCSS

## Local Run

1. Start a database from `Personal Blog/Docker/docker-compose.yml` or provide your own PostgreSQL / MongoDB instance.
2. Provide secrets via environment variables or .NET user secrets:
   - `Jwt__Key`
   - `AdminCredentials__Login`
   - `AdminCredentials__Password`
3. Choose a storage backend with `CurrentDatabase=PostgreSQL` or `CurrentDatabase=MongoDB`.
4. Run the app:

```bash
dotnet run --project "Personal Blog/Personal Blog.csproj"
```

The demo pages are available under:

- `http://localhost:5000/html/welcome.html`
- `http://localhost:5000/html/articles.html`
- `http://localhost:5000/html/login.html`

## CI

The repository contains a base GitHub Actions workflow in `.github/workflows/ci.yml`.
It reuses the composite action from `.github/actions/build-n-restore/action.yml` and runs:

- `dotnet restore "Personal Blog.sln"`
- `dotnet build "Personal Blog.sln" --no-restore`

This workflow is the foundation for the security pipelines below.

## Security Checks

This repository is prepared for the lab on automated application security testing.
At the current stage the security setup includes the base CI pipeline and SAST only.

Important: several findings are intentionally left in the codebase for demonstration.
They should be fixed before any production use.

### SAST

Tool: **Semgrep**

Files:

- Workflow: `.github/workflows/sast.yml`
- Registry rulesets: `p/default`, `p/csharp`

What the workflow does:

- restores and builds the solution;
- runs Semgrep with public community rules from the Semgrep Registry;
- uploads a SARIF report as a workflow artifact;
- publishes SARIF to GitHub code scanning.

How to run locally:

```bash
python -m pip install semgrep
mkdir -p reports/sast
semgrep scan --config p/default --config p/csharp --sarif --output reports/sast/semgrep.sarif --metrics=off .
```

Expected demo findings:

- findings from the public Semgrep C# and default rulesets for the current codebase
- the exact result set depends on the registry rules version available at scan time
- if some of the intentionally insecure places are not detected by the public rules alone, they can still be discussed during the defense as manual review examples

## What To Show At Defense

Suggested demo flow for the current step:

1. Run `Base CI` and show that the project builds in a clean environment.
2. Open `SAST - Semgrep` and show the findings in the artifact or GitHub code scanning UI.
3. Point to the exact insecure places in `AuthService.cs`, `login.js` and `articles-admin.js`.
4. Explain which findings are intentionally left for the lab and how they should be remediated later.

## Reports In GitHub Actions

Artifacts currently published:

- `sast-semgrep-report`

## Recommended Remediation After The Defense

- stop logging JWTs, passwords and similar secrets;
- move development credentials fully to environment variables, GitHub Secrets or .NET user secrets;
- replace `innerHTML` with safe DOM rendering using `textContent` and created nodes.
