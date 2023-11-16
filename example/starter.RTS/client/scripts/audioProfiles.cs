//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// Channel assignments (channel 0 is unused in-game).

$GuiAudioType     = 1;
$SimAudioType     = 2;
$MessageAudioType = 3;

new AudioDescription(AudioGui)
{
   volume   = 1.0;
   isLooping= false;
   is3D     = false;
   type     = $GuiAudioType;
};

new AudioDescription(AudioMessage)
{
   volume   = 1.0;
   isLooping= false;
   is3D     = false;
   type     = $MessageAudioType;
};

new AudioProfile(AudioButtonOver)
{
   filename = "~/data/sound/buttonOver.wav";
   description = "AudioGui";
   preload = true;
};

$UnitAudioType = 5; // channel 5 if it works
new AudioDescription(AudioUnits)
{
   volume = 1.5;
   isLooping = true;
   is3D = true;
   type = $UnitAudioType; // channel
};

new AudioProfile(AudioUnitDeath)
{
   filename = "~/data/sound/orc_death.ogg";
   description = "AudioUnits";
   preload=true;
};
new AudioProfile(AudioUnitPain)
{
   filename = "~/data/sound/orc_pain.ogg";
   description = "AudioUnits";
   preload=true;
};