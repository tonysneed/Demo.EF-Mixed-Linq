# Demo: EF Mixed Linq

Demo of mixing Linq to Objects with Linq to Entities

*This project demonstrates that mixing LINQ to Entities with LINQ to Objects is not supported.  
The LINQ to Entities query should instead use something like the Contains operator to filter values,  
so that the filtering is executed remotely.*

## Setup
1. Create a NorthwindSlim database in local SQL Server instance.
2. Download: http://bit.ly/northwindslim
3. Run NorthwindSlim.sql script using SQL Server Management Studio.
