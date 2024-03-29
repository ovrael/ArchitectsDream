using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts.Stargate
{
    public class RuneData : ScriptableObject
    {
        [SerializeField, PreviewSprite]
        private Sprite sprite;
        public Sprite Sprite { get { return sprite; } }
    }
}
