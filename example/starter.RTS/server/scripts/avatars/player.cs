//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------
// RTS KIT + WORLD DOM MOD + iAI merge :: JY

// Timeouts for corpse deletion.
$CorpseTimeoutValue = 10 * 1000;

// Damage Rate for entering Liquid
$DamageLava       = 0.01;
$DamageHotLava    = 0.01;
$DamageCrustyLava = 0.01;

//
$PlayerDeathAnim::TorsoFrontFallForward = 1;
$PlayerDeathAnim::TorsoFrontFallBack = 2;
$PlayerDeathAnim::TorsoBackFallForward = 3;
$PlayerDeathAnim::TorsoLeftSpinDeath = 4;
$PlayerDeathAnim::TorsoRightSpinDeath = 5;
$PlayerDeathAnim::LegsLeftGimp = 6;
$PlayerDeathAnim::LegsRightGimp = 7;
$PlayerDeathAnim::TorsoBackFallForward = 8;
$PlayerDeathAnim::HeadFrontDirect = 9;
$PlayerDeathAnim::HeadBackFallForward = 10;
$PlayerDeathAnim::ExplosionBlowBack = 11;


//----------------------------------------------------------------------------
// RTSUnitData Datablock methods
//----------------------------------------------------------------------------

//----------------------------------------------------------------------------
function RTSUnitData::onAdd(%this,%obj)
{
   %RTStype = %obj.getRTSUnitTypeName();        // warrior, orcTownCenter
   %objClassName = %obj.getId().getClassName(); // RTSUnit, RTSBuilding
   debugTxt("RTSUnitData::onAdd() object className=" @ %objClassName SPC ",RTStype=" SPC %RTStype);
      
   // Vehicle timeout
   %obj.mountVehicle = true;

   // Default dynamic RTSUnitData stats
   if  (! (%this.rechargeRate  > 0))
      %this.rechargeRate = 0.05;       // default
   if  (! (%this.rechargeRate  > 0))
      %this.repairRate = 0.05;       // default   
      
   // set them
   %obj.setRechargeRate(%this.rechargeRate);
   %obj.setRepairRate(%this.repairRate);
   
   // try to increment RTSUnit and RTSBuilding for the client ... this might work.
   %client = %obj.client;  
   %client.stats[%RTStype]++; // just a test :D -- JY
   
   //keep track of units built per client   
   if (%objClassName $= "RTSUnit") 
      %client.stats["Units Built"]++;
   else
      %client.stats["Bldg Built"]++;
   
   // Check to see if this should have a projectile,
   // OR if they need to be equipped
   if (%objClassName $= "RTSUnit") 
       switch$(%RTStype)
         {
            case "shaman":
               %obj.mountImage(StickImage,0); 
               %obj.setProjectileDatablock(GreenProjectile);      // exploding ball
               %projDB = %obj.getProjectileDatablock();
               if ($Projectile::Green == 0)
                 $Projectile::Green = %projDB;
               
            case "guardian":
               %obj.mountImage(gLanceImage,0); 
               %obj.mountImage(gHelmetImage,1); 
               %obj.mountImage(gShieldImage,2); 
               
            case "archer":
               %obj.setProjectileDatablock(ArrowProjectile);         // no FX or EXPLOSION
               %obj.mountImage(BowImage,0);                          // unit holds a elf bow               
               %projDB = %obj.getProjectileDatablock();
               if ($Projectile::Arrow == 0)
                 $Projectile::Arrow = %projDB; 
                 
            case "goblin":
               %obj.setProjectileDatablock(FireballProjectile);      // exploding ball
               %projDB = %obj.getProjectileDatablock();
               if ($Projectile::Fireball == 0)
                 $Projectile::Fireball = %projDB;
                 
            default:
               if (%this.isMelee == false)
                  {
                      // if RANGED attacks, set the projectile Datablock
                     %obj.setProjectileDatablock(CrossbowProjectile);            
                     %projDB = %obj.getProjectileDatablock();
                     if ($Projectile::Default == 0)
                        $Projectile::Default = %projDB; // DB # was ???
                     echo("RTSUnitData::onAdd projDB=(" @ %projDB @ ") for rts type " @ %RTStype);               
                  }
         } 
         
//-----------------------Begin Bug Fix: http://garagegames.com/mg/forums/result.thread.php?qt=23339	  
   %obj.addModifier(BaseStats);
//-----------------------End Bug Fix 
}

