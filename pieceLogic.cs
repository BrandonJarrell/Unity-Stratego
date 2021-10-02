using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceLogic : MonoBehaviour
{
   public int value;
   public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
      if(this.transform.tag == "p1")
         prefab = GameObject.Find("blueTeam");
      else
         prefab = GameObject.Find("redTeam");

      GameObject fakeTile = Instantiate(prefab, this.transform.position, Quaternion.identity);
      fakeTile.transform.SetParent(this.transform); // set this as a child
    }


   /**********************************************************
    *  determines if the object can move, the destination is open, or if a fight should happen
    * *******************************************************/
   public void movePiece(GameObject destinationTile, GameObject leavingTile)
   {
      GameObject table = GameObject.Find("Table");
      // is the tile occupied and is it not a flag or bomb
      if (!destinationTile.GetComponent<tileLogic>().occupied && (value != 1) && (value != 12))
      {
         this.transform.position = destinationTile.GetComponent<tileLogic>().abovePosition;
         leavingTile.GetComponent<tileLogic>().assignAbovePieceNull();
         destinationTile.GetComponent<tileLogic>().assignAbovePiece(this.gameObject);
         this.transform.GetChild(0).transform.position = this.transform.position;
         table.GetComponent<gameLogic>().switchCamera(); // switch turns
      }
      // is the occupied tile occupied by a piece (or is it no go zone)
      else if ((destinationTile.GetComponent<tileLogic>().abovePiece != null) && (value != 1) && (value != 12))
      {
         print("it should be working");
         fight(destinationTile, leavingTile); // FIGHT
         // inside fight is the switchCamera() called at the end.   this is because
      }
      else
         print("Invalid Move"); // Place holder for movement restrictions indication
   }


   /***************************
   * give the tile its properties
   ****************************/
   public void assignTile(Texture tileImage, int tileValue, string tag)
	{
      value = tileValue;
      GetComponent<Renderer>().material.mainTexture = tileImage;
      transform.tag = tag;
      if (tag == "p1") // turn if its player 1's piece
         transform.Rotate(0f, 180f, 0.0f, Space.Self);

   }


   /******************
    *  FIGHT (determines the winner of the fight, or game if flag is found)
    * ***************/
   public void fight(GameObject destinationTile, GameObject attackingTile)
	{
      bool isBomb = false;
      switch(destinationTile.GetComponent<tileLogic>().abovePiece.GetComponent<pieceLogic>().value)
		{

         //ATTACKING A FLAG
         case 1:
            destinationTile.GetComponent<tileLogic>().abovePiece.SetActive(false);
            destinationTile.GetComponent<tileLogic>().assignAbovePiece(this.transform.gameObject);   // set the tile to own this piece
            this.transform.position = destinationTile.GetComponent<tileLogic>().abovePosition; // move the piece
            attackingTile.GetComponent<tileLogic>().assignAbovePieceNull(); // clear the attacking tile
             
            GameObject table = GameObject.Find("Table"); // reference the table
            if (destinationTile.GetComponent<tileLogic>().abovePiece.transform.tag == "p1") //if player 1
               table.GetComponent<gameLogic>().winner("Player 1"); // make him win
            else
               table.GetComponent<gameLogic>().winner("Player 2"); // make player 2 win
            break;

         //ATTACKING A SPY
         case 2:
            if (value == 11) // if we are a general, die to the spy
            {
               attackingTile.GetComponent<tileLogic>().abovePiece.SetActive(false); // die
               attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();
            }
            else // if its anything else, compare normally
				{
               if(value > 2) // if this is greater than the spy, kill the spy
					{
                  destinationTile.GetComponent<tileLogic>().abovePiece.SetActive(false);
                  destinationTile.GetComponent<tileLogic>().assignAbovePiece(this.transform.gameObject);   // set the tile to own this piece
                  this.transform.position = destinationTile.GetComponent<tileLogic>().abovePosition; // move the piece
                  attackingTile.GetComponent<tileLogic>().assignAbovePieceNull(); // clear the attacking tile
               }
               else // kill both spies
					{
                  attackingTile.GetComponent<tileLogic>().abovePiece.SetActive(false); //die
                  attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();
                  destinationTile.GetComponent<tileLogic>().abovePiece.SetActive(false); //die
                  destinationTile.GetComponent<tileLogic>().assignAbovePieceNull();
               }
            }
            break;

         //ATTACKING A GENERAL
         case 11:
            if(value == 2)  // if we are the spy, kill the general
				{
               destinationTile.GetComponent<tileLogic>().abovePiece.SetActive(false);
               destinationTile.GetComponent<tileLogic>().assignAbovePiece(this.transform.gameObject);   // set the tile to own this piece
               this.transform.position = destinationTile.GetComponent<tileLogic>().abovePosition; // move the piece
               attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();              //clear attacking tile
            }
            else
				{
               if (value == 11) // if its both generals, kill both
               {
                  attackingTile.GetComponent<tileLogic>().abovePiece.SetActive(false); // die
                  attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();      // clear the attacking tile
                  destinationTile.GetComponent<tileLogic>().abovePiece.SetActive(false); //die
                  destinationTile.GetComponent<tileLogic>().assignAbovePieceNull(); 
               }
               else // general will beat everything else
					{
                  attackingTile.GetComponent<tileLogic>().abovePiece.SetActive(false);
                  attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();   // clear the attacking tile
               }
            }
            break;

         //ATTACKING A BOMB
         case 12:
            if (value == 4) // if we are the bomb squad
            {
               destinationTile.GetComponent<tileLogic>().abovePiece.SetActive(false);              // delete the bomb
               destinationTile.GetComponent<tileLogic>().assignAbovePiece(this.transform.gameObject);   // set the tile to own this piece
               this.transform.position = destinationTile.GetComponent<tileLogic>().abovePosition; // move the piece
               attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();    //clear attacking tile
            }
            else // if not, we die
            {
               attackingTile.GetComponent<tileLogic>().abovePiece.SetActive(false);
               attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();
               isBomb = true;
            }
            break;
         //ATTACKING A NORMAL  
         default:
            if(value > destinationTile.GetComponent<tileLogic>().abovePiece.GetComponent<pieceLogic>().value ) // if we are higher value
				{
               destinationTile.GetComponent<tileLogic>().abovePiece.SetActive(false);
               destinationTile.GetComponent<tileLogic>().assignAbovePiece(this.transform.gameObject);   // set the tile to own this piece
               this.transform.position = destinationTile.GetComponent<tileLogic>().abovePosition; // move the piece
               attackingTile.GetComponent<tileLogic>().assignAbovePieceNull(); //clear attacking tile
            }
            else if(value == destinationTile.GetComponent<tileLogic>().abovePiece.GetComponent<pieceLogic>().value)  // if we are the same value
				{
               attackingTile.GetComponent<tileLogic>().abovePiece.SetActive(false); // die
               attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();      // clear the attacking tile
               destinationTile.GetComponent<tileLogic>().abovePiece.SetActive(false); //die
               destinationTile.GetComponent<tileLogic>().assignAbovePieceNull();
            }
            else // last option is that we are less that that piece and lose
				{
               attackingTile.GetComponent<tileLogic>().abovePiece.SetActive(false); // die
               attackingTile.GetComponent<tileLogic>().assignAbovePieceNull();      // clear the attacking tile
            }
            break;    
      }
      GameObject game = GameObject.Find("Table");
      game.GetComponent<gameLogic>().switchCamera();
      game.GetComponent<gameLogic>().fightSound(isBomb);
   }

}
