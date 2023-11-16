function GuiMapHud::onMouseDown(%this, %worldPos)
{
   $RTSCamera.setCameraPosition(getWord(%worldPos, 0), getWord(%worldPos, 1));
}

function GuiMapHud::onMouseDragged(%this, %worldPos)
{
   $RTSCamera.setCameraPosition(getWord(%worldPos, 0), getWord(%worldPos, 1));
}

function GuiMapHud::onRightMouseDown(%this, %worldPos)
{
   commandToServer('IssueMove', getWord(%worldPos, 0),
                                getWord(%worldPos, 1),
                                getWord(%worldPos, 2));
}