function RTSUnitData::onRemove(%this, %obj)
{
   if (%obj.client.player == %obj)
      %obj.client.player = 0;
}

function RTSUnitData::onNewDataBlock(%this,%obj)
{
}


//----------------------------------------------------------------------------

function RTSUnitData::onMount(%this,%obj,%vehicle,%node)
{
   if (%node == 0)  {
      %obj.setTransform("0 0 0 0 0 1 0");
      %obj.setActionThread(%vehicle.getDatablock().mountPose[%node],true,true);
      %obj.lastWeapon = %obj.getMountedImage($WeaponSlot);

      %obj.unmountImage($WeaponSlot);
      %obj.setControlObject(%vehicle);
      %obj.client.setObjectActiveImage(%vehicle, 2);
   }
}

function RTSUnitData::onUnmount( %this, %obj, %vehicle, %node )
{
   if (%node == 0)
      %obj.mountImage(%obj.lastWeapon, $WeaponSlot);
}

function RTSUnitData::doDismount(%this, %obj, %forced)
{
   // This function is called by player.cc when the jump trigger
   // is true while mounted
   if (!%obj.isMounted())
      return;

   // Position above dismount point
   %pos    = getWords(%obj.getTransform(), 0, 2);
   %oldPos = %pos;
   %vec[0] = " 0  0  1";
   %vec[1] = " 0  0  1";
   %vec[2] = " 0  0 -1";
   %vec[3] = " 1  0  0";
   %vec[4] = "-1  0  0";
   %impulseVec  = "0 0 0";
   %vec[0] = MatrixMulVector( %obj.getTransform(), %vec[0]);

   // Make sure the point is valid
   %pos = "0 0 0";
   %numAttempts = 5;
   %success     = -1;
   for (%i = 0; %i < %numAttempts; %i++) {
      %pos = VectorAdd(%oldPos, VectorScale(%vec[%i], c3));
      if (%obj.checkDismountPoint(%oldPos, %pos)) 
      {
         %success = %i;
         %impulseVec = %vec[%i];
         break;
      }
   }
   if (%forced && %success == -1)
      %pos = %oldPos;

   %obj.mountVehicle = false;
   %obj.schedule(4000, "setMountVehicle", true);

   // Position above dismount point
   %obj.setTransform(%pos);
   %obj.applyImpulse(%pos, VectorScale(%impulseVec, %obj.getDataBlock().mass));
   %obj.setPilot(false);
   %obj.vehicleTurret = "";
}


//----------------------------------------------------------------------------

function RTSUnitData::onCollision(%this,%obj,%col)
{
   //error("RTSUnitData - collision");

   if (%obj.getState() $= "Dead")
      return;

   // Try and pickup all items
   if (%col.getClassName() $= "Item")
      %obj.pickup(%col);
   if (%col.getClassName() $= "Resource")
      debugTxt("collide with resource");

   // Mount vehicles
   %this = %col.getDataBlock();
   if ((%this.className $= WheeledVehicleData) && %obj.mountVehicle &&
         %obj.getState() $= "Move" && %col.mountable) {

      // Only mount drivers for now.
      %node = 0;
      %col.mountObject(%obj,%node);
      %obj.mVehicle = %col;
   }
}

function RTSUnitData::onImpact(%this, %obj, %collidedObject, %vec, %vecLen)
{
   //error("RTSUnitData - IMPACT");
   %obj.damage(0, VectorAdd(%obj.getPosition(),%vec),
      %vecLen * %this.speedDamageScale, "Impact");
}


