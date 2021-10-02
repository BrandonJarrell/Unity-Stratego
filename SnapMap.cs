using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SnapMap : MonoBehaviour
{
   float tileX;
   float tileZ;
   Vector3 firstTile, location;
   public GameObject prefab;
   GameObject madeCube;
   int nameCounter;

    // Start is called before the first frame update
    void Start()
    {
      nameCounter = 0;
      tileX = (transform.localScale.x / 10);
      tileZ = (transform.localScale.z / 10);
      firstTile = new Vector3((this.transform.position.x - ((this.transform.localScale.x / 2) - (tileX / 2))), (this.transform.position.y + 1f), (this.transform.position.z - ((this.transform.localScale.z / 2) - (tileZ / 2))));

      //Instantiate(prefab, firstTile, Quaternion.identity);

      for (int rowZ = 0; rowZ < 10; rowZ++)
       {
         for (int colX = 0; colX < 10; colX++)
         {
            location = new Vector3((firstTile.x + (tileX * colX)), (this.transform.position.y +0.2f), (firstTile.z + (tileZ * rowZ)));
            madeCube = Instantiate(prefab, location, Quaternion.identity);
            madeCube.GetComponent<tileLogic>().xPosition = colX + 1;
            madeCube.GetComponent<tileLogic>().zPosition = rowZ + 1;
            // Assigning cube ownership through tags
            if (rowZ < 4)
               madeCube.tag = "p1";
            else if (rowZ > 3 && rowZ < 6)
               madeCube.tag = "neutral";
            else if (rowZ > 5)
               madeCube.tag = "p2";

            // Naming the newly created cube object
            string name = ((colX +1) + "," + (rowZ + 1));
            madeCube.name = name;
         }  
       }

   }
}
