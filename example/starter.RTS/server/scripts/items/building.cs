// map some server side building qualities
// note: for expansion purposes, we make this 2d:
//  first index is the building index, second is the building level
// this isn't perfect by any means, but it does give you a direction to go in
// for upgradeable buildings

// factory
$Buildings::requiredBuildSupplies[0,0] = "Gold 250 Stone 45";
$Buildings::requestId[0,0]             = "PlaceFactoryBuilding";
$Buildings::ActionType[0,0]            = "Supply Wood 6 10";

// shop
$Buildings::requiredBuildSupplies[1,0] = "Gold 300 Wood 75";
$Buildings::requestId[1,0]             = "PlaceShopBuilding";
$Buildings::ActionType[1,0]           = "Supply Gold 50 8";

// barracks 
$Buildings::requiredBuildSupplies[2,0] = "Gold 200 Wood 50 Stone 100";
$Buildings::requestId[2,0]             = "PlaceBarracksBuilding";
$Buildings::ActionType[2,0]           = "Hire warrior knightress archer"; // only the "hire" portion is implemented

// farm
$Buildings::requiredBuildSupplies[3,0] = "Gold 150 Wood 75";
$Buildings::requestId[3,0]             = "PlaceFarmBuilding";
$Buildings::ActionType[3,0]           = "Supply Food 15 12";

// foundry
$Buildings::requiredBuildSupplies[4,0] = "Gold 350 Stone 45 Wood 20";
$Buildings::requestId[4,0]             = "PlaceFoundryBuilding";
$Buildings::ActionType[4,0]           = "Supply Stone 10 12";

// town center
$Buildings::requiredBuildSupplies[5,0] = "Gold 350 Stone 45 Wood 20";
$Buildings::requestId[5,0]             = "PlaceTownCenterBuilding";
$Buildings::ActionType[5,0]           = "Hire villager";

// ---- ORC (ENEMY) BUILDS --- //
// factory
$Buildings::requiredBuildSupplies[6,0] = "Gold 250 Stone 45";
$Buildings::requestId[6,0]             = "PlaceFactoryBuilding";
$Buildings::ActionType[6,0]            = "Supply Wood 10 10";

// shop
$Buildings::requiredBuildSupplies[7,0] = "Gold 300 Wood 75";
$Buildings::requestId[7,0]             = "PlaceShopBuilding";
$Buildings::ActionType[7,0]           = "Supply Gold 50 8";

// barracks 
$Buildings::requiredBuildSupplies[8,0] = "Gold 200 Wood 50 Stone 100";
$Buildings::requestId[8,0]             = "PlaceBarracksBuilding";
$Buildings::ActionType[8,0]           = "Hire warrior knightress archer"; // only the "hire" portion is implemented

// farm
$Buildings::requiredBuildSupplies[9,0] = "Gold 150 Wood 75";
$Buildings::requestId[9,0]             = "PlaceFarmBuilding";
$Buildings::ActionType[9,0]           = "Supply Food 5 12";

// foundry
$Buildings::requiredBuildSupplies[10,0] = "Gold 350 Stone 45 Wood 20";
$Buildings::requestId[10,0]             = "PlaceFoundryBuilding";
$Buildings::ActionType[10,0]           = "Supply Stone 10 12";

// town center
$Buildings::requiredBuildSupplies[11,0] = "Gold 350 Stone 45 Wood 20";
$Buildings::requestId[11,0]             = "PlaceTownCenterBuilding";
$Buildings::ActionType[11,0]           = "Hire villager";

// NOTE: as described in /client/scripts/buildings.cs, this is not a clean
// implementation. All this information should be stored in a script array
// and get methods made for each input/output combination, instead of
// manually copying the information in each method

function getBuildingIndexFromDataBlockName( %datablockName )
{
  // simple indexer to map datablock names to an index for use in arrays
  switch$(%datablockName)
  {
    case "TestBuildingBlock":
      return 0;
    case "factoryBlock":
      return 0;
    case "shopBlock":
      return 1;
    case "barracksBlock":
      return 2;
    case "farmBlock":
      return 3;
    case "foundryBlock":
      return 4;
    case "townCenterBlock":
      return 5;
   // ORC buildings
    case "orcFactoryBlock":
      return 6;
    case "orcShopBlock":
      return 7;
    case "orcBarracksBlock":
      return 8;
    case "orcFarmBlock":
      return 9;
    case "orcFoundryBlock":
      return 10;
    case "orcTownCenterBlock":
      return 11;      
  }
}

