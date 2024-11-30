using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadIntro : MonoBehaviour
{
   public void LoadScene()
   {
        SceneManager.LoadScene("Introduction");
   }
}
