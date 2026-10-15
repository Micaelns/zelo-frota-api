namespace Domain.Enum;

[Flags]
public enum LicenseCategory
{
    None = 0,
    A = 1 << 0,  // 00001
    B = 1 << 1,  // 00010
    C = 1 << 2,  // 00100
    D = 1 << 3,  // 01000
    E = 1 << 4,  // 10000

    // Combinações comuns gravadas no sistema
    AB = A | B,  // 00011
    AC = A | C,  // 00101
    AD = A | D,  // 01001
    AE = A | E   // 10001
}
