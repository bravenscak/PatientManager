using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Enums
{
    public enum CheckUpType
    {
        GP, // Opći tjelesni pregled
        KRV, // Test krvi
        X_RAY, // rendgensko skeniranje
        CT, // CT sken
        MR, // MRI sken
        ULTRA, // Ultrazvuk
        EKG, // Elektrokardiogram
        ECHO, // Ehokardiogram
        EYE, // pregled očiju
        DERM, // Dermatološki pregled
        DENTA, // pregled zuba
        MAMMO, // Mamografija
        NEURO // Neurološki pregled
    }
}
