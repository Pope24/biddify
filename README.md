1. Project structure.

Biddify
│

├── Biddify.Biddify           => Controller, Pages, Program.cs

├── Biddify.Service           => Interface + Service class to handle business operations

├── Biddify.Repository        => Interface + Repository class access data

├── Biddify.DataAccess        => DbContext, Entity, Migration

├── Biddify.Domain            => model (resquest, response), enum, constant

└── Biddify.Common            => class helper, util, mapping, exception

2. Database Diagram.

![supabase-schema-ldnwicqmrfclrbcpkrmo (2)](https://github.com/user-attachments/assets/150c8434-5e3d-43ed-a663-4d4261bfe273)
