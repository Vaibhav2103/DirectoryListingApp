# DirectoryListingApp
Add specific Directory structure in db with win app and retrieve it and display in web app


Create Database in MS Sql 
And After Create Table for save directory name.

Table Script

CREATE TABLE [DBO].[directory]
(
  id  int not null identity(1,1) primary key clustered,
  parent_id int null  foreign key references dbo.directory(id),
  name  varchar(256)  collate SQL_Latin1_General_CP1_CI_AS  not null, -- case-insensensitive, accent-sensitive
  unique(id, name)
)
go
