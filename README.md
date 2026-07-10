# Win32-Content-Prep-Tool-GUI

<p align="center">
  <strong>A simple Windows GUI for packaging Win32 apps into <code>.intunewin</code> files</strong>
</p>

<p align="center">
  A lightweight <strong>C# / WinForms</strong> front-end for <code>IntuneWinAppUtil.exe</code>, built to simplify Win32 app packaging for <strong>Microsoft Intune</strong>.
</p>

<p align="center">
  <img alt="License" src="https://img.shields.io/badge/license-MIT-blue.svg" />
  <img alt="Platform" src="https://img.shields.io/badge/platform-Windows-0078D6.svg" />
  <img alt="Language" src="https://img.shields.io/badge/language-C%23-239120.svg" />
  <img alt="UI" src="https://img.shields.io/badge/UI-WinForms-6A5ACD.svg" />
</p>

---

## Overview

**Win32-Content-Prep-Tool-GUI** is a Windows desktop application that provides a graphical interface for **Microsoft's `IntuneWinAppUtil.exe`**.

Instead of manually building command-line arguments, the application lets you:

- select an installer file (`.exe` or `.msi`)
- choose an output folder
- optionally include catalog files
- enable or disable verbose output
- launch packaging into an **`.intunewin`** file

The goal is to make Win32 content preparation easier and more accessible for **Microsoft Intune** administrators.

---

## Features

- Simple **Windows Forms** interface
- File picker for Win32 installers:
  - `.exe`
  - `.msi`
- Output folder selection
- Optional bundled catalog support
- Verbose mode with a dedicated output window
- One-click conversion to `.intunewin`
- Copy generated command-line arguments to clipboard
- Automatic handling of `IntuneWinAppUtil.exe`
  - downloads it if missing
  - can refresh it if the local copy is older than 1 day
  - verifies the downloaded binary with a SHA-256 hash before use

---

## Getting Started

### Option 1 — Download the EXE (Recommended)

If you just want to use the tool:

1. Go to the **Releases** section of this repository
2. Download the latest compiled `.exe`
3. Extract it if needed
4. Run `Win32-Content-Prep-Tool-GUI.exe`

> You do **not** need to manually download `IntuneWinAppUtil.exe` in advance.  
> If it is not already available locally, the application can download it automatically when needed.

---

### Option 2 — Build from Source

If you want to build the project yourself:

#### Requirements

- **Windows**
- **Visual Studio** or another .NET-compatible IDE
- A .NET SDK matching the project target framework

#### Project target

The project currently targets:

```xml
<TargetFramework>net10.0-windows</TargetFramework>
```

#### Steps

1. Clone the repository

   ```bash
   git clone https://github.com/Miiraak/Win32-Content-Prep-Tool-GUI.git
   ```

2. Open the project in **Visual Studio**

3. Build and run the application

---

## Usage

1. Launch the application
2. Select a Win32 installer (`.exe` or `.msi`)
3. Select an output folder
4. Optionally enable bundled catalog support and choose a catalog folder
5. Optionally enable **Verbose**
6. Click **Convert to .Intunewin**

After the conversion completes, the application opens the output folder automatically.

---

## How It Works

The application builds arguments for `IntuneWinAppUtil.exe` in this format:

```bash
-c "<sourceFolder>" -s "<setupFile>" -o "<outputFolderPath>"
```

Depending on the selected options, it may also append:

```bash
-a "<catalogFolderPath>"
-q
```

Where:

- `-a` includes bundled catalog files
- `-q` disables verbose output

The application runs the tool through PowerShell and, when verbose mode is enabled, streams standard output and error output into a dedicated window.

---

## Automatic Tool Download

When conversion starts, the application checks for `IntuneWinAppUtil.exe` in a temporary folder.

Behavior observed in the current code:

- if the tool is missing, the app asks whether it should download it
- if the tool already exists but is older than **1 day**, the app can offer to refresh it
- after download, the binary is validated using a **SHA-256 hash**
- if validation fails, the file is deleted and execution stops

This helps ensure the tool used for packaging is the expected version.

---

## Interface Overview

### Main Form

The main window includes:

- installer selection
- output folder selection
- catalog folder selection
- bundled catalog toggle
- verbose toggle
- conversion button
- command-line copy button

### Verbose Form

A separate verbose window is used to display process output when verbose mode is enabled.

---

## Project Structure

```text
.
├── README.md
├── LICENSE.txt
├── Win32-Content-Prep-Tool-GUI.slnx
└── Win32-Content-Prep-Tool-GUI/
    ├── Program.cs
    ├── main.cs
    ├── main.Designer.cs
    ├── main.resx
    ├── VerboseForm.cs
    ├── VerboseForm.resx
    ├── Win32-Content-Prep-Tool-GUI.csproj
    └── Properties/
        ├── Resources.Designer.cs
        └── Resources.resx
```

---

## Use Cases

This project is useful for:

- **Microsoft Intune administrators**
- **Endpoint management engineers**
- **Win32 application packagers**
- users who prefer a GUI over repetitive command-line usage

---

## Notes

- This is a **Windows-only** project
- The app depends on `IntuneWinAppUtil.exe` for the actual packaging step
- However, the executable is **handled automatically by the app when needed**, so it is not a manual prerequisite for first-time users

---

## Screenshots

[IMG]

---

## License

This project is licensed under the **MIT License**.  
See [`LICENSE.txt`](LICENSE.txt) for details.

---

## Related Project

This GUI relies on Microsoft’s Win32 content prep utility:

- `IntuneWinAppUtil.exe`
- Microsoft repository: `microsoft/Microsoft-Win32-Content-Prep-Tool`

---

## Author

Created by **[Miiraak](https://github.com/Miiraak)**.

If this project helps you, consider starring the repository.
