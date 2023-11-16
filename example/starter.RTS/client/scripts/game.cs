//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//----------------------------------------------------------------------------
// Game start / end events sent from the server
//----------------------------------------------------------------------------

function clientCmdGameStart(%seq)
{
   PlayerListGui.zeroScores();
}

function clientCmdGameEnd(%seq)
{
   // Stop local activity... the game will be destroyed on the server
   alxStopAll();

   // Copy the current scores from the player list into the
   // end game gui (bit of a hack for now).
   EndGameGuiList.clear();
   for (%i = 0; %i < PlayerListGuiList.rowCount(); %i++)
   {
      %text = PlayerListGuiList.getRowText(%i);
      %id = PlayerListGuiList.getRowId(%i);
      EndGameGuiList.addRow(%id,%text);
   }
   EndGameGuiList.sortNumerical(1,false);

   // Display the end-game screen
   Canvas.setContent(EndGameGui);
}

function doClientAttackAnimation(%attacker, %target)
{
   if(%target.getDamageState() !$= "Enabled")
      return;
      
   %attacker.playAttackAnimation();   
   %projDB = %attacker.getProjectileDatablock();

   %vel = VectorSub(%target.getPosition(), %attacker.getPosition());
   %vel = VectorNormalize(%vel);
   %vel = VectorScale(%vel, 5);
   %vel = VectorScale(%vel, %projDB.muzzleVelocity / VectorLen(%vel));   

   // Create the projectile object, only if there is a projectile datablock   
   if( isObject( %projDB ) )
   {  
      %startPos = %attacker.getMuzzlePoint(0); //%attacker.getEyePoint();
      %endPos = %target.getEyePoint();
      
      echo("\c2 ** Creating new Projectile (" @ %projDB @")... starting at (" @  %attacker.getPosition() @ "," @ %startPos @ ") and traveling to (" @  %target.getPosition() @ "," @ %endPos @ ")" );      
      
      switch(%projDB)
      {
         case $Projectile::Green:
            %speed = 18;
            %type = "GreenProjectile";         
         case $Projectile::Arrow:
            %speed = 16;         
            %type = "ArrowProjectile";
         case $Projectile::Fireball:
            %speed = 14;
            %type = "FireballProjectile";
         default:
            %speed = 12;
            %type = "DefaultProjectile";
      }
      echo("\c2 Created Proj of type (" @ %type @ ") with speed " @ %speed );
      %p = new RTSProjectile() 
           {
               dataBlock        = %projDB;
               initialVelocity  = %vel;
               targetPos        = %endPos;
               speed            = %speed;
               initialPosition  = VectorAdd(%startPos, "0 0 1");
               sourceObject     = %attacker;
            };                   

      MissionCleanup.add(%p);
   }
}
