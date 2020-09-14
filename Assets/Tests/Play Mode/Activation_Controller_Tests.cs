using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Activation_Controller_Tests
    {

        // TESTS TO WRITE
        /* If character dies during turn, next entity does activate
         * If character dies during CharacterOnActivationEnd phase (e.g. from poisoned damage), next entity does activate
         * If character dies during turn and is last defender, game over event does trigger
         * If character dies during turn and is last enemy, victory event does trigger 
         * 
         */
    }
}
