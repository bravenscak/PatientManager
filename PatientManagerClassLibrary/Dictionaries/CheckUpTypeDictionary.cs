using PatientManagerClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Dictionaries
{
    public static class CheckUpTypeDictionary
    {
        public static Dictionary<CheckUpType, string> Descriptions = new Dictionary<CheckUpType, string>
        {
            { CheckUpType.GP, "Opći tjelesni pregled" },
            { CheckUpType.KRV, "Test krvi" },
            { CheckUpType.X_RAY, "Rendgensko skeniranje" },
            { CheckUpType.CT, "CT sken" },
            { CheckUpType.MR, "MRI sken" },
            { CheckUpType.ULTRA, "Ultrazvuk" },
            { CheckUpType.EKG, "Elektrokardiogram" },
            { CheckUpType.ECHO, "Ehokardiogram" },
            { CheckUpType.EYE, "Pregled očiju" },
            { CheckUpType.DERM, "Dermatološki pregled" },
            { CheckUpType.DENTA, "Pregled zuba" },
            { CheckUpType.MAMMO, "Mamografija" },
            { CheckUpType.NEURO, "Neurološki pregled" }
        };
    }
}
