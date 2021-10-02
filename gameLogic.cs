using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameLogic : MonoBehaviour
{
   bool gameMasterStarted;

   public AudioSource source;
   public AudioClip clip1;
   public AudioClip clip2;
   public AudioClip clip3;
   public AudioClip bombClip;

   // Start is called before the first frame update
   void Start()
   {
      //source = this.transform.gameObject;
      gameMasterStarted = false;
      this.transform.Find("p1Camera").GetComponent<Camera>().enabled = true;
      this.transform.Find("p2Camera").GetComponent<Camera>().enabled = false;
      this.transform.Find("p1Camera").GetComponent<AudioListener>().enabled = true;

      this.transform.Find("p1Canvas").gameObject.SetActive(true);
      this.transform.Find("p2Canvas").gameObject.SetActive(false);
      this.transform.Find("p2Camera").GetComponent<AudioListener>().enabled = false;

   }



   void Update()
	{

      // For temp quick play and demonstrating
      if (Input.GetKeyDown("space"))
         switchTurn();

      // For temp quick play and demonstrating
      if (Input.GetKeyDown("m"))
         startGame();
   }




   public void switchTurn()  // incase further logic is needed
	{
      switchCamera();
	}


   /*********************************
 * SWITCHED CAMERA VEIWS
 *********************************/ 
   public void switchCamera()
   {
      if (this.transform.Find("p1Camera").GetComponent<Camera>().enabled) // if p1 is displaying
      {
         this.transform.Find("p1Camera").GetComponent<Camera>().enabled = false; // disable p1
         this.transform.Find("p1Camera").GetComponent<cameraSelecting>().isDisplaying = false; // tell camera it's not displaying
         this.transform.Find("p1Canvas").gameObject.SetActive(false);
         this.transform.Find("p1Camera").GetComponent<AudioListener>().enabled = false;

         this.transform.Find("p2Camera").GetComponent<Camera>().enabled = true; // enable p2
         this.transform.Find("p2Camera").GetComponent<cameraSelecting>().isDisplaying = true; // and this one is displaying
         this.transform.Find("p2Camera").GetComponent<AudioListener>().enabled = true;
         if (!gameMasterStarted)
            this.transform.Find("p2Canvas").gameObject.SetActive(true);

      }
      else                                                     // If p1 is NOT displaying
      {
         this.transform.Find("p2Camera").GetComponent<Camera>().enabled = false; // disable p2
         this.transform.Find("p2Camera").GetComponent<cameraSelecting>().isDisplaying = false;
         this.transform.Find("p2Canvas").gameObject.SetActive(false);
         this.transform.Find("p2Camera").GetComponent<AudioListener>().enabled = false;


         this.transform.Find("p1Camera").GetComponent<Camera>().enabled = true; // enable p1
         this.transform.Find("p1Camera").GetComponent<cameraSelecting>().isDisplaying = true;
         this.transform.Find("p1Camera").GetComponent<AudioListener>().enabled = true;
         if (!gameMasterStarted)
            this.transform.Find("p1Canvas").gameObject.SetActive(true);
      }
   }


   /*******************
    *  Starts the game
    *******************/
   public void startGame()
   {

      this.transform.Find("p1Canvas").gameObject.SetActive(false);
      this.transform.Find("p2Canvas").gameObject.SetActive(false);
      if (!gameMasterStarted)
      {
         gameMasterStarted = true;
         this.transform.Find("p1Camera").GetComponent<cameraSelecting>().gameStarted = true;
         this.transform.Find("p2Camera").GetComponent<cameraSelecting>().gameStarted = true;
      }
      else
      {
         gameMasterStarted = false;
         this.transform.Find("p1Camera").GetComponent<cameraSelecting>().gameStarted = false;
         this.transform.Find("p2Camera").GetComponent<cameraSelecting>().gameStarted = false;
      }
   }

   public void fightSound(bool bombSound)
   {
      if (bombSound)
         source.PlayOneShot(bombClip);
      else
      {
         int soundChoice = Random.Range(1, 4);
         print(soundChoice);
         switch (soundChoice)
         {
            case 1:
               source.PlayOneShot(clip1);
               break;
            case 2:
               source.PlayOneShot(clip2);
               break;
            case 3:
               source.PlayOneShot(clip3);
               break;
         }
      }
	}

   public void winner(string player)
	{
      print(player + " Has Won The Game!!!");
	}

   // end of class
}
