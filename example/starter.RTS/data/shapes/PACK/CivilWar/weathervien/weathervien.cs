//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

datablock StaticShapeData(weathervien)
{
   // The category variable determins where the item
   // shows up in the mission editor's creator tree.
   // Path is relative to where the script is executed from.
   category = "weathervien";
   shapeFile = "~/data/shapes/weathervien/weathervien.dts";
   isPlaying = 1;
  
   
   preload = true;


};

function weathervien::onAdd(%this, %obj)
{
   %obj.playThread(0, "animation");
}

