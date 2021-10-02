using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnButton : MonoBehaviour

{
   public int tileValue;
   public Texture tileImage;


   public void assignCameraValues()
   {

      // No easy way to reference the name via tag or anything like that (that I know on), this will have to do
      if (transform.tag == "p1")
         this.transform.parent.parent.Find("p1Camera").GetComponent<cameraSelecting>().assignSpawnTileValues(tileImage, tileValue, this.transform.gameObject);
      else
         this.transform.parent.parent.Find("p2Camera").GetComponent<cameraSelecting>().assignSpawnTileValues(tileImage, tileValue, this.transform.gameObject);
   }

//END
}

    
