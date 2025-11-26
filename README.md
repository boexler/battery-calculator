# Battery Calculator

A Blazor WebAssembly (WASM) application hosted on GitHub Pages.

## Overview

This is a simple Hello World example built with Blazor WASM (.NET 8) and automatically deployed to GitHub Pages using GitHub Actions.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Local Development

### Run the Application

```bash
dotnet run
```

The application will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Build for Production

```bash
dotnet build --configuration Release
dotnet publish --configuration Release --output ./publish
```

The published files will be in the `./publish/wwwroot` directory.

## Project Structure

```
battery-calculator/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── MainLayout.razor.css
│   ├── Pages/
│   │   ├── Home.razor
│   │   └── Home.razor.css
│   ├── App.razor
│   └── _Imports.razor
├── wwwroot/
│   ├── css/
│   │   └── app.css
│   └── index.html
├── .github/
│   └── workflows/
│       └── deploy.yml
├── Program.cs
└── battery-calculator.csproj
```

## GitHub Pages Deployment

The application is automatically deployed to GitHub Pages when changes are pushed to the `main` branch.

### Setup GitHub Pages

1. Go to your repository settings on GitHub
2. Navigate to **Pages** in the left sidebar
3. Under **Source**, select **GitHub Actions**
4. The workflow will automatically build and deploy on each push to `main`

### Base Path Configuration

The application is configured to use `/battery-calculator/` as the base path for GitHub Pages. This is set in:
- `wwwroot/index.html` - The `<base href="/battery-calculator/" />` tag

### Manual Deployment

If you need to manually trigger a deployment:

1. Go to the **Actions** tab in your GitHub repository
2. Select the **Deploy to GitHub Pages** workflow
3. Click **Run workflow**

## Technologies

- **.NET 8** - Framework
- **Blazor WebAssembly** - UI Framework
- **GitHub Actions** - CI/CD
- **GitHub Pages** - Hosting

## License

[Add your license here]
