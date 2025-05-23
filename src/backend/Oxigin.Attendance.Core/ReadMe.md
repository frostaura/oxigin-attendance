# Oxigin.Attendance.Core
The purpose for this project is to hold all core code which should be abstracted from the various platforms that the core code can be used across. For example Blazor vs API vs Native etc.

## Architectural Principals
We adhere to the iDesign architectural principals for this project and we have a subdirectory for each of the main layers in the N-tier architecture.

**Layers**
- Managers (Orchestrators. Example: Use cases should have methods in managers to represent them.)
- Engines (For complex, isolated and functional operations. Example: Calculating payouts given transactions and discount rules.)
- Data (for IO communication in and outbound. Example: Querying the blockchain, filesystem or datastore(s).)