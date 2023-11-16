#include "game/RTS/guiMapHud.h"
#include "game/RTS/RTSUnit.h"
#include "game/missionArea.h"
#include "game/RTS/RTSCamera.h"
#include "game/RTS/RTSConnection.h"

struct ClientUnit{
 S32 team;
 Point2I point;
};//ClientUnit


//invoked to render the actual map with everything on it
void GuiMapHud::onRender(Point2I offset, const RectI &updateRect)
{
   RTSConnection* conn = (RTSConnection*)NetConnection::getConnectionToServer();
   //need a net connection
   if (!mConnection || !mTerrainBlock)
      return;

   //update the pings
   U32 currTime = Platform::getVirtualMilliseconds();
   if (mLastRenderTime)
      updatePings(currTime - mLastRenderTime);
   mLastRenderTime = currTime;

   //render the map
//   dglSetBitmapModulation(ColorF(1.0, 1.0, 1.0));
//   dglDrawBitmap(mRenderTexture, offset, GFlip_Y);

   const RectI missionArea = MissionArea::smMissionArea;

     Point2I drawChunkPoint;			// Will hold a point to render some map at
     RectI mapRect;				// Will hold our source rect from the map image
     ClientUnit* unitList = 0;		// List of important info about our units
     ClientUnit* unitIter;			// To iterate through the list later
     int listSize = 0;			// Count of the unit list
     int vision = 10;			// Unit vision range on the map (in pixels?)
     RTSConnection* myConn = 0; // Will hold our connection to the game
     S32 localTeam = 0;                  // WIll hold our team id

     myConn = (RTSConnection*)mConnection->getConnectionToServer();
     localTeam = myConn->getTeam(); // Now get our team id

     for(SimSetIterator itr(mConnection); *itr; ++itr)
     {
      if((*itr)->getType() & PlayerObjectType)
           listSize++;
     }//for
     unitIter = unitList = new ClientUnit[listSize];

   //render the units
//   glPointSize(2.0f);
//   glColor3f(0.0, 0.0, 1.0);
//   glBegin(GL_POINTS);
//   static char buff[19];
//   dSprintf(buff, sizeof(buff), "$Server::TeamInfo0");
   dglSetBitmapModulation(ColorF(1.0, 1.0, 1.0));
   static char buff[20];  

   // only capture the map info first pass
   for (SimSetIterator itr(mConnection); *itr; ++itr)
   {
      if ((*itr)->getType() & PlayerObjectType)
      {
         if (RTSUnit* unit = dynamic_cast<RTSUnit*>(*itr))
         {
            if (!conn->isUnitVisible(unit))
               continue;
            Point2F screenPos;
            if (projectPoint(unit->getPosition(), &screenPos))
            {
               // Only give us map vision for our team
                  if(localTeam == unit->getTeam())         
                  {   
                   F32 unitVision = ((RTSUnitData*)(unit->getDataBlock()))->mVision + unit->mNetModifier.mVision;
                   vision = unitVision / 10;
                   // since default vision was 10.0 and default mVision is 100                  
                 // Get the XY of the upper left corner that the unit can see  
                 drawChunkPoint.set(offset.x + screenPos.x - vision, offset.y + screenPos.y - vision);  

                 // Get the rect of the map to render. Since it's flipped  
                 // on the Y axis before rendering we need to "come in"  
                 // from the opposite side  
                 mapRect.set(screenPos.x - vision, mTextureSize - screenPos.y - vision, vision * 2, vision * 2);  
                      
                 // Drop the rect from the map at point drawChunk with  
                 // the clipping rectangle mapRect and then flip it  
//                 dglDrawBitmapSR(mRenderTexture, drawChunkPoint, mapRect, GFlip_Y);  
                 dglDrawBitmapCircularSR(mRenderTexture, drawChunkPoint, mapRect, 12, GFlip_Y);
                 // Save the info we'll need to render the blip later  
               }//if
               unitIter->point.set(offset.x + screenPos.x, offset.y + screenPos.y);  
               unitIter->team = unit->getTeam();  
               unitIter++;  
            } 
         }
      }
   }  

   // now try to draw the units
   glPointSize(2.0f);  
   glColor3f(0.0, 0.0, 1.0);  
   glBegin(GL_POINTS);  
   dSprintf(buff, sizeof(buff), "$Server::TeamInfo0");
   unitIter = unitList;                        // Reset the iterator  
   for(int i = 0; i < listSize; i++)
   {  
        buff[17] = '0' + unitIter->team;
        const char* pref = Con::getVariable(buff);  
        ColorF teamColor;  
        dSscanf(pref, "%f %f %f", &teamColor.red, &teamColor.green, &teamColor.blue);  
        glColor3f(teamColor.red, teamColor.green, teamColor.blue);  
        glVertex2f(unitIter->point.x, unitIter->point.y);                      
        unitIter++;  
   } // for
   glEnd();
   delete []unitList;                      // Cleanup!
   glPointSize(1.0f);

   //render the camera box
   if (RTSCamera* camera = dynamic_cast<RTSCamera*>(mConnection->getControlObject()))
   {
      RectF cameraRect = camera->getViewableRect();
      Point2F center = cameraRect.point + cameraRect.extent * 0.5;

      Point2F screenCenter;
      projectPoint(center, &screenCenter);
      Point2F squareOffset;
      projectPoint(Point3F(center.x + cameraRect.extent.x / 2, center.y + cameraRect.extent.y / 2, 0), &squareOffset);
      squareOffset -= screenCenter;
      screenCenter.x += offset.x;
      screenCenter.y += offset.y;

      glColor3f(0.0, 1.0, 0.0);
      glBegin(GL_LINE_LOOP);
      glVertex2f(screenCenter.x - squareOffset.x, screenCenter.y - squareOffset.y);
      glVertex2f(screenCenter.x + squareOffset.x, screenCenter.y - squareOffset.y);
      glVertex2f(screenCenter.x + squareOffset.x, screenCenter.y + squareOffset.y);
      glVertex2f(screenCenter.x - squareOffset.x, screenCenter.y + squareOffset.y);
      glEnd();
   }


   //render gutters to hide what's outside the mission area
   if (missionArea.extent.x < missionArea.extent.y)
   {
      S32 rectWidth = (1 - ((F32)missionArea.extent.x / missionArea.extent.y)) / 2 * mBounds.extent.x;
      RectI rect(offset.x, offset.y, rectWidth, mBounds.extent.y);
      dglDrawRectFill(rect, ColorI(0,0,0));
      rect.point.x = offset.x + mBounds.extent.x - rectWidth;
      dglDrawRectFill(rect, ColorI(0,0,0));
   }
   else if (missionArea.extent.y < missionArea.extent.x)
   {
      S32 rectHeight = (1 - ((F32)missionArea.extent.y / missionArea.extent.x)) / 2 * mBounds.extent.y;
      RectI rect(offset.x, offset.y, mBounds.extent.x, rectHeight);
      dglDrawRectFill(rect, ColorI(0,0,0));
      rect.point.y = offset.y + mBounds.extent.y - rectHeight;
      dglDrawRectFill(rect, ColorI(0,0,0));
   }


   //render pings
   glEnable(GL_TEXTURE_2D);
   glBindTexture(GL_TEXTURE_2D, mPingTexture.getGLName());
   glEnable(GL_BLEND);
   glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
   glBegin(GL_QUADS);
   PingLocationEvent* event = mPingHead;
   while (event)
   {
      glColor3f(event->color.red, event->color.green, event->color.blue);
      Point2F center = Point2F(offset.x + event->screenPos.x, offset.y + event->screenPos.y);
      F32 magnitude = mFabs(pingMagnitude * mSin(event->t / (F32)pingCycleMS * M_PI));
      magnitude += pingRadius;
      glTexCoord2f(0.0, 1.0);
      glVertex2f(center.x - magnitude, center.y - magnitude);
      glTexCoord2f(1.0, 1.0);
      glVertex2f(center.x + magnitude, center.y - magnitude);
      glTexCoord2f(1.0, 0.0);
      glVertex2f(center.x + magnitude, center.y + magnitude);
      glTexCoord2f(0.0, 0.0);
      glVertex2f(center.x - magnitude, center.y + magnitude);
      event = event->next;
   }
   glEnd();
   glDisable(GL_BLEND);
   glDisable(GL_TEXTURE_2D);
}
