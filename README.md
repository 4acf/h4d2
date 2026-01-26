H4D2 is a Left 4 Dead 2 demake inspired by Notch's [Herp Fortress](https://www.youtube.com/watch?v=4J_8HkQj9mU).
You play as the infected and must eliminate all the survivors.

Game logic and rendering is handled with a minimal custom engine, but the game does make use of SFML + SkiaSharp libraries for things like windowing, audio, mouse and keyboard input, and reading image files.

It is the first game I have ever made and it originally started off as just a screensaver but evolved into something you could actually play.

If nothing else it's a good way to kill 20 minutes.

# Installation/Uninstallation
Download the latest release for your operating system. Supported operating systems are: 
- Windows (64-bit) 
- Linux (64-bit)

H4D2 does not require installation. You can simply run the .exe if you're on Windows, or run the AppImage if on Linux.

H4D2 however stores your settings and personal level records outside of the downloaded files.
- On Windows, you can find this data in `AppData/Roaming/h4d2`
- On Linux, you can usually find this data in `/home/{your username}/.config/h4d2`

To fully remove the game from your machine, delete both the downloaded files and the settings/level records data.

# Controls and Gameplay Details
- **Move Camera:** `W`, `A`, `S`, `D` or `Arrow Keys`
- **Pause / Unpause:** `Esc`
- **Select / Unselect Special Infected:** `1–8 (depending on level)`
- **Spawn Special Infected:** `Mouse 1`

As of v0.1 there are 15 levels. Kill all survivors to complete a level.

# Changelog
### v0.1
- Initial release!
- 15 playable levels with varying layouts, survivors, and spawnable specials.
- Playable on: Windows (64-bit) and Linux (64-bit)

Devlogs will be included in future releases.

### DEVELOPER SECTION

# File Structure Overview
```
h4d2/
├── h4d2/
│   ├── Entities/
│   ├── GUI/
│   ├── Infrastructure/ (reusable utilities/tools that are unspecific to the game)
│   │   ├── H4D2/       (utilities/tools that are specific to H4D2)
│   ├── Levels/
│   ├── Particles/
│   ├── Resources/
│   ├── Spawners/
│   └── Weapons/
│   ├── AudioManager.cs
│   ├── Game.cs
│   ├── H4D2.cs         (entry point)
│   ├── Input.cs
│   ├── h4d2.csproj
└── h4d2.sln
```

Code that uses SFML libraries should only exist in the top level (h4d2/h4d2), except for Game.cs.


# Project Setup
- Ensure you have the .NET SDK 9.0 installed.
- Clone the repository and restore dependencies
```bash
git clone https://github.com/4acf/h4d2.git
cd h4d2
dotnet restore
```
- Run with `dotnet run --project h4d2/h4d2.csproj`

Or just open the solution in your IDE of choice and build/run from there.

# Level Layout Requirements
Since there's no built-in level editor, making levels is done with the following ruleset. 

Levels are saved as png images with 4 recognized colors.
- Black (0x000000) denotes a wall.
- <span style="color: #FF0000;">Red (0xff0000)</span> denotes a health spawn location.
- <span style="color: #00FF00;">Green (0x00ff00)</span> denotes a zombie spawn location.
- <span style="color: #0000FF;">Blue (0x0000ff)</span> denotes the survivor spawn location.

Anything else will be ignored. For consistency, every other pixel should be white (0xffffff).

Additional rules:
- The outermost pixels of the image should all either be a wall or a zombie-spawn location.
- Zombie spawn locations can only have one non-wall tile adjacent to it (in the 4 cardinal directions). 
- There should only be one survivor spawn location.
- Passages that are less than 3 tiles wide are not game-breaking but they are not recommended.

1 pixel of the image translates to 1 tile.

# Known Issues/Improvements
### Player-facing
- In rare situations, mobs will get stuck on corners
- In rare situations, survivors will miss zombies that are right next to them

### Developer-facing
- Object pooling is planned to optimize resource allocation of small, Gen0 objects
- Configuration files will be refactored to a format that allows for changes during runtime

# Contributing
If you'd like to make a contribution:
- Fork the repo and branch from `dev`
- Make your changes
- Commit and push your branch
- Open a Pull Request **into `dev`**


If you are opening an issue regarding a bug, please include steps to reproduce the bug.