function getBuildingIndexFromUnitTypeName( %unitTypeName )  
{
  // simple indexer to map rts unit type names (only thing client knows about)
  // to an index for use in arrays
  switch$(%UnitTypeName)
  {
    case "testbuilding":
      return 0;
    case "factory":
      return 0;
    case "shop":
      return 1;
    case "barracks":
      return 2;
    case "farm":
      return 3;
    case "foundry":
      return 4;
    case "townCenter":
      return 5;
   // orcs
    case "orcFactory":
      return 6;
    case "orcShop":
      return 7;
    case "orcBarracks":
      return 8;
    case "orcFarm":
      return 9;
    case "orcFoundry":
      return 10;
    case "orcTownCenter":
      return 11;     
  }
}

function getDataBlockFromUnitTypeName( %unitTypeName )  
{
  // simple indexer to map rts unit type names (only thing client knows about)
  // to an index for use in arrays
  switch$(%datablockName)
  {
    case "testbuilding":
      return "TestBuildingBlock";
    case "factory":
      return "factoryBlock";
    case "shop":
      return "shopBlock";
    case "barracks":
      return "barracksBlock";
    case "farm":
      return "farmBlock";
    case "foundry":
      return "foundryBlock";
    case "townCenter":
      return "townCenterBlock";
    // ORCS
    case "orcFactory":
      return "orcFactoryBlock";
    case "orcShop":
      return "orcShopBlock";
    case "orcBarracks":
      return "orcBarracksBlock";
    case "orcFarm":
      return "orcFarmBlock";
    case "orcFoundry":
      return "orcFoundryBlock";
    case "orcTownCenter":
      return "orcTownCenterBlock"; 
  }
}

// NOTE: The above three functions MUST be refactored

datablock RTSUnitData(TestBuildingBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/human/snow/human1.dts";
   RTSUnitTypeName = "factory"; 
   boundingBox = "10.0 10.0 3.0";
};

datablock RTSUnitData(factoryBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/human/snow/human1.dts";
   RTSUnitTypeName = "factory";   
   boundingBox = "10.0 10.0 3.0";
   Collects = "Wood";
   maxDamage = 800;
};

datablock RTSUnitData(barracksBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/human/snow/human4.dts";
   RTSUnitTypeName = "barracks";
   boundingBox = "10.0 10.0 3.0";
   maxDamage = 900;
};

datablock RTSUnitData(shopBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/human/snow/human1.dts";   
   RTSUnitTypeName = "shop";
   boundingBox = "10.0 10.0 3.0";
   Collects = "Gold";
   maxDamage = 800;
};

datablock RTSUnitData(farmBlock : UnitBaseBlock)
{  
   shapeFile = "~/data/shapes/animated/human/snow/human2.dts";
   RTSUnitTypeName = "farm";
   boundingBox = "10.0 10.0 3.0";
   Collects = "Food";
   maxDamage = 700;
};

datablock RTSUnitData(foundryBlock : UnitBaseBlock)
{  
   shapeFile = "~/data/shapes/animated/human/snow/human3.dts";   
   RTSUnitTypeName = "foundry";
   boundingBox = "10.0 10.0 3.0";
   Collects = "Stone";
   maxDamage = 800;
};

datablock RTSUnitData(townCenterBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/human/snow/human5.dts";
   RTSUnitTypeName = "townCenter";
   boundingBox = "10.0 10.0 3.0";
   maxDamage = 1200;
};
// ORC buildings
datablock RTSUnitData(orcFactoryBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/orc/snow/orc1.dts";
   RTSUnitTypeName = "orcFactory";
   boundingBox = "10.0 10.0 3.0";
   Collects = "Wood";
   maxDamage = 600;
};

datablock RTSUnitData(orcBarracksBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/orc/snow/orc4.dts";
   RTSUnitTypeName = "orcBarracks";
   boundingBox = "10.0 10.0 3.0";
   maxDamage = 800;
};

