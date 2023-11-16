datablock RTSUnitData(warriorBlock : UnitBaseBlock)
{
   shapeFile="~/data/shapes/warrior/warrior.dts";  
   RTSUnitTypeName="warrior";
   baseDamage=69; ///92;
   attackDelay=46;
   damagePlus=3;   
   armor=10;   
   moveSpeed=4.5;   
   range=5;   
   maxDamage=450; //maxDamage = health
   vision=75;
   isMelee=true;
   
   rechargeRate=0.5;
   repairRate=0.5;
   
   boundingBox = "2.0 2.0 2.0";   
};

function warriorBlock::onAttack(%this, %attacker, %target)
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