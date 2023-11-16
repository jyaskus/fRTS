// Load dts shapes and merge animations
exec("~/data/shapes/player/player.cs");

//exec Shape .cs file for each model
exec("~/data/shapes/warrior/warriorShape.cs");
exec("~/data/shapes/knightress/knightressShape.cs");
exec("~/data/shapes/elf/archerShape.cs"); // JY - need to load all the shapes
exec("~/data/shapes/beast/beastShape.cs");
exec("~/data/shapes/RTSCreaturePack/Goblin/goblinShape.cs");
exec("~/data/shapes/RTSCreaturePack/Stoneman/stonemanShape.cs");
exec("~/data/shapes/PACK/guardian/guardianShape.cs"); // jy dungeon guardians
exec("~/data/shapes/PACK/guardian/shamanShape.cs");   //
exec("~/data/shapes/rat/ratShape.cs");                // RATS!

datablock DecalData(PlayerFootprint)
{
   sizeX       = 0.25;
   sizeY       = 0.25;
   textureName = "~/data/shapes/player/footprint";
};

datablock DecalData(DestMarker)
{
   sizeX       = 1.0;
   sizeY       = 1.0;
   textureName = "~/data/shapes/dest_marker";
};
datablock DecalData(RedMarker)
{
   sizeX       = 1.0;
   sizeY       = 1.0;
   textureName = "~/data/shapes/marker_red";
};
datablock DecalData(OrangeMarker)
{
   sizeX       = 1.0;
   sizeY       = 1.0;
   textureName = "~/data/shapes/marker_orange";
};
datablock DecalData(BlueMarker)
{
   sizeX       = 1.0;
   sizeY       = 1.0;
   textureName = "~/data/shapes/marker_blue";
};

datablock DebrisData( PlayerDebris )
{
   explodeOnMaxBounce = false;

   elasticity = 0.15;
   friction = 0.5;

   lifetime = 4.0;
   lifetimeVariance = 0.0;

   minSpinSpeed = 40;
   maxSpinSpeed = 600;

   numBounces = 5;
   bounceVariance = 0;

   staticOnMaxBounce = true;
   gravModifier = 1.0;

   useRadiusMass = true;
   baseRadius = 1;

   velocity = 20.0;
   velocityVariance = 12.0;
};             

datablock RTSUnitData(UnitBaseBlock)
{
   renderFirstPerson = false;
   emap = true; // JY 

   shapeFile = "~/data/shapes/player/player.dts";
   RTSUnitTypeName = "base";

   cameraMaxDist = 3;
   computeCRC = true;
  
   canObserve = true;
   cmdCategory = "Clients";

   debrisShapeName = "~/data/shapes/player/debris_player.dts";
   debris = playerDebris;

   aiAvoidThis = true;

   // Foot Prints
   decalData   = PlayerFootprint;
   decalOffset = 0.25;
   
   footPuffEmitter = LightPuffEmitter;
   footPuffNumParts = 10;
   footPuffRadius = 0.25;

   dustEmitter = LiftoffDustEmitter;

   splash = PlayerSplash;
   splashVelocity = 4.0;
   splashAngle = 67.0;
   splashFreqMod = 300.0;
   splashVelEpsilon = 0.60;
   bubbleEmitTime = 0.4;
   splashEmitter[0] = PlayerFoamDropletsEmitter;
   splashEmitter[1] = PlayerFoamEmitter;
   splashEmitter[2] = PlayerBubbleEmitter;
   mediumSplashSoundVelocity = 10.0;   
   hardSplashSoundVelocity = 20.0;   
   exitSplashSoundVelocity = 5.0;

   // Controls over slope of runnable/jumpable surfaces
   runSurfaceAngle  = 60;

   // Footstep Sounds
   FootSoftSound        = FootLightSoftSound;
   FootHardSound        = FootLightHardSound;
   FootMetalSound       = FootLightMetalSound;
   FootSnowSound        = FootLightSnowSound;
   FootShallowSound     = FootLightShallowSplashSound;
   FootWadingSound      = FootLightWadingSound;
   FootUnderwaterSound  = FootLightUnderwaterSound;

   //FootBubblesSound     = FootLightBubblesSound;
   //movingBubblesSound   = ArmorMoveBubblesSound;
   //waterBreathSound     = WaterBreathMaleSound;

   //impactSoftSound      = ImpactLightSoftSound;
   //impactHardSound      = ImpactLightHardSound;
   //impactMetalSound     = ImpactLightMetalSound;
   //impactSnowSound      = ImpactLightSnowSound;
   
   //impactWaterEasy      = ImpactLightWaterEasySound;
   //impactWaterMedium    = ImpactLightWaterMediumSound;
   //impactWaterHard      = ImpactLightWaterHardSound;
   
   groundImpactMinSpeed    = 10.0;
   groundImpactShakeFreq   = "4.0 4.0 4.0";
   groundImpactShakeAmp    = "1.0 1.0 1.0";
   groundImpactShakeDuration = 0.8;
   groundImpactShakeFalloff = 10.0;
   
   //exitingWater         = ExitingWaterLightSound;
   
   observeParameters = "0.5 4.5 4.5";
   
   // RTS properties
   doWaterInteraction = true; // JY - enabling these for effect
   doShadow = true;
   doLookAnimation = false;
};