function RTSUnitData::damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
{
//   debugTxt("\c2 RTSUnitData::damage------------------------------");
      
   %RTStype = %obj.getRTSUnitTypeName();     // warrior, orcTownCenter
   %objClassName = %obj.getId().getClassName(); // RTSUnit, RTSBuilding  
   
   if (%obj.getState() $= "Dead")
      {
         debugTxt("\c2 RTSUnitData::damage - object already dead, why hurt it more.");
         return;
      }
      
   %obj.applyDamage(%damage);
   %location = "Body";

   // Deal with client callbacks here because we don't have this
   // information in the onDamage or onDisable methods
   %client = %obj.client;
   %sourceClient = %sourceObject ? %sourceObject.client : 0;
   
   // JY - give credit for damage
   %sourceClient.stats["Damage Delt"]+= %damage;
   %sourceObject.stats["Damage Delt"]+= %damage;

   // see if the damage was enough to kill it
   if (%obj.getState() $= "Dead")
   {
      // this gets called more than once, so make sure not to give extra credit for kills
      if (!(%obj.died == true))
         {
            %client.onDeath(%sourceObject, %sourceClient, %damageType, %location);      
            %obj.onDeath(%client, %sourceObject, %sourceClient, %damageType, %location);
         }
   }
   else
    if (!isObject(%obj.getAimObject()))
    {  // JY - return attack if no target
       // if we dont have an aim object, and just got attacked -- return fire       
       if ( %objClassName $= "RTSBuilding")
       {
//          debugTxt("\c2 buildings do not (currently) return aggression.");
          // units only, make sure buildings dont attack
          // but they should SHOUT for help ...
       }
       else
       {
          debugTxt("\c2 RTSUnitData::damage - Unit has no target and was attacked, so it is returning fire.");
          AISetToAttack(%obj,%sourceObject);  
       }
    }  
}

function RTSUnitData::onDamage(%this, %obj, %delta)
{   
   if (%obj.getState() $= "Dead")
      return;
      
   %RTStype = %obj.getRTSUnitTypeName();     // warrior, orcTownCenter
   %className = %obj.getId().getClassName(); // RTSUnit, RTSBuilding
//   debugTxt("RTSUnitData::onDamage - %obj.getRTSUnitTypeName=" SPC %RTStype );
//   debugTxt("RTSUnitData::onDamage - %obj.getID.getClassName=" SPC %objClassName );  
      
   // play pain cry if significant damage
   if (%delta > $GAME::minDmg)
      {        
         messageClient(%obj.client, 'MsgUnitAttacked', "", getWords(%obj.getPosition(), 0, 2));                
         debugTxt("RTSUnitData::onDamage -" SPC %className SPC "dmg=" SPC %delta);         
         if (%className $= "RTSUnit")
            {
               %obj.playPainCry();
               %obj.playHurtAnimation();
            }
      }
}

function RTSUnitData::onDisabled(%this,%obj,%state)
{  
   if (! isObject(%obj))
      {
         debugTxt("\c2 RTSUnitData::onDisabled -- client obj not valid -- ignoring");
         return;
      }
   
   if (%obj.isDisabled == true)
      return;
   %obj.isDisabled = true; // only run this once per unit ...
             
   %RTStype = %obj.getRTSUnitTypeName();     // warrior, orcTownCenter
   %objClassName = %obj.getId().getClassName(); // RTSUnit, RTSBuilding
   
   debugTxt("RTSUnitData::onDisabled - RTS Unit Type=" SPC %RTStype SPC ", class Name=" SPC %objClassName);
    
   // The player object sets the "disabled" state when damage exceeds
   // it's maxDamage value.  This is method is invoked by ShapeBase
   // state mangement code.

   // If we want to deal with the damage information that actually
   // caused this death, then we would have to move this code into
   // the script "damage" method.
   %obj.playDeathCry();
   %obj.playDeathAnimation();
   %obj.setDamageFlash(0.75);

   // Release the main weapon trigger
   %obj.setImageTrigger(0,false);

   // Schedule corpse removal.  Just keeping the place clean.
   %obj.schedule($CorpseTimeoutValue - 1000, "startFade", 1000, 0, true);
   %obj.schedule($CorpseTimeoutValue, "delete"); 
}

//-----------------------------------------------------------------------------

function RTSUnitData::onLeaveMissionArea(%this, %obj)
{
   // Inform the client
   %obj.client.onLeaveMissionArea();
}

function RTSUnitData::onEnterMissionArea(%this, %obj)
{
   // Inform the client
   %obj.client.onEnterMissionArea();
}

//-----------------------------------------------------------------------------

function RTSUnitData::onEnterLiquid(%this, %obj, %coverage, %type)
{
   switch(%type)
   {
      case 0: //Water
      case 1: //Ocean Water
      case 2: //River Water
      case 3: //Stagnant Water
      case 4: //Lava
         %obj.setDamageDt(%this, $DamageLava, "Lava");
      case 5: //Hot Lava
         %obj.setDamageDt(%this, $DamageHotLava, "Lava");
      case 6: //Crusty Lava
         %obj.setDamageDt(%this, $DamageCrustyLava, "Lava");
      case 7: //Quick Sand
   }
}

