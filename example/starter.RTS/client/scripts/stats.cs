addMessageCallback( 'MsgStatsList', handleStatsList );
addMessageCallback( 'MsgFullStatUpdate', handleFullStatUpdate );
addMessageCallback( 'MsgStatUpdate', handleStatUpdate );

function handleStatsList(%msgType, %msgString, %statsList)
{
   $NumStats = 0;
   while (getField(%statsList, $NumStats) !$= "")
   {
      %word = getField(%statsList, $NumStats);
      $StatToNumber[%word] = $NumStats;
      $NumberToStat[$NumStats] = %word;
      $NumStats++;
   }
}

function handleFullStatUpdate(%msgType, %msgString, %ghostId, %statsList)
{
   %obj = ServerConnection.resolveGhostID(%ghostId);
   for (%k = 0; %k < $NumStats; %k++)
   {
      %obj.stats[$NumberToStat[%k]] = getField(%statsList, %k);
   }
}

function handleStatUpdate(%msgType, %msgString, %ghostId, %statName, %stat)
{
   %obj = ServerConnection.resolveGhostID(%ghostId);
   %obj.stats[%statName] = %stat;
}

