# Stardew Valley Mod bGames

bGames es una tecnología diseñada para crear un equilibrio entre las actividades diarias y los videojuegos. ¡Tus tareas diarias te ayudan a mejorar en los juegos! Con bGames, puedes ganar puntos en función de tus actividades diarias. Mediante sensores, se recopilan tus datos y se transforman en puntos, que se pueden utilizar para mejorar tu personaje en el juego.

Este concepto llevó al desarrollo de este mod para Stardew Valley, integrando bGames directamente en el juego.
En el mod, puedes intercambiar tus puntos de bGames por habilidades temporales (Buffs) o mejorar tu personaje subiendo de nivel las habilidades con tus puntos.

## [Descargar [Mod](https://www.nexusmods.com/stardewvalley/mods/31466)]
(Se requiere [SMAPI](https://smapi.io/) para ejecutar el mod) 

## Habilidades temporales que incluye el mod:
| Name  | Description  | Duration  |
|------------|------------|------------|
| Foraging Buff     | Temporarily increases Foraging skill by +5 levels     | 7 min     |
| Luck Buff     | Increases Luck skill by +5 levels     | 7 min     |
| Farming Buff     | Temporarily increases Farming skill by +5 levels     | 7 min     |
| Mining Buff     |  Temporarily increases Mining skill by +5 levels     | 7 min     |
| Fishing Buff     | Temporarily increases Fishing skill by +5 levels     | 7min     |
| Speed Buff     | Increases walking speed by 50%     | 7 min     |
| Stamina Buff     | Reduces energy consumption by 50% when performing actions     | 4 min     |

![PT-10-Menu de Buffs](https://github.com/user-attachments/assets/8743cfd6-209d-4b8b-8e02-41b1c69f2b2c)

---

## Habilidades de mejora de personaje que incluye el mod:
| Name     | Description          |
|----------|----------------------|
| Mining   | Grants 50 EXP points |
| Farming  | Grants 50 EXP points |
| Fishing  | Grants 50 EXP points |
| Combat   | Grants 50 EXP points |
| Foraging | Grants 50 EXP points |

![PT-10-Mejroa de habilidades](https://github.com/user-attachments/assets/b33216e5-0e90-45c8-b316-cc8413e67b47)

## Otras funcionalidades:

Ventana principal del mod

![PT-10-Mostar habilidades](https://github.com/user-attachments/assets/1fe8af8d-0dd9-4887-934b-690ae7b0c0f1)

Inicio de sesion con cuenta bGames

![PT-09-Nuevo login](https://github.com/user-attachments/assets/803a28b5-770b-49af-863f-c66be550d593)

Alertas

![PT-08-Notificacion al activar buff](https://github.com/user-attachments/assets/23528358-c03b-4968-b3d1-53fcb1441945)

Informacion de la habilidad

![PT-06-Duracion del mod](https://github.com/user-attachments/assets/d423e7ca-c13c-48e3-9fbd-11aeae767dbf)

## Observaciones
- No puedes activar más de dos habilidades temporales al mismo tiempo, ¡solo una a la vez!
- Para usar el mod correctamente, debes configurar bGames (las instrucciones se proporcionan al final del post).
- Los puntos de bGames gastados en el mod no se pueden recuperar; una vez utilizados, no se pueden reembolsar.
- Si te vas a dormir con un Buff activo, ¡se perderá! Planifica cuándo activarlo para aprovechar al máximo sus efectos.
- La tecnología de bGames está en constante expansión; sin embargo, por el momento, no es posible crear una cuenta. En su lugar, se proporcionarán cuentas pregeneradas con puntos para que puedas usar el mod.

## bGames Services Installation:
Para instalar los servicios de [bGames](https://github.com/BlendedGames-bGames/bGames-dev-services.git), accede al repositorio de GitHub y sigue los pasos que te indicamos.
Se recomienda utilizar Docker para desplegar los servicios, ya que simplifica el proceso de instalación. Si no tienes instalado GitHub en tu PC, no hay problema. Simplemente descarga el repositorio manualmente. Para ello, haz clic en el botón “<> Código”, donde encontrarás la opción para descargar el archivo.

# Instrucciones para desarrolladores
## Clonar el repositorio
Descarga el repositorio desde GitHub ejecutando el siguiente comando:
```shell
git clone https://github.com/MoisesGodoy17/bGames-Stardew-Valley-Mod.git
```
Para ejecutar el proyecto se requiere:
- Docker 27.2.0
- Visual Studio
- Net 6.0
- [SMAPI](https://smapi.io/)
- Stardew Valley

## Instalar SMAPI Library en el proyecto
Para ejecutar correctamente el proyecto requiere configurar el PATH de destion donde se compilara el mod. En las dependencias del juego actualizar la ruta, ingresar la ruta de la carpeta 'Content' del juego.

![Dependencias del mod](https://github.com/user-attachments/assets/ab332d03-06c8-4e21-884c-80d7f8c47875)

