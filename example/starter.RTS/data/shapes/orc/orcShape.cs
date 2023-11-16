//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

datablock TSShapeConstructor(orcDts)
{
   baseShape = "./orc.dts";
   sequence0 = "./player_root.dsq root";           // root
   sequence1 = "~/data/shapes/player/player_forward.dsq run";
   sequence2 = "~/data/shapes/player/player_diesidelf.dsq die";
   sequence3 = "./peon_gather.dsq gather";
   sequence4 = "~/data/shapes/player/player_recoilde.dsq hurt";
   sequence5 = "~/data/shapes/player/player_h1swing.dsq attack1";
   sequence6 = "~/data/shapes/player/player_h1slice.dsq attack2";
   sequence7 = "~/data/shapes/player/player_h1jumpattack.dsq attack3";
   sequence8 = "~/data/shapes/player/player_h1thrust.dsq attack4";   
};         
