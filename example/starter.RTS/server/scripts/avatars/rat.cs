datablock RTSUnitData(ratBlock : UnitBaseBlock)
{
   shapeFile="~/data/shapes/rat/rat.dts";  
   RTSUnitTypeName="rat";
   baseDamage=28; 
   attackDelay=22;
   damagePlus=3;   
   armor=3;   
   moveSpeed=6;   
   range=6;   
   maxDamage=210; //maxDamage = health
   vision=60;
   isMelee=true;
   
   rechargeRate=0.3;
   repairRate=0.3;
   
   boundingBox = "2.0 2.0 2.0";   
};

function ratBlock::onAttack(%this, %attacker, %target)
{
   %damage = %attacker.getDataBlock().baseDamage;
   if(%attacker.getNetModifier().baseDamage)
      %damage *= %attacker.getNetModifier().baseDamage;

   %armor  = %target.getDataBlock().armor;
   if(%target.getNetModifier().armor)
      %armor *= %target.getNetModifier().armor;
      
   if(%damage > %armor)
      %damage -= %armor;
   else
      %damage = 0;

   %target.damage(%attacker,0,%damage,"MELEE");
}