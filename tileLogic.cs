using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileLogic : MonoBehaviour
{
   public bool occupied;
   public bool selected;
   public float xPosition;
   public float zPosition;
   public GameObject abovePiece;
   public Vector3 abovePosition; // The position for pieces to move to (easy reference)

    // Start is called before the first frame update
    void Start()
    {
      string name = gameObject.name;
      print(name);
      switch (name) // couldn't think of a more elgant & effecient way to do this
		{
         case "3,5":
            occupied = true;
            break;

         case "3,6":
            occupied = true;
            break;

         case "4,5":
            occupied = true;
            break;

         case "4,6":
            occupied = true;
            break;

         case "7,5":
            occupied = true;
            break;

         case "7,6":
            occupied = true;
            break;

         case "8,5":
            occupied = true;
            break;

         case "8,6":
            occupied = true;
            break;

         default:
            occupied = false;
            break;
      }

      abovePosition = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);
    }





   /**************************
    *   SWITCH SELECTING
    **************************/
   public void changeSelected()
	{
      if (selected)
         selected = false;
      else
         selected = true;
}



   /**************************
    *   ABOVE PIECE ASSIGNING 
    **************************/
   public void assignAbovePiece(GameObject piece)
	{
      abovePiece = piece;
      occupied = true;
	}

   public void assignAbovePieceNull()
	{
      abovePiece = null;
      occupied = false;
	}

   /**************************
    *   Allows access to the above piece's functions
    *   (function is for further error checking)
    **************************/
   public void moveAbove(GameObject tileDestination, GameObject leavingTile)
	{
      if (abovePiece != null) // incase the tile is empty, you cant tell it to move the above piece
         abovePiece.GetComponent<pieceLogic>().movePiece(tileDestination, leavingTile);
	}

}
