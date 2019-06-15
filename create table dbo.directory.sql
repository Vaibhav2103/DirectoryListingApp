create table dbo.directory
(
  id        int not null identity(1,1) primary key clustered ,
  parent_id int     null               foreign key references dbo.directory( id ) ,
  name      varchar(256) collate SQL_Latin1_General_CP1_CI_AS not null , -- case-insensensitive, accent-sensitive

  unique ( id , name ) ,

)
go

truncate table directory
Select * From directory





--Create table Dbo.Directory1
--(
--Id int Not null Identity(1,1) Primary Key Clustered,
--Parent_id Int Null Foreign key references Dbo.Directory1(id),
--Name Varchar(500) ,
--Unique(id,name)
--)