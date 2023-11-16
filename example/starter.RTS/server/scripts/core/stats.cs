// Fun stats!
//-----------------
//
// Stat names and indices are generated at the top of this file, and are sent
// to the clients when connecting.  When an object is selected, all of the
// stat values are sent to the client.  When a stat changes while an object is selected
// it sent to the client.  Have fun with fun stats!

//stat defs...
$NumStats = -1;

$NumberToStat[$NumStats++] = "Kills";
$NumberToStat[$NumStats++] = "Damage Delt";
$NumberToStat[$NumStats++] = "Units Built";

$NumStats++;

for (%k = 0; %k < $NumStats; %k++)
{
   $StatToNumber[$NumberToStat[%k]] = %k;
}

//----------------------------------------------------------------

function sendStatsListToClient(%client)
{
   for (%k = 0; %k < $NumStats; %k++)
   {
      %statString = %statString TAB $NumberToStat[%k];
   }
   %statString = ltrim(%statString);
   messageClient(%client, 'MsgStatsList', "", %statString);
}

function sendAllStatsToClient(%client, %obj)
{
   for (%k = 0; %k < $NumStats; %k++)
   {
      %statString = %statString TAB %obj.stats[$NumberToStat[%k]];
   }
   %statString = ltrim(%statString); //trim off the first space
   messageClient(%client, 'MsgFullStatUpdate', "", %client.getGhostId(%obj), %statString);
}

function sendOneStatToClient(%client, %obj, %statName)
{
   messageClient(%client, 'MsgStatUpdate', "", %client.getGhostId(%obj), %statName, %obj.stats[%statName]);
}

function setStat(%obj, %statName, %value)
{
   %obj.stats[%statName] = %value;
   for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
   {
      %client = ClientGroup.getObject(%clientIndex);
      %selection = %client.selection;
      for (%sel = 0; %sel < %selection.getCount(); %sel++)
      {
         %selObj = %selection.getObject(%sel);
         if (%selObj == %obj)
         {
            sendOneStatToClient(%client, %obj, %statName);
            break;
         }
      }
   }
}
