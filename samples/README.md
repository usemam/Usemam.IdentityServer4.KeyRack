## How to use these samples

There are 3 apps in this folder:
- **Client** - app that connects for sample IdentityServer app.
- **FileSystem** - IdentityServer app with default KeyRack options enabled (keys persisted to files, no key data protection).
- **Database** - IdentityServer app with database key persistence and key data protection enabled.

To run sample:
1. Build all 3 apps.
2. Run either FileSystem or Database sample app - default https://localhost:5001 URL will be exposed.
3. Run Client app to access exposed URL and run test authentication steps.