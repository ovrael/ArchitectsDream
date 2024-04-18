using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssemblyCSharp.Assets.EditorUtils
{
    public class TypeConstraintAttribute : PropertyAttribute
    {
        private Type type;

        public TypeConstraintAttribute(Type type)
        {
            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }
    }
}
