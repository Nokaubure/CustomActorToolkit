# CustomActorToolkit
Tool to create custom actors for Zelda64 games

Releasing source code of this tool as I feel it already did its job in the Zelda64 community and newer ones should appear sooner or later. For those who dont know what Custom Actor Toolkit (CAT) is, a quick explanation...

- Able to dissasemble and assemble actors directly from a targeted ROM in MIPS language, and inject them into ROM (obsolete because OoT decomp is now a thing)

- Able to compile and inject actors written in C, using either z64ovl or z64hdr libraries

- Able to extract and inject objects from a targeted ROM

- Support to zzrtl (tool to manage projects)

- Support for Link and pause menu overlays, even if they're not technically actors (treaten as actor IDs 0 and -1)

- Extra features:
	- Random Table Drop Editor (OoT)
	- Copy animations from one zobj to another
	- Get the offsets of all the textures from a zobj


Credits:

- Nokaubure for all the coding of this tool
- ZZT32 for making nOvl
- mzxrules for making Atom
- z64me and CrookedPoe for making z64ovl, and all the people involved with it
- all the people involved in z64hdr and zzrtl

