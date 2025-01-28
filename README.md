# VolCon

Audio Volume Control Tool (C#/.NET)

VolCon is a command-line application designed to control the audio volume of a selected audio device.

## Prerequisites

- **.NET Version**: This application requires .NET 7 to run.

## Installation

To build and publish VolCon, follow these steps:
```bash
> dotnet restore
> dotnet build
> dotnet publish -c Release -r win-x64 --self-contained /p:PublishSingleFile=true
```
This will produce `VolCon.exe` with an approximate size of 67 MB.

If you want to reduce the file size, replace the `--self-contained` flag with `--no-self-contained` to exclude the .NET runtime from the executable.

## Author
[Hadi Cahyadi](mailto:cumulus13@gmail.com)

[![Buy Me a Coffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/cumulus13)

[![Donate via Ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/cumulus13)

[Support me on Patreon](https://www.patreon.com/cumulus13)