function RTSUnitData::onLeaveLiquid(%this, %obj, %type)
{
   %obj.clearDamageDt();
}


//-----------------------------------------------------------------------------

function RTSUnitData::onTrigger(%this, %obj, %triggerNum, %val)
{
   %RTStype = %obj.getRTSUnitTypeName();     // warrior, orcTownCenter
   %objClassName = %obj.getId().getClassName(); // RTSUnit, RTSBuilding
   debugTxt("\c2 RTSUnitData::onTrigger - %obj.getRTSUnitTypeName=" SPC %RTStype SPC "%obj.getID.getClassName=" SPC %objClassName SPC " triggerNum=" SPC %triggerNum);
   // This method is invoked when the player receives a trigger
   // move event.  The player automatically triggers slot 0 and
   // slot one off of triggers # 0 & 1.  Trigger # 2 is also used
   // as the jump key.
}


//-----------------------------------------------------------------------------
// Player methods
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
function Player::onDeath(%this, %client, %sourceObject, %sourceClient, %damageType, %location)
{   
   // try to only let the object die once
   if (%this.died == true)
      return;      
   %this.died = true;

   %unitType  = %this.getDatablock().RTSUnitTypeName;    // warrior, orcTownCenter
   %className = %this.getDatablock().className;          // RTSUnitData   ,RTSBuilding?   
   
   // JY - give object credit
   %sourceObject.stats["Kills"]++;     // unit kills per client
   %client.stats["Deaths"]++;          // units deaths per client
}
//----------------------------------------------------------------------------
function Player::kill(%this, %damageType)
{
   %this.damage(0, %this.getPosition(), 10000, %damageType);
}
//----------------------------------------------------------------------------
function Player::mountVehicles(%this,%bool)
{
   // If set to false, this variable disables vehicle mounting.
   %this.mountVehicle = %bool;
}

function Player::isPilot(%this)
{
   %vehicle = %this.getObjectMount();
   // There are two "if" statements to avoid a script warning.
   if (%vehicle)
      if (%vehicle.getMountNodeObject(0) == %this)
         return true;
   return false;
}


//----------------------------------------------------------------------------

function Player::playDeathAnimation(%this)
{  
   %unitType  = %this.getDatablock().RTSUnitTypeName;    // warrior, orcTownCenter
   %className = %this.getDatablock().className;          // RTSUnitData   ,RTSBuilding?

   // This will choose from 11 possible death animations.
   //if (%this.deathIdx++ > 11)
   //   %this.deathIdx = 1;
   //%this.setActionThread("death" @ %this.deathIdx);
   
   // The RTS example unit has "die" as it's animation name
   // The bot has no death animation, the building is a different name

   %anim = "die";   
   if( %className $= "RTSBuilding" )
      %anim = "Destroy";
      
   %this.setActionThread(%anim, true);         
   debugTxt("Player::playDeathAnimation (" SPC %unitType @ "," @ %className @ "," @ %anim @ ")");      
}

function Player::playAttackAnimation(%this)
{   
   %unitType  = %this.getDatablock().RTSUnitTypeName;    // warrior
   %className = %this.getDatablock().className;          // RTSUnitData   

   // play sound when attacking
   %this.playAttackSound();

   %anim="attack";   
   %max = 1;
   
   switch$(%unitType)
   {
      case "warrior":
         %max = 6;
      case "knightress":
         %max = 3;
      case "stoneman":
         %max = 5;
      case "goblin":
         %max = 5;         
      case "elf":
         %max = 3;         
      case "orc":
         %max = 3;
      case "archer":
         %max = 0;
         %anim = "fire";
   }
   
   %idx = getRandom(1,%max);
   
   if (%max > 1)
      %anim = "attack" @ %idx;
      
   %this.setActionThread(%anim);          // attack
      
   debugTxt("Player::playAttackAnimation (" SPC %unitType @ "," @ %className @ "," @ %anim @ ")" );      
}

function Player::playRunAnimation(%this)
{
   %unitType  = %this.getDatablock().RTSUnitTypeName;    // warrior
   %className = %this.getDatablock().className;          // RTSUnitData   
   
   %anim = "run";

   if ( (%unitType $= "warrior") || (%unitType $= "knightress") )
      %anim = "run" @ getRandom(1,3);
   
   %this.setActionThread(%anim);
   debugTxt("Player::playRunAnimation (" SPC %unitType @ "," @ %className @ "," @ %anim @ ")" );            
}

