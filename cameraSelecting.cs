using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class cameraSelecting : MonoBehaviour
{
   public bool isDisplaying;
   GameObject selectedTile;
   public bool gameStarted;
   public int tileValue;
   public Texture tileImage;
   GameObject buttonUsed;
   public GameObject prefab;
   public int placedPieces;

    // Start is called before the first frame update
    void Start()
    {
      placedPieces = 0;
      buttonUsed = null;
      tileImage = null;
      tileValue = 0;
      //isDisplaying = false;
      selectedTile = null;
      gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (gameStarted && isDisplaying && Input.GetMouseButtonDown(0))
         select();
      if (isDisplaying && Input.GetMouseButtonDown(1) && selectedTile != null && gameStarted)
         moveTo();

      // for placing your pieces BEFORE the game has started. 
      if (!gameStarted && isDisplaying && Input.GetMouseButtonDown(1) && (tileImage != null) && (buttonUsed != null)) 
         select(); //!gameSelected looks like its pointless because of the other select call, but they are two different keys
    }


   /**********************
    * * Select with left click
    *********************/
   public void select()
	{

      // Cast the ray
      RaycastHit hit;
      Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

      //Did it hit the tile we want AND the game is started?
      if (Physics.Raycast(ray, out hit) && gameStarted)
      {
         if (null != selectedTile && (hit.collider.name != selectedTile.name)) // if we click again and its not the same object
         {
            selectedTile.GetComponent<tileLogic>().changeSelected();
            hit.collider.GetComponent<tileLogic>().changeSelected();
            selectedTile = hit.collider.gameObject;
            print(hit.collider.name);
         }
         else   // this is the VERY first click
         {
            hit.collider.GetComponent<tileLogic>().changeSelected();
            selectedTile = hit.collider.gameObject;
            print(hit.collider.name);
         }
      } 
      else if(Physics.Raycast(ray, out hit) && (hit.collider.tag == transform.tag) && !gameStarted && !hit.collider.GetComponent<tileLogic>().occupied)  // this is for the start of the game, places the pieces of tiles
      {
         spawnTile(hit.collider.gameObject);
         placedPieces++;
         if (placedPieces == 40 && this.transform.tag != "p2")
            this.transform.parent.GetComponent<gameLogic>().switchCamera();
         else if(placedPieces == 40)
			{
            this.transform.parent.GetComponent<gameLogic>().switchCamera();
            this.transform.parent.GetComponent<gameLogic>().startGame();
         }         
            
		}
   }


   /**********************
   * Right Click selects a tile and sends
   *    that game object to the piece to movexa
   **********************/
   void moveTo()
	{
      //Cast the ray
      RaycastHit hit;
      Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

      //Check if it hit
      if (Physics.Raycast(ray, out hit) && selectedTile.GetComponent<tileLogic>().abovePiece != null)
      {

         /********** PIECE DISTANCE RESTRICTION*************************/
         float xNumber = selectedTile.GetComponent<tileLogic>().xPosition; // x constraint
         float zNumber = selectedTile.GetComponent<tileLogic>().zPosition; // z constraint

         // |_______________is it one number up?______________________________|  or |________________is it one number down?_____________________________|
         if ((((hit.collider.GetComponent<tileLogic>().xPosition == (xNumber - 1)) || (hit.collider.GetComponent<tileLogic>().xPosition == (xNumber + 1)))
            // AND not left or right
            && hit.collider.GetComponent<tileLogic>().zPosition == zNumber) || // OR is it the opposite?
            ((hit.collider.GetComponent<tileLogic>().zPosition == (zNumber - 1)) || (hit.collider.GetComponent<tileLogic>().zPosition == (zNumber + 1)))
            && hit.collider.GetComponent<tileLogic>().xPosition == xNumber) 
         { 
            // is the destination NOT occupied AND we own the moving piece        
            if ((!hit.collider.GetComponent<tileLogic>().occupied) && (selectedTile.GetComponent<Collider>().GetComponent<tileLogic>().abovePiece.tag == transform.tag))
            {
               selectedTile.GetComponent<tileLogic>().moveAbove(hit.collider.gameObject, selectedTile.GetComponent<Collider>().gameObject);
               selectedTile = null;
            }
            // |    did we hit a tile that is occupied be a piece           | && |           is the selected tile's tag == to the camera tag (do I own it?)            |  &&
            else if ((hit.collider.GetComponent<tileLogic>().abovePiece != null) && (selectedTile.GetComponent<Collider>().GetComponent<tileLogic>().abovePiece.transform.tag == transform.tag)
            && (hit.collider.GetComponent<tileLogic>().abovePiece.transform.tag != this.transform.tag))

            // |            is the tile im trying to move to's piece not my own                                      |
            // if all that is true, then its a BATTLE
            {
               selectedTile.GetComponent<tileLogic>().abovePiece.GetComponent<pieceLogic>().fight(hit.collider.gameObject, selectedTile.transform.gameObject);
               selectedTile = null;
            }
            else
               print("Invalid move");
         }
         else
            print(hit.collider.GetComponent<tileLogic>().xPosition + "," + hit.collider.GetComponent<tileLogic>().zPosition + " is not a valid move");
      }
      else
         print("You Must select a piece");
   }



   /*************************************
   * assign the going to be created tile the value and image
   *******************************************/
   public void assignSpawnTileValues(Texture incomingImage, int assignValue, GameObject buttonBeingUsed)
   {
      // assign it stuff via the parameters
      tileImage = incomingImage;
      tileValue = assignValue;
      buttonUsed = buttonBeingUsed;
   }

   public void spawnTile(GameObject destinationTile)
   {
      if(tileValue != 0)
		{

         // create the piece
         GameObject madePiece = Instantiate(prefab, destinationTile.GetComponent<tileLogic>().abovePosition, Quaternion.identity);
         //give it its values
         madePiece.GetComponent<pieceLogic>().assignTile(tileImage, tileValue, transform.tag);
         // assign the piece the tile
         destinationTile.GetComponent<tileLogic>().assignAbovePiece(madePiece);

         // controls which camera can see the object through layers
         if (this.tag == "p1")
            madePiece.layer = 7;
         else
            madePiece.layer = 8;

         // set everything to null so you cant spawn it again
         tileValue = 0;
         tileImage = null;
         buttonUsed.SetActive(false);
         buttonUsed = null;
         
		}
   }



   // the end of class
}
