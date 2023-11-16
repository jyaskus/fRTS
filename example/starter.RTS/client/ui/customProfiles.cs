//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (c) 2002 GarageGames.Com
//-----------------------------------------------------------------------------

new GuiControlProfile (GuiRTSContentProfile)
{
   canKeyFocus = true;
   opaque = true;
   fillColor = "255 255 255";
};

new GuiControlProfile (GuiDefaultProfile)
{
   tab = false;
   canKeyFocus = false;
   hasBitmapArray = false;
   mouseOverSelected = false;

   // fill color
   opaque = false;
   fillColor   = "213 120 81";
   fillColorHL = "223 138 86";
   fillColorNA = "223 138 86";

   // border color
   border = false;
   borderColor   = "0 0 0"; 
   borderColorHL = "179 134 94";
   borderColorNA = "126 79 37";

   // font
   fontType = "Arial";
   fontSize = 14;

   fontColor = "0 0 0";
   fontColorHL = "255 255 255";
   fontColorNA = "0 0 0";
   fontColorSEL= "200 200 200";

   // bitmap information
   bitmap = "./demoWindow";
   bitmapBase = "";
   textOffset = "0 0";

   // used by guiTextControl
   modal = true;
   justify = "left";
   autoSizeWidth = false;
   autoSizeHeight = false;
   returnTab = false;
   numbersOnly = false;
   cursorColor = "0 0 0 255";

   // sounds
   soundButtonDown = "";
   soundButtonOver = "";
};

new GuiControlProfile (GuiWindowProfile)
{
   opaque = true;
   border = 2;
   fillColor   = "213 120 81";
   fillColorHL = "223 138 86";
   fillColorNA = "223 138 86";
   fontColor   = "255 255 255";
   fontColorHL = "255 255 255";
   text        = "GuiWindowCtrl test";
   bitmap      = "./demoWindow";
   textOffset  = "6 6";
   hasBitmapArray = true;
   justify = "center";
};

new GuiControlProfile (GuiScrollProfile)
{
   opaque = false;
   fillColor = "203 113 76";
   border = 3;
   borderThickness = 2;
   borderColor = "0 0 0";
   bitmap = "./demoScroll";
   hasBitmapArray = true;
};

new GuiControlProfile (GuiCheckBoxProfile)
{
   opaque = false;
   fillColor = "232 133 96";
   border = false;
   borderColor = "0 0 0";
   fontSize = 14;
   fontColor = "0 0 0";
   fontColorHL = "255 255 255";
   fixedExtent = true;
   justify = "left";
   bitmap = "./demoCheck";
   hasBitmapArray = true;
};

new GuiControlProfile (GuiRadioProfile)
{
   fontSize = 14;
   fillColor = "232 133 96";
   fontColorHL = "255 255 255";
   fixedExtent = true;
   bitmap = "./demoRadio";
   hasBitmapArray = true;
};