datablock RTSUnitData(orcShopBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/orc/snow/orc1.dts"; 
   RTSUnitTypeName = "orcShop";
   boundingBox = "10.0 10.0 3.0";
   Collects = "Gold";
   maxDamage = 600;
};

datablock RTSUnitData(orcFarmBlock : UnitBaseBlock)
{  
   shapeFile = "~/data/shapes/animated/orc/snow/orc2.dts";
   RTSUnitTypeName = "orcFarm";
   boundingBox = "10.0 10.0 3.0";
   Collects = "Food";
   maxDamage = 500;
};

datablock RTSUnitData(orcFoundryBlock : UnitBaseBlock)
{  
   shapeFile = "~/data/shapes/animated/orc/snow/orc3.dts"; 
   RTSUnitTypeName = "orcFoundry";
   boundingBox = "10.0 10.0 3.0";
   Collects = "Stone";
   maxDamage = 800;
};

datablock RTSUnitData(orcTownCenterBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/animated/orc/snow/orc5.dts";
   RTSUnitTypeName = "orcTownCenter";
   boundingBox = "10.0 10.0 3.0";
   maxDamage = 1000;
};

function RTSUnit::initBuildingActions(%this)
{
  // handler to hook in functionality of a building from it's base infoset
  echo("RTSUnit::initBuildingActions");
  %buildingIndex = getBuildingIndexFromDataBlockName(%this.getDataBlock().getName());
  %actionList = $Buildings::ActionType[%buildingIndex,0]; // note hardcoded 0, building levels not implemented
  %actionType = getWord(%actionList, 0); // grab the first word in the info string, it's what the building does
  echo("RTSUnit::initBuildingActions--index is (" @ %buildingIndex @ 
      ") actionList is (" @ %actionList @ ") Action type is (" @ %actionType @ ")");

  switch$(%actionType)  
  {
    case "Shop" : // not implemented
      return;
    case "Supply":
      %suppliesString = getWords(%actionList, 1); // grab everything after the type
      // parse it out
      for (%supplyIndex = 0; 1 ; %supplyIndex++)
      {
        
        %supplyToAdd = getWords(%suppliesString, %supplyIndex * 3,
                               (%supplyIndex *3) + 1);
        echo("inside action type, parsing type (" @ %actionType @ "), supplies to add (" @
             %supplyToAdd @ ")");
        if (%supplyToAdd $= "")
          break; // no more supplies to parse
        %supplyScheduleTime = getWord(%suppliesString, (%supplyIndex * 3) + 2); 
        repeatingAddSupplies(%this, %this, (%supplyScheduleTime * 1000), "LOCAL",
        "ScheduledRepeatingSupplyAdd", %supplyToAdd, "true");
      }
    case "Train": // functionality is implemented via several client side
                  // and server side commandToClient/commandToServer calls.
                  // would be best to re-factor and implement here if possible. 
      return;
  } // end switch
}  

