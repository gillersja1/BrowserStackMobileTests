# BrowserStack Mobile Test Example (C# / Playwright)

A small example project that runs Playwright/NUnit tests on **real mobile
devices** (not emulators) via [BrowserStack Automate](https://www.browserstack.com/automate),
using the official `BrowserStack.TestAdapter` NuGet package.

The test code itself is plain Playwright + NUnit — there's no
BrowserStack-specific API to learn. The SDK intercepts test execution and
routes each browser session to a real device in BrowserStack's cloud, based
on the platforms you list in `browserstack.yml`.

## Prerequisites

- .NET 8 SDK (BrowserStack's C# SDK supports .NET 6+ but **not** .NET 10+)
- A [BrowserStack](https://www.browserstack.com/users/sign_up) account (a free trial works) with your Username and Access Key
- **Note:** BrowserStack's C# SDK is not currently supported on Apple Silicon (M1/M2/M3) Macs.

## Project structure

```
BrowserStackMobileTests/
├── Tests/
│   └── MobileSauceDemoTests.cs   # Plain Playwright/NUnit tests
├── browserstack.yml              # Device/browser matrix + credentials
├── BrowserStackMobileTests.csproj
└── .github/workflows/browserstack-mobile.yml
```

## Setup

1. Restore and build:

   ```bash
   dotnet restore
   dotnet build
   ```

2. Set your credentials as environment variables. The SDK reads them
   directly, so they never need to be written into `browserstack.yml`:

   ```bash
   export BROWSERSTACK_USERNAME="your-username"          # macOS/Linux
   export BROWSERSTACK_ACCESS_KEY="your-access-key"

   $env:BROWSERSTACK_USERNAME = "your-username"          # Windows PowerShell
   $env:BROWSERSTACK_ACCESS_KEY = "your-access-key"
   ```

3. Adjust the `platforms` list in `browserstack.yml` to whichever real
   devices you want to test against. The included example runs on an
   iPhone 15 Pro Max (Safari); Android devices aren't yet supported by the
   Playwright .NET binding (see the note in `browserstack.yml`).
   Browse the full device catalog here:
   [supported browsers & OS for Playwright](https://www.browserstack.com/list-of-browsers-and-platforms/playwright).

## Running the tests

No special command — run it exactly like any other `dotnet test` suite. The
SDK does the routing:

```bash
dotnet test
```

Each test in `MobileSauceDemoTests.cs` will run once per device listed in
`browserstack.yml`. Results, videos, and logs are available on the
[BrowserStack Automate dashboard](https://automate.browserstack.com/).

## What's being tested

The suite targets [saucedemo.com](https://www.saucedemo.com/) — a public
demo store built for test automation practice — and checks:

- The login form renders correctly on a mobile viewport
- A standard user can log in successfully
- The hamburger menu (mobile nav) opens and shows the logout link

## CI/CD

`.github/workflows/browserstack-mobile.yml` runs the suite on every push/PR
to `main`. It keeps credentials out of the repo by:

1. Running a [gitleaks](https://github.com/gitleaks/gitleaks) scan over the
   full git history, failing the build if any credential has been committed
2. Passing GitHub secrets (`BROWSERSTACK_USERNAME`, `BROWSERSTACK_ACCESS_KEY`)
   to `dotnet test` as environment variables, which the SDK reads natively
3. Uploading the `.trx` results as a build artifact

To use this, add `BROWSERSTACK_USERNAME` and `BROWSERSTACK_ACCESS_KEY` as
repository secrets (Settings → Secrets and variables → Actions).

## Possible extensions

- Add BrowserStack Local testing to point at a staging environment instead of a public site
- Add tablet form factors (e.g. Samsung Galaxy Tab, iPad) to the platform matrix
- Combine with the Allure reporting setup from a desktop test suite for one unified report
- Add BrowserStack's `browserstack.geoLocation` capability to test region-specific behavior (Enterprise plan only)
