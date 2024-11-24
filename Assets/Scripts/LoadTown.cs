using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTown : MonoBehaviour
{
   public void LoadScene()
   {
        SceneManager.LoadScene("Town Map");
   }
}