// JY -- function to place building w/o regard to costs - used when loading mission,etc
function serverCmdPlantBuilding(%conn, %transform, %data)
{
   echo("serverCmdPlantBuilding--client(" @ %conn @ ") building at " SPC %transform );   
   // JY - skip all the checks
   
   %b = new RTSBuilding()
   {
      datablock = %data;
      scale = "1 1 1";
   };
   %b.setTransform( %transform );
   
   %b.client = %conn;
   %b.setTeam(%conn.getTeam());
   %b.setControllingConnection(%conn);
   %conn.buildings.add(%b);
   
//   %b.playRootAnimation();
   %b.setActionThread( "idle" ); // skip the build stage for these buildings
//   %b.schedule(10000, "setActionThread", "idle"); // 10s delay for it to be built -
   
  echo("serverCmdPlaceBuilding()--building (" @ %b @ "):");
  %b.initBuildingActions();
}
// -------------------------------------
//     %b.spawnAI = %b.schedule(4000,"AiTrain",4,%conn); // random build unit ?
function RTSUnit::AiTrain(%this,%index,%conn)
{
   echo("\c9 AiTrain -- enter ... " @ %this.getClassName @ " ," @ %this.getId() SPC "team=" @ %this.getTeam() );
   %state = %this.getState();
   
   // cancel any pending spawns if building is destroyed
   if ( (%state $= "Dead") || (! isObject(%this)) )
      {
         echo("\c9 AI building destroyed - canceling its build cycle");
         cancel(%this.spawnAI);
         return;
      }   
   
   // location to build unit
   %location = %this.getId().getTransform();
   %startX = getWord(%location, 0);              
   %startY = getWord(%location, 1);   
   %location = %startX SPC %startY SPC "250" SPC "0 0" SPC %conn.getTeam();
       
   // build the unit
   %conn.createPlayer(%location, %index);   
   MapHud.createPingEvent(%startX SPC %startY, "1 0 0"); // red ... DEBUG (JY)
   
   // determine the next unit to spawn
   %newUnit = getRandom(4,6);
   
   // and time to build it
   %buildTime = 10000;
   switch(%newUnit)
   {
      case 4:      
        %buildTime = 4000; // 4s
      case 5:
        %buildTime = 8000; // 8s
      case 6:
        %buildTime = 9000; // 9s
   }
   %buildTime = (%buildTime * 2); // increase the delay a bit, so doesnt spawn so fast -
   
   // create next spawn            
   %this.spawnAI = %this.schedule(%buildTime,"AiTrain",%newUnit,%conn); 
   echo("\c9 AiTrain (delay = " @ %buildTime @ "-- exit");
}
// JY -- function to place ENEMY AI buildings w/o regard to costs - used when loading mission,etc
function serverCmdPlaceAiBuilding(%conn, %transform, %data)
{
  echo("serverCmdPlaceAiBuilding--client(" @ %conn @ ") building at " SPC %transform );   
   // JY - skip all the checks
   
  %b = new RTSBuilding()
   {
      datablock = %data;
      scale = "1 1 1";
   };
  %b.setTransform( %transform );
   
  %b.client = %conn;
  %b.setTeam(%conn.getTeam());
  %b.setControllingConnection(%conn);
  %conn.buildings.add(%b);
   
  %b.setActionThread( "idle" ); // skip the build stage for these buildings
   
  echo("serverCmdPlaceAiBuilding()--building (" @ %b SPC %data @ "):");
 //  %b.initBuildingActions();
 // maybe add a .schedule here for AI
 // JY - TO DO -- the schedule can check if bldg alive and spawn a unit if so -- or there could be a queue of sorts w/ array and counter
 if (%data $= "orcBarracksBlock")
 {
    // "fake" the building of units
    %b.spawnAI = %b.schedule(8000,"AiTrain",4,%conn); // 8s
 }
}
// ----------------------------------------------------  
function serverCmdPlaceBuilding(%conn, %store, %transform, %data, %zTweak)
{
  echo("serverCmdPlaceBuilding--client(" @ %conn @ ") building (" @  
       %data @ ")\n transform (" @ %transform @ ") Tweak (" @ %zTweak @ ")");
   //TODO: do some checks to verify that we can place a building here
   
   // first, check to see if we have the right supplies
   if (%store $= "LOCAL")
   {
     %activeStore = %conn.resourceStore;
   }
   else
   {
     %activeStore = %store;
   }
   
   %requiredSupplies = $Buildings::requiredBuildSupplies[(getBuildingIndexFromDataBlockName( %data )),0];
   %requestId = $Buildings::requestId[(getBuildingIndexFromDataBlockName( %data )),0];
   
   %authString = resourceStore::requestSpendSupplies(%conn,
                                                     %activeStore,
                                                     %requestId, 
                                                     %requiredSupplies, 
                                                     "false");

   %successStatus = getWord(%authString,0);
   if (%successStatus $= "DENY") 
   {
//   	echo("serverCmdPlaceBuilding--request by (" @ %conn @ ")"  SPC getWord(%authString, 1) SPC "DENIED");
     messageClient(%conn, 'MsgPurchaseDenied', "", "Cannot place Building! missing:" SPC getWords(%authString, 2) );
   	return;
  }
  else
  {
//  	echo("serverCmdPlaceBuilding--request by" SPC %client SPC getWord(%authString, 1) SPC "APPROVED");
  }
   %b = new RTSBuilding()
   {
      datablock = %data;
      scale = "1 1 1";
   };
   %b.setTransform( %transform );
   
   %b.client = %conn;
   %b.setTeam(%conn.getTeam());
   %b.setControllingConnection(%conn);
   %conn.buildings.add(%b);
   
//   %b.playRootAnimation();    
   %b.setActionThread( "Create" );
   %b.schedule(10000, "setActionThread", "idle"); // 10s delay for it to be built -
   // JY -- PLAY BUILD SOUND here
   serverPlay3D(buildSound,%transform);
   
   // Note: using schedule is a very basic example. In a real game, adding the supply
   // to the store would probably be dependent on some form of game event, like
   // a working returning a load of carried supplies to a player's building
   // for example, how the villagers work. As you increase your building's capabilities,
   // most (if not all) will not be supply type buildings.
  echo("serverCmdPlaceBuilding()--building (" @ %b @ "):");
  %b.initBuildingActions();
}

