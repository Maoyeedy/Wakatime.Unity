# Wakatime/Wakapi for Unity

<img width="853" alt="25 04 23_08 00 42" src="https://github.com/user-attachments/assets/231661e9-2419-4c18-a9b7-2fa96606a048" />

Forked from [vanBassum/Wakatime.Unity](https://github.com/vanBassum/Wakatime.Unity).

### Features Added:
1. Support for [Wakapi](https://github.com/muety/wakapi), in addition to [WakaTime](https://wakatime.com/).
2. Detecting config from `~/.wakatime.cfg`
3. Detecting [wakatime-cli](https://github.com/wakatime/wakatime-cli) installed in System PATH
5. Toolbar status icon, showing how much time you've spent on the project.

### Fixes/Tweaks:
1. Set language type to `UnityEditor`, so it won't show up as `unknown` in Wakatime/Wakapi
2. Directly uses `System.DateTimeOffset` to prevent precision loss in float.
3. More buttons to Read/Reset in Settings Panel.

### TODO
- [ ] Add `Detect System wakatime-cli` button, and remove bundled wakatime-cli.
- [ ] Different mode for displaying time status: `All Time`, `Today`, `Last 7 days`.
