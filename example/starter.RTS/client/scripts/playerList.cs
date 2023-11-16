//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Hook into the client update messages to maintain our player list
// and scoreboard.
//-----------------------------------------------------------------------------

addMessageCallback('MsgClientJoin', handleClientJoin);
addMessageCallback('MsgClientDrop', handleClientDrop);
addMessageCallback('MsgClientScoreChanged', handleClientScoreChanged);
addMessageCallback('MsgClientReady', handleClientReady);
//-----------------------------------------------------------------------------

function handleClientJoin(%msgType, %msgString, %clientName, %clientId,
   %guid, %score, %isAI, %isAdmin, %isSuperAdmin )
{
   PlayerListGui.update(%clientId,detag(%clientName),%isSuperAdmin,
      %isAdmin,%isAI,%score);
   
   // Update or add the player to the lobby control
   %tag = %isSuperAdmin ? "[Super]" :
	  (%isAdmin ? "[Admin]" :
	  (%isAI ? "[Bot]" :
	  "[Human]"));

   %text = "NOT Ready" TAB %tag TAB StripMLControlChars(detag(%clientName));
   echo(%text SPC %clientId);
   if (LobbyPlayerList.getRowNumById(%clientId) == -1)
      LobbyPlayerList.addRow(%clientId, %text);
   else
      LobbyPlayerList.setRowById(%clientId, %text);
}

function handleClientDrop(%msgType, %msgString, %clientName, %clientId)
{
   PlayerListGui.remove(%clientId);
   LobbyPlayerList.removeRowById(%clientId);
}

function handleClientScoreChanged(%msgType, %msgString, %score, %clientId)
{
   PlayerListGui.updateScore(%clientId,%score);
}

function handleClientReady(%msgType, %msgString, %clientId, %ready)
{
   %text = LobbyPlayerList.getRowTextById(%clientId);
   
   if (%ready)
      %text = setField(%text, 0, "Ready!");
   else
      %text = setField(%text, 0, "NOT Ready");
      
   LobbyPlayerList.setRowById(%clientId, %text);
}
