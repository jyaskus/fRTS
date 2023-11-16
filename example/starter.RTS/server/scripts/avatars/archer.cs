datablock RTSUnitData(archerBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/elf/elf.dts"; 
   RTSUnitTypeName = "archer"; 
   baseDamage = 32; //48;
   attackDelay = 22;
   damagePlus = 3;
   armor = 2;   
   moveSpeed = 4.5;   
   range = 34;
   maxDamage = 220;
   vision = 150;
   isMelee = false;
   boundingBox = "2.0 2.0 2.0"; 
   repairRate=0.1;   
};

function archerBlock::onAttack(%this, %attacker, %target)
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

  %target.damage(%attacker,0,%damage,"RANGED");
}