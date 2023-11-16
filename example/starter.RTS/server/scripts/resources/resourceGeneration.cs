datablock StaticShapeData(Tree){
    shapeFile = "~/data/shapes/trees/tree1.dts";
    RTSUnitTypeName = "treeResource";
    category ="Resources";
    className = "Resource";
    maxDamage=150;
};
datablock StaticShapeData(Stone){
//    shapeFile = "~/data/shapes/rocks/quartz.dts";
    shapeFile = "~/data/shapes/rocks/rock1.dts"; // stone from stones
    RTSUnitTypeName = "rockResource";
    category = "Resources";
    className = "Resource";
    maxDamage=250;
};

datablock StaticShapeData(Gold){
    shapeFile = "~/data/shapes/rocks/quartz.dts";    // gold from gems
    RTSUnitTypeName = "goldResource";
    category = "Resources";
    className = "Resource";
    maxDamage=100;
};

// multiple versions of trees
// if ( SNOW == true ) ... TreeNoSnow01-07 or TreeSnow01-07
datablock StaticShapeData(Tree1 : Tree) { shapeFile = "~/data/shapes/winter/TreeSnow01.dts"; };
datablock StaticShapeData(Tree2 : Tree) { shapeFile = "~/data/shapes/winter/TreeSnow02.dts"; };
datablock StaticShapeData(Tree3 : Tree) { shapeFile = "~/data/shapes/winter/TreeSnow03.dts"; };
datablock StaticShapeData(Tree4 : Tree) { shapeFile = "~/data/shapes/winter/TreeSnow04.dts"; };
datablock StaticShapeData(Tree5 : Tree) { shapeFile = "~/data/shapes/winter/TreeSnow05.dts"; };
datablock StaticShapeData(Tree6 : Tree) { shapeFile = "~/data/shapes/winter/TreeSnow06.dts"; };
datablock StaticShapeData(Tree7 : Tree) { shapeFile = "~/data/shapes/winter/TreeSnow07.dts"; };

// multiple versions of rocks ... Rock01-05 or RockWithSnow01-05
datablock StaticShapeData(Stone1 : Stone) { shapeFile = "~/data/shapes/winter/RockWithSnow01.dts"; };
datablock StaticShapeData(Stone2 : Stone) { shapeFile = "~/data/shapes/winter/RockWithSnow02.dts"; };
datablock StaticShapeData(Stone3 : Stone) { shapeFile = "~/data/shapes/winter/RockWithSnow03.dts"; };
datablock StaticShapeData(Stone4 : Stone) { shapeFile = "~/data/shapes/winter/RockWithSnow04.dts"; };
datablock StaticShapeData(Stone5 : Stone) { shapeFile = "~/data/shapes/winter/RockWithSnow05.dts"; };

function Tree::onAdd(%data, %obj)
{
//   Parent::onCreate(%data, %obj);
  %obj.resourceType = "Wood";
  %obj.maxDamage += getRandom(50,200);
}
function Stone::onAdd(%data, %obj)
{
  %obj.resourceType = "Stone";
  %obj.maxDamage += getRandom(50,500);
}
function Gold::onAdd(%data, %obj)
{
  %obj.resourceType = "Gold";
  %obj.maxDamage += getRandom(50,150);
}

// so they get initialized properly ... or collection wont work
function Tree1::onAdd(%data, %obj)  {  Tree::onAdd(%data,%obj);  }
function Tree2::onAdd(%data, %obj)  {  Tree::onAdd(%data,%obj);  }
function Tree3::onAdd(%data, %obj)  {  Tree::onAdd(%data,%obj);  }
function Tree4::onAdd(%data, %obj)  {  Tree::onAdd(%data,%obj);  }
function Tree5::onAdd(%data, %obj)  {  Tree::onAdd(%data,%obj);  }
function Tree6::onAdd(%data, %obj)  {  Tree::onAdd(%data,%obj);  }
function Tree7::onAdd(%data, %obj)  {  Tree::onAdd(%data,%obj);  }

function Stone1::onAdd(%data, %obj)  {  Stone::onAdd(%data,%obj);  }
function Stone2::onAdd(%data, %obj)  {  Stone::onAdd(%data,%obj);  }
function Stone3::onAdd(%data, %obj)  {  Stone::onAdd(%data,%obj);  }
function Stone4::onAdd(%data, %obj)  {  Stone::onAdd(%data,%obj);  }
function Stone5::onAdd(%data, %obj)  {  Stone::onAdd(%data,%obj);  }


// no reason at all to have a serverCmd to seed resources. that gives the client control
// over what the server does with resources.

//function serverCmdSeedResource(%client, %type, %seed){
//               seedResource(%type, %seed);
//}

