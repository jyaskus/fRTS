datablock RTSUnitData(guardianBlock : UnitBaseBlock)
{
   shapeFile="~/data/shapes/PACK/guardian/guardian.dts";  
   RTSUnitTypeName="guardian";
   baseDamage=30; ///45;
   attackDelay=28;
   damagePlus=3;   
   armor=6;   
   moveSpeed=5.5;   
   range=5;   
   maxDamage=280; //maxDamage = health
   vision=40;
   isMelee=true;
   
   rechargeRate=0.3;
   repairRate=0.3;
   
   boundingBox = "2.0 2.0 2.0";   
};

function guardianBlock::onAttack(%this, %attacker, %target)
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