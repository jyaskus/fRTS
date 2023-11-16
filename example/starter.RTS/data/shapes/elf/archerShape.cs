//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

datablock TSShapeConstructor(archerDts)
{
   baseShape = "./elf.dts";
   sequence0 = "./elf_root.dsq root";
   sequence1 = "~/data/shapes/player/player_forward.dsq run";
   sequence2 = "~/data/shapes/player/player_diehead.dsq die";
   sequence3 = "./elf_recoilde.dsq hurt";                      // when damage
   sequence4 = "./peon_gather.dsq gather";
   sequence5 = "~/data/shapes/player/player_h1swing.dsq attack1";
   sequence6 = "~/data/shapes/player/player_h1slice.dsq attack2";
   sequence7 = "~/data/shapes/player/player_h1jumpattack.dsq attack3";
   sequence8 = "~/data/shapes/player/player_h1thrust.dsq attack4";
   sequence9 = "./elf_recoilde.dsq fire";                      // fire bow anim ?
};