//function seedResource(%type, %seed, %max)
function seedResource(%type, %max)
{
         %minX = getWord(MissionArea.Area, 0);
         %minY = getWord(MissionArea.Area, 1);
         
         // JY -- BUG FIX ... fields 2 3 are extent size, not max x or y
         %maxX = getWord(MissionArea.Area, 2);        %maxX += %minX; 
         %maxY =  getWord(MissionArea.Area, 3);        %maxY += %minY;

         debugTxt("seedResource:: Mission Area= " SPC %minX SPC %minY SPC %maxX SPC %maxY SPC "type=" SPC %type);
                
         // JY - test to see if I can read it from the mission file
         %treeMin = MissionInfo.treeline;
         %rockMin = MissionInfo.rockline;
         if (%treeMin $= "")
            %treeMin = $Game::TreeLine; // default if not found in mission file
         if (%rockMin $= "")
            %rockMin = $Game::RockLine; // default if not found in mission file         
         debugTxt("seedResource: treeMin=" SPC %treeMin SPC ",rockMin=" SPC %rockMin);
                  
         %scale = "0.5 0.5 0.5";
         
         %count = 0;
         %loops = 0;
         
         while (%count < %max)
         {
            %i = getRandom(%minX,%maxX);
            %j = getRandom(%minY,%maxY);
            
            %rd =  getRandom(1,10);
            
            %loops++;
            if (%loops > 5000)
               {
                  debugTxt("\c2 seedResource: Looped through 5000 random locations -- giving up on adding more of this resource (" @ %type @ ")");
                  return;                  
               }

            // now 10% chance
            if (%rd == 1) 
            {
               // Check elevation
               %z = getWord(getTerrainLevel(%i SPC %j SPC 500, 1), 2);
                  
               // JY - try to place them at certain elevations
               if (%type $= "Tree")
               {
                  if (%z < %treeMin)
                  {
//                     echo("seedResource: " @ %z SPC "too low elevation for trees");
                     continue; // next location
                  }
                  if (%z > %rockMin)
                  {
//                      echo("seedResource: " @ %z SPC "too high elevation for trees");
                      continue; // next location
                  }
               }
               else if (%type $= "Stone")
                  {
                     if (%z < %treeMin)
                     {
//                        echo("seedResource: " @ %z SPC "too low elevation for gems");
                        continue; // next location
                     }                     
                  }
               else if ( %z < %rockMin)
                  {
 //                    echo("seedResource: " @ %z @ " too low elevation for rocks");
                     continue; // check next 
                  }
              // ------------
              %count++; // keep track of how many you place
              %dbType = %type;
                       
               if (%type $= "Gold")
               {
                    %z += 0.25; // raise slightly - want them to float over the ground
                    // rotate = true;
                    // gems that earn gold                  
               }
                 
               // setup trees using random-ness
               if (%type $= "Tree")
               {
                  %randTree = getRandom(1,7); // 1 - 7  
                  %dbType  = "Tree" @ %randTree;                        
               }
                                  
               if (%type $= "Stone")
               {
                    %rand = getRandom(1,5); // 1 - 5
                    %dbType  = "Stone" @ %rand;
                    %z -= 1.5; // lower the gold down so it doesnt float over terrain                    
               }
                  
               %resource = new StaticShape() 
               {
                    scale = %scale;
                    dataBlock = %dbType;
               };
                     
               %resource.setTransform( %i SPC %j SPC %z SPC "0 0 0.5 3.14" );
               ResourceSet.add(%resource);
               echo("Added resource (" @ %resource @ ") of type (" @ %dbType @ ") at (" @ %resource.getTransform() @ ")");                  
            }          
         }
}

function getTerrainLevel(%pos,%rad)
{
	while(%retries < 1000)
	{
		%x = getWord(%pos, 0) + mFloor(getRandom(%rad * 2) - %rad);
		%y = getWord(%pos, 1) + mFloor(getRandom(%rad * 2) - %rad);
		%z = getWord(%pos, 2) + mFloor(getRandom(%rad * 2) - %rad);

		%start 		= %x @ " " @ %y @ " 5000";
		%end 		= %x @ " " @ %y @ " -1";
		%ground 	= containerRayCast(%start, %end, $TypeMasks::TerrainObjectType, 0);
		%z 		= getWord(%ground, 3);
		%z += 0.05;


		%position = %x @ " " @ %y @ " " @ %z;

		%mask = ($TypeMasks::VehicleObjectType |
			$TypeMasks::MoveableObjectType |
			$TypeMasks::StaticShapeObjectType |
			$TypeMasks::ForceFieldObjectType |
			$TypeMasks::InteriorObjectType |
			$TypeMasks::ItemObjectType);

		if (ContainerBoxEmpty(%mask,%position,3.5))
		{
			return %position;
		}
		else
			%retries++;
	}
     return "0 0 1300 1 0 0 0";
}
