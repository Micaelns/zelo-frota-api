namespace Domain.Enum;

[Flags]
public enum LicenseCategory
{
    None = 0,
    A = 1 << 0,  // 00001 = 1
    B = 1 << 1,  // 00010 = 2
    C = 1 << 2,  // 00100 = 4
    D = 1 << 3,  // 01000 = 8
    E = 1 << 4,  // 10000 = 16

    // Combinações comuns gravadas no sistema
    AB = A | B,  // 00011 = 3
    AC = A | C,  // 00101 = 5
    AD = A | D,  // 01001 = 9
    AE = A | E   // 10001 = 17
}
