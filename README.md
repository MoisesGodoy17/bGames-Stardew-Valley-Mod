# Stardew Valley Mod bGames

[![es](https://img.shields.io/badge/lang-es-green.svg)](./README-ES.md)
[![en](https://img.shields.io/badge/lang-en-blue.svg)](./README.md)

bGames is a technology designed to create a balance between daily activities and video games. Your daily tasks help you improve in games! With bGames, you can earn points based on your daily activities. Using sensors, your data is collected and transformed into points, which can be used to enhance your character in the game.

This concept led to the development of this mod for Stardew Valley, integrating bGames directly into the game.  
In the mod, you can exchange your bGames points for temporary abilities (Buffs) or improve your character by leveling up skills with your points.

## [Download [Mod](https://www.nexusmods.com/stardewvalley/mods/31466)]  
([SMAPI](https://smapi.io/) is required to run the mod)

---

## Temporary abilities included in the mod:
| Name  | Description  | Duration  |
|------------|------------|------------|
| Foraging Buff     | Temporarily increases Foraging skill by +5 levels     | 7 min     |
| Luck Buff     | Increases Luck skill by +5 levels     | 7 min     |
| Farming Buff     | Temporarily increases Farming skill by +5 levels     | 7 min     |
| Mining Buff     |  Temporarily increases Mining skill by +5 levels     | 7 min     |
| Fishing Buff     | Temporarily increases Fishing skill by +5 levels     | 7 min     |
| Speed Buff     | Increases walking speed by 50%     | 7 min     |
| Stamina Buff     | Reduces energy consumption by 50% when performing actions     | 4 min     |

![PT-10-Buffs Menu](https://github.com/user-attachments/assets/8743cfd6-209d-4b8b-8e02-41b1c69f2b2c)

---

## Character upgrade abilities included in the mod:
| Name     | Description          |
|----------|----------------------|
| Mining   | Grants 50 EXP points |
| Farming  | Grants 50 EXP points |
| Fishing  | Grants 50 EXP points |
| Combat   | Grants 50 EXP points |
| Foraging | Grants 50 EXP points |

![PT-10-Skill Upgrade](https://github.com/user-attachments/assets/b33216e5-0e90-45c8-b316-cc8413e67b47)

---

## Other features:

### Mod's main window  
![PT-10-Show Skills](https://github.com/user-attachments/assets/1fe8af8d-0dd9-4887-934b-690ae7b0c0f1)

### Login with bGames account  
![PT-09-New Login](https://github.com/user-attachments/assets/803a28b5-770b-49af-863f-c66be550d593)

### Alerts  
![PT-08-Notification when activating buff](https://github.com/user-attachments/assets/23528358-c03b-4968-b3d1-53fcb1441945)

### Ability information  
![PT-06-Buff Duration](https://github.com/user-attachments/assets/d423e7ca-c13c-48e3-9fbd-11aeae767dbf)

---

## Notes:
- You cannot activate more than two temporary abilities at the same time—only one at a time!
- To use the mod correctly, you must configure bGames (instructions are provided at the end of the post).
- bGames points spent in the mod cannot be recovered; once used, they cannot be refunded.
- If you go to sleep with an active Buff, it will be lost! Plan when to activate it to make the most of its effects.
- bGames technology is constantly expanding; however, at the moment, it is not possible to create an account. Instead, pre-generated accounts with points will be provided so you can use the mod.

## Installation
[You must have [SMAPI](https://smapi.io/) installed to run the mod]
- Download the 'bGamesPointsMod.rar' file from the git hub
- Extract the downloaded file and copy the contents into the `Mods` folder in Satrdew Valley
- You must have the bGames services running on your machine to be able to use the mod (installation is explained in the next section)

---

## bGames Services Installation:
To install the [bGames](https://github.com/BlendedGames-bGames/bGames-dev-services.git) services, access the GitHub repository and follow the provided steps.  
It is recommended to use Docker to deploy the services, as it simplifies the installation process. If you do not have GitHub installed on your PC, no problem. Simply download the repository manually. To do so, click the **"<> Code"** button, where you will find the option to download the file.

---

# Developer Instructions

## Clone the repository
Download the repository from GitHub by running the following command:

```shell
git clone https://github.com/MoisesGodoy17/bGames-Stardew-Valley-Mod.git
```
Requirements to run the project:
- Docker 27.2.0
- Visual Studio
- .NET 6.0
- [SMAPI](https://smapi.io/)
- Stardew Valley

## Install SMAPI Library in the project
To correctly execute the project, it is necessary to configure the destination PATH where the mod will be compiled. In the game dependencies, update the path by entering the route of the game's 'Content' folder.

![Dependencias del mod](https://github.com/user-attachments/assets/ab332d03-06c8-4e21-884c-80d7f8c47875)

