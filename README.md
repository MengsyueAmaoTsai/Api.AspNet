# RichillCapital.Api

## Getting Started

### Run Development

```powershell
dotnet watch run --project ./src/RichillCapital.Api -c Debug
```

### Build and Test

```powershell
dotnet build ./RichillCapital.Api.sln -c Release --nologo
dotnet test ./RichillCapital.Api.sln -c Release --nologo
dotnet publish ./src/RichillCapital.Api/RichillCapital.Api.csproj -c Release -o ./artifacts --no-restore --no-build --nologo
```