function serverCmdQueueTrainUnit(%client, %store, %unitType)
{

 	echo("serverCmdQueueTrainUnit--request by (" @ %client @ ") unit type"  SPC %unitType);


  // first, see if we have a good building selected
  if (%client.selection.getCount() > 1) 
  {
    error("serverCmdQueueTrainUnit--client (" @ %client @ ") had more than one selection in selection group!");
    return;
  }
  %activeBuilding = %client.selection.getObject(0);
   
   // second, check to see if we have the right supplies
   switch$(%unitType)
   {
     case "0" :
       %requiredSupplies = "Gold 170 Food 225 Stone 100 Wood 5";
       %requestId = "QueueTrainWarrior";
       %trainDuration = 10000;
     case "1" :
       %requiredSupplies = "Gold 105 Food 200 Stone 120 Wood 5";
       %requestId = "QueueTrainKnightress";
       %trainDuration = 8600;
     case "2" :
       %requiredSupplies = "Gold 90 Food 110 Stone 50 Wood 130";
       %requestId = "QueueTrainArcher";
       %trainDuration = 7600;
     case "3" :
       %requiredSupplies = "Gold 45 Food 90 Stone 20";
       %requestId = "QueueTrainVillager";
       %trainDuration = 3100;
   }
      
   if (%store $= "LOCAL")
   {
     %activeStore = %client.resourceStore;
   }
   else
   {
     %activeStore = %store;
   }
   %authString = resourceStore::requestSpendSupplies(%client,
                                                     %activeStore,
                                                     %requestId, 
                                                     %requiredSupplies, 
                                                     "false");

   %successStatus = getWord(%authString,0);
   if (%successStatus $= "DENY") 
   {
//   	echo("serverCmdPlaceBuilding--request by (" @ %conn @ ")"  SPC getWord(%authString, 1) SPC "DENIED");
     messageClient(%client, 'MsgPurchaseDenied', "", "Cannot train unit! missing:" SPC getWords(%authString, 2) );
     commandToClient(%client, 'BuildUnitDenied', %client.getGhostId(%activeBuilding) );

   	return;
  }
  else
  {
//  	echo("serverCmdPlaceBuilding--request by" SPC %client SPC getWord(%authString, 1) SPC "APPROVED");
  }

  %activeBuilding.TrainUnitEventId = schedule(%trainDuration,
                                     %activeBuilding,
                                     "trainUnitDurationComplete", 
	                                   %client, %activeBuilding, %unitType);
  echo("serverCmdQueueTrainUnit--queued up unit type" SPC %unitType SPC 
       "at builiding (" @ %activeBuilding @ ") for client (" @ %client @ "), duration" @
       %trainDuration );
  commandToClient(%client, 'InitBuildMenuStatusBar', %client.getGhostId(%activeBuilding),
                   %trainDuration );
}
  
function trainUnitDurationComplete(%client, %building, %unitType)
{
// 	echo("trainUnitDurationComplete--request by (" @ %client @ ") unit type" SPC 
// 	   %unitType SPC "placing near building (" @ %building @ ")");

  // ok, unit is paid for, training time is complete, let's hand 'em over!
  // figure out a spawn point here.
  %spawnCenter = %building.getPosition();
  %spawnOffset = "-8 -8 0";
  %spawnPoint = VectorAdd( %spawnCenter, %spawnOffset); 
  %client.createPlayer( getWords(%spawnPoint,0,2), %unitType);
  messageClient(%client, 'MsgUnitComplete', "", %spawnPoint);
  commandToClient(%client, 'BuildUnitComplete', %client.getGhostId(%building) );
}
