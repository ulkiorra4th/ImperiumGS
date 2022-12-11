using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameSaves
{
    public static class GameSavesKeeper
    {
        [Serializable]
        public static class CharacterSettings
        {
            public static float health;
            public static float damage;
            public static float speed;
            public static float mana;
        }

        [Serializable]
        public static class MapSettings
        {

        }

        [Serializable]
        public static class EnemiesSettings
        {
            public static float health;
            public static float damage;
            public static float speed;
            public static float mana;
        }


    }
}