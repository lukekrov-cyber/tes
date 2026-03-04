# tes
This repository contains a simple .NET console application (`ServiceApp`) that installs itself as a Windows Service named `test`.

## Structure

- **ServiceApp/** - C# project implementing the service and installer logic.

## Usage

1. Open the `ServiceApp` folder in Visual Studio or use the CLI.
2. Build the project (`dotnet build`).
3. Run the resulting `ServiceApp.exe` on a Windows machine. You will be prompted for administrator permission (UAC).
4. When run, the executable will create a Windows Service named `test` configured to start automatically.
5. Verify the service via `services.msc` or `sc query test` and reboot to confirm auto-start.

> ⚠️ This code is for learning purposes and should not be distributed without proper security review.
