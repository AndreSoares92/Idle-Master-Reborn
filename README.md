# Idle Master Reborn

Get your [Steam Trading Cards](https://steamcommunity.com/tradingcards/) quickly by simulating in-game status for games with remaining card drops — or farm hours on any games in your Steam library.

> **Idle Master Reborn** is a modernized continuation of *Idle Master* / *Idle Master Extended*, licensed under the GNU General Public License v2.0.

## Features

- **Modern Steam Login (WebView2):** Built-in Edge WebView2 sign-in with manual cookie fallback for advanced users.
- **Multi-Account Switcher:** Save and switch between multiple Steam accounts (`File → Accounts`) with Windows DPAPI-encrypted cookie storage.
- **Hour Booster & Game Library (`File → Farm Hours`):** Browse your entire Steam library with sortable columns, select games to idle, and optionally set a **target hour goal** (automatically stops when all selected games reach the target).
- **Auto / Light / Dark Theme:** Follows your Windows app theme automatically or lets you choose Light/Dark manually, including dark title bars and menus.
- **25 Languages:** Includes **Auto (System)** language detection alongside 24 localized languages.
- **Idling Modes & Priority:** Fast mode (simultaneous card dropping), individual/2-hour modes, and drop-count priority (default, most drops, least drops).
- **Blacklist with Game Names:** Manage ignored App IDs together with their game names.
- **System Tray & Window Persistence:** Quick tray menu (`Open window`, `Pause/Resume`, `Exit`) and automatic saving of window positions.
- **Update Checker:** Checks [GitHub Releases](https://github.com/AndreSoares92/Idle-Master-Reborn/releases) for new versions (`Help → Check for Updates` and automatic startup check).

## Requirements

- Windows 10 / 11 (or Windows with .NET Framework 4.8)
- Steam client installed, running, and logged in
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
- [Microsoft Edge WebView2 Runtime](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) (pre-installed on Windows 10/11)

## Installation

1. Download the latest `IdleMasterReborn-vX.X.X.zip` from the [Releases](https://github.com/AndreSoares92/Idle-Master-Reborn/releases) page.
2. Extract all files (keeping the `Languages` folder next to `IdleMasterReborn.exe`).
3. Make sure Steam is open and logged in, then run `IdleMasterReborn.exe`.

## Building from Source

1. Ensure NuGet packages (`HtmlAgilityPack` and `Microsoft.Web.WebView2`) are present in `Source\packages\`.
2. Open `Source\IdleMasterReborn.sln` in Visual Studio or build via MSBuild:

```powershell
& "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "Source\IdleMasterReborn\IdleMasterReborn.csproj" /p:Configuration=Release /p:Platform=x86 /t:Rebuild
```

The compiled output will be placed in `Source\IdleMasterReborn\bin\x86\Release\`.

## License

This project is licensed under the **GNU General Public License v2.0** — see the [LICENSE](LICENSE) file for details.
