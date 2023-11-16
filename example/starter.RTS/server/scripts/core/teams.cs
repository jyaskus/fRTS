function sendTeamInfoToClient(%client)
{
   %info = $Pref::Server::maxTeams;
   for (%i = 0; %i < $Pref::Server::maxTeams; %i++)
   {
      %info = %info TAB $Pref::Server::TeamInfo[%i];
   }
   messageClient(%client, 'MsgTeamInfo', "", %info);
}