//
function Player::playHurtAnimation(%this)
{
   %this.setActionThread("hurt");
}


function Player::playRootAnimation(%this)
{
   %unitType  = %this.getDatablock().RTSUnitTypeName;    // warrior
   %className = %this.getDatablock().className;          // RTSUnitData   ?
  
   %anim = "root";
   if( %className $= "RTSBuilding" )
      %anim = "idle";
      
   %this.setActionThread( %anim );
   debugTxt("Player::playRootAnimation (" SPC %unitType @ "," @ %className @ "," @ %anim @ ")");      
}

function Player::playCelAnimation(%this,%anim)
{
   if (%this.getState() !$= "Dead")
      %this.setActionThread("cel"@%anim);
}


//----------------------------------------------------------------------------

function Player::playDeathCry( %this )
{
   serverPlay3D(UnitDeathCry,%this.getTransform());
//   debugTxt("Player::playDeathCry"); 
}

function Player::playPainCry( %this )
{
   serverPlay3D(UnitPainCry,%this.getTransform());
//   debugTxt("Player::playPainCry");
}

function Player::playAttackSound( %this )
{
   serverPlay3D(UnitAttackSound,%this.getTransform());
//   debugTxt("Player::playAttackSound");
}
//----------------------------------------------------------------------------

// Horrible AI code.
function Player::onAdd(%this)
{
   %unitType  = %this.getDatablock().RTSUnitTypeName;    // warrior
   %className = %this.getDatablock().className;          // RTSUnitData   
   %bMelee = %this.getDatablock().isMelee;          // RTSUnitData   
   debugTxt("\c2 - Player::onAdd (type,classname,isMelee) =(" SPC %unitType @ "," @ %className @ "," @ %bMelee @ ")");

/*      
   // Check to see if this should have a projectile, so look at the datablock
   if ((%className $= "RTSUnit") && (%this.isMelee == false))
      {
         debugTxt("Player::onAdd() object " SPC %className SPC "is not melee, projectile set to (CrossbowProjectile)");
         %this.setProjectileDatablock(CrossbowProjectile);
      }
*/      
}

