//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// These are the datablocks for the rain and lightning effects below
datablock AudioProfile(HeavyRainSound)
{
   filename    = "~/data/sound/environment/ambient/rain.ogg";
   description = AudioLooping2d;
};

datablock PrecipitationData(HeavyRain)
{
   soundProfile = "HeavyRainSound";

   dropTexture = "~/data/environment/rain";
   splashTexture = "~/data/environment/water_splash";
   dropSize = 0.75;
   splashSize = 0.2;
   useTrueBillboards = false;
   splashMS = 250;
};
//-----------------------------------------------------------------------------

datablock AudioProfile(ThunderCrash1Sound)
{
   filename  = "~/data/sound/environment/ambient/thunder1.ogg";
   description = Audio2d;
};

datablock AudioProfile(ThunderCrash2Sound)
{
   filename  = "~/data/sound/environment/ambient/thunder2.ogg";
   description = Audio2d;
};

datablock AudioProfile(ThunderCrash3Sound)
{
   filename  = "~/data/sound/environment/ambient/thunder3.ogg";
   description = Audio2d;
};

datablock AudioProfile(ThunderCrash4Sound)
{
   filename  = "~/data/sound/environment/ambient/thunder4.ogg";
   description = Audio2d;
};

datablock LightningData(LightningStorm)
{
   strikeTextures[0]  = "starter.RTS/data/environment/lightning1frame1";
   strikeTextures[1]  = "starter.RTS/data/environment/lightning1frame2";
   strikeTextures[2]  = "starter.RTS/data/environment/lightning1frame3";
   
   //strikeSound = LightningHitSound;
   thunderSounds[0] = ThunderCrash1Sound;
   thunderSounds[1] = ThunderCrash2Sound;
   thunderSounds[2] = ThunderCrash3Sound;
   thunderSounds[3] = ThunderCrash4Sound;
   
   directDamageType = $DamageType::Lightning;     
   directDamage = 100;     
};

datablock PrecipitationData(HeavySnow)
{
   dropTexture = "~/data/environment/snow";
   splashTexture = "~/data/environment/snow";
   dropSize = 0.27;
   splashSize = 0.27;
   useTrueBillboards = false;
   splashMS = 50;
};

function LightningData::applyDamage(%data, %lightningObj, %targetObject, %position, %normal)  
{  
    %targetObject.damage(%lightningObj, %position, %data.directDamage, %data.directDamageType);  
}