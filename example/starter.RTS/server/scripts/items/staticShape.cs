//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Hook into the mission editor.

function StaticShapeData::onCreate(%data, %obj)
{
   Parent::onCreate(%data, %obj);
   //do nothing
}

function StaticShapeData::create(%data)
{
   // The mission editor invokes this method when it wants to create
   // an object of the given datablock type.
   %obj = new StaticShape() {
      dataBlock = %data;
   };
   %data.onCreate(%obj);
    
   return %obj;
}


