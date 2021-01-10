[![Build Status](https://dev.azure.com/useinm/Usemam.IdentityServer4.KeyRack/_apis/build/status/usemam.Usemam.IdentityServer4.KeyRack?branchName=master)](https://dev.azure.com/useinm/Usemam.IdentityServer4.KeyRack/_build/latest?definitionId=4&branchName=master)
## About Usemam.IdentityServer4.KeyRack
KeyRack is a seamless token signing key management extension for [IdentityServer4](https://github.com/IdentityServer/IdentityServer4). Its mission is to allow for keys to be created and rotated fully automatically - without any manual intervention.

### Packages
- [Usemam.IdentityServer4.KeyRack](https://www.nuget.org/packages/Usemam.IdentityServer4.KeyRack/) - core package
- [Usemam.IdentityServer4.KeyRack.DataProtection](https://www.nuget.org/packages/Usemam.IdentityServer4.KeyRack.DataProtection/) - adds data protection for keys using [Microsoft.AspNetCore.DataProtection.Abstractions](https://www.nuget.org/packages/Microsoft.AspNetCore.DataProtection.Abstractions/)
- [Usemam.IdentityServer4.KeyRack.EntityFramework](https://www.nuget.org/packages/Usemam.IdentityServer4.KeyRack.EntityFramework/) - adds database persistence for keys