function Player::onReachDestination(%this, %dest)
{
//   debugTxt("--------------------------------------");

  %distance = VectorDist(%this.getPosition(), %this.curGoal);

//   debugTxt(" Player::onReachDestination status is (" @ %this.status @ ") and my current goal is (" @ %this.curGoal @ ")");
//   debugTxt("Player::onReachDestination distance to curGoal (" @ %this.curGoal @ ") is (" @ %distance @ ")" );
   
   %distance = mAbs( VectorDist(%this.getPosition(), %this.curGoal) );
   if (%distance < $Game::CLOSE_ENOUGH) // && (!(%this.curGoal $= "")))   
      %this.curGoal = ""; // JY - just set it regardless not bother with logic
   
   if (%this.status $= "Collecting" && %this.curGoal $= "")
   {
//     %this.setActionThread("gather", true);
     %this.setActionThread("attack",true);
     debugTxt("Player::onReachDestination--(" @ %this @ ") is currently collecting"); //, scheduling collection in 2s.");    
     %this.collectResource(%this.collecting, 2000);
   }
   if (%this.status $= "Full" && %this.curGoal $= "")
   {
    //Gold.count +=  %this.currentAmt;
    debugTxt("Player::onReachDestination-- (" @ %this @ ") is dropping off (" @ %this.resourceType @ ")");
    resourceStore::requestAddSupplies(%this.client, "LOCAL", "", %this.resourceType SPC %this.currentAmt, false);
    //resourceStore::SetSupply(%this.client, );
    %this.currentAmt = "";
    if (!(%this.resourcePos $= "" ) )
    {
      %this.setMoveDestination(%this.resourcePos);
      %this.curGoal = %this.resourcePos;
      %this.status = "Collecting";
      debugTxt("\c2 [Villager slide BUG] Player::onReachDestination-- (" @ %this @ ") now moving to (" @ %this.resourcePos @ ")");
      return;
    }
    else
    {
      debugTxt("\c2 Player::onReachDestination--villager caught trying to harvest at a spot that doesn't exist!");
      %this.setMoveGoal("");
      %this.status = "Idle";
      return;
    }
   }
//--
if(%this.curGoal $= "")
   {
      debugTxt(%this @ " - Arrived at " SPC %this.getTransform() );
      %this.didWP = 0;  //added
      return;
   }
   
   
   %nextWP = PathManager::getNextWaypoint(%this.getPosition(), %this.curGoal, %this.didWP);  //added third argument
   %this.didWP++;  //added
   
   // move   
   %this.setMoveDestination(%nextWP);
   
   // this checks if close enough ? 0.01 a bit too much accuracy   
   %distance = mAbs( VectorDist(%this.getPosition(), %this.curGoal) );
   if ((%distance < $Game::CLOSE_ENOUGH) && (!(%this.curGoal $= "")))
      %this.curGoal = "";
  //else
  //    debugTxt(%this @ " - waypoint = " @ %nextWP);
      
//   debugTxt("Player::onReachDestination - (" @ %this @ ") status=(" @ %this.status @ ") goal=(" @ %this.curGoal @ ")");
}
function Player::collectResource(%this, %type ,%timeout)
{
// NOTE: The tracking of resource being carried (type and amount) isn't very authoritative.
// Currently, the player can "cheat" by setting a villager to collect an almost full carry amount
// on one resource, then manually change the villager to another resource, and in one gather cycle,
// fill up on the -new- resource and return. Best implementation (suggested) is to either allow the
// villager to track the amount for each possible resource type, and/or have their carryAmt set to 0
// if the currently harvesting resource type is not the same as currently carrying resource type (not implemented)

//This next line check the remaining health of the currentTargetResource  
  %resHealthLeft = %this.currentTargetResource.getDataBlock().maxDamage - %this.currentTargetResource.getDamageLevel();  
  if(%resHealthLeft > 5)  
  { 
//     debugTxt("Villager (" @ %this @ ") is collecting (" @ %this.resourceType @ ") every (" @ %timeout/1000 @ ") seconds");
     %this.currentAmt +=5;
     %this.currentTargetResource.applyDamage(5); 
     if(%this.currentAmt > %this.maxAmt)
     {
//         debugTxt("Villager (" @ %this @ ") is heading to Town Center carrying (" @ %this.resourceType @ ")"); 
         %TC = findTC(%this.client,%this);
         if (%TC.getPosition() $= "")
         {
           debugTxt("\c2 - Player::collectResource--No TC Found! Idling");
           %this.status = "Idle";
           if(isEventPending(%this.resourceID) )
           {
             cancel(%this.resourceID);
           }
           return;
         }
               
         // TC was found      
//         debugTxt("which is at (" @ %TC.getPosition() @ ")");       
         %this.TCLoc = %TC.getPosition();

         if(isEventPending(%this.resourceID))
         {
           cancel(%this.resourceID);
         }
         %this.status = "Full";

         %this.setMoveDestination(%this.TCLoc);
         %this.curGoal = %this.TCLoc;
     }
     else
     {
         %this.resourceID = %this.schedule (%timeout,"collectResource" , %type,  %timeout);
     }
  } // if resHealthLeft > 0  
  else //kill the resource.  To add: send the villager to the TC to drop off if they harvested anything before depletion, also try to clear the harvesting event and the current resource ID.  
  {       
    %this.currentTargetResource.schedule($CorpseTimeoutValue - 1000, "startFade", 1000, 0, true);  
    %this.currentTargetResource.schedule($CorpseTimeoutValue, "delete");  
    debugTxt("\c2 resource exhausted, scheduling for deletion");
  }   
}


function findTC(%client,%villager)
{
   %tmpClosest = "1000000";
   %tmpClosestID = 0;
//   debugTxt("This villager is trying to drop off some " @ %villager.resourceType);
   for(%i = 0; %i < %client.buildings.getCount(); %i++)
    {
       %bl = %client.buildings.getObject(%i);
//       debugTxt("This building can collect: " @ %bl.getDataBlock().Collects);
       if(%bl.getDataBlock().getName() $= "townCenterBlock" || %bl.getDataBlock().Collects $= %villager.resourceType)
       {
         %vel = VectorLen(VectorSub(%bl.getPosition(), %villager.getPosition()));
         if(%vel < %tmpClosest)
            {
               %tmpClosest = %vel;
               %tmpClosestID = %bl;
            }
//         debugTxt("Distance to TC:" @ %vel);
       }
    }

   if(%tmpClosestID != 0)
   {
//      debugTxt("Villager is looking for closest drop off point. Found (" @ %tmpClosestID @ ") at (" @ %tmpClosestID.getPosition() @ ")");
      return %tmpClosestID;
   }
   debugTxt("\2c findTC--could not find a TC, HANDLE THIS STATE");
   return false;
}

