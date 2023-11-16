datablock RTSUnitData(elfBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/elf/elf.dts"; 
   RTSUnitTypeName = "villager"; // "orc"; // JY - this might work ?!

   baseDamage=15; // 20;
   attackDelay=30;
   damagePlus=3;   
   armor=2;   
   moveSpeed = 5;  
   range=4;   
   maxDamage=200; //maxDamage = health
   vision=75;     
   isMelee=true;
      
   boundingBox = "2.0 2.0 2.0"; 
};
/* functions */
function elfBlock::onAttack(%this, %attacker, %target)
{
   %damage = %attacker.getDataBlock().baseDamage;
   if (%attacker.getNetModifier().baseDamage)
      %damage *= %attacker.getNetModifier().baseDamage;

   %armor  = %target.getDataBlock().armor;
   if (%target.getNetModifier().armor)
      %armor *= %target.getNetModifier().armor;
      
   if(%damage > %armor)
      %damage -= %armor;
   else
      %damage = 0;

  %target.damage(%attacker,0,%damage,"MELEE");
}
function elfBlock::onAdd(%this,%obj)
{
      %obj.mountImage(AxeImage,0);
      echo("RTSUnitData::onAdd -- mounting Axe to " SPC %obj.getName() );         
}