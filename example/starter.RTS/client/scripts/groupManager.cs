//This is the client side group manager.  This allows the user to bind
//keys to certain groups of units for quick selection.  Also included
//is a way to snap the camera to a group by double tapping the group's key.

//negative value means unlimited
$MaxGroupSize = -1;
$MaxGroups = 10;  //one for each # key
$DoubleTapTime = 250;


//--------------------------------------------
// Group Manager
//--------------------------------------------

if (!isObject(GroupManager))
{
   new ScriptObject(GroupManager)
   {
      class = GMan;
   };

   for (%k = 0; %k < $MaxGroups; %k++)
   {
      GroupManager.groups[%k] = new SimSet();
   }
}

function GMan::isValidGroup(%this, %group, %error)
{
   if (%group $= "" || %group < 0 || %group >= $MaxGroups)
   {
      warn(%error);
      return false;
   }
   return true;
}

function GMan::addObjectToGroup(%this, %group, %obj)
{
   if (!(%this.isValidGroup(%group, "Trying to add to non-existent group!")))
      return false;

   if ($MaxGroupSize > 0 && %this.groups[%group].getCount() >= $MaxGroupSize)
      return false;

   %this.groups[%group].add(%obj);
   return true;
}

function GMan::setGroup(%this, %group)
{
   if (!(%this.isValidGroup(%group, "Trying to set a non-existent group!")))
      return;

   %this.groups[%group].clear();
   for (%k = 0; %k < PlayGui.getSelectionSize(); %k++)
   {
      %this.groups[%group].add(PlayGui.getSelectedObject(%k));
   }
}

function GMan::dumpGroup(%this, %group)
{
   if (!(%this.isValidGroup(%group, "Trying to dump a non-existent group!")))
      return;

   %this.groups[%group].listObjects();
}

function GMan::dumpGroups(%this)
{
   for (%k = 0; %k < $MaxGroups; %k++)
   {
      echo(" --Group " @ %k @ ":");
      %this.groups[%k].listObjects();
   }
}

function GMan::getGroup(%this, %group)
{
   if (!(%this.isValidGroup(%group, "Trying to access non-existent group!")))
      return 0;

   return %this.groups[%group];
}

function GMan::selectGroup(%this, %group)
{
   if (!(%this.isValidGroup(%group, "Trying to select non-existent group!")))
      return false;

   PlayGui.clearSelection();
   PlayGui.selectGroup(%this.groups[%group]);
   return true;
}

function GMan::addGroupSelection(%this, %group)
{
   if (!(%this.isValidGroup(%group, "Trying to add non-existent group!")))
      return;

   PlayGui.selectGroup(%this.groups[%group]);
}

function GMan::selectGroupFromKeyboard(%this, %group)
{
   if (!(%this.selectGroup(%group)))
      return;

   if (%this.selectedGroup == %group)
   {
      if (getSimtime() - %this.selectTime < $DoubleTapTime)
      {
         // snap to group
         %pos = %this.getGroup(%group).getObject(0).getPosition();
         $RTSCamera.setCameraPosition(getWord(%pos, 0), getWord(%pos, 1));
      }
   }
   %this.selectedGroup = %group;
   %this.selectTime = getSimTime();
}

function GMan::createGroup(%this, %group)
{
   if (!(%this.isValidGroup(%group, "Trying to assign a non-existent group!")))
      return;

   %this.setGroup(%group);
}

function GMan::addGroupToGroup(%this, %group)
{
   if (!(%this.isValidGroup(%group, "Trying to add to a non-existent group!")))
      return;

   for (%k = 0; %k < PlayGui.getSelectionSize(); %k++)
   {
      //we can only add as many objects as we have group slots
      if ( !( %this.addObjectToGroup( %group, PlayGui.getSelectedObject( ( %k ) ) ) ) )
         break;
   }
   %this.selectGroup(%group);
}

