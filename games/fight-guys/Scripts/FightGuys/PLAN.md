## Scene Tree (SceneTree.gd / uid://bckhwgj31dkcj)
FightGuys (Node2D) ── root of the fight scene
├── PlayerCharacter (CharacterBody2D + Character)  ◄─ player's character
│   ├── SpriteRenderer (Player sprite)
│   └── Label (Player health bar)
├── EnemyCharacter (CharacterBody2D + Character)    ◄─ AI opponent
│   ├── SpriteRenderer (Enemy sprite)
│   └── Label (Enemy health bar)
├── GameController (Node2D)                       ◄─ turn logic, state machine
│   ├── Label (Turn indicator: "Your Turn" / "AI's Turn")
│   ├── Label (Round counter)
│   └── ControlPanel (Attack / Defend / Heal buttons)
└── InputHandler (Node2D)                          ◄─ keyboard input listener
    └── SignalEmitter (triggers actions in GameController)
File Structure (Scripts/FightGuys/)
File	Role	Key Methods
FightGuys.gd	Root scene node	_Ready(), _Process()
PlayerCharacter.cs	Player's character body (already exists)	Movement, jump physics
EnemyCharacter.cs	AI character body — mirrors PlayerCharacter	Movement, jump physics
ControllerCharacter.cs	Shared combat logic — both players inherit from this	attack(), defend(), heal(), takeDamage()
HealthSystem.cs	Health & damage manager (your class)	calculateEffectiveDamage(), applyHealing(), isAlive()
TurnController.cs	Turn-based state machine	_CurrentPlayer(), processMove(), switchTurn(), checkWinCondition()
AIController.cs	Heuristic AI decision tree	evaluateActions(), pickBestAction(), scoreAttack(), scoreDefend(), etc.
UIPanel.gd / ControlPanel.gd	Turn buttons UI	Button click handlers → emit to TurnController
Core Logic Flow (in TurnController.cs)
Player clicks Attack/Defend/Heal
    ↓
  TurnController.processMove(action)
    ↓
  PlayerCharacter.attack(defendingCharacter)   // or defend()/heal()
  OR EnemyCharacter.attack(playerCharacter)    // AI response
    ↓
  HealthSystem.applyDamage(damage, defender)     // accounts for defense stat
    ↓
  TurnController.checkWinCondition()             // KO reset?
    ↓
  TurnController.switchTurn()                    // player ↔ enemy
Quick Notes on Your Current Files
Character.cs — your base class. Both PlayerCharacter and EnemyCharacter should inherit from this (or a shared ControllerCharacter) so stats/attack logic are DRY.
PlayerCharacter.cs — has movement/jumping code. This belongs in the character body, not the health system. Keep it as-is; it just renders/positions the sprite.
FightGuys.cs — root node. Currently minimal. Will wire up TurnController, AIController, and UIPanel.