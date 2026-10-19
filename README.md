<div align="center">

# 🔦 EMIT: THE CYCLE STORY — 3D Laser Puzzle & Narrative Adventure

**Приключенческая 3D-головоломка с геометрической оптикой лазеров, системой логических вентилей и нелинейными диалогами с механикой кармы**

[![Unity](https://img.shields.io/badge/Engine-Unity%206-black?logo=unity)](https://unity.com/)
[![Language](https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Genre](https://img.shields.io/badge/Genre-3D%20Puzzle%20%7C%20Narrative%20Adventure-purple)]()

[**🇷🇺 Русская версия**](#-о-проекте) | [**🇬🇧 English Version (Click below or scroll)**](#-english-version)

</div>

---

<details>
<summary><b>🌐 Switch to English / Read in English (Click to expand)</b></summary>

<br>

## 🎯 Project Overview (English Version)

**Emit: The Cycle Story** is an authorial 3D puzzle-adventure game built in Unity. The core gameplay combines spatial laser-reflection puzzles and boolean logic circuits with a branching narrative driven by a persistent Karma system. The player navigates dark industrial environments using time-limited signal flares, manipulates movable mirrors and turrets, and routes laser beams into optical receivers to unlock level transitions.

### ⚙️ Technical & Architectural Highlights (Unity / C#)
* **Recursive Raycast Laser Optics (`LaserEmitter.cs`, `LaserReceiver.cs`):**
  Calculates multi-bounce laser trajectories using `Physics.Raycast` and `Vector3.Reflect(direction, hit.normal)` up to a configurable bounce limit. Features zero-allocation line rendering via cached `Vector3[]` buffers and timestamp-based knockback/damage cooldowns.
* **Modular Boolean Signal Circuitry (`ISignal.cs`, `LogicAnd.cs`, `LogicTimer.cs`, `Wire.cs`):**
  A designer-friendly signal graph architecture allowing level designers to wire laser receivers, timers, latches (`LogicActivateForever`), inverters (`Wire`), and boolean logic gates (`AND`, `OR`, `NOR`, `NAND`) directly in the Unity Inspector without writing custom level scripts.
* **Branching Dialogues & Karma State (`KarmaManager.cs`, `DialogController.cs`):**
  Tracks player moral choices across scenes (`DontDestroyOnLoad`). Dialogue triggers dynamically evaluate current Karma thresholds (`carmaBorders`) and external signal states before initiating node-based conversations.
* **Context-Sensitive Interaction & Flare System (`PlayerController.cs`, `MovableObject.cs`, `FireManager.cs`):**
  State-driven character controller (`Walking`, `MovingObject`, `RotatingObject`, `Stunned`, `Dead`) with contact-normal ground detection, automatic pruning of disabled interaction triggers, and a resource-based flare lighting mechanic with cooldowns.

</details>

---

## 🎮 О проекте

**Emit: The Cycle Story** — авторская 3D-головоломка с элементами нарративного приключения на Unity. Игра объединяет пространственные пазлы на базе отражения лазерных лучей и булевой логики с нелинейной системой диалогов, зависящей от накопленной кармы игрока.

Игрок исследует темные уровни, используя ограниченный запас временных фаеров для освещения пути, передвигает и вращает отражающие турели/зеркала и выстраивает цепочки лазерных лучей для запитки приемников и открытия дверей.

---

## 🛠 Архитектура и ключевые механики (C# / Unity 6)

Проект построен вокруг модульных систем, позволяющих собирать сложные головоломки прямо в редакторе Unity:

* **Геометрическая оптика и трассировка лазера (`LaserEmitter`, `LaserReceiver`):**
  Расчет многократных рикошетов луча через `Physics.Raycast` и векторное отражение `Vector3.Reflect(direction, hit.normal)` с ограничением глубины рекурсии (`maxBounces`). Отрисовка луча через `LineRenderer` оптимизирована за счет переиспользования буфера вершин `Vector3[]` без лишних аллокаций памяти в `Update`.
* **Конструктор логических схем и сигналов (`ISignal`, `LogicAnd`, `LogicTimer`, `Wire`):**
  Абстрактная система сигналов позволяет связывать элементы уровня в полноценные логические цепи прямо в Инспекторе:
  * **Вентили (`LogicAnd`):** Поддержка логических операций `AND`, `OR`, `NOR`, `NAND` над произвольным списком источников сигнала с предварительным кешированием компонентов.
  * **Таймеры и триггеры-защелки (`LogicTimer`, `LogicActivateForever`, `LaserReceiver`):** Требуют удерживать луч лазера в приемнике заданное количество секунд для срабатывания механизма или двери (`SceneLoader`).
* **Система кармы и ветвления диалогов (`KarmaManager`, `DialogController`):**
  Межсценовый синглтон отслеживает моральный рейтинг игрока. Контроллер диалогов проверяет попадание текущей кармы в заданный диапазон `[min, max]` и наличие входного сигнала `ISignal`, открывая уникальные ветки сюжета и катсцены (`CutsceneManager` на базе `PlayableDirector`).
* **Контроллер персонажа и механика фаеров (`PlayerController`, `FireManager`, `MovableObject`):**
  Машина состояний игрока (`Walking`, `MovingObject`, `RotatingObject`, `Stunned`, `Dead`) с проверкой приземления по нормали поверхности, физическим отталкиванием при попадании под лазер (`ForceKick`) и системой расходуемых фаеров с фазами горения, затухания и перезарядки.

---

## 🧩 Геймдизайн: взаимодействие систем

| Система | Геймплейная роль |
| :--- | :--- |
| **Отражение лазера + Турели** | Пространственная задача: игрок должен сопоставить углы падения и отражения, двигая (`MovingObject`) или поворачивая (`RotatingObject`) зеркала и турели. |
| **Удержание луча (`LaserReceiver` + `LogicTimer`)** | Защищает пазлы от случайного прохождения («прострела» лучом во время вращения): луч должен стабильно питать приемник несколько секунд, чтобы открыть дверь. |
| **Система фаеров (`FireManager`)** | Менеджмент ресурсов: фаер освещает окружение лишь на `6` секунд и требует `10` секунд перезарядки, заставляя игрока запоминать расположение зеркал и ловушек. |
| **Карма и развилки (`KarmaManager`)** | Реиграбельность: ответы в диалогах меняют счетчик кармы, блокируя или открывая альтернативные диалоговые графы на последующих уровнях. |

---

## 📂 Структура скриптов (`Assets/Scripts`)

```text
Assets/Scripts/
├── Character & Camera
│   ├── PlayerController.cs       # Движение, машина состояний, интеракция и механика фаеров
│   ├── CameraController.cs       # Плавное следование камеры (LateUpdate Lerp)
│   ├── ParabolaMovement.cs       # Скриптовые прыжки по параболической траектории
│   ├── LedgeGrab.cs              # Механика подтягивания на уступы
│   └── PlayerLifes.cs            # Отображение здоровья и состояния игрока в UI
├── Lasers & Logic Circuits
│   ├── ISignal.cs                # Базовый абстрактный класс источника логического сигнала
│   ├── LaserEmitter.cs           # Трассировка луча, рикошеты от зеркал и урон игроку
│   ├── LaserReceiver.cs          # Оптический приемник лазера, генерирующий сигнал
│   ├── LaserMovement.cs          # Патрулирование / движение лазерных излучателей
│   ├── LogicAnd.cs               # Логические вентили (AND, OR, NOR, NAND)
│   ├── LogicTimer.cs             # Таймер удержания сигнала для открытия дверей
│   ├── LogicActivateForever.cs   # Логическая защелка (RS-триггер постоянной активации)
│   └── Wire.cs                   # Передатчик и инвертор сигнала (NOT)
├── Narrative, Karma & Level Flow
│   ├── KarmaManager.cs           # Межсценовое хранение и модификация кармы
│   ├── DialogController.cs       # Условный запуск диалоговых графов по карме и сигналам
│   ├── CutsceneManager.cs        # Управление катсценами через PlayableDirector (Timeline)
│   ├── LevelManager.cs           # Завершение уровня, сохранения и переходы
│   └── SceneLoader.cs            # Двери/переходы между сценами с проверкой массива сигналов
└── Interactive Objects
    ├── MovableObject.cs          # Захват и перемещение объектов игроком (IInteractable)
    ├── Rope.cs / RopeSegment.cs  # Физика и отрисовка сегментированных канатов
    └── FireManager.cs            # Глобальный учет запаса фаеров между сценами
```
