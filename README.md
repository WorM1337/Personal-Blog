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
At the current stage the security setup includes the base CI pipeline, SAST and DAST.

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

### DAST

Tool: **OWASP ZAP Baseline**

Files:

- Workflow: `.github/workflows/dast.yml`
- Rules file: `.zap/rules.tsv`

What the workflow does:

- restores and builds the solution;
- starts MongoDB in GitHub Actions;
- launches the ASP.NET application locally in CI;
- runs OWASP ZAP Baseline against the running app;
- saves the ZAP report as an artifact.

How to run locally:

1. Start MongoDB or another supported database instance.
2. Start the application with the required environment variables:

```bash
ASPNETCORE_URLS=http://127.0.0.1:5000 \
Urls='http://127.0.0.1:5000;' \
CurrentDatabase=MongoDB \
ConnectionStrings__MongoDB='mongodb://127.0.0.1:27017' \
Jwt__Key='demo-insecure-jwt-key-for-security-lab' \
Jwt__ExpireHours=2 \
AdminCredentials__Login=admin \
AdminCredentials__Password=admin \
dotnet run --no-launch-profile --project "Personal Blog/Personal Blog.csproj"
```

3. In another terminal run ZAP Baseline:

```bash
mkdir -p reports/dast
docker run --rm --network host -v "$(pwd):/zap/wrk/:rw" ghcr.io/zaproxy/zaproxy:stable zap-baseline.py -t http://127.0.0.1:5000/html/welcome.html -r reports/dast/report_html.html -J reports/dast/report_json.json -w reports/dast/report_md.md -c .zap/rules.tsv -a
```

Expected demo findings:

- missing `Content-Security-Policy`
- missing `X-Frame-Options`
- missing `X-Content-Type-Options`
- missing `Strict-Transport-Security`

The configured rules keep these alerts visible in the report so they can be shown during the defense.


## Reports In GitHub Actions

Artifacts currently published:

- `sast-semgrep-report`
- `dast-zap-report`
- `dast-app-log`