function Player::onReachTarget(%this, %target)
{
   //error("Player CALLBACK! " @ %target);   
}

function Player::setMoveGoal(%this, %dest)
{
   %this.curGoal = %dest;
   
   // Kick off the movement.
   %this.onReachDestination(%this.getPosition());
}

function Player::stopAll(%this)
{
   %this.stop();
   %this.clearAim();
}


// ------------------------------
// JY - to find a target
function Player::startscan(%this,%maxRange)
{   
//   debugTxt("\c2 Player::startscan ---------------------------------");
   if(! isObject(%this))
      return; // do i exist?
           
   if (%this.getState() $= "Dead")
      return; // if i am dead    
      
   if( isObject(%this.getAimObject()) )
   {
      cancel(%this.scanloop);
      %this.scanloop = %this.schedule(10000,"startscan", %this, %maxRange);  // wait 10s
      return; // exit if already have an aim object
   }
   
   if ( (%this.searchDelay $= "") || (%this.searchDelay < 5000) || (%this.searchDelay > 30000))
      %this.searchDelay = 5000; // 5 seconds default      
        
   // ok search
   InitContainerRadiusSearch(%this.getPosition(), %maxRange, $TypeMasks::PlayerObjectType);
   
   // init the variables
   %nearestTarget = (%maxRange + 1); // find a target closer than this
   %targetAquired = false;
   %theTargetObject = 0; // final target
   
   while ((%targetObject = containerSearchNext()) != 0)
   {
      if ( %targetObject.getTeam() $= %this.getTeam() )
      {
//           debugTxt("\c2 ... ignoring - same team");           
      }
      else if (%targetObject.getClassName() $= "RTSBuilding")
         {
            // ignore buildings for now
         }
         else
         {          
            // bad guy found - find the closest one
            %targetDistance = vectorDist(%this.getTransform(), %targetObject.getTransform());
            if  (%targetDistance < %nearestTarget)
            {
               %nearestTarget = %targetDistance;
               %theTargetObject = %targetObject;
               %targetAquired = true;
               debugTxt("\c2 " @ %this.getClassName() @ " - Target aquired");
            }
         }      
   }
   
  if (%targetAquired $= true)
  {
//     %this.client.sendAttackEvent(%player.selectionWrapper, %theTargetObject);
//     %this.setAimObject(%theTargetObject);     
//     debugTxt("\c2 attacking new target");
     AISetToAttack(%theTargetObject,%this); 
     
     %this.searchDelay+=5000; // add a 5 seconds delay
  }
  else
  {
     if (%this.getState() $= "idle")
      {
           debugTxt("\c2 " @ %this.getClassName() @ " - moving to random target");
           // no target found, wander aimlessly - a short distance ?  
           %currX = getWord(%this.getTransform(), 0);
           %currY = getWord(%this.getTransform(), 1);
           %newDest = getRandomMove(%currX,%currY);
           %this.setMoveGoal(%newDest);
      }
//      else
//           debugTxt("\c2 unit not idle ... ");
  }

  cancel(%this.scanloop);
  %this.scanloop = %this.schedule(%this.searchDelay,"startscan", %this, %maxRange);  
  
//  debugTxt("\c2 --------------------------------- Player::startscan");
}

// ---------------------------
exec ("~/server/scripts/avatars/base.cs");
// Load dts shapes and merge animations

// Load unit datablocks
exec ("~/server/scripts/avatars/warrior.cs");   // JY
exec ("~/server/scripts/avatars/archer.cs");    
exec ("~/server/scripts/avatars/knightress.cs");
exec ("~/server/scripts/avatars/villager.cs");
exec ("~/server/scripts/avatars/beast.cs");     
exec ("~/server/scripts/avatars/goblin.cs");    
exec ("~/server/scripts/avatars/stoneman.cs");  
exec ("~/server/scripts/avatars/guardian.cs"); // jy dungeon guardians
exec ("~/server/scripts/avatars/shaman.cs"); // jy dungeon shamans
exec ("~/server/scripts/avatars/rat.cs"); // RATS!

// Load the rest of the stuff.
exec ("~/server/scripts/avatars/pathManager.cs");
