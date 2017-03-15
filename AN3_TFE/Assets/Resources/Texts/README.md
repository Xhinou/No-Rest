File name must have this template :

language_sceneID_npcID_questStep_depth+choice[equalto:depth]-buttonsCount(0 is endDialog)/"buttons"(if is a button)

language : string value for language
sceneID : Match with the ID of the scene (specified in the QuestManager)
npcID : Match with the Id of the npc the player's talking to (specified in the NpcManager attached to the npc)
questStep : Match with the actual step of the quest of this npc (specified in the QuestManager)
depth : The depth of the choices tree
choice : 0=left choice; 1=right choice; range of numbers must be equal to [depth] (example: if depth = 1, file name's depth+choice must have the format "00")
buttonsCount : number of choices after the dialog (0 is the last dialog and close the text box)
