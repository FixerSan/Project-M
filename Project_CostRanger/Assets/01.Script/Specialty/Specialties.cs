using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Specialties
{
    public class Test : Specialty
    {
        public Test()
        {
            specialLevel = 0;
            specialtyCount = 0;
        }

        public override void CheckCount()
        {
            if (specialtyCount >= 1)
                specialLevel = 1;

            if(specialtyCount >= 2)
                specialLevel = 2;
        }

        public override void Effect()
        {
            if(specialLevel == 1)
            {

            }

            if(specialLevel == 2)
            {

            }
        }
    }
}
