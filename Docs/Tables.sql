use TombProspectors;
go

CREATE TABLE [dbo].[UserHistory] (
	[Id] int identity primary key not null,
	[UserName] varchar(50) not null,
	[Target] varchar(50) not null,
	[Action] varchar(50) not null,
	[Value] varchar(50) not null,
	[Created] datetime2 not null
) ON [PRIMARY];

create table [dbo].[Users] (
	[Id] int identity not null,
	[UserName] varchar(50) primary key not null,
	[Password] varchar(50) not null,
	[Email] nvarchar(50) not null,
	[Role] varchar(10) not null,
	[Created] datetime2 not null,
	[LastLogin] datetime2 null
) on [PRIMARY];

create table [dbo].[RootChalices] (
	ChaliceId varchar(50) primary key not null,
	ChaliceName varchar(50) not null
) on [PRIMARY];

create table [dbo].[Loot] (
	Id int identity primary key not null,
	ItemName nvarchar(100) not null,
	Note nvarchar(250),
	WikiLink nvarchar(250),
	Updated datetime2 not null
) on [PRIMARY];

create table [dbo].[DungeonGlyphs] (
	Glyph varchar(20) primary key not null,
	ShortDescription varchar(250) not null,
	Layers int not null,
	RootChalice varchar(50) not null,
	Fetid bit not null default(0),
	Rotted bit not null default(0),
	Cursed bit not null default(0),
	Bosses varchar(250) not null,
	Loot varchar(250) not null,
	Notes nvarchar(MAX),
	Submitter varchar(50) not null,
	Upvotes int not null default(1),
	Downvotes int not null default(0),
	Updated datetime2 not null
) on [PRIMARY];

create table [dbo].[Bosses] (
	Id int identity primary key,
	BossName varchar(150) not null,
	WikiLink nvarchar(250),
	Updated datetime2 not null
) on [PRIMARY];

create table Comments (
	Id int primary key identity not null,
	CommentText varchar(500) not null,
	Glyph varchar(20) not null,
	PostedBy varchar(50) not null,
	Posted datetime2(0) not null
) on [PRIMARY];