Name: Kenneth Mak
Stu#: 101035611


Notes:
Check instruction option in the main menu to see the types of enemies
Warrior and Rifleman are basic AI
Shotgunner has some Wander AI (while reloading, he'll run around)
Warlord is the boss mob. He spawns at 50 kills, and comes with a formation

You can press Esc in the game to pause and exit to main menu

//DEBUG_MODE//
To access the debug menu, you have to open the unity project. You cannot access via .exe
Once done, access the 'Game Scene'. The first object in the hierachy is GameMaster.
Click on GameMaster. It contains GameMaster.cs and NumberMaster.cs 

GameMaster.cs contains all the spawning for enemy, ally, as well as the debug mode.
	Check the public bool 'debugMode' to gain access to hotkey commands.
Debug Mode commands
	Pressing 5 spawns a Spearman with no cost
	Pressing 6 spawns an Archer with no cost
	Pressing R kills all the enemies
	Pressing T will start the boss wave


	You can also change your starting gold value
Under NumberMaster.cs in the GameMaster gameobject are the public float variables
	Change gold to whatever you